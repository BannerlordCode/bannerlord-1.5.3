using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000025 RID: 37
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClanMessageReceivedMessage : Message
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002A40 File Offset: 0x00000C40
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00002A48 File Offset: 0x00000C48
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002A51 File Offset: 0x00000C51
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00002A59 File Offset: 0x00000C59
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x060000D8 RID: 216 RVA: 0x00002A62 File Offset: 0x00000C62
		public ClanMessageReceivedMessage()
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00002A6A File Offset: 0x00000C6A
		public ClanMessageReceivedMessage(string playerName, string message)
		{
			this.PlayerName = playerName;
			this.Message = message;
		}
	}
}
