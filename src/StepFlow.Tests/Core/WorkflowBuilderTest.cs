using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Builders;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowBuilderTest
{
    [TestMethod]
    public void BuildLinearDefinition()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .Step<Step1>(x => x
                .Id("a1"))
            .Step<Step2>(x => x
                .Id("a2")
                .Input(step => step.Property1, _ => 5))
            .Step<Step3>(x => x
                .Input(step => step.Property1, data => data.A)
                .Input(step => step.Property2, _ => "val2")
                .Output(data => data.B, _ => 10));

        WorkflowDefinition definition = builder.BuildDefinition();

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(3, definition.Nodes.Count);
        WorkflowAssert.StepDefinition(definition.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false, "a1");
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 1, false, "a2");
        WorkflowAssert.StepDefinition(definition.Nodes[2], "StepFlow.Tests.TestSteps.Step3", 2, true);
    }

    [TestMethod]
    public void BuildDefinitionWithIfScenario1()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .Step<Step1>()
            .If("if1", data => data.A > 10, _ => _
                .Step<Step2>()
                .Step<Step2>())
            .Step<Step3>();

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[1];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(3, definition.Nodes.Count);
        Assert.AreEqual(2, if1Node.Nodes.Count);

        WorkflowAssert.StepDefinition(definition.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.IfDefinition(definition.Nodes[1], "(data.A > 10)", "if1");
        WorkflowAssert.StepDefinition(if1Node.Nodes[0], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.StepDefinition(if1Node.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[2], "StepFlow.Tests.TestSteps.Step3", 0, false);
    }

    [TestMethod]
    public void BuildDefinitionWithIfScenario2()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .If(data => data.A > 10, _ => _
                .Step<Step1>())
            .If(data => data.B == 5, _ => _
                .Step<Step2>())
            .Step<Step3>();

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[0];
        WorkflowIfDefinition if2Node = (WorkflowIfDefinition)definition.Nodes[1];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(3, definition.Nodes.Count);
        Assert.AreEqual(1, if1Node.Nodes.Count);
        Assert.AreEqual(1, if2Node.Nodes.Count);

        WorkflowAssert.IfDefinition(definition.Nodes[0], "(data.A > 10)");
        WorkflowAssert.StepDefinition(if1Node.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.IfDefinition(definition.Nodes[1], "(data.B == 5)");
        WorkflowAssert.StepDefinition(if2Node.Nodes[0], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[2], "StepFlow.Tests.TestSteps.Step3", 0, false);
    }

    [TestMethod]
    public void BuildDefinitionWithIfScenario3()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .If(data => data.A > 10, if1 => if1
                .If(data => data.B < 5, if2 => if2
                    .If(data => true, if3 => if3
                        .Step<Step1>())))
            .Step<Step2>();

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[0];
        WorkflowIfDefinition if2Node = (WorkflowIfDefinition)if1Node.Nodes[0];
        WorkflowIfDefinition if3Node = (WorkflowIfDefinition)if2Node.Nodes[0];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(2, definition.Nodes.Count);
        Assert.AreEqual(1, if1Node.Nodes.Count);
        Assert.AreEqual(1, if2Node.Nodes.Count);
        Assert.AreEqual(1, if3Node.Nodes.Count);

        WorkflowAssert.IfDefinition(if1Node, "(data.A > 10)");
        WorkflowAssert.IfDefinition(if2Node, "(data.B < 5)");
        WorkflowAssert.IfDefinition(if3Node, "True");
        WorkflowAssert.StepDefinition(if3Node.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 0, false);
    }

    [TestMethod]
    public void BuildDefinitionWithIfScenario4()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .If(data => data.A > 10, if1 => if1
                .If(data => data.B < 5, if2 => if2
                    .Step<Step1>())
                .If(data => true, if3 => if3
                    .Step<Step2>()))
            .Step<Step3>();

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[0];
        WorkflowIfDefinition if2Node = (WorkflowIfDefinition)if1Node.Nodes[0];
        WorkflowIfDefinition if3Node = (WorkflowIfDefinition)if1Node.Nodes[1];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(2, definition.Nodes.Count);
        Assert.AreEqual(2, if1Node.Nodes.Count);
        Assert.AreEqual(1, if2Node.Nodes.Count);
        Assert.AreEqual(1, if3Node.Nodes.Count);

        WorkflowAssert.IfDefinition(if1Node, "(data.A > 10)");
        WorkflowAssert.IfDefinition(if2Node, "(data.B < 5)");
        WorkflowAssert.StepDefinition(if2Node.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.IfDefinition(if3Node, "True");
        WorkflowAssert.StepDefinition(if3Node.Nodes[0], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step3", 0, false);
    }

    [TestMethod]
    public void BuildDefinitionWitGoToScenario1()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .Step<Step1>(x => x
                .Id("a1"))
            .Step<Step2>()
            .Step<Step3>()
            .GoTo("a1");

        WorkflowDefinition definition = builder.BuildDefinition();

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(4, definition.Nodes.Count);
        WorkflowAssert.StepDefinition(definition.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false, "a1");
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[2], "StepFlow.Tests.TestSteps.Step3", 0, false);
        WorkflowAssert.GoToDefinition(definition.Nodes[3], "a1");
    }

    [TestMethod]
    public void BuildDefinitionWitGoToScenario2()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .If("if1", data => data.A > 10, _ => _
                .Step<Step1>()
                .Step<Step2>()
                .GoTo("if1"))
            .Step<Step3>();

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[0];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(2, definition.Nodes.Count);
        Assert.AreEqual(3, if1Node.Nodes.Count);

        WorkflowAssert.IfDefinition(if1Node, "(data.A > 10)", "if1");
        WorkflowAssert.StepDefinition(if1Node.Nodes[0], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.StepDefinition(if1Node.Nodes[1], "StepFlow.Tests.TestSteps.Step2", 0, false);
        WorkflowAssert.GoToDefinition(if1Node.Nodes[2], "if1");
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step3", 0, false);
    }

    [TestMethod]
    public void BuildDefinitionWitGoToScenario3()
    {
        WorkflowBuilder<WorkflowData> builder = new();
        builder
            .If(data => data.A > 10, if1 => if1
                .If(data => data.B < 5, if2 => if2
                    .If(data => true, if3 => if3
                        .GoTo("a2"))))
            .Step<Step1>()
            .Step<Step1>(x => x
                .Id("a2"));

        WorkflowDefinition definition = builder.BuildDefinition();
        WorkflowIfDefinition if1Node = (WorkflowIfDefinition)definition.Nodes[0];
        WorkflowIfDefinition if2Node = (WorkflowIfDefinition)if1Node.Nodes[0];
        WorkflowIfDefinition if3Node = (WorkflowIfDefinition)if2Node.Nodes[0];

        Assert.AreEqual("StepFlow.Tests.Core.WorkflowBuilderTest+WorkflowData", definition.DataType.FullName);
        Assert.AreEqual(3, definition.Nodes.Count);
        Assert.AreEqual(1, if1Node.Nodes.Count);
        Assert.AreEqual(1, if2Node.Nodes.Count);
        Assert.AreEqual(1, if3Node.Nodes.Count);

        WorkflowAssert.IfDefinition(if1Node, "(data.A > 10)");
        WorkflowAssert.IfDefinition(if2Node, "(data.B < 5)");
        WorkflowAssert.IfDefinition(if3Node, "True");
        WorkflowAssert.GoToDefinition(if3Node.Nodes[0], "a2");
        WorkflowAssert.StepDefinition(definition.Nodes[1], "StepFlow.Tests.TestSteps.Step1", 0, false);
        WorkflowAssert.StepDefinition(definition.Nodes[2], "StepFlow.Tests.TestSteps.Step1", 0, false, "a2");
    }

    private class WorkflowData
    {
        public int A { get; set; } = default!;

        public int B { get; set; } = default!;
    }
}