using System;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core;

internal class WorkflowStepBuilder
{
    public WorkflowStepBuilder(Type stepType, IStepPropertiesAccessor propertiesAccessor)
    {
        _stepType = stepType;
        _propertiesAccessor = propertiesAccessor;
    }

    public WorkflowStepDefinition Build()
    {
        WorkflowStepDefinition definition = new(_stepType)
        {
            Input = _propertiesAccessor.GetInput(),
            Output = _propertiesAccessor.GetOutput()
        };
        return definition;
    }

    private readonly Type _stepType;
    private readonly IStepPropertiesAccessor _propertiesAccessor;
}