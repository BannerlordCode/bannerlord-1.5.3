using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BB RID: 187
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RejoinBattleRequestMessage : Message
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00004580 File Offset: 0x00002780
		// (set) Token: 0x06000369 RID: 873 RVA: 0x00004588 File Offset: 0x00002788
		[JsonProperty]
		public bool IsRejoinAccepted { get; private set; }

		// Token: 0x0600036A RID: 874 RVA: 0x00004591 File Offset: 0x00002791
		public RejoinBattleRequestMessage()
		{
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00004599 File Offset: 0x00002799
		public RejoinBattleRequestMessage(bool isRejoinAccepted)
		{
			this.IsRejoinAccepted = isRejoinAccepted;
		}
	}
}
