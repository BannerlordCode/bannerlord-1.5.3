using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000069 RID: 105
	public class MBList<T> : MBReadOnlyList<T>
	{
		// Token: 0x06000356 RID: 854 RVA: 0x0000C337 File Offset: 0x0000A537
		public MBList()
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000C33F File Offset: 0x0000A53F
		public MBList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000C348 File Offset: 0x0000A548
		public MBList(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000C351 File Offset: 0x0000A551
		public MBList(List<T> collection)
			: base(collection)
		{
		}
	}
}
