using System.Collections.Generic;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core;

public interface IStepPropertiesAccessor
{
    List<PropertyMap> GetInput();

    PropertyMap? GetOutput();
}