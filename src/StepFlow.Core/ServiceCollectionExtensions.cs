using Microsoft.Extensions.DependencyInjection;
using StepFlow.Contracts;

namespace StepFlow.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStepFlow(this IServiceCollection services)
    {
        services.AddTransient<IfStep>();
        services.AddSingleton<IWorkflowExecutor, WorkflowExecutor>();
        return services;
    }
}