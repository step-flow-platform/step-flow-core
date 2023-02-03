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
            StepPropertyMapper<TStep, TData> propertyMapper = new();
            WorkflowStepBuilder stepBuilder = new WorkflowStepBuilder(typeof(TStep), propertyMapper);
            _stepBuilders.Add(stepBuilder);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<IStepPropertyMapper<TStep, TData>> mapperAction)
            where TStep : IStep
        {
            StepPropertyMapper<TStep, TData> propertyMapper = new();
            mapperAction(propertyMapper);

            WorkflowStepBuilder stepBuilder = new WorkflowStepBuilder(typeof(TStep), propertyMapper);
            _stepBuilders.Add(stepBuilder);
            return this;
        }

        public WorkflowDefinition Build()
        {
            WorkflowDefinition definition = new(typeof(TData))
            {
                Steps = _stepBuilders.Select(stepBuilder => stepBuilder.Build()).ToList()
            };

            return definition;
        }

        private readonly List<WorkflowStepBuilder> _stepBuilders = new();
    }
}