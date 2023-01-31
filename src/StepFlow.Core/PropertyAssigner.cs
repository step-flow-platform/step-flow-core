using System.Linq;
using System.Linq.Expressions;

namespace StepFlow.Core;

internal class PropertyAssigner
{
    public PropertyAssigner(LambdaExpression sourceExpression, LambdaExpression targetExpression)
    {
        _sourceExpression = sourceExpression;
        _targetExpression = targetExpression;
    }

    public void Assign(object source, object target)
    {
        object resolvedValue = _sourceExpression.Compile().DynamicInvoke(source);
        UnaryExpression valueExpression =
            Expression.Convert(Expression.Constant(resolvedValue), _targetExpression.ReturnType);
        LambdaExpression assignExpression =
            Expression.Lambda(Expression.Assign(_targetExpression.Body, valueExpression),
                _targetExpression.Parameters.Single());
        assignExpression.Compile().DynamicInvoke(target);
    }

    private readonly LambdaExpression _sourceExpression;

    private readonly LambdaExpression _targetExpression;
}