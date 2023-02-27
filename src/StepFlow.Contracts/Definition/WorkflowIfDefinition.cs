using System.Collections.Generic;
using System.Linq.Expressions;

namespace StepFlow.Contracts.Definition;

public record WorkflowIfDefinition(
        string? Id,
        LambdaExpression Condition,
        List<WorkflowNodeDefinition> Nodes)
    : WorkflowNodeDefinition(Id);