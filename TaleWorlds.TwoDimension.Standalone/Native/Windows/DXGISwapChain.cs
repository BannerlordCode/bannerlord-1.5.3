using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200002F RID: 47
	public static class DXGISwapChain
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00006FA0 File Offset: 0x000051A0
		public static int Present(IntPtr swapChain, uint syncInterval, uint flags)
		{
			return ((DXGISwapChain.FnPresent)VTable.Get(swapChain, 8, typeof(DXGISwapChain.FnPresent)))(swapChain, syncInterval, flags);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006FC0 File Offset: 0x000051C0
		public static int GetBuffer(IntPtr swapChain, ref Guid riid, out IntPtr surface)
		{
			return ((DXGISwapChain.FnGetBuffer)VTable.Get(swapChain, 9, typeof(DXGISwapChain.FnGetBuffer)))(swapChain, 0U, ref riid, out surface);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006FE2 File Offset: 0x000051E2
		public static int ResizeBuffers(IntPtr swapChain, uint width, uint height)
		{
			return ((DXGISwapChain.FnResizeBuffers)VTable.Get(swapChain, 13, typeof(DXGISwapChain.FnResizeBuffers)))(swapChain, 0U, width, height, 0U, 0U);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007006 File Offset: 0x00005206
		public static void Release(IntPtr comObj)
		{
			if (comObj == IntPtr.Zero)
			{
				return;
			}
			((DXGISwapChain.FnRelease)VTable.Get(comObj, 2, typeof(DXGISwapChain.FnRelease)))(comObj);
		}

		// Token: 0x04000145 RID: 325
		private const int Slot_Present = 8;

		// Token: 0x04000146 RID: 326
		private const int Slot_GetBuffer = 9;

		// Token: 0x04000147 RID: 327
		private const int Slot_ResizeBuffers = 13;

		// Token: 0x04000148 RID: 328
		private const int Slot_Release = 2;

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x06000215 RID: 533
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnPresent(IntPtr self, uint SyncInterval, uint Flags);

		// Token: 0x02000074 RID: 116
		// (Invoke) Token: 0x06000219 RID: 537
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnGetBuffer(IntPtr self, uint Buffer, ref Guid riid, out IntPtr ppSurface);

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x0600021D RID: 541
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnResizeBuffers(IntPtr self, uint BufferCount, uint Width, uint Height, uint NewFormat, uint SwapChainFlags);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x06000221 RID: 545
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate uint FnRelease(IntPtr self);
	}
}
