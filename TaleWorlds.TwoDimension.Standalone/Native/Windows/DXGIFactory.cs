using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000030 RID: 48
	public static class DXGIFactory
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00007033 File Offset: 0x00005233
		public static int EnumAdapters(IntPtr factory, uint index, out IntPtr adapter)
		{
			return ((DXGIFactory.FnEnumAdapters)VTable.Get(factory, 7, typeof(DXGIFactory.FnEnumAdapters)))(factory, index, out adapter);
		}

		// Token: 0x04000149 RID: 329
		private const int Slot_EnumAdapters = 7;

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x06000225 RID: 549
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnEnumAdapters(IntPtr self, uint Index, out IntPtr ppAdapter);
	}
}
