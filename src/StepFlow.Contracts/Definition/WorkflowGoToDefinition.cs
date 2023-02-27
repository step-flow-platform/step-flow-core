namespace StepFlow.Contracts.Definition;

public record WorkflowGoToDefinition(
        string? Id,
        string NextNodeId)
    : WorkflowNodeDefinition(Id);