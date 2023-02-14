using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definition;

public record WorkflowDefinition(
    Type DataType,
    List<WorkflowNodeDefinition> Nodes);