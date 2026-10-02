using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013F RID: 319
	public class ClanIncomeSortControllerVM : ViewModel
	{
		// Token: 0x06001E73 RID: 7795 RVA: 0x0006DF78 File Offset: 0x0006C178
		public ClanIncomeSortControllerVM(MBBindingList<ClanFinanceWorkshopItemVM> workshopList, MBBindingList<ClanSupporterGroupVM> supporterList, MBBindingList<ClanFinanceAlleyItemVM> alleyList)
		{
			this._workshopList = workshopList;
			this._supporterList = supporterList;
			this._alleyList = alleyList;
			this._workshopNameComparer = new ClanIncomeSortControllerVM.WorkshopItemNameComparer();
			this._supporterNameComparer = new ClanIncomeSortControllerVM.SupporterItemNameComparer();
			this._alleyNameComparer = new ClanIncomeSortControllerVM.AlleyItemNameComparer();
			this._workshopLocationComparer = new ClanIncomeSortControllerVM.WorkshopItemLocationComparer();
			this._alleyLocationComparer = new ClanIncomeSortControllerVM.AlleyItemLocationComparer();
			this._workshopIncomeComparer = new ClanIncomeSortControllerVM.WorkshopItemIncomeComparer();
			this._supporterIncomeComparer = new ClanIncomeSortControllerVM.SupporterItemIncomeComparer();
			this._alleyIncomeComparer = new ClanIncomeSortControllerVM.AlleyItemIncomeComparer();
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0006DFF8 File Offset: 0x0006C1F8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.IncomeText = GameTexts.FindText("str_income", null).ToString();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0006E050 File Offset: 0x0006C250
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
			this._workshopNameComparer.SetSortMode(this.NameState == 1);
			this._supporterNameComparer.SetSortMode(this.NameState == 1);
			this._alleyNameComparer.SetSortMode(this.NameState == 1);
			this._workshopList.Sort(this._workshopNameComparer);
			this._supporterList.Sort(this._supporterNameComparer);
			this._alleyList.Sort(this._alleyNameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0006E104 File Offset: 0x0006C304
		public void ExecuteSortByLocation()
		{
			int locationState = this.LocationState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.LocationState = (locationState + 1) % 3;
			if (this.LocationState == 0)
			{
				int locationState2 = this.LocationState;
				this.LocationState = locationState2 + 1;
			}
			this._workshopLocationComparer.SetSortMode(this.LocationState == 1);
			this._alleyLocationComparer.SetSortMode(this.LocationState == 1);
			this._workshopList.Sort(this._workshopLocationComparer);
			this._alleyList.Sort(this._alleyLocationComparer);
			this.IsLocationSelected = true;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0006E194 File Offset: 0x0006C394
		public void ExecuteSortByIncome()
		{
			int incomeState = this.IncomeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.IncomeState = (incomeState + 1) % 3;
			if (this.IncomeState == 0)
			{
				int incomeState2 = this.IncomeState;
				this.IncomeState = incomeState2 + 1;
			}
			this._workshopIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._supporterIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._alleyIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._workshopList.Sort(this._workshopIncomeComparer);
			this._supporterList.Sort(this._supporterIncomeComparer);
			this._alleyList.Sort(this._alleyIncomeComparer);
			this.IsIncomeSelected = true;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0006E248 File Offset: 0x0006C448
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.LocationState = (int)state;
			this.IncomeState = (int)state;
			this.IsNameSelected = false;
			this.IsLocationSelected = false;
			this.IsIncomeSelected = false;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0006E274 File Offset: 0x0006C474
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x0006E27D File Offset: 0x0006C47D
		// (set) Token: 0x06001E7B RID: 7803 RVA: 0x0006E285 File Offset: 0x0006C485
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

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0006E2A3 File Offset: 0x0006C4A3
		// (set) Token: 0x06001E7D RID: 7805 RVA: 0x0006E2AB File Offset: 0x0006C4AB
		[DataSourceProperty]
		public int LocationState
		{
			get
			{
				return this._locationState;
			}
			set
			{
				if (value != this._locationState)
				{
					this._locationState = value;
					base.OnPropertyChangedWithValue(value, "LocationState");
				}
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0006E2C9 File Offset: 0x0006C4C9
		// (set) Token: 0x06001E7F RID: 7807 RVA: 0x0006E2D1 File Offset: 0x0006C4D1
		[DataSourceProperty]
		public int IncomeState
		{
			get
			{
				return this._incomeState;
			}
			set
			{
				if (value != this._incomeState)
				{
					this._incomeState = value;
					base.OnPropertyChangedWithValue(value, "IncomeState");
				}
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0006E2EF File Offset: 0x0006C4EF
		// (set) Token: 0x06001E81 RID: 7809 RVA: 0x0006E2F7 File Offset: 0x0006C4F7
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

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x0006E315 File Offset: 0x0006C515
		// (set) Token: 0x06001E83 RID: 7811 RVA: 0x0006E31D File Offset: 0x0006C51D
		[DataSourceProperty]
		public bool IsLocationSelected
		{
			get
			{
				return this._isLocationSelected;
			}
			set
			{
				if (value != this._isLocationSelected)
				{
					this._isLocationSelected = value;
					base.OnPropertyChangedWithValue(value, "IsLocationSelected");
				}
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0006E33B File Offset: 0x0006C53B
		// (set) Token: 0x06001E85 RID: 7813 RVA: 0x0006E343 File Offset: 0x0006C543
		[DataSourceProperty]
		public bool IsIncomeSelected
		{
			get
			{
				return this._isIncomeSelected;
			}
			set
			{
				if (value != this._isIncomeSelected)
				{
					this._isIncomeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsIncomeSelected");
				}
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0006E361 File Offset: 0x0006C561
		// (set) Token: 0x06001E87 RID: 7815 RVA: 0x0006E369 File Offset: 0x0006C569
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

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x0006E38C File Offset: 0x0006C58C
		// (set) Token: 0x06001E89 RID: 7817 RVA: 0x0006E394 File Offset: 0x0006C594
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0006E3B7 File Offset: 0x0006C5B7
		// (set) Token: 0x06001E8B RID: 7819 RVA: 0x0006E3BF File Offset: 0x0006C5BF
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeText");
				}
			}
		}

		// Token: 0x04000DED RID: 3565
		private readonly MBBindingList<ClanFinanceWorkshopItemVM> _workshopList;

		// Token: 0x04000DEE RID: 3566
		private readonly MBBindingList<ClanSupporterGroupVM> _supporterList;

		// Token: 0x04000DEF RID: 3567
		private readonly MBBindingList<ClanFinanceAlleyItemVM> _alleyList;

		// Token: 0x04000DF0 RID: 3568
		private readonly ClanIncomeSortControllerVM.WorkshopItemNameComparer _workshopNameComparer;

		// Token: 0x04000DF1 RID: 3569
		private readonly ClanIncomeSortControllerVM.SupporterItemNameComparer _supporterNameComparer;

		// Token: 0x04000DF2 RID: 3570
		private readonly ClanIncomeSortControllerVM.AlleyItemNameComparer _alleyNameComparer;

		// Token: 0x04000DF3 RID: 3571
		private readonly ClanIncomeSortControllerVM.WorkshopItemLocationComparer _workshopLocationComparer;

		// Token: 0x04000DF4 RID: 3572
		private readonly ClanIncomeSortControllerVM.AlleyItemLocationComparer _alleyLocationComparer;

		// Token: 0x04000DF5 RID: 3573
		private readonly ClanIncomeSortControllerVM.WorkshopItemIncomeComparer _workshopIncomeComparer;

		// Token: 0x04000DF6 RID: 3574
		private readonly ClanIncomeSortControllerVM.SupporterItemIncomeComparer _supporterIncomeComparer;

		// Token: 0x04000DF7 RID: 3575
		private readonly ClanIncomeSortControllerVM.AlleyItemIncomeComparer _alleyIncomeComparer;

		// Token: 0x04000DF8 RID: 3576
		private int _nameState;

		// Token: 0x04000DF9 RID: 3577
		private int _locationState;

		// Token: 0x04000DFA RID: 3578
		private int _incomeState;

		// Token: 0x04000DFB RID: 3579
		private bool _isNameSelected;

		// Token: 0x04000DFC RID: 3580
		private bool _isLocationSelected;

		// Token: 0x04000DFD RID: 3581
		private bool _isIncomeSelected;

		// Token: 0x04000DFE RID: 3582
		private string _nameText;

		// Token: 0x04000DFF RID: 3583
		private string _locationText;

		// Token: 0x04000E00 RID: 3584
		private string _incomeText;

		// Token: 0x020002AF RID: 687
		public abstract class WorkshopItemComparerBase : IComparer<ClanFinanceWorkshopItemVM>
		{
			// Token: 0x06002781 RID: 10113 RVA: 0x00085374 File Offset: 0x00083574
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06002782 RID: 10114
			public abstract int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y);

			// Token: 0x04001396 RID: 5014
			protected bool _isAcending;
		}

		// Token: 0x020002B0 RID: 688
		public abstract class SupporterItemComparerBase : IComparer<ClanSupporterGroupVM>
		{
			// Token: 0x06002784 RID: 10116 RVA: 0x00085385 File Offset: 0x00083585
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06002785 RID: 10117
			public abstract int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y);

			// Token: 0x04001397 RID: 5015
			protected bool _isAcending;
		}

		// Token: 0x020002B1 RID: 689
		public abstract class AlleyItemComparerBase : IComparer<ClanFinanceAlleyItemVM>
		{
			// Token: 0x06002787 RID: 10119 RVA: 0x00085396 File Offset: 0x00083596
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06002788 RID: 10120
			public abstract int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y);

			// Token: 0x04001398 RID: 5016
			protected bool _isAcending;
		}

		// Token: 0x020002B2 RID: 690
		public class WorkshopItemNameComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x0600278A RID: 10122 RVA: 0x000853A7 File Offset: 0x000835A7
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002B3 RID: 691
		public class SupporterItemNameComparer : ClanIncomeSortControllerVM.SupporterItemComparerBase
		{
			// Token: 0x0600278C RID: 10124 RVA: 0x000853DE File Offset: 0x000835DE
			public override int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002B4 RID: 692
		public class AlleyItemNameComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x0600278E RID: 10126 RVA: 0x00085415 File Offset: 0x00083615
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002B5 RID: 693
		public class WorkshopItemLocationComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x06002790 RID: 10128 RVA: 0x0008544C File Offset: 0x0008364C
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				int num = this.GetDistanceToMainParty(y).CompareTo(this.GetDistanceToMainParty(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x06002791 RID: 10129 RVA: 0x00085480 File Offset: 0x00083680
			private float GetDistanceToMainParty(ClanFinanceWorkshopItemVM item)
			{
				return item.Workshop.Settlement.Position.Distance(Hero.MainHero.GetCampaignPosition());
			}
		}

		// Token: 0x020002B6 RID: 694
		public class AlleyItemLocationComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x06002793 RID: 10131 RVA: 0x000854B8 File Offset: 0x000836B8
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				int num = this.GetDistanceToMainParty(y).CompareTo(this.GetDistanceToMainParty(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x06002794 RID: 10132 RVA: 0x000854EC File Offset: 0x000836EC
			private float GetDistanceToMainParty(ClanFinanceAlleyItemVM item)
			{
				return item.Alley.Settlement.Position.Distance(Hero.MainHero.GetCampaignPosition());
			}
		}

		// Token: 0x020002B7 RID: 695
		public class WorkshopItemIncomeComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x06002796 RID: 10134 RVA: 0x00085524 File Offset: 0x00083724
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				if (this._isAcending)
				{
					return y.Workshop.ProfitMade.CompareTo(x.Workshop.ProfitMade) * -1;
				}
				return y.Workshop.ProfitMade.CompareTo(x.Workshop.ProfitMade);
			}
		}

		// Token: 0x020002B8 RID: 696
		public class SupporterItemIncomeComparer : ClanIncomeSortControllerVM.SupporterItemComparerBase
		{
			// Token: 0x06002798 RID: 10136 RVA: 0x00085580 File Offset: 0x00083780
			public override int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y)
			{
				if (this._isAcending)
				{
					return y.TotalInfluenceBonus.CompareTo(x.TotalInfluenceBonus) * -1;
				}
				return y.TotalInfluenceBonus.CompareTo(x.TotalInfluenceBonus);
			}
		}

		// Token: 0x020002B9 RID: 697
		public class AlleyItemIncomeComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x0600279A RID: 10138 RVA: 0x000855C8 File Offset: 0x000837C8
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				if (this._isAcending)
				{
					return y.Income.CompareTo(x.Income) * -1;
				}
				return y.Income.CompareTo(x.Income);
			}
		}
	}
}
