using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts.Definitions;
using StepFlow.Core.Builders;
using StepFlow.Tests.TestWorkflowTypes;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowBuilderTest
{
    [TestMethod]
    public void BuildWorkflowDefinition()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        TestWorkflow workflow = new();
        workflow.Build(builder);
        WorkflowDefinition definition = builder.BuildDefinition();

        Assert.AreEqual("StepFlow.Tests.TestWorkflowTypes.WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(WorkflowNodeType.Branch, definition.MainBranch.NodeType);

        WorkflowBranchDefinition mainBranch = (definition.MainBranch as WorkflowBranchDefinition)!;
        TestHelper.AssertBranch(mainBranch, false, 6);
        TestHelper.AssertStep(mainBranch.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        TestHelper.AssertStep(mainBranch.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step2", 3, false);
        TestHelper.AssertStep(mainBranch.Nodes[2], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
        TestHelper.AssertStep(mainBranch.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        TestHelper.AssertStep(mainBranch.Nodes[5], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 0, false);

        WorkflowBranchDefinition ifBranch1 = (mainBranch.Nodes[4] as WorkflowBranchDefinition)!;
        TestHelper.AssertBranch(ifBranch1, true, 4);
        TestHelper.AssertStep(ifBranch1.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        TestHelper.AssertStep(ifBranch1.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step4", 2, false);
        TestHelper.AssertStep(ifBranch1.Nodes[3], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);

        WorkflowBranchDefinition ifBranch2 = (ifBranch1.Nodes[2] as WorkflowBranchDefinition)!;
        TestHelper.AssertBranch(ifBranch2, true, 2);
        TestHelper.AssertStep(ifBranch2.Nodes[0], "StepFlow.Tests.TestWorkflowTypes.Steps.Step1", 0, false);
        TestHelper.AssertStep(ifBranch2.Nodes[1], "StepFlow.Tests.TestWorkflowTypes.Steps.Step3", 1, true);
    }
}