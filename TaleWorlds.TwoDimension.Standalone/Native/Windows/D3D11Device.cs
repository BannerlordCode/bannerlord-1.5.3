using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200002D RID: 45
	public static class D3D11Device
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00006A32 File Offset: 0x00004C32
		public static int CreateBuffer(IntPtr device, ref D3D11_BUFFER_DESC desc, out IntPtr buffer)
		{
			return ((D3D11Device.FnCreateBuffer)VTable.Get(device, 3, typeof(D3D11Device.FnCreateBuffer)))(device, ref desc, IntPtr.Zero, out buffer);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006A57 File Offset: 0x00004C57
		public static int CreateTexture2D(IntPtr device, ref D3D11_TEXTURE2D_DESC desc, ref D3D11_SUBRESOURCE_DATA initialData, out IntPtr texture)
		{
			return ((D3D11Device.FnCreateTexture2D)VTable.Get(device, 5, typeof(D3D11Device.FnCreateTexture2D)))(device, ref desc, ref initialData, out texture);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006A78 File Offset: 0x00004C78
		public static int CreateTexture2DEmpty(IntPtr device, ref D3D11_TEXTURE2D_DESC desc, out IntPtr texture)
		{
			return ((D3D11Device.FnCreateTexture2DNoData)VTable.Get(device, 5, typeof(D3D11Device.FnCreateTexture2DNoData)))(device, ref desc, IntPtr.Zero, out texture);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006A9D File Offset: 0x00004C9D
		public static int CreateShaderResourceView(IntPtr device, IntPtr resource, out IntPtr srv)
		{
			return ((D3D11Device.FnCreateShaderResourceView)VTable.Get(device, 7, typeof(D3D11Device.FnCreateShaderResourceView)))(device, resource, IntPtr.Zero, out srv);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006AC2 File Offset: 0x00004CC2
		public static int CreateRenderTargetView(IntPtr device, IntPtr resource, out IntPtr rtv)
		{
			return ((D3D11Device.FnCreateRenderTargetView)VTable.Get(device, 9, typeof(D3D11Device.FnCreateRenderTargetView)))(device, resource, IntPtr.Zero, out rtv);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006AE8 File Offset: 0x00004CE8
		public static int CreateVertexShader(IntPtr device, IntPtr bytecode, int bytecodeLen, out IntPtr vs)
		{
			return ((D3D11Device.FnCreateVertexShader)VTable.Get(device, 12, typeof(D3D11Device.FnCreateVertexShader)))(device, bytecode, (UIntPtr)((ulong)((long)bytecodeLen)), IntPtr.Zero, out vs);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006B15 File Offset: 0x00004D15
		public static int CreatePixelShader(IntPtr device, IntPtr bytecode, int bytecodeLen, out IntPtr ps)
		{
			return ((D3D11Device.FnCreatePixelShader)VTable.Get(device, 15, typeof(D3D11Device.FnCreatePixelShader)))(device, bytecode, (UIntPtr)((ulong)((long)bytecodeLen)), IntPtr.Zero, out ps);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006B42 File Offset: 0x00004D42
		public static int CreateInputLayout(IntPtr device, D3D11_INPUT_ELEMENT_DESC[] elements, IntPtr vsBytecode, int vsLen, out IntPtr inputLayout)
		{
			return ((D3D11Device.FnCreateInputLayout)VTable.Get(device, 11, typeof(D3D11Device.FnCreateInputLayout)))(device, elements, (uint)elements.Length, vsBytecode, (UIntPtr)((ulong)((long)vsLen)), out inputLayout);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006B6F File Offset: 0x00004D6F
		public static int CreateBlendState(IntPtr device, ref D3D11_BLEND_DESC desc, out IntPtr blendState)
		{
			return ((D3D11Device.FnCreateBlendState)VTable.Get(device, 20, typeof(D3D11Device.FnCreateBlendState)))(device, ref desc, out blendState);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006B90 File Offset: 0x00004D90
		public static int CreateRasterizerState(IntPtr device, ref D3D11_RASTERIZER_DESC desc, out IntPtr rasterizerState)
		{
			return ((D3D11Device.FnCreateRasterizerState)VTable.Get(device, 22, typeof(D3D11Device.FnCreateRasterizerState)))(device, ref desc, out rasterizerState);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006BB1 File Offset: 0x00004DB1
		public static int CreateSamplerState(IntPtr device, ref D3D11_SAMPLER_DESC desc, out IntPtr samplerState)
		{
			return ((D3D11Device.FnCreateSamplerState)VTable.Get(device, 23, typeof(D3D11Device.FnCreateSamplerState)))(device, ref desc, out samplerState);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006BD2 File Offset: 0x00004DD2
		public static int GetDeviceRemovedReason(IntPtr device)
		{
			return ((D3D11Device.FnGetDeviceRemovedReason)VTable.Get(device, 40, typeof(D3D11Device.FnGetDeviceRemovedReason)))(device);
		}

		// Token: 0x04000118 RID: 280
		private const int Slot_CreateBuffer = 3;

		// Token: 0x04000119 RID: 281
		private const int Slot_CreateTexture1D = 4;

		// Token: 0x0400011A RID: 282
		private const int Slot_CreateTexture2D = 5;

		// Token: 0x0400011B RID: 283
		private const int Slot_CreateTexture3D = 6;

		// Token: 0x0400011C RID: 284
		private const int Slot_CreateShaderResourceView = 7;

		// Token: 0x0400011D RID: 285
		private const int Slot_CreateUnorderedAccessView = 8;

		// Token: 0x0400011E RID: 286
		private const int Slot_CreateRenderTargetView = 9;

		// Token: 0x0400011F RID: 287
		private const int Slot_CreateDepthStencilView = 10;

		// Token: 0x04000120 RID: 288
		private const int Slot_CreateInputLayout = 11;

		// Token: 0x04000121 RID: 289
		private const int Slot_CreateVertexShader = 12;

		// Token: 0x04000122 RID: 290
		private const int Slot_CreateGeometryShader = 13;

		// Token: 0x04000123 RID: 291
		private const int Slot_CreateGSWithSO = 14;

		// Token: 0x04000124 RID: 292
		private const int Slot_CreatePixelShader = 15;

		// Token: 0x04000125 RID: 293
		private const int Slot_CreateHullShader = 16;

		// Token: 0x04000126 RID: 294
		private const int Slot_CreateDomainShader = 17;

		// Token: 0x04000127 RID: 295
		private const int Slot_CreateComputeShader = 18;

		// Token: 0x04000128 RID: 296
		private const int Slot_CreateClassLinkage = 19;

		// Token: 0x04000129 RID: 297
		private const int Slot_CreateBlendState = 20;

		// Token: 0x0400012A RID: 298
		private const int Slot_CreateDepthStencilState = 21;

		// Token: 0x0400012B RID: 299
		private const int Slot_CreateRasterizerState = 22;

		// Token: 0x0400012C RID: 300
		private const int Slot_CreateSamplerState = 23;

		// Token: 0x0400012D RID: 301
		private const int Slot_GetDeviceRemovedReason = 40;

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x06000189 RID: 393
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateBuffer(IntPtr self, ref D3D11_BUFFER_DESC pDesc, IntPtr pInitialData, out IntPtr ppBuffer);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x0600018D RID: 397
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateTexture2D(IntPtr self, ref D3D11_TEXTURE2D_DESC pDesc, ref D3D11_SUBRESOURCE_DATA pInitialData, out IntPtr ppTexture2D);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x06000191 RID: 401
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateTexture2DNoData(IntPtr self, ref D3D11_TEXTURE2D_DESC pDesc, IntPtr pInitialData, out IntPtr ppTexture2D);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x06000195 RID: 405
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateShaderResourceView(IntPtr self, IntPtr pResource, IntPtr pDesc, out IntPtr ppSRV);

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x06000199 RID: 409
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateRenderTargetView(IntPtr self, IntPtr pResource, IntPtr pDesc, out IntPtr ppRTV);

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x0600019D RID: 413
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateVertexShader(IntPtr self, IntPtr pShaderBytecode, UIntPtr bytecodeLength, IntPtr pClassLinkage, out IntPtr ppVertexShader);

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x060001A1 RID: 417
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreatePixelShader(IntPtr self, IntPtr pShaderBytecode, UIntPtr bytecodeLength, IntPtr pClassLinkage, out IntPtr ppPixelShader);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x060001A5 RID: 421
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateInputLayout(IntPtr self, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] D3D11_INPUT_ELEMENT_DESC[] pInputElementDescs, uint numElements, IntPtr pShaderBytecodeWithInputSignature, UIntPtr bytecodeLength, out IntPtr ppInputLayout);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x060001A9 RID: 425
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateBlendState(IntPtr self, ref D3D11_BLEND_DESC pBlendStateDesc, out IntPtr ppBlendState);

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x060001AD RID: 429
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateRasterizerState(IntPtr self, ref D3D11_RASTERIZER_DESC pRasterizerDesc, out IntPtr ppRasterizerState);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x060001B1 RID: 433
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnCreateSamplerState(IntPtr self, ref D3D11_SAMPLER_DESC pSamplerDesc, out IntPtr ppSamplerState);

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x060001B5 RID: 437
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnGetDeviceRemovedReason(IntPtr self);
	}
}
