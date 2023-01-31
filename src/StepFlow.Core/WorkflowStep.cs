using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class WorkflowStep<TStep, TData> : IWorkflowStep
    where TStep : IStep
{
    public WorkflowStep(Action<TStep, TData> setupAction)
    {
        _setupAction = setupAction;
    }

    public async Task Execute(IServiceProvider serviceProvider, object data)
    {
        TStep step = ConstructStep(serviceProvider);
        SetupStep(step, data);
        await ExecuteStep(step);
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
            throw new StepFlowException($"Setup step '{step.GetType()}' failed.", exception);
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
}