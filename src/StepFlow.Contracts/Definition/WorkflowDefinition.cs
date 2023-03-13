using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definition;

public record WorkflowDefinition(
    string Name,
    Type DataType,
    List<WorkflowNodeDefinition> Nodes);