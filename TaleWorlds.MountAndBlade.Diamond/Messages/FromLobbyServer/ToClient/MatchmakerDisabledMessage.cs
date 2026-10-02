using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000052 RID: 82
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class MatchmakerDisabledMessage : Message
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000338C File Offset: 0x0000158C
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00003394 File Offset: 0x00001594
		[JsonProperty]
		public int RemainingTime { get; private set; }

		// Token: 0x060001B8 RID: 440 RVA: 0x0000339D File Offset: 0x0000159D
		public MatchmakerDisabledMessage()
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000033A5 File Offset: 0x000015A5
		public MatchmakerDisabledMessage(int remainingTime)
		{
			this.RemainingTime = remainingTime;
		}
	}
}
