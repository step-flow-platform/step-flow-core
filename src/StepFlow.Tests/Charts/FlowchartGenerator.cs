// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using StepFlow.Contracts;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Tests.Charts;
//
// public class FlowchartGenerator
// {
//     public string Generate(WorkflowDefinition workflow)
//     {
//         foreach (WorkflowNodeDefinition node in workflow.Nodes)
//         {
//             ProcessNode(node);
//         }
//
//         ReduceGoToNodes();
//         AddStartNode();
//
//         StringBuilder builder = new();
//         AddHeader(builder);
//         BuildNodes(builder);
//         builder.AppendLine();
//         BuildRelations(builder);
//         AddFooter(builder);
//         ReplaceNodeIds(builder);
//         return builder.ToString();
//     }
//
//     private void ProcessNode(WorkflowNodeDefinition nodeDefinition)
//     {
//         switch (nodeDefinition)
//         {
//             case WorkflowStepDefinition stepDefinition:
//                 ProcessStepNode(stepDefinition);
//                 break;
//             case WorkflowIfDefinition ifDefinition:
//                 ProcessIfNode(ifDefinition);
//                 break;
//             case WorkflowGoToDefinition goToDefinition:
//                 ProcessGoToNode(goToDefinition);
//                 break;
//             default:
//                 throw new StepFlowException($"Unexpected NodeType: '{nodeDefinition.GetType().Name}'");
//         }
//     }
//
//     private void ProcessStepNode(WorkflowStepDefinition definition)
//     {
//         NodeModel node = new(definition.Id, definition.StepType.Name, NodeTypeModel.Step);
//         _nodes.Add(node);
//
//         if (definition.NextNodeId is not null)
//         {
//             NodesRelationModel relation = new(node.Id, definition.NextNodeId, null);
//             _relations.Add(relation);
//         }
//         else
//         {
//             AddEndNodeRelation(node.Id);
//         }
//     }
//
//     private void ProcessIfNode(WorkflowIfDefinition definition)
//     {
//         string condition = $"\"{definition.Condition.Body}\"";
//         NodeModel node = new(definition.Id, condition, NodeTypeModel.If);
//         _nodes.Add(node);
//
//         if (definition.TrueNodeId is not null)
//         {
//             NodesRelationModel relation = new(node.Id, definition.TrueNodeId, "true");
//             _relations.Add(relation);
//         }
//
//         if (definition.FalseNodeId is not null)
//         {
//             NodesRelationModel relation = new(node.Id, definition.FalseNodeId, "false");
//             _relations.Add(relation);
//         }
//         else
//         {
//             AddEndNodeRelation(node.Id, "false");
//         }
//     }
//
//     private void ProcessGoToNode(WorkflowGoToDefinition definition)
//     {
//         NodeModel node = new(definition.Id, string.Empty, NodeTypeModel.GoTo);
//         _nodes.Add(node);
//
//         NodesRelationModel relation = new(node.Id, definition.NextNodeId, null);
//         _relations.Add(relation);
//     }
//
//     private void ReduceGoToNodes()
//     {
//         IEnumerable<NodeModel> goToNodes = _nodes.Where(x => x.Type is NodeTypeModel.GoTo);
//         foreach (NodeModel goToNode in goToNodes)
//         {
//             NodesRelationModel goToRelation = _relations.Single(x => x.FromId == goToNode.Id);
//             _relations.Remove(goToRelation);
//
//             IEnumerable<NodesRelationModel> relations = _relations.Where(x => x.ToId == goToNode.Id);
//             foreach (NodesRelationModel relation in relations)
//             {
//                 relation.ToId = goToRelation.ToId;
//             }
//         }
//     }
//
//     private void AddStartNode()
//     {
//         NodeModel? firstNode = _nodes.FirstOrDefault();
//         NodeModel startNode = new NodeModel("startNode", "Start", NodeTypeModel.Control);
//         _nodes.Insert(0, startNode);
//         if (firstNode is not null)
//         {
//             NodesRelationModel relation = new(startNode.Id, firstNode.Id, null);
//             _relations.Insert(0, relation);
//         }
//     }
//
//     private void AddEndNodeRelation(string fromId, string? title = null)
//     {
//         if (_endNode is null)
//         {
//             _endNode = new NodeModel("endNode", "End", NodeTypeModel.Control);
//             _nodes.Add(_endNode);
//         }
//
//         NodesRelationModel relation = new(fromId, _endNode.Id, title);
//         _relations.Add(relation);
//     }
//
//     private void BuildNodes(StringBuilder builder)
//     {
//         foreach (NodeModel node in _nodes)
//         {
//             switch (node.Type)
//             {
//                 case NodeTypeModel.Step:
//                     builder.AppendLine($"{node.Id}[{node.Text}]");
//                     break;
//                 case NodeTypeModel.If:
//                     builder.AppendLine($"{node.Id}{{{node.Text}}}");
//                     break;
//                 case NodeTypeModel.Control:
//                     builder.AppendLine($"{node.Id}(({node.Text}))");
//                     break;
//             }
//         }
//     }
//
//     private void BuildRelations(StringBuilder builder)
//     {
//         foreach (NodesRelationModel relation in _relations)
//         {
//             string title = relation.Title != null ? $"|{relation.Title}|" : string.Empty;
//             builder.AppendLine($"{relation.FromId} -->{title} {relation.ToId}");
//         }
//     }
//
//     private void AddHeader(StringBuilder builder)
//     {
//         builder.AppendLine("```mermaid");
//         builder.AppendLine("flowchart TB");
//     }
//
//     private void AddFooter(StringBuilder builder)
//     {
//         builder.AppendLine("```");
//     }
//
//     private void ReplaceNodeIds(StringBuilder builder)
//     {
//         for (int i = 0; i < _nodes.Count; ++i)
//         {
//             if (Guid.TryParse(_nodes[i].Id, out _))
//             {
//                 builder.Replace(_nodes[i].Id, $"node{i + 1}");
//             }
//         }
//     }
//
//     private readonly List<NodeModel> _nodes = new();
//     private readonly List<NodesRelationModel> _relations = new();
//     private NodeModel? _endNode;
// }