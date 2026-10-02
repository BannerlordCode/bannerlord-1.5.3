using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004F RID: 79
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameRequestResultMessage : Message
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00003334 File Offset: 0x00001534
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000333C File Offset: 0x0000153C
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060001AF RID: 431 RVA: 0x00003345 File Offset: 0x00001545
		public JoinPremadeGameRequestResultMessage()
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000334D File Offset: 0x0000154D
		public JoinPremadeGameRequestResultMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
