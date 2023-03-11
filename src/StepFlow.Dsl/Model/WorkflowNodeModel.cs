using System.Collections.Generic;
using System.Dynamic;

namespace StepFlow.Dsl.Model;

public class WorkflowNodeModel
{
    public string Type { get; set; } = default!;

    public string? Id { get; set; } = default!;

    public ExpandoObject? Input { get; set; } = default!;

    public ExpandoObject? Output { get; set; } = default!;

    public string? Condition { get; set; } = default!;

    public string? NextId { get; set; } = default!;

    public List<WorkflowNodeModel> Steps { get; set; } = default!;
}