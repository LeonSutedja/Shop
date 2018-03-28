using System;
using System.Linq.Expressions;

namespace Shop.Infrastructure.TableCreator
{
    /// <summary>
    /// Expression chaining code is from: http://stevesspace.com/2016/06/chaining-expressions-in-c/
    /// </summary>
    public static class ExpressionExtension
    {
        public static Expression<Func<TIn, TOut>> Chain<TIn, TInterstitial, TOut>(
          this Expression<Func<TIn, TInterstitial>> inner,
          Expression<Func<TInterstitial, TOut>> outer)
        {
            var visitor = new SwapVisitor(outer.Parameters[0], inner.Body);
            return Expression.Lambda<Func<TIn, TOut>>(visitor.Visit(outer.Body), inner.Parameters);
        }
    }

    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression _source, _replacement;

        public SwapVisitor(Expression source, Expression replacement)
        {
            _source = source;
            _replacement = replacement;
        }

        public override Expression Visit(Expression node)
        {
            return node == _source ? _replacement : base.Visit(node);
        }
    }
}