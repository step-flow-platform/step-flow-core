using System.Dynamic;

namespace StepFlow.Dsl.Model
{
    public class WorkflowStepModel
    {
        public string Name { get; set; } = default!;

        public ExpandoObject? Input { get; set; } = default!;

        public ExpandoObject? Output { get; set; } = default!;
    }
}