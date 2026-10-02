using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B3 RID: 179
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PartyMessage : Message
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000434B File Offset: 0x0000254B
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00004353 File Offset: 0x00002553
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000338 RID: 824 RVA: 0x0000435C File Offset: 0x0000255C
		public PartyMessage()
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00004364 File Offset: 0x00002564
		public PartyMessage(string message)
		{
			this.Message = message;
		}
	}
}
