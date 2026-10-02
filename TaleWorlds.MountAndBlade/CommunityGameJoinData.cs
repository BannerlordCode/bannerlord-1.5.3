using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002ED RID: 749
	public class CommunityGameJoinData
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000A784F File Offset: 0x000A5A4F
		// (set) Token: 0x06002B6E RID: 11118 RVA: 0x000A7857 File Offset: 0x000A5A57
		public string Name { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x000A7860 File Offset: 0x000A5A60
		// (set) Token: 0x06002B70 RID: 11120 RVA: 0x000A7868 File Offset: 0x000A5A68
		public PlayerId PlayerId { get; set; }
	}
}
