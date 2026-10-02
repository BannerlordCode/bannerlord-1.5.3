using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200003A RID: 58
	internal static class Gdi32
	{
		// Token: 0x0600013F RID: 319
		[DllImport("gdi32.dll")]
		public static extern int ChoosePixelFormat(IntPtr hdc, [In] ref PixelFormatDescriptor ppfd);

		// Token: 0x06000140 RID: 320
		[DllImport("gdi32.dll")]
		public static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PixelFormatDescriptor ppfd);

		// Token: 0x06000141 RID: 321
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SwapBuffers(IntPtr hdc);

		// Token: 0x06000142 RID: 322
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x06000143 RID: 323
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(IntPtr colorRef);

		// Token: 0x06000144 RID: 324
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		// Token: 0x06000145 RID: 325
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

		// Token: 0x06000146 RID: 326
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr ho);

		// Token: 0x06000147 RID: 327
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteDC(IntPtr hdc);

		// Token: 0x06000148 RID: 328
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int cx, int cy);

		// Token: 0x06000149 RID: 329
		[DllImport("gdi32.dll")]
		public static extern int StretchDIBits(IntPtr hdc, int xDest, int yDest, int DestWidth, int DestHeight, int xSrc, int ySrc, int SrcWidth, int SrcHeight, byte[] lpBits, ref BitmapInfo lpbmi, uint iUsage, int rop);

		// Token: 0x0600014A RID: 330
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BitmapInfo pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint offset);
	}
}
