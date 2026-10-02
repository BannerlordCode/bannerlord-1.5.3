using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AA RID: 170
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRecentPlayersStatusMessage : Message
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00004189 File Offset: 0x00002389
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00004191 File Offset: 0x00002391
		[JsonProperty]
		public PlayerId[] RecentPlayers { get; private set; }

		// Token: 0x0600030E RID: 782 RVA: 0x0000419A File Offset: 0x0000239A
		public GetRecentPlayersStatusMessage()
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000041A2 File Offset: 0x000023A2
		public GetRecentPlayersStatusMessage(PlayerId[] recentPlayers)
		{
			this.RecentPlayers = recentPlayers;
		}
	}
}
