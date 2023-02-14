using System;
using Microsoft.Extensions.DependencyInjection;
using StepFlow.Core;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.UseCases;

public abstract class WorkflowTestBase
{
    protected static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStepFlow();
        services.AddTransient<IncrementStep>();
        services.AddTransient<ConcatenateStringsStep>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}