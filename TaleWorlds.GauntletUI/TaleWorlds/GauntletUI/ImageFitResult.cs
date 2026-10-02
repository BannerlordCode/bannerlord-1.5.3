using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002C RID: 44
	public readonly struct ImageFitResult
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000EF07 File Offset: 0x0000D107
		public ImageFitResult(float offsetX, float offsetY, float width, float height)
		{
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0400019E RID: 414
		public readonly float OffsetX;

		// Token: 0x0400019F RID: 415
		public readonly float OffsetY;

		// Token: 0x040001A0 RID: 416
		public readonly float Width;

		// Token: 0x040001A1 RID: 417
		public readonly float Height;
	}
}
