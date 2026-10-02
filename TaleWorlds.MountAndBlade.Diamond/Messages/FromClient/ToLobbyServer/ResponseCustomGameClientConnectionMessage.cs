using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C4 RID: 196
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ResponseCustomGameClientConnectionMessage : Message
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000478A File Offset: 0x0000298A
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00004792 File Offset: 0x00002992
		[JsonProperty]
		public PlayerJoinGameResponseDataFromHost[] PlayerJoinData { get; private set; }

		// Token: 0x0600039B RID: 923 RVA: 0x0000479B File Offset: 0x0000299B
		public ResponseCustomGameClientConnectionMessage()
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000047A3 File Offset: 0x000029A3
		public ResponseCustomGameClientConnectionMessage(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			this.PlayerJoinData = playerJoinData;
		}
	}
}
