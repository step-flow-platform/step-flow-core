using StepFlow.Contracts.Definitions;

namespace StepFlow.Core.Builders;

internal interface IWorkflowNodeBuilder
{
    WorkflowNodeDefinition Build();
}