using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.UseCases;

[TestClass]
public class LinearWorkflowTest : WorkflowTestBase
{
    [TestMethod]
    public void ExecuteLinearWorkflow()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        LinearWorkflowData workflowData = new();
        LinearWorkflow workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(4, workflowData.Value);
    }

    private class LinearWorkflowData
    {
        public int Value { get; set; } = default;
    }

    private class LinearWorkflow : IWorkflow<LinearWorkflowData>
    {
        public void Build(IWorkflowBuilder<LinearWorkflowData> builder)
        {
            builder
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, _ => 1)
                    .Output(data => data.Value, step => step.IncrementedValue))
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, data => data.Value)
                    .Output(data => data.Value, step => step.IncrementedValue))
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, data => data.Value)
                    .Output(data => data.Value, step => step.IncrementedValue));
        }
    }
}