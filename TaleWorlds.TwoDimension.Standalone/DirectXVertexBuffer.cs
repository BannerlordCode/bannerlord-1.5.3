using System;
using System.Runtime.InteropServices;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000007 RID: 7
	public class DirectXVertexBuffer : IDisposable
	{
		// Token: 0x0600004C RID: 76 RVA: 0x0000439D File Offset: 0x0000259D
		private DirectXVertexBuffer()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000043BC File Offset: 0x000025BC
		public static DirectXVertexBuffer Create(IntPtr device)
		{
			DirectXVertexBuffer directXVertexBuffer = new DirectXVertexBuffer();
			directXVertexBuffer._device = device;
			if (!directXVertexBuffer.CreateVertexBuffer(131072) || !directXVertexBuffer.CreateIndexBuffer(131072))
			{
				return null;
			}
			return directXVertexBuffer;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000043F4 File Offset: 0x000025F4
		private bool CreateVertexBuffer(int floatCapacity)
		{
			ComRelease.Release(this._vertexBuffer);
			this._vertexBuffer = IntPtr.Zero;
			D3D11_BUFFER_DESC d3D11_BUFFER_DESC = new D3D11_BUFFER_DESC
			{
				ByteWidth = (uint)(floatCapacity * 4),
				Usage = 2U,
				BindFlags = 1U,
				CPUAccessFlags = 65536U,
				MiscFlags = 0U,
				StructureByteStride = 0U
			};
			if (D3D11Device.CreateBuffer(this._device, ref d3D11_BUFFER_DESC, out this._vertexBuffer) < 0)
			{
				return false;
			}
			this._floatCapacity = floatCapacity;
			return true;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004478 File Offset: 0x00002678
		private bool CreateIndexBuffer(int uintCapacity)
		{
			ComRelease.Release(this._indexBuffer);
			this._indexBuffer = IntPtr.Zero;
			D3D11_BUFFER_DESC d3D11_BUFFER_DESC = new D3D11_BUFFER_DESC
			{
				ByteWidth = (uint)(uintCapacity * 4),
				Usage = 2U,
				BindFlags = 2U,
				CPUAccessFlags = 65536U,
				MiscFlags = 0U,
				StructureByteStride = 0U
			};
			if (D3D11Device.CreateBuffer(this._device, ref d3D11_BUFFER_DESC, out this._indexBuffer) < 0)
			{
				return false;
			}
			this._uintCapacity = uintCapacity;
			return true;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000044FC File Offset: 0x000026FC
		public void LoadVertexData(IntPtr context, float[] interleavedData)
		{
			if (interleavedData == null || interleavedData.Length == 0)
			{
				return;
			}
			if (interleavedData.Length > this._floatCapacity)
			{
				int i;
				for (i = this._floatCapacity; i < interleavedData.Length; i *= 2)
				{
				}
				Debug.Print(string.Format("[LAUNCHER]: Growing vertex buffer from {0} to {1} floats.", this._floatCapacity, i), 0, Debug.DebugColor.White, 17592186044416UL);
				if (!this.CreateVertexBuffer(i))
				{
					return;
				}
			}
			D3D11_MAPPED_SUBRESOURCE d3D11_MAPPED_SUBRESOURCE;
			if (D3D11Context.Map(context, this._vertexBuffer, 4U, out d3D11_MAPPED_SUBRESOURCE) < 0)
			{
				return;
			}
			try
			{
				Marshal.Copy(interleavedData, 0, d3D11_MAPPED_SUBRESOURCE.pData, interleavedData.Length);
			}
			finally
			{
				D3D11Context.Unmap(context, this._vertexBuffer);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000045A8 File Offset: 0x000027A8
		public void LoadIndexData(IntPtr context, uint[] indices)
		{
			if (indices == null || indices.Length == 0)
			{
				return;
			}
			if (indices.Length > this._uintCapacity)
			{
				int i;
				for (i = this._uintCapacity; i < indices.Length; i *= 2)
				{
				}
				Debug.Print(string.Format("[LAUNCHER]: Growing index buffer from {0} to {1} uints.", this._uintCapacity, i), 0, Debug.DebugColor.White, 17592186044416UL);
				if (!this.CreateIndexBuffer(i))
				{
					return;
				}
			}
			D3D11_MAPPED_SUBRESOURCE d3D11_MAPPED_SUBRESOURCE;
			if (D3D11Context.Map(context, this._indexBuffer, 4U, out d3D11_MAPPED_SUBRESOURCE) < 0)
			{
				return;
			}
			int[] array = new int[indices.Length];
			for (int j = 0; j < indices.Length; j++)
			{
				array[j] = (int)indices[j];
			}
			try
			{
				Marshal.Copy(array, 0, d3D11_MAPPED_SUBRESOURCE.pData, array.Length);
			}
			finally
			{
				D3D11Context.Unmap(context, this._indexBuffer);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004674 File Offset: 0x00002874
		public void Bind(IntPtr context)
		{
			D3D11Context.IASetVertexBuffers(context, this._vertexBuffer, 16U);
			D3D11Context.IASetIndexBuffer(context, this._indexBuffer);
			D3D11Context.IASetPrimitiveTopology(context);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004696 File Offset: 0x00002896
		public void Dispose()
		{
			ComRelease.Release(this._vertexBuffer);
			this._vertexBuffer = IntPtr.Zero;
			ComRelease.Release(this._indexBuffer);
			this._indexBuffer = IntPtr.Zero;
		}

		// Token: 0x0400002E RID: 46
		private int _floatCapacity = 131072;

		// Token: 0x0400002F RID: 47
		private int _uintCapacity = 131072;

		// Token: 0x04000030 RID: 48
		private IntPtr _device;

		// Token: 0x04000031 RID: 49
		private IntPtr _vertexBuffer;

		// Token: 0x04000032 RID: 50
		private IntPtr _indexBuffer;

		// Token: 0x04000033 RID: 51
		public const uint Stride = 16U;
	}
}
