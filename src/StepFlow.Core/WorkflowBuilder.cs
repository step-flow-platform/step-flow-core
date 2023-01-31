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
            WorkflowStep<TStep, TData> workflowStep = new((_, _) => { }, (_, _) => { });
            _steps.Add(workflowStep);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<TStep> stepSetup)
            where TStep : IStep
        {
            WorkflowStep<TStep, TData> workflowStep = new((step, _) => stepSetup(step), (_, _) => { });
            _steps.Add(workflowStep);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<TStep, TData> stepSetup)
            where TStep : IStep
        {
            WorkflowStep<TStep, TData> workflowStep = new(stepSetup, (_, _) => { });
            _steps.Add(workflowStep);
            return this;
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<TStep, TData> stepSetup, Action<TData, TStep> stepResult)
            where TStep : IStep
        {
            WorkflowStep<TStep, TData> workflowStep = new(stepSetup, stepResult);
            _steps.Add(workflowStep);
            return this;
        }

        public List<IWorkflowStep> GetSteps()
        {
            return _steps;
        }

        private readonly List<IWorkflowStep> _steps = new();
    }
}