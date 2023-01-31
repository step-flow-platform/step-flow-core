using System.Threading.Tasks;

namespace StepFlow.Contracts
{
    public interface IWorkflowExecutor
    {
        Task StartWorkflow<TData>(IWorkflow<TData> workflow)
            where TData : new();
    }
}