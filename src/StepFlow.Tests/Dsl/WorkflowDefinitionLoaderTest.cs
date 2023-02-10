using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts.Definitions;
using StepFlow.Dsl;

namespace StepFlow.Tests.Dsl;

[TestClass]
public class WorkflowDefinitionLoaderTest
{
    [TestMethod]
    public void LoadJsonDefinition()
    {
        string json = File.ReadAllText("Dsl/JsonAssets/test-workflow.json");

        WorkflowDefinitionLoaderOptions loaderOptions = new()
        {
            AssemblyName = "StepFlow.Tests",
            DataNamespace = "StepFlow.Tests.TestWorkflowTypes",
            StepsNamespace = "StepFlow.Tests.TestWorkflowTypes.Steps"
        };
        WorkflowDefinitionLoader loader = new(loaderOptions);
        WorkflowDefinition definition = loader.Load(json, Deserializers.Json);

        Assert.AreEqual("StepFlow.Tests.TestWorkflowTypes.WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(WorkflowNodeType.Branch, definition.MainBranch.NodeType);

        WorkflowBranchDefinition mainBranch = (definition.MainBranch as WorkflowBranchDefinition)!;
        AssertBranch(mainBranch, false, 6);
        AssertStep(mainBranch.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        AssertStep(mainBranch.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step2", 3, false);
        AssertStep(mainBranch.Nodes[2], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
        AssertStep(mainBranch.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        AssertStep(mainBranch.Nodes[5], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 0, false);

        WorkflowBranchDefinition ifBranch1 = (mainBranch.Nodes[4] as WorkflowBranchDefinition)!;
        AssertBranch(ifBranch1, true, 4);
        AssertStep(ifBranch1.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        AssertStep(ifBranch1.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step4", 2, false);
        AssertStep(ifBranch1.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);

        WorkflowBranchDefinition ifBranch2 = (ifBranch1.Nodes[2] as WorkflowBranchDefinition)!;
        AssertBranch(ifBranch2, true, 2);
        AssertStep(ifBranch2.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        AssertStep(ifBranch2.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
    }

    private void AssertBranch(WorkflowBranchDefinition? branch, bool expectedHasCondition, int expectedNodesCount)
    {
        Assert.IsNotNull(branch);
        Assert.AreEqual(expectedHasCondition, branch.Condition != null);
        Assert.AreEqual(expectedNodesCount, branch.Nodes.Count);
    }

    private void AssertStep(WorkflowNodeDefinition node, string expectedType, int expectedInputCount,
        bool expectedHasOutput)
    {
        WorkflowStepDefinition? step = node as WorkflowStepDefinition;
        Assert.IsNotNull(step);
        Assert.AreEqual(expectedType, step.StepType.FullName);
        Assert.AreEqual(expectedInputCount, step.Input.Count);
        Assert.AreEqual(expectedHasOutput, step.Output != null);
    }
}