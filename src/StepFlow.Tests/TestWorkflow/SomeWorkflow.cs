using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class SomeWorkflow : IWorkflow
{
    public void Build(IWorkflowBuilder builder)
    {
        builder
            .Step<FirstStep>()
            .Step<SecondStep>()
            .Step<SecondStep>()
            .Step<FirstStep>();
    }
}