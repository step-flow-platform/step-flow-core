using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;

namespace StepFlow.Tests.Charts;

public class FlowchartGenerator
{
    public string Generate(WorkflowDefinition workflow)
    {
        WorkflowBranchDefinition? mainBranchDefinition = workflow.MainBranch as WorkflowBranchDefinition;
        if (mainBranchDefinition is null)
        {
            throw new StepFlowException("MainBranch is not WorkflowBranchDefinition");
        }

        ProcessBranch(mainBranchDefinition);

        StringBuilder builder = new();
        AddHeader(builder);
        BuildNodes(builder);
        builder.AppendLine();
        BuildRelations(builder);
        AddFooter(builder);
        return builder.ToString();
    }

    private void ProcessBranch(WorkflowBranchDefinition branchDefinition)
    {
        foreach (WorkflowNodeDefinition nodeDefinition in branchDefinition.Nodes)
        {
            ProcessNode(nodeDefinition);
            if (nodeDefinition.NodeType is WorkflowNodeType.Branch)
            {
                NodeModel branchNode = _nodes.Last();
                ProcessBranch((WorkflowBranchDefinition)nodeDefinition);
                _branchNodes.Push(branchNode);
            }
        }
    }

    private void ProcessNode(WorkflowNodeDefinition nodeDefinition)
    {
        NodeModel node = nodeDefinition.NodeType switch
        {
            WorkflowNodeType.Branch => ConvertBranch((WorkflowBranchDefinition)nodeDefinition),
            WorkflowNodeType.Step => ConvertStep((WorkflowStepDefinition)nodeDefinition),
            _ => throw new StepFlowException($"Unexpected NodeType: '{nodeDefinition.NodeType}'")
        };

        if (_nodes.Count > 0)
        {
            NodeModel lastNode = _nodes.Last();
            string? relationTitle = lastNode.Type is NodeTypeModel.If ? "true" : null;
            _relations.Add(new NodesRelationModel(lastNode, node, relationTitle));
            while (_branchNodes.TryPop(out var branchNode))
            {
                _relations.Add(new NodesRelationModel(branchNode, node, "false"));
            }
        }

        _nodes.Add(node);
    }

    private NodeModel ConvertStep(WorkflowStepDefinition stepDefinition)
    {
        string id = $"node{++_nodeCounter}";
        return new NodeModel(id, stepDefinition.StepType.Name, NodeTypeModel.Step);
    }

    private NodeModel ConvertBranch(WorkflowBranchDefinition branchDefinition)
    {
        string id = $"node{++_nodeCounter}";
        string condition = branchDefinition.Condition is not null ? branchDefinition.Condition.ToString() : " ";
        return new NodeModel(id, condition, NodeTypeModel.If);
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
            }
        }
    }

    private void BuildRelations(StringBuilder builder)
    {
        foreach (NodesRelationModel relation in _relations)
        {
            string title = relation.Title != null ? $"|{relation.Title}|" : string.Empty;
            builder.AppendLine($"{relation.From.Id} -->{title} {relation.To.Id}");
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
    private readonly List<NodesRelationModel> _relations = new();
    private readonly Stack<NodeModel> _branchNodes = new();
    private int _nodeCounter;
}