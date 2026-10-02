using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000117 RID: 279
	public class SmeltingItemVM : ViewModel
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x0006077A File Offset: 0x0005E97A
		// (set) Token: 0x0600193D RID: 6461 RVA: 0x00060771 File Offset: 0x0005E971
		public EquipmentElement EquipmentElement { get; private set; }

		// Token: 0x0600193F RID: 6463 RVA: 0x00060784 File Offset: 0x0005E984
		public SmeltingItemVM(EquipmentElement equipmentElement, Action<SmeltingItemVM> onSelection, Action<SmeltingItemVM, bool> onItemLockedStateChange, bool isLocked, int numOfItems)
		{
			this._onSelection = onSelection;
			this._onItemLockedStateChange = onItemLockedStateChange;
			this.EquipmentElement = equipmentElement;
			this.Yield = new MBBindingList<CraftingResourceItemVM>();
			this.InputMaterials = new MBBindingList<CraftingResourceItemVM>();
			this.LockHint = new HintViewModel(GameTexts.FindText("str_lock_in_inventory", null).SetTextVariable("TRANSFERABLE", GameTexts.FindText("str_items", null).ToString()), null);
			int[] smeltingOutputForItem = Campaign.Current.Models.SmithingModel.GetSmeltingOutputForItem(equipmentElement.Item);
			for (int i = 0; i < smeltingOutputForItem.Length; i++)
			{
				if (smeltingOutputForItem[i] > 0)
				{
					this.Yield.Add(new CraftingResourceItemVM((CraftingMaterials)i, smeltingOutputForItem[i], 0));
				}
				else if (smeltingOutputForItem[i] < 0)
				{
					this.InputMaterials.Add(new CraftingResourceItemVM((CraftingMaterials)i, -smeltingOutputForItem[i], 0));
				}
			}
			this.IsLocked = isLocked;
			this.Visual = new ItemImageIdentifierVM(equipmentElement.Item, "");
			this.NumOfItems = numOfItems;
			this.HasMoreThanOneItem = this.NumOfItems > 1;
			this.RefreshValues();
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x00060894 File Offset: 0x0005EA94
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.EquipmentElement.Item.Name.ToString();
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000608C5 File Offset: 0x0005EAC5
		public void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000608D3 File Offset: 0x0005EAD3
		public void ExecuteShowItemTooltip()
		{
			InformationManager.ShowTooltip(typeof(ItemObject), new object[] { this.EquipmentElement });
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000608F8 File Offset: 0x0005EAF8
		public void ExecuteHideItemTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x000608FF File Offset: 0x0005EAFF
		// (set) Token: 0x06001945 RID: 6469 RVA: 0x00060907 File Offset: 0x0005EB07
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x00060925 File Offset: 0x0005EB25
		// (set) Token: 0x06001947 RID: 6471 RVA: 0x0006092D File Offset: 0x0005EB2D
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> Yield
		{
			get
			{
				return this._yield;
			}
			set
			{
				if (value != this._yield)
				{
					this._yield = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "Yield");
				}
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0006094B File Offset: 0x0005EB4B
		// (set) Token: 0x06001949 RID: 6473 RVA: 0x00060953 File Offset: 0x0005EB53
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> InputMaterials
		{
			get
			{
				return this._inputMaterials;
			}
			set
			{
				if (value != this._inputMaterials)
				{
					this._inputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "InputMaterials");
				}
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x00060971 File Offset: 0x0005EB71
		// (set) Token: 0x0600194B RID: 6475 RVA: 0x00060979 File Offset: 0x0005EB79
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x0006099C File Offset: 0x0005EB9C
		// (set) Token: 0x0600194D RID: 6477 RVA: 0x000609A4 File Offset: 0x0005EBA4
		[DataSourceProperty]
		public int NumOfItems
		{
			get
			{
				return this._numOfItems;
			}
			set
			{
				if (value != this._numOfItems)
				{
					this._numOfItems = value;
					base.OnPropertyChangedWithValue(value, "NumOfItems");
				}
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x000609C2 File Offset: 0x0005EBC2
		// (set) Token: 0x0600194F RID: 6479 RVA: 0x000609CA File Offset: 0x0005EBCA
		[DataSourceProperty]
		public bool HasMoreThanOneItem
		{
			get
			{
				return this._hasMoreThanOneItem;
			}
			set
			{
				if (value != this._hasMoreThanOneItem)
				{
					this._hasMoreThanOneItem = value;
					base.OnPropertyChangedWithValue(value, "HasMoreThanOneItem");
				}
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x000609E8 File Offset: 0x0005EBE8
		// (set) Token: 0x06001951 RID: 6481 RVA: 0x000609F0 File Offset: 0x0005EBF0
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x00060A0E File Offset: 0x0005EC0E
		// (set) Token: 0x06001953 RID: 6483 RVA: 0x00060A16 File Offset: 0x0005EC16
		[DataSourceProperty]
		public HintViewModel LockHint
		{
			get
			{
				return this._lockHint;
			}
			set
			{
				if (value != this._lockHint)
				{
					this._lockHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockHint");
				}
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00060A34 File Offset: 0x0005EC34
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00060A3C File Offset: 0x0005EC3C
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
					this._onItemLockedStateChange(this, value);
				}
			}
		}

		// Token: 0x04000B8D RID: 2957
		private readonly Action<SmeltingItemVM> _onSelection;

		// Token: 0x04000B8E RID: 2958
		private readonly Action<SmeltingItemVM, bool> _onItemLockedStateChange;

		// Token: 0x04000B8F RID: 2959
		private ItemImageIdentifierVM _visual;

		// Token: 0x04000B90 RID: 2960
		private string _name;

		// Token: 0x04000B91 RID: 2961
		private int _numOfItems;

		// Token: 0x04000B92 RID: 2962
		private MBBindingList<CraftingResourceItemVM> _inputMaterials;

		// Token: 0x04000B93 RID: 2963
		private MBBindingList<CraftingResourceItemVM> _yield;

		// Token: 0x04000B94 RID: 2964
		private HintViewModel _lockHint;

		// Token: 0x04000B95 RID: 2965
		private bool _isSelected;

		// Token: 0x04000B96 RID: 2966
		private bool _isLocked;

		// Token: 0x04000B97 RID: 2967
		private bool _hasMoreThanOneItem;
	}
}
