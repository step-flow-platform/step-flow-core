// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Tests;
//
// public static class TestHelper
// {
//     public static void AssertBranch(WorkflowBranchDefinition? branch, bool expectedHasCondition, int expectedNodesCount)
//     {
//         Assert.IsNotNull(branch);
//         Assert.AreEqual(expectedHasCondition, branch.Condition != null);
//         Assert.AreEqual(expectedNodesCount, branch.Nodes.Count);
//     }
//
//     public static void AssertStep(WorkflowNodeDefinition node, string expectedType, int expectedInputCount,
//         bool expectedHasOutput)
//     {
//         WorkflowStepDefinition? step = node as WorkflowStepDefinition;
//         Assert.IsNotNull(step);
//         Assert.AreEqual(expectedType, step.StepType.FullName);
//         Assert.AreEqual(expectedInputCount, step.Input.Count);
//         Assert.AreEqual(expectedHasOutput, step.Output != null);
//     }
// }