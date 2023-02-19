using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.UseCases;

[TestClass]
public class GoToWorkflowTest : WorkflowTestBase
{
    [DataTestMethod]
    [DataRow(1, 1, "acdfg")]
    [DataRow(6, 1, "abg")]
    [DataRow(1, 31, "acdeg")]
    public void ExecuteGotoWorkflowScenario1(int value1, int value2, string expectedResult)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        GoToWorkflowData workflowData = new()
        {
            Value1 = value1,
            Value2 = value2,
            Result = string.Empty
        };

        GoToWorkflow workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(expectedResult, workflowData.Result);
    }

    [DataTestMethod]
    [DataRow(1, 2, "a+b")]
    [DataRow(1, 5, "a++++b")]
    [DataRow(3, 10, "a+++++++b")]
    [DataRow(2, 2, "ab")]
    public void ExecuteGotoWorkflowScenario2(int value1, int value2, string expectedResult)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        GoToWorkflowData workflowData = new()
        {
            Value1 = value1,
            Value2 = value2,
            Result = string.Empty
        };

        GoToWorkflow2 workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(expectedResult, workflowData.Result);
    }

    private class GoToWorkflowData
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public string Result { get; set; } = default!;
    }

    private class GoToWorkflow : IWorkflow<GoToWorkflowData>
    {
        public void Build(IWorkflowBuilder<GoToWorkflowData> builder)
        {
            builder
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "a")
                    .Output(data => data.Result, step => step.Result))
                .If(data => data.Value1 > 5, _ => _
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "b")
                        .Output(data => data.Result, step => step.Result))
                    .GoTo("LastStep"))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "c")
                    .Output(data => data.Result, step => step.Result))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "d")
                    .Output(data => data.Result, step => step.Result))
                .If(data => data.Value2 > 10, _ => _
                    .If(data => data.Value2 > 20, __ => __
                        .Step<ConcatenateStringsStep>(x => x
                            .Input(step => step.First, data => data.Result)
                            .Input(step => step.Second, _ => "e")
                            .Output(data => data.Result, step => step.Result))
                        .If(data => data.Value2 > 30, a => a
                            .GoTo("LastStep"))))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "f")
                    .Output(data => data.Result, step => step.Result))
                .Step<ConcatenateStringsStep>(x => x
                    .Id("LastStep")
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "g")
                    .Output(data => data.Result, step => step.Result));
        }
    }

    private class GoToWorkflow2 : IWorkflow<GoToWorkflowData>
    {
        public void Build(IWorkflowBuilder<GoToWorkflowData> builder)
        {
            builder
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "a")
                    .Output(data => data.Result, step => step.Result))
                .If("loop", data => data.Value1 < data.Value2, _ => _
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "+")
                        .Output(data => data.Result, step => step.Result))
                    .Step<IncrementStep>(x => x
                        .Input(step => step.Value, data => data.Value1)
                        .Output(data => data.Value1, step => step.IncrementedValue))
                    .GoTo("loop"))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "b")
                    .Output(data => data.Result, step => step.Result));
        }
    }
}