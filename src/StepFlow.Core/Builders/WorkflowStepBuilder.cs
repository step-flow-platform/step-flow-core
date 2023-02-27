// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using StepFlow.Contracts;
// using StepFlow.Contracts.Definition;
//
// namespace StepFlow.Core.Builders;
//
// internal class WorkflowStepBuilder<TStep, TData> : IWorkflowStepBuilder<TStep, TData>, IWorkflowNodeBuilder
//     where TStep : IStep
// {
//     public WorkflowStepBuilder()
//     {
//         NodeId = Guid.NewGuid().ToString();
//     }
//
//     public string NodeId { get; private set; }
//
//     public string? NextNodeId { get; set; }
//
//     public IWorkflowStepBuilder<TStep, TData> Id(string id)
//     {
//         NodeId = id;
//         return this;
//     }
//
//     public IWorkflowStepBuilder<TStep, TData> Input<TInput>(Expression<Func<TStep, TInput>> stepProperty,
//         Expression<Func<TData, TInput>> value)
//     {
//         _input.Add(new PropertyMap(value, stepProperty));
//         return this;
//     }
//
//     public void Output<TInput>(Expression<Func<TData, TInput>> value, Expression<Func<TStep, TInput>> stepProperty)
//     {
//         _output = new PropertyMap(stepProperty, value);
//     }
//
//     public WorkflowNodeDefinition Build()
//     {
//         WorkflowStepDefinition definition = new(NodeId, NextNodeId, typeof(TStep), _input, _output);
//         return definition;
//     }
//
//     private readonly List<PropertyMap> _input = new();
//     private PropertyMap? _output;
// }