using System;
using System.Collections.Generic;
using StepFlow.Contracts;

namespace StepFlow.Core
{
    internal class WorkflowBuilder<TData> : IWorkflowBuilder<TData>
        where TData : new()
    {
        public IWorkflowBuilder<TData> Step<TStep>()
            where TStep : IStep
        {
            StepPropertyMapper<TStep, TData> propertyMapper = new();
            WorkflowStep<TStep, TData> step = new(propertyMapper);
            _steps.Add(step);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<IStepPropertyMapper<TStep, TData>> mapperAction)
            where TStep : IStep
        {
            StepPropertyMapper<TStep, TData> propertyMapper = new();
            mapperAction(propertyMapper);

            WorkflowStep<TStep, TData> step = new(propertyMapper);
            _steps.Add(step);
            return this;
        }

        public List<IWorkflowStep> GetSteps()
        {
            return _steps;
        }

        private readonly List<IWorkflowStep> _steps = new();
    }
}