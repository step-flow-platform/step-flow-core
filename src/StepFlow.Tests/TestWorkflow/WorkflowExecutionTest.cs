using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;
using StepFlow.Core;
using StepFlow.Dsl;

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

    [TestMethod]
    public async Task ExecuteJsonWorkflow()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;
        IWorkflowDefinitionLoader definitionLoader = serviceProvider.GetService<IWorkflowDefinitionLoader>()!;

        string json = await File.ReadAllTextAsync("TestWorkflow/json-workflow.json");
        WorkflowDefinition definition = definitionLoader.Load(json, Deserializers.Json);
        await workflowExecutor.StartWorkflow(definition);
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStepFlow();
        services.AddStepFlowDsl(new WorkflowDefinitionLoaderOptions
        {
            AssemblyName = "StepFlow.Tests", DataNamespace = "StepFlow.Tests.TestWorkflow",
            StepsNamespace = "StepFlow.Tests.TestWorkflow"
        });
        services.AddTransient<FirstStep>();
        services.AddTransient<SecondStep>();
        services.AddTransient<PrintStep>();
        services.AddTransient<SumStep>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}