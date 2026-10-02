using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004E RID: 78
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameRequestMessage : Message
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00003273 File Offset: 0x00001473
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000327B File Offset: 0x0000147B
		[JsonProperty]
		public Guid ChallengerPartyId { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00003284 File Offset: 0x00001484
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000328C File Offset: 0x0000148C
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00003295 File Offset: 0x00001495
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000329D File Offset: 0x0000149D
		[JsonProperty]
		public string Sigil { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000032A6 File Offset: 0x000014A6
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000032AE File Offset: 0x000014AE
		[JsonProperty]
		public PlayerId[] ChallengerPlayers { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000032B7 File Offset: 0x000014B7
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000032BF File Offset: 0x000014BF
		[JsonProperty]
		public PlayerId ChallengerPartyLeaderId { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000032C8 File Offset: 0x000014C8
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000032D0 File Offset: 0x000014D0
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x060001A9 RID: 425 RVA: 0x000032D9 File Offset: 0x000014D9
		public JoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000032E1 File Offset: 0x000014E1
		public JoinPremadeGameRequestMessage(Guid challengerPartyId, string clanName, string sigil, PlayerId[] challengerPlayers, PlayerId challengerPartyLeaderId, PremadeGameType premadeGameType)
		{
			this.ChallengerPartyId = challengerPartyId;
			this.ClanName = clanName;
			this.Sigil = sigil;
			this.ChallengerPlayers = challengerPlayers;
			this.ChallengerPartyLeaderId = challengerPartyLeaderId;
			this.PremadeGameType = premadeGameType;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00003316 File Offset: 0x00001516
		public static JoinPremadeGameRequestMessage CreateClanGameRequest(Guid challengerPartyId, string clanName, string sigil, PlayerId[] challengerPlayers)
		{
			return new JoinPremadeGameRequestMessage(challengerPartyId, clanName, sigil, challengerPlayers, PlayerId.Empty, PremadeGameType.Clan);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00003327 File Offset: 0x00001527
		public static JoinPremadeGameRequestMessage CreatePracticeGameRequest(Guid challengerPartyId, PlayerId leaderId, PlayerId[] challengerPlayers)
		{
			return new JoinPremadeGameRequestMessage(challengerPartyId, null, null, challengerPlayers, leaderId, PremadeGameType.Practice);
		}
	}
}
