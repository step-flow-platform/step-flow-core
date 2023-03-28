using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.UseCases;

[TestClass]
public class PublishWorkflowEventTest : WorkflowTestBase
{
    [TestMethod]
    public async Task RunWorkflowWithWaitEvent()
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowHost host = serviceProvider.GetService<IWorkflowHost>()!;

        host.RegisterWorkflow<Workflow, WorkflowData>();
        WorkflowData data = new();
        host.RunWorkflow("Workflow", data);

        await Task.Delay(300);
        Assert.AreEqual(1, data.Value);

        host.PublishEvent("SomeEvent");
        await Task.Delay(300);

        Assert.AreEqual(2, data.Value);
    }

    private class WorkflowData
    {
        public int Value { get; set; } = default;
    }

    private class Workflow : IWorkflow<WorkflowData>
    {
        public string Name => "Workflow";

        public void Build(IWorkflowBuilder<WorkflowData> builder)
        {
            builder
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, _ => 0)
                    .Output(data => data.Value, step => step.IncrementedValue))
                .WaitForEvent("SomeEvent")
                .Step<IncrementStep>(x => x
                    .Input(step => step.Value, data => data.Value)
                    .Output(data => data.Value, step => step.IncrementedValue));
        }
    }
}