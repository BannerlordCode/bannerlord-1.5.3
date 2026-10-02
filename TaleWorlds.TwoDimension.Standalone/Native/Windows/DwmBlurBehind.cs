using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000036 RID: 54
	internal struct DwmBlurBehind
	{
		// Token: 0x0400014F RID: 335
		public BlurBehindConstraints dwFlags;

		// Token: 0x04000150 RID: 336
		[MarshalAs(UnmanagedType.Bool)]
		public bool fEnable;

		// Token: 0x04000151 RID: 337
		public IntPtr hRgnBlur;

		// Token: 0x04000152 RID: 338
		[MarshalAs(UnmanagedType.Bool)]
		public bool fTransitionOnMaximized;
	}
}
