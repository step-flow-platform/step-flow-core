using StepFlow.Contracts;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.TestWorkflowTypes;

public class TestWorkflow : IWorkflow<WorkflowData>
{
    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .Step<Step1>()
            .Step<Step2>(x => x
                .Input(step => step.Prop1, _ => "SomeValue")
                .Input(step => step.Prop2, _ => 42)
                .Input(step => step.Prop3, data => data.StringValue))
            .Step<Step3>(x => x
                .Input(step => step.Prop, _ => "value")
                .Output(data => data.StringValue, step => step.Result))
            .Step<Step1>()
            .If(data => data.IntValue > 5, _ => _
                .Step<Step1>()
                .Step<Step4>(x => x
                    .Input(step => step.PropA, _ => "val-a")
                    .Input(step => step.PropB, _ => 1990))
                .If(data => data.BoolValue, __ => __
                    .Step<Step1>()
                    .Step<Step3>(x => x
                        .Input(step => step.Prop, data => data.StringValue)
                        .Output(data => data.StringValue, step => step.Result)))
                .Step<Step1>())
            .Step<Step3>();
    }
}