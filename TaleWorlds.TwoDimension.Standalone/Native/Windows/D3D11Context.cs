using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200002E RID: 46
	public static class D3D11Context
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00006BF1 File Offset: 0x00004DF1
		public static void ClearRenderTargetView(IntPtr ctx, IntPtr rtv, float[] color)
		{
			((D3D11Context.FnClearRenderTargetView)VTable.Get(ctx, 50, typeof(D3D11Context.FnClearRenderTargetView)))(ctx, rtv, color);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006C12 File Offset: 0x00004E12
		public static void OMSetRenderTargets(IntPtr ctx, IntPtr rtv)
		{
			((D3D11Context.FnOMSetRenderTargets)VTable.Get(ctx, 33, typeof(D3D11Context.FnOMSetRenderTargets)))(ctx, 1U, new IntPtr[] { rtv }, IntPtr.Zero);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006C41 File Offset: 0x00004E41
		public static void OMSetBlendState(IntPtr ctx, IntPtr blendState)
		{
			((D3D11Context.FnOMSetBlendState)VTable.Get(ctx, 35, typeof(D3D11Context.FnOMSetBlendState)))(ctx, blendState, IntPtr.Zero, uint.MaxValue);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006C67 File Offset: 0x00004E67
		public static void RSSetViewports(IntPtr ctx, D3D11_VIEWPORT vp)
		{
			((D3D11Context.FnRSSetViewports)VTable.Get(ctx, 44, typeof(D3D11Context.FnRSSetViewports)))(ctx, 1U, new D3D11_VIEWPORT[] { vp });
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006C95 File Offset: 0x00004E95
		public static void RSSetScissorRects(IntPtr ctx, D3D11_RECT rect)
		{
			((D3D11Context.FnRSSetScissorRects)VTable.Get(ctx, 45, typeof(D3D11Context.FnRSSetScissorRects)))(ctx, 1U, new D3D11_RECT[] { rect });
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006CC3 File Offset: 0x00004EC3
		public static void RSSetState(IntPtr ctx, IntPtr state)
		{
			((D3D11Context.FnRSSetState)VTable.Get(ctx, 43, typeof(D3D11Context.FnRSSetState)))(ctx, state);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006CE3 File Offset: 0x00004EE3
		public static void IASetInputLayout(IntPtr ctx, IntPtr layout)
		{
			((D3D11Context.FnIASetInputLayout)VTable.Get(ctx, 17, typeof(D3D11Context.FnIASetInputLayout)))(ctx, layout);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006D04 File Offset: 0x00004F04
		public static void IASetVertexBuffers(IntPtr ctx, IntPtr buffer, uint stride)
		{
			((D3D11Context.FnIASetVertexBuffers)VTable.Get(ctx, 18, typeof(D3D11Context.FnIASetVertexBuffers)))(ctx, 0U, 1U, new IntPtr[] { buffer }, new uint[] { stride }, new uint[1]);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006D4A File Offset: 0x00004F4A
		public static void IASetIndexBuffer(IntPtr ctx, IntPtr buffer)
		{
			((D3D11Context.FnIASetIndexBuffer)VTable.Get(ctx, 19, typeof(D3D11Context.FnIASetIndexBuffer)))(ctx, buffer, 42U, 0U);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006D6D File Offset: 0x00004F6D
		public static void IASetPrimitiveTopology(IntPtr ctx)
		{
			((D3D11Context.FnIASetPrimitiveTopology)VTable.Get(ctx, 24, typeof(D3D11Context.FnIASetPrimitiveTopology)))(ctx, 4U);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006D8D File Offset: 0x00004F8D
		public static void VSSetShader(IntPtr ctx, IntPtr vs)
		{
			((D3D11Context.FnVSSetShader)VTable.Get(ctx, 11, typeof(D3D11Context.FnVSSetShader)))(ctx, vs, IntPtr.Zero, 0U);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006DB3 File Offset: 0x00004FB3
		public static void PSSetShader(IntPtr ctx, IntPtr ps)
		{
			((D3D11Context.FnPSSetShader)VTable.Get(ctx, 9, typeof(D3D11Context.FnPSSetShader)))(ctx, ps, IntPtr.Zero, 0U);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006DD9 File Offset: 0x00004FD9
		public static void PSSetShaderResources(IntPtr ctx, uint slot, IntPtr srv)
		{
			((D3D11Context.FnPSSetShaderResources)VTable.Get(ctx, 8, typeof(D3D11Context.FnPSSetShaderResources)))(ctx, slot, 1U, new IntPtr[] { srv });
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006E03 File Offset: 0x00005003
		public static void PSClearShaderResource(IntPtr ctx, uint slot)
		{
			((D3D11Context.FnPSSetShaderResources)VTable.Get(ctx, 8, typeof(D3D11Context.FnPSSetShaderResources)))(ctx, slot, 1U, new IntPtr[] { IntPtr.Zero });
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006E31 File Offset: 0x00005031
		public static void PSSetSamplers(IntPtr ctx, IntPtr sampler)
		{
			((D3D11Context.FnPSSetSamplers)VTable.Get(ctx, 10, typeof(D3D11Context.FnPSSetSamplers)))(ctx, 0U, 1U, new IntPtr[] { sampler });
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006E5C File Offset: 0x0000505C
		public static void VSSetConstantBuffers(IntPtr ctx, uint slot, IntPtr cb)
		{
			((D3D11Context.FnVSSetConstantBuffers)VTable.Get(ctx, 7, typeof(D3D11Context.FnVSSetConstantBuffers)))(ctx, slot, 1U, new IntPtr[] { cb });
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006E86 File Offset: 0x00005086
		public static void PSSetConstantBuffers(IntPtr ctx, uint slot, IntPtr cb)
		{
			((D3D11Context.FnPSSetConstantBuffers)VTable.Get(ctx, 16, typeof(D3D11Context.FnPSSetConstantBuffers)))(ctx, slot, 1U, new IntPtr[] { cb });
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006EB1 File Offset: 0x000050B1
		public static void DrawIndexed(IntPtr ctx, int indexCount)
		{
			((D3D11Context.FnDrawIndexed)VTable.Get(ctx, 12, typeof(D3D11Context.FnDrawIndexed)))(ctx, (uint)indexCount, 0U, 0);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006ED3 File Offset: 0x000050D3
		public static int Map(IntPtr ctx, IntPtr resource, uint mapType, out D3D11_MAPPED_SUBRESOURCE mapped)
		{
			return ((D3D11Context.FnMap)VTable.Get(ctx, 14, typeof(D3D11Context.FnMap)))(ctx, resource, 0U, mapType, 0U, out mapped);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006EF7 File Offset: 0x000050F7
		public static void Unmap(IntPtr ctx, IntPtr resource)
		{
			((D3D11Context.FnUnmap)VTable.Get(ctx, 15, typeof(D3D11Context.FnUnmap)))(ctx, resource, 0U);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006F18 File Offset: 0x00005118
		public static void CopyResource(IntPtr ctx, IntPtr dst, IntPtr src)
		{
			((D3D11Context.FnCopyResource)VTable.Get(ctx, 47, typeof(D3D11Context.FnCopyResource)))(ctx, dst, src);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006F39 File Offset: 0x00005139
		public static void UpdateSubresource(IntPtr ctx, IntPtr resource, IntPtr data, uint rowPitch)
		{
			((D3D11Context.FnUpdateSubresource)VTable.Get(ctx, 48, typeof(D3D11Context.FnUpdateSubresource)))(ctx, resource, 0U, IntPtr.Zero, data, rowPitch, 0U);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006F62 File Offset: 0x00005162
		public static void ClearState(IntPtr ctx)
		{
			((D3D11Context.FnClearState)VTable.Get(ctx, 110, typeof(D3D11Context.FnClearState)))(ctx);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006F81 File Offset: 0x00005181
		public static void Flush(IntPtr ctx)
		{
			((D3D11Context.FnFlush)VTable.Get(ctx, 111, typeof(D3D11Context.FnFlush)))(ctx);
		}

		// Token: 0x0400012E RID: 302
		private const int Slot_VSSetConstantBuffers = 7;

		// Token: 0x0400012F RID: 303
		private const int Slot_PSSetShaderResources = 8;

		// Token: 0x04000130 RID: 304
		private const int Slot_PSSetShader = 9;

		// Token: 0x04000131 RID: 305
		private const int Slot_PSSetSamplers = 10;

		// Token: 0x04000132 RID: 306
		private const int Slot_VSSetShader = 11;

		// Token: 0x04000133 RID: 307
		private const int Slot_DrawIndexed = 12;

		// Token: 0x04000134 RID: 308
		private const int Slot_Map = 14;

		// Token: 0x04000135 RID: 309
		private const int Slot_Unmap = 15;

		// Token: 0x04000136 RID: 310
		private const int Slot_PSSetConstantBuffers = 16;

		// Token: 0x04000137 RID: 311
		private const int Slot_IASetInputLayout = 17;

		// Token: 0x04000138 RID: 312
		private const int Slot_IASetVertexBuffers = 18;

		// Token: 0x04000139 RID: 313
		private const int Slot_IASetIndexBuffer = 19;

		// Token: 0x0400013A RID: 314
		private const int Slot_IASetPrimitiveTopology = 24;

		// Token: 0x0400013B RID: 315
		private const int Slot_OMSetRenderTargets = 33;

		// Token: 0x0400013C RID: 316
		private const int Slot_OMSetBlendState = 35;

		// Token: 0x0400013D RID: 317
		private const int Slot_RSSetState = 43;

		// Token: 0x0400013E RID: 318
		private const int Slot_RSSetViewports = 44;

		// Token: 0x0400013F RID: 319
		private const int Slot_RSSetScissorRects = 45;

		// Token: 0x04000140 RID: 320
		private const int Slot_CopyResource = 47;

		// Token: 0x04000141 RID: 321
		private const int Slot_UpdateSubresource = 48;

		// Token: 0x04000142 RID: 322
		private const int Slot_ClearRenderTargetView = 50;

		// Token: 0x04000143 RID: 323
		private const int Slot_ClearState = 110;

		// Token: 0x04000144 RID: 324
		private const int Slot_Flush = 111;

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x060001B9 RID: 441
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnClearRenderTargetView(IntPtr self, IntPtr pRenderTargetView, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] ColorRGBA);

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x060001BD RID: 445
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnOMSetRenderTargets(IntPtr self, uint NumViews, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] ppRTVs, IntPtr pDSV);

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x060001C1 RID: 449
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnOMSetBlendState(IntPtr self, IntPtr pBlendState, IntPtr BlendFactor, uint SampleMask);

		// Token: 0x0200005F RID: 95
		// (Invoke) Token: 0x060001C5 RID: 453
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnRSSetViewports(IntPtr self, uint NumViewports, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] D3D11_VIEWPORT[] pViewports);

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x060001C9 RID: 457
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnRSSetScissorRects(IntPtr self, uint NumRects, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] D3D11_RECT[] pRects);

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x060001CD RID: 461
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnRSSetState(IntPtr self, IntPtr pRasterizerState);

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x060001D1 RID: 465
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnIASetInputLayout(IntPtr self, IntPtr pInputLayout);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x060001D5 RID: 469
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnIASetVertexBuffers(IntPtr self, uint StartSlot, uint NumBuffers, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] ppVertexBuffers, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] pStrides, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] pOffsets);

		// Token: 0x02000064 RID: 100
		// (Invoke) Token: 0x060001D9 RID: 473
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnIASetIndexBuffer(IntPtr self, IntPtr pIndexBuffer, uint Format, uint Offset);

		// Token: 0x02000065 RID: 101
		// (Invoke) Token: 0x060001DD RID: 477
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnIASetPrimitiveTopology(IntPtr self, uint Topology);

		// Token: 0x02000066 RID: 102
		// (Invoke) Token: 0x060001E1 RID: 481
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnVSSetShader(IntPtr self, IntPtr pVertexShader, IntPtr ppClassInstances, uint NumClassInstances);

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x060001E5 RID: 485
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnPSSetShader(IntPtr self, IntPtr pPixelShader, IntPtr ppClassInstances, uint NumClassInstances);

		// Token: 0x02000068 RID: 104
		// (Invoke) Token: 0x060001E9 RID: 489
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnPSSetShaderResources(IntPtr self, uint StartSlot, uint NumViews, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] ppSRVs);

		// Token: 0x02000069 RID: 105
		// (Invoke) Token: 0x060001ED RID: 493
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnPSSetSamplers(IntPtr self, uint StartSlot, uint NumSamplers, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] ppSamplers);

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x060001F1 RID: 497
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnVSSetConstantBuffers(IntPtr self, uint StartSlot, uint NumBuffers, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] ppCBs);

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x060001F5 RID: 501
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnPSSetConstantBuffers(IntPtr self, uint StartSlot, uint NumBuffers, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] ppCBs);

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x060001F9 RID: 505
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnDrawIndexed(IntPtr self, uint IndexCount, uint StartIndexLocation, int BaseVertexLocation);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x060001FD RID: 509
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int FnMap(IntPtr self, IntPtr pResource, uint Subresource, uint MapType, uint MapFlags, out D3D11_MAPPED_SUBRESOURCE pMappedResource);

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x06000201 RID: 513
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnUnmap(IntPtr self, IntPtr pResource, uint Subresource);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x06000205 RID: 517
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnCopyResource(IntPtr self, IntPtr pDstResource, IntPtr pSrcResource);

		// Token: 0x02000070 RID: 112
		// (Invoke) Token: 0x06000209 RID: 521
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnUpdateSubresource(IntPtr self, IntPtr pDstResource, uint DstSubresource, IntPtr pDstBox, IntPtr pSrcData, uint SrcRowPitch, uint SrcDepthPitch);

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x0600020D RID: 525
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnClearState(IntPtr self);

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x06000211 RID: 529
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void FnFlush(IntPtr self);
	}
}
