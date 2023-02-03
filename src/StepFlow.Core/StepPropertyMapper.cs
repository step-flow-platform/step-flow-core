using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core;

internal class StepPropertyMapper<TStep, TData> : IStepPropertyMapper<TStep, TData>, IStepPropertiesAccessor
    where TStep : IStep
{
    public IStepPropertyMapper<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
        Expression<Func<TData, TInput>> value)
    {
        _input.Add(new PropertyMap(value, stepProperty));
        return this;
    }

    public void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty)
    {
        _output = new PropertyMap(stepProperty, value);
    }

    public List<PropertyMap> GetInput()
    {
        return _input;
    }

    public PropertyMap? GetOutput()
    {
        return _output;
    }

    private readonly List<PropertyMap> _input = new();
    private PropertyMap? _output;
}