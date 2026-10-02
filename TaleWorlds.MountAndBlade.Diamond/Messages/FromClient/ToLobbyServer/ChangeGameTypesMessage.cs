using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000080 RID: 128
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeGameTypesMessage : Message
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00003B7D File Offset: 0x00001D7D
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00003B85 File Offset: 0x00001D85
		[JsonProperty]
		public string[] GameTypes { get; private set; }

		// Token: 0x06000279 RID: 633 RVA: 0x00003B8E File Offset: 0x00001D8E
		public ChangeGameTypesMessage()
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00003B96 File Offset: 0x00001D96
		public ChangeGameTypesMessage(string[] gameTypes)
		{
			this.GameTypes = gameTypes;
		}
	}
}
