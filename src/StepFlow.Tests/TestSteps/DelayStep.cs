using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestSteps;

public class DelayStep : IStep
{
    public int Milliseconds { get; set; }

    public async Task ExecuteAsync()
    {
        await Task.Delay(Milliseconds);
    }
}