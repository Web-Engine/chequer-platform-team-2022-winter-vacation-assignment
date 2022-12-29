using CsvLite.Models.Identifiers;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Parsers.Antlr;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.TreeImpl.Attributes;
using CsvLite.Sql.TreeImpl.Expressions;
using CsvLite.Sql.TreeImpl.Expressions.Functions;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;
using static CsvLite.Sql.TreeImpl.Expressions.BinaryBooleanAlgebraExpressionNode;
using static CsvLite.Sql.TreeImpl.Expressions.BinaryCalculateExpressionNode;
using static CsvLite.Sql.TreeImpl.Expressions.ComparisonExpressionNode;
using static CsvLite.Sql.TreeImpl.Expressions.UnaryBooleanAlgebraExpressionNode;

namespace CsvLite.Sql.Parsers.Visitors;

public static class ExpressionVisitor
{
    public static IExpressionNode VisitExpression(ExpressionContext context)
    {
        return context switch
        {
            Expression_binaryBooleanAlgebraContext binaryBooleanAlgebraContext =>
                VisitExpression_binaryBooleanAlgebra(binaryBooleanAlgebraContext),
            Expression_unaryBooleanAlgebraContext unaryBooleanAlgebraContext => VisitExpression_unaryBooleanAlgebra(
                unaryBooleanAlgebraContext),
            Expression_comparisonContext comparisonContext => VisitExpression_comparison(comparisonContext),
            Expression_valueContext valueContext => VisitExpression_value(valueContext),

            _ => throw new InvalidOperationException()
        };
    }

    private static IExpressionNode VisitExpression_binaryBooleanAlgebra(Expression_binaryBooleanAlgebraContext context)
    {
        var leftExpressionNode = VisitExpression(context.left);
        var rightExpressionNode = VisitExpression(context.right);

        var @operator = context.@operator.Type switch
        {
            AntlrSqlLexer.AND => BinaryBooleanAlgebraOperator.And,
            AntlrSqlLexer.OR => BinaryBooleanAlgebraOperator.Or,

            _ => throw new InvalidOperationException()
        };

        return new BinaryBooleanAlgebraExpressionNode(
            leftExpressionNode,
            @operator,
            rightExpressionNode
        );
    }

    private static IExpressionNode VisitExpression_unaryBooleanAlgebra(Expression_unaryBooleanAlgebraContext context)
    {
        var expressionNode = VisitExpression(context.expression());

        var @operator = context.@operator.Type switch
        {
            AntlrSqlLexer.NOT => UnaryBooleanAlgebraOperator.Not,

            _ => throw new InvalidOperationException()
        };

        return new UnaryBooleanAlgebraExpressionNode(
            @operator,
            expressionNode
        );
    }

    private static IExpressionNode VisitExpression_comparison(Expression_comparisonContext context)
    {
        var leftExpressionNode = VisitExpressionValue(context.left);
        var rightExpressionNode = VisitExpressionValue(context.right);

        var @operator = context.@operator.Type switch
        {
            AntlrSqlLexer.EQ__ => ComparisonOperator.Equal,
            AntlrSqlLexer.NEQ__ => ComparisonOperator.NotEqual,
            AntlrSqlLexer.GT__ => ComparisonOperator.GreaterThan,
            AntlrSqlLexer.GTE__ => ComparisonOperator.GreaterThanOrEqual,
            AntlrSqlLexer.LT__ => ComparisonOperator.LessThan,
            AntlrSqlLexer.LTE__ => ComparisonOperator.LessThanOrEqual,

            _ => throw new InvalidOperationException("Unknown operator")
        };

        return new ComparisonExpressionNode(
            leftExpressionNode,
            @operator,
            rightExpressionNode
        );
    }

    private static IExpressionNode VisitExpression_value(Expression_valueContext context)
    {
        return VisitExpressionValue(context.expressionValue());
    }

    public static IEnumerable<IAttributeReferenceNode> VisitReferenceAttributeList(
        ReferenceAttributeListContext context)
    {
        return context.referenceAttribute().Select(VisitReferenceAttribute);
    }

