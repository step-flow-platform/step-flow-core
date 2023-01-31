using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class SomeWorkflow : IWorkflow<WorkflowData>
{
    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .Step<FirstStep>()
            .Step<PrintStep>(step => { step.Line = "Hello, StepFlow!"; })
            .Step<PrintStep>((step, data) => { step.Line = data.LineToPrint; })
            .Step<SecondStep>();
    }
}