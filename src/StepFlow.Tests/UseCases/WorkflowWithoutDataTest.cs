using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.UseCases;

[TestClass]
public class WorkflowWithoutDataTest : WorkflowTestBase
{
    [TestMethod]
    public void ExecuteLinearWorkflow()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        WorkflowWithoutData workflowWithoutData = new();
        workflowExecutor.StartWorkflow(workflowWithoutData);
    }

    private class WorkflowWithoutData : IWorkflow
    {
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, _ => 1))
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, _ => 2))
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, _ => 3));
        }
    }
}