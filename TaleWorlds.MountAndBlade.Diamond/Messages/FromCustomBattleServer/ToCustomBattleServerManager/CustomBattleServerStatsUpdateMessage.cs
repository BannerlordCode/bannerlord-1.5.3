using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000006 RID: 6
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class CustomBattleServerStatsUpdateMessage : Message
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000021E8 File Offset: 0x000003E8
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000021F0 File Offset: 0x000003F0
		[JsonProperty]
		public BattleResult BattleResult { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000021F9 File Offset: 0x000003F9
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002201 File Offset: 0x00000401
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000220A File Offset: 0x0000040A
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002212 File Offset: 0x00000412
		[JsonProperty]
		public Dictionary<string, int> PlayerScores { get; set; }

		// Token: 0x06000025 RID: 37 RVA: 0x0000221B File Offset: 0x0000041B
		public CustomBattleServerStatsUpdateMessage()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002224 File Offset: 0x00000424
		public CustomBattleServerStatsUpdateMessage(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this.BattleResult = battleResult;
			this.TeamScores = teamScores;
			this.PlayerScores = playerScores.ToDictionary<KeyValuePair<PlayerId, int>, string, int>((KeyValuePair<PlayerId, int> kvp) => kvp.Key.ToString(), (KeyValuePair<PlayerId, int> kvp) => kvp.Value);
		}
	}
}
