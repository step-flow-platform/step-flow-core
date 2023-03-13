using System.Threading.Tasks;
using StepFlow.Contracts.Definition;

namespace StepFlow.Contracts
{
    public interface IWorkflowExecutor
    {
        Task Start(WorkflowDefinition definition, object? data = null);

        Task Execute<TData>(IWorkflow<TData> workflow, TData? data = default)
            where TData : new();
    }
}