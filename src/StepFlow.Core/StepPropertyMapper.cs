using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class StepPropertyMapper<TStep, TData> : IStepPropertyMapper<TStep, TData>
    where TStep : IStep
{
    public IStepPropertyMapper<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
        Expression<Func<TData, TInput>> value)
    {
        _inputs.Add(new PropertyAssigner(value, stepProperty));
        return this;
    }

    public void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty)
    {
        _output = new PropertyAssigner(stepProperty, value);
    }

    public void MapInputs(TStep step, object data)
    {
        foreach (PropertyAssigner input in _inputs)
        {
            input.Assign(data, step);
        }
    }

    public void MapOutput(TStep step, object data)
    {
        _output?.Assign(step, data);
    }

    private readonly List<PropertyAssigner> _inputs = new();
    private PropertyAssigner? _output;
}