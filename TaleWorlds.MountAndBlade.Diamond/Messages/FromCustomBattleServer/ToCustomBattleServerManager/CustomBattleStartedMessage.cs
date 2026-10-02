using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000007 RID: 7
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class CustomBattleStartedMessage : Message
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000228F File Offset: 0x0000048F
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002297 File Offset: 0x00000497
		[JsonProperty]
		public string GameType { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000022A0 File Offset: 0x000004A0
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000022A8 File Offset: 0x000004A8
		[JsonProperty]
		public Dictionary<string, int> PlayerTeams { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000022B1 File Offset: 0x000004B1
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000022B9 File Offset: 0x000004B9
		[JsonProperty]
		public List<string> FactionNames { get; set; }

		// Token: 0x0600002D RID: 45 RVA: 0x000022C2 File Offset: 0x000004C2
		public CustomBattleStartedMessage()
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000022CC File Offset: 0x000004CC
		public CustomBattleStartedMessage(string gameType, Dictionary<PlayerId, int> playerTeams, List<string> factionNames)
		{
			this.GameType = gameType;
			this.PlayerTeams = playerTeams.ToDictionary<KeyValuePair<PlayerId, int>, string, int>((KeyValuePair<PlayerId, int> kvp) => kvp.Key.ToString(), (KeyValuePair<PlayerId, int> kvp) => kvp.Value);
			this.FactionNames = factionNames;
		}
	}
}
