using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Tournament
{
	// Token: 0x02000010 RID: 16
	public class TournamentTeamVM : ViewModel
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00006635 File Offset: 0x00004835
		public List<TournamentParticipantVM> Participants { get; }

		// Token: 0x06000107 RID: 263 RVA: 0x00006640 File Offset: 0x00004840
		public TournamentTeamVM()
		{
			this.Participant1 = new TournamentParticipantVM();
			this.Participant2 = new TournamentParticipantVM();
			this.Participant3 = new TournamentParticipantVM();
			this.Participant4 = new TournamentParticipantVM();
			this.Participant5 = new TournamentParticipantVM();
			this.Participant6 = new TournamentParticipantVM();
			this.Participant7 = new TournamentParticipantVM();
			this.Participant8 = new TournamentParticipantVM();
			this.Participants = new List<TournamentParticipantVM> { this.Participant1, this.Participant2, this.Participant3, this.Participant4, this.Participant5, this.Participant6, this.Participant7, this.Participant8 };
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000671D File Offset: 0x0000491D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Participants.ForEach(delegate(TournamentParticipantVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000674F File Offset: 0x0000494F
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00006757 File Offset: 0x00004957
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00006775 File Offset: 0x00004975
		// (set) Token: 0x0600010C RID: 268 RVA: 0x0000677D File Offset: 0x0000497D
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000679B File Offset: 0x0000499B
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000067A3 File Offset: 0x000049A3
		[DataSourceProperty]
		public TournamentParticipantVM Participant1
		{
			get
			{
				return this._participant1;
			}
			set
			{
				if (value != this._participant1)
				{
					this._participant1 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant1");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000067C1 File Offset: 0x000049C1
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000067C9 File Offset: 0x000049C9
		[DataSourceProperty]
		public TournamentParticipantVM Participant2
		{
			get
			{
				return this._participant2;
			}
			set
			{
				if (value != this._participant2)
				{
					this._participant2 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant2");
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000067E7 File Offset: 0x000049E7
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000067EF File Offset: 0x000049EF
		[DataSourceProperty]
		public TournamentParticipantVM Participant3
		{
			get
			{
				return this._participant3;
			}
			set
			{
				if (value != this._participant3)
				{
					this._participant3 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant3");
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000680D File Offset: 0x00004A0D
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00006815 File Offset: 0x00004A15
		[DataSourceProperty]
		public TournamentParticipantVM Participant4
		{
			get
			{
				return this._participant4;
			}
			set
			{
				if (value != this._participant4)
				{
					this._participant4 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant4");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006833 File Offset: 0x00004A33
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000683B File Offset: 0x00004A3B
		[DataSourceProperty]
		public TournamentParticipantVM Participant5
		{
			get
			{
				return this._participant5;
			}
			set
			{
				if (value != this._participant5)
				{
					this._participant5 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant5");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006859 File Offset: 0x00004A59
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00006861 File Offset: 0x00004A61
		[DataSourceProperty]
		public TournamentParticipantVM Participant6
		{
			get
			{
				return this._participant6;
			}
			set
			{
				if (value != this._participant6)
				{
					this._participant6 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant6");
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000687F File Offset: 0x00004A7F
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00006887 File Offset: 0x00004A87
		[DataSourceProperty]
		public TournamentParticipantVM Participant7
		{
			get
			{
				return this._participant7;
			}
			set
			{
				if (value != this._participant7)
				{
					this._participant7 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant7");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000068A5 File Offset: 0x00004AA5
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000068AD File Offset: 0x00004AAD
		[DataSourceProperty]
		public TournamentParticipantVM Participant8
		{
			get
			{
				return this._participant8;
			}
			set
			{
				if (value != this._participant8)
				{
					this._participant8 = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "Participant8");
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000068CB File Offset: 0x00004ACB
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000068D3 File Offset: 0x00004AD3
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

		// Token: 0x0600011F RID: 287 RVA: 0x000068F4 File Offset: 0x00004AF4
		public void Initialize()
		{
			this.IsValid = this._team != null;
			for (int i = 0; i < this.Count; i++)
			{
				TournamentParticipant tournamentParticipant = this._team.Participants.ElementAtOrDefault<TournamentParticipant>(i);
				this.Participants[i].Refresh(tournamentParticipant, Color.FromUint(this._team.TeamColor));
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006955 File Offset: 0x00004B55
		public void Initialize(TournamentTeam team)
		{
			this._team = team;
			this.Count = team.TeamSize;
			this.IsValid = this._team != null;
			this.Initialize();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006980 File Offset: 0x00004B80
		public void Refresh()
		{
			this.IsValid = this._team != null;
			base.OnPropertyChanged("Count");
			int num = 0;
			foreach (TournamentParticipantVM tournamentParticipantVM in this.Participants.Where<TournamentParticipantVM>((TournamentParticipantVM p) => p.IsValid))
			{
				base.OnPropertyChanged("Participant" + num);
				tournamentParticipantVM.Refresh();
				num++;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006A24 File Offset: 0x00004C24
		public IEnumerable<TournamentParticipantVM> GetParticipants()
		{
			if (this.Participant1.IsValid)
			{
				yield return this.Participant1;
			}
			if (this.Participant2.IsValid)
			{
				yield return this.Participant2;
			}
			if (this.Participant3.IsValid)
			{
				yield return this.Participant3;
			}
			if (this.Participant4.IsValid)
			{
				yield return this.Participant4;
			}
			if (this.Participant5.IsValid)
			{
				yield return this.Participant5;
			}
			if (this.Participant6.IsValid)
			{
				yield return this.Participant6;
			}
			if (this.Participant7.IsValid)
			{
				yield return this.Participant7;
			}
			if (this.Participant8.IsValid)
			{
				yield return this.Participant8;
			}
			yield break;
		}

		// Token: 0x04000075 RID: 117
		private TournamentTeam _team;

		// Token: 0x04000077 RID: 119
		private int _count = -1;

		// Token: 0x04000078 RID: 120
		private TournamentParticipantVM _participant1;

		// Token: 0x04000079 RID: 121
		private TournamentParticipantVM _participant2;

		// Token: 0x0400007A RID: 122
		private TournamentParticipantVM _participant3;

		// Token: 0x0400007B RID: 123
		private TournamentParticipantVM _participant4;

		// Token: 0x0400007C RID: 124
		private TournamentParticipantVM _participant5;

		// Token: 0x0400007D RID: 125
		private TournamentParticipantVM _participant6;

		// Token: 0x0400007E RID: 126
		private TournamentParticipantVM _participant7;

		// Token: 0x0400007F RID: 127
		private TournamentParticipantVM _participant8;

		// Token: 0x04000080 RID: 128
		private int _score;

		// Token: 0x04000081 RID: 129
		private bool _isValid;
	}
}
