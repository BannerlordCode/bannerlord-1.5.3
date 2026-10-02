using System;

namespace psai.net
{
	// Token: 0x0200001A RID: 26
	public class ThemeInfo
	{
		// Token: 0x060001DD RID: 477 RVA: 0x000093F8 File Offset: 0x000075F8
		public override string ToString()
		{
			return string.Concat(new object[] { this.id, ": ", this.name, " [", this.type, "]" });
		}

		// Token: 0x0400010C RID: 268
		public int id;

		// Token: 0x0400010D RID: 269
		public ThemeType type;

		// Token: 0x0400010E RID: 270
		public int[] segmentIds;

		// Token: 0x0400010F RID: 271
		public string name;
	}
}
