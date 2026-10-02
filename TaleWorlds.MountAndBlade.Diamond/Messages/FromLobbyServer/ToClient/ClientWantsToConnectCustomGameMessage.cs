using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000027 RID: 39
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClientWantsToConnectCustomGameMessage : Message
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002AA8 File Offset: 0x00000CA8
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00002AB0 File Offset: 0x00000CB0
		[JsonProperty]
		public PlayerJoinGameData[] PlayerJoinGameData { get; private set; }

		// Token: 0x060000E0 RID: 224 RVA: 0x00002AB9 File Offset: 0x00000CB9
		public ClientWantsToConnectCustomGameMessage()
		{
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002AC1 File Offset: 0x00000CC1
		public ClientWantsToConnectCustomGameMessage(PlayerJoinGameData[] playerJoinGameData)
		{
			this.PlayerJoinGameData = playerJoinGameData;
		}
	}
}
