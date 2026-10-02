using System;
using System.Collections.Generic;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002D RID: 45
	internal class CaseInsensitiveComparer : IEqualityComparer<string>
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00006FFA File Offset: 0x000051FA
		public bool Equals(string x, string y)
		{
			return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007004 File Offset: 0x00005204
		public int GetHashCode(string x)
		{
			return x.ToLowerInvariant().GetHashCode();
		}
	}
}
