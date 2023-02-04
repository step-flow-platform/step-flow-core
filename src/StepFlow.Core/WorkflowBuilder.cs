using System;
using System.Collections.Generic;
using System.Linq;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core
{
    internal class WorkflowBuilder<TData> : IWorkflowBuilder<TData>
        where TData : new()
    {
        public IWorkflowBuilder<TData> Step<TStep>()
            where TStep : IStep
        {
            StepPropertyBuilder<TStep, TData> propertyBuilder = new();
            _steps.Add(propertyBuilder);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<IStepPropertyBuilder<TStep, TData>> mapperAction)
            where TStep : IStep
        {
            StepPropertyBuilder<TStep, TData> propertyBuilder = new();
            mapperAction(propertyBuilder);

            _steps.Add(propertyBuilder);
            return this;
        }

        public WorkflowDefinition Build()
        {
            WorkflowDefinition definition = new(typeof(TData))
            {
                Steps = _steps.Select(stepBuilder => stepBuilder.Build()).ToList()
            };

            return definition;
        }

        private readonly List<IStepPropertyBuilder> _steps = new();
    }
}