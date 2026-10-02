using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryScanFilter : PathFilter
	{
		// Token: 0x06000C16 RID: 3094 RVA: 0x00030557 File Offset: 0x0002E757
		public QueryScanFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00030566 File Offset: 0x0002E766
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken jtoken in current)
			{
				JContainer jcontainer = jtoken as JContainer;
				if (jcontainer != null)
				{
					foreach (JToken jtoken2 in jcontainer.DescendantsAndSelf())
					{
						if (this.Expression.IsMatch(root, jtoken2, settings))
						{
							yield return jtoken2;
						}
					}
					IEnumerator<JToken> enumerator2 = null;
				}
				else if (this.Expression.IsMatch(root, jtoken, settings))
				{
					yield return jtoken;
				}
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003E5 RID: 997
		internal QueryExpression Expression;
	}
}
