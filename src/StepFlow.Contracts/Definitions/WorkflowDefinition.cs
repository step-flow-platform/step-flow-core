using System;

namespace StepFlow.Contracts.Definitions;

public record WorkflowDefinition(
    Type DataType,
    WorkflowNodeDefinition MainBranch);