using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestSteps;

public class Step3 : IStep
{
    public int Property1 { get; set; } = default!;

    public string Property2 { get; set; } = default!;

    public Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}