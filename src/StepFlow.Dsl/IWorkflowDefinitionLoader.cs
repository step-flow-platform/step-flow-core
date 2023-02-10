using System;
using StepFlow.Contracts.Definitions;
using StepFlow.Dsl.Model;

namespace StepFlow.Dsl;

public interface IWorkflowDefinitionLoader
{
    WorkflowDefinition Load(string source, Func<string, WorkflowDefinitionModel?> deserializer);
}