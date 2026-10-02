using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000069 RID: 105
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class SigilChangeAnswerMessage : Message
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000037E0 File Offset: 0x000019E0
		// (set) Token: 0x0600021E RID: 542 RVA: 0x000037E8 File Offset: 0x000019E8
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x000037F1 File Offset: 0x000019F1
		public SigilChangeAnswerMessage()
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000037F9 File Offset: 0x000019F9
		public SigilChangeAnswerMessage(bool answer)
		{
			this.Successful = answer;
		}
	}
}
