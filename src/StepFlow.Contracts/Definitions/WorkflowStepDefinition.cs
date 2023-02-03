using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definitions
{
    public class WorkflowStepDefinition
    {
        public WorkflowStepDefinition(Type stepType)
        {
            StepType = stepType;
            Input = new List<PropertyMap>();
        }

        public Type StepType { get; }

        public List<PropertyMap> Input { get; set; }

        public PropertyMap? Output { get; set; }
    }
}