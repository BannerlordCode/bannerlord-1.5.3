using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000021 RID: 33
	public struct D3D11_TEXTURE2D_DESC
	{
		// Token: 0x040000C4 RID: 196
		public uint Width;

		// Token: 0x040000C5 RID: 197
		public uint Height;

		// Token: 0x040000C6 RID: 198
		public uint MipLevels;

		// Token: 0x040000C7 RID: 199
		public uint ArraySize;

		// Token: 0x040000C8 RID: 200
		public uint Format;

		// Token: 0x040000C9 RID: 201
		public DXGI_SAMPLE_DESC SampleDesc;

		// Token: 0x040000CA RID: 202
		public uint Usage;

		// Token: 0x040000CB RID: 203
		public uint BindFlags;

		// Token: 0x040000CC RID: 204
		public uint CPUAccessFlags;

		// Token: 0x040000CD RID: 205
		public uint MiscFlags;
	}
}
