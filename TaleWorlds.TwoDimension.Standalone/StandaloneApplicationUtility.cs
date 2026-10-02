using System;
using System.Runtime.InteropServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000E RID: 14
	internal static class StandaloneApplicationUtility
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00005DDC File Offset: 0x00003FDC
		public static void TerminateWithMessageBox(string title, string message)
		{
			Debug.ShowMessageBox(message, title, 1U);
			Marshal.WriteInt32(IntPtr.Zero, 0);
		}
	}
}
