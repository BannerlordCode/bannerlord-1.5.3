using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004C RID: 76
	public abstract class ThumbnailCreationData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0001147E File Offset: 0x0000F67E
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00011486 File Offset: 0x0000F686
		public bool IsProcessed { get; internal set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0001148F File Offset: 0x0000F68F
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00011497 File Offset: 0x0000F697
		public string RenderId { get; protected set; }

		// Token: 0x0600027D RID: 637 RVA: 0x000114A0 File Offset: 0x0000F6A0
		public ThumbnailCreationData(string renderId, Action<Texture> setAction, Action cancelAction)
		{
			this.RenderId = renderId;
			this.SetAction = setAction;
			this.CancelAction = cancelAction;
		}

		// Token: 0x04000154 RID: 340
		public readonly Action<Texture> SetAction;

		// Token: 0x04000155 RID: 341
		public readonly Action CancelAction;
	}
}
