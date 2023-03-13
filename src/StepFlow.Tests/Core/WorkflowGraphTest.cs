using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Graph;
using StepFlow.Tests.TestSteps;

namespace StepFlow.Tests.Core;

[TestClass]
public class WorkflowGraphTest
{
    [TestMethod]
    public void LinearDefinition()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(), null),
                new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(), null),
                new WorkflowStepDefinition("a3", typeof(Step1), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode a1 = graph.Node();
        WorkflowNode a2 = graph.Node(a1.Directions[0]);
        WorkflowNode a3 = graph.Node(a2.Directions[0]);

        AssertGraphNode(a1, "a1", new List<string> { "a2" });
        AssertGraphNode(a2, "a2", new List<string> { "a3" });
        AssertGraphNode(a3, "a3", new List<string>());
    }

    [TestMethod]
    public void DefinitionWithIfScenario1()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(), null),
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(), null),
                        new WorkflowStepDefinition("a3", typeof(Step1), new List<PropertyMap>(), null)
                    }),
                new WorkflowStepDefinition("a4", typeof(Step1), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode a1 = graph.Node();
        WorkflowNode if1 = graph.Node(a1.Directions[0]);
        WorkflowNode a2 = graph.Node(if1.Directions[0]);
        WorkflowNode a3 = graph.Node(a2.Directions[0]);
        WorkflowNode a4 = graph.Node(a3.Directions[0]);

        AssertGraphNode(if1, "if1", new List<string> { "a2", "a4" });
        AssertGraphNode(a1, "a1", new List<string> { "if1" });
        AssertGraphNode(a2, "a2", new List<string> { "a3" });
        AssertGraphNode(a3, "a3", new List<string> { "a4" });
        AssertGraphNode(a4, "a4", new List<string>());
    }

    [TestMethod]
    public void DefinitionWithIfScenario2()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(), null),
                    }),
                new WorkflowIfDefinition("if2", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(), null),
                    }),
                new WorkflowStepDefinition("a3", typeof(Step1), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode if1 = graph.Node();
        WorkflowNode a1 = graph.Node(if1.Directions[0]);
        WorkflowNode if2 = graph.Node(a1.Directions[0]);
        WorkflowNode a2 = graph.Node(if2.Directions[0]);
        WorkflowNode a3 = graph.Node(a2.Directions[0]);

        AssertGraphNode(if1, "if1", new List<string> { "a1", "if2" });
        AssertGraphNode(if2, "if2", new List<string> { "a2", "a3" });
        AssertGraphNode(a1, "a1", new List<string> { "if2" });
        AssertGraphNode(a2, "a2", new List<string> { "a3" });
        AssertGraphNode(a3, "a3", new List<string>());
    }

    [TestMethod]
    public void DefinitionWithIfScenario3()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowIfDefinition("if2", null!,
                            new List<WorkflowNodeDefinition>
                            {
                                new WorkflowIfDefinition("if3", null!,
                                    new List<WorkflowNodeDefinition>
                                    {
                                        new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(),
                                            null),
                                    }),
                            })
                    }),
                new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode if1 = graph.Node();
        WorkflowNode if2 = graph.Node(if1.Directions[0]);
        WorkflowNode if3 = graph.Node(if2.Directions[0]);
        WorkflowNode a1 = graph.Node(if3.Directions[0]);
        WorkflowNode a2 = graph.Node(a1.Directions[0]);

        AssertGraphNode(if1, "if1", new List<string> { "if2", "a2" });
        AssertGraphNode(if2, "if2", new List<string> { "if3", "a2" });
        AssertGraphNode(if3, "if3", new List<string> { "a1", "a2" });
        AssertGraphNode(a1, "a1", new List<string> { "a2" });
        AssertGraphNode(a2, "a2", new List<string>());
    }

    [TestMethod]
    public void DefinitionWithIfScenario4()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowIfDefinition("if2", null!,
                            new List<WorkflowNodeDefinition>
                            {
                                new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(),
                                    null),
                            }),
                        new WorkflowIfDefinition("if3", null!,
                            new List<WorkflowNodeDefinition>
                            {
                                new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(),
                                    null),
                            })
                    }),
                new WorkflowStepDefinition("a3", typeof(Step3), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode if1 = graph.Node();
        WorkflowNode if2 = graph.Node(if1.Directions[0]);
        WorkflowNode a1 = graph.Node(if2.Directions[0]);
        WorkflowNode if3 = graph.Node(a1.Directions[0]);
        WorkflowNode a2 = graph.Node(if3.Directions[0]);
        WorkflowNode a3 = graph.Node(a2.Directions[0]);

        AssertGraphNode(if1, "if1", new List<string> { "if2", "a3" });
        AssertGraphNode(if2, "if2", new List<string> { "a1", "if3" });
        AssertGraphNode(if3, "if3", new List<string> { "a2", "a3" });
        AssertGraphNode(a1, "a1", new List<string> { "if3" });
        AssertGraphNode(a2, "a2", new List<string> { "a3" });
        AssertGraphNode(a3, "a3", new List<string>());
    }

    [TestMethod]
    public void DefinitionWitGoToScenario1()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(), null),
                new WorkflowStepDefinition("a2", typeof(Step2), new List<PropertyMap>(), null),
                new WorkflowStepDefinition("a3", typeof(Step2), new List<PropertyMap>(), null),
                new WorkflowGoToDefinition("goto1", "a1")
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode a1 = graph.Node();
        WorkflowNode a2 = graph.Node(a1.Directions[0]);
        WorkflowNode a3 = graph.Node(a2.Directions[0]);
        WorkflowNode goto1 = graph.Node(a3.Directions[0]);

        AssertGraphNode(a1, "a1", new List<string> { "a2" });
        AssertGraphNode(a2, "a2", new List<string> { "a3" });
        AssertGraphNode(a3, "a3", new List<string> { "goto1" });
        AssertGraphNode(goto1, "goto1", new List<string> { "a1" });
    }

    [TestMethod]
    public void DefinitionWitGoToScenario2()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowStepDefinition("a1", typeof(Step1), new List<PropertyMap>(), null),
                        new WorkflowStepDefinition("a2", typeof(Step1), new List<PropertyMap>(), null),
                        new WorkflowGoToDefinition("goto1", "if1")
                    }),
                new WorkflowStepDefinition("a3", typeof(Step1), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode if1 = graph.Node();
        WorkflowNode a1 = graph.Node(if1.Directions[0]);
        WorkflowNode a2 = graph.Node(a1.Directions[0]);
        WorkflowNode goto1 = graph.Node(a2.Directions[0]);
        WorkflowNode a3 = graph.Node(if1.Directions[1]);

        AssertGraphNode(if1, "if1", new List<string> { "a1", "a3" });
        AssertGraphNode(a1, "a1", new List<string> { "a2" });
        AssertGraphNode(a2, "a2", new List<string> { "goto1" });
        AssertGraphNode(a3, "a3", new List<string>());
        AssertGraphNode(goto1, "goto1", new List<string> { "if1" });
    }

    [TestMethod]
    public void DefinitionWitGoToScenario3()
    {
        WorkflowDefinition definition = new("workflow", typeof(object),
            new List<WorkflowNodeDefinition>
            {
                new WorkflowIfDefinition("if1", null!,
                    new List<WorkflowNodeDefinition>
                    {
                        new WorkflowIfDefinition("if2", null!,
                            new List<WorkflowNodeDefinition>
                            {
                                new WorkflowIfDefinition("if3", null!,
                                    new List<WorkflowNodeDefinition>
                                    {
                                        new WorkflowGoToDefinition("goto1", "a2")
                                    }),
                            })
                    }),
                new WorkflowStepDefinition("a1", typeof(Step3), new List<PropertyMap>(), null),
                new WorkflowStepDefinition("a2", typeof(Step3), new List<PropertyMap>(), null)
            });

        WorkflowGraph graph = new(definition);
        WorkflowNode if1 = graph.Node();
        WorkflowNode if2 = graph.Node(if1.Directions[0]);
        WorkflowNode if3 = graph.Node(if2.Directions[0]);
        WorkflowNode goto1 = graph.Node(if3.Directions[0]);
        WorkflowNode a1 = graph.Node(if1.Directions[1]);
        WorkflowNode a2 = graph.Node(a1.Directions[0]);

        AssertGraphNode(if1, "if1", new List<string> { "if2", "a1" });
        AssertGraphNode(if2, "if2", new List<string> { "if3", "a1" });
        AssertGraphNode(if3, "if3", new List<string> { "goto1", "a1" });
        AssertGraphNode(a1, "a1", new List<string> { "a2" });
        AssertGraphNode(a2, "a2", new List<string>());
        AssertGraphNode(goto1, "goto1", new List<string> { "a2" });
    }

    private void AssertGraphNode(WorkflowNode node, string expectedId, List<string> expectedDirections)
    {
        Assert.AreEqual(expectedId, node.Id);
        CollectionAssert.AreEqual(expectedDirections, node.Directions.ToList());
    }
}