using System.Linq.Expressions;

namespace StepFlow.Contracts.Definition;

public record PropertyMap(
    LambdaExpression Source,
    LambdaExpression Target);