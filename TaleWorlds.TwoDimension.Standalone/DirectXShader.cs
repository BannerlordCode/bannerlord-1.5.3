using System;
using System.Runtime.InteropServices;
using System.Text;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000005 RID: 5
	public class DirectXShader : IDisposable
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003C5E File Offset: 0x00001E5E
		private DirectXShader()
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003C68 File Offset: 0x00001E68
		public static DirectXShader CreateShader(IntPtr device, string hlslSource, string shaderName)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr intPtr = IntPtr.Zero;
			byte[] bytes = Encoding.UTF8.GetBytes(hlslSource);
			GCHandle gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			DirectXShader directXShader;
			try
			{
				IntPtr intPtr2 = gchandle.AddrOfPinnedObject();
				IntPtr intPtr3 = (IntPtr)bytes.Length;
				if (D3DCompiler.D3DCompile(intPtr2, intPtr3, shaderName + ".hlsl", IntPtr.Zero, IntPtr.Zero, "VSMain", "vs_5_0", 0U, 0U, out zero, out intPtr) < 0)
				{
					D3DCompiler.GetErrorMessage(intPtr);
					directXShader = null;
				}
				else
				{
					if (intPtr != IntPtr.Zero)
					{
						ID3DBlob.Release(intPtr);
						intPtr = IntPtr.Zero;
					}
					if (D3DCompiler.D3DCompile(intPtr2, intPtr3, shaderName + ".hlsl", IntPtr.Zero, IntPtr.Zero, "PSMain", "ps_5_0", 0U, 0U, out zero2, out intPtr) < 0)
					{
						D3DCompiler.GetErrorMessage(intPtr);
						directXShader = null;
					}
					else
					{
						if (intPtr != IntPtr.Zero)
						{
							ID3DBlob.Release(intPtr);
							intPtr = IntPtr.Zero;
						}
						IntPtr bufferPointer = ID3DBlob.GetBufferPointer(zero);
						int bufferSize = ID3DBlob.GetBufferSize(zero);
						IntPtr bufferPointer2 = ID3DBlob.GetBufferPointer(zero2);
						int bufferSize2 = ID3DBlob.GetBufferSize(zero2);
						IntPtr intPtr4;
						IntPtr intPtr5;
						if (D3D11Device.CreateVertexShader(device, bufferPointer, bufferSize, out intPtr4) < 0)
						{
							directXShader = null;
						}
						else if (D3D11Device.CreatePixelShader(device, bufferPointer2, bufferSize2, out intPtr5) < 0)
						{
							ComRelease.Release(intPtr4);
							directXShader = null;
						}
						else
						{
							D3D11_INPUT_ELEMENT_DESC[] array = new D3D11_INPUT_ELEMENT_DESC[]
							{
								new D3D11_INPUT_ELEMENT_DESC
								{
									SemanticName = "POSITION",
									SemanticIndex = 0U,
									Format = 16U,
									InputSlot = 0U,
									AlignedByteOffset = 0U,
									InputSlotClass = 0U,
									InstanceDataStepRate = 0U
								},
								new D3D11_INPUT_ELEMENT_DESC
								{
									SemanticName = "TEXCOORD",
									SemanticIndex = 0U,
									Format = 16U,
									InputSlot = 0U,
									AlignedByteOffset = 8U,
									InputSlotClass = 0U,
									InstanceDataStepRate = 0U
								}
							};
							IntPtr intPtr6;
							if (D3D11Device.CreateInputLayout(device, array, bufferPointer, bufferSize, out intPtr6) < 0)
							{
								ComRelease.Release(intPtr4);
								ComRelease.Release(intPtr5);
								directXShader = null;
							}
							else
							{
								directXShader = new DirectXShader
								{
									_vertexShader = intPtr4,
									_pixelShader = intPtr5,
									_inputLayout = intPtr6
								};
							}
						}
					}
				}
			}
			finally
			{
				gchandle.Free();
				if (zero != IntPtr.Zero)
				{
					ID3DBlob.Release(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					ID3DBlob.Release(zero2);
				}
				if (intPtr != IntPtr.Zero)
				{
					ID3DBlob.Release(intPtr);
				}
			}
			return directXShader;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003F00 File Offset: 0x00002100
		public void Use(IntPtr context)
		{
			D3D11Context.VSSetShader(context, this._vertexShader);
			D3D11Context.PSSetShader(context, this._pixelShader);
			D3D11Context.IASetInputLayout(context, this._inputLayout);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003F28 File Offset: 0x00002128
		public void Dispose()
		{
			ComRelease.Release(this._vertexShader);
			this._vertexShader = IntPtr.Zero;
			ComRelease.Release(this._pixelShader);
			this._pixelShader = IntPtr.Zero;
			ComRelease.Release(this._inputLayout);
			this._inputLayout = IntPtr.Zero;
		}

		// Token: 0x04000024 RID: 36
		private IntPtr _vertexShader;

		// Token: 0x04000025 RID: 37
		private IntPtr _pixelShader;

		// Token: 0x04000026 RID: 38
		private IntPtr _inputLayout;
	}
}
