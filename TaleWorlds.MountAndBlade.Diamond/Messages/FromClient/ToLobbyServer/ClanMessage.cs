using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000086 RID: 134
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ClanMessage : Message
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00003C6D File Offset: 0x00001E6D
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00003C75 File Offset: 0x00001E75
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000291 RID: 657 RVA: 0x00003C7E File Offset: 0x00001E7E
		public ClanMessage()
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00003C86 File Offset: 0x00001E86
		public ClanMessage(string message)
		{
			this.Message = message;
		}
	}
}
