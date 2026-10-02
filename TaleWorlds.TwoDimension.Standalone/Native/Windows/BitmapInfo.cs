using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000016 RID: 22
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapInfo
	{
		// Token: 0x04000071 RID: 113
		public BitmapInfoHeader bmiHeader;

		// Token: 0x04000072 RID: 114
		public byte r;

		// Token: 0x04000073 RID: 115
		public byte g;

		// Token: 0x04000074 RID: 116
		public byte b;

		// Token: 0x04000075 RID: 117
		public byte a;
	}
}