    public static IAttributeReferenceNode VisitReferenceAttribute(ReferenceAttributeContext context)
    {
        var relationIdentifier = context.relationIdentifier?.ToIdentifier();
        var attributeIdentifier = context.attributeIdentifier.ToIdentifier();

        if (relationIdentifier is null)
            return new AttributeReferenceNode(
                new ExplicitAttributeReference(new QualifiedIdentifier(attributeIdentifier))
            );


        return new AttributeReferenceNode(
            new ExplicitAttributeReference(
                new QualifiedIdentifier(relationIdentifier, attributeIdentifier)
            )
        );
    }

    public static AttributeReferenceNode VisitReferenceAllAttribute(ReferenceAllAttributeContext context)
    {
        return new AttributeReferenceNode(
            new AllAttributeReference(
                context.relationIdentifier?.ToIdentifier()
            )
        );
    }

    public static IExpressionNode VisitExpressionValue(ExpressionValueContext context)
    {
        return context switch
        {
            ExpressionValue_parenthesisContext parenthesisContext =>
                VisitExpressionValue_parenthesis(parenthesisContext),

            ExpressionValue_unaryContext unaryContext => VisitExpressionValue_unary(unaryContext),
            ExpressionValue_binaryContext binaryContext => VisitExpressionValue_binary(binaryContext),
            ExpressionValue_functionCallContext functionCallContext =>
                VisitExpressionValue_functionCall(functionCallContext),

            ExpressionValue_literalContext primitiveContext => ExpressionValue_literal(primitiveContext),

            ExpressionValue_referenceAllAttributeContext referenceAllAttributeContext =>
                VisitExpressionValue_referenceAllAttribute(referenceAllAttributeContext),

            ExpressionValue_referenceAttributeContext referenceAttributeContext =>
                VisitExpressionValue_referenceAttribute(referenceAttributeContext),

            _ => throw new InvalidOperationException("Unknown ExpressionValueContext")
        };
    }

    public static IExpressionNode VisitExpressionInParenthesis(ExpressionInParenthesisContext context)
    {
        return context switch
        {
            ExpressionInParenthesis_expressionContext expressionContext => VisitExpressionInParenthesis_expression(
                expressionContext),
            ExpressionInParenthesis_selectContext selectContext => VisitExpressionInParenthesis_select(selectContext),

            _ => throw new InvalidOperationException("Unknown ExpressionInParenthesisContext")
        };
    }

    private static IExpressionNode VisitExpressionInParenthesis_expression(
        ExpressionInParenthesis_expressionContext context)
    {
        return VisitExpression(context.expression());
    }

    private static IExpressionNode VisitExpressionInParenthesis_select(ExpressionInParenthesis_selectContext context)
    {
        var relationNode = RelationVisitor.VisitStatementSelect(context.statementSelect());

        return new RelationExpressionNode(relationNode);
    }

    private static IExpressionNode VisitExpressionValue_parenthesis(ExpressionValue_parenthesisContext context)
    {
        return VisitExpressionInParenthesis(context.expressionInParenthesis());
    }

    private static IExpressionNode VisitExpressionValue_unary(ExpressionValue_unaryContext context)
    {
        throw new NotSupportedException("Unary expression not supported yet");
    }

    private static IExpressionNode VisitExpressionValue_binary(ExpressionValue_binaryContext context)
    {
        var leftExpressionNode = VisitExpressionValue(context.left);
        var rightExpressionNode = VisitExpressionValue(context.right);

        var @operator = context.@operator.Type switch
        {
            AntlrSqlLexer.PLUS__ => BinaryCalculateOperator.Addition,
            AntlrSqlLexer.DASH__ => BinaryCalculateOperator.Subtraction,
            AntlrSqlLexer.ASTERISK__ => BinaryCalculateOperator.Multiplication,
            AntlrSqlLexer.SLASH__ => BinaryCalculateOperator.Division,
            AntlrSqlLexer.PERCENT__ => BinaryCalculateOperator.Modulus,

            _ => throw new InvalidOperationException("Unknown binary operator"),
        };

        return new BinaryCalculateExpressionNode(
            leftExpressionNode,
            @operator,
            rightExpressionNode
        );
    }

