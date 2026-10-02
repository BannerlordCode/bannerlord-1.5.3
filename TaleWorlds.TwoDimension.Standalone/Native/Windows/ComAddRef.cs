using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000033 RID: 51
	public static class ComAddRef
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000070B1 File Offset: 0x000052B1
		public static void AddRef(IntPtr comObj)
		{
			if (comObj == IntPtr.Zero)
			{
				return;
			}
			((ComAddRef.FnAddRef)Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(Marshal.ReadIntPtr(comObj), IntPtr.Size), typeof(ComAddRef.FnAddRef)))(comObj);
		}

		// Token: 0x0400014D RID: 333
		private const int Slot_AddRef = 1;

		// Token: 0x0200007B RID: 123
		// (Invoke) Token: 0x06000235 RID: 565
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate uint FnAddRef(IntPtr self);
	}
}
