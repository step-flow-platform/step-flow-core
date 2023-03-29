using Microsoft.Extensions.DependencyInjection;
using StepFlow.Contracts;

namespace StepFlow.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStepFlow(this IServiceCollection services)
    {
        services.AddSingleton<IWorkflowExecutor, WorkflowExecutor>();
        services.AddSingleton<IWorkflowHost, WorkflowHost>();
        services.AddSingleton<WorkflowEventsDispatcher>();
        services.AddTransient<WaitEventStep>();
        return services;
    }
}