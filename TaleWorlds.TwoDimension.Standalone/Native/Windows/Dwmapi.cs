using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000035 RID: 53
	internal static class Dwmapi
	{
		// Token: 0x0600013B RID: 315
		[DllImport("Dwmapi.dll")]
		public static extern IntPtr DwmEnableBlurBehindWindow(IntPtr hwnd, [In] ref DwmBlurBehind ppfd);
	}
}
