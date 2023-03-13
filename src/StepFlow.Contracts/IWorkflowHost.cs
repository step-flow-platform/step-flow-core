using System;

namespace StepFlow.Contracts;

public interface IWorkflowHost
{
    public event EventHandler<string> WorkflowCompleted;

    public void RegisterWorkflow<TWorkflow>()
        where TWorkflow : IWorkflow;

    public void RegisterWorkflow<TWorkflow, TData>()
        where TData : new()
        where TWorkflow : IWorkflow<TData>;

    public string RunWorkflow(string name, object? data = null);
}