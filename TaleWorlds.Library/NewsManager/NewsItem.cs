using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TaleWorlds.Library.NewsManager
{
	// Token: 0x020000AB RID: 171
	public struct NewsItem
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00016D39 File Offset: 0x00014F39
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x00016D41 File Offset: 0x00014F41
		public string Title { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00016D4A File Offset: 0x00014F4A
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00016D52 File Offset: 0x00014F52
		public string Description { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00016D5B File Offset: 0x00014F5B
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00016D63 File Offset: 0x00014F63
		public string ImageSourcePath { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00016D6C File Offset: 0x00014F6C
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00016D74 File Offset: 0x00014F74
		public List<NewsType> Feeds { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00016D7D File Offset: 0x00014F7D
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00016D85 File Offset: 0x00014F85
		public string NewsLink { get; set; }

		// Token: 0x020000F6 RID: 246
		[JsonConverter(typeof(StringEnumConverter))]
		public enum NewsTypes
		{
			// Token: 0x0400031D RID: 797
			LauncherSingleplayer,
			// Token: 0x0400031E RID: 798
			LauncherMultiplayer,
			// Token: 0x0400031F RID: 799
			MultiplayerLobby
		}
	}
}
