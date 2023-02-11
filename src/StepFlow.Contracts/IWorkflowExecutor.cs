using System.Threading.Tasks;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Contracts
{
    public interface IWorkflowExecutor
    {
        Task StartWorkflow<TData>(IWorkflow<TData> workflow, TData? data = default)
            where TData : new();

        Task StartWorkflow(WorkflowDefinition definition, object? data = null);
    }
}