using System.Dynamic;

namespace StepFlow.Dsl.Model;

public record WorkflowStepModel(
        string Name,
        ExpandoObject? Input,
        ExpandoObject? Output)
    : WorkflowNodeModel(Name);