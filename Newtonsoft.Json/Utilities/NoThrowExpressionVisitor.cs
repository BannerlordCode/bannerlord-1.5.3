using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x00014DE7 File Offset: 0x00012FE7
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
			}
			return base.VisitConditional(node);
		}

		// Token: 0x040001DB RID: 475
		internal static readonly object ErrorResult = new object();
	}
}
