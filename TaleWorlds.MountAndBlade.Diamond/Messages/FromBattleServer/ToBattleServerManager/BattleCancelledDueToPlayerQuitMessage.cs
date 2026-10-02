using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D0 RID: 208
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleCancelledDueToPlayerQuitMessage : Message
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000049AA File Offset: 0x00002BAA
		// (set) Token: 0x060003CF RID: 975 RVA: 0x000049B2 File Offset: 0x00002BB2
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000049BB File Offset: 0x00002BBB
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x000049C3 File Offset: 0x00002BC3
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x060003D2 RID: 978 RVA: 0x000049CC File Offset: 0x00002BCC
		public BattleCancelledDueToPlayerQuitMessage()
		{
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000049D4 File Offset: 0x00002BD4
		public BattleCancelledDueToPlayerQuitMessage(PlayerId playerId, string gameType)
		{
			this.PlayerId = playerId;
			this.GameType = gameType;
		}
	}
}
