using System.Collections.Generic;
using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Graph;

public class WorkflowNode
{
    public WorkflowNode(string id, WorkflowNodeType nodeType, WorkflowNodeDefinition definition)
    {
        Id = id;
        NodeType = nodeType;
        Definition = definition;
    }

    public string Id { get; }

    public WorkflowNodeType NodeType { get; }

    public WorkflowNodeDefinition Definition { get; }

    public IReadOnlyList<string> Directions => _definitions;

    public void AddDirection(string id)
    {
        _definitions.Add(id);
    }

    private readonly List<string> _definitions = new();
}