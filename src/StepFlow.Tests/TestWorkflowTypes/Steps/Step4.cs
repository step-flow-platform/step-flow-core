using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflowTypes.Steps;

public class Step4 : IStep
{
    public string PropA { get; set; } = default!;

    public int PropB { get; set; } = default!;

    public Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}