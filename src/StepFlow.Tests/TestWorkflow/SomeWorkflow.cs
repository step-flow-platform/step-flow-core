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
            .Step<SumStep>((step, data) =>
            {
                step.Number1 = data.Number1;
                step.Number2 = 4;
            }, (data, step) => { data.SumResult = step.Result; })
            .Step<PrintStep>((step, data) => { step.Line = data.SumResult.ToString(); })
            .Step<SecondStep>();
    }
}