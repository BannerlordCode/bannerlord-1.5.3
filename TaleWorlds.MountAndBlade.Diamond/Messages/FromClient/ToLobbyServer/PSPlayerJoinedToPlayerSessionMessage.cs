using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B7 RID: 183
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PSPlayerJoinedToPlayerSessionMessage : Message
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00004403 File Offset: 0x00002603
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000440B File Offset: 0x0000260B
		[JsonProperty]
		public ulong InviterPlayerAccountId { get; private set; }

		// Token: 0x0600034A RID: 842 RVA: 0x00004414 File Offset: 0x00002614
		public PSPlayerJoinedToPlayerSessionMessage()
		{
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000441C File Offset: 0x0000261C
		public PSPlayerJoinedToPlayerSessionMessage(ulong inviterPlayerAccountId)
		{
			this.InviterPlayerAccountId = inviterPlayerAccountId;
		}
	}
}
