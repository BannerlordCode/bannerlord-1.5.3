using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Tournament
{
	// Token: 0x0200000D RID: 13
	public class TournamentMatchVM : ViewModel
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005A44 File Offset: 0x00003C44
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00005A4C File Offset: 0x00003C4C
		public TournamentMatch Match { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005A55 File Offset: 0x00003C55
		public List<TournamentTeamVM> Teams { get; }

		// Token: 0x060000B5 RID: 181 RVA: 0x00005A60 File Offset: 0x00003C60
		public TournamentMatchVM()
		{
			this.Team1 = new TournamentTeamVM();
			this.Team2 = new TournamentTeamVM();
			this.Team3 = new TournamentTeamVM();
			this.Team4 = new TournamentTeamVM();
			this.Teams = new List<TournamentTeamVM> { this.Team1, this.Team2, this.Team3, this.Team4 };
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005AE8 File Offset: 0x00003CE8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Teams.ForEach(delegate(TournamentTeamVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005B1C File Offset: 0x00003D1C
		public void Initialize()
		{
			foreach (TournamentTeamVM tournamentTeamVM in this.Teams)
			{
				if (tournamentTeamVM.IsValid)
				{
					tournamentTeamVM.Initialize();
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005B78 File Offset: 0x00003D78
		public void Initialize(TournamentMatch match)
		{
			int num = 0;
			this.Match = match;
			this.IsValid = this.Match != null;
			this.Count = match.Teams.Count<TournamentTeam>();
			foreach (TournamentTeam tournamentTeam in match.Teams)
			{
				this.Teams[num].Initialize(tournamentTeam);
				num++;
			}
			this.State = 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005C04 File Offset: 0x00003E04
		public void Refresh(bool forceRefresh)
		{
			if (forceRefresh)
			{
				base.OnPropertyChanged("Count");
			}
			for (int i = 0; i < this.Count; i++)
			{
				TournamentTeamVM tournamentTeamVM = this.Teams[i];
				if (forceRefresh)
				{
					base.OnPropertyChanged("Team" + i + 1);
				}
				tournamentTeamVM.Refresh();
				for (int j = 0; j < tournamentTeamVM.Count; j++)
				{
					TournamentParticipantVM tournamentParticipantVM = tournamentTeamVM.Participants[j];
					tournamentParticipantVM.Score = tournamentParticipantVM.Participant.Score.ToString();
					tournamentParticipantVM.IsQualifiedForNextRound = this.Match.Winners.Contains(tournamentParticipantVM.Participant);
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005CBC File Offset: 0x00003EBC
		public void RefreshActiveMatch()
		{
			for (int i = 0; i < this.Count; i++)
			{
				TournamentTeamVM tournamentTeamVM = this.Teams[i];
				for (int j = 0; j < tournamentTeamVM.Count; j++)
				{
					TournamentParticipantVM tournamentParticipantVM = tournamentTeamVM.Participants[j];
					tournamentParticipantVM.Score = tournamentParticipantVM.Participant.Score.ToString();
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005D1C File Offset: 0x00003F1C
		public void Refresh(TournamentMatchVM target)
		{
			base.OnPropertyChanged("Count");
			int num = 0;
			foreach (TournamentTeamVM tournamentTeamVM in this.Teams.Where<TournamentTeamVM>((TournamentTeamVM t) => t.IsValid))
			{
				base.OnPropertyChanged("Team" + num + 1);
				tournamentTeamVM.Refresh();
				num++;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005DB8 File Offset: 0x00003FB8
		public IEnumerable<TournamentParticipantVM> GetParticipants()
		{
			List<TournamentParticipantVM> list = new List<TournamentParticipantVM>();
			if (this.Team1.IsValid)
			{
				list.AddRange(this.Team1.GetParticipants());
			}
			if (this.Team2.IsValid)
			{
				list.AddRange(this.Team2.GetParticipants());
			}
			if (this.Team3.IsValid)
			{
				list.AddRange(this.Team3.GetParticipants());
			}
			if (this.Team4.IsValid)
			{
				list.AddRange(this.Team4.GetParticipants());
			}
			return list;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005E44 File Offset: 0x00004044
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00005E4C File Offset: 0x0000404C
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005E6A File Offset: 0x0000406A
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005E72 File Offset: 0x00004072
		[DataSourceProperty]
		public int State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (value != this._state)
				{
					this._state = value;
					base.OnPropertyChangedWithValue(value, "State");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005E90 File Offset: 0x00004090
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00005E98 File Offset: 0x00004098
		[DataSourceProperty]
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				if (value != this._count)
				{
					this._count = value;
					base.OnPropertyChangedWithValue(value, "Count");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005EB6 File Offset: 0x000040B6
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005EBE File Offset: 0x000040BE
		[DataSourceProperty]
		public TournamentTeamVM Team1
		{
			get
			{
				return this._team1;
			}
			set
			{
				if (value != this._team1)
				{
					this._team1 = value;
					base.OnPropertyChangedWithValue<TournamentTeamVM>(value, "Team1");
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005EDC File Offset: 0x000040DC
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00005EE4 File Offset: 0x000040E4
		[DataSourceProperty]
		public TournamentTeamVM Team2
		{
			get
			{
				return this._team2;
			}
			set
			{
				if (value != this._team2)
				{
					this._team2 = value;
					base.OnPropertyChangedWithValue<TournamentTeamVM>(value, "Team2");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005F02 File Offset: 0x00004102
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005F0A File Offset: 0x0000410A
		[DataSourceProperty]
		public TournamentTeamVM Team3
		{
			get
			{
				return this._team3;
			}
			set
			{
				if (value != this._team3)
				{
					this._team3 = value;
					base.OnPropertyChangedWithValue<TournamentTeamVM>(value, "Team3");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005F28 File Offset: 0x00004128
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00005F30 File Offset: 0x00004130
		[DataSourceProperty]
		public TournamentTeamVM Team4
		{
			get
			{
				return this._team4;
			}
			set
			{
				if (value != this._team4)
				{
					this._team4 = value;
					base.OnPropertyChangedWithValue<TournamentTeamVM>(value, "Team4");
				}
			}
		}

		// Token: 0x04000054 RID: 84
		private TournamentTeamVM _team1;

		// Token: 0x04000055 RID: 85
		private TournamentTeamVM _team2;

		// Token: 0x04000056 RID: 86
		private TournamentTeamVM _team3;

		// Token: 0x04000057 RID: 87
		private TournamentTeamVM _team4;

		// Token: 0x04000058 RID: 88
		private int _count = -1;

		// Token: 0x04000059 RID: 89
		private int _state = -1;

		// Token: 0x0400005A RID: 90
		private bool _isValid;

		// Token: 0x02000074 RID: 116
		public enum TournamentMatchState
		{
			// Token: 0x04000369 RID: 873
			Unfinished,
			// Token: 0x0400036A RID: 874
			Current,
			// Token: 0x0400036B RID: 875
			Over,
			// Token: 0x0400036C RID: 876
			Active
		}
	}
}
