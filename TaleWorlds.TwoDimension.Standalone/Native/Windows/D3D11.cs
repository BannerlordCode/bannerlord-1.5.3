using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000038 RID: 56
	public static class D3D11
	{
		// Token: 0x0600013C RID: 316
		[DllImport("d3d11.dll")]
		public static extern int D3D11CreateDevice(DXGI.IDXGIAdapter adapter, D3D11.D3D_DRIVER_TYPE driverType, IntPtr software, uint flags, IntPtr featureLevels, int featureLevelCount, int sdkVersion, out IntPtr ppDevice, IntPtr pFeatureLevel, out IntPtr ppImmediateContext);

		// Token: 0x0200007D RID: 125
		public enum D3D_DRIVER_TYPE
		{
			// Token: 0x040001FC RID: 508
			UNKNOWN,
			// Token: 0x040001FD RID: 509
			HARDWARE,
			// Token: 0x040001FE RID: 510
			REFERENCE,
			// Token: 0x040001FF RID: 511
			NULL_DRIVER,
			// Token: 0x04000200 RID: 512
			SOFTWARE,
			// Token: 0x04000201 RID: 513
			WARP
		}
	}
}
