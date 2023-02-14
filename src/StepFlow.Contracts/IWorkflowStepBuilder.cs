using System;
using System.Linq.Expressions;

namespace StepFlow.Contracts
{
    public interface IWorkflowStepBuilder<TStep, TData>
        where TStep : IStep
    {
        IWorkflowStepBuilder<TStep, TData> Id(string id);

        IWorkflowStepBuilder<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
            Expression<Func<TData, TInput>> value);

        void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty);
    }
}