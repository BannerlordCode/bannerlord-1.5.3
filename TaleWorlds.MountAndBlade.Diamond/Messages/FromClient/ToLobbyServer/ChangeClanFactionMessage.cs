using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007E RID: 126
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeClanFactionMessage : Message
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00003B2D File Offset: 0x00001D2D
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00003B35 File Offset: 0x00001D35
		[JsonProperty]
		public string NewFaction { get; private set; }

		// Token: 0x06000271 RID: 625 RVA: 0x00003B3E File Offset: 0x00001D3E
		public ChangeClanFactionMessage()
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00003B46 File Offset: 0x00001D46
		public ChangeClanFactionMessage(string newFaction)
		{
			this.NewFaction = newFaction;
		}
	}
}
