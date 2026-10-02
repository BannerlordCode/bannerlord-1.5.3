using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000110 RID: 272
	public class WeaponDesignResultPopupVM : ViewModel
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0005BCBC File Offset: 0x00059EBC
		public WeaponDesignResultPopupVM(ItemObject craftedItem, TextObject itemName, Action onFinalize, Crafting crafting, CraftingOrder completedOrder, ItemCollectionElementViewModel itemVisualModel, MBBindingList<ItemFlagVM> weaponFlagIconsList, Func<CraftingSecondaryUsageItemVM, MBBindingList<WeaponDesignResultPropertyItemVM>> onGetPropertyList, Action<CraftingSecondaryUsageItemVM> onUsageSelected)
		{
			this._craftedItem = craftedItem;
			this._onFinalize = onFinalize;
			this._crafting = crafting;
			this._completedOrder = completedOrder;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this._onUsageSelected = onUsageSelected;
			this.SecondaryUsageSelector = new SelectorVM<CraftingSecondaryUsageItemVM>(new List<string>(), -1, new Action<SelectorVM<CraftingSecondaryUsageItemVM>>(this.OnUsageSelected));
			this.WeaponFlagIconsList = weaponFlagIconsList;
			this._onGetPropertyList = onGetPropertyList;
			ItemModifier currentItemModifier = this._craftingBehavior.GetCurrentItemModifier();
			if (currentItemModifier != null)
			{
				TextObject textObject = currentItemModifier.Name.CopyTextObject();
				textObject.SetTextVariable("ITEMNAME", itemName.ToString());
				this.ItemName = textObject.ToString();
			}
			else
			{
				this.ItemName = itemName.ToString();
			}
			this.ItemName = this.ItemName.Trim();
			this.ItemVisualModel = itemVisualModel;
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<CraftingWeaponResultPopupToggledEvent>(new CraftingWeaponResultPopupToggledEvent(true));
			}
			this.RefreshValues();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0005BDB4 File Offset: 0x00059FB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.IsInOrderMode = this._completedOrder != null;
			this.WeaponCraftedText = new TextObject("{=0mqdFC2x}Weapon Crafted!", null).ToString();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.RefreshUsages();
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0005BE08 File Offset: 0x0005A008
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0005BE1C File Offset: 0x0005A01C
		private void RefreshUsages()
		{
			this.SecondaryUsageSelector.ItemList.Clear();
			MBReadOnlyList<WeaponComponentData> weapons = this._crafting.GetCurrentCraftedItemObject(false, null).Weapons;
			int num = this.SecondaryUsageSelector.SelectedIndex;
			int num2 = 0;
			for (int i = 0; i < weapons.Count; i++)
			{
				if (CampaignUIHelper.IsItemUsageApplicable(weapons[i]))
				{
					TextObject textObject = GameTexts.FindText("str_weapon_usage", weapons[i].WeaponDescriptionId);
					this.SecondaryUsageSelector.AddItem(new CraftingSecondaryUsageItemVM(textObject, num2, i, this.SecondaryUsageSelector));
					if (this.IsInOrderMode)
					{
						WeaponComponentData orderWeapon = this._completedOrder.GetStatWeapon();
						num = this._crafting.GetCurrentCraftedItemObject(false, null).Weapons.FindIndex((WeaponComponentData x) => x.WeaponDescriptionId == orderWeapon.WeaponDescriptionId);
					}
					else
					{
						CraftingOrder completedOrder = this._completedOrder;
						if (((completedOrder != null) ? completedOrder.GetStatWeapon().WeaponDescriptionId : null) == weapons[i].WeaponDescriptionId)
						{
							num = num2;
						}
					}
					num2++;
				}
			}
			this.SecondaryUsageSelector.SelectedIndex = ((num >= 0) ? num : 0);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0005BF40 File Offset: 0x0005A140
		private void OnUsageSelected(SelectorVM<CraftingSecondaryUsageItemVM> selector)
		{
			Func<CraftingSecondaryUsageItemVM, MBBindingList<WeaponDesignResultPropertyItemVM>> onGetPropertyList = this._onGetPropertyList;
			this.DesignResultPropertyList = ((onGetPropertyList != null) ? onGetPropertyList(selector.SelectedItem) : null);
			if (this._isInOrderMode)
			{
				bool flag;
				TextObject textObject;
				TextObject textObject2;
				int num;
				this._craftingBehavior.GetOrderResult(this._completedOrder, this._craftedItem, out flag, out textObject, out textObject2, out num);
				this.CraftedWeaponInitialWorth = this._completedOrder.BaseGoldReward;
				this.CraftedWeaponFinalWorth = num;
				this.IsOrderSuccessful = flag;
				this.CraftedWeaponWorthText = new TextObject("{=ZIn8W5ZG}Worth", null).ToString();
				this.DesignResultPropertyList.Add(new WeaponDesignResultPropertyItemVM(new TextObject("{=QmfZjCo1}Worth: ", null), (float)this.CraftedWeaponInitialWorth, (float)this.CraftedWeaponInitialWorth, (float)(this.CraftedWeaponFinalWorth - this.CraftedWeaponInitialWorth), false, true, false));
				this.OrderOwnerRemarkText = textObject.ToString();
				this.OrderResultText = textObject2.ToString();
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0005C01C File Offset: 0x0005A21C
		private void UpdateConfirmAvailability()
		{
			if (this.IsInOrderMode)
			{
				this.CanConfirm = true;
				this.ConfirmDisabledReasonHint = new HintViewModel();
				return;
			}
			Tuple<bool, TextObject> tuple = CampaignUIHelper.IsStringApplicableForItemName(this.ItemName);
			this.CanConfirm = tuple.Item1;
			this.ConfirmDisabledReasonHint = new HintViewModel(tuple.Item2, null);
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0005C070 File Offset: 0x0005A270
		public void ExecuteFinalizeCrafting()
		{
			TextObject textObject = new TextObject("{=!}" + this.ItemName, null);
			this._crafting.SetCraftedWeaponName(textObject);
			this._craftingBehavior.SetCraftedWeaponName(this._craftedItem, textObject);
			Action onFinalize = this._onFinalize;
			if (onFinalize != null)
			{
				onFinalize();
			}
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<CraftingWeaponResultPopupToggledEvent>(new CraftingWeaponResultPopupToggledEvent(false));
			}
			if (!this._isInOrderMode)
			{
				TextObject textObject2 = GameTexts.FindText("crafting_added_to_inventory", null);
				textObject2.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, false);
				textObject2.SetTextVariable("ITEM_NAME", this.ItemName);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
			}
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0005C126 File Offset: 0x0005A326
		public void ExecuteRandomCraftName()
		{
			this.ItemName = this._crafting.GetRandomCraftName().ToString();
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x0005C13E File Offset: 0x0005A33E
		// (set) Token: 0x060017FB RID: 6139 RVA: 0x0005C146 File Offset: 0x0005A346
		[DataSourceProperty]
		public MBBindingList<ItemFlagVM> WeaponFlagIconsList
		{
			get
			{
				return this._weaponFlagIconsList;
			}
			set
			{
				if (value != this._weaponFlagIconsList)
				{
					this._weaponFlagIconsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemFlagVM>>(value, "WeaponFlagIconsList");
				}
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x0005C164 File Offset: 0x0005A364
		// (set) Token: 0x060017FD RID: 6141 RVA: 0x0005C16C File Offset: 0x0005A36C
		[DataSourceProperty]
		public bool IsInOrderMode
		{
			get
			{
				return this._isInOrderMode;
			}
			set
			{
				if (value != this._isInOrderMode)
				{
					this._isInOrderMode = value;
					base.OnPropertyChangedWithValue(value, "IsInOrderMode");
				}
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x0005C18A File Offset: 0x0005A38A
		// (set) Token: 0x060017FF RID: 6143 RVA: 0x0005C192 File Offset: 0x0005A392
		[DataSourceProperty]
		public int CraftedWeaponFinalWorth
		{
			get
			{
				return this._craftedWeaponFinalWorth;
			}
			set
			{
				if (value != this._craftedWeaponFinalWorth)
				{
					this._craftedWeaponFinalWorth = value;
					base.OnPropertyChangedWithValue(value, "CraftedWeaponFinalWorth");
				}
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x0005C1B0 File Offset: 0x0005A3B0
		// (set) Token: 0x06001801 RID: 6145 RVA: 0x0005C1B8 File Offset: 0x0005A3B8
		[DataSourceProperty]
		public int CraftedWeaponPriceDifference
		{
			get
			{
				return this._craftedWeaponPriceDifference;
			}
			set
			{
				if (value != this._craftedWeaponPriceDifference)
				{
					this._craftedWeaponPriceDifference = value;
					base.OnPropertyChangedWithValue(value, "CraftedWeaponPriceDifference");
				}
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x0005C1D6 File Offset: 0x0005A3D6
		// (set) Token: 0x06001803 RID: 6147 RVA: 0x0005C1DE File Offset: 0x0005A3DE
		[DataSourceProperty]
		public int CraftedWeaponInitialWorth
		{
			get
			{
				return this._craftedWeaponInitialWorth;
			}
			set
			{
				if (value != this._craftedWeaponInitialWorth)
				{
					this._craftedWeaponInitialWorth = value;
					base.OnPropertyChangedWithValue(value, "CraftedWeaponInitialWorth");
				}
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x0005C1FC File Offset: 0x0005A3FC
		// (set) Token: 0x06001805 RID: 6149 RVA: 0x0005C204 File Offset: 0x0005A404
		[DataSourceProperty]
		public string CraftedWeaponWorthText
		{
			get
			{
				return this._craftedWeaponWorthText;
			}
			set
			{
				if (value != this._craftedWeaponWorthText)
				{
					this._craftedWeaponWorthText = value;
					base.OnPropertyChangedWithValue<string>(value, "CraftedWeaponWorthText");
				}
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06001806 RID: 6150 RVA: 0x0005C227 File Offset: 0x0005A427
		// (set) Token: 0x06001807 RID: 6151 RVA: 0x0005C22F File Offset: 0x0005A42F
		[DataSourceProperty]
		public bool IsOrderSuccessful
		{
			get
			{
				return this._isOrderSuccessful;
			}
			set
			{
				if (value != this._isOrderSuccessful)
				{
					this._isOrderSuccessful = value;
					base.OnPropertyChangedWithValue(value, "IsOrderSuccessful");
				}
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x0005C24D File Offset: 0x0005A44D
		// (set) Token: 0x06001809 RID: 6153 RVA: 0x0005C255 File Offset: 0x0005A455
		[DataSourceProperty]
		public bool CanConfirm
		{
			get
			{
				return this._canConfirm;
			}
			set
			{
				if (value != this._canConfirm)
				{
					this._canConfirm = value;
					base.OnPropertyChangedWithValue(value, "CanConfirm");
				}
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x0005C273 File Offset: 0x0005A473
		// (set) Token: 0x0600180B RID: 6155 RVA: 0x0005C27B File Offset: 0x0005A47B
		[DataSourceProperty]
		public string OrderResultText
		{
			get
			{
				return this._orderResultText;
			}
			set
			{
				if (value != this._orderResultText)
				{
					this._orderResultText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderResultText");
				}
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x0005C29E File Offset: 0x0005A49E
		// (set) Token: 0x0600180D RID: 6157 RVA: 0x0005C2A6 File Offset: 0x0005A4A6
		[DataSourceProperty]
		public string OrderOwnerRemarkText
		{
			get
			{
				return this._orderOwnerRemarkText;
			}
			set
			{
				if (value != this._orderOwnerRemarkText)
				{
					this._orderOwnerRemarkText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderOwnerRemarkText");
				}
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x0005C2C9 File Offset: 0x0005A4C9
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x0005C2D1 File Offset: 0x0005A4D1
		[DataSourceProperty]
		public string WeaponCraftedText
		{
			get
			{
				return this._weaponCraftedText;
			}
			set
			{
				if (value != this._weaponCraftedText)
				{
					this._weaponCraftedText = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponCraftedText");
				}
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x0005C2F4 File Offset: 0x0005A4F4
		// (set) Token: 0x06001811 RID: 6161 RVA: 0x0005C2FC File Offset: 0x0005A4FC
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x0005C31F File Offset: 0x0005A51F
		// (set) Token: 0x06001813 RID: 6163 RVA: 0x0005C327 File Offset: 0x0005A527
		[DataSourceProperty]
		public MBBindingList<WeaponDesignResultPropertyItemVM> DesignResultPropertyList
		{
			get
			{
				return this._designResultPropertyList;
			}
			set
			{
				if (value != this._designResultPropertyList)
				{
					this._designResultPropertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponDesignResultPropertyItemVM>>(value, "DesignResultPropertyList");
				}
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0005C345 File Offset: 0x0005A545
		// (set) Token: 0x06001815 RID: 6165 RVA: 0x0005C34D File Offset: 0x0005A54D
		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					this.UpdateConfirmAvailability();
					base.OnPropertyChangedWithValue<string>(value, "ItemName");
				}
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0005C376 File Offset: 0x0005A576
		// (set) Token: 0x06001817 RID: 6167 RVA: 0x0005C37E File Offset: 0x0005A57E
		[DataSourceProperty]
		public ItemCollectionElementViewModel ItemVisualModel
		{
			get
			{
				return this._itemVisualModel;
			}
			set
			{
				if (value != this._itemVisualModel)
				{
					this._itemVisualModel = value;
					base.OnPropertyChangedWithValue<ItemCollectionElementViewModel>(value, "ItemVisualModel");
				}
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x0005C39C File Offset: 0x0005A59C
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x0005C3A4 File Offset: 0x0005A5A4
		[DataSourceProperty]
		public HintViewModel ConfirmDisabledReasonHint
		{
			get
			{
				return this._confirmDisabledReasonHint;
			}
			set
			{
				if (value != this._confirmDisabledReasonHint)
				{
					this._confirmDisabledReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ConfirmDisabledReasonHint");
				}
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x0005C3C2 File Offset: 0x0005A5C2
		// (set) Token: 0x0600181B RID: 6171 RVA: 0x0005C3CA File Offset: 0x0005A5CA
		[DataSourceProperty]
		public SelectorVM<CraftingSecondaryUsageItemVM> SecondaryUsageSelector
		{
			get
			{
				return this._secondaryUsageSelector;
			}
			set
			{
				if (value != this._secondaryUsageSelector)
				{
					this._secondaryUsageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<CraftingSecondaryUsageItemVM>>(value, "SecondaryUsageSelector");
				}
			}
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0005C3E8 File Offset: 0x0005A5E8
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x0005C3F7 File Offset: 0x0005A5F7
		// (set) Token: 0x0600181E RID: 6174 RVA: 0x0005C3FF File Offset: 0x0005A5FF
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x04000AEB RID: 2795
		private readonly Action<CraftingSecondaryUsageItemVM> _onUsageSelected;

		// Token: 0x04000AEC RID: 2796
		private readonly Func<CraftingSecondaryUsageItemVM, MBBindingList<WeaponDesignResultPropertyItemVM>> _onGetPropertyList;

		// Token: 0x04000AED RID: 2797
		private readonly Action _onFinalize;

		// Token: 0x04000AEE RID: 2798
		private readonly Crafting _crafting;

		// Token: 0x04000AEF RID: 2799
		private readonly CraftingOrder _completedOrder;

		// Token: 0x04000AF0 RID: 2800
		private readonly ItemObject _craftedItem;

		// Token: 0x04000AF1 RID: 2801
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000AF2 RID: 2802
		private MBBindingList<ItemFlagVM> _weaponFlagIconsList;

		// Token: 0x04000AF3 RID: 2803
		private bool _isInOrderMode;

		// Token: 0x04000AF4 RID: 2804
		private string _orderResultText;

		// Token: 0x04000AF5 RID: 2805
		private string _orderOwnerRemarkText;

		// Token: 0x04000AF6 RID: 2806
		private bool _isOrderSuccessful;

		// Token: 0x04000AF7 RID: 2807
		private bool _canConfirm;

		// Token: 0x04000AF8 RID: 2808
		private string _craftedWeaponWorthText;

		// Token: 0x04000AF9 RID: 2809
		private int _craftedWeaponInitialWorth;

		// Token: 0x04000AFA RID: 2810
		private int _craftedWeaponPriceDifference;

		// Token: 0x04000AFB RID: 2811
		private int _craftedWeaponFinalWorth;

		// Token: 0x04000AFC RID: 2812
		private string _weaponCraftedText;

		// Token: 0x04000AFD RID: 2813
		private string _doneLbl;

		// Token: 0x04000AFE RID: 2814
		private MBBindingList<WeaponDesignResultPropertyItemVM> _designResultPropertyList;

		// Token: 0x04000AFF RID: 2815
		private string _itemName;

		// Token: 0x04000B00 RID: 2816
		private ItemCollectionElementViewModel _itemVisualModel;

		// Token: 0x04000B01 RID: 2817
		private HintViewModel _confirmDisabledReasonHint;

		// Token: 0x04000B02 RID: 2818
		private SelectorVM<CraftingSecondaryUsageItemVM> _secondaryUsageSelector;

		// Token: 0x04000B03 RID: 2819
		private InputKeyItemVM _doneInputKey;
	}
}
