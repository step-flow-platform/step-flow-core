using System;

namespace StepFlow.Contracts
{
    public interface IWorkflowBuilder<TData>
        where TData : new()
    {
        IWorkflowBuilder<TData> Step<TStep>()
            where TStep : IStep;

        IWorkflowBuilder<TData> Step<TStep>(Action<IStepPropertyMapper<TStep, TData>> propertyMapper)
            where TStep : IStep;
    }
}