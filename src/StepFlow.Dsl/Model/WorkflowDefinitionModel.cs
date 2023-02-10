using System.Collections.Generic;

namespace StepFlow.Dsl.Model
{
    public record WorkflowDefinitionModel(
        string? Data,
        List<WorkflowNodeModel> Steps);
}