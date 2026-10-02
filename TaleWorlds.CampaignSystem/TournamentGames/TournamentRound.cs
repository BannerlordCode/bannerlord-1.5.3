using System;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002EA RID: 746
	public class TournamentRound
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x000A90E7 File Offset: 0x000A72E7
		// (set) Token: 0x06002899 RID: 10393 RVA: 0x000A90EF File Offset: 0x000A72EF
		public TournamentMatch[] Matches { get; private set; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x000A90F8 File Offset: 0x000A72F8
		// (set) Token: 0x0600289B RID: 10395 RVA: 0x000A9100 File Offset: 0x000A7300
		public int CurrentMatchIndex { get; private set; }

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x000A9109 File Offset: 0x000A7309
		public TournamentMatch CurrentMatch
		{
			get
			{
				if (this.CurrentMatchIndex >= this.Matches.Length)
				{
					return null;
				}
				return this.Matches[this.CurrentMatchIndex];
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000A912C File Offset: 0x000A732C
		public TournamentRound(int participantCount, int numberOfMatches, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this.Matches = new TournamentMatch[numberOfMatches];
			this.CurrentMatchIndex = 0;
			int num = participantCount / numberOfMatches;
			for (int i = 0; i < numberOfMatches; i++)
			{
				this.Matches[i] = new TournamentMatch(num, numberOfTeamsPerMatch, numberOfWinnerParticipants / numberOfMatches, qualificationMode);
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000A9178 File Offset: 0x000A7378
		public void OnMatchEnded()
		{
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000A9198 File Offset: 0x000A7398
		public void EndMatch()
		{
			this.CurrentMatch.End();
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000A91C0 File Offset: 0x000A73C0
		public void AddParticipant(TournamentParticipant participant, bool firstTime = false)
		{
			foreach (TournamentMatch tournamentMatch in this.Matches)
			{
				if (tournamentMatch.IsParticipantRequired())
				{
					tournamentMatch.AddParticipant(participant, firstTime);
					return;
				}
			}
		}
	}
}
