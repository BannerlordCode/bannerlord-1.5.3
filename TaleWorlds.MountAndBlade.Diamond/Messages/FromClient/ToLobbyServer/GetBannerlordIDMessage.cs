using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000099 RID: 153
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetBannerlordIDMessage : Message
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00003FB9 File Offset: 0x000021B9
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00003FC1 File Offset: 0x000021C1
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002DF RID: 735 RVA: 0x00003FCA File Offset: 0x000021CA
		public GetBannerlordIDMessage()
		{
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00003FD2 File Offset: 0x000021D2
		public GetBannerlordIDMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
