using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepFlow.Contracts.Definition;
using StepFlow.Core.Graph;

namespace StepFlow.Tests.Charts;

public class FlowchartGenerator
{
    public string Generate(WorkflowDefinition definition)
    {
        WorkflowGraph graph = new WorkflowGraph(definition);
        Process(graph);

        StringBuilder builder = new();
        ReduceGoToNodes();
        AddStartNode();
        AddHeader(builder);
        BuildNodes(builder);
        builder.AppendLine();
        BuildRelations(builder);
        AddFooter(builder);
        return builder.ToString();
    }

    private void Process(WorkflowGraph graph)
    {
        IReadOnlyList<WorkflowNode> nodes = graph.GetNodes();
        foreach (WorkflowNode node in nodes)
        {
            switch (node.NodeType)
            {
                case WorkflowNodeType.Step:
                    ProcessStepNode(node);
                    break;
                case WorkflowNodeType.If:
                    ProcessIfNode(node);
                    break;
                case WorkflowNodeType.GoTo:
                    ProcessGoToNode(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void ProcessStepNode(WorkflowNode node)
    {
        WorkflowStepDefinition definition = (WorkflowStepDefinition)node.Definition;
        NodeModel model = new(node.Id, definition.StepType.Name, NodeTypeModel.Step);
        _nodes.Add(model);

        switch (node.Directions.Count)
        {
            case 1:
                NodesDirectionModel direction = new(node.Id, node.Directions[0], null);
                _directions.Add(direction);
                break;
            case 0:
                AddEndNodeRelation(node.Id);
                break;
            default:
                throw new ApplicationException("Unexpected workflow STEP directions count.");
        }
    }

    private void ProcessIfNode(WorkflowNode node)
    {
        WorkflowIfDefinition definition = (WorkflowIfDefinition)node.Definition;
        string condition = $"\"{definition.Condition.Body}\"";
        NodeModel model = new(node.Id, condition, NodeTypeModel.If);
        _nodes.Add(model);

        switch (node.Directions.Count)
        {
            case 1:
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[0], "true"));
                AddEndNodeRelation(node.Id, "false");
                break;
            case 2:
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[0], "true"));
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[1], "false"));
                break;
            default:
                throw new ApplicationException("Unexpected workflow IF directions count.");
        }
    }

    private void ProcessGoToNode(WorkflowNode node)
    {
        NodeModel model = new(node.Id, ".", NodeTypeModel.GoTo);
        _nodes.Add(model);
        _directions.Add(new NodesDirectionModel(node.Id, node.Directions[0], null));
    }

    private void ReduceGoToNodes()
    {
        IEnumerable<NodeModel> goToNodes = _nodes.Where(x => x.Type is NodeTypeModel.GoTo);
        foreach (NodeModel goToNode in goToNodes)
        {
            NodesDirectionModel goToRelation = _directions.Single(x => x.FromId == goToNode.Id);
            _directions.Remove(goToRelation);

            IEnumerable<NodesDirectionModel> relations = _directions.Where(x => x.ToId == goToNode.Id);
            foreach (NodesDirectionModel relation in relations)
            {
                relation.ToId = goToRelation.ToId;
            }
        }
    }

    private void AddStartNode()
    {
        NodeModel? firstNode = _nodes.FirstOrDefault();
        NodeModel startNode = new NodeModel("startNode", "Start", NodeTypeModel.Control);
        _nodes.Insert(0, startNode);
        if (firstNode is not null)
        {
            NodesDirectionModel direction = new(startNode.Id, firstNode.Id, null);
            _directions.Insert(0, direction);
        }
    }

    private void AddEndNodeRelation(string fromId, string? title = null)
    {
        if (_endNode is null)
        {
            _endNode = new NodeModel("endNode", "End", NodeTypeModel.Control);
            _nodes.Add(_endNode);
        }

        NodesDirectionModel direction = new(fromId, _endNode.Id, title);
        _directions.Add(direction);
    }

    private void BuildNodes(StringBuilder builder)
    {
        foreach (NodeModel node in _nodes)
        {
            switch (node.Type)
            {
                case NodeTypeModel.Step:
                    builder.AppendLine($"{node.Id}[{node.Text}]");
                    break;
                case NodeTypeModel.If:
                    builder.AppendLine($"{node.Id}{{{node.Text}}}");
                    break;
                case NodeTypeModel.Control:
                    builder.AppendLine($"{node.Id}(({node.Text}))");
                    break;
                case NodeTypeModel.GoTo:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void BuildRelations(StringBuilder builder)
    {
        foreach (NodesDirectionModel relation in _directions)
        {
            string title = relation.Title != null ? $"|{relation.Title}|" : string.Empty;
            builder.AppendLine($"{relation.FromId} -->{title} {relation.ToId}");
        }
    }

    private void AddHeader(StringBuilder builder)
    {
        builder.AppendLine("```mermaid");
        builder.AppendLine("flowchart TB");
    }

    private void AddFooter(StringBuilder builder)
    {
        builder.AppendLine("```");
    }

    private readonly List<NodeModel> _nodes = new();
    private readonly List<NodesDirectionModel> _directions = new();
    private NodeModel? _endNode;
}