using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Builders;

namespace StepFlow.Core;

internal class WorkflowHost : IWorkflowHost
{
    public WorkflowHost(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _definitions = new Dictionary<string, WorkflowDefinition>();
    }

    public event EventHandler<string>? WorkflowCompleted;

    public void RegisterWorkflow<TWorkflow>()
        where TWorkflow : IWorkflow
    {
        SaveWorkflow<TWorkflow, object>();
    }

    public void RegisterWorkflow<TWorkflow, TData>()
        where TWorkflow : IWorkflow<TData>
        where TData : new()
    {
        SaveWorkflow<TWorkflow, TData>();
    }

    public string RunWorkflow(string name, object? data = null)
    {
        WorkflowDefinition definition = _definitions[name];
        string runningId = Guid.NewGuid().ToString();
        Task.Run(async () =>
        {
            IWorkflowExecutor executor = _serviceProvider.GetService<IWorkflowExecutor>()!;
            await executor.Start(definition, data);
            WorkflowCompleted?.Invoke(this, runningId);
        });

        return runningId;
    }

    private void SaveWorkflow<TWorkflow, TData>()
        where TWorkflow : IWorkflow<TData>
        where TData : new()
    {
        TWorkflow workflow = ActivatorUtilities.CreateInstance<TWorkflow>(_serviceProvider);
        WorkflowBuilder<TData> builder = new();
        workflow.Build(builder);

        WorkflowDefinition definition = builder.BuildDefinition(workflow.Name);
        if (_definitions.ContainsKey(definition.Name))
        {
            throw new StepFlowException($"Workflow '{definition.Name}' is already registered");
        }

        _definitions.Add(definition.Name, definition);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, WorkflowDefinition> _definitions;
}