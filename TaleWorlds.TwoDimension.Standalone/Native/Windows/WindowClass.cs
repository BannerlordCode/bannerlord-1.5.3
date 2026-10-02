using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000044 RID: 68
	public struct WindowClass
	{
		// Token: 0x04000196 RID: 406
		public uint style;

		// Token: 0x04000197 RID: 407
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WndProc lpfnWndProc;

		// Token: 0x04000198 RID: 408
		public int cbClsExtra;

		// Token: 0x04000199 RID: 409
		public int cbWndExtra;

		// Token: 0x0400019A RID: 410
		public IntPtr hInstance;

		// Token: 0x0400019B RID: 411
		public IntPtr hIcon;

		// Token: 0x0400019C RID: 412
		public IntPtr hCursor;

		// Token: 0x0400019D RID: 413
		public IntPtr hbrBackground;

		// Token: 0x0400019E RID: 414
		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpszMenuName;

		// Token: 0x0400019F RID: 415
		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpszClassName;
	}
}
