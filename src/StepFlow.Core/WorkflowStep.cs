using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class WorkflowStep<TStep, TData> : IWorkflowStep
    where TStep : IStep
{
    public WorkflowStep(Action<TStep, TData> setupAction, Action<TData, TStep> resultAction)
    {
        _setupAction = setupAction;
        _resultAction = resultAction;
    }

    public async Task Execute(IServiceProvider serviceProvider, object data)
    {
        TStep step = ConstructStep(serviceProvider);
        SetupStep(step, data);
        await ExecuteStep(step);
        ResultStep(step, data);
    }

    private TStep ConstructStep(IServiceProvider serviceProvider)
    {
        Type stepType = typeof(TStep);
        if (serviceProvider.GetService(stepType) is not TStep step)
        {
            throw new StepFlowException($"Step type '{stepType} not registered.");
        }

        return step;
    }

    private void SetupStep(TStep step, object data)
    {
        try
        {
            _setupAction(step, (TData)data);
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Setup step '{step.GetType()}' action failed.", exception);
        }
    }

    private void ResultStep(TStep step, object data)
    {
        try
        {
            _resultAction((TData)data, step);
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Result step '{step.GetType()}' action failed.", exception);
        }
    }

    private async Task ExecuteStep(TStep step)
    {
        try
        {
            await step.ExecuteAsync();
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Step execution '{step.GetType()}' failed.", exception);
        }
    }

    private readonly Action<TStep, TData> _setupAction;
    private readonly Action<TData, TStep> _resultAction;
}