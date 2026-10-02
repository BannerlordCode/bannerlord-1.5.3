using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000020 RID: 32
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ClanCreationRequestAnsweredMessage : Message
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00002957 File Offset: 0x00000B57
		// (set) Token: 0x060000BF RID: 191 RVA: 0x0000295F File Offset: 0x00000B5F
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00002968 File Offset: 0x00000B68
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002970 File Offset: 0x00000B70
		[JsonProperty]
		public ClanCreationAnswer ClanCreationAnswer { get; private set; }

		// Token: 0x060000C2 RID: 194 RVA: 0x00002979 File Offset: 0x00000B79
		public ClanCreationRequestAnsweredMessage()
		{
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002981 File Offset: 0x00000B81
		public ClanCreationRequestAnsweredMessage(PlayerId playerId, ClanCreationAnswer clanCreationAnswer)
		{
			this.PlayerId = playerId;
			this.ClanCreationAnswer = clanCreationAnswer;
		}
	}
}
