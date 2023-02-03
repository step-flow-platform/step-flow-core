using System;
using System.Threading.Tasks;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

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
            WorkflowDefinition definition = builder.Build();
            await StartWorkflow(definition);
        }

        public async Task StartWorkflow(WorkflowDefinition definition)
        {
            object data = Activator.CreateInstance(definition.DataType);
            foreach (WorkflowStepDefinition stepDefinition in definition.Steps)
            {
                IStep step = ConstructStep(stepDefinition.StepType);
                ProcessStepInput(stepDefinition, step, data);
                await ExecuteStep(step);
                ProcessStepOutput(stepDefinition, step, data);
            }
        }

        private IStep ConstructStep(Type stepType)
        {
            object? stepService = _serviceProvider.GetService(stepType);
            if (stepService is null)
            {
                throw new StepFlowException($"Step type '{stepType} is not registered");
            }

            if (stepService is not IStep step)
            {
                throw new StepFlowException($"Step type '{stepType} is not IStep");
            }

            return step;
        }

        private void ProcessStepInput(WorkflowStepDefinition stepDefinition, IStep step, object data)
        {
            try
            {
                foreach (PropertyMap propertyMap in stepDefinition.Input)
                {
                    PropertyAssigner assigner = new(propertyMap.Source, propertyMap.Target);
                    assigner.Assign(data, step);
                }
            }
            catch (Exception exception)
            {
                throw new StepFlowException($"Failed to process input for step '{step.GetType()}'", exception);
            }
        }

        private async Task ExecuteStep(IStep step)
        {
            try
            {
                await step.ExecuteAsync();
            }
            catch (Exception exception)
            {
                throw new StepFlowException($"Failed to execute step '{step.GetType()}'", exception);
            }
        }

        private void ProcessStepOutput(WorkflowStepDefinition stepDefinition, IStep step, object data)
        {
            try
            {
                if (stepDefinition.Output is null)
                {
                    return;
                }

                PropertyAssigner assigner = new(stepDefinition.Output.Source, stepDefinition.Output.Target);
                assigner.Assign(step, data);
            }
            catch (Exception exception)
            {
                throw new StepFlowException($"Failed to process output for step '{step.GetType()}'", exception);
            }
        }

        private readonly IServiceProvider _serviceProvider;
    }
}