using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002E8 RID: 744
	public class TournamentMatch
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x000A8B13 File Offset: 0x000A6D13
		public IEnumerable<TournamentTeam> Teams
		{
			get
			{
				return this._teams.AsEnumerable<TournamentTeam>();
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x000A8B20 File Offset: 0x000A6D20
		public IEnumerable<TournamentParticipant> Participants
		{
			get
			{
				return this._participants.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x000A8B2D File Offset: 0x000A6D2D
		// (set) Token: 0x0600287B RID: 10363 RVA: 0x000A8B35 File Offset: 0x000A6D35
		public TournamentMatch.MatchState State { get; private set; }

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x000A8B3E File Offset: 0x000A6D3E
		public IEnumerable<TournamentParticipant> Winners
		{
			get
			{
				return this._winners.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000A8B4B File Offset: 0x000A6D4B
		public bool IsReady
		{
			get
			{
				return this.State == TournamentMatch.MatchState.Ready;
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000A8B58 File Offset: 0x000A6D58
		public TournamentMatch(int participantCount, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this._participants = new List<TournamentParticipant>();
			this._participantCount = participantCount;
			this._teams = new TournamentTeam[numberOfTeamsPerMatch];
			this._winners = new List<TournamentParticipant>();
			this._numberOfWinnerParticipants = numberOfWinnerParticipants;
			this.QualificationMode = qualificationMode;
			this._teamSize = participantCount / numberOfTeamsPerMatch;
			int[] array = new int[] { 119, 118, 120, 121 };
			int num = 0;
			for (int i = 0; i < numberOfTeamsPerMatch; i++)
			{
				this._teams[i] = new TournamentTeam(this._teamSize, BannerManager.GetColor(array[num]), Banner.CreateOneColoredEmptyBanner(array[num]));
				num++;
				num %= 4;
			}
			this.State = TournamentMatch.MatchState.Ready;
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000A8BFE File Offset: 0x000A6DFE
		public void End()
		{
			this.State = TournamentMatch.MatchState.Finished;
			this._winners = this.GetWinners();
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000A8C14 File Offset: 0x000A6E14
		public void Start()
		{
			if (this.State != TournamentMatch.MatchState.Started)
			{
				this.State = TournamentMatch.MatchState.Started;
				foreach (TournamentParticipant tournamentParticipant in this.Participants)
				{
					tournamentParticipant.ResetScore();
				}
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000A8C70 File Offset: 0x000A6E70
		public TournamentParticipant GetParticipant(int uniqueSeed)
		{
			return this._participants.FirstOrDefault<TournamentParticipant>((TournamentParticipant p) => p.Descriptor.CompareTo(uniqueSeed) == 0);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000A8CA1 File Offset: 0x000A6EA1
		public bool IsParticipantRequired()
		{
			return this._participants.Count < this._participantCount;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000A8CB8 File Offset: 0x000A6EB8
		public void AddParticipant(TournamentParticipant participant, bool firstTime)
		{
			this._participants.Add(participant);
			foreach (TournamentTeam tournamentTeam in this.Teams)
			{
				if (tournamentTeam.IsParticipantRequired() && ((participant.Team != null && participant.Team.TeamColor == tournamentTeam.TeamColor) || firstTime))
				{
					tournamentTeam.AddParticipant(participant);
					return;
				}
			}
			foreach (TournamentTeam tournamentTeam2 in this.Teams)
			{
				if (tournamentTeam2.IsParticipantRequired())
				{
					tournamentTeam2.AddParticipant(participant);
					break;
				}
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000A8D84 File Offset: 0x000A6F84
		public bool IsPlayerParticipating()
		{
			return this.Participants.Any<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000A8DB0 File Offset: 0x000A6FB0
		public bool IsPlayerWinner()
		{
			if (this.IsPlayerParticipating())
			{
				return this.GetWinners().Any<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
			}
			return false;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000A8DE8 File Offset: 0x000A6FE8
		private List<TournamentParticipant> GetWinners()
		{
			List<TournamentParticipant> list = new List<TournamentParticipant>();
			if (this.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
			{
				List<TournamentParticipant> list2 = this._participants.OrderByDescending<TournamentParticipant, int>((TournamentParticipant x) => x.Score).Take<TournamentParticipant>(this._numberOfWinnerParticipants).ToList<TournamentParticipant>();
				using (List<TournamentParticipant>.Enumerator enumerator = this._participants.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TournamentParticipant tournamentParticipant = enumerator.Current;
						if (list2.Contains(tournamentParticipant))
						{
							tournamentParticipant.IsAssigned = false;
							list.Add(tournamentParticipant);
						}
					}
					return list;
				}
			}
			if (this.QualificationMode == TournamentGame.QualificationMode.TeamScore)
			{
				IOrderedEnumerable<TournamentTeam> orderedEnumerable = this._teams.OrderByDescending<TournamentTeam, int>((TournamentTeam x) => x.Score);
				List<TournamentTeam> list3 = orderedEnumerable.Take<TournamentTeam>(this._numberOfWinnerParticipants / this._teamSize).ToList<TournamentTeam>();
				foreach (TournamentTeam tournamentTeam in this._teams)
				{
					if (list3.Contains(tournamentTeam))
					{
						foreach (TournamentParticipant tournamentParticipant2 in tournamentTeam.Participants)
						{
							tournamentParticipant2.IsAssigned = false;
							list.Add(tournamentParticipant2);
						}
					}
				}
				foreach (TournamentTeam tournamentTeam2 in orderedEnumerable)
				{
					int num = this._numberOfWinnerParticipants - list.Count;
					if (tournamentTeam2.Participants.Count<TournamentParticipant>() >= num)
					{
						IOrderedEnumerable<TournamentParticipant> orderedEnumerable2 = tournamentTeam2.Participants.OrderByDescending<TournamentParticipant, int>((TournamentParticipant x) => x.Score);
						list.AddRange(orderedEnumerable2.Take<TournamentParticipant>(num));
						break;
					}
					list.AddRange(tournamentTeam2.Participants);
				}
			}
			return list;
		}

		// Token: 0x04000BC2 RID: 3010
		private readonly int _numberOfWinnerParticipants;

		// Token: 0x04000BC3 RID: 3011
		public readonly TournamentGame.QualificationMode QualificationMode;

		// Token: 0x04000BC4 RID: 3012
		private readonly TournamentTeam[] _teams;

		// Token: 0x04000BC5 RID: 3013
		private readonly List<TournamentParticipant> _participants;

		// Token: 0x04000BC7 RID: 3015
		private List<TournamentParticipant> _winners;

		// Token: 0x04000BC8 RID: 3016
		private readonly int _participantCount;

		// Token: 0x04000BC9 RID: 3017
		private int _teamSize;

		// Token: 0x020006B2 RID: 1714
		public enum MatchState
		{
			// Token: 0x04001B8E RID: 7054
			Ready,
			// Token: 0x04001B8F RID: 7055
			Started,
			// Token: 0x04001B90 RID: 7056
			Finished
		}
	}
}
