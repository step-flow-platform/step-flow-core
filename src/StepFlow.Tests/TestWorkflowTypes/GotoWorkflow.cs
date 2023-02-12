using StepFlow.Contracts;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.TestWorkflowTypes;

public class GotoWorkflow : IWorkflow<GotoWorkflow.GotoWorkflowData>
{
    public class GotoWorkflowData
    {
        public int Value { get; set; }
    }

    public void Build(IWorkflowBuilder<GotoWorkflowData> builder)
    {
        builder
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, _ => 0)
                .Output(data => data.Value, step => step.IncrementedValue))
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, data => data.Value)
                .Output(data => data.Value, step => step.IncrementedValue))
            .GoTo("LastStep")
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, data => data.Value)
                .Output(data => data.Value, step => step.IncrementedValue))
            .Step<IncrementStep>(x => x
                .Id("LastStep")
                .Input(step => step.Value, data => data.Value)
                .Output(data => data.Value, step => step.IncrementedValue));
    }
}