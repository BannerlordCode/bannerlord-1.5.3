using System;
using System.IO;
using System.Runtime.InteropServices;
using StbSharp;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000006 RID: 6
	public class DirectXTexture : ITexture, IDisposable
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003F77 File Offset: 0x00002177
		public bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003F7A File Offset: 0x0000217A
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003F82 File Offset: 0x00002182
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003F8A File Offset: 0x0000218A
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003F92 File Offset: 0x00002192
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003F9B File Offset: 0x0000219B
		public IntPtr ShaderResourceView
		{
			get
			{
				return this._srv;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003FA3 File Offset: 0x000021A3
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003FAB File Offset: 0x000021AB
		public bool ClampToEdge { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00003FBC File Offset: 0x000021BC
		public void LoadFromFile(IntPtr device, ResourceDepot resourceDepot, string name)
		{
			string filePath = resourceDepot.GetFilePath(name + ".png");
			this.LoadFromFile(device, filePath);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003FE4 File Offset: 0x000021E4
		public void LoadFromFile(IntPtr device, string fullPathName)
		{
			this._device = device;
			if (!File.Exists(fullPathName))
			{
				return;
			}
			Image image = null;
			using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(fullPathName)))
			{
				image = new ImageReader().Read(memoryStream, 0);
			}
			if (image == null)
			{
				return;
			}
			this._width = image.Width;
			this._height = image.Height;
			this._name = Path.GetFileName(fullPathName);
			uint num;
			byte[] array;
			uint num2;
			switch (image.Comp)
			{
			case 1:
				num = 61U;
				array = image.Data;
				num2 = (uint)this._width;
				break;
			case 2:
				return;
			case 3:
				num = 28U;
				array = DirectXTexture.ExpandRGBToRGBA(image.Data, this._width, this._height);
				num2 = (uint)(this._width * 4);
				break;
			case 4:
				num = 28U;
				array = image.Data;
				num2 = (uint)(this._width * 4);
				break;
			default:
				return;
			}
			D3D11_TEXTURE2D_DESC d3D11_TEXTURE2D_DESC = new D3D11_TEXTURE2D_DESC
			{
				Width = (uint)this._width,
				Height = (uint)this._height,
				MipLevels = 1U,
				ArraySize = 1U,
				Format = num,
				SampleDesc = new DXGI_SAMPLE_DESC
				{
					Count = 1U,
					Quality = 0U
				},
				Usage = 0U,
				BindFlags = 8U,
				CPUAccessFlags = 0U,
				MiscFlags = 0U
			};
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				D3D11_SUBRESOURCE_DATA d3D11_SUBRESOURCE_DATA = new D3D11_SUBRESOURCE_DATA
				{
					pSysMem = gchandle.AddrOfPinnedObject(),
					SysMemPitch = num2,
					SysMemSlicePitch = 0U
				};
				if (D3D11Device.CreateTexture2D(device, ref d3D11_TEXTURE2D_DESC, ref d3D11_SUBRESOURCE_DATA, out this._texture) < 0)
				{
					return;
				}
			}
			finally
			{
				gchandle.Free();
			}
			if (D3D11Device.CreateShaderResourceView(device, this._texture, out this._srv) < 0)
			{
				ComRelease.Release(this._texture);
				this._texture = IntPtr.Zero;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000041E0 File Offset: 0x000023E0
		public void CopyFrom(DirectXTexture other)
		{
			ComRelease.Release(this._srv);
			this._srv = IntPtr.Zero;
			ComRelease.Release(this._texture);
			this._texture = IntPtr.Zero;
			this._width = other._width;
			this._height = other._height;
			this._name = other._name;
			this._device = other._device;
			this._texture = other._texture;
			this._srv = other._srv;
			if (this._texture != IntPtr.Zero)
			{
				ComAddRef.AddRef(this._texture);
			}
			if (this._srv != IntPtr.Zero)
			{
				ComAddRef.AddRef(this._srv);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000429C File Offset: 0x0000249C
		public static DirectXTexture FromFile(IntPtr device, ResourceDepot resourceDepot, string name)
		{
			DirectXTexture directXTexture = new DirectXTexture();
			directXTexture.LoadFromFile(device, resourceDepot, name);
			if (!directXTexture.IsLoaded())
			{
				return null;
			}
			return directXTexture;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000042C4 File Offset: 0x000024C4
		public static DirectXTexture FromFile(string fullPath)
		{
			DirectXGraphicsContext active = DirectXGraphicsContext.Active;
			if (active == null)
			{
				return null;
			}
			DirectXTexture directXTexture = new DirectXTexture();
			directXTexture.LoadFromFile(active.DeviceHandle, fullPath);
			if (!directXTexture.IsLoaded())
			{
				return null;
			}
			return directXTexture;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000042FA File Offset: 0x000024FA
		public bool IsLoaded()
		{
			return this._srv != IntPtr.Zero;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000430C File Offset: 0x0000250C
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004314 File Offset: 0x00002514
		public void Dispose()
		{
			ComRelease.Release(this._srv);
			this._srv = IntPtr.Zero;
			ComRelease.Release(this._texture);
			this._texture = IntPtr.Zero;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004344 File Offset: 0x00002544
		private static byte[] ExpandRGBToRGBA(byte[] rgb, int width, int height)
		{
			int num = width * height;
			byte[] array = new byte[num * 4];
			for (int i = 0; i < num; i++)
			{
				array[i * 4] = rgb[i * 3];
				array[i * 4 + 1] = rgb[i * 3 + 1];
				array[i * 4 + 2] = rgb[i * 3 + 2];
				array[i * 4 + 3] = byte.MaxValue;
			}
			return array;
		}

		// Token: 0x04000027 RID: 39
		private int _width;

		// Token: 0x04000028 RID: 40
		private int _height;

		// Token: 0x04000029 RID: 41
		private string _name;

		// Token: 0x0400002A RID: 42
		private IntPtr _texture;

		// Token: 0x0400002B RID: 43
		private IntPtr _srv;

		// Token: 0x0400002C RID: 44
		private IntPtr _device;
	}
}
