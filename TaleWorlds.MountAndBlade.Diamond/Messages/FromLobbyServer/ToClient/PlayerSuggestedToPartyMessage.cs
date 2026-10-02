using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005F RID: 95
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerSuggestedToPartyMessage : Message
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000035C6 File Offset: 0x000017C6
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000035CE File Offset: 0x000017CE
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000035D7 File Offset: 0x000017D7
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000035DF File Offset: 0x000017DF
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000035E8 File Offset: 0x000017E8
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000035F0 File Offset: 0x000017F0
		[JsonProperty]
		public PlayerId SuggestingPlayerId { get; private set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000035F9 File Offset: 0x000017F9
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00003601 File Offset: 0x00001801
		[JsonProperty]
		public string SuggestingPlayerName { get; private set; }

		// Token: 0x060001F2 RID: 498 RVA: 0x0000360A File Offset: 0x0000180A
		public PlayerSuggestedToPartyMessage()
		{
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00003612 File Offset: 0x00001812
		public PlayerSuggestedToPartyMessage(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.SuggestingPlayerId = suggestingPlayerId;
			this.SuggestingPlayerName = suggestingPlayerName;
		}
	}
}
