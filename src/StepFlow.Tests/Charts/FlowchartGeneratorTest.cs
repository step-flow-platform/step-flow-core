using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;
using StepFlow.Core.Builders;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.Charts;

[TestClass]
public class FlowchartGeneratorTest
{
    [TestMethod]
    public void GenerateFlowchart()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        Workflow workflow = new();
        workflow.Build(builder);
        WorkflowDefinition definition = builder.BuildDefinition();

        FlowchartGenerator generator = new();
        string flowchart = generator.Generate(definition);

        Assert.IsFalse(string.IsNullOrEmpty(flowchart));
    }

    private class Workflow : IWorkflow<WorkflowData>
    {
        public void Build(IWorkflowBuilder<WorkflowData> builder)
        {
            builder
                .Step<Step1>()
                .If(data => data.A > 5, _ => _
                    .Step<Step1>())
                .If(data => data.B > 10, _ => _
                    .Step<Step1>()
                    .If(data => data.A >= data.B, __ => __
                        .Step<Step1>()
                        .Step<Step2>()
                        .Step<Step3>()
                        .Step<Step4>()))
                .Step<Step2>();
        }
    }

    private class WorkflowData
    {
        public int A { get; set; }

        public int B { get; set; }
    }
}