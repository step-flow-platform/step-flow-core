// using System.Linq.Expressions;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Core.Builders;
//
// public class WorkflowIfBuilder : IWorkflowNodeBuilder
// {
//     public WorkflowIfBuilder(string nodeId, LambdaExpression condition)
//     {
//         NodeId = nodeId;
//         _condition = condition;
//     }
//
//     public string NodeId { get; }
//
//     public string? NextNodeId { get; set; }
//
//     public string? NextFalseNodeId { get; set; }
//
//     public WorkflowNodeDefinition Build()
//     {
//         WorkflowIfDefinition definition = new(NodeId, _condition, NextNodeId, NextFalseNodeId);
//         return definition;
//     }
//
//     private readonly LambdaExpression _condition;
// }