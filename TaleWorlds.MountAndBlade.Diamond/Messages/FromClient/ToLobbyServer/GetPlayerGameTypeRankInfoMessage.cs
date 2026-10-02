using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A4 RID: 164
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerGameTypeRankInfoMessage : Message
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000040A9 File Offset: 0x000022A9
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x000040B1 File Offset: 0x000022B1
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002F8 RID: 760 RVA: 0x000040BA File Offset: 0x000022BA
		public GetPlayerGameTypeRankInfoMessage()
		{
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000040C2 File Offset: 0x000022C2
		public GetPlayerGameTypeRankInfoMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
