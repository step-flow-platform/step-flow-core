using System;

namespace StepFlow.Contracts;

public interface IWorkflowHost
{
    event EventHandler<string> WorkflowCompleted;

    event EventHandler<WorkflowEvent> EventPublished;

    void RegisterWorkflow<TWorkflow>()
        where TWorkflow : IWorkflow;

    void RegisterWorkflow<TWorkflow, TData>()
        where TData : new()
        where TWorkflow : IWorkflow<TData>;

    string RunWorkflow(string name, object? data = null);

    void PublishEvent(string eventName, string? eventKey = null, string? eventData = null);
}