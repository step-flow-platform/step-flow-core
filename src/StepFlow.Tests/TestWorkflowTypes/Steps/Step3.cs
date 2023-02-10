using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflowTypes.Steps;

public class Step3 : IStep
{
    public string Prop { get; set; } = default!;

    public string Result { get; private set; } = default!;

    public Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}