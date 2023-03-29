using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

internal class WorkflowBuilder<TData> : IWorkflowBuilder<TData>
    where TData : new()
{
    public IWorkflowBuilder<TData> Step<TStep>()
        where TStep : IStep
    {
        return Step<TStep>(_ => { });
    }

    public IWorkflowBuilder<TData> Step<TStep>(Action<IWorkflowStepBuilder<TStep, TData>> stepBuilderAction)
        where TStep : IStep
    {
        WorkflowStepBuilder<TStep, TData> workflowStepBuilder = new();
        stepBuilderAction(workflowStepBuilder);
        _nodes.Add(workflowStepBuilder);
        return this;
    }

    public IWorkflowBuilder<TData> If(Expression<Func<TData, bool>> condition,
        Action<IWorkflowBuilder<TData>> ifBuilderAction)
    {
        return If(null, condition, ifBuilderAction);
    }

    public IWorkflowBuilder<TData> If(string? id, Expression<Func<TData, bool>> condition,
        Action<IWorkflowBuilder<TData>> ifBuilderAction)
    {
        WorkflowBuilder<TData> subBuilder = new();
        ifBuilderAction(subBuilder);

        WorkflowIfBuilder ifBuilder = new(id, condition, subBuilder._nodes);
        _nodes.Add(ifBuilder);
        return this;
    }

    public void GoTo(string stepName)
    {
        WorkflowGotoBuilder goToBuilder = new(null, stepName);
        _nodes.Add(goToBuilder);
    }

    public IWorkflowBuilder<TData> WaitForEvent(string eventName)
    {
        Step<WaitEventStep>(x => x
            .Input(step => step.EventName, _ => eventName));
        return this;
    }

    public WorkflowDefinition BuildDefinition(string name)
    {
        IEnumerable<WorkflowNodeDefinition> nodeDefinitions = _nodes.Select(x => x.Build());
        return new WorkflowDefinition(name, typeof(TData), nodeDefinitions.ToList());
    }

    private readonly List<IWorkflowNodeBuilder> _nodes = new();
}