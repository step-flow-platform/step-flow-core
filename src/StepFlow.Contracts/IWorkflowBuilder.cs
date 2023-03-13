using System;
using System.Linq.Expressions;

namespace StepFlow.Contracts;

public interface IWorkflowBuilder<TData>
    where TData : new()
{
    IWorkflowBuilder<TData> Step<TStep>()
        where TStep : IStep;

    IWorkflowBuilder<TData> Step<TStep>(Action<IWorkflowStepBuilder<TStep, TData>> stepBuilderAction)
        where TStep : IStep;

    IWorkflowBuilder<TData> If(Expression<Func<TData, bool>> condition,
        Action<IWorkflowBuilder<TData>> ifBuilderAction);

    IWorkflowBuilder<TData> If(string id, Expression<Func<TData, bool>> condition,
        Action<IWorkflowBuilder<TData>> ifBuilderAction);

    void GoTo(string nodeId);
}