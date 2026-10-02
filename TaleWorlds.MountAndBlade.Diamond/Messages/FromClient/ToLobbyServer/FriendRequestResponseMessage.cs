using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000094 RID: 148
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class FriendRequestResponseMessage : Message
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00003EE8 File Offset: 0x000020E8
		// (set) Token: 0x060002CA RID: 714 RVA: 0x00003EF0 File Offset: 0x000020F0
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00003EF9 File Offset: 0x000020F9
		// (set) Token: 0x060002CC RID: 716 RVA: 0x00003F01 File Offset: 0x00002101
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00003F0A File Offset: 0x0000210A
		// (set) Token: 0x060002CE RID: 718 RVA: 0x00003F12 File Offset: 0x00002112
		[JsonProperty]
		public bool IsAccepted { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00003F1B File Offset: 0x0000211B
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x00003F23 File Offset: 0x00002123
		[JsonProperty]
		public bool IsBlocked { get; private set; }

		// Token: 0x060002D1 RID: 721 RVA: 0x00003F2C File Offset: 0x0000212C
		public FriendRequestResponseMessage()
		{
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00003F34 File Offset: 0x00002134
		public FriendRequestResponseMessage(PlayerId playerId, bool dontUseNameForUnknownPlayer, bool isAccepted, bool isBlocked)
		{
			this.PlayerId = playerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
			this.IsAccepted = isAccepted;
			this.IsBlocked = isBlocked;
		}
	}
}
