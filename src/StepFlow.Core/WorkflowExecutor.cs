using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core
{
    internal class WorkflowExecutor : IWorkflowExecutor
    {
        public WorkflowExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartWorkflow(IWorkflow workflow)
        {
            WorkflowBuilder builder = new();
            workflow.Build(builder);

            List<Type> stepTypes = builder.GetSteps();
            List<IStep> steps = GetSteps(stepTypes);

            foreach (IStep step in steps)
            {
                await ExecuteStep(step);
            }
        }

        private List<IStep> GetSteps(List<Type> stepTypes)
        {
            List<IStep> steps = new();
            foreach (Type stepType in stepTypes)
            {
                IStep? step = _serviceProvider.GetService(stepType) as IStep;
                if (step is null)
                {
                    throw new StepFlowException($"Step type '{stepType} not registered.");
                }

                steps.Add(step);
            }

            return steps;
        }

        private async Task ExecuteStep(IStep step)
        {
            try
            {
                await step.ExecuteAsync();
            }
            catch (Exception exception)
            {
                throw new StepFlowException($"Step '{step.GetType()}' failed.", exception);
            }
        }

        private readonly IServiceProvider _serviceProvider;
    }
}