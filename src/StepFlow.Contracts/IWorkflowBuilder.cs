namespace StepFlow.Contracts
{
    public interface IWorkflowBuilder
    {
        IWorkflowBuilder Step<T>()
            where T : IStep;
    }
}