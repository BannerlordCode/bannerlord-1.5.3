using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000058 RID: 88
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerInvitedToPartyMessage : Message
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00003434 File Offset: 0x00001634
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000343C File Offset: 0x0000163C
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00003445 File Offset: 0x00001645
		// (set) Token: 0x060001CA RID: 458 RVA: 0x0000344D File Offset: 0x0000164D
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x060001CB RID: 459 RVA: 0x00003456 File Offset: 0x00001656
		public PlayerInvitedToPartyMessage()
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000345E File Offset: 0x0000165E
		public PlayerInvitedToPartyMessage(PlayerId playerId, string playerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
		}
	}
}
