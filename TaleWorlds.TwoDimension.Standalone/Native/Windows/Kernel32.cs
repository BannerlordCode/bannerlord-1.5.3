using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200003B RID: 59
	public static class Kernel32
	{
		// Token: 0x0600014B RID: 331
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x0600014C RID: 332
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x0600014D RID: 333
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetLastError();

		// Token: 0x0600014E RID: 334
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern IntPtr GetConsoleWindow();

		// Token: 0x0600014F RID: 335
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		public static extern int GetUserGeoID(Kernel32.GeoTypeId type);

		// Token: 0x02000084 RID: 132
		public enum GeoTypeId
		{
			// Token: 0x04000215 RID: 533
			Nation = 16,
			// Token: 0x04000216 RID: 534
			Region = 14
		}
	}
}