    private static IExpressionNode VisitExpressionValue_functionCall(ExpressionValue_functionCallContext context)
    {
        return VisitExpressionFunctionCall(context.expressionFunctionCall());
    }

    private static IExpressionNode ExpressionValue_literal(ExpressionValue_literalContext context)
    {
        return VisitExpressionLiteral(context.expressionLiteral());
    }

    private static IExpressionNode VisitExpressionValue_referenceAllAttribute(
        ExpressionValue_referenceAllAttributeContext context)
    {
        return new AttributeReferenceExpressionNode(
            VisitReferenceAllAttribute(context.referenceAllAttribute())
        );
    }

    private static IExpressionNode VisitExpressionValue_referenceAttribute(
        ExpressionValue_referenceAttributeContext context)
    {
        return new AttributeReferenceExpressionNode(
            VisitReferenceAttribute(context.referenceAttribute())
        );
    }

    private static IExpressionNode VisitExpressionFunctionCall(ExpressionFunctionCallContext context)
    {
        var functionName = context.identifier().ToIdentifier().Value;
        var expressions = VisitExpressionList(context.expressionList());

        return functionName.ToUpperInvariant() switch
        {
            "COUNT" => new CountExpressionNode(expressions.Single(), false),
            "MAX" => new MaxExpressionNode(expressions.Single()),
            "MIN" => new MinExpressionNode(expressions.Single()),

            _ => throw new InvalidOperationException($"Unknown function {functionName}")
        };
    }

    private static IEnumerable<IExpressionNode> VisitExpressionList(ExpressionListContext context)
    {
        return context.expression().Select(VisitExpression);
    }

    private static IExpressionNode VisitExpressionLiteral(ExpressionLiteralContext context)
    {
        return context switch
        {
            ExpressionLiteral_stringContext stringContext => VisitExpressionLiteral_string(stringContext),
            ExpressionLiteral_booleanContext booleanContext => VisitExpressionLiteral_boolean(booleanContext),
            ExpressionLiteral_integerContext integerContext => VisitExpressionLiteral_integer(integerContext),
            ExpressionLiteral_doubleContext doubleContext => VisitExpressionLiteral_double(doubleContext),

            _ => throw new InvalidOperationException("Unknown ExpressionLiteral")
        };
    }

    private static IExpressionNode VisitExpressionLiteral_string(ExpressionLiteral_stringContext context)
    {
        return VisitLiteralString(context.literalString());
    }

    private static IExpressionNode VisitExpressionLiteral_boolean(ExpressionLiteral_booleanContext context)
    {
        return VisitLiteralBoolean(context.literalBoolean());
    }

    private static IExpressionNode VisitExpressionLiteral_integer(ExpressionLiteral_integerContext context)
    {
        return VisitLiteralInteger(context.literalInteger());
    }

    private static IExpressionNode VisitExpressionLiteral_double(ExpressionLiteral_doubleContext context)
    {
        return VisitLiteralDouble(context.literalDouble());
    }

    private static IExpressionNode VisitLiteralString(LiteralStringContext context)
    {
        return new LiteralExpressionNode(context.GetString());
    }

    private static IExpressionNode VisitLiteralBoolean(LiteralBooleanContext context)
    {
        return new LiteralExpressionNode(context.GetBoolean());
    }

    private static IExpressionNode VisitLiteralInteger(LiteralIntegerContext context)
    {
        return new LiteralExpressionNode(context.GetInteger());
    }

    private static IExpressionNode VisitLiteralDouble(LiteralDoubleContext context)
    {
        throw new NotSupportedException("Double not supported yet");
    }

    public static TupleExpressionNode VisitValueList(ValueListContext context)
    {
        return new TupleExpressionNode(context.valueItem().Select(VisitValueItem));
    }

    public static IExpressionNode VisitValueItem(ValueItemContext context)
    {
        return VisitExpression(context.expression());
    }
}