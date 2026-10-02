using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000095 RID: 149
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetAnotherPlayerDataMessage : Message
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00003F59 File Offset: 0x00002159
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00003F61 File Offset: 0x00002161
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002D5 RID: 725 RVA: 0x00003F6A File Offset: 0x0000216A
		public GetAnotherPlayerDataMessage()
		{
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00003F72 File Offset: 0x00002172
		public GetAnotherPlayerDataMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
