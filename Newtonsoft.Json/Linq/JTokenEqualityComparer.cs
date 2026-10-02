using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C5 RID: 197
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x0002CE66 File Offset: 0x0002B066
		[NullableContext(2)]
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0002CE6F File Offset: 0x0002B06F
		[NullableContext(1)]
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
