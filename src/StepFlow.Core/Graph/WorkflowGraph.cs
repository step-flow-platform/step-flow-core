using System;
using System.Collections.Generic;
using System.Linq;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;

namespace StepFlow.Core.Graph;

public class WorkflowGraph
{
    public WorkflowGraph(WorkflowDefinition workflowDefinition)
    {
        // TODO: Make sure there are no nodes after GoTo
        WorkflowDataType = workflowDefinition.DataType;
        Process(workflowDefinition.Nodes);
    }

    public Type WorkflowDataType { get; }

    public WorkflowNode Node(string? id = null)
    {
        id ??= _firstNodeId;
        if (id is null)
        {
            throw new StepFlowException("Nodes not found");
        }

        return _nodes[id];
    }

    public IReadOnlyList<WorkflowNode> GetNodes()
    {
        return _nodes.Values.ToList();
    }

    private void Process(IEnumerable<WorkflowNodeDefinition> nodeDefinitions)
    {
        foreach (WorkflowNodeDefinition nodeDefinition in nodeDefinitions)
        {
            string nodeId = nodeDefinition.Id ?? MakeNodeId();
            _parent?.AddDirection(nodeId);

            while (_stack.TryPop(out WorkflowNode stackNode))
            {
                stackNode.AddDirection(nodeId);
            }

            WorkflowNode node;
            switch (nodeDefinition)
            {
                case WorkflowStepDefinition stepDefinition:
                    node = new WorkflowNode(nodeId, WorkflowNodeType.Step, stepDefinition);
                    SaveNode(node);
                    _parent = node;
                    break;
                case WorkflowIfDefinition ifDefinition:
                    node = new WorkflowNode(nodeId, WorkflowNodeType.If, ifDefinition);
                    SaveNode(node);
                    _parent = node;
                    Process(ifDefinition.Nodes);
                    _stack.Push(node);
                    break;
                case WorkflowGoToDefinition goToDefinition:
                    node = new WorkflowNode(nodeId, WorkflowNodeType.GoTo, goToDefinition);
                    node.AddDirection(goToDefinition.NextNodeId);
                    SaveNode(node);
                    _parent = null;
                    break;
                default:
                    throw new StepFlowException("Unexpected workflow node definition type");
            }
        }
    }

    private void SaveNode(WorkflowNode node)
    {
        _nodes[node.Id] = node;
        if (_nodes.Count == 1)
        {
            _firstNodeId = node.Id;
        }
    }

    private string MakeNodeId()
    {
        return $"node{++_nodesCount}";
    }

    private readonly Dictionary<string, WorkflowNode> _nodes = new();
    private readonly Stack<WorkflowNode> _stack = new();
    private string? _firstNodeId;
    private WorkflowNode? _parent;
    private int _nodesCount;
}