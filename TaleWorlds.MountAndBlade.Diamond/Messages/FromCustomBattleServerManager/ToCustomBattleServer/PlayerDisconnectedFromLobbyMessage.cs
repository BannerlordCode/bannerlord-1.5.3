using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000012 RID: 18
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class PlayerDisconnectedFromLobbyMessage : Message
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000026EC File Offset: 0x000008EC
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000026F4 File Offset: 0x000008F4
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000084 RID: 132 RVA: 0x000026FD File Offset: 0x000008FD
		public PlayerDisconnectedFromLobbyMessage()
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002705 File Offset: 0x00000905
		public PlayerDisconnectedFromLobbyMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
