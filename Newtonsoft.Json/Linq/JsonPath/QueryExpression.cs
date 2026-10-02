using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class QueryExpression
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x0002FFD5 File Offset: 0x0002E1D5
		public QueryExpression(QueryOperator @operator)
		{
			this.Operator = @operator;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002FFE4 File Offset: 0x0002E1E4
		public bool IsMatch(JToken root, JToken t)
		{
			return this.IsMatch(root, t, null);
		}

		// Token: 0x06000C08 RID: 3080
		public abstract bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings);

		// Token: 0x040003E0 RID: 992
		internal QueryOperator Operator;
	}
}
