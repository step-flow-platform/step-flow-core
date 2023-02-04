using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core;

internal class StepPropertyBuilder<TStep, TData> : IStepPropertyBuilder<TStep, TData>, IStepPropertyBuilder
    where TStep : IStep
{
    public IStepPropertyBuilder<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
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

    public WorkflowStepDefinition Build()
    {
        WorkflowStepDefinition definition = new(typeof(TStep))
        {
            Input = _input,
            Output = _output
        };
        return definition;
    }

    private readonly List<PropertyMap> _input = new();
    private PropertyMap? _output;
}

internal interface IStepPropertyBuilder
{
    public WorkflowStepDefinition Build();
}