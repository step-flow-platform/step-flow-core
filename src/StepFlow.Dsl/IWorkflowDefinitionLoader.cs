using System;
using StepFlow.Contracts.Definition;
using StepFlow.Dsl.Model;

namespace StepFlow.Dsl;

public interface IWorkflowDefinitionLoader
{
    WorkflowDefinition Load(string source, Func<string, WorkflowDefinitionModel?> deserializer);
}