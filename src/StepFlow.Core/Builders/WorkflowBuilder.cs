using System;
using System.Linq.Expressions;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Core.Builders
{
    internal class WorkflowBuilder<TData> : IWorkflowBuilder<TData>
        where TData : new()
    {
        public IWorkflowBuilder<TData> Step<TStep>()
            where TStep : IStep
        {
            return _mainBranchBuilder.Step<TStep>();
        }

        public IWorkflowBuilder<TData> Step<TStep>(Action<IStepBuilder<TStep, TData>> stepBuilderAction)
            where TStep : IStep
        {
            return _mainBranchBuilder.Step(stepBuilderAction);
        }

        public IWorkflowBuilder<TData> If(Expression<Func<TData, bool>> condition,
            Action<IWorkflowBuilder<TData>> ifBuilderAction)
        {
            return _mainBranchBuilder.If(condition, ifBuilderAction);
        }

        public IWorkflowBuilder<TData> GoTo(string stepId)
        {
            return _mainBranchBuilder.GoTo(stepId);
        }

        public WorkflowDefinition BuildDefinition()
        {
            WorkflowDefinition definition = new(typeof(TData), _mainBranchBuilder.Build());
            return definition;
        }

        private readonly WorkflowBranchBuilder<TData> _mainBranchBuilder = new();
    }
}