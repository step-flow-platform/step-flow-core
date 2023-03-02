using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestSteps;

public class Step2 : IStep
{
    public int Property1 { get; set; } = default!;

    public Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}