using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

internal class WorkflowGotoBuilder : IWorkflowNodeBuilder
{
    public WorkflowGotoBuilder(string? nodeId, string nextNodeId)
    {
        _nodeId = nodeId;
        _nextNodeId = nextNodeId;
    }

    public WorkflowNodeDefinition Build()
    {
        WorkflowGoToDefinition definition = new(_nodeId, _nextNodeId);
        return definition;
    }

    private readonly string? _nodeId;
    private readonly string _nextNodeId;
}