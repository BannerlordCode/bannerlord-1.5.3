using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000323 RID: 803
	public class MultiplayerGameTypeInfo
	{
		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000B2A50 File Offset: 0x000B0C50
		// (set) Token: 0x06002DE6 RID: 11750 RVA: 0x000B2A58 File Offset: 0x000B0C58
		public string GameModule { get; private set; }

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x000B2A61 File Offset: 0x000B0C61
		// (set) Token: 0x06002DE8 RID: 11752 RVA: 0x000B2A69 File Offset: 0x000B0C69
		public string GameType { get; private set; }

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002DE9 RID: 11753 RVA: 0x000B2A72 File Offset: 0x000B0C72
		// (set) Token: 0x06002DEA RID: 11754 RVA: 0x000B2A7A File Offset: 0x000B0C7A
		public List<string> Scenes { get; private set; }

		// Token: 0x06002DEB RID: 11755 RVA: 0x000B2A83 File Offset: 0x000B0C83
		public MultiplayerGameTypeInfo(string gameModule, string gameType)
		{
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Scenes = new List<string>();
		}
	}
}
