using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts;
using StepFlow.Tests.TestWorkflowTypes.Steps;

namespace StepFlow.Tests.UseCases;

[TestClass]
public class IfWorkflowTest : WorkflowTestBase
{
    [DataTestMethod]
    [DataRow(1, 1, "abg")]
    [DataRow(6, 1, "abcdg")]
    [DataRow(6, 11, "abcdefg")]
    [DataRow(1, 11, "abg")]
    public void ExecuteIfWorkflowScenario1(int value1, int value2, string expectedResult)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IfWorkflowData workflowData = new()
        {
            Value1 = value1,
            Value2 = value2,
            Result = string.Empty
        };

        IfWorkflow workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(expectedResult, workflowData.Result);
    }

    [DataTestMethod]
    [DataRow(1, 1, "d")]
    [DataRow(6, 1, "acd")]
    [DataRow(6, 11, "abcd")]
    [DataRow(1, 11, "d")]
    public void ExecuteIfWorkflowScenario2(int value1, int value2, string expectedResult)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IfWorkflowData workflowData = new()
        {
            Value1 = value1,
            Value2 = value2,
            Result = string.Empty
        };

        IfWorkflow2 workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(expectedResult, workflowData.Result);
    }

    [DataTestMethod]
    [DataRow(1, 1, "")]
    [DataRow(6, 1, "a")]
    [DataRow(6, 11, "ab")]
    [DataRow(1, 11, "")]
    public void ExecuteIfWorkflowScenario3(int value1, int value2, string expectedResult)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        IWorkflowExecutor workflowExecutor = serviceProvider.GetService<IWorkflowExecutor>()!;

        IfWorkflowData workflowData = new()
        {
            Value1 = value1,
            Value2 = value2,
            Result = string.Empty
        };

        IfWorkflow3 workflow = new();
        workflowExecutor.StartWorkflow(workflow, workflowData);

        Assert.AreEqual(expectedResult, workflowData.Result);
    }

    private class IfWorkflowData
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public string Result { get; set; } = default!;
    }

    private class IfWorkflow : IWorkflow<IfWorkflowData>
    {
        public void Build(IWorkflowBuilder<IfWorkflowData> builder)
        {
            builder
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "a")
                    .Output(data => data.Result, step => step.Result))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "b")
                    .Output(data => data.Result, step => step.Result))
                .If(data => data.Value1 > 5, _ => _
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "c")
                        .Output(data => data.Result, step => step.Result))
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "d")
                        .Output(data => data.Result, step => step.Result))
                    .If(data => data.Value2 > 10, __ => __
                        .Step<ConcatenateStringsStep>(x => x
                            .Input(step => step.First, data => data.Result)
                            .Input(step => step.Second, _ => "e")
                            .Output(data => data.Result, step => step.Result))
                        .Step<ConcatenateStringsStep>(x => x
                            .Input(step => step.First, data => data.Result)
                            .Input(step => step.Second, _ => "f")
                            .Output(data => data.Result, step => step.Result))))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "g")
                    .Output(data => data.Result, step => step.Result));
        }
    }

    private class IfWorkflow2 : IWorkflow<IfWorkflowData>
    {
        public void Build(IWorkflowBuilder<IfWorkflowData> builder)
        {
            builder
                .If(data => data.Value1 > 5, _ => _
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "a")
                        .Output(data => data.Result, step => step.Result))
                    .If(data => data.Value2 > 10, __ => __
                        .Step<ConcatenateStringsStep>(x => x
                            .Input(step => step.First, data => data.Result)
                            .Input(step => step.Second, _ => "b")
                            .Output(data => data.Result, step => step.Result)))
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "c")
                        .Output(data => data.Result, step => step.Result)))
                .Step<ConcatenateStringsStep>(x => x
                    .Input(step => step.First, data => data.Result)
                    .Input(step => step.Second, _ => "d")
                    .Output(data => data.Result, step => step.Result));
        }
    }

    private class IfWorkflow3 : IWorkflow<IfWorkflowData>
    {
        public void Build(IWorkflowBuilder<IfWorkflowData> builder)
        {
            builder
                .If(data => data.Value1 > 5, _ => _
                    .Step<ConcatenateStringsStep>(x => x
                        .Input(step => step.First, data => data.Result)
                        .Input(step => step.Second, _ => "a")
                        .Output(data => data.Result, step => step.Result))
                    .If(data => data.Value2 > 10, __ => __
                        .Step<ConcatenateStringsStep>(x => x
                            .Input(step => step.First, data => data.Result)
                            .Input(step => step.Second, _ => "b")
                            .Output(data => data.Result, step => step.Result))));
        }
    }
}