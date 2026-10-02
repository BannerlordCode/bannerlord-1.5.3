using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CE RID: 206
	[NullableContext(1)]
	[Nullable(0)]
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		// Token: 0x06000BDC RID: 3036 RVA: 0x0002EC40 File Offset: 0x0002CE40
		public ArrayMultipleIndexFilter(List<int> indexes)
		{
			this.Indexes = indexes;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0002EC4F File Offset: 0x0002CE4F
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			foreach (JToken t in current)
			{
				foreach (int num in this.Indexes)
				{
					JToken tokenIndex = PathFilter.GetTokenIndex(t, settings, num);
					if (tokenIndex != null)
					{
						yield return tokenIndex;
					}
				}
				List<int>.Enumerator enumerator2 = default(List<int>.Enumerator);
				t = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040003C8 RID: 968
		internal List<int> Indexes;
	}
}
