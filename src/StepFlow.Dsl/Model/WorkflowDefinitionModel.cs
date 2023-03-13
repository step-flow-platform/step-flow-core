using System.Collections.Generic;

namespace StepFlow.Dsl.Model
{
    public class WorkflowDefinitionModel
    {
        public string Name { get; set; } = default!;

        public string? Data { get; set; }

        public List<WorkflowNodeModel> Steps { get; set; } = default!;
    }
}