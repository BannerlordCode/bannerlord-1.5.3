using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001E RID: 30
	public class MissionScoreboardPlayerSortControllerVM : ViewModel
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007428 File Offset: 0x00005628
		public MissionScoreboardPlayerSortControllerVM(ref MBBindingList<MissionScoreboardPlayerVM> listToControl)
		{
			this._listToControl = listToControl;
			this._nameComparer = new MissionScoreboardPlayerSortControllerVM.ItemNameComparer();
			this._scoreComparer = new MissionScoreboardPlayerSortControllerVM.ItemScoreComparer();
			this._killComparer = new MissionScoreboardPlayerSortControllerVM.ItemKillComparer();
			this._assistComparer = new MissionScoreboardPlayerSortControllerVM.ItemAssistComparer();
			this.ExecuteSortByScore();
			this.RefreshValues();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007498 File Offset: 0x00005698
		public override void RefreshValues()
		{
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.ScoreText = GameTexts.FindText("str_scoreboard_header", "score").ToString();
			this.KillText = GameTexts.FindText("str_scoreboard_header", "kill").ToString();
			this.AssistText = GameTexts.FindText("str_scoreboard_header", "assist").ToString();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007510 File Offset: 0x00005710
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsScoreSelected)
			{
				this._listToControl.Sort(this._scoreComparer);
				return;
			}
			if (this.IsKillSelected)
			{
				this._listToControl.Sort(this._killComparer);
				return;
			}
			if (this.IsAssistSelected)
			{
				this._listToControl.Sort(this._assistComparer);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007584 File Offset: 0x00005784
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				int nameState2 = this.NameState;
				this.NameState = nameState2 + 1;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000075F0 File Offset: 0x000057F0
		public void ExecuteSortByScore()
		{
			int scoreState = this.ScoreState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.ScoreState = (scoreState + 1) % 3;
			if (this.ScoreState == 0)
			{
				int scoreState2 = this.ScoreState;
				this.ScoreState = scoreState2 + 1;
			}
			this._scoreComparer.SetSortMode(this.ScoreState == 1);
			this._listToControl.Sort(this._scoreComparer);
			this.IsScoreSelected = true;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000765C File Offset: 0x0000585C
		public void ExecuteSortByKill()
		{
			int killState = this.KillState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.KillState = (killState + 1) % 3;
			if (this.KillState == 0)
			{
				int killState2 = this.KillState;
				this.KillState = killState2 + 1;
			}
			this._killComparer.SetSortMode(this.KillState == 1);
			this._listToControl.Sort(this._killComparer);
			this.IsKillSelected = true;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000076C8 File Offset: 0x000058C8
		public void ExecuteSortByAssist()
		{
			int assistState = this.AssistState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.AssistState = (assistState + 1) % 3;
			if (this.AssistState == 0)
			{
				int assistState2 = this.AssistState;
				this.AssistState = assistState2 + 1;
			}
			this._assistComparer.SetSortMode(this.AssistState == 1);
			this._listToControl.Sort(this._assistComparer);
			this.IsAssistSelected = true;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007732 File Offset: 0x00005932
		private void SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState state)
		{
			this.NameState = (int)state;
			this.ScoreState = (int)state;
			this.KillState = (int)state;
			this.AssistState = (int)state;
			this.IsNameSelected = false;
			this.IsScoreSelected = false;
			this.IsKillSelected = false;
			this.IsAssistSelected = false;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000776C File Offset: 0x0000596C
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00007774 File Offset: 0x00005974
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007797 File Offset: 0x00005997
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000779F File Offset: 0x0000599F
		[DataSourceProperty]
		public string ScoreText
		{
			get
			{
				return this._scoreText;
			}
			set
			{
				if (value != this._scoreText)
				{
					this._scoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "ScoreText");
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x000077C2 File Offset: 0x000059C2
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x000077CA File Offset: 0x000059CA
		[DataSourceProperty]
		public string KillText
		{
			get
			{
				return this._killText;
			}
			set
			{
				if (value != this._killText)
				{
					this._killText = value;
					base.OnPropertyChangedWithValue<string>(value, "KillText");
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000077ED File Offset: 0x000059ED
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x000077F5 File Offset: 0x000059F5
		[DataSourceProperty]
		public string AssistText
		{
			get
			{
				return this._assistText;
			}
			set
			{
				if (value != this._assistText)
				{
					this._assistText = value;
					base.OnPropertyChangedWithValue<string>(value, "AssistText");
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007818 File Offset: 0x00005A18
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00007820 File Offset: 0x00005A20
		[DataSourceProperty]
		public int NameState
		{
			get
			{
				return this._nameState;
			}
			set
			{
				if (value != this._nameState)
				{
					this._nameState = value;
					base.OnPropertyChangedWithValue(value, "NameState");
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000783E File Offset: 0x00005A3E
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00007846 File Offset: 0x00005A46
		[DataSourceProperty]
		public int ScoreState
		{
			get
			{
				return this._scoreState;
			}
			set
			{
				if (value != this._scoreState)
				{
					this._scoreState = value;
					base.OnPropertyChangedWithValue(value, "ScoreState");
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007864 File Offset: 0x00005A64
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000786C File Offset: 0x00005A6C
		[DataSourceProperty]
		public int KillState
		{
			get
			{
				return this._killState;
			}
			set
			{
				if (value != this._killState)
				{
					this._killState = value;
					base.OnPropertyChangedWithValue(value, "KillState");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000788A File Offset: 0x00005A8A
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00007892 File Offset: 0x00005A92
		[DataSourceProperty]
		public int AssistState
		{
			get
			{
				return this._assistState;
			}
			set
			{
				if (value != this._assistState)
				{
					this._assistState = value;
					base.OnPropertyChangedWithValue(value, "AssistState");
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000078B0 File Offset: 0x00005AB0
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x000078B8 File Offset: 0x00005AB8
		[DataSourceProperty]
		public bool IsNameSelected
		{
			get
			{
				return this._isNameSelected;
			}
			set
			{
				if (value != this._isNameSelected)
				{
					this._isNameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsNameSelected");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000078D6 File Offset: 0x00005AD6
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x000078DE File Offset: 0x00005ADE
		[DataSourceProperty]
		public bool IsScoreSelected
		{
			get
			{
				return this._isScoreSelected;
			}
			set
			{
				if (value != this._isScoreSelected)
				{
					this._isScoreSelected = value;
					base.OnPropertyChangedWithValue(value, "IsScoreSelected");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000078FC File Offset: 0x00005AFC
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00007904 File Offset: 0x00005B04
		[DataSourceProperty]
		public bool IsKillSelected
		{
			get
			{
				return this._isKillSelected;
			}
			set
			{
				if (value != this._isKillSelected)
				{
					this._isKillSelected = value;
					base.OnPropertyChangedWithValue(value, "IsKillSelected");
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007922 File Offset: 0x00005B22
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000792A File Offset: 0x00005B2A
		[DataSourceProperty]
		public bool IsAssistSelected
		{
			get
			{
				return this._isAssistSelected;
			}
			set
			{
				if (value != this._isAssistSelected)
				{
					this._isAssistSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAssistSelected");
				}
			}
		}

		// Token: 0x040000EA RID: 234
		private const string _nameHeaderID = "name";

		// Token: 0x040000EB RID: 235
		private const string _scoreHeaderID = "score";

		// Token: 0x040000EC RID: 236
		private const string _killHeaderID = "kill";

		// Token: 0x040000ED RID: 237
		private const string _assistHeaderID = "assist";

		// Token: 0x040000EE RID: 238
		private readonly MBBindingList<MissionScoreboardPlayerVM> _listToControl;

		// Token: 0x040000EF RID: 239
		private readonly MissionScoreboardPlayerSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040000F0 RID: 240
		private readonly MissionScoreboardPlayerSortControllerVM.ItemScoreComparer _scoreComparer;

		// Token: 0x040000F1 RID: 241
		private readonly MissionScoreboardPlayerSortControllerVM.ItemKillComparer _killComparer;

		// Token: 0x040000F2 RID: 242
		private readonly MissionScoreboardPlayerSortControllerVM.ItemAssistComparer _assistComparer;

		// Token: 0x040000F3 RID: 243
		private string _nameText;

		// Token: 0x040000F4 RID: 244
		private string _scoreText;

		// Token: 0x040000F5 RID: 245
		private string _killText;

		// Token: 0x040000F6 RID: 246
		private string _assistText;

		// Token: 0x040000F7 RID: 247
		private int _nameState = 1;

		// Token: 0x040000F8 RID: 248
		private int _scoreState = 1;

		// Token: 0x040000F9 RID: 249
		private int _killState = 1;

		// Token: 0x040000FA RID: 250
		private int _assistState = 1;

		// Token: 0x040000FB RID: 251
		private bool _isNameSelected;

		// Token: 0x040000FC RID: 252
		private bool _isScoreSelected;

		// Token: 0x040000FD RID: 253
		private bool _isKillSelected;

		// Token: 0x040000FE RID: 254
		private bool _isAssistSelected;

		// Token: 0x020000C8 RID: 200
		private enum SortState
		{
			// Token: 0x0400084A RID: 2122
			Default,
			// Token: 0x0400084B RID: 2123
			Ascending,
			// Token: 0x0400084C RID: 2124
			Descending
		}

		// Token: 0x020000C9 RID: 201
		public abstract class ItemComparerBase : IComparer<MissionScoreboardPlayerVM>
		{
			// Token: 0x06001188 RID: 4488 RVA: 0x00036B1A File Offset: 0x00034D1A
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06001189 RID: 4489
			public abstract int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y);

			// Token: 0x0400084D RID: 2125
			protected bool _isAscending;
		}

		// Token: 0x020000CA RID: 202
		public class ItemNameComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600118B RID: 4491 RVA: 0x00036B2B File Offset: 0x00034D2B
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				return y.Name.CompareTo(x.Name) * (this._isAscending ? (-1) : 1);
			}
		}

		// Token: 0x020000CB RID: 203
		public class ItemScoreComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600118D RID: 4493 RVA: 0x00036B54 File Offset: 0x00034D54
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				return y.Score.CompareTo(x.Score) * (this._isAscending ? (-1) : 1);
			}
		}

		// Token: 0x020000CC RID: 204
		public class ItemKillComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600118F RID: 4495 RVA: 0x00036B8C File Offset: 0x00034D8C
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				MissionScoreboardStatItemVM missionScoreboardStatItemVM = x.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "kill");
				MissionScoreboardStatItemVM missionScoreboardStatItemVM2 = y.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "kill");
				if (missionScoreboardStatItemVM != null && missionScoreboardStatItemVM2 != null)
				{
					return int.Parse(missionScoreboardStatItemVM2.Item).CompareTo(int.Parse(missionScoreboardStatItemVM.Item)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}

		// Token: 0x020000CD RID: 205
		public class ItemAssistComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001191 RID: 4497 RVA: 0x00036C2C File Offset: 0x00034E2C
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				MissionScoreboardStatItemVM missionScoreboardStatItemVM = x.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "assist");
				MissionScoreboardStatItemVM missionScoreboardStatItemVM2 = y.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "assist");
				if (missionScoreboardStatItemVM != null && missionScoreboardStatItemVM2 != null)
				{
					return int.Parse(missionScoreboardStatItemVM2.Item).CompareTo(int.Parse(missionScoreboardStatItemVM.Item)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}
	}
}
