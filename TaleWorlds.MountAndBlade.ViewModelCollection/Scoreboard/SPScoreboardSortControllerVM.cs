using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000014 RID: 20
	public class SPScoreboardSortControllerVM : ViewModel
	{
		// Token: 0x0600017D RID: 381 RVA: 0x00005E84 File Offset: 0x00004084
		public SPScoreboardSortControllerVM(ref MBBindingList<SPScoreboardPartyVM> listToControl)
		{
			this._listToControl = listToControl;
			this._remainingComparer = new SPScoreboardSortControllerVM.ItemRemainingComparer();
			this._killComparer = new SPScoreboardSortControllerVM.ItemKillComparer();
			this._upgradeComparer = new SPScoreboardSortControllerVM.ItemUpgradeComparer();
			this._deadComparer = new SPScoreboardSortControllerVM.ItemDeadComparer();
			this._woundedComparer = new SPScoreboardSortControllerVM.ItemWoundedComparer();
			this._routedComparer = new SPScoreboardSortControllerVM.ItemRoutedComparer();
			this._memberComparer = new SPScoreboardSortControllerVM.ItemMemberComparer();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005EEC File Offset: 0x000040EC
		public void ExecuteSortByRemaining()
		{
			int remainingState = this.RemainingState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.RemainingState = (remainingState + 1) % 3;
			this._remainingComparer.SetSortMode(this.RemainingState == 1);
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._remainingComparer;
			if (this.RemainingState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsRemainingSelected = this.RemainingState != 0;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005F90 File Offset: 0x00004190
		public void ExecuteSortByKill()
		{
			int killState = this.KillState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.KillState = (killState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._killComparer;
			if (this.KillState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._killComparer.SetSortMode(this.KillState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsKillSelected = this.KillState != 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006034 File Offset: 0x00004234
		public void ExecuteSortByUpgrade()
		{
			int upgradeState = this.UpgradeState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.UpgradeState = (upgradeState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._upgradeComparer;
			if (this.UpgradeState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._upgradeComparer.SetSortMode(this.UpgradeState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsUpgradeSelected = this.UpgradeState != 0;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000060D8 File Offset: 0x000042D8
		public void ExecuteSortByDead()
		{
			int deadState = this.DeadState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.DeadState = (deadState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._deadComparer;
			if (this.DeadState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._deadComparer.SetSortMode(this.DeadState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsDeadSelected = this.DeadState != 0;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000617C File Offset: 0x0000437C
		public void ExecuteSortByWounded()
		{
			int woundedState = this.WoundedState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.WoundedState = (woundedState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._woundedComparer;
			if (this.WoundedState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._woundedComparer.SetSortMode(this.WoundedState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsWoundedSelected = this.WoundedState != 0;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006220 File Offset: 0x00004420
		public void ExecuteSortByRouted()
		{
			int routedState = this.RoutedState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.RoutedState = (routedState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._routedComparer;
			if (this.RoutedState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._routedComparer.SetSortMode(this.RoutedState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsRoutedSelected = this.RoutedState != 0;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000062C4 File Offset: 0x000044C4
		private void SetAllStates(SPScoreboardSortControllerVM.SortState state)
		{
			this.RemainingState = (int)state;
			this.KillState = (int)state;
			this.UpgradeState = (int)state;
			this.DeadState = (int)state;
			this.WoundedState = (int)state;
			this.RoutedState = (int)state;
			this.IsRemainingSelected = false;
			this.IsKillSelected = false;
			this.IsUpgradeSelected = false;
			this.IsDeadSelected = false;
			this.IsWoundedSelected = false;
			this.IsRoutedSelected = false;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006325 File Offset: 0x00004525
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000632D File Offset: 0x0000452D
		[DataSourceProperty]
		public int RemainingState
		{
			get
			{
				return this._remainingState;
			}
			set
			{
				if (value != this._remainingState)
				{
					this._remainingState = value;
					base.OnPropertyChanged("RemainingState");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000634A File Offset: 0x0000454A
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006352 File Offset: 0x00004552
		[DataSourceProperty]
		public bool IsRemainingSelected
		{
			get
			{
				return this._isRemainingSelected;
			}
			set
			{
				if (value != this._isRemainingSelected)
				{
					this._isRemainingSelected = value;
					base.OnPropertyChanged("IsRemainingSelected");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000636F File Offset: 0x0000456F
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00006377 File Offset: 0x00004577
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
					base.OnPropertyChanged("KillState");
				}
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006394 File Offset: 0x00004594
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000639C File Offset: 0x0000459C
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
					base.OnPropertyChanged("IsKillSelected");
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000063B9 File Offset: 0x000045B9
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000063C1 File Offset: 0x000045C1
		[DataSourceProperty]
		public int UpgradeState
		{
			get
			{
				return this._upgradeState;
			}
			set
			{
				if (value != this._upgradeState)
				{
					this._upgradeState = value;
					base.OnPropertyChanged("UpgradeState");
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000063DE File Offset: 0x000045DE
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000063E6 File Offset: 0x000045E6
		[DataSourceProperty]
		public bool IsUpgradeSelected
		{
			get
			{
				return this._isUpgradeSelected;
			}
			set
			{
				if (value != this._isUpgradeSelected)
				{
					this._isUpgradeSelected = value;
					base.OnPropertyChanged("IsUpgradeSelected");
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006403 File Offset: 0x00004603
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000640B File Offset: 0x0000460B
		[DataSourceProperty]
		public int DeadState
		{
			get
			{
				return this._deadState;
			}
			set
			{
				if (value != this._deadState)
				{
					this._deadState = value;
					base.OnPropertyChanged("DeadState");
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006428 File Offset: 0x00004628
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00006430 File Offset: 0x00004630
		[DataSourceProperty]
		public bool IsDeadSelected
		{
			get
			{
				return this._isDeadSelected;
			}
			set
			{
				if (value != this._isDeadSelected)
				{
					this._isDeadSelected = value;
					base.OnPropertyChanged("IsDeadSelected");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000644D File Offset: 0x0000464D
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00006455 File Offset: 0x00004655
		[DataSourceProperty]
		public int WoundedState
		{
			get
			{
				return this._woundedState;
			}
			set
			{
				if (value != this._woundedState)
				{
					this._woundedState = value;
					base.OnPropertyChanged("WoundedState");
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006472 File Offset: 0x00004672
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000647A File Offset: 0x0000467A
		[DataSourceProperty]
		public bool IsWoundedSelected
		{
			get
			{
				return this._isWoundedSelected;
			}
			set
			{
				if (value != this._isWoundedSelected)
				{
					this._isWoundedSelected = value;
					base.OnPropertyChanged("IsWoundedSelected");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006497 File Offset: 0x00004697
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000649F File Offset: 0x0000469F
		[DataSourceProperty]
		public int RoutedState
		{
			get
			{
				return this._routedState;
			}
			set
			{
				if (value != this._routedState)
				{
					this._routedState = value;
					base.OnPropertyChanged("RoutedState");
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000064BC File Offset: 0x000046BC
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000064C4 File Offset: 0x000046C4
		[DataSourceProperty]
		public bool IsRoutedSelected
		{
			get
			{
				return this._isRoutedSelected;
			}
			set
			{
				if (value != this._isRoutedSelected)
				{
					this._isRoutedSelected = value;
					base.OnPropertyChanged("IsRoutedSelected");
				}
			}
		}

		// Token: 0x040000B0 RID: 176
		private readonly MBBindingList<SPScoreboardPartyVM> _listToControl;

		// Token: 0x040000B1 RID: 177
		private readonly SPScoreboardSortControllerVM.ItemRemainingComparer _remainingComparer;

		// Token: 0x040000B2 RID: 178
		private readonly SPScoreboardSortControllerVM.ItemKillComparer _killComparer;

		// Token: 0x040000B3 RID: 179
		private readonly SPScoreboardSortControllerVM.ItemUpgradeComparer _upgradeComparer;

		// Token: 0x040000B4 RID: 180
		private readonly SPScoreboardSortControllerVM.ItemDeadComparer _deadComparer;

		// Token: 0x040000B5 RID: 181
		private readonly SPScoreboardSortControllerVM.ItemWoundedComparer _woundedComparer;

		// Token: 0x040000B6 RID: 182
		private readonly SPScoreboardSortControllerVM.ItemRoutedComparer _routedComparer;

		// Token: 0x040000B7 RID: 183
		private readonly SPScoreboardSortControllerVM.ItemMemberComparer _memberComparer;

		// Token: 0x040000B8 RID: 184
		private int _remainingState;

		// Token: 0x040000B9 RID: 185
		private bool _isRemainingSelected;

		// Token: 0x040000BA RID: 186
		private int _killState;

		// Token: 0x040000BB RID: 187
		private bool _isKillSelected;

		// Token: 0x040000BC RID: 188
		private int _upgradeState;

		// Token: 0x040000BD RID: 189
		private bool _isUpgradeSelected;

		// Token: 0x040000BE RID: 190
		private int _deadState;

		// Token: 0x040000BF RID: 191
		private bool _isDeadSelected;

		// Token: 0x040000C0 RID: 192
		private int _woundedState;

		// Token: 0x040000C1 RID: 193
		private bool _isWoundedSelected;

		// Token: 0x040000C2 RID: 194
		private int _routedState;

		// Token: 0x040000C3 RID: 195
		private bool _isRoutedSelected;

		// Token: 0x0200009D RID: 157
		private enum SortState
		{
			// Token: 0x0400057E RID: 1406
			Default,
			// Token: 0x0400057F RID: 1407
			Ascending,
			// Token: 0x04000580 RID: 1408
			Descending
		}

		// Token: 0x0200009E RID: 158
		public abstract class ScoreboardUnitItemComparerBase : IComparer<SPScoreboardUnitVM>
		{
			// Token: 0x06000BC4 RID: 3012 RVA: 0x00028C18 File Offset: 0x00026E18
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06000BC5 RID: 3013
			public abstract int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y);

			// Token: 0x04000581 RID: 1409
			protected bool _isAscending;
		}

		// Token: 0x0200009F RID: 159
		public class ItemRemainingComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC7 RID: 3015 RVA: 0x00028C2C File Offset: 0x00026E2C
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Remaining.CompareTo(x.Score.Remaining) * -1;
				}
				return y.Score.Remaining.CompareTo(x.Score.Remaining);
			}
		}

		// Token: 0x020000A0 RID: 160
		public class ItemKillComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC9 RID: 3017 RVA: 0x00028C88 File Offset: 0x00026E88
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Kill.CompareTo(x.Score.Kill) * -1;
				}
				return y.Score.Kill.CompareTo(x.Score.Kill);
			}
		}

		// Token: 0x020000A1 RID: 161
		public class ItemUpgradeComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BCB RID: 3019 RVA: 0x00028CE4 File Offset: 0x00026EE4
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.ReadyToUpgrade.CompareTo(x.Score.ReadyToUpgrade) * -1;
				}
				return y.Score.ReadyToUpgrade.CompareTo(x.Score.ReadyToUpgrade);
			}
		}

		// Token: 0x020000A2 RID: 162
		public class ItemDeadComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BCD RID: 3021 RVA: 0x00028D40 File Offset: 0x00026F40
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Dead.CompareTo(x.Score.Dead) * -1;
				}
				return y.Score.Dead.CompareTo(x.Score.Dead);
			}
		}

		// Token: 0x020000A3 RID: 163
		public class ItemWoundedComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BCF RID: 3023 RVA: 0x00028D9C File Offset: 0x00026F9C
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Wounded.CompareTo(x.Score.Wounded) * -1;
				}
				return y.Score.Wounded.CompareTo(x.Score.Wounded);
			}
		}

		// Token: 0x020000A4 RID: 164
		public class ItemRoutedComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BD1 RID: 3025 RVA: 0x00028DF8 File Offset: 0x00026FF8
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Routed.CompareTo(x.Score.Routed) * -1;
				}
				return y.Score.Routed.CompareTo(x.Score.Routed);
			}
		}

		// Token: 0x020000A5 RID: 165
		public class ItemMemberComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BD3 RID: 3027 RVA: 0x00028E54 File Offset: 0x00027054
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (x.Character.IsPlayerCharacter && !y.Character.IsPlayerCharacter)
				{
					return -1;
				}
				if (!x.Character.IsPlayerCharacter && y.Character.IsPlayerCharacter)
				{
					return 1;
				}
				if (x.IsHero && !y.IsHero)
				{
					return -1;
				}
				if (!x.IsHero && y.IsHero)
				{
					return 1;
				}
				return x.Character.Name.ToString().CompareTo(y.Character.Name.ToString());
			}
		}
	}
}
