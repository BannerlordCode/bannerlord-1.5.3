using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002EB RID: 747
	public class TournamentTeam
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000A91F7 File Offset: 0x000A73F7
		// (set) Token: 0x060028A2 RID: 10402 RVA: 0x000A91FF File Offset: 0x000A73FF
		public int TeamSize { get; private set; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000A9208 File Offset: 0x000A7408
		// (set) Token: 0x060028A4 RID: 10404 RVA: 0x000A9210 File Offset: 0x000A7410
		public uint TeamColor { get; private set; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x000A9219 File Offset: 0x000A7419
		// (set) Token: 0x060028A6 RID: 10406 RVA: 0x000A9221 File Offset: 0x000A7421
		public Banner TeamBanner { get; private set; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000A922A File Offset: 0x000A742A
		// (set) Token: 0x060028A8 RID: 10408 RVA: 0x000A9232 File Offset: 0x000A7432
		public bool IsPlayerTeam { get; private set; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000A923B File Offset: 0x000A743B
		public IEnumerable<TournamentParticipant> Participants
		{
			get
			{
				return this._participants.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x000A9248 File Offset: 0x000A7448
		public int Score
		{
			get
			{
				int num = 0;
				foreach (TournamentParticipant tournamentParticipant in this._participants)
				{
					num += tournamentParticipant.Score;
				}
				return num;
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000A92A0 File Offset: 0x000A74A0
		public TournamentTeam(int teamSize, uint teamColor, Banner teamBanner)
		{
			this.TeamColor = teamColor;
			this.TeamBanner = teamBanner;
			this.TeamSize = teamSize;
			this._participants = new List<TournamentParticipant>();
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000A92C8 File Offset: 0x000A74C8
		public bool IsParticipantRequired()
		{
			return this._participants.Count < this.TeamSize;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000A92DD File Offset: 0x000A74DD
		public void AddParticipant(TournamentParticipant participant)
		{
			participant.IsAssigned = true;
			this._participants.Add(participant);
			participant.SetTeam(this);
			if (participant.IsPlayer)
			{
				this.IsPlayerTeam = true;
			}
		}

		// Token: 0x04000BD2 RID: 3026
		private List<TournamentParticipant> _participants;
	}
}
