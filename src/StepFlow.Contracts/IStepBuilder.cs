using System;
using System.Linq.Expressions;

namespace StepFlow.Contracts
{
    public interface IStepBuilder<TStep, TData>
        where TStep : IStep
    {
        IStepBuilder<TStep, TData> Id(string id);

        IStepBuilder<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
            Expression<Func<TData, TInput>> value);

        void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty);
    }
}