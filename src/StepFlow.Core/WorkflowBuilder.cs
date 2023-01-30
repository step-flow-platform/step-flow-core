using System;
using System.Collections.Generic;
using StepFlow.Contracts;

namespace StepFlow.Core
{
    internal class WorkflowBuilder : IWorkflowBuilder
    {
        public IWorkflowBuilder Step<T>()
            where T : IStep
        {
            _steps.Add(typeof(T));
            return this;
        }

        public List<Type> GetSteps()
        {
            return _steps;
        }

        private readonly List<Type> _steps = new();
    }
}