using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

internal class WorkflowIfBuilder : IWorkflowNodeBuilder
{
    public WorkflowIfBuilder(string? nodeId, LambdaExpression condition, List<IWorkflowNodeBuilder> nodes)
    {
        _nodeId = nodeId;
        _condition = condition;
        _nodes = nodes;
    }

    public WorkflowNodeDefinition Build()
    {
        List<WorkflowNodeDefinition> nodeDefinitions = _nodes.Select(x => x.Build()).ToList();
        WorkflowIfDefinition definition = new(_nodeId, _condition, nodeDefinitions);
        return definition;
    }

    private readonly string? _nodeId;
    private readonly LambdaExpression _condition;
    private readonly List<IWorkflowNodeBuilder> _nodes;
}