using System;
using System.Linq;
using System.Threading.Tasks;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Builders;
using StepFlow.Core.Graph;

namespace StepFlow.Core;

internal class WorkflowExecutor : IWorkflowExecutor
{
    public WorkflowExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Start(WorkflowDefinition definition, object? data = null)
    {
        WorkflowGraph graph = new WorkflowGraph(definition);
        await Process(graph, data);
    }

    public async Task Execute<TData>(IWorkflow<TData> workflow, TData? data = default)
        where TData : new()
    {
        WorkflowBuilder<TData> builder = new();
        workflow.Build(builder);
        WorkflowDefinition definition = builder.BuildDefinition(workflow.Name);
        WorkflowGraph graph = new WorkflowGraph(definition);
        await Process(graph, data);
    }

    private async Task Process(WorkflowGraph graph, object? data = null)
    {
        data ??= Activator.CreateInstance(graph.WorkflowDataType);
        string? nextNodeId = null;
        do
        {
            WorkflowNode node = graph.Node(nextNodeId);
            nextNodeId = node.NodeType switch
            {
                WorkflowNodeType.Step => await ProcessStep(node, data),
                WorkflowNodeType.If => ProcessIf(node, data),
                WorkflowNodeType.GoTo => node.Directions[0],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        while (nextNodeId is not null);
    }

    private async Task<string?> ProcessStep(WorkflowNode node, object data)
    {
        WorkflowStepDefinition definition = (WorkflowStepDefinition)node.Definition;
        IStep step = ConstructStep(definition.StepType);

        try
        {
            ProcessStepInput(definition, step, data);
            await step.ExecuteAsync();
            ProcessStepOutput(definition, step, data);
            return node.Directions.FirstOrDefault();
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Failed to execute step '{step.GetType()}'", exception);
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
        foreach (PropertyMap propertyMap in stepDefinition.Input)
        {
            PropertyAssigner assigner = new(propertyMap.Source, propertyMap.Target);
            assigner.Assign(data, step);
        }
    }

    private void ProcessStepOutput(WorkflowStepDefinition stepDefinition, IStep step, object data)
    {
        if (stepDefinition.Output is null)
        {
            return;
        }

        PropertyAssigner assigner = new(stepDefinition.Output.Source, stepDefinition.Output.Target);
        assigner.Assign(step, data);
    }

    private string? ProcessIf(WorkflowNode node, object data)
    {
        WorkflowIfDefinition definition = (WorkflowIfDefinition)node.Definition;
        object? conditionResult = definition.Condition.Compile().DynamicInvoke(data);
        return conditionResult switch
        {
            true => node.Directions.First(),
            false => node.Directions.Count > 1 ? node.Directions[1] : null,
            _ => throw new StepFlowException("Unexpected IF condition result")
        };
    }

    private readonly IServiceProvider _serviceProvider;
}