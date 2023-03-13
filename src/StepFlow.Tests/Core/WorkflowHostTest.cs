using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Core;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowHostTest
{
    [TestMethod]
    [Timeout(1000)]
    public async Task RunWorkflow()
    {
        IWorkflowHost workflowHost = Configure();

        bool completed = false;
        StringBuilder result = new();
        workflowHost.RegisterWorkflow<Workflow, WorkflowData>();
        WorkflowData data = new();
        string runningId = workflowHost.RunWorkflow("Workflow", data);
        workflowHost.WorkflowCompleted += (_, id) =>
        {
            if (id == runningId)
            {
                result.Append('a');
                completed = true;
            }
        };

        await Task.Delay(100);
        result.Append('b');

        while (!completed)
        {
            await Task.Delay(100);
        }

        Assert.AreEqual("ba", result.ToString());
        Assert.AreEqual("abx", data.Value);
    }

    private IWorkflowHost Configure()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStepFlow();
        services.AddTransient<DelayStep>();
        services.AddTransient<ConcatenateStringsStep>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<IWorkflowHost>()!;
    }

    private class WorkflowData
    {
        public string Value { get; set; } = default!;
    }

    private class Workflow : IWorkflow<WorkflowData>
    {
        public string Name => "Workflow";

        public void Build(IWorkflowBuilder<WorkflowData> builder)
        {
            builder
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, _ => "a")
                    .Input(step => step.Second, _ => "b")
                    .Output(data => data.Value, step => step.Result))
                .Step<DelayStep>(x => x
                    .Input(step => step.Milliseconds, _ => 300))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Value)
                    .Input(step => step.Second, _ => "x")
                    .Output(data => data.Value, step => step.Result));
        }
    }
}