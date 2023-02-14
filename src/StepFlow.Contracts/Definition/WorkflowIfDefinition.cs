using System.Linq.Expressions;

namespace StepFlow.Contracts.Definition;

public record WorkflowIfDefinition(
        string Id,
        LambdaExpression Condition,
        string? TrueNodeId,
        string? FalseNodeId)
    : WorkflowNodeDefinition(Id);