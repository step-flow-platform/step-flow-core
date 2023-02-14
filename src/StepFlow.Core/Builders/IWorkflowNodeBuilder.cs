using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

internal interface IWorkflowNodeBuilder
{
    public string NodeId { get; }

    public string? NextNodeId { get; set; }

    WorkflowNodeDefinition Build();
}