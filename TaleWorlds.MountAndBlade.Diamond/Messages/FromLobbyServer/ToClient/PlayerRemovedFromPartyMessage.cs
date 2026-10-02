using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005D RID: 93
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerRemovedFromPartyMessage : Message
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000350C File Offset: 0x0000170C
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00003514 File Offset: 0x00001714
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000351D File Offset: 0x0000171D
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00003525 File Offset: 0x00001725
		[JsonProperty]
		public PartyRemoveReason Reason { get; private set; }

		// Token: 0x060001E0 RID: 480 RVA: 0x0000352E File Offset: 0x0000172E
		public PlayerRemovedFromPartyMessage()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00003536 File Offset: 0x00001736
		public PlayerRemovedFromPartyMessage(PlayerId playerId, PartyRemoveReason reason)
		{
			this.PlayerId = playerId;
			this.Reason = reason;
		}
	}
}
