using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000096 RID: 150
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetAnotherPlayerStateMessage : Message
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00003F81 File Offset: 0x00002181
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00003F89 File Offset: 0x00002189
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002D9 RID: 729 RVA: 0x00003F92 File Offset: 0x00002192
		public GetAnotherPlayerStateMessage()
		{
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00003F9A File Offset: 0x0000219A
		public GetAnotherPlayerStateMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
