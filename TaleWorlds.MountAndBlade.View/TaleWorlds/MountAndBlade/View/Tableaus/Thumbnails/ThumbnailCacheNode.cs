using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000049 RID: 73
	public class ThumbnailCacheNode
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0001137D File Offset: 0x0000F57D
		public ThumbnailCacheNode()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00011385 File Offset: 0x0000F585
		public ThumbnailCacheNode(string key, Texture value, int frameNo)
		{
			this.Key = key;
			this.Value = value;
			this.FrameNo = frameNo;
			this.ReferenceCount = 0;
		}

		// Token: 0x0400014A RID: 330
		public string Key;

		// Token: 0x0400014B RID: 331
		public Texture Value;

		// Token: 0x0400014C RID: 332
		public int FrameNo;

		// Token: 0x0400014D RID: 333
		public int ReferenceCount;
	}
}
