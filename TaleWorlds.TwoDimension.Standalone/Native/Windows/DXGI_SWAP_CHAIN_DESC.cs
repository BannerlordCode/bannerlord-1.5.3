using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001D RID: 29
	public struct DXGI_SWAP_CHAIN_DESC
	{
		// Token: 0x040000B2 RID: 178
		public DXGI_MODE_DESC BufferDesc;

		// Token: 0x040000B3 RID: 179
		public DXGI_SAMPLE_DESC SampleDesc;

		// Token: 0x040000B4 RID: 180
		public uint BufferUsage;

		// Token: 0x040000B5 RID: 181
		public uint BufferCount;

		// Token: 0x040000B6 RID: 182
		public IntPtr OutputWindow;

		// Token: 0x040000B7 RID: 183
		public int Windowed;

		// Token: 0x040000B8 RID: 184
		public uint SwapEffect;

		// Token: 0x040000B9 RID: 185
		public uint Flags;
	}
}
