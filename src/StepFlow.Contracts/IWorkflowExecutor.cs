using System.Threading.Tasks;

namespace StepFlow.Contracts
{
    public interface IWorkflowExecutor
    {
        Task StartWorkflow(IWorkflow workflow);
    }
}