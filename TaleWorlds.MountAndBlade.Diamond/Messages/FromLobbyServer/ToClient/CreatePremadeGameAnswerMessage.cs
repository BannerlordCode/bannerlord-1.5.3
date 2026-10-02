using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000029 RID: 41
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CreatePremadeGameAnswerMessage : Message
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00002AF8 File Offset: 0x00000CF8
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00002B00 File Offset: 0x00000D00
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000E8 RID: 232 RVA: 0x00002B09 File Offset: 0x00000D09
		public CreatePremadeGameAnswerMessage()
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00002B11 File Offset: 0x00000D11
		public CreatePremadeGameAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
