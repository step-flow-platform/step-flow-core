using System.Collections.Generic;

namespace StepFlow.Dsl.Model
{
    public class WorkflowDefinitionModel
    {
        public string? Data { get; set; } = default!;

        public List<WorkflowNodeModel> Steps { get; set; } = default!;
    }
}