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

        // WorkflowDefinition definition = Convert(model);
        // return definition;
        return null!;
    }

    /*private WorkflowDefinition Convert(WorkflowDefinitionModel model)
    {
        Type dataType = typeof(object);
        if (model.Data is not null)
        {
            string dataTypeName = $"{_options.DataNamespace}.{model.Data}, {_options.AssemblyName}";
            dataType = Type.GetType(dataTypeName, true, true)!;
        }

        WorkflowBranchDefinition mainBranch =
        foreach (WorkflowStepModel stepModel in model.Steps)
        {
            mainBranch.Steps.Add(ConvertStep(stepModel, dataType));
        }

        WorkflowDefinition definition = new(dataType);
        definition.Branches.Add(mainBranch);
        return definition;
    }*/

    /*private WorkflowBranchDefinition ConvertBranch(WorkflowBranchModel model)
    {
    }*/

    /*private WorkflowStepDefinition ConvertStep(WorkflowStepModel model, Type dataType)
    {
        string typeName = $"{_options.StepsNamespace}.{model.Name}, {_options.AssemblyName}";
        Type stepType = Type.GetType(typeName, true, true)!;
        WorkflowStepDefinition stepDefinition = new(stepType);

        if (model.Input is not null)
        {
            foreach (KeyValuePair<string, object> inputPropertyPair in model.Input)
            {
                PropertyMap inputPropertyMap = ConvertPropertyMap(dataType, inputPropertyPair.Value.ToString(),
                    "data", stepType, inputPropertyPair.Key);
                stepDefinition.Input.Add(inputPropertyMap);
            }
        }

        if (model.Output is not null)
        {
            (string dataPropertyName, object expression) = model.Output.Single();
            stepDefinition.Output =
                ConvertPropertyMap(stepType, expression.ToString(), "step", dataType, dataPropertyName);
        }

        return stepDefinition;
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
    }*/

    private readonly WorkflowDefinitionLoaderOptions _options;
}