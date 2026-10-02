using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000034 RID: 52
	public static class ComRelease
	{
		// Token: 0x0600013A RID: 314 RVA: 0x000070EC File Offset: 0x000052EC
		public static void Release(IntPtr comObj)
		{
			if (comObj == IntPtr.Zero)
			{
				return;
			}
			((ComRelease.FnRelease)Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(Marshal.ReadIntPtr(comObj), 2 * IntPtr.Size), typeof(ComRelease.FnRelease)))(comObj);
		}

		// Token: 0x0400014E RID: 334
		private const int Slot_Release = 2;

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x06000239 RID: 569
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate uint FnRelease(IntPtr self);
	}
}
