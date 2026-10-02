using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013D RID: 317
	public class ClanFiefsSortControllerVM : ViewModel
	{
		// Token: 0x06001E25 RID: 7717 RVA: 0x0006CF09 File Offset: 0x0006B109
		public ClanFiefsSortControllerVM(List<MBBindingList<ClanSettlementItemVM>> listsToControl)
		{
			this._listsToControl = listsToControl;
			this._nameComparer = new ClanFiefsSortControllerVM.ItemNameComparer();
			this._governorComparer = new ClanFiefsSortControllerVM.ItemGovernorComparer();
			this._profitComparer = new ClanFiefsSortControllerVM.ItemProfitComparer();
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0006CF3C File Offset: 0x0006B13C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.GovernorText = GameTexts.FindText("str_notable_governor", null).ToString();
			this.ProfitText = GameTexts.FindText("str_profit", null).ToString();
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0006CF94 File Offset: 0x0006B194
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
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._nameComparer);
			}
			this.IsNameSelected = true;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0006D038 File Offset: 0x0006B238
		public void ExecuteSortByGovernor()
		{
			int governorState = this.GovernorState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.GovernorState = (governorState + 1) % 3;
			if (this.GovernorState == 0)
			{
				int governorState2 = this.GovernorState;
				this.GovernorState = governorState2 + 1;
			}
			this._governorComparer.SetSortMode(this.GovernorState == 1);
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._governorComparer);
			}
			this.IsGovernorSelected = true;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0006D0DC File Offset: 0x0006B2DC
		public void ExecuteSortByProfit()
		{
			int profitState = this.ProfitState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ProfitState = (profitState + 1) % 3;
			if (this.ProfitState == 0)
			{
				int profitState2 = this.ProfitState;
				this.ProfitState = profitState2 + 1;
			}
			this._profitComparer.SetSortMode(this.ProfitState == 1);
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._profitComparer);
			}
			this.IsProfitSelected = true;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0006D180 File Offset: 0x0006B380
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.GovernorState = (int)state;
			this.ProfitState = (int)state;
			this.IsNameSelected = false;
			this.IsGovernorSelected = false;
			this.IsProfitSelected = false;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0006D1AC File Offset: 0x0006B3AC
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x0006D1B5 File Offset: 0x0006B3B5
		// (set) Token: 0x06001E2D RID: 7725 RVA: 0x0006D1BD File Offset: 0x0006B3BD
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

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x0006D1DB File Offset: 0x0006B3DB
		// (set) Token: 0x06001E2F RID: 7727 RVA: 0x0006D1E3 File Offset: 0x0006B3E3
		[DataSourceProperty]
		public int GovernorState
		{
			get
			{
				return this._governorState;
			}
			set
			{
				if (value != this._governorState)
				{
					this._governorState = value;
					base.OnPropertyChangedWithValue(value, "GovernorState");
				}
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x0006D201 File Offset: 0x0006B401
		// (set) Token: 0x06001E31 RID: 7729 RVA: 0x0006D209 File Offset: 0x0006B409
		[DataSourceProperty]
		public int ProfitState
		{
			get
			{
				return this._profitState;
			}
			set
			{
				if (value != this._profitState)
				{
					this._profitState = value;
					base.OnPropertyChangedWithValue(value, "ProfitState");
				}
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0006D227 File Offset: 0x0006B427
		// (set) Token: 0x06001E33 RID: 7731 RVA: 0x0006D22F File Offset: 0x0006B42F
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

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0006D24D File Offset: 0x0006B44D
		// (set) Token: 0x06001E35 RID: 7733 RVA: 0x0006D255 File Offset: 0x0006B455
		[DataSourceProperty]
		public bool IsGovernorSelected
		{
			get
			{
				return this._isGovernorSelected;
			}
			set
			{
				if (value != this._isGovernorSelected)
				{
					this._isGovernorSelected = value;
					base.OnPropertyChangedWithValue(value, "IsGovernorSelected");
				}
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0006D273 File Offset: 0x0006B473
		// (set) Token: 0x06001E37 RID: 7735 RVA: 0x0006D27B File Offset: 0x0006B47B
		[DataSourceProperty]
		public bool IsProfitSelected
		{
			get
			{
				return this._isProfitSelected;
			}
			set
			{
				if (value != this._isProfitSelected)
				{
					this._isProfitSelected = value;
					base.OnPropertyChangedWithValue(value, "IsProfitSelected");
				}
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006D299 File Offset: 0x0006B499
		// (set) Token: 0x06001E39 RID: 7737 RVA: 0x0006D2A1 File Offset: 0x0006B4A1
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

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0006D2C4 File Offset: 0x0006B4C4
		// (set) Token: 0x06001E3B RID: 7739 RVA: 0x0006D2CC File Offset: 0x0006B4CC
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0006D2EF File Offset: 0x0006B4EF
		// (set) Token: 0x06001E3D RID: 7741 RVA: 0x0006D2F7 File Offset: 0x0006B4F7
		[DataSourceProperty]
		public string ProfitText
		{
			get
			{
				return this._profitText;
			}
			set
			{
				if (value != this._profitText)
				{
					this._profitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfitText");
				}
			}
		}

		// Token: 0x04000DCA RID: 3530
		private readonly List<MBBindingList<ClanSettlementItemVM>> _listsToControl;

		// Token: 0x04000DCB RID: 3531
		private readonly ClanFiefsSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000DCC RID: 3532
		private readonly ClanFiefsSortControllerVM.ItemGovernorComparer _governorComparer;

		// Token: 0x04000DCD RID: 3533
		private readonly ClanFiefsSortControllerVM.ItemProfitComparer _profitComparer;

		// Token: 0x04000DCE RID: 3534
		private int _nameState;

		// Token: 0x04000DCF RID: 3535
		private int _governorState;

		// Token: 0x04000DD0 RID: 3536
		private int _profitState;

		// Token: 0x04000DD1 RID: 3537
		private bool _isNameSelected;

		// Token: 0x04000DD2 RID: 3538
		private bool _isGovernorSelected;

		// Token: 0x04000DD3 RID: 3539
		private bool _isProfitSelected;

		// Token: 0x04000DD4 RID: 3540
		private string _nameText;

		// Token: 0x04000DD5 RID: 3541
		private string _governorText;

		// Token: 0x04000DD6 RID: 3542
		private string _profitText;

		// Token: 0x020002A3 RID: 675
		public abstract class ItemComparerBase : IComparer<ClanSettlementItemVM>
		{
			// Token: 0x0600274B RID: 10059 RVA: 0x0008480F File Offset: 0x00082A0F
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x0600274C RID: 10060
			public abstract int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y);

			// Token: 0x04001372 RID: 4978
			protected bool _isAcending;
		}

		// Token: 0x020002A4 RID: 676
		public class ItemNameComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600274E RID: 10062 RVA: 0x00084820 File Offset: 0x00082A20
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002A5 RID: 677
		public class ItemGovernorComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002750 RID: 10064 RVA: 0x00084858 File Offset: 0x00082A58
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					if (y.HasGovernor && x.HasGovernor)
					{
						return y.Governor.NameText.CompareTo(x.Governor.NameText) * -1;
					}
					if (y.HasGovernor)
					{
						return 1;
					}
					if (x.HasGovernor)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (y.HasGovernor && x.HasGovernor)
					{
						return y.Governor.NameText.CompareTo(x.Governor.NameText);
					}
					if (y.HasGovernor)
					{
						return 1;
					}
					if (x.HasGovernor)
					{
						return -1;
					}
					return 0;
				}
			}
		}

		// Token: 0x020002A6 RID: 678
		public class ItemProfitComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002752 RID: 10066 RVA: 0x000848FC File Offset: 0x00082AFC
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					return y.TotalProfit.Value.CompareTo(x.TotalProfit.Value) * -1;
				}
				return y.TotalProfit.Value.CompareTo(x.TotalProfit.Value);
			}
		}
	}
}
