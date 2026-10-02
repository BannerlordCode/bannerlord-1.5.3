using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004D RID: 77
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameAnswerMessage : Message
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000324B File Offset: 0x0000144B
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00003253 File Offset: 0x00001453
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x0600019B RID: 411 RVA: 0x0000325C File Offset: 0x0000145C
		public JoinPremadeGameAnswerMessage()
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00003264 File Offset: 0x00001464
		public JoinPremadeGameAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
