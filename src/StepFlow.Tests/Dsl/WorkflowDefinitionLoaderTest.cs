// using System.IO;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using StepFlow.Dsl;
//
// namespace StepFlow.Tests.Dsl;
//
// [TestClass]
// public class WorkflowDefinitionLoaderTest
// {
//     [TestMethod]
//     public void LoadJsonDefinition()
//     {
//         string json = File.ReadAllText("Dsl/JsonAssets/test-workflow.json");
//
//         WorkflowDefinitionLoaderOptions loaderOptions = new()
//         {
//             AssemblyName = "StepFlow.Tests",
//             DataNamespace = "StepFlow.Tests.TestWorkflowTypes",
//             StepsNamespace = "StepFlow.Tests.TestWorkflowTypes.Steps"
//         };
//         WorkflowDefinitionLoader loader = new(loaderOptions);
//         WorkflowDefinition definition = loader.Load(json, Deserializers.Json);
//
//         Assert.AreEqual("StepFlow.Tests.TestWorkflowTypes.WorkflowData", definition.DataType.FullName);
//         Assert.AreEqual(WorkflowNodeType.Branch, definition.MainBranch.NodeType);
//
//         WorkflowBranchDefinition mainBranch = (definition.MainBranch as WorkflowBranchDefinition)!;
//         WorkflowAssert.AssertBranch(mainBranch, false, 6);
//         WorkflowAssert.StepDefinition(mainBranch.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
//         WorkflowAssert.StepDefinition(mainBranch.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step2", 3, false);
//         WorkflowAssert.StepDefinition(mainBranch.Nodes[2], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
//         WorkflowAssert.StepDefinition(mainBranch.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
//         WorkflowAssert.StepDefinition(mainBranch.Nodes[5], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 0, false);
//
//         WorkflowBranchDefinition ifBranch1 = (mainBranch.Nodes[4] as WorkflowBranchDefinition)!;
//         WorkflowAssert.AssertBranch(ifBranch1, true, 4);
//         WorkflowAssert.StepDefinition(ifBranch1.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
//         WorkflowAssert.StepDefinition(ifBranch1.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step4", 2, false);
//         WorkflowAssert.StepDefinition(ifBranch1.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
//
//         WorkflowBranchDefinition ifBranch2 = (ifBranch1.Nodes[2] as WorkflowBranchDefinition)!;
//         WorkflowAssert.AssertBranch(ifBranch2, true, 2);
//         WorkflowAssert.StepDefinition(ifBranch2.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
//         WorkflowAssert.StepDefinition(ifBranch2.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
//     }
// }