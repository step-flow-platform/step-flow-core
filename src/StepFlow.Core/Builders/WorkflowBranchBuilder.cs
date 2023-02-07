using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core.Builders;

internal class WorkflowBranchBuilder<TData> : IWorkflowBuilder<TData>, IWorkflowNodeBuilder
    where TData : new()
{
    public WorkflowBranchBuilder()
    {
        _condition = null;
    }

    public WorkflowBranchBuilder(LambdaExpression condition)
    {
        _condition = condition;
    }

    public IWorkflowBuilder<TData> Step<TStep>()
        where TStep : IStep
    {
        StepBuilder<TStep, TData> stepBuilder = new();
        _steps.Add(stepBuilder);
        return this;
    }

    public IWorkflowBuilder<TData> Step<TStep>(Action<IStepBuilder<TStep, TData>> stepBuilderAction)
        where TStep : IStep
    {
        StepBuilder<TStep, TData> stepBuilder = new();
        stepBuilderAction(stepBuilder);
        _steps.Add(stepBuilder);
        return this;
    }

    public IWorkflowBuilder<TData> If(Expression<Func<TData, bool>> condition,
        Action<IWorkflowBuilder<TData>> ifBuilderAction)
    {
        WorkflowBranchBuilder<TData> branchBuilder = new(condition);
        ifBuilderAction(branchBuilder);
        _steps.Add(branchBuilder);
        return this;
    }

    public WorkflowNodeDefinition Build()
    {
        IEnumerable<WorkflowNodeDefinition> nodes = _steps.Select(stepBuilder => stepBuilder.Build()).ToList();
        WorkflowBranchDefinition definition = new(_condition, nodes.ToList());
        return definition;
    }

    private readonly List<IWorkflowNodeBuilder> _steps = new();
    private readonly LambdaExpression? _condition;
}