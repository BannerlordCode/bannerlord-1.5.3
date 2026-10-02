using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000DA RID: 218
	[NullableContext(1)]
	[Nullable(0)]
	internal class RootFilter : PathFilter
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x0003058B File Offset: 0x0002E78B
		private RootFilter()
		{
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00030593 File Offset: 0x0002E793
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			return new JToken[] { root };
		}

		// Token: 0x040003E6 RID: 998
		public static readonly RootFilter Instance = new RootFilter();
	}
}
