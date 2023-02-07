using System.Collections.Generic;
using System.Linq.Expressions;

namespace StepFlow.Contracts.Definitions;

public record WorkflowBranchDefinition(
        LambdaExpression? Condition,
        List<WorkflowNodeDefinition> Nodes)
    : WorkflowNodeDefinition(WorkflowNodeType.Branch);