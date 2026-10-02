using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection
{
	// Token: 0x0200000C RID: 12
	public class ItemVM : ViewModel
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002C09 File Offset: 0x00000E09
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002C11 File Offset: 0x00000E11
		public int TypeId { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002C1A File Offset: 0x00000E1A
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002C22 File Offset: 0x00000E22
		public int Version { get; protected set; }

		// Token: 0x06000072 RID: 114 RVA: 0x00002C2B File Offset: 0x00000E2B
		public ItemVM()
		{
			this.RefreshValues();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002C47 File Offset: 0x00000E47
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002C4F File Offset: 0x00000E4F
		[DataSourceProperty]
		public EquipmentIndex ItemType
		{
			get
			{
				if (this._itemType == EquipmentIndex.None)
				{
					return this.GetItemTypeWithItemObject();
				}
				return this._itemType;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002C67 File Offset: 0x00000E67
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00002C6F File Offset: 0x00000E6F
		[DataSourceProperty]
		public ItemImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002C8D File Offset: 0x00000E8D
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002C95 File Offset: 0x00000E95
		[DataSourceProperty]
		public string StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				if (value != this._stringId)
				{
					this._stringId = value;
					base.OnPropertyChangedWithValue<string>(value, "StringId");
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002CB8 File Offset: 0x00000EB8
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00002CC0 File Offset: 0x00000EC0
		[DataSourceProperty]
		public string ItemDescription
		{
			get
			{
				return this._itemDescription;
			}
			set
			{
				if (value != this._itemDescription)
				{
					this._itemDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemDescription");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002CE3 File Offset: 0x00000EE3
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002CEB File Offset: 0x00000EEB
		[DataSourceProperty]
		public bool IsFiltered
		{
			get
			{
				return this._isFiltered;
			}
			set
			{
				if (value != this._isFiltered)
				{
					this._isFiltered = value;
					base.OnPropertyChangedWithValue(value, "IsFiltered");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002D09 File Offset: 0x00000F09
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00002D11 File Offset: 0x00000F11
		[DataSourceProperty]
		public int ItemCost
		{
			get
			{
				return this._itemCost;
			}
			set
			{
				if (value != this._itemCost)
				{
					this._itemCost = value;
					base.OnPropertyChangedWithValue(value, "ItemCost");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002D2F File Offset: 0x00000F2F
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00002D37 File Offset: 0x00000F37
		[DataSourceProperty]
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				if (value != this._typeName)
				{
					this._typeName = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeName");
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002D5A File Offset: 0x00000F5A
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00002D62 File Offset: 0x00000F62
		[DataSourceProperty]
		public HintViewModel PreviewHint
		{
			get
			{
				return this._previewHint;
			}
			set
			{
				if (value != this._previewHint)
				{
					this._previewHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PreviewHint");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002D80 File Offset: 0x00000F80
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00002D88 File Offset: 0x00000F88
		[DataSourceProperty]
		public HintViewModel EquipHint
		{
			get
			{
				return this._equipHint;
			}
			set
			{
				if (value != this._equipHint)
				{
					this._equipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipHint");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002DA6 File Offset: 0x00000FA6
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00002DAE File Offset: 0x00000FAE
		[DataSourceProperty]
		public HintViewModel UnequipHint
		{
			get
			{
				return this._unequipHint;
			}
			set
			{
				if (value != this._unequipHint)
				{
					this._unequipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UnequipHint");
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002DCC File Offset: 0x00000FCC
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00002DD4 File Offset: 0x00000FD4
		[DataSourceProperty]
		public BasicTooltipViewModel SlaughterHint
		{
			get
			{
				return this._slaughterHint;
			}
			set
			{
				if (value != this._slaughterHint)
				{
					this._slaughterHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SlaughterHint");
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002DF2 File Offset: 0x00000FF2
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00002DFA File Offset: 0x00000FFA
		[DataSourceProperty]
		public BasicTooltipViewModel DonateHint
		{
			get
			{
				return this._donateHint;
			}
			set
			{
				if (value != this._donateHint)
				{
					this._donateHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DonateHint");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002E18 File Offset: 0x00001018
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00002E20 File Offset: 0x00001020
		[DataSourceProperty]
		public BasicTooltipViewModel BuyAndEquipHint
		{
			get
			{
				return this._buyAndEquip;
			}
			set
			{
				if (value != this._buyAndEquip)
				{
					this._buyAndEquip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "BuyAndEquipHint");
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002E3E File Offset: 0x0000103E
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00002E46 File Offset: 0x00001046
		[DataSourceProperty]
		public BasicTooltipViewModel SellHint
		{
			get
			{
				return this._sellHint;
			}
			set
			{
				if (value != this._sellHint)
				{
					this._sellHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SellHint");
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002E64 File Offset: 0x00001064
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002E6C File Offset: 0x0000106C
		[DataSourceProperty]
		public BasicTooltipViewModel BuyHint
		{
			get
			{
				return this._buyHint;
			}
			set
			{
				if (value != this._buyHint)
				{
					this._buyHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "BuyHint");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002E8A File Offset: 0x0000108A
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002E92 File Offset: 0x00001092
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

		// Token: 0x06000093 RID: 147 RVA: 0x00002EB0 File Offset: 0x000010B0
		public void ExecutePreviewItem()
		{
			if (!UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessPreviewItem(this);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002ECA File Offset: 0x000010CA
		public void ExecuteUnequipItem()
		{
			if (!UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessUnequipItem(this);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002EE4 File Offset: 0x000010E4
		public void ExecuteEquipItem()
		{
			if (!UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessEquipItem(this);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002EFE File Offset: 0x000010FE
		public static void ReleaseStaticContent()
		{
			ItemVM.ProcessEquipItem = null;
			ItemVM.ProcessPreviewItem = null;
			ItemVM.ProcessUnequipItem = null;
			ItemVM.ProcessBuyItem = null;
			ItemVM.ProcessItemSelect = null;
			ItemVM.ProcessItemTooltip = null;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002F24 File Offset: 0x00001124
		public void ExecuteRefreshTooltip()
		{
			if (ItemVM.ProcessItemTooltip != null && !UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessItemTooltip(this);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002F45 File Offset: 0x00001145
		public void ExecuteCancelTooltip()
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002F47 File Offset: 0x00001147
		public void ExecuteBuyItem()
		{
			if (!UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessBuyItem(this, false);
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002F62 File Offset: 0x00001162
		public void ExecuteSelectItem()
		{
			if (!UiStringHelper.IsStringNoneOrEmptyForUi(this.StringId))
			{
				ItemVM.ProcessItemSelect(this);
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002F7C File Offset: 0x0000117C
		public EquipmentIndex GetItemTypeWithItemObject()
		{
			if (this.ItemRosterElement.EquipmentElement.Item == null)
			{
				return EquipmentIndex.None;
			}
			ItemObject.ItemTypeEnum type = this.ItemRosterElement.EquipmentElement.Item.Type;
			switch (type)
			{
			case ItemObject.ItemTypeEnum.Horse:
				return EquipmentIndex.ArmorItemEndSlot;
			case ItemObject.ItemTypeEnum.OneHandedWeapon:
			case ItemObject.ItemTypeEnum.TwoHandedWeapon:
			case ItemObject.ItemTypeEnum.Polearm:
			case ItemObject.ItemTypeEnum.Bow:
			case ItemObject.ItemTypeEnum.Crossbow:
			case ItemObject.ItemTypeEnum.Sling:
			case ItemObject.ItemTypeEnum.Thrown:
			case ItemObject.ItemTypeEnum.Goods:
				break;
			case ItemObject.ItemTypeEnum.Arrows:
			case ItemObject.ItemTypeEnum.Bolts:
			case ItemObject.ItemTypeEnum.SlingStones:
				return EquipmentIndex.WeaponItemBeginSlot;
			case ItemObject.ItemTypeEnum.Shield:
				if (this._typeName == ItemObject.ItemTypeEnum.Invalid.ToString())
				{
					this._typeName = ItemObject.ItemTypeEnum.Horse.ToString();
				}
				return EquipmentIndex.WeaponItemBeginSlot;
			case ItemObject.ItemTypeEnum.HeadArmor:
				return EquipmentIndex.NumAllWeaponSlots;
			case ItemObject.ItemTypeEnum.BodyArmor:
				return EquipmentIndex.Body;
			case ItemObject.ItemTypeEnum.LegArmor:
				return EquipmentIndex.Leg;
			case ItemObject.ItemTypeEnum.HandArmor:
				return EquipmentIndex.Gloves;
			default:
				switch (type)
				{
				case ItemObject.ItemTypeEnum.Cape:
					return EquipmentIndex.Cape;
				case ItemObject.ItemTypeEnum.HorseHarness:
					return EquipmentIndex.HorseHarness;
				case ItemObject.ItemTypeEnum.Banner:
					return EquipmentIndex.ExtraWeaponSlot;
				}
				break;
			}
			if (this.ItemRosterElement.EquipmentElement.Item.WeaponComponent != null)
			{
				return EquipmentIndex.WeaponItemBeginSlot;
			}
			return EquipmentIndex.None;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003084 File Offset: 0x00001284
		protected void OnItemTypeUpdated()
		{
			this.TypeId = (int)this.ItemRosterElement.EquipmentElement.Item.Type;
			this.TypeName = this.ItemRosterElement.EquipmentElement.Item.Type.ToString();
		}

		// Token: 0x0400002A RID: 42
		public static Action<ItemVM> ProcessEquipItem;

		// Token: 0x0400002B RID: 43
		public static Action<ItemVM> ProcessPreviewItem;

		// Token: 0x0400002C RID: 44
		public static Action<ItemVM> ProcessUnequipItem;

		// Token: 0x0400002D RID: 45
		public static Action<ItemVM, bool> ProcessBuyItem;

		// Token: 0x0400002E RID: 46
		public static Action<ItemVM> ProcessItemSelect;

		// Token: 0x0400002F RID: 47
		public static Action<ItemVM> ProcessItemTooltip;

		// Token: 0x04000032 RID: 50
		private string _typeName;

		// Token: 0x04000033 RID: 51
		private int _itemCost = -1;

		// Token: 0x04000034 RID: 52
		private bool _isFiltered;

		// Token: 0x04000035 RID: 53
		private string _itemDescription;

		// Token: 0x04000036 RID: 54
		public ItemRosterElement ItemRosterElement;

		// Token: 0x04000037 RID: 55
		public EquipmentIndex _itemType = EquipmentIndex.None;

		// Token: 0x04000038 RID: 56
		private ItemImageIdentifierVM _imageIdentifier;

		// Token: 0x04000039 RID: 57
		private HintViewModel _previewHint;

		// Token: 0x0400003A RID: 58
		private HintViewModel _equipHint;

		// Token: 0x0400003B RID: 59
		private HintViewModel _unequipHint;

		// Token: 0x0400003C RID: 60
		private BasicTooltipViewModel _buyAndEquip;

		// Token: 0x0400003D RID: 61
		private BasicTooltipViewModel _sellHint;

		// Token: 0x0400003E RID: 62
		private BasicTooltipViewModel _buyHint;

		// Token: 0x0400003F RID: 63
		private HintViewModel _lockHint;

		// Token: 0x04000040 RID: 64
		private BasicTooltipViewModel _slaughterHint;

		// Token: 0x04000041 RID: 65
		private BasicTooltipViewModel _donateHint;

		// Token: 0x04000042 RID: 66
		private string _stringId;
	}
}
