using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002F RID: 47
	public struct SpriteNinePatchParameters
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000854F File Offset: 0x0000674F
		public SpriteNinePatchParameters(int leftWidth, int rightWidth, int topHeight, int bottomHeight)
		{
			this.IsValid = true;
			this.LeftWidth = leftWidth;
			this.RightWidth = rightWidth;
			this.TopHeight = topHeight;
			this.BottomHeight = bottomHeight;
		}

		// Token: 0x04000109 RID: 265
		public static SpriteNinePatchParameters Empty;

		// Token: 0x0400010A RID: 266
		public bool IsValid;

		// Token: 0x0400010B RID: 267
		public int LeftWidth;

		// Token: 0x0400010C RID: 268
		public int RightWidth;

		// Token: 0x0400010D RID: 269
		public int TopHeight;

		// Token: 0x0400010E RID: 270
		public int BottomHeight;
	}
}
