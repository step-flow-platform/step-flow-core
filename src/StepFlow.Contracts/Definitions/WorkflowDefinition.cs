using System;
using System.Collections.Generic;

namespace StepFlow.Contracts.Definitions
{
    public class WorkflowDefinition
    {
        public WorkflowDefinition(Type dataType)
        {
            DataType = dataType;
            Steps = new List<WorkflowStepDefinition>();
        }

        public Type DataType { get; }

        public List<WorkflowStepDefinition> Steps { get; set; }
    }
}