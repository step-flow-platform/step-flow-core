using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestSteps;

public class IncrementStep : IStep
{
    public int Value { get; set; } = default!;

    public int IncrementedValue { get; private set; }

    public Task ExecuteAsync()
    {
        IncrementedValue = Value + 1;
        return Task.CompletedTask;
    }
}