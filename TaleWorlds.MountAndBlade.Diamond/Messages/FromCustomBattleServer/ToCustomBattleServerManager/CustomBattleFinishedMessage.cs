using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000003 RID: 3
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class CustomBattleFinishedMessage : Message
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002070 File Offset: 0x00000270
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002078 File Offset: 0x00000278
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002081 File Offset: 0x00000281
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002089 File Offset: 0x00000289
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002092 File Offset: 0x00000292
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000209A File Offset: 0x0000029A
		[JsonProperty]
		public Dictionary<string, int> PlayerScores { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x000020A3 File Offset: 0x000002A3
		public CustomBattleFinishedMessage()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020AC File Offset: 0x000002AC
		public CustomBattleFinishedMessage(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this.BattleResult = battleResult;
			this.TeamScores = teamScores;
			this.PlayerScores = playerScores.ToDictionary<KeyValuePair<PlayerId, int>, string, int>((KeyValuePair<PlayerId, int> kvp) => kvp.Key.ToString(), (KeyValuePair<PlayerId, int> kvp) => kvp.Value);
		}
	}
}
