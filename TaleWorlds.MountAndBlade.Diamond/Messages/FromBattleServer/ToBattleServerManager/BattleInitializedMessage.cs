using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D3 RID: 211
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleInitializedMessage : Message
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00004AE8 File Offset: 0x00002CE8
		[JsonProperty]
		public string GameType { get; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00004AF0 File Offset: 0x00002CF0
		[JsonProperty]
		public List<PlayerId> AssignedPlayers { get; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00004AF8 File Offset: 0x00002CF8
		[JsonProperty]
		public string Faction1 { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00004B00 File Offset: 0x00002D00
		[JsonProperty]
		public string Faction2 { get; }

		// Token: 0x060003E7 RID: 999 RVA: 0x00004B08 File Offset: 0x00002D08
		public BattleInitializedMessage()
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00004B10 File Offset: 0x00002D10
		public BattleInitializedMessage(string gameType, List<PlayerId> assignedPlayers, string faction1, string faction2)
		{
			this.GameType = gameType;
			this.AssignedPlayers = assignedPlayers;
			this.Faction1 = faction1;
			this.Faction2 = faction2;
		}
	}
}
