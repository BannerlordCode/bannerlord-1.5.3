using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D1 RID: 209
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleEndedMessage : Message
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x000049EA File Offset: 0x00002BEA
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x000049F2 File Offset: 0x00002BF2
		[JsonProperty]
		public BattleResult BattleResult { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000049FB File Offset: 0x00002BFB
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00004A03 File Offset: 0x00002C03
		[JsonProperty]
		public GameLog[] GameLogs { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00004A0C File Offset: 0x00002C0C
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x00004A14 File Offset: 0x00002C14
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDataEntries { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00004A1D File Offset: 0x00002C1D
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00004A25 File Offset: 0x00002C25
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00004A2E File Offset: 0x00002C2E
		// (set) Token: 0x060003DD RID: 989 RVA: 0x00004A36 File Offset: 0x00002C36
		[JsonProperty]
		public Dictionary<string, int> PlayerScores { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00004A3F File Offset: 0x00002C3F
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00004A47 File Offset: 0x00002C47
		[JsonProperty]
		public int GameTime { get; set; }

		// Token: 0x060003E0 RID: 992 RVA: 0x00004A50 File Offset: 0x00002C50
		public BattleEndedMessage()
		{
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00004A58 File Offset: 0x00002C58
		public BattleEndedMessage(BattleResult battleResult, GameLog[] gameLogs, Dictionary<ValueTuple<PlayerId, string, string>, int> badgeDataDictionary, int gameTime, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this.BattleResult = battleResult;
			this.GameLogs = gameLogs;
			this.BadgeDataEntries = BadgeDataEntry.ToList(badgeDataDictionary);
			this.TeamScores = teamScores;
			this.PlayerScores = playerScores.ToDictionary<KeyValuePair<PlayerId, int>, string, int>((KeyValuePair<PlayerId, int> kvp) => kvp.Key.ToString(), (KeyValuePair<PlayerId, int> kvp) => kvp.Value);
			this.GameTime = gameTime;
		}
	}
}
