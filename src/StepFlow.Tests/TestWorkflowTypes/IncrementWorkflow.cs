using StepFlow.Contracts;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.TestWorkflowTypes;

public class IncrementWorkflow : IWorkflow<IncrementWorkflowData>
{
    public void Build(IWorkflowBuilder<IncrementWorkflowData> builder)
    {
        builder
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, data => data.StartValue)
                .Output(data => data.ResultValue, step => step.IncrementedValue))
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, data => data.ResultValue)
                .Output(data => data.ResultValue, step => step.IncrementedValue))
            .If(data => data.ResultValue >= data.FirstCheckValue, _ => _
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, data => data.ResultValue)
                    .Output(data => data.ResultValue, step => step.IncrementedValue))
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, data => data.ResultValue)
                    .Output(data => data.ResultValue, step => step.IncrementedValue))
                .If(data => data.ResultValue >= data.SecondCheckValue, __ => __
                    .Step<IncrementStep>(x => x
                        .Input(step => step.Value, data => data.ResultValue)
                        .Output(data => data.ResultValue, step => step.IncrementedValue))))
            .Step<IncrementStep>(x => x
                .Input(step => step.Value, data => data.ResultValue)
                .Output(data => data.ResultValue, step => step.IncrementedValue));
    }
}