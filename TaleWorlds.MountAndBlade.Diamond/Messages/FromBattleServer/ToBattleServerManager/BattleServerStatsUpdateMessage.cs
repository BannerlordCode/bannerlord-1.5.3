using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D6 RID: 214
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class BattleServerStatsUpdateMessage : Message
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00004BFC File Offset: 0x00002DFC
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00004C04 File Offset: 0x00002E04
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00004C0D File Offset: 0x00002E0D
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00004C15 File Offset: 0x00002E15
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; private set; }

		// Token: 0x060003FE RID: 1022 RVA: 0x00004C1E File Offset: 0x00002E1E
		public BattleServerStatsUpdateMessage()
		{
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00004C26 File Offset: 0x00002E26
		public BattleServerStatsUpdateMessage(BattleResult battleResult, Dictionary<int, int> teamScores)
		{
			this.BattleResult = battleResult;
			this.TeamScores = teamScores;
		}
	}
}
