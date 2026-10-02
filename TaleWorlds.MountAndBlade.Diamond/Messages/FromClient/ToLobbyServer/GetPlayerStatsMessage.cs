using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A5 RID: 165
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerStatsMessage : Message
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000040D1 File Offset: 0x000022D1
		// (set) Token: 0x060002FB RID: 763 RVA: 0x000040D9 File Offset: 0x000022D9
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002FC RID: 764 RVA: 0x000040E2 File Offset: 0x000022E2
		public GetPlayerStatsMessage()
		{
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000040EA File Offset: 0x000022EA
		public GetPlayerStatsMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
