using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B1 RID: 177
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class KickFromClanMessage : Message
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000042FB File Offset: 0x000024FB
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00004303 File Offset: 0x00002503
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000330 RID: 816 RVA: 0x0000430C File Offset: 0x0000250C
		public KickFromClanMessage()
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00004314 File Offset: 0x00002514
		public KickFromClanMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
