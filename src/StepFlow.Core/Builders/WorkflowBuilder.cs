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

        public WorkflowDefinition Build()
        {
            WorkflowDefinition definition = new(typeof(TData), _mainBranchBuilder.Build());
            return definition;
        }

        private WorkflowBranchBuilder<TData> _mainBranchBuilder = new();
    }
}