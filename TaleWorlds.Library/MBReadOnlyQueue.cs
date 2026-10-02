using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006F RID: 111
	public class MBReadOnlyQueue<T> : Queue<T>
	{
		// Token: 0x060003E9 RID: 1001 RVA: 0x0000DF16 File Offset: 0x0000C116
		public MBReadOnlyQueue()
		{
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000DF1E File Offset: 0x0000C11E
		public MBReadOnlyQueue(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000DF27 File Offset: 0x0000C127
		public MBReadOnlyQueue(Queue<T> queue)
			: base(queue)
		{
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000DF30 File Offset: 0x0000C130
		public MBReadOnlyQueue(IEnumerable<T> collection)
			: base(collection)
		{
		}
	}
}
