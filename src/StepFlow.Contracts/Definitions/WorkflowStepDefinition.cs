using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definitions;

public record WorkflowStepDefinition(
        Type StepType,
        List<PropertyMap> Input,
        PropertyMap? Output)
    : WorkflowNodeDefinition(WorkflowNodeType.Step);