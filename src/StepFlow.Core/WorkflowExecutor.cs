using System;
using System.Linq;
using System.Threading.Tasks;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Builders;

namespace StepFlow.Core
{
    internal class WorkflowExecutor : IWorkflowExecutor
    {
        public WorkflowExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartWorkflow<TData>(IWorkflow<TData> workflow, TData? data = default)
            where TData : new()
        {
            WorkflowBuilder<TData> builder = new();
            workflow.Build(builder);
            WorkflowDefinition definition = builder.BuildDefinition();
            await StartWorkflow(definition, data);
        }

        public async Task StartWorkflow(WorkflowDefinition definition, object? data = null)
        {
            data ??= Activator.CreateInstance(definition.DataType);

            string? nextNodeId = definition.Nodes.FirstOrDefault()?.Id;
            while (nextNodeId is not null)
            {
                WorkflowNodeDefinition node = definition.Nodes.Single(x => x.Id == nextNodeId);
                nextNodeId = await ProcessNode(node, data);
            }
        }

        private async Task<string?> ProcessNode(WorkflowNodeDefinition nodeDefinition, object data)
        {
            return nodeDefinition switch
            {
                WorkflowStepDefinition stepDefinition => await ProcessStep(stepDefinition, data),
                WorkflowIfDefinition ifDefinition => ProcessIf(ifDefinition, data),
                WorkflowGoToDefinition goToDefinition => goToDefinition.NextNodeId,
                _ => throw new StepFlowException("Unexpected Node type")
            };
        }

        private async Task<string?> ProcessStep(WorkflowStepDefinition definition, object data)
        {
            IStep step = ConstructStep(definition.StepType);
            ProcessStepInput(definition, step, data);
            await ExecuteStep(step);
            ProcessStepOutput(definition, step, data);
            return definition.NextNodeId;
        }

        private string? ProcessIf(WorkflowIfDefinition definition, object data)
        {
            object? conditionResult = definition.Condition.Compile().DynamicInvoke(data);
            return conditionResult switch
            {
                true => definition.TrueNodeId,
                false => definition.FalseNodeId,
                _ => throw new StepFlowException("Unexpected IF condition result")
            };
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