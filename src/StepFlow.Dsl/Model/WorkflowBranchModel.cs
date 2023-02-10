using System.Collections.Generic;

namespace StepFlow.Dsl.Model;

public record WorkflowBranchModel(
        string Name,
        string Condition,
        List<WorkflowNodeModel> Steps)
    : WorkflowNodeModel(Name);