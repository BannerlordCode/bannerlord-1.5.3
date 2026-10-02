using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000144 RID: 324
	[Serializable]
	public class PlayerBattleServerInformation
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0000D07D File Offset: 0x0000B27D
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x0000D085 File Offset: 0x0000B285
		public int PeerIndex { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0000D08E File Offset: 0x0000B28E
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x0000D096 File Offset: 0x0000B296
		public int SessionKey { get; set; }

		// Token: 0x060008CE RID: 2254 RVA: 0x0000D09F File Offset: 0x0000B29F
		public PlayerBattleServerInformation()
		{
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000D0A7 File Offset: 0x0000B2A7
		public PlayerBattleServerInformation(int peerIndex, int sessionKey)
		{
			this.PeerIndex = peerIndex;
			this.SessionKey = sessionKey;
		}
	}
}
