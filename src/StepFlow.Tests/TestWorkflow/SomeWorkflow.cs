using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class SomeWorkflow : IWorkflow<WorkflowData>
{
    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .Step<FirstStep>()
            .Step<PrintStep>(m => m
                .Input(step => step.Line, _ => "Hello, StepFlow!"))
            .Step<PrintStep>(m => m
                .Input(step => step.Line, data => data.LineToPrint))
            .Step<SumStep>(m => m
                .Input(step => step.Number1, data => data.Number1)
                .Input(step => step.Number2, _ => 4)
                .Output(data => data.SumResult, step => step.Result))
            .Step<PrintStep>(m => m
                .Input(step => step.Line, data => data.SumResult.ToString()))
            .Step<SecondStep>();
    }
}