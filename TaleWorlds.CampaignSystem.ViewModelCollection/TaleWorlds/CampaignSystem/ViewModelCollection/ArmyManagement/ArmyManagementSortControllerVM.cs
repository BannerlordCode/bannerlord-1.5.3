using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x02000163 RID: 355
	public class ArmyManagementSortControllerVM : ViewModel
	{
		// Token: 0x06002279 RID: 8825 RVA: 0x0007AB68 File Offset: 0x00078D68
		public ArmyManagementSortControllerVM(MBBindingList<ArmyManagementItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._distanceComparer = new ArmyManagementSortControllerVM.ItemDistanceComparer();
			this._costComparer = new ArmyManagementSortControllerVM.ItemCostComparer();
			this._strengthComparer = new ArmyManagementSortControllerVM.ItemStrengthComparer();
			this._nameComparer = new ArmyManagementSortControllerVM.ItemNameComparer();
			this._clanComparer = new ArmyManagementSortControllerVM.ItemClanComparer();
			this._shipCountComparer = new ArmyManagementSortControllerVM.ItemShipCountComparer();
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0007ABC4 File Offset: 0x00078DC4
		public void ExecuteSortByDistance()
		{
			int distanceState = this.DistanceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DistanceState = (distanceState + 1) % 3;
			if (this.DistanceState == 0)
			{
				int distanceState2 = this.DistanceState;
				this.DistanceState = distanceState2 + 1;
			}
			this._distanceComparer.SetSortMode(this.DistanceState == 1);
			this._listToControl.Sort(this._distanceComparer);
			this.IsDistanceSelected = true;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x0007AC30 File Offset: 0x00078E30
		public void ExecuteSortByCost()
		{
			int costState = this.CostState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.CostState = (costState + 1) % 3;
			if (this.CostState == 0)
			{
				int costState2 = this.CostState;
				this.CostState = costState2 + 1;
			}
			this._costComparer.SetSortMode(this.CostState == 1);
			this._listToControl.Sort(this._costComparer);
			this.IsCostSelected = true;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x0007AC9C File Offset: 0x00078E9C
		public void ExecuteSortByStrength()
		{
			int strengthState = this.StrengthState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.StrengthState = (strengthState + 1) % 3;
			if (this.StrengthState == 0)
			{
				int strengthState2 = this.StrengthState;
				this.StrengthState = strengthState2 + 1;
			}
			this._strengthComparer.SetSortMode(this.StrengthState == 1);
			this._listToControl.Sort(this._strengthComparer);
			this.IsStrengthSelected = true;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0007AD08 File Offset: 0x00078F08
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

		// Token: 0x0600227E RID: 8830 RVA: 0x0007AD74 File Offset: 0x00078F74
		public void ExecuteSortByClan()
		{
			int clanState = this.ClanState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ClanState = (clanState + 1) % 3;
			if (this.ClanState == 0)
			{
				int clanState2 = this.ClanState;
				this.ClanState = clanState2 + 1;
			}
			this._clanComparer.SetSortMode(this.ClanState == 1);
			this._listToControl.Sort(this._clanComparer);
			this.IsClanSelected = true;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0007ADE0 File Offset: 0x00078FE0
		public void ExecuteSortByShipCount()
		{
			int shipCountState = this.ShipCountState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ShipCountState = (shipCountState + 1) % 3;
			if (this.ShipCountState == 0)
			{
				int shipCountState2 = this.ShipCountState;
				this.ShipCountState = shipCountState2 + 1;
			}
			this._shipCountComparer.SetSortMode(this.ShipCountState == 1);
			this._listToControl.Sort(this._shipCountComparer);
			this.IsShipCountSelected = true;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0007AE4C File Offset: 0x0007904C
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.DistanceState = (int)state;
			this.CostState = (int)state;
			this.StrengthState = (int)state;
			this.NameState = (int)state;
			this.ClanState = (int)state;
			this.ShipCountState = (int)state;
			this.IsDistanceSelected = false;
			this.IsCostSelected = false;
			this.IsNameSelected = false;
			this.IsClanSelected = false;
			this.IsStrengthSelected = false;
			this.IsShipCountSelected = false;
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x0007AEAD File Offset: 0x000790AD
		// (set) Token: 0x06002282 RID: 8834 RVA: 0x0007AEB5 File Offset: 0x000790B5
		[DataSourceProperty]
		public int DistanceState
		{
			get
			{
				return this._distanceState;
			}
			set
			{
				if (value != this._distanceState)
				{
					this._distanceState = value;
					base.OnPropertyChangedWithValue(value, "DistanceState");
				}
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x0007AED3 File Offset: 0x000790D3
		// (set) Token: 0x06002284 RID: 8836 RVA: 0x0007AEDB File Offset: 0x000790DB
		[DataSourceProperty]
		public int CostState
		{
			get
			{
				return this._costState;
			}
			set
			{
				if (value != this._costState)
				{
					this._costState = value;
					base.OnPropertyChangedWithValue(value, "CostState");
				}
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x0007AEF9 File Offset: 0x000790F9
		// (set) Token: 0x06002286 RID: 8838 RVA: 0x0007AF01 File Offset: 0x00079101
		[DataSourceProperty]
		public int StrengthState
		{
			get
			{
				return this._strengthState;
			}
			set
			{
				if (value != this._strengthState)
				{
					this._strengthState = value;
					base.OnPropertyChangedWithValue(value, "StrengthState");
				}
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x0007AF1F File Offset: 0x0007911F
		// (set) Token: 0x06002288 RID: 8840 RVA: 0x0007AF27 File Offset: 0x00079127
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

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x0007AF45 File Offset: 0x00079145
		// (set) Token: 0x0600228A RID: 8842 RVA: 0x0007AF4D File Offset: 0x0007914D
		[DataSourceProperty]
		public int ClanState
		{
			get
			{
				return this._clanState;
			}
			set
			{
				if (value != this._clanState)
				{
					this._clanState = value;
					base.OnPropertyChangedWithValue(value, "ClanState");
				}
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x0007AF6B File Offset: 0x0007916B
		// (set) Token: 0x0600228C RID: 8844 RVA: 0x0007AF73 File Offset: 0x00079173
		[DataSourceProperty]
		public int ShipCountState
		{
			get
			{
				return this._shipCountState;
			}
			set
			{
				if (value != this._shipCountState)
				{
					this._shipCountState = value;
					base.OnPropertyChangedWithValue(value, "ShipCountState");
				}
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x0007AF91 File Offset: 0x00079191
		// (set) Token: 0x0600228E RID: 8846 RVA: 0x0007AF99 File Offset: 0x00079199
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

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x0007AFB7 File Offset: 0x000791B7
		// (set) Token: 0x06002290 RID: 8848 RVA: 0x0007AFBF File Offset: 0x000791BF
		[DataSourceProperty]
		public bool IsCostSelected
		{
			get
			{
				return this._isCostSelected;
			}
			set
			{
				if (value != this._isCostSelected)
				{
					this._isCostSelected = value;
					base.OnPropertyChangedWithValue(value, "IsCostSelected");
				}
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x0007AFDD File Offset: 0x000791DD
		// (set) Token: 0x06002292 RID: 8850 RVA: 0x0007AFE5 File Offset: 0x000791E5
		[DataSourceProperty]
		public bool IsStrengthSelected
		{
			get
			{
				return this._isStrengthSelected;
			}
			set
			{
				if (value != this._isStrengthSelected)
				{
					this._isStrengthSelected = value;
					base.OnPropertyChangedWithValue(value, "IsStrengthSelected");
				}
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x0007B003 File Offset: 0x00079203
		// (set) Token: 0x06002294 RID: 8852 RVA: 0x0007B00B File Offset: 0x0007920B
		[DataSourceProperty]
		public bool IsDistanceSelected
		{
			get
			{
				return this._isDistanceSelected;
			}
			set
			{
				if (value != this._isDistanceSelected)
				{
					this._isDistanceSelected = value;
					base.OnPropertyChangedWithValue(value, "IsDistanceSelected");
				}
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x0007B029 File Offset: 0x00079229
		// (set) Token: 0x06002296 RID: 8854 RVA: 0x0007B031 File Offset: 0x00079231
		[DataSourceProperty]
		public bool IsClanSelected
		{
			get
			{
				return this._isClanSelected;
			}
			set
			{
				if (value != this._isClanSelected)
				{
					this._isClanSelected = value;
					base.OnPropertyChangedWithValue(value, "IsClanSelected");
				}
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x0007B04F File Offset: 0x0007924F
		// (set) Token: 0x06002298 RID: 8856 RVA: 0x0007B057 File Offset: 0x00079257
		[DataSourceProperty]
		public bool IsShipCountSelected
		{
			get
			{
				return this._isShipCountSelected;
			}
			set
			{
				if (value != this._isShipCountSelected)
				{
					this._isShipCountSelected = value;
					base.OnPropertyChangedWithValue(value, "IsShipCountSelected");
				}
			}
		}

		// Token: 0x04000FC1 RID: 4033
		private readonly MBBindingList<ArmyManagementItemVM> _listToControl;

		// Token: 0x04000FC2 RID: 4034
		private readonly ArmyManagementSortControllerVM.ItemDistanceComparer _distanceComparer;

		// Token: 0x04000FC3 RID: 4035
		private readonly ArmyManagementSortControllerVM.ItemCostComparer _costComparer;

		// Token: 0x04000FC4 RID: 4036
		private readonly ArmyManagementSortControllerVM.ItemStrengthComparer _strengthComparer;

		// Token: 0x04000FC5 RID: 4037
		private readonly ArmyManagementSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000FC6 RID: 4038
		private readonly ArmyManagementSortControllerVM.ItemClanComparer _clanComparer;

		// Token: 0x04000FC7 RID: 4039
		private readonly ArmyManagementSortControllerVM.ItemShipCountComparer _shipCountComparer;

		// Token: 0x04000FC8 RID: 4040
		private int _distanceState;

		// Token: 0x04000FC9 RID: 4041
		private int _costState;

		// Token: 0x04000FCA RID: 4042
		private int _strengthState;

		// Token: 0x04000FCB RID: 4043
		private int _nameState;

		// Token: 0x04000FCC RID: 4044
		private int _clanState;

		// Token: 0x04000FCD RID: 4045
		private int _shipCountState;

		// Token: 0x04000FCE RID: 4046
		private bool _isNameSelected;

		// Token: 0x04000FCF RID: 4047
		private bool _isCostSelected;

		// Token: 0x04000FD0 RID: 4048
		private bool _isStrengthSelected;

		// Token: 0x04000FD1 RID: 4049
		private bool _isDistanceSelected;

		// Token: 0x04000FD2 RID: 4050
		private bool _isClanSelected;

		// Token: 0x04000FD3 RID: 4051
		private bool _isShipCountSelected;

		// Token: 0x020002F4 RID: 756
		public abstract class ItemComparerBase : IComparer<ArmyManagementItemVM>
		{
			// Token: 0x06002885 RID: 10373 RVA: 0x0008732D File Offset: 0x0008552D
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06002886 RID: 10374
			public abstract int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y);

			// Token: 0x06002887 RID: 10375 RVA: 0x00087336 File Offset: 0x00085536
			protected int ResolveEquality(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				return x.LeaderNameText.CompareTo(y.LeaderNameText);
			}

			// Token: 0x04001452 RID: 5202
			protected bool _isAscending;
		}

		// Token: 0x020002F5 RID: 757
		public class ItemDistanceComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002889 RID: 10377 RVA: 0x00087354 File Offset: 0x00085554
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.DistInTime.CompareTo(x.DistInTime);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002F6 RID: 758
		public class ItemCostComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600288B RID: 10379 RVA: 0x00087398 File Offset: 0x00085598
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Cost.CompareTo(x.Cost);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002F7 RID: 759
		public class ItemStrengthComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600288D RID: 10381 RVA: 0x000873DC File Offset: 0x000855DC
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Strength.CompareTo(x.Strength);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				int num2 = y.ShipCount.CompareTo(x.ShipCount);
				if (num2 != 0)
				{
					return num2 * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002F8 RID: 760
		public class ItemNameComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600288F RID: 10383 RVA: 0x00087447 File Offset: 0x00085647
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				if (this._isAscending)
				{
					return y.LeaderNameText.CompareTo(x.LeaderNameText) * -1;
				}
				return y.LeaderNameText.CompareTo(x.LeaderNameText);
			}
		}

		// Token: 0x020002F9 RID: 761
		public class ItemClanComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002891 RID: 10385 RVA: 0x00087480 File Offset: 0x00085680
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002FA RID: 762
		public class ItemShipCountComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002893 RID: 10387 RVA: 0x000874D8 File Offset: 0x000856D8
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.ShipCount.CompareTo(x.ShipCount);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
