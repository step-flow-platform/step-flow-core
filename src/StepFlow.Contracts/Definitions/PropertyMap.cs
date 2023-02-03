using System.Linq.Expressions;

namespace StepFlow.Contracts.Definitions;

public class PropertyMap
{
    public PropertyMap(LambdaExpression source, LambdaExpression target)
    {
        Source = source;
        Target = target;
    }

    public LambdaExpression Source { get; }

    public LambdaExpression Target { get; }
}