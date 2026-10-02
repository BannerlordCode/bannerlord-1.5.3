using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000034 RID: 52
	internal static class GCHandleFactory
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007098 File Offset: 0x00005298
		static GCHandleFactory()
		{
			for (int i = 0; i < 512; i++)
			{
				GCHandleFactory._handles.Add(GCHandle.Alloc(null, GCHandleType.Pinned));
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000070DC File Offset: 0x000052DC
		public static GCHandle GetHandle()
		{
			object locker = GCHandleFactory._locker;
			lock (locker)
			{
				if (GCHandleFactory._handles.Count > 0)
				{
					GCHandle gchandle = GCHandleFactory._handles[GCHandleFactory._handles.Count - 1];
					GCHandleFactory._handles.RemoveAt(GCHandleFactory._handles.Count - 1);
					return gchandle;
				}
			}
			return GCHandle.Alloc(null, GCHandleType.Pinned);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000715C File Offset: 0x0000535C
		public static void ReturnHandle(GCHandle handle)
		{
			object locker = GCHandleFactory._locker;
			lock (locker)
			{
				GCHandleFactory._handles.Add(handle);
			}
		}

		// Token: 0x040000AC RID: 172
		private static List<GCHandle> _handles = new List<GCHandle>();

		// Token: 0x040000AD RID: 173
		private static object _locker = new object();
	}
}
