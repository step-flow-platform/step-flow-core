using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definition;

public record WorkflowStepDefinition(
        string? Id,
        Type StepType,
        List<PropertyMap> Input,
        PropertyMap? Output)
    : WorkflowNodeDefinition(Id);