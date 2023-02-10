using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflowTypes.Steps;

public class Step2 : IStep
{
    public string Prop1 { get; set; } = default!;

    public int Prop2 { get; set; } = default!;

    public string Prop3 { get; set; } = default!;

    public Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}