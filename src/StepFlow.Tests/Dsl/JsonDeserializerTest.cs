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

        AssertStep(model.Steps[0], "SimpleStep", null, null);

        AssertStep(model.Steps[1], "StepWithInput", new List<KeyValuePair<string, object>>
            {
                new("SomePropStr", "StringValue"),
                new("OtherPropInt", "42"),
                new("PropExpr", "data.Value")
            },
            null);

        AssertStep(model.Steps[2], "StepWithInputAndOutput", new List<KeyValuePair<string, object>>
            {
                new("StepProperty", "value")
            },
            new KeyValuePair<string, object>("Result", "step.Result"));
    }

    [TestMethod]
    public void DeserializeWorkflowDefinitionWithNestedIfBranches()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/workflow-with-nested-if-branches.json");

        WorkflowDefinitionModel? model = Deserializers.Json(json);

        Assert.IsNotNull(model);
        Assert.AreEqual("WorkflowData", model.Data);
        Assert.AreEqual(3, model.Steps.Count);

        AssertStep(model.Steps[0], "step1", null, null);

        WorkflowBranchModel? step2 = model.Steps[1] as WorkflowBranchModel;
        Assert.IsNotNull(step2);
        Assert.AreEqual("+If", step2.Name);
        Assert.AreEqual("data.Result > 5", step2.Condition);
        Assert.AreEqual(3, step2.Steps.Count);

        AssertStep(model.Steps[2], "step3", null, null);

        AssertStep(step2.Steps[0], "step11", null, null);

        AssertStep(step2.Steps[1], "step12", null, null);

        WorkflowBranchModel? step13 = step2.Steps[2] as WorkflowBranchModel;
        Assert.IsNotNull(step13);
        Assert.AreEqual("+If", step13.Name);
        Assert.AreEqual("data.SomeFlag", step13.Condition);
        Assert.AreEqual(2, step13.Steps.Count);

        AssertStep(step13.Steps[0], "step111", null, null);

        AssertStep(step13.Steps[1], "step112", null, null);
    }

    private void AssertStep(WorkflowNodeModel node, string expectedName,
        List<KeyValuePair<string, object>>? expectedInput, KeyValuePair<string, object>? expectedOutput)
    {
        WorkflowStepModel? step = node as WorkflowStepModel;

        Assert.IsNotNull(step);
        Assert.AreEqual(expectedName, step.Name);

        if (expectedInput != null)
        {
            Assert.IsNotNull(step.Input);
            List<KeyValuePair<string, object>> inputList = step.Input
                .Select(x => new KeyValuePair<string, object>(x.Key, x.Value?.ToString()!))
                .ToList();
            CollectionAssert.AreEqual(expectedInput, inputList);
        }
        else
        {
            Assert.IsNull(step.Input);
        }

        if (expectedOutput != null)
        {
            Assert.IsNotNull(step.Output);
            KeyValuePair<string, object> output = step.Output
                .Select(x => new KeyValuePair<string, object>(x.Key, x.Value?.ToString()!))
                .Single();
            Assert.AreEqual(expectedOutput.Value.Key, output.Key);
            Assert.AreEqual(expectedOutput.Value.Value, output.Value);
        }
        else
        {
            Assert.IsNull(step.Output);
        }
    }
}