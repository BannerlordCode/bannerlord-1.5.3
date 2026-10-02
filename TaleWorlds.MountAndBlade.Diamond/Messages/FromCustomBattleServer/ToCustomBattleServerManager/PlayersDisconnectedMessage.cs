using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000A RID: 10
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class PlayersDisconnectedMessage : Message
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000023B7 File Offset: 0x000005B7
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000023BF File Offset: 0x000005BF
		[JsonProperty]
		public PlayerDisconnectData[] Players { get; private set; }

		// Token: 0x0600003D RID: 61 RVA: 0x000023C8 File Offset: 0x000005C8
		public PlayersDisconnectedMessage()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000023D0 File Offset: 0x000005D0
		public PlayersDisconnectedMessage(PlayerDisconnectData[] players)
		{
			this.Players = players;
		}
	}
}
