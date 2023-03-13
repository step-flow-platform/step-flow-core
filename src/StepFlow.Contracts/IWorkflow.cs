namespace StepFlow.Contracts
{
    public interface IWorkflow<TData>
        where TData : new()
    {
        public string Name { get; }

        void Build(IWorkflowBuilder<TData> builder);
    }

    public interface IWorkflow : IWorkflow<object>
    {
    }
}