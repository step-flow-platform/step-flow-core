using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Core;
using StepFlow.Tests.TestWorkflowTypes;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowExecutorTest
{
    [TestMethod]
    public async Task WorkflowScenario1()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IncrementWorkflowData data = new()
        {
            FirstCheckValue = 100
        };
        await workflowExecutor.StartWorkflow(new IncrementWorkflow(), data);

        Assert.AreEqual(0, data.StartValue);
        Assert.AreEqual(3, data.ResultValue);
    }

    [TestMethod]
    public async Task WorkflowScenario2()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IncrementWorkflowData data = new()
        {
            StartValue = 5,
            FirstCheckValue = 7,
            SecondCheckValue = 100
        };
        await workflowExecutor.StartWorkflow(new IncrementWorkflow(), data);

        Assert.AreEqual(5, data.StartValue);
        Assert.AreEqual(10, data.ResultValue);
    }

    [TestMethod]
    public async Task WorkflowScenario3()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IncrementWorkflowData data = new()
        {
            StartValue = 12,
            FirstCheckValue = 12,
            SecondCheckValue = 16
        };
        await workflowExecutor.StartWorkflow(new IncrementWorkflow(), data);

        Assert.AreEqual(12, data.StartValue);
        Assert.AreEqual(18, data.ResultValue);
    }

    [TestMethod]
    public async Task GotoWorkflowScenario1()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        GotoWorkflow.GotoWorkflowData data = new();
        await workflowExecutor.StartWorkflow(new GotoWorkflow(), data);

        Assert.AreEqual(3, data.Value);
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStepFlow();
        services.AddTransient<IncrementStep>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}