using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Builders;

internal interface IWorkflowNodeBuilder
{
    WorkflowNodeDefinition Build();
}