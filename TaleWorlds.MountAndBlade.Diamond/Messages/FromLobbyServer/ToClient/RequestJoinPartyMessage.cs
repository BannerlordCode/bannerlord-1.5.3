using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000065 RID: 101
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RequestJoinPartyMessage : Message
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000036F7 File Offset: 0x000018F7
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000036FF File Offset: 0x000018FF
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00003708 File Offset: 0x00001908
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00003710 File Offset: 0x00001910
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00003719 File Offset: 0x00001919
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00003721 File Offset: 0x00001921
		[JsonProperty]
		public PlayerId ViaPlayerId { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000372A File Offset: 0x0000192A
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00003732 File Offset: 0x00001932
		[JsonProperty]
		public string ViaPlayerName { get; private set; }

		// Token: 0x0600020F RID: 527 RVA: 0x0000373B File Offset: 0x0000193B
		public RequestJoinPartyMessage()
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00003743 File Offset: 0x00001943
		public RequestJoinPartyMessage(PlayerId playerId, string playerName, PlayerId viaPlayerId, string viaPlayerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ViaPlayerId = viaPlayerId;
			this.ViaPlayerName = viaPlayerName;
		}
	}
}
