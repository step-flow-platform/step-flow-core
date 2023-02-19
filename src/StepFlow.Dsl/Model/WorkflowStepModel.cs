using System.Dynamic;

namespace StepFlow.Dsl.Model;

public class WorkflowStepModel : WorkflowNodeModel
{
    public ExpandoObject? Input { get; set; } = default!;

    public ExpandoObject? Output { get; set; } = default!;
}