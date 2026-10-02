using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006E RID: 110
	public class MBReadOnlyList<T> : List<T>
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public MBReadOnlyList()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000DF04 File Offset: 0x0000C104
		public MBReadOnlyList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000DF0D File Offset: 0x0000C10D
		public MBReadOnlyList(IEnumerable<T> collection)
			: base(collection)
		{
		}
	}
}
