using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definition;

public record WorkflowStepDefinition(
        string Id,
        string? NextNodeId,
        Type StepType,
        List<PropertyMap> Input,
        PropertyMap? Output)
    : WorkflowNodeDefinition(Id);