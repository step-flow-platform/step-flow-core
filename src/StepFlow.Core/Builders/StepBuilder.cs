using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core.Builders;

internal class StepBuilder<TStep, TData> : IStepBuilder<TStep, TData>, IWorkflowNodeBuilder
    where TStep : IStep
{
    public IStepBuilder<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
        Expression<Func<TData, TInput>> value)
    {
        _input.Add(new PropertyMap(value, stepProperty));
        return this;
    }

    public void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty)
    {
        _output = new PropertyMap(stepProperty, value);
    }

    public WorkflowNodeDefinition Build()
    {
        WorkflowStepDefinition definition = new(typeof(TStep), _input, _output);
        return definition;
    }

    private readonly List<PropertyMap> _input = new();
    private PropertyMap? _output;
}