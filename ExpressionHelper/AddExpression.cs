using System.Linq.Expressions;

namespace Util.Common
{
    public class AddExpression : ExpressionVisitor
    {
        private readonly Expression from;
        private readonly Expression to;
        public AddExpression(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }
        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}
