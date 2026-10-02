using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000031 RID: 49
	public static class DXGIAdapter
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00007053 File Offset: 0x00005253
		public static int EnumOutputs(IntPtr adapter, uint index, out IntPtr output)
		{
			return ((DXGIAdapter.FnEnumOutputs)VTable.Get(adapter, 7, typeof(DXGIAdapter.FnEnumOutputs)))(adapter, index, out output);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007073 File Offset: 0x00005273
		public static int GetDesc(IntPtr adapter, out DXGI.DXGI_ADAPTER_DESC desc)
		{
			return ((DXGIAdapter.FnGetDesc)VTable.Get(adapter, 8, typeof(DXGIAdapter.FnGetDesc)))(adapter, out desc);
		}

		// Token: 0x0400014A RID: 330
		private const int Slot_EnumOutputs = 7;

		// Token: 0x0400014B RID: 331
		private const int Slot_GetDesc = 8;

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x06000229 RID: 553
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnEnumOutputs(IntPtr self, uint Output, out IntPtr ppOutput);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x0600022D RID: 557
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnGetDesc(IntPtr self, out DXGI.DXGI_ADAPTER_DESC pDesc);
	}
}
