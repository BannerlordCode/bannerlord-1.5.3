using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	public class ClanCreationPlayerData
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x000070BD File Offset: 0x000052BD
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x000070C5 File Offset: 0x000052C5
		public PlayerSessionId PlayerSessionId { get; private set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x000070CE File Offset: 0x000052CE
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x000070D6 File Offset: 0x000052D6
		public ClanCreationAnswer ClanCreationAnswer { get; private set; }

		// Token: 0x06000594 RID: 1428 RVA: 0x000070DF File Offset: 0x000052DF
		public ClanCreationPlayerData(PlayerSessionId playerSessionId, ClanCreationAnswer answer)
		{
			this.PlayerSessionId = playerSessionId;
			this.ClanCreationAnswer = answer;
		}
	}
}
