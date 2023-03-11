using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using StepFlow.Contracts;
using StepFlow.Contracts.Definition;
using StepFlow.Dsl.Model;

namespace StepFlow.Dsl;

internal class WorkflowDefinitionLoader : IWorkflowDefinitionLoader
{
    public WorkflowDefinitionLoader(WorkflowDefinitionLoaderOptions options)
    {
        _options = options;
    }

    public WorkflowDefinition Load(string source, Func<string, WorkflowDefinitionModel?> deserializer)
    {
        WorkflowDefinitionModel? model = deserializer(source);
        if (model is null)
        {
            throw new StepFlowException("Failed to deserialize workflow model");
        }

        WorkflowDefinition definition = Convert(model);
        return definition;
    }

    private WorkflowDefinition Convert(WorkflowDefinitionModel model)
    {
        Type dataType = typeof(object);
        if (model.Data is not null)
        {
            string dataTypeName = $"{_options.DataNamespace}.{model.Data}, {_options.AssemblyName}";
            dataType = Type.GetType(dataTypeName, true, true)!;
        }

        List<WorkflowNodeDefinition> nodes = model.Steps.Select(x => ConvertNode(x, dataType)).ToList();
        WorkflowDefinition definition = new(dataType, nodes);
        return definition;
    }

    private WorkflowNodeDefinition ConvertNode(WorkflowNodeModel nodeModel, Type dataType)
    {
        return nodeModel.Id switch
        {
            "+If" => ConvertIf(nodeModel, dataType),
            "+GoTo" => ConvertGoTo(nodeModel),
            _ => ConvertStep(nodeModel, dataType)
        };
    }

    private WorkflowIfDefinition ConvertIf(WorkflowNodeModel nodeModel, Type dataType)
    {
        if (nodeModel.Condition is null)
        {
            throw new StepFlowException("Condition property must be declared in the IF node");
        }

        ParameterExpression conditionParameter = Expression.Parameter(dataType, "data");
        LambdaExpression condition =
            DynamicExpressionParser.ParseLambda(new[] { conditionParameter }, typeof(object), nodeModel.Condition);
        List<WorkflowNodeDefinition> nodes = nodeModel.Steps.Select(x => ConvertNode(x, dataType)).ToList();
        return new WorkflowIfDefinition(nodeModel.Id, condition, nodes);
    }

    private WorkflowGoToDefinition ConvertGoTo(WorkflowNodeModel nodeModel)
    {
        if (nodeModel.NextId is null)
        {
            throw new StepFlowException("NextId property must be declared in the GOTO node");
        }

        return new WorkflowGoToDefinition(nodeModel.Id, nodeModel.NextId);
    }

    private WorkflowStepDefinition ConvertStep(WorkflowNodeModel nodeModel, Type dataType)
    {
        string typeName = $"{_options.StepsNamespace}.{nodeModel.Type}, {_options.AssemblyName}";
        Type stepType = Type.GetType(typeName, true, true)!;

        List<PropertyMap> input = new();
        if (nodeModel.Input is not null)
        {
            foreach (KeyValuePair<string, object> inputPropertyPair in nodeModel.Input)
            {
                PropertyMap inputPropertyMap = ConvertPropertyMap(dataType, inputPropertyPair.Value.ToString(), "data",
                    stepType, inputPropertyPair.Key);
                input.Add(inputPropertyMap);
            }
        }

        PropertyMap? output = null;
        if (nodeModel.Output is not null)
        {
            (string dataPropertyName, object expression) = nodeModel.Output.Single();
            output = ConvertPropertyMap(stepType, expression.ToString(), "step", dataType, dataPropertyName);
        }

        return new WorkflowStepDefinition(nodeModel.Id, stepType, input, output);
    }

    private PropertyMap ConvertPropertyMap(Type sourceType, string sourceExpression, string sourceParameterName,
        Type targetType, string targetPropertyName)
    {
        PropertyInfo? targetPropertyInfo = targetType.GetProperty(targetPropertyName);
        if (targetPropertyInfo is null)
        {
            throw new StepFlowException($"Unknown property '{targetPropertyName}' in type '{targetType}'");
        }

        ParameterExpression sourceParameter = Expression.Parameter(sourceType, sourceParameterName);
        LambdaExpression source =
            DynamicExpressionParser.ParseLambda(new[] { sourceParameter }, typeof(object), sourceExpression);

        ParameterExpression targetParameter = Expression.Parameter(targetType);
        MemberExpression targetProperty = Expression.Property(targetParameter, targetPropertyInfo);
        Type targetFuncType = typeof(Func<,>).MakeGenericType(targetType, targetPropertyInfo.PropertyType);
        LambdaExpression target = Expression.Lambda(targetFuncType, targetProperty, targetParameter);

        return new PropertyMap(source, target);
    }

    private readonly WorkflowDefinitionLoaderOptions _options;
}