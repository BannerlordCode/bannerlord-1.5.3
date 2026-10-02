using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E4 RID: 228
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class PlayerDisconnectedFromLobbyMessage : Message
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00004EC4 File Offset: 0x000030C4
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00004ECC File Offset: 0x000030CC
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600043E RID: 1086 RVA: 0x00004ED5 File Offset: 0x000030D5
		public PlayerDisconnectedFromLobbyMessage()
		{
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00004EDD File Offset: 0x000030DD
		public PlayerDisconnectedFromLobbyMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
