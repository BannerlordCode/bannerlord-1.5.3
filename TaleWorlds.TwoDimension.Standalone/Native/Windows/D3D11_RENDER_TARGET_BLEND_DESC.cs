using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000027 RID: 39
	public struct D3D11_RENDER_TARGET_BLEND_DESC
	{
		// Token: 0x040000E4 RID: 228
		public int BlendEnable;

		// Token: 0x040000E5 RID: 229
		public uint SrcBlend;

		// Token: 0x040000E6 RID: 230
		public uint DestBlend;

		// Token: 0x040000E7 RID: 231
		public uint BlendOp;

		// Token: 0x040000E8 RID: 232
		public uint SrcBlendAlpha;

		// Token: 0x040000E9 RID: 233
		public uint DestBlendAlpha;

		// Token: 0x040000EA RID: 234
		public uint BlendOpAlpha;

		// Token: 0x040000EB RID: 235
		public byte RenderTargetWriteMask;

		// Token: 0x040000EC RID: 236
		private byte _pad0;

		// Token: 0x040000ED RID: 237
		private byte _pad1;

		// Token: 0x040000EE RID: 238
		private byte _pad2;
	}
}
