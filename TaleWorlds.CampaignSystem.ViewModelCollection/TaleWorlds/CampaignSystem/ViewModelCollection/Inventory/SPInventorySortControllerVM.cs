using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000099 RID: 153
	public class SPInventorySortControllerVM : ViewModel
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x000370DA File Offset: 0x000352DA
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x000370E2 File Offset: 0x000352E2
		public SPInventorySortControllerVM.InventoryItemSortOption? CurrentSortOption { get; private set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x000370EB File Offset: 0x000352EB
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x000370F3 File Offset: 0x000352F3
		public SPInventorySortControllerVM.InventoryItemSortState? CurrentSortState { get; private set; }

		// Token: 0x06000D03 RID: 3331 RVA: 0x000370FC File Offset: 0x000352FC
		public SPInventorySortControllerVM(ref MBBindingList<SPItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._typeComparer = new SPInventorySortControllerVM.ItemTypeComparer();
			this._nameComparer = new SPInventorySortControllerVM.ItemNameComparer();
			this._quantityComparer = new SPInventorySortControllerVM.ItemQuantityComparer();
			this._costComparer = new SPInventorySortControllerVM.ItemCostComparer();
			this.RefreshValues();
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00037149 File Offset: 0x00035349
		public void SortByOption(SPInventorySortControllerVM.InventoryItemSortOption sortOption, SPInventorySortControllerVM.InventoryItemSortState sortState)
		{
			this.SetAllStates((sortState == SPInventorySortControllerVM.InventoryItemSortState.Ascending) ? SPInventorySortControllerVM.InventoryItemSortState.Descending : SPInventorySortControllerVM.InventoryItemSortState.Ascending);
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Type)
			{
				this.ExecuteSortByType();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Name)
			{
				this.ExecuteSortByName();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Quantity)
			{
				this.ExecuteSortByQuantity();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Cost)
			{
				this.ExecuteSortByCost();
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00037183 File Offset: 0x00035383
		public void SortByDefaultState()
		{
			this.ExecuteSortByType();
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0003718C File Offset: 0x0003538C
		public void SortByCurrentState()
		{
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Type);
				return;
			}
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Name);
				return;
			}
			if (this.IsQuantitySelected)
			{
				this._listToControl.Sort(this._quantityComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Quantity);
				return;
			}
			if (this.IsCostSelected)
			{
				this._listToControl.Sort(this._costComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Cost);
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00037230 File Offset: 0x00035430
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.NameState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Name);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000372BC File Offset: 0x000354BC
		public void ExecuteSortByType()
		{
			int typeState = this.TypeState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.TypeState = (typeState + 1) % 3;
			if (this.TypeState == 0)
			{
				this.TypeState++;
			}
			this._typeComparer.SetSortMode(this.TypeState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.TypeState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._typeComparer);
			this.IsTypeSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Type);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00037348 File Offset: 0x00035548
		public void ExecuteSortByQuantity()
		{
			int quantityState = this.QuantityState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.QuantityState = (quantityState + 1) % 3;
			if (this.QuantityState == 0)
			{
				this.QuantityState++;
			}
			this._quantityComparer.SetSortMode(this.QuantityState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.QuantityState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._quantityComparer);
			this.IsQuantitySelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Quantity);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000373D4 File Offset: 0x000355D4
		public void ExecuteSortByCost()
		{
			int costState = this.CostState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.CostState = (costState + 1) % 3;
			if (this.CostState == 0)
			{
				this.CostState++;
			}
			this._costComparer.SetSortMode(this.CostState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.CostState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._costComparer);
			this.IsCostSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Cost);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00037460 File Offset: 0x00035660
		private void SetAllStates(SPInventorySortControllerVM.InventoryItemSortState state)
		{
			this.TypeState = (int)state;
			this.NameState = (int)state;
			this.QuantityState = (int)state;
			this.CostState = (int)state;
			this.IsTypeSelected = false;
			this.IsNameSelected = false;
			this.IsQuantitySelected = false;
			this.IsCostSelected = false;
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?(state);
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000374B1 File Offset: 0x000356B1
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x000374B9 File Offset: 0x000356B9
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

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000374D7 File Offset: 0x000356D7
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x000374DF File Offset: 0x000356DF
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

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x000374FD File Offset: 0x000356FD
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00037505 File Offset: 0x00035705
		[DataSourceProperty]
		public int QuantityState
		{
			get
			{
				return this._quantityState;
			}
			set
			{
				if (value != this._quantityState)
				{
					this._quantityState = value;
					base.OnPropertyChangedWithValue(value, "QuantityState");
				}
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00037523 File Offset: 0x00035723
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x0003752B File Offset: 0x0003572B
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

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00037549 File Offset: 0x00035749
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00037551 File Offset: 0x00035751
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

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0003756F File Offset: 0x0003576F
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00037577 File Offset: 0x00035777
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

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00037595 File Offset: 0x00035795
		// (set) Token: 0x06000D19 RID: 3353 RVA: 0x0003759D File Offset: 0x0003579D
		[DataSourceProperty]
		public bool IsQuantitySelected
		{
			get
			{
				return this._isQuantitySelected;
			}
			set
			{
				if (value != this._isQuantitySelected)
				{
					this._isQuantitySelected = value;
					base.OnPropertyChangedWithValue(value, "IsQuantitySelected");
				}
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x000375BB File Offset: 0x000357BB
		// (set) Token: 0x06000D1B RID: 3355 RVA: 0x000375C3 File Offset: 0x000357C3
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

		// Token: 0x040005E3 RID: 1507
		private MBBindingList<SPItemVM> _listToControl;

		// Token: 0x040005E4 RID: 1508
		private SPInventorySortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x040005E5 RID: 1509
		private SPInventorySortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040005E6 RID: 1510
		private SPInventorySortControllerVM.ItemQuantityComparer _quantityComparer;

		// Token: 0x040005E7 RID: 1511
		private SPInventorySortControllerVM.ItemCostComparer _costComparer;

		// Token: 0x040005EA RID: 1514
		private int _typeState;

		// Token: 0x040005EB RID: 1515
		private int _nameState;

		// Token: 0x040005EC RID: 1516
		private int _quantityState;

		// Token: 0x040005ED RID: 1517
		private int _costState;

		// Token: 0x040005EE RID: 1518
		private bool _isTypeSelected;

		// Token: 0x040005EF RID: 1519
		private bool _isNameSelected;

		// Token: 0x040005F0 RID: 1520
		private bool _isQuantitySelected;

		// Token: 0x040005F1 RID: 1521
		private bool _isCostSelected;

		// Token: 0x02000200 RID: 512
		public enum InventoryItemSortState
		{
			// Token: 0x040011B2 RID: 4530
			Default,
			// Token: 0x040011B3 RID: 4531
			Ascending,
			// Token: 0x040011B4 RID: 4532
			Descending
		}

		// Token: 0x02000201 RID: 513
		public enum InventoryItemSortOption
		{
			// Token: 0x040011B6 RID: 4534
			Type,
			// Token: 0x040011B7 RID: 4535
			Name,
			// Token: 0x040011B8 RID: 4536
			Quantity,
			// Token: 0x040011B9 RID: 4537
			Cost
		}

		// Token: 0x02000202 RID: 514
		public abstract class ItemComparer : IComparer<SPItemVM>
		{
			// Token: 0x0600253B RID: 9531 RVA: 0x000815C7 File Offset: 0x0007F7C7
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600253C RID: 9532
			public abstract int Compare(SPItemVM x, SPItemVM y);

			// Token: 0x0600253D RID: 9533 RVA: 0x000815D0 File Offset: 0x0007F7D0
			protected int ResolveEquality(SPItemVM x, SPItemVM y)
			{
				return x.ItemDescription.CompareTo(y.ItemDescription);
			}

			// Token: 0x040011BA RID: 4538
			protected bool _isAscending;
		}

		// Token: 0x02000203 RID: 515
		public class ItemTypeComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x0600253F RID: 9535 RVA: 0x000815EC File Offset: 0x0007F7EC
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int itemObjectTypeSortIndex = CampaignUIHelper.GetItemObjectTypeSortIndex(x.ItemRosterElement.EquipmentElement.Item);
				int num = CampaignUIHelper.GetItemObjectTypeSortIndex(y.ItemRosterElement.EquipmentElement.Item).CompareTo(itemObjectTypeSortIndex);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				num = x.ItemCost.CompareTo(y.ItemCost);
				if (num != 0)
				{
					return num;
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x02000204 RID: 516
		public class ItemNameComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002541 RID: 9537 RVA: 0x00081671 File Offset: 0x0007F871
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				if (this._isAscending)
				{
					return y.ItemDescription.CompareTo(x.ItemDescription) * -1;
				}
				return y.ItemDescription.CompareTo(x.ItemDescription);
			}
		}

		// Token: 0x02000205 RID: 517
		public class ItemQuantityComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002543 RID: 9539 RVA: 0x000816A8 File Offset: 0x0007F8A8
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int num = y.ItemCount.CompareTo(x.ItemCount);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x02000206 RID: 518
		public class ItemCostComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002545 RID: 9541 RVA: 0x000816EC File Offset: 0x0007F8EC
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int num = y.ItemCost.CompareTo(x.ItemCost);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
