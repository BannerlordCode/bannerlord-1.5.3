using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BD RID: 189
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveClanOfficerRoleForPlayerMessage : Message
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000045D0 File Offset: 0x000027D0
		// (set) Token: 0x06000371 RID: 881 RVA: 0x000045D8 File Offset: 0x000027D8
		[JsonProperty]
		public PlayerId RemovedOfficerId { get; private set; }

		// Token: 0x06000372 RID: 882 RVA: 0x000045E1 File Offset: 0x000027E1
		public RemoveClanOfficerRoleForPlayerMessage()
		{
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000045E9 File Offset: 0x000027E9
		public RemoveClanOfficerRoleForPlayerMessage(PlayerId removedOfficerId)
		{
			this.RemovedOfficerId = removedOfficerId;
		}
	}
}
