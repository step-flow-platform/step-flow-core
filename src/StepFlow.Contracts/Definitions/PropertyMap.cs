using System.Linq.Expressions;

namespace StepFlow.Contracts.Definitions;

public record PropertyMap(
    LambdaExpression Source,
    LambdaExpression Target);