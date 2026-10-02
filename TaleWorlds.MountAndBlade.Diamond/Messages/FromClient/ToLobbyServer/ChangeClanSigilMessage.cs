using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007F RID: 127
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeClanSigilMessage : Message
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00003B55 File Offset: 0x00001D55
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00003B5D File Offset: 0x00001D5D
		[JsonProperty]
		public string NewSigil { get; private set; }

		// Token: 0x06000275 RID: 629 RVA: 0x00003B66 File Offset: 0x00001D66
		public ChangeClanSigilMessage()
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00003B6E File Offset: 0x00001D6E
		public ChangeClanSigilMessage(string newSigil)
		{
			this.NewSigil = newSigil;
		}
	}
}
