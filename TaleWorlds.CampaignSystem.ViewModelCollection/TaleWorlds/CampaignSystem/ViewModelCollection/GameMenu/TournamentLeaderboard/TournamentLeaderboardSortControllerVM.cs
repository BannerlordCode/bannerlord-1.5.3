using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000B4 RID: 180
	public class TournamentLeaderboardSortControllerVM : ViewModel
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x00044E08 File Offset: 0x00043008
		public TournamentLeaderboardSortControllerVM(ref MBBindingList<TournamentLeaderboardEntryItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._prizeComparer = new TournamentLeaderboardSortControllerVM.ItemPrizeComparer();
			this._nameComparer = new TournamentLeaderboardSortControllerVM.ItemNameComparer();
			this._placementComparer = new TournamentLeaderboardSortControllerVM.ItemPlacementComparer();
			this._victoriesComparer = new TournamentLeaderboardSortControllerVM.ItemVictoriesComparer();
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00044E44 File Offset: 0x00043044
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
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

		// Token: 0x06001105 RID: 4357 RVA: 0x00044EB0 File Offset: 0x000430B0
		public void ExecuteSortByPrize()
		{
			int prizeState = this.PrizeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PrizeState = (prizeState + 1) % 3;
			if (this.PrizeState == 0)
			{
				int prizeState2 = this.PrizeState;
				this.PrizeState = prizeState2 + 1;
			}
			this._prizeComparer.SetSortMode(this.PrizeState == 1);
			this._listToControl.Sort(this._prizeComparer);
			this.IsPrizeSelected = true;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00044F1C File Offset: 0x0004311C
		public void ExecuteSortByPlacement()
		{
			int placementState = this.PlacementState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PlacementState = (placementState + 1) % 3;
			if (this.PlacementState == 0)
			{
				int placementState2 = this.PlacementState;
				this.PlacementState = placementState2 + 1;
			}
			this._placementComparer.SetSortMode(this.PlacementState == 1);
			this._listToControl.Sort(this._placementComparer);
			this.IsPlacementSelected = true;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00044F88 File Offset: 0x00043188
		public void ExecuteSortByVictories()
		{
			int victoriesState = this.VictoriesState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.VictoriesState = (victoriesState + 1) % 3;
			if (this.VictoriesState == 0)
			{
				int victoriesState2 = this.VictoriesState;
				this.VictoriesState = victoriesState2 + 1;
			}
			this._victoriesComparer.SetSortMode(this.VictoriesState == 1);
			this._listToControl.Sort(this._victoriesComparer);
			this.IsVictoriesSelected = true;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00044FF2 File Offset: 0x000431F2
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.PrizeState = (int)state;
			this.PlacementState = (int)state;
			this.VictoriesState = (int)state;
			this.IsNameSelected = false;
			this.IsVictoriesSelected = false;
			this.IsPrizeSelected = false;
			this.IsPlacementSelected = false;
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0004502C File Offset: 0x0004322C
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x00045034 File Offset: 0x00043234
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

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x00045052 File Offset: 0x00043252
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x0004505A File Offset: 0x0004325A
		[DataSourceProperty]
		public int VictoriesState
		{
			get
			{
				return this._victoriesState;
			}
			set
			{
				if (value != this._victoriesState)
				{
					this._victoriesState = value;
					base.OnPropertyChangedWithValue(value, "VictoriesState");
				}
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x00045078 File Offset: 0x00043278
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x00045080 File Offset: 0x00043280
		[DataSourceProperty]
		public int PrizeState
		{
			get
			{
				return this._prizeState;
			}
			set
			{
				if (value != this._prizeState)
				{
					this._prizeState = value;
					base.OnPropertyChangedWithValue(value, "PrizeState");
				}
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0004509E File Offset: 0x0004329E
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x000450A6 File Offset: 0x000432A6
		[DataSourceProperty]
		public int PlacementState
		{
			get
			{
				return this._placementState;
			}
			set
			{
				if (value != this._placementState)
				{
					this._placementState = value;
					base.OnPropertyChangedWithValue(value, "PlacementState");
				}
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x000450C4 File Offset: 0x000432C4
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x000450CC File Offset: 0x000432CC
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

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x000450EA File Offset: 0x000432EA
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x000450F2 File Offset: 0x000432F2
		[DataSourceProperty]
		public bool IsPrizeSelected
		{
			get
			{
				return this._isPrizeSelected;
			}
			set
			{
				if (value != this._isPrizeSelected)
				{
					this._isPrizeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPrizeSelected");
				}
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00045110 File Offset: 0x00043310
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x00045118 File Offset: 0x00043318
		[DataSourceProperty]
		public bool IsPlacementSelected
		{
			get
			{
				return this._isPlacementSelected;
			}
			set
			{
				if (value != this._isPlacementSelected)
				{
					this._isPlacementSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPlacementSelected");
				}
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x00045136 File Offset: 0x00043336
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x0004513E File Offset: 0x0004333E
		[DataSourceProperty]
		public bool IsVictoriesSelected
		{
			get
			{
				return this._isVictoriesSelected;
			}
			set
			{
				if (value != this._isVictoriesSelected)
				{
					this._isVictoriesSelected = value;
					base.OnPropertyChangedWithValue(value, "IsVictoriesSelected");
				}
			}
		}

		// Token: 0x040007B8 RID: 1976
		private readonly MBBindingList<TournamentLeaderboardEntryItemVM> _listToControl;

		// Token: 0x040007B9 RID: 1977
		private readonly TournamentLeaderboardSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040007BA RID: 1978
		private readonly TournamentLeaderboardSortControllerVM.ItemPrizeComparer _prizeComparer;

		// Token: 0x040007BB RID: 1979
		private readonly TournamentLeaderboardSortControllerVM.ItemPlacementComparer _placementComparer;

		// Token: 0x040007BC RID: 1980
		private readonly TournamentLeaderboardSortControllerVM.ItemVictoriesComparer _victoriesComparer;

		// Token: 0x040007BD RID: 1981
		private int _nameState;

		// Token: 0x040007BE RID: 1982
		private int _prizeState;

		// Token: 0x040007BF RID: 1983
		private int _placementState;

		// Token: 0x040007C0 RID: 1984
		private int _victoriesState;

		// Token: 0x040007C1 RID: 1985
		private bool _isNameSelected;

		// Token: 0x040007C2 RID: 1986
		private bool _isPrizeSelected;

		// Token: 0x040007C3 RID: 1987
		private bool _isPlacementSelected;

		// Token: 0x040007C4 RID: 1988
		private bool _isVictoriesSelected;

		// Token: 0x02000226 RID: 550
		public abstract class ItemComparerBase : IComparer<TournamentLeaderboardEntryItemVM>
		{
			// Token: 0x060025B5 RID: 9653 RVA: 0x0008235B File Offset: 0x0008055B
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x060025B6 RID: 9654
			public abstract int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y);

			// Token: 0x0400123A RID: 4666
			protected bool _isAcending;
		}

		// Token: 0x02000227 RID: 551
		public class ItemNameComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025B8 RID: 9656 RVA: 0x0008236C File Offset: 0x0008056C
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x02000228 RID: 552
		public class ItemPrizeComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025BA RID: 9658 RVA: 0x000823A4 File Offset: 0x000805A4
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.PrizeValue.CompareTo(x.PrizeValue) * -1;
				}
				return y.PrizeValue.CompareTo(x.PrizeValue);
			}
		}

		// Token: 0x02000229 RID: 553
		public class ItemPlacementComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025BC RID: 9660 RVA: 0x000823EC File Offset: 0x000805EC
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.PlacementOnLeaderboard.CompareTo(x.PlacementOnLeaderboard) * -1;
				}
				return y.PlacementOnLeaderboard.CompareTo(x.PlacementOnLeaderboard);
			}
		}

		// Token: 0x0200022A RID: 554
		public class ItemVictoriesComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025BE RID: 9662 RVA: 0x00082434 File Offset: 0x00080634
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.Victories.CompareTo(x.Victories) * -1;
				}
				return y.Victories.CompareTo(x.Victories);
			}
		}
	}
}
