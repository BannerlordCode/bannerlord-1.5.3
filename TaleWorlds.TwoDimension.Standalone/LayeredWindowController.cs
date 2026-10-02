using System;
using System.Drawing;
using System.Runtime.InteropServices;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000C RID: 12
	public class LayeredWindowController
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00005488 File Offset: 0x00003688
		public LayeredWindowController(IntPtr windowHandle, int width, int height, DirectXGraphicsContext context)
		{
			this._windowHandle = windowHandle;
			this._context = context;
			User32.SetWindowLong(this._windowHandle, -20, 524288U);
			this._screenDC = User32.GetDC(IntPtr.Zero);
			this._memoryDC = Gdi32.CreateCompatibleDC(this._screenDC);
			this.SetSize(width, height);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005500 File Offset: 0x00003700
		public void SetSize(int width, int height)
		{
			if (width <= 0 || height <= 0)
			{
				return;
			}
			if (width == this._width && height == this._height)
			{
				return;
			}
			this._width = width;
			this._height = height;
			this.ReleaseDibResources();
			this.ReleaseStaging();
			this._rowBuffer = new byte[this._width * 4];
			this.CreateStagingTexture();
			this.CreateDib();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005564 File Offset: 0x00003764
		private void CreateStagingTexture()
		{
			if (this._context == null || this._context.DeviceHandle == IntPtr.Zero)
			{
				return;
			}
			D3D11_TEXTURE2D_DESC d3D11_TEXTURE2D_DESC = new D3D11_TEXTURE2D_DESC
			{
				Width = (uint)this._width,
				Height = (uint)this._height,
				MipLevels = 1U,
				ArraySize = 1U,
				Format = 87U,
				SampleDesc = new DXGI_SAMPLE_DESC
				{
					Count = 1U,
					Quality = 0U
				},
				Usage = 3U,
				BindFlags = 0U,
				CPUAccessFlags = 131072U,
				MiscFlags = 0U
			};
			if (D3D11Device.CreateTexture2DEmpty(this._context.DeviceHandle, ref d3D11_TEXTURE2D_DESC, out this._stagingTexture) < 0)
			{
				this._stagingTexture = IntPtr.Zero;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005638 File Offset: 0x00003838
		private void ReleaseStaging()
		{
			if (this._stagingTexture != IntPtr.Zero)
			{
				ComRelease.Release(this._stagingTexture);
				this._stagingTexture = IntPtr.Zero;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005664 File Offset: 0x00003864
		private void CreateDib()
		{
			BitmapInfo bitmapInfo = default(BitmapInfo);
			bitmapInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BitmapInfoHeader));
			bitmapInfo.bmiHeader.biWidth = this._width;
			bitmapInfo.bmiHeader.biHeight = -this._height;
			bitmapInfo.bmiHeader.biPlanes = 1;
			bitmapInfo.bmiHeader.biBitCount = 32;
			bitmapInfo.bmiHeader.biCompression = 0U;
			bitmapInfo.bmiHeader.biSizeImage = 0U;
			bitmapInfo.bmiHeader.biXPelsPerMeter = 0;
			bitmapInfo.bmiHeader.biYPelsPerMeter = 0;
			bitmapInfo.bmiHeader.biClrUsed = 0U;
			bitmapInfo.bmiHeader.biClrImportant = 0U;
			bitmapInfo.r = 0;
			bitmapInfo.g = 0;
			bitmapInfo.b = 0;
			bitmapInfo.a = 0;
			this._hDib = Gdi32.CreateDIBSection(this._screenDC, ref bitmapInfo, 0U, out this._dibBits, IntPtr.Zero, 0U);
			if (this._hDib == IntPtr.Zero)
			{
				this._dibBits = IntPtr.Zero;
				return;
			}
			this._hOldBitmap = Gdi32.SelectObject(this._memoryDC, this._hDib);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005798 File Offset: 0x00003998
		private void ReleaseDibResources()
		{
			if (this._hDib != IntPtr.Zero)
			{
				if (this._hOldBitmap != IntPtr.Zero)
				{
					Gdi32.SelectObject(this._memoryDC, this._hOldBitmap);
					this._hOldBitmap = IntPtr.Zero;
				}
				Gdi32.DeleteObject(this._hDib);
				this._hDib = IntPtr.Zero;
				this._dibBits = IntPtr.Zero;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005808 File Offset: 0x00003A08
		public void PostRender()
		{
			if (this._width <= 0 || this._height <= 0)
			{
				return;
			}
			if (this._context == null || this._stagingTexture == IntPtr.Zero)
			{
				return;
			}
			if (this._context.DeviceContextHandle == IntPtr.Zero)
			{
				return;
			}
			if (this._context.IsDeviceLost)
			{
				return;
			}
			if (this._hDib == IntPtr.Zero || this._dibBits == IntPtr.Zero)
			{
				return;
			}
			IntPtr currentBackBuffer = this._context.GetCurrentBackBuffer();
			if (currentBackBuffer == IntPtr.Zero)
			{
				return;
			}
			D3D11Context.CopyResource(this._context.DeviceContextHandle, this._stagingTexture, currentBackBuffer);
			ComRelease.Release(currentBackBuffer);
			D3D11_MAPPED_SUBRESOURCE d3D11_MAPPED_SUBRESOURCE;
			int num = D3D11Context.Map(this._context.DeviceContextHandle, this._stagingTexture, 1U, out d3D11_MAPPED_SUBRESOURCE);
			if (num < 0)
			{
				this._context.ReportDeviceLost(num);
				return;
			}
			try
			{
				int num2 = this._width * 4;
				for (int i = 0; i < this._height; i++)
				{
					IntPtr intPtr = d3D11_MAPPED_SUBRESOURCE.pData + i * (int)d3D11_MAPPED_SUBRESOURCE.RowPitch;
					IntPtr intPtr2 = this._dibBits + i * num2;
					Marshal.Copy(intPtr, this._rowBuffer, 0, num2);
					Marshal.Copy(this._rowBuffer, 0, intPtr2, num2);
				}
			}
			finally
			{
				D3D11Context.Unmap(this._context.DeviceContextHandle, this._stagingTexture);
			}
			Rectangle rectangle;
			User32.GetWindowRect(this._windowHandle, out rectangle);
			Point point = new Point(rectangle.Left, rectangle.Top);
			Size size = new Size(this._width, this._height);
			User32.UpdateLayeredWindow(this._windowHandle, this._screenDC, ref point, ref size, this._memoryDC, ref this._localOriginPoint, 0, ref this._blendFunction, 2);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000059E0 File Offset: 0x00003BE0
		public void OnFinalize()
		{
			this.ReleaseDibResources();
			this.ReleaseStaging();
			User32.ReleaseDC(IntPtr.Zero, this._screenDC);
			Gdi32.DeleteDC(this._memoryDC);
		}

		// Token: 0x04000048 RID: 72
		private const int GwlExStyle = -20;

		// Token: 0x04000049 RID: 73
		private const uint WsExLayered = 524288U;

		// Token: 0x0400004A RID: 74
		private readonly IntPtr _windowHandle;

		// Token: 0x0400004B RID: 75
		private readonly IntPtr _screenDC;

		// Token: 0x0400004C RID: 76
		private readonly IntPtr _memoryDC;

		// Token: 0x0400004D RID: 77
		private DirectXGraphicsContext _context;

		// Token: 0x0400004E RID: 78
		private IntPtr _stagingTexture;

		// Token: 0x0400004F RID: 79
		private IntPtr _hDib;

		// Token: 0x04000050 RID: 80
		private IntPtr _dibBits;

		// Token: 0x04000051 RID: 81
		private IntPtr _hOldBitmap;

		// Token: 0x04000052 RID: 82
		private int _width;

		// Token: 0x04000053 RID: 83
		private int _height;

		// Token: 0x04000054 RID: 84
		private byte[] _rowBuffer;

		// Token: 0x04000055 RID: 85
		private BlendFunction _blendFunction = BlendFunction.Default;

		// Token: 0x04000056 RID: 86
		private Point _localOriginPoint = new Point(0, 0);
	}
}
