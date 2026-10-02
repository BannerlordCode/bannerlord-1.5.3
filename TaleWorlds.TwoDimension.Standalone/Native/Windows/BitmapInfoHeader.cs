using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000017 RID: 23
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapInfoHeader
	{
		// Token: 0x04000076 RID: 118
		public uint biSize;

		// Token: 0x04000077 RID: 119
		public int biWidth;

		// Token: 0x04000078 RID: 120
		public int biHeight;

		// Token: 0x04000079 RID: 121
		public ushort biPlanes;

		// Token: 0x0400007A RID: 122
		public ushort biBitCount;

		// Token: 0x0400007B RID: 123
		public uint biCompression;

		// Token: 0x0400007C RID: 124
		public uint biSizeImage;

		// Token: 0x0400007D RID: 125
		public int biXPelsPerMeter;

		// Token: 0x0400007E RID: 126
		public int biYPelsPerMeter;

		// Token: 0x0400007F RID: 127
		public uint biClrUsed;

		// Token: 0x04000080 RID: 128
		public uint biClrImportant;
	}
}
