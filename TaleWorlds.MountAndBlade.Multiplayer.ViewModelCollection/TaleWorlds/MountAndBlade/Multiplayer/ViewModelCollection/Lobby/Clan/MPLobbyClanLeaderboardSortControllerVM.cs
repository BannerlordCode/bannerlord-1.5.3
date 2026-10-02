using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006E RID: 110
	public class MPLobbyClanLeaderboardSortControllerVM : ViewModel
	{
		// Token: 0x06000ABC RID: 2748 RVA: 0x00020FCF File Offset: 0x0001F1CF
		public MPLobbyClanLeaderboardSortControllerVM(ref ClanLeaderboardEntry[] listToControl, Action onSorted)
		{
			this._listToControl = listToControl;
			this._winComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemWinComparer();
			this._lossComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemLossComparer();
			this._nameComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemNameComparer();
			this._onSorted = onSorted;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00021008 File Offset: 0x0001F208
		private void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._nameComparer);
			this.IsNameSelected = true;
			this._onSorted();
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002107C File Offset: 0x0001F27C
		private void ExecuteSortByWin()
		{
			int winState = this.WinState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.WinState = (winState + 1) % 3;
			if (this.WinState == 0)
			{
				this.WinState++;
			}
			this._winComparer.SetSortMode(this.WinState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._winComparer);
			this.IsWinSelected = true;
			this._onSorted();
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000210F0 File Offset: 0x0001F2F0
		private void ExecuteSortByLoss()
		{
			int lossState = this.LossState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.LossState = (lossState + 1) % 3;
			if (this.LossState == 0)
			{
				this.LossState++;
			}
			this._lossComparer.SetSortMode(this.LossState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._lossComparer);
			this.IsLossSelected = true;
			this._onSorted();
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00021163 File Offset: 0x0001F363
		private void SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState state)
		{
			this.NameState = (int)state;
			this.WinState = (int)state;
			this.LossState = (int)state;
			this.IsNameSelected = false;
			this.IsWinSelected = false;
			this.IsLossSelected = false;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0002118F File Offset: 0x0001F38F
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00021197 File Offset: 0x0001F397
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

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x000211B5 File Offset: 0x0001F3B5
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x000211BD File Offset: 0x0001F3BD
		[DataSourceProperty]
		public int WinState
		{
			get
			{
				return this._winState;
			}
			set
			{
				if (value != this._winState)
				{
					this._winState = value;
					base.OnPropertyChangedWithValue(value, "WinState");
				}
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000211DB File Offset: 0x0001F3DB
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x000211E3 File Offset: 0x0001F3E3
		[DataSourceProperty]
		public int LossState
		{
			get
			{
				return this._lossState;
			}
			set
			{
				if (value != this._lossState)
				{
					this._lossState = value;
					base.OnPropertyChangedWithValue(value, "LossState");
				}
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00021201 File Offset: 0x0001F401
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00021209 File Offset: 0x0001F409
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

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00021227 File Offset: 0x0001F427
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x0002122F File Offset: 0x0001F42F
		[DataSourceProperty]
		public bool IsWinSelected
		{
			get
			{
				return this._isWinSelected;
			}
			set
			{
				if (value != this._isWinSelected)
				{
					this._isWinSelected = value;
					base.OnPropertyChangedWithValue(value, "IsWinSelected");
				}
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0002124D File Offset: 0x0001F44D
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00021255 File Offset: 0x0001F455
		[DataSourceProperty]
		public bool IsLossSelected
		{
			get
			{
				return this._isLossSelected;
			}
			set
			{
				if (value != this._isLossSelected)
				{
					this._isLossSelected = value;
					base.OnPropertyChangedWithValue(value, "IsLossSelected");
				}
			}
		}

		// Token: 0x040004E2 RID: 1250
		private readonly ClanLeaderboardEntry[] _listToControl;

		// Token: 0x040004E3 RID: 1251
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040004E4 RID: 1252
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemWinComparer _winComparer;

		// Token: 0x040004E5 RID: 1253
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemLossComparer _lossComparer;

		// Token: 0x040004E6 RID: 1254
		private Action _onSorted;

		// Token: 0x040004E7 RID: 1255
		private int _nameState;

		// Token: 0x040004E8 RID: 1256
		private int _winState;

		// Token: 0x040004E9 RID: 1257
		private int _lossState;

		// Token: 0x040004EA RID: 1258
		private bool _isNameSelected;

		// Token: 0x040004EB RID: 1259
		private bool _isWinSelected;

		// Token: 0x040004EC RID: 1260
		private bool _isLossSelected;

		// Token: 0x02000155 RID: 341
		private enum SortState
		{
			// Token: 0x04000A1C RID: 2588
			Default,
			// Token: 0x04000A1D RID: 2589
			Ascending,
			// Token: 0x04000A1E RID: 2590
			Descending
		}

		// Token: 0x02000156 RID: 342
		private abstract class ItemComparerBase : IComparer<ClanLeaderboardEntry>
		{
			// Token: 0x060012FF RID: 4863 RVA: 0x0003CABE File Offset: 0x0003ACBE
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06001300 RID: 4864
			public abstract int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y);

			// Token: 0x04000A1F RID: 2591
			protected bool _isAcending;
		}

		// Token: 0x02000157 RID: 343
		private class ItemNameComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001302 RID: 4866 RVA: 0x0003CACF File Offset: 0x0003ACCF
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x02000158 RID: 344
		private class ItemWinComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001304 RID: 4868 RVA: 0x0003CB08 File Offset: 0x0003AD08
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.WinCount.CompareTo(x.WinCount) * -1;
				}
				return y.WinCount.CompareTo(x.WinCount);
			}
		}

		// Token: 0x02000159 RID: 345
		private class ItemLossComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001306 RID: 4870 RVA: 0x0003CB50 File Offset: 0x0003AD50
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.LossCount.CompareTo(x.LossCount) * -1;
				}
				return y.LossCount.CompareTo(x.LossCount);
			}
		}
	}
}
