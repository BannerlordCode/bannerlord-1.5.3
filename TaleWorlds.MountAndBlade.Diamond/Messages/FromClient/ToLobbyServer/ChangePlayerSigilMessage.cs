using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000081 RID: 129
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangePlayerSigilMessage : Message
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00003BA5 File Offset: 0x00001DA5
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00003BAD File Offset: 0x00001DAD
		[JsonProperty]
		public string SigilId { get; private set; }

		// Token: 0x0600027D RID: 637 RVA: 0x00003BB6 File Offset: 0x00001DB6
		public ChangePlayerSigilMessage()
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00003BBE File Offset: 0x00001DBE
		public ChangePlayerSigilMessage(string sigilId)
		{
			this.SigilId = sigilId;
		}
	}
}
