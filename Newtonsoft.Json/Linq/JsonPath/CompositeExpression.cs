using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D6 RID: 214
	[NullableContext(1)]
	[Nullable(0)]
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0002FFEF File Offset: 0x0002E1EF
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x0002FFF7 File Offset: 0x0002E1F7
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06000C0B RID: 3083 RVA: 0x00030000 File Offset: 0x0002E200
		public CompositeExpression(QueryOperator @operator)
			: base(@operator)
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00030014 File Offset: 0x0002E214
		public override bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings)
		{
			QueryOperator @operator = this.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t, settings))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t, settings))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
