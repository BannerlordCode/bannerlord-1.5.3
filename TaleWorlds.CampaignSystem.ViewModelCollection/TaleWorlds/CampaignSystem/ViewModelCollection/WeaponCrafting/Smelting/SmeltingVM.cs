using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000119 RID: 281
	public class SmeltingVM : ViewModel
	{
		// Token: 0x06001970 RID: 6512 RVA: 0x00060E20 File Offset: 0x0005F020
		public SmeltingVM(Action updateValuesOnSelectItemAction, Action updateValuesOnSmeltItemAction)
		{
			this.SortController = new SmeltingSortControllerVM();
			this._updateValuesOnSelectItemAction = updateValuesOnSelectItemAction;
			this._updateValuesOnSmeltItemAction = updateValuesOnSmeltItemAction;
			this._playerItemRoster = MobileParty.MainParty.ItemRoster;
			this._smithingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			IViewDataTracker campaignBehavior = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._lockedItemIDs = campaignBehavior.GetInventoryLocks().ToList<string>();
			this.RefreshList();
			this.RefreshValues();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00060E94 File Offset: 0x0005F094
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SelectAllHint = new HintViewModel(new TextObject("{=k1E9DuKi}Select All", null), null);
			SmeltingItemVM currentSelectedItem = this.CurrentSelectedItem;
			if (currentSelectedItem != null)
			{
				currentSelectedItem.RefreshValues();
			}
			this.SmeltableItemList.ApplyActionOnAllItems(delegate(SmeltingItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00060F04 File Offset: 0x0005F104
		internal void OnCraftingHeroChanged(CraftingAvailableHeroItemVM newHero)
		{
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00060F08 File Offset: 0x0005F108
		public void RefreshList()
		{
			this.SmeltableItemList = new MBBindingList<SmeltingItemVM>();
			this.SortController.SetListToControl(this.SmeltableItemList);
			for (int i = 0; i < this._playerItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = this._playerItemRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.EquipmentElement.Item.IsCraftedWeapon)
				{
					bool flag = this.IsItemLocked(elementCopyAtIndex.EquipmentElement);
					SmeltingItemVM smeltingItemVM = new SmeltingItemVM(elementCopyAtIndex.EquipmentElement, new Action<SmeltingItemVM>(this.OnItemSelection), new Action<SmeltingItemVM, bool>(this.ProcessLockItem), flag, elementCopyAtIndex.Amount);
					string id = smeltingItemVM.Visual.Id;
					SmeltingItemVM currentSelectedItem = this.CurrentSelectedItem;
					string text;
					if (currentSelectedItem == null)
					{
						text = null;
					}
					else
					{
						ItemImageIdentifierVM visual = currentSelectedItem.Visual;
						text = ((visual != null) ? visual.Id : null);
					}
					if (id == text)
					{
						this.OnItemSelection(smeltingItemVM);
					}
					this.SmeltableItemList.Add(smeltingItemVM);
				}
			}
			if (this.SmeltableItemList.Count == 0)
			{
				this.CurrentSelectedItem = null;
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0006100C File Offset: 0x0005F20C
		private void OnItemSelection(SmeltingItemVM newItem)
		{
			if (newItem != this.CurrentSelectedItem)
			{
				if (this.CurrentSelectedItem != null)
				{
					this.CurrentSelectedItem.IsSelected = false;
				}
				this.CurrentSelectedItem = newItem;
				this.CurrentSelectedItem.IsSelected = true;
			}
			this._updateValuesOnSelectItemAction();
			WeaponDesign weaponDesign = this.CurrentSelectedItem.EquipmentElement.Item.WeaponDesign;
			this.WeaponTypeName = ((weaponDesign != null) ? weaponDesign.Template.TemplateName.ToString() : null) ?? string.Empty;
			WeaponDesign weaponDesign2 = this.CurrentSelectedItem.EquipmentElement.Item.WeaponDesign;
			this.WeaponTypeCode = ((weaponDesign2 != null) ? weaponDesign2.Template.StringId : null) ?? string.Empty;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000610CC File Offset: 0x0005F2CC
		public void TrySmeltingSelectedItems(Hero currentCraftingHero)
		{
			if (this._currentSelectedItem != null)
			{
				if (this._currentSelectedItem.IsLocked)
				{
					string text = new TextObject("{=wMiLUTNY}Are you sure you want to smelt this weapon? It is locked in the inventory.", null).ToString();
					InformationManager.ShowInquiry(new InquiryData("", text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						this.SmeltSelectedItems(currentCraftingHero);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				this.SmeltSelectedItems(currentCraftingHero);
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00061170 File Offset: 0x0005F370
		private void ProcessLockItem(SmeltingItemVM item, bool isLocked)
		{
			if (item == null)
			{
				return;
			}
			string itemLockStringID = CampaignUIHelper.GetItemLockStringID(item.EquipmentElement);
			if (isLocked && !this._lockedItemIDs.Contains(itemLockStringID))
			{
				this._lockedItemIDs.Add(itemLockStringID);
				return;
			}
			if (!isLocked && this._lockedItemIDs.Contains(itemLockStringID))
			{
				this._lockedItemIDs.Remove(itemLockStringID);
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x000611CC File Offset: 0x0005F3CC
		private void SmeltSelectedItems(Hero currentCraftingHero)
		{
			if (this._currentSelectedItem != null && this._smithingBehavior != null)
			{
				ICraftingCampaignBehavior smithingBehavior = this._smithingBehavior;
				if (smithingBehavior != null)
				{
					smithingBehavior.DoSmelting(currentCraftingHero, this._currentSelectedItem.EquipmentElement);
				}
			}
			this.RefreshList();
			this.SortController.SortByCurrentState();
			if (this.CurrentSelectedItem != null)
			{
				int num = this.SmeltableItemList.FindIndex<SmeltingItemVM>((SmeltingItemVM i) => i.EquipmentElement.Item == this.CurrentSelectedItem.EquipmentElement.Item);
				SmeltingItemVM smeltingItemVM = ((num != -1) ? this.SmeltableItemList[num] : this.SmeltableItemList.FirstOrDefault<SmeltingItemVM>());
				this.OnItemSelection(smeltingItemVM);
			}
			this._updateValuesOnSmeltItemAction();
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00061268 File Offset: 0x0005F468
		private bool IsItemLocked(EquipmentElement equipmentElement)
		{
			string itemLockStringID = CampaignUIHelper.GetItemLockStringID(equipmentElement);
			return this._lockedItemIDs.Contains(itemLockStringID);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00061288 File Offset: 0x0005F488
		public void SaveItemLockStates()
		{
			Campaign.Current.GetCampaignBehavior<IViewDataTracker>().SetInventoryLocks(this._lockedItemIDs);
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x0006129F File Offset: 0x0005F49F
		// (set) Token: 0x0600197B RID: 6523 RVA: 0x000612A7 File Offset: 0x0005F4A7
		[DataSourceProperty]
		public string WeaponTypeName
		{
			get
			{
				return this._weaponTypeName;
			}
			set
			{
				if (value != this._weaponTypeName)
				{
					this._weaponTypeName = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeName");
				}
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x000612CA File Offset: 0x0005F4CA
		// (set) Token: 0x0600197D RID: 6525 RVA: 0x000612D2 File Offset: 0x0005F4D2
		[DataSourceProperty]
		public string WeaponTypeCode
		{
			get
			{
				return this._weaponTypeCode;
			}
			set
			{
				if (value != this._weaponTypeCode)
				{
					this._weaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeCode");
				}
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x000612F5 File Offset: 0x0005F4F5
		// (set) Token: 0x0600197F RID: 6527 RVA: 0x000612FD File Offset: 0x0005F4FD
		[DataSourceProperty]
		public SmeltingItemVM CurrentSelectedItem
		{
			get
			{
				return this._currentSelectedItem;
			}
			set
			{
				if (value != this._currentSelectedItem)
				{
					this._currentSelectedItem = value;
					base.OnPropertyChangedWithValue<SmeltingItemVM>(value, "CurrentSelectedItem");
					this.IsAnyItemSelected = value != null;
				}
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x00061325 File Offset: 0x0005F525
		// (set) Token: 0x06001981 RID: 6529 RVA: 0x0006132D File Offset: 0x0005F52D
		[DataSourceProperty]
		public bool IsAnyItemSelected
		{
			get
			{
				return this._isAnyItemSelected;
			}
			set
			{
				if (value != this._isAnyItemSelected)
				{
					this._isAnyItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyItemSelected");
				}
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0006134B File Offset: 0x0005F54B
		// (set) Token: 0x06001983 RID: 6531 RVA: 0x00061353 File Offset: 0x0005F553
		[DataSourceProperty]
		public MBBindingList<SmeltingItemVM> SmeltableItemList
		{
			get
			{
				return this._smeltableItemList;
			}
			set
			{
				if (value != this._smeltableItemList)
				{
					this._smeltableItemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SmeltingItemVM>>(value, "SmeltableItemList");
				}
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x00061371 File Offset: 0x0005F571
		// (set) Token: 0x06001985 RID: 6533 RVA: 0x00061379 File Offset: 0x0005F579
		[DataSourceProperty]
		public HintViewModel SelectAllHint
		{
			get
			{
				return this._selectAllHint;
			}
			set
			{
				if (value != this._selectAllHint)
				{
					this._selectAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SelectAllHint");
				}
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x00061397 File Offset: 0x0005F597
		// (set) Token: 0x06001987 RID: 6535 RVA: 0x0006139F File Offset: 0x0005F59F
		[DataSourceProperty]
		public SmeltingSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<SmeltingSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000BA5 RID: 2981
		private ItemRoster _playerItemRoster;

		// Token: 0x04000BA6 RID: 2982
		private Action _updateValuesOnSelectItemAction;

		// Token: 0x04000BA7 RID: 2983
		private Action _updateValuesOnSmeltItemAction;

		// Token: 0x04000BA8 RID: 2984
		private List<string> _lockedItemIDs;

		// Token: 0x04000BA9 RID: 2985
		private readonly ICraftingCampaignBehavior _smithingBehavior;

		// Token: 0x04000BAA RID: 2986
		private string _weaponTypeName;

		// Token: 0x04000BAB RID: 2987
		private string _weaponTypeCode;

		// Token: 0x04000BAC RID: 2988
		private SmeltingItemVM _currentSelectedItem;

		// Token: 0x04000BAD RID: 2989
		private MBBindingList<SmeltingItemVM> _smeltableItemList;

		// Token: 0x04000BAE RID: 2990
		private SmeltingSortControllerVM _sortController;

		// Token: 0x04000BAF RID: 2991
		private HintViewModel _selectAllHint;

		// Token: 0x04000BB0 RID: 2992
		private bool _isAnyItemSelected;
	}
}
