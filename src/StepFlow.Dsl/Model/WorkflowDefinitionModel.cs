using System.Collections.Generic;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Dsl.Model
{
    public record WorkflowDefinitionModel(
        string? Data,
        List<WorkflowNodeModel> Steps);
}