using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007B RID: 123
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class BuyCosmeticMessage : Message
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00003AF5 File Offset: 0x00001CF5
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00003AFD File Offset: 0x00001CFD
		[JsonProperty]
		public string CosmeticId { get; private set; }

		// Token: 0x0600026B RID: 619 RVA: 0x00003B06 File Offset: 0x00001D06
		public BuyCosmeticMessage()
		{
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00003B0E File Offset: 0x00001D0E
		public BuyCosmeticMessage(string cosmeticId)
		{
			this.CosmeticId = cosmeticId;
		}
	}
}
