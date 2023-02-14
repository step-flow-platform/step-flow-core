using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflowTypes.Steps;

public class ConcatenateStringsStep : IStep
{
    public string First { get; set; } = default!;

    public string Second { get; set; } = default!;

    public string Result { get; private set; } = default!;

    public Task ExecuteAsync()
    {
        Result = First + Second;
        return Task.CompletedTask;
    }
}