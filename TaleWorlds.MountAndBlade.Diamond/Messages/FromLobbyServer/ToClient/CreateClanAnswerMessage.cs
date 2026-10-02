using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000028 RID: 40
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CreateClanAnswerMessage : Message
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00002AD8 File Offset: 0x00000CD8
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000E4 RID: 228 RVA: 0x00002AE1 File Offset: 0x00000CE1
		public CreateClanAnswerMessage()
		{
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00002AE9 File Offset: 0x00000CE9
		public CreateClanAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
