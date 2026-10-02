using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001C RID: 28
	public static class D3D11Native
	{
		// Token: 0x06000108 RID: 264
		[DllImport("d3d11.dll")]
		public static extern int D3D11CreateDevice(IntPtr pAdapter, int driverType, IntPtr software, uint flags, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] int[] pFeatureLevels, int featureLevelCount, int sdkVersion, out IntPtr ppDevice, out int pFeatureLevel, out IntPtr ppImmediateContext);

		// Token: 0x06000109 RID: 265
		[DllImport("d3d11.dll")]
		public static extern int D3D11CreateDeviceAndSwapChain(IntPtr pAdapter, int driverType, IntPtr software, uint flags, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] int[] pFeatureLevels, int featureLevelCount, int sdkVersion, ref DXGI_SWAP_CHAIN_DESC pSwapChainDesc, out IntPtr ppSwapChain, out IntPtr ppDevice, out int pFeatureLevel, out IntPtr ppImmediateContext);

		// Token: 0x04000089 RID: 137
		public const int D3D11_SDK_VERSION = 7;

		// Token: 0x0400008A RID: 138
		public const uint D3D11_CREATE_DEVICE_BGRA_SUPPORT = 32U;

		// Token: 0x0400008B RID: 139
		public const int D3D_FEATURE_LEVEL_11_0 = 45056;

		// Token: 0x0400008C RID: 140
		public const int D3D_FEATURE_LEVEL_10_1 = 41216;

		// Token: 0x0400008D RID: 141
		public const int D3D_FEATURE_LEVEL_10_0 = 40960;

		// Token: 0x0400008E RID: 142
		public const uint D3D11_BIND_VERTEX_BUFFER = 1U;

		// Token: 0x0400008F RID: 143
		public const uint D3D11_BIND_INDEX_BUFFER = 2U;

		// Token: 0x04000090 RID: 144
		public const uint D3D11_BIND_CONSTANT_BUFFER = 4U;

		// Token: 0x04000091 RID: 145
		public const uint D3D11_BIND_SHADER_RESOURCE = 8U;

		// Token: 0x04000092 RID: 146
		public const uint D3D11_BIND_RENDER_TARGET = 32U;

		// Token: 0x04000093 RID: 147
		public const uint D3D11_USAGE_DEFAULT = 0U;

		// Token: 0x04000094 RID: 148
		public const uint D3D11_USAGE_DYNAMIC = 2U;

		// Token: 0x04000095 RID: 149
		public const uint D3D11_USAGE_STAGING = 3U;

		// Token: 0x04000096 RID: 150
		public const uint D3D11_CPU_ACCESS_WRITE = 65536U;

		// Token: 0x04000097 RID: 151
		public const uint D3D11_CPU_ACCESS_READ = 131072U;

		// Token: 0x04000098 RID: 152
		public const uint D3D11_MAP_READ = 1U;

		// Token: 0x04000099 RID: 153
		public const uint D3D11_MAP_WRITE = 2U;

		// Token: 0x0400009A RID: 154
		public const uint D3D11_MAP_READ_WRITE = 3U;

		// Token: 0x0400009B RID: 155
		public const uint D3D11_MAP_WRITE_DISCARD = 4U;

		// Token: 0x0400009C RID: 156
		public const uint D3D11_MAP_WRITE_NO_OVERWRITE = 5U;

		// Token: 0x0400009D RID: 157
		public const uint D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST = 4U;

		// Token: 0x0400009E RID: 158
		public const uint D3D11_BLEND_ZERO = 1U;

		// Token: 0x0400009F RID: 159
		public const uint D3D11_BLEND_ONE = 2U;

		// Token: 0x040000A0 RID: 160
		public const uint D3D11_BLEND_SRC_ALPHA = 5U;

		// Token: 0x040000A1 RID: 161
		public const uint D3D11_BLEND_INV_SRC_ALPHA = 6U;

		// Token: 0x040000A2 RID: 162
		public const uint D3D11_BLEND_OP_ADD = 1U;

		// Token: 0x040000A3 RID: 163
		public const uint D3D11_TEXTURE_ADDRESS_WRAP = 1U;

		// Token: 0x040000A4 RID: 164
		public const uint D3D11_TEXTURE_ADDRESS_CLAMP = 3U;

		// Token: 0x040000A5 RID: 165
		public const uint D3D11_FILTER_MIN_MAG_MIP_LINEAR = 21U;

		// Token: 0x040000A6 RID: 166
		public const uint D3D11_FILL_SOLID = 3U;

		// Token: 0x040000A7 RID: 167
		public const uint D3D11_CULL_NONE = 1U;

		// Token: 0x040000A8 RID: 168
		public const uint DXGI_FORMAT_UNKNOWN = 0U;

		// Token: 0x040000A9 RID: 169
		public const uint DXGI_FORMAT_R32G32_FLOAT = 16U;

		// Token: 0x040000AA RID: 170
		public const uint DXGI_FORMAT_R8G8B8A8_UNORM = 28U;

		// Token: 0x040000AB RID: 171
		public const uint DXGI_FORMAT_R32_UINT = 42U;

		// Token: 0x040000AC RID: 172
		public const uint DXGI_FORMAT_R8_UNORM = 61U;

		// Token: 0x040000AD RID: 173
		public const uint DXGI_FORMAT_B8G8R8A8_UNORM = 87U;

		// Token: 0x040000AE RID: 174
		public const uint DXGI_USAGE_RENDER_TARGET_OUTPUT = 32U;

		// Token: 0x040000AF RID: 175
		public const uint DXGI_SWAP_EFFECT_DISCARD = 0U;

		// Token: 0x040000B0 RID: 176
		public const int DXGI_ERROR_DEVICE_REMOVED = -2005270523;

		// Token: 0x040000B1 RID: 177
		public const int DXGI_ERROR_DEVICE_RESET = -2005270521;
	}
}
