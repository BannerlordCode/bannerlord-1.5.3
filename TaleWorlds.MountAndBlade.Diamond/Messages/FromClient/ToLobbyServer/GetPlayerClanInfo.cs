using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A2 RID: 162
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerClanInfo : Message
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00004079 File Offset: 0x00002279
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x00004081 File Offset: 0x00002281
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002F3 RID: 755 RVA: 0x0000408A File Offset: 0x0000228A
		public GetPlayerClanInfo()
		{
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00004092 File Offset: 0x00002292
		public GetPlayerClanInfo(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
