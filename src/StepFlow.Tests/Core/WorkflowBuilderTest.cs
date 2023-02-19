using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Builders;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowBuilderTest
{
    [TestMethod]
    public void BuildWorkflowDefinition()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        Workflow workflow = new();
        workflow.Build(builder);
        WorkflowDefinition definition = builder.BuildDefinition();

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(10, definition.Nodes.Count);

        WorkflowAssert.StepDefinition(definition.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false, "id1");
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.IfDefinition(definition.Nodes[2], "(data.A > 10)", "if1");
        WorkflowAssert.StepDefinition(definition.Nodes[3], "StepFlow.Tests.TestSteps.Step3", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[4], "StepFlow.Tests.TestSteps.Step3", 0, false);
        WorkflowAssert.IfDefinition(definition.Nodes[5], "(data.B > 20)");
        WorkflowAssert.StepDefinition(definition.Nodes[6], "StepFlow.Tests.TestSteps.Step4", 0, false, "id2");
        WorkflowAssert.StepDefinition(definition.Nodes[7], "StepFlow.Tests.TestSteps.Step4", 0, false);
        WorkflowAssert.GoToDefinition(definition.Nodes[8], "if1");
        WorkflowAssert.StepDefinition(definition.Nodes[9], "StepFlow.Tests.TestSteps.Step5", 0, false);
    }

    private class WorkflowData
    {
        public int A { get; set; } = default!;

        public int B { get; set; } = default!;
    }

    private class Workflow : IWorkflow<WorkflowData>
    {
        public void Build(IWorkflowBuilder<WorkflowData> builder)
        {
            builder
                .Step<Step1>(x => x
                    .Id("id1"))
                .Step<Step2>()
                .If("if1", data => data.A > 10, _ => _
                    .Step<Step3>()
                    .Step<Step3>()
                    .If(data => data.B > 20, __ => __
                        .Step<Step4>(x => x
                            .Id("id2"))
                        .Step<Step4>()
                        .GoTo("if1")))
                .Step<Step5>();
        }
    }
}