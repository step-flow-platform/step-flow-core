using System;
using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

public class WorkflowGotoBuilder : IWorkflowNodeBuilder
{
    public WorkflowGotoBuilder(string nextNodeId)
    {
        NodeId = Guid.NewGuid().ToString();
        _nextNodeId = nextNodeId;
    }

    public string NodeId { get; }

    public string? NextNodeId
    {
        get => _nextNodeId;
        set => throw new InvalidOperationException();
    }

    public WorkflowNodeDefinition Build()
    {
        WorkflowGoToDefinition definition = new(NodeId, _nextNodeId);
        return definition;
    }

    private readonly string _nextNodeId;
}