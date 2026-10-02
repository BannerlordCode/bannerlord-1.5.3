using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000029 RID: 41
	public struct D3D11_RASTERIZER_DESC
	{
		// Token: 0x040000F9 RID: 249
		public uint FillMode;

		// Token: 0x040000FA RID: 250
		public uint CullMode;

		// Token: 0x040000FB RID: 251
		public int FrontCounterClockwise;

		// Token: 0x040000FC RID: 252
		public int DepthBias;

		// Token: 0x040000FD RID: 253
		public float DepthBiasClamp;

		// Token: 0x040000FE RID: 254
		public float SlopeScaledDepthBias;

		// Token: 0x040000FF RID: 255
		public int DepthClipEnable;

		// Token: 0x04000100 RID: 256
		public int ScissorEnable;

		// Token: 0x04000101 RID: 257
		public int MultisampleEnable;

		// Token: 0x04000102 RID: 258
		public int AntialiasedLineEnable;
	}
}
