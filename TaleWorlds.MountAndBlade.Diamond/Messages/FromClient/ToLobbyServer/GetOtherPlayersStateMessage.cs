using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200009F RID: 159
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetOtherPlayersStateMessage : Message
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00004009 File Offset: 0x00002209
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00004011 File Offset: 0x00002211
		[JsonProperty]
		public List<PlayerId> Players { get; private set; }

		// Token: 0x060002E8 RID: 744 RVA: 0x0000401A File Offset: 0x0000221A
		public GetOtherPlayersStateMessage()
		{
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00004022 File Offset: 0x00002222
		public GetOtherPlayersStateMessage(List<PlayerId> players)
		{
			this.Players = players;
		}
	}
}
