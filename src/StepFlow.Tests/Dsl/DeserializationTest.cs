using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Dsl;
using StepFlow.Dsl.Model;

namespace StepFlow.Tests.Dsl;

[TestClass]
public class DeserializationTest
{
    [DataTestMethod]
    [DataRow("Dsl/JsonAssets/workflow-without-steps.json", DeserializerType.Json)]
    [DataRow("Dsl/YamlAssets/workflow-without-steps.yaml", DeserializerType.Yaml)]
    public void DeserializeWorkflowDefinitionWithoutSteps(string filePath, DeserializerType deserializerType)
    {
        string file = File.ReadAllText(filePath);
        WorkflowDefinitionModel? model = Deserialize(file, deserializerType);

        Assert.IsNotNull(model);
        Assert.AreEqual("DataType", model.Data);
        Assert.IsTrue(model.Steps is null || model.Steps.Count == 0);
    }

    [DataTestMethod]
    [DataRow("Dsl/JsonAssets/workflow-with-steps.json", DeserializerType.Json)]
    [DataRow("Dsl/YamlAssets/workflow-with-steps.yaml", DeserializerType.Yaml)]
    public void DeserializeWorkflowDefinitionWithSteps(string filePath, DeserializerType deserializerType)
    {
        string file = File.ReadAllText(filePath);
        WorkflowDefinitionModel? model = Deserialize(file, deserializerType);

        Assert.IsNotNull(model);
        Assert.AreEqual("WorkflowData", model.Data);
        Assert.AreEqual(3, model.Steps.Count);

        AssertStepDefinition(model.Steps[0], "SimpleStep", null, null);

        AssertStepDefinition(model.Steps[1], "StepWithInput", new List<KeyValuePair<string, object>>
            {
                new("SomePropStr", "StringValue"),
                new("OtherPropInt", "42"),
                new("PropExpr", "data.Value")
            },
            null);

        AssertStepDefinition(model.Steps[2], "StepWithInputAndOutput", new List<KeyValuePair<string, object>>
            {
                new("StepProperty", "value")
            },
            new KeyValuePair<string, object>("Result", "step.Result"));
    }

    [DataTestMethod]
    [DataRow("Dsl/JsonAssets/workflow-with-nested-if.json", DeserializerType.Json)]
    [DataRow("Dsl/YamlAssets/workflow-with-nested-if.yaml", DeserializerType.Yaml)]
    public void DeserializeWorkflowDefinitionWithNestedIf(string filePath, DeserializerType deserializerType)
    {
        string file = File.ReadAllText(filePath);
        WorkflowDefinitionModel? model = Deserialize(file, deserializerType);

        Assert.IsNotNull(model);
        Assert.AreEqual("WorkflowData", model.Data);
        Assert.AreEqual(3, model.Steps.Count);

        AssertStepDefinition(model.Steps[0], "step1", null, null);
        AssertIfDefinition(model.Steps[1], "data.Result > 5", 3);
        AssertStepDefinition(model.Steps[2], "step3", null, null);
        AssertStepDefinition(model.Steps[1].Steps[0], "step11", null, null);
        AssertStepDefinition(model.Steps[1].Steps[1], "step12", null, null);
        AssertIfDefinition(model.Steps[1].Steps[2], "data.SomeFlag", 2);
        AssertStepDefinition(model.Steps[1].Steps[2].Steps[0], "step111", null, null);
        AssertStepDefinition(model.Steps[1].Steps[2].Steps[1], "step112", null, null);
    }

    [DataTestMethod]
    [DataRow("Dsl/JsonAssets/workflow-with-goto.json", DeserializerType.Json)]
    [DataRow("Dsl/YamlAssets/workflow-with-goto.yaml", DeserializerType.Yaml)]
    public void DeserializeWorkflowDefinitionWithGoTo(string filePath, DeserializerType deserializerType)
    {
        string file = File.ReadAllText(filePath);
        WorkflowDefinitionModel? model = Deserialize(file, deserializerType);

        Assert.IsNotNull(model);
        Assert.AreEqual("WorkflowData", model.Data);
        Assert.AreEqual(4, model.Steps.Count);

        AssertStepDefinition(model.Steps[0], "s1", null, null);
        Assert.AreEqual("a1", model.Steps[0].Id);
        AssertStepDefinition(model.Steps[1], "s2", null, null);
        AssertStepDefinition(model.Steps[2], "s3", null, null);
        AssertGoToDefinition(model.Steps[3], "a1");
    }

    private WorkflowDefinitionModel? Deserialize(string data, DeserializerType deserializerType)
    {
        return deserializerType switch
        {
            DeserializerType.Json => Deserializers.Json(data),
            DeserializerType.Yaml => Deserializers.Yaml(data),
            _ => throw new ArgumentOutOfRangeException(nameof(deserializerType), deserializerType, null)
        };
    }

    private void AssertStepDefinition(WorkflowNodeModel node, string expectedType,
        List<KeyValuePair<string, object>>? expectedInput, KeyValuePair<string, object>? expectedOutput)
    {
        Assert.AreEqual(expectedType, node.Type);

        if (expectedInput != null)
        {
            Assert.IsNotNull(node.Input);
            List<KeyValuePair<string, object>> inputList = node.Input
                .Select(x => new KeyValuePair<string, object>(x.Key, x.Value?.ToString()!))
                .ToList();
            CollectionAssert.AreEqual(expectedInput, inputList);
        }
        else
        {
            Assert.IsNull(node.Input);
        }

        if (expectedOutput != null)
        {
            Assert.IsNotNull(node.Output);
            KeyValuePair<string, object> output = node.Output
                .Select(x => new KeyValuePair<string, object>(x.Key, x.Value?.ToString()!))
                .Single();
            Assert.AreEqual(expectedOutput.Value.Key, output.Key);
            Assert.AreEqual(expectedOutput.Value.Value, output.Value);
        }
        else
        {
            Assert.IsNull(node.Output);
        }

        Assert.IsNull(node.Steps);
        Assert.IsNull(node.Condition);
        Assert.IsNull(node.NextId);
    }

    private void AssertIfDefinition(WorkflowNodeModel node, string expectedCondition, int expectedStepsCount)
    {
        Assert.AreEqual("+If", node.Type);
        Assert.AreEqual(expectedCondition, node.Condition);
        Assert.AreEqual(expectedStepsCount, node.Steps.Count);
        Assert.IsNull(node.Input);
        Assert.IsNull(node.Output);
        Assert.IsNull(node.NextId);
    }

    private void AssertGoToDefinition(WorkflowNodeModel node, string expectedNextId)
    {
        Assert.AreEqual("+GoTo", node.Type);
        Assert.AreEqual(expectedNextId, node.NextId);
        Assert.IsNull(node.Input);
        Assert.IsNull(node.Output);
        Assert.IsNull(node.Steps);
        Assert.IsNull(node.Condition);
    }
}