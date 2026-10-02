using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000032 RID: 50
	public static class DXGIOutput
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00007092 File Offset: 0x00005292
		public static int GetDesc(IntPtr output, out DXGI.DXGI_OUTPUT_DESC desc)
		{
			return ((DXGIOutput.FnGetDesc)VTable.Get(output, 7, typeof(DXGIOutput.FnGetDesc)))(output, out desc);
		}

		// Token: 0x0400014C RID: 332
		private const int Slot_GetDesc = 7;

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x06000231 RID: 561
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnGetDesc(IntPtr self, out DXGI.DXGI_OUTPUT_DESC pDesc);
	}
}
