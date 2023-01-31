using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Core;

namespace StepFlow.Tests.TestWorkflow;

[TestClass]
public class WorkflowExecutionTest
{
    [TestMethod]
    public async Task ExecuteWorkflow()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        await workflowExecutor.StartWorkflow(new SomeWorkflow());
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStepFlow();
        services.AddTransient<FirstStep>();
        services.AddTransient<SecondStep>();
        services.AddTransient<PrintStep>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}