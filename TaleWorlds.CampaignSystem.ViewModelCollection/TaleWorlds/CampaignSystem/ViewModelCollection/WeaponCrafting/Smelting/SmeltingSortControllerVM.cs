using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000118 RID: 280
	public class SmeltingSortControllerVM : ViewModel
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x00060A67 File Offset: 0x0005EC67
		public SmeltingSortControllerVM()
		{
			this._yieldComparer = new SmeltingSortControllerVM.ItemYieldComparer();
			this._typeComparer = new SmeltingSortControllerVM.ItemTypeComparer();
			this._nameComparer = new SmeltingSortControllerVM.ItemNameComparer();
			this.RefreshValues();
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00060A98 File Offset: 0x0005EC98
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SortNameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.SortTypeText = new TextObject("{=zMMqgxb1}Type", null).ToString();
			this.SortYieldText = new TextObject("{=v3OF6vBg}Yield", null).ToString();
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00060AED File Offset: 0x0005ECED
		public void SetListToControl(MBBindingList<SmeltingItemVM> listToControl)
		{
			this._listToControl = listToControl;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00060AF8 File Offset: 0x0005ECF8
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsYieldSelected)
			{
				this._listToControl.Sort(this._yieldComparer);
				return;
			}
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
			}
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00060B54 File Offset: 0x0005ED54
		public void ExecuteSortByName()
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

		// Token: 0x0600195B RID: 6491 RVA: 0x00060BBC File Offset: 0x0005EDBC
		public void ExecuteSortByYield()
		{
			int yieldState = this.YieldState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.YieldState = (yieldState + 1) % 3;
			if (this.YieldState == 0)
			{
				this.YieldState++;
			}
			this._yieldComparer.SetSortMode(this.YieldState == 1);
			this._listToControl.Sort(this._yieldComparer);
			this.IsYieldSelected = true;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00060C24 File Offset: 0x0005EE24
		public void ExecuteSortByType()
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

		// Token: 0x0600195D RID: 6493 RVA: 0x00060C8C File Offset: 0x0005EE8C
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.TypeState = (int)state;
			this.YieldState = (int)state;
			this.IsNameSelected = false;
			this.IsTypeSelected = false;
			this.IsYieldSelected = false;
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x00060CB8 File Offset: 0x0005EEB8
		// (set) Token: 0x0600195F RID: 6495 RVA: 0x00060CC0 File Offset: 0x0005EEC0
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

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00060CDE File Offset: 0x0005EEDE
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x00060CE6 File Offset: 0x0005EEE6
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

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x00060D04 File Offset: 0x0005EF04
		// (set) Token: 0x06001963 RID: 6499 RVA: 0x00060D0C File Offset: 0x0005EF0C
		[DataSourceProperty]
		public int YieldState
		{
			get
			{
				return this._yieldState;
			}
			set
			{
				if (value != this._yieldState)
				{
					this._yieldState = value;
					base.OnPropertyChangedWithValue(value, "YieldState");
				}
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x00060D2A File Offset: 0x0005EF2A
		// (set) Token: 0x06001965 RID: 6501 RVA: 0x00060D32 File Offset: 0x0005EF32
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

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x00060D50 File Offset: 0x0005EF50
		// (set) Token: 0x06001967 RID: 6503 RVA: 0x00060D58 File Offset: 0x0005EF58
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

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x00060D76 File Offset: 0x0005EF76
		// (set) Token: 0x06001969 RID: 6505 RVA: 0x00060D7E File Offset: 0x0005EF7E
		[DataSourceProperty]
		public bool IsYieldSelected
		{
			get
			{
				return this._isYieldSelected;
			}
			set
			{
				if (value != this._isYieldSelected)
				{
					this._isYieldSelected = value;
					base.OnPropertyChangedWithValue(value, "IsYieldSelected");
				}
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x00060D9C File Offset: 0x0005EF9C
		// (set) Token: 0x0600196B RID: 6507 RVA: 0x00060DA4 File Offset: 0x0005EFA4
		[DataSourceProperty]
		public string SortTypeText
		{
			get
			{
				return this._sortTypeText;
			}
			set
			{
				if (value != this._sortTypeText)
				{
					this._sortTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortTypeText");
				}
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x00060DC7 File Offset: 0x0005EFC7
		// (set) Token: 0x0600196D RID: 6509 RVA: 0x00060DCF File Offset: 0x0005EFCF
		[DataSourceProperty]
		public string SortNameText
		{
			get
			{
				return this._sortNameText;
			}
			set
			{
				if (value != this._sortNameText)
				{
					this._sortNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortNameText");
				}
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x00060DF2 File Offset: 0x0005EFF2
		// (set) Token: 0x0600196F RID: 6511 RVA: 0x00060DFA File Offset: 0x0005EFFA
		[DataSourceProperty]
		public string SortYieldText
		{
			get
			{
				return this._sortYieldText;
			}
			set
			{
				if (value != this._sortYieldText)
				{
					this._sortYieldText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortYieldText");
				}
			}
		}

		// Token: 0x04000B98 RID: 2968
		private MBBindingList<SmeltingItemVM> _listToControl;

		// Token: 0x04000B99 RID: 2969
		private readonly SmeltingSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000B9A RID: 2970
		private readonly SmeltingSortControllerVM.ItemYieldComparer _yieldComparer;

		// Token: 0x04000B9B RID: 2971
		private readonly SmeltingSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x04000B9C RID: 2972
		private int _nameState;

		// Token: 0x04000B9D RID: 2973
		private int _yieldState;

		// Token: 0x04000B9E RID: 2974
		private int _typeState;

		// Token: 0x04000B9F RID: 2975
		private bool _isNameSelected;

		// Token: 0x04000BA0 RID: 2976
		private bool _isYieldSelected;

		// Token: 0x04000BA1 RID: 2977
		private bool _isTypeSelected;

		// Token: 0x04000BA2 RID: 2978
		private string _sortTypeText;

		// Token: 0x04000BA3 RID: 2979
		private string _sortNameText;

		// Token: 0x04000BA4 RID: 2980
		private string _sortYieldText;

		// Token: 0x0200027E RID: 638
		public abstract class ItemComparerBase : IComparer<SmeltingItemVM>
		{
			// Token: 0x060026C9 RID: 9929 RVA: 0x00083909 File Offset: 0x00081B09
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060026CA RID: 9930
			public abstract int Compare(SmeltingItemVM x, SmeltingItemVM y);

			// Token: 0x060026CB RID: 9931 RVA: 0x00083912 File Offset: 0x00081B12
			protected int ResolveEquality(SmeltingItemVM x, SmeltingItemVM y)
			{
				return x.Name.CompareTo(y.Name);
			}

			// Token: 0x04001310 RID: 4880
			protected bool _isAscending;
		}

		// Token: 0x0200027F RID: 639
		public class ItemNameComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060026CD RID: 9933 RVA: 0x0008392D File Offset: 0x00081B2D
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				if (this._isAscending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x02000280 RID: 640
		public class ItemYieldComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060026CF RID: 9935 RVA: 0x00083964 File Offset: 0x00081B64
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				int num = y.Yield.Count.CompareTo(x.Yield.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x02000281 RID: 641
		public class ItemTypeComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060026D1 RID: 9937 RVA: 0x000839B4 File Offset: 0x00081BB4
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				int itemObjectTypeSortIndex = CampaignUIHelper.GetItemObjectTypeSortIndex(x.EquipmentElement.Item);
				int num = CampaignUIHelper.GetItemObjectTypeSortIndex(y.EquipmentElement.Item).CompareTo(itemObjectTypeSortIndex);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
