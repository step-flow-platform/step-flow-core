// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Tests;
//
// public static class WorkflowAssert
// {
//     public static void StepDefinition(WorkflowNodeDefinition node, string expectedType, int expectedInputCount,
//         bool expectedHasOutput, string? expectedId = null)
//     {
//         WorkflowStepDefinition? stepDefinition = node as WorkflowStepDefinition;
//         Assert.IsNotNull(stepDefinition);
//         Assert.AreEqual(expectedType, stepDefinition.StepType.FullName);
//         Assert.AreEqual(expectedInputCount, stepDefinition.Input.Count);
//         Assert.AreEqual(expectedHasOutput, stepDefinition.Output != null);
//         if (expectedId is not null)
//         {
//             Assert.AreEqual(expectedId, stepDefinition.Id);
//         }
//     }
//
//     public static void IfDefinition(WorkflowNodeDefinition node, string expectedCondition, string? expectedId = null)
//     {
//         WorkflowIfDefinition? ifDefinition = node as WorkflowIfDefinition;
//         Assert.IsNotNull(ifDefinition);
//         Assert.AreEqual(expectedCondition, ifDefinition.Condition.Body.ToString());
//         if (expectedId is not null)
//         {
//             Assert.AreEqual(expectedId, ifDefinition.Id);
//         }
//     }
//
//     public static void GoToDefinition(WorkflowNodeDefinition node, string expectedNextNodeId)
//     {
//         WorkflowGoToDefinition? goToDefinition = node as WorkflowGoToDefinition;
//         Assert.IsNotNull(goToDefinition);
//         Assert.AreEqual(expectedNextNodeId, goToDefinition.NextNodeId);
//     }
// }