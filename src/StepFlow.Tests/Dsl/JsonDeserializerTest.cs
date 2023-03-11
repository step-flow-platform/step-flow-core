using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Dsl;
using StepFlow.Dsl.Model;

namespace StepFlow.Tests.Dsl;

[TestClass]
public class JsonDeserializerTest
{
    [TestMethod]
    public void DeserializeWorkflowDefinitionWithoutSteps()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/workflow-without-steps.json");

        WorkflowDefinitionModel? model = Deserializers.Json(json);

        Assert.IsNotNull(model);
        Assert.AreEqual("DataType", model.Data);
        Assert.AreEqual(0, model.Steps.Count);
    }

    [TestMethod]
    public void DeserializeWorkflowDefinitionWithSteps()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/workflow-with-steps.json");

        WorkflowDefinitionModel? model = Deserializers.Json(json);

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

    [TestMethod]
    public void DeserializeWorkflowDefinitionWithNestedIf()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/workflow-with-nested-if.json");

        WorkflowDefinitionModel? model = Deserializers.Json(json);

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

    [TestMethod]
    public void DeserializeWorkflowDefinitionWithGoTo()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/workflow-with-goto.json");

        WorkflowDefinitionModel? model = Deserializers.Json(json);

        Assert.IsNotNull(model);
        Assert.AreEqual("WorkflowData", model.Data);
        Assert.AreEqual(4, model.Steps.Count);

        AssertStepDefinition(model.Steps[0], "s1", null, null);
        Assert.AreEqual("a1", model.Steps[0].Id);
        AssertStepDefinition(model.Steps[1], "s2", null, null);
        AssertStepDefinition(model.Steps[2], "s3", null, null);
        AssertGoToDefinition(model.Steps[3], "a1");
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