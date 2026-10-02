using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000060 RID: 96
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PremadeGameEligibilityStatusMessage : Message
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00003637 File Offset: 0x00001837
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000363F File Offset: 0x0000183F
		[JsonProperty]
		public PremadeGameType[] EligibleGameTypes { get; private set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x00003648 File Offset: 0x00001848
		public PremadeGameEligibilityStatusMessage()
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00003650 File Offset: 0x00001850
		public PremadeGameEligibilityStatusMessage(PremadeGameType[] eligibleGameTypes)
		{
			this.EligibleGameTypes = eligibleGameTypes;
		}
	}
}
