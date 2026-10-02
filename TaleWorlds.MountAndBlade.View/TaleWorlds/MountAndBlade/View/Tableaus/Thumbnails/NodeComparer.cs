using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004A RID: 74
	public class NodeComparer : IComparer<ThumbnailCacheNode>
	{
		// Token: 0x06000272 RID: 626 RVA: 0x000113A9 File Offset: 0x0000F5A9
		public int Compare(ThumbnailCacheNode x, ThumbnailCacheNode y)
		{
			return x.FrameNo.CompareTo(y.FrameNo);
		}
	}
}
