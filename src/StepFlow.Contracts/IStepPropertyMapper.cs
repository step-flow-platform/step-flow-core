using System;
using System.Linq.Expressions;

namespace StepFlow.Contracts
{
    public interface IStepPropertyMapper<TStep, TData>
        where TStep : IStep
    {
        IStepPropertyMapper<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
            Expression<Func<TData, TInput>> value);

        void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty);
    }
}