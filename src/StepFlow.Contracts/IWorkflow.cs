namespace StepFlow.Contracts
{
    public interface IWorkflow<TData>
        where TData : new()
    {
        void Build(IWorkflowBuilder<TData> builder);
    }

    public interface IWorkflow : IWorkflow<object>
    {
    }
}