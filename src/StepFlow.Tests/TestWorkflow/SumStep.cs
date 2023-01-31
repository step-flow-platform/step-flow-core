using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class SumStep : IStep
{
    public int Number1 { get; set; }

    public int Number2 { get; set; }

    public int Result { get; set; }

    public Task ExecuteAsync()
    {
        Result = Number1 + Number2;
        return Task.CompletedTask;
    }
}