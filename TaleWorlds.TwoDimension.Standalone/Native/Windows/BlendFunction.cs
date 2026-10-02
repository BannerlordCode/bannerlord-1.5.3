using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000019 RID: 25
	public struct BlendFunction
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000068D5 File Offset: 0x00004AD5
		public BlendFunction(AlphaFormatFlags op, byte flags, byte alpha, AlphaFormatFlags format)
		{
			this.BlendOp = (byte)op;
			this.BlendFlags = flags;
			this.SourceConstantAlpha = alpha;
			this.AlphaFormat = (byte)format;
		}

		// Token: 0x04000084 RID: 132
		public byte BlendOp;

		// Token: 0x04000085 RID: 133
		public byte BlendFlags;

		// Token: 0x04000086 RID: 134
		public byte SourceConstantAlpha;

		// Token: 0x04000087 RID: 135
		public byte AlphaFormat;

		// Token: 0x04000088 RID: 136
		public static readonly BlendFunction Default = new BlendFunction(AlphaFormatFlags.Over, 0, byte.MaxValue, AlphaFormatFlags.Alpha);
	}
}
