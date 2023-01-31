using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class WorkflowStep<TStep, TData> : IWorkflowStep
    where TStep : IStep
{
    public WorkflowStep(StepPropertyMapper<TStep, TData> propertyMapper)
    {
        _propertyMapper = propertyMapper;
    }

    public async Task Execute(IServiceProvider serviceProvider, object data)
    {
        TStep step = ConstructStep(serviceProvider);
        ProcessStepInput(step, data);
        await ExecuteStep(step);
        ProcessStepOutput(step, data);
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

    private void ProcessStepInput(TStep step, object data)
    {
        try
        {
            _propertyMapper.MapInputs(step, data);
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Setup step '{step.GetType()}' action failed.", exception);
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

    private void ProcessStepOutput(TStep step, object data)
    {
        try
        {
            _propertyMapper.MapOutput(step, data);
        }
        catch (Exception exception)
        {
            throw new StepFlowException($"Result step '{step.GetType()}' action failed.", exception);
        }
    }

    private readonly StepPropertyMapper<TStep, TData> _propertyMapper;
}