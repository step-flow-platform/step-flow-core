using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using StepFlow.Contracts;
using StepFlow.Contracts.Definitions;
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

        List<WorkflowNodeDefinition> mainBranchNodes = model.Steps.Select(x => ConvertNode(x, dataType)).ToList();
        WorkflowBranchDefinition mainBranch = new(null, mainBranchNodes);
        WorkflowDefinition definition = new(dataType, mainBranch);
        return definition;
    }

    private WorkflowNodeDefinition ConvertNode(WorkflowNodeModel nodeModel, Type dataType)
    {
        return nodeModel.Name switch
        {
            "+If" => ConvertIfBranch(nodeModel, dataType),
            _ => ConvertStep(nodeModel, dataType)
        };
    }

    private WorkflowBranchDefinition ConvertIfBranch(WorkflowNodeModel nodeModel, Type dataType)
    {
        WorkflowBranchModel? branchModel = nodeModel as WorkflowBranchModel;
        if (branchModel is null)
        {
            throw new StepFlowException("Failed to load definition");
        }

        ParameterExpression conditionParameter = Expression.Parameter(dataType, "data");
        LambdaExpression condition =
            DynamicExpressionParser.ParseLambda(new[] { conditionParameter }, typeof(object), branchModel.Condition);
        List<WorkflowNodeDefinition> nodes = branchModel.Steps.Select(x => ConvertNode(x, dataType)).ToList();
        return new WorkflowBranchDefinition(condition, nodes);
    }

    private WorkflowStepDefinition ConvertStep(WorkflowNodeModel nodeModel, Type dataType)
    {
        WorkflowStepModel? stepModel = nodeModel as WorkflowStepModel;
        if (stepModel is null)
        {
            throw new StepFlowException("Failed to load definition");
        }

        string typeName = $"{_options.StepsNamespace}.{stepModel.Name}, {_options.AssemblyName}";
        Type stepType = Type.GetType(typeName, true, true)!;

        List<PropertyMap> input = new();
        if (stepModel.Input is not null)
        {
            foreach (KeyValuePair<string, object> inputPropertyPair in stepModel.Input)
            {
                PropertyMap inputPropertyMap = ConvertPropertyMap(dataType, inputPropertyPair.Value.ToString(), "data",
                    stepType, inputPropertyPair.Key);
                input.Add(inputPropertyMap);
            }
        }

        PropertyMap? output = null;
        if (stepModel.Output is not null)
        {
            (string dataPropertyName, object expression) = stepModel.Output.Single();
            output = ConvertPropertyMap(stepType, expression.ToString(), "step", dataType, dataPropertyName);
        }

        return new WorkflowStepDefinition(stepType, input, output);
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