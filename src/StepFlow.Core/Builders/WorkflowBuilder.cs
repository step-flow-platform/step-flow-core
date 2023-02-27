// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Expressions;
// using StepFlow.Contracts;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Core.Builders
// {
//     internal class WorkflowBuilder<TData> : IWorkflowBuilder<TData>
//         where TData : new()
//     {
//         public IWorkflowBuilder<TData> Step<TStep>()
//             where TStep : IStep
//         {
//             return Step<TStep>(_ => { });
//         }
//
//         public IWorkflowBuilder<TData> Step<TStep>(Action<IWorkflowStepBuilder<TStep, TData>> stepBuilderAction)
//             where TStep : IStep
//         {
//             WorkflowStepBuilder<TStep, TData> workflowStepBuilder = new();
//             stepBuilderAction(workflowStepBuilder);
//
//             MakeNodeRelations(workflowStepBuilder.NodeId);
//             _nodes.Add(workflowStepBuilder);
//             return this;
//         }
//
//         public IWorkflowBuilder<TData> If(Expression<Func<TData, bool>> condition,
//             Action<IWorkflowBuilder<TData>> ifBuilderAction)
//         {
//             return If(Guid.NewGuid().ToString(), condition, ifBuilderAction);
//         }
//
//         public IWorkflowBuilder<TData> If(string id, Expression<Func<TData, bool>> condition,
//             Action<IWorkflowBuilder<TData>> ifBuilderAction)
//         {
//             WorkflowIfBuilder ifBuilder = new(id, condition);
//             MakeNodeRelations(ifBuilder.NodeId);
//             _nodes.Add(ifBuilder);
//
//             ifBuilderAction(this);
//             _ifNodes.Push(ifBuilder);
//             return this;
//         }
//
//         public void GoTo(string stepName)
//         {
//             WorkflowGotoBuilder goToBuilder = new(stepName);
//             MakeNodeRelations(goToBuilder.NodeId);
//             _nodes.Add(goToBuilder);
//         }
//
//         public WorkflowDefinition BuildDefinition()
//         {
//             IEnumerable<WorkflowNodeDefinition> nodeDefinitions = _nodes.Select(x => x.Build());
//             return new WorkflowDefinition(typeof(TData), nodeDefinitions.ToList());
//         }
//
//         private void MakeNodeRelations(string nodeId)
//         {
//             if (_nodes.Count > 0)
//             {
//                 IWorkflowNodeBuilder lastNode = _nodes.Last();
//                 lastNode.NextNodeId ??= nodeId;
//             }
//
//             while (_ifNodes.TryPop(out var ifNode))
//             {
//                 ifNode.NextFalseNodeId = nodeId;
//             }
//         }
//
//         private readonly List<IWorkflowNodeBuilder> _nodes = new();
//         private readonly Stack<WorkflowIfBuilder> _ifNodes = new();
//     }
// }