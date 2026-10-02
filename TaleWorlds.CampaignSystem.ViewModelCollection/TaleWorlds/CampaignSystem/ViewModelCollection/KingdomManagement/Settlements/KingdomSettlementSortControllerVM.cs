using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements
{
	// Token: 0x0200006E RID: 110
	public class KingdomSettlementSortControllerVM : ViewModel
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x00025E1C File Offset: 0x0002401C
		public KingdomSettlementSortControllerVM(MBBindingList<KingdomSettlementItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._typeComparer = new KingdomSettlementSortControllerVM.ItemTypeComparer();
			this._prosperityComparer = new KingdomSettlementSortControllerVM.ItemProsperityComparer();
			this._defendersComparer = new KingdomSettlementSortControllerVM.ItemDefendersComparer();
			this._ownerComparer = new KingdomSettlementSortControllerVM.ItemOwnerComparer();
			this._nameComparer = new KingdomSettlementSortControllerVM.ItemNameComparer();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00025E70 File Offset: 0x00024070
		private void ExecuteSortByType()
		{
			int typeState = this.TypeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.TypeState = (typeState + 1) % 3;
			if (this.TypeState == 0)
			{
				this.TypeState++;
			}
			this._typeComparer.SetSortMode(this.TypeState == 1);
			this._listToControl.Sort(this._typeComparer);
			this.IsTypeSelected = true;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00025ED8 File Offset: 0x000240D8
		private void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00025F40 File Offset: 0x00024140
		private void ExecuteSortByOwner()
		{
			int ownerState = this.OwnerState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.OwnerState = (ownerState + 1) % 3;
			if (this.OwnerState == 0)
			{
				this.OwnerState++;
			}
			this._ownerComparer.SetSortMode(this.OwnerState == 1);
			this._listToControl.Sort(this._ownerComparer);
			this.IsOwnerSelected = true;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00025FA8 File Offset: 0x000241A8
		private void ExecuteSortByProsperity()
		{
			int prosperityState = this.ProsperityState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ProsperityState = (prosperityState + 1) % 3;
			if (this.ProsperityState == 0)
			{
				this.ProsperityState++;
			}
			this._prosperityComparer.SetSortMode(this.ProsperityState == 1);
			this._listToControl.Sort(this._prosperityComparer);
			this.IsProsperitySelected = true;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00026010 File Offset: 0x00024210
		private void ExecuteSortByDefenders()
		{
			int defendersState = this.DefendersState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DefendersState = (defendersState + 1) % 3;
			if (this.DefendersState == 0)
			{
				int defendersState2 = this.DefendersState;
				this.DefendersState = defendersState2 + 1;
			}
			this._defendersComparer.SetSortMode(this.DefendersState == 1);
			this._listToControl.Sort(this._defendersComparer);
			this.IsDefendersSelected = true;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002607C File Offset: 0x0002427C
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.TypeState = (int)state;
			this.NameState = (int)state;
			this.OwnerState = (int)state;
			this.ProsperityState = (int)state;
			this.DefendersState = (int)state;
			this.IsTypeSelected = false;
			this.IsNameSelected = false;
			this.IsProsperitySelected = false;
			this.IsOwnerSelected = false;
			this.IsDefendersSelected = false;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x000260CF File Offset: 0x000242CF
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x000260D7 File Offset: 0x000242D7
		[DataSourceProperty]
		public int TypeState
		{
			get
			{
				return this._typeState;
			}
			set
			{
				if (value != this._typeState)
				{
					this._typeState = value;
					base.OnPropertyChangedWithValue(value, "TypeState");
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000260F5 File Offset: 0x000242F5
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x000260FD File Offset: 0x000242FD
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

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0002611B File Offset: 0x0002431B
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00026123 File Offset: 0x00024323
		[DataSourceProperty]
		public int OwnerState
		{
			get
			{
				return this._ownerState;
			}
			set
			{
				if (value != this._ownerState)
				{
					this._ownerState = value;
					base.OnPropertyChangedWithValue(value, "OwnerState");
				}
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x00026141 File Offset: 0x00024341
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x00026149 File Offset: 0x00024349
		[DataSourceProperty]
		public int ProsperityState
		{
			get
			{
				return this._prosperityState;
			}
			set
			{
				if (value != this._prosperityState)
				{
					this._prosperityState = value;
					base.OnPropertyChangedWithValue(value, "ProsperityState");
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00026167 File Offset: 0x00024367
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0002616F File Offset: 0x0002436F
		[DataSourceProperty]
		public int DefendersState
		{
			get
			{
				return this._defendersState;
			}
			set
			{
				if (value != this._defendersState)
				{
					this._defendersState = value;
					base.OnPropertyChangedWithValue(value, "DefendersState");
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0002618D File Offset: 0x0002438D
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x00026195 File Offset: 0x00024395
		[DataSourceProperty]
		public bool IsTypeSelected
		{
			get
			{
				return this._isTypeSelected;
			}
			set
			{
				if (value != this._isTypeSelected)
				{
					this._isTypeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsTypeSelected");
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x000261B3 File Offset: 0x000243B3
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x000261BB File Offset: 0x000243BB
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

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x000261D9 File Offset: 0x000243D9
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x000261E1 File Offset: 0x000243E1
		[DataSourceProperty]
		public bool IsDefendersSelected
		{
			get
			{
				return this._isDefendersSelected;
			}
			set
			{
				if (value != this._isDefendersSelected)
				{
					this._isDefendersSelected = value;
					base.OnPropertyChangedWithValue(value, "IsDefendersSelected");
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000261FF File Offset: 0x000243FF
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x00026207 File Offset: 0x00024407
		[DataSourceProperty]
		public bool IsOwnerSelected
		{
			get
			{
				return this._isOwnerSelected;
			}
			set
			{
				if (value != this._isOwnerSelected)
				{
					this._isOwnerSelected = value;
					base.OnPropertyChangedWithValue(value, "IsOwnerSelected");
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00026225 File Offset: 0x00024425
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0002622D File Offset: 0x0002442D
		[DataSourceProperty]
		public bool IsProsperitySelected
		{
			get
			{
				return this._isProsperitySelected;
			}
			set
			{
				if (value != this._isProsperitySelected)
				{
					this._isProsperitySelected = value;
					base.OnPropertyChangedWithValue(value, "IsProsperitySelected");
				}
			}
		}

		// Token: 0x04000383 RID: 899
		private readonly MBBindingList<KingdomSettlementItemVM> _listToControl;

		// Token: 0x04000384 RID: 900
		private readonly KingdomSettlementSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x04000385 RID: 901
		private readonly KingdomSettlementSortControllerVM.ItemProsperityComparer _prosperityComparer;

		// Token: 0x04000386 RID: 902
		private readonly KingdomSettlementSortControllerVM.ItemDefendersComparer _defendersComparer;

		// Token: 0x04000387 RID: 903
		private readonly KingdomSettlementSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000388 RID: 904
		private readonly KingdomSettlementSortControllerVM.ItemOwnerComparer _ownerComparer;

		// Token: 0x04000389 RID: 905
		private int _typeState;

		// Token: 0x0400038A RID: 906
		private int _nameState;

		// Token: 0x0400038B RID: 907
		private int _ownerState;

		// Token: 0x0400038C RID: 908
		private int _prosperityState;

		// Token: 0x0400038D RID: 909
		private int _defendersState;

		// Token: 0x0400038E RID: 910
		private bool _isTypeSelected;

		// Token: 0x0400038F RID: 911
		private bool _isNameSelected;

		// Token: 0x04000390 RID: 912
		private bool _isOwnerSelected;

		// Token: 0x04000391 RID: 913
		private bool _isProsperitySelected;

		// Token: 0x04000392 RID: 914
		private bool _isDefendersSelected;

		// Token: 0x020001CB RID: 459
		public abstract class ItemComparerBase : IComparer<KingdomSettlementItemVM>
		{
			// Token: 0x0600249A RID: 9370 RVA: 0x00080885 File Offset: 0x0007EA85
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600249B RID: 9371
			public abstract int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y);

			// Token: 0x0600249C RID: 9372 RVA: 0x0008088E File Offset: 0x0007EA8E
			protected int ResolveEquality(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				return x.Settlement.Name.ToString().CompareTo(y.Settlement.Name.ToString());
			}

			// Token: 0x04001147 RID: 4423
			protected bool _isAscending;
		}

		// Token: 0x020001CC RID: 460
		public class ItemNameComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600249E RID: 9374 RVA: 0x000808C0 File Offset: 0x0007EAC0
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				if (this._isAscending)
				{
					return y.Settlement.Name.ToString().CompareTo(x.Settlement.Name.ToString()) * -1;
				}
				return y.Settlement.Name.ToString().CompareTo(x.Settlement.Name.ToString());
			}
		}

		// Token: 0x020001CD RID: 461
		public class ItemClanComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A0 RID: 9376 RVA: 0x0008092C File Offset: 0x0007EB2C
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Settlement.OwnerClan.Name.ToString().CompareTo(x.Settlement.OwnerClan.Name.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CE RID: 462
		public class ItemOwnerComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A2 RID: 9378 RVA: 0x0008098C File Offset: 0x0007EB8C
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Owner.NameText.CompareTo(x.Owner.NameText);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CF RID: 463
		public class ItemVillagesComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A4 RID: 9380 RVA: 0x000809D8 File Offset: 0x0007EBD8
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Villages.Count.CompareTo(x.Villages.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001D0 RID: 464
		public class ItemTypeComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A6 RID: 9382 RVA: 0x00080A28 File Offset: 0x0007EC28
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Settlement.IsCastle.CompareTo(x.Settlement.IsCastle);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001D1 RID: 465
		public class ItemProsperityComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A8 RID: 9384 RVA: 0x00080A78 File Offset: 0x0007EC78
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Prosperity.CompareTo(x.Prosperity);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001D2 RID: 466
		public class ItemFoodComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024AA RID: 9386 RVA: 0x00080ABC File Offset: 0x0007ECBC
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				float num = ((y.Settlement.Town != null) ? y.Settlement.Town.FoodStocks : 0f);
				float num2 = ((x.Settlement.Town != null) ? x.Settlement.Town.FoodStocks : 0f);
				int num3 = num.CompareTo(num2);
				if (num3 != 0)
				{
					return num3 * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001D3 RID: 467
		public class ItemGarrisonComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024AC RID: 9388 RVA: 0x00080B40 File Offset: 0x0007ED40
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Garrison.CompareTo(x.Garrison);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001D4 RID: 468
		private class ItemDefendersComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024AE RID: 9390 RVA: 0x00080B84 File Offset: 0x0007ED84
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Defenders.CompareTo(x.Defenders);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
