using System.Threading.Tasks;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Contracts
{
    public interface IWorkflowExecutor
    {
        Task StartWorkflow<TData>(IWorkflow<TData> workflow)
            where TData : new();

        Task StartWorkflow(WorkflowDefinition definition);
    }
}