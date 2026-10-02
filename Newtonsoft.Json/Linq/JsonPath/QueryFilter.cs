using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryFilter : PathFilter
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x00030523 File Offset: 0x0002E723
		public QueryFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00030532 File Offset: 0x0002E732
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jtoken))
				{
					if (this.Expression.IsMatch(root, jtoken2, settings))
					{
						yield return jtoken2;
					}
				}
				IEnumerator<JToken> enumerator2 = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E4 RID: 996
		internal QueryExpression Expression;
	}
}
