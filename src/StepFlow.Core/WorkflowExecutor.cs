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

        public async Task StartWorkflow<TData>(IWorkflow<TData> workflow)
            where TData : new()
        {
            WorkflowBuilder<TData> builder = new();
            workflow.Build(builder);

            List<IWorkflowStep> steps = builder.GetSteps();
            TData data = new();
            foreach (IWorkflowStep step in steps)
            {
                await step.Execute(_serviceProvider, data);
            }
        }

        private readonly IServiceProvider _serviceProvider;
    }
}