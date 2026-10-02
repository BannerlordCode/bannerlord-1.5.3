using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000056 RID: 86
	public class MissionMainAgentEquipmentControllerVM : ViewModel
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x00019210 File Offset: 0x00017410
		public MissionMainAgentEquipmentControllerVM(Action<EquipmentIndex> onDropEquipment, Action<SpawnedItemEntity, EquipmentIndex> onEquipItem)
		{
			this._onDropEquipment = onDropEquipment;
			this._onEquipItem = onEquipItem;
			this.DropActions = new MBBindingList<EquipmentActionItemVM>();
			this.EquipActions = new MBBindingList<EquipmentActionItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001925E File Offset: 0x0001745E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._dropLocalizedText = GameTexts.FindText("str_inventory_drop", null);
			this._replaceWithLocalizedText = GameTexts.FindText("str_replace_with", null);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00019288 File Offset: 0x00017488
		public void OnDropControllerToggle(bool isActive)
		{
			this.SelectedItemText = "";
			if (isActive && Agent.Main != null)
			{
				this.DropActions.Clear();
				this.DropActions.Add(new EquipmentActionItemVM(GameTexts.FindText("str_cancel", null).ToString(), "None", null, new Action<EquipmentActionItemVM>(this.OnItemSelected), false));
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
					if (!missionWeapon.IsEmpty)
					{
						string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon.Item);
						bool flag = this.IsWieldedWeaponAtIndex(equipmentIndex);
						string weaponName = this.GetWeaponName(missionWeapon);
						this.DropActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag));
					}
				}
			}
			else
			{
				EquipmentActionItemVM equipmentActionItemVM = this.DropActions.SingleOrDefault<EquipmentActionItemVM>((EquipmentActionItemVM a) => a.IsSelected);
				if (equipmentActionItemVM != null)
				{
					this.HandleDropItemActionSelection(equipmentActionItemVM.Identifier);
				}
			}
			this.IsDropControllerActive = isActive;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000193A0 File Offset: 0x000175A0
		private void HandleDropItemActionSelection(object selectedItem)
		{
			if (selectedItem is EquipmentIndex)
			{
				EquipmentIndex equipmentIndex = (EquipmentIndex)selectedItem;
				this._onDropEquipment(equipmentIndex);
				return;
			}
			if (selectedItem != null)
			{
				Debug.FailedAssert("Unidentified action on drop wheel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentEquipmentControllerVM.cs", "HandleDropItemActionSelection", 106);
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000193E4 File Offset: 0x000175E4
		public void SetCurrentFocusedWeaponEntity(SpawnedItemEntity weaponEntity)
		{
			this._focusedWeaponEntity = weaponEntity;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000193F0 File Offset: 0x000175F0
		public void OnEquipControllerToggle(bool isActive)
		{
			this.SelectedItemText = "";
			this.FocusedItemText = "";
			if (isActive && Agent.Main != null)
			{
				this.EquipActions.Clear();
				this.EquipActions.Add(new EquipmentActionItemVM(GameTexts.FindText("str_cancel", null).ToString(), "None", null, new Action<EquipmentActionItemVM>(this.OnItemSelected), false));
				if (this._focusedWeaponEntity.WeaponCopy.Item.Type == ItemObject.ItemTypeEnum.Shield && this.DoesPlayerHaveAtLeastOneShield())
				{
					this._pickText.SetTextVariable("ITEM_NAME", this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString());
					this.FocusedItemText = this._pickText.ToString();
					for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
					{
						MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
						if (!missionWeapon.IsEmpty && missionWeapon.Item.Type == ItemObject.ItemTypeEnum.Shield)
						{
							string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon.Item);
							bool flag = this.IsWieldedWeaponAtIndex(equipmentIndex);
							string weaponName = this.GetWeaponName(missionWeapon);
							this.EquipActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag));
						}
					}
				}
				else
				{
					Agent main = Agent.Main;
					if (main != null && main.CanInteractableWeaponBePickedUp(this._focusedWeaponEntity))
					{
						this._pickText.SetTextVariable("ITEM_NAME", this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString());
						this.FocusedItemText = this._pickText.ToString();
						bool flag2 = Agent.Main.WillDropWieldedShield(this._focusedWeaponEntity);
						for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
						{
							MissionWeapon missionWeapon2 = Mission.Current.MainAgent.Equipment[equipmentIndex2];
							if (!missionWeapon2.IsEmpty && (!flag2 || missionWeapon2.IsShield()))
							{
								string itemTypeAsString2 = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon2.Item);
								bool flag3 = this.IsWieldedWeaponAtIndex(equipmentIndex2);
								string weaponName2 = this.GetWeaponName(missionWeapon2);
								this.EquipActions.Add(new EquipmentActionItemVM(weaponName2, itemTypeAsString2, equipmentIndex2, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag3));
							}
						}
					}
					else
					{
						this.FocusedItemText = this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString();
						EquipmentActionItemVM equipmentActionItemVM = new EquipmentActionItemVM(GameTexts.FindText("str_pickup_to_equip", null).ToString(), "PickUp", this._focusedWeaponEntity, new Action<EquipmentActionItemVM>(this.OnItemSelected), false)
						{
							IsSelected = true
						};
						this.EquipActions.Add(equipmentActionItemVM);
					}
				}
				EquipmentIndex itemIndexThatQuickPickUpWouldReplace = MissionEquipment.SelectWeaponPickUpSlot(Agent.Main, this._focusedWeaponEntity.WeaponCopy, this._focusedWeaponEntity.IsStuckMissile());
				EquipmentActionItemVM equipmentActionItemVM2 = this.EquipActions.SingleOrDefault<EquipmentActionItemVM>(delegate(EquipmentActionItemVM a)
				{
					object identifier;
					if ((identifier = a.Identifier) is EquipmentIndex)
					{
						EquipmentIndex equipmentIndex3 = (EquipmentIndex)identifier;
						return equipmentIndex3 == itemIndexThatQuickPickUpWouldReplace;
					}
					return false;
				});
				if (equipmentActionItemVM2 != null)
				{
					equipmentActionItemVM2.IsSelected = true;
				}
			}
			else
			{
				EquipmentActionItemVM equipmentActionItemVM3 = this.EquipActions.SingleOrDefault<EquipmentActionItemVM>((EquipmentActionItemVM a) => a.IsSelected);
				if (equipmentActionItemVM3 != null)
				{
					this.HandleEquipItemActionSelection(equipmentActionItemVM3.Identifier);
				}
			}
			this.IsEquipControllerActive = isActive;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00019742 File Offset: 0x00017942
		public void OnCancelEquipController()
		{
			this.IsEquipControllerActive = false;
			this.EquipActions.Clear();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00019756 File Offset: 0x00017956
		public void OnCancelDropController()
		{
			this.IsDropControllerActive = false;
			this.DropActions.Clear();
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001976C File Offset: 0x0001796C
		private void HandleEquipItemActionSelection(object selectedItem)
		{
			if (selectedItem is EquipmentIndex)
			{
				EquipmentIndex equipmentIndex = (EquipmentIndex)selectedItem;
				if (this._focusedWeaponEntity != null)
				{
					this._onEquipItem(this._focusedWeaponEntity, equipmentIndex);
					return;
				}
			}
			SpawnedItemEntity spawnedItemEntity;
			if ((spawnedItemEntity = selectedItem as SpawnedItemEntity) != null)
			{
				this._onEquipItem(spawnedItemEntity, EquipmentIndex.None);
				return;
			}
			if (selectedItem != null)
			{
				Debug.FailedAssert("Unidentified action on drop wheel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentEquipmentControllerVM.cs", "HandleEquipItemActionSelection", 223);
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000197DC File Offset: 0x000179DC
		private void OnItemSelected(EquipmentActionItemVM item)
		{
			if (this.IsEquipControllerActive)
			{
				if (item.Identifier == null || item.Identifier is SpawnedItemEntity)
				{
					this.EquipText = "";
				}
				else
				{
					this.EquipText = this._replaceWithLocalizedText.ToString();
				}
			}
			else if (item.Identifier == null)
			{
				this.DropText = "";
			}
			else
			{
				this.DropText = this._dropLocalizedText.ToString();
			}
			this.SelectedItemText = item.ActionText;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00019858 File Offset: 0x00017A58
		private string GetWeaponName(MissionWeapon weapon)
		{
			string text = weapon.Item.Name.ToString();
			WeaponComponentData currentUsageItem = weapon.CurrentUsageItem;
			if (currentUsageItem != null && currentUsageItem.IsShield)
			{
				text = string.Concat(new object[] { text, " (", weapon.HitPoints, " / ", weapon.ModifiedMaxHitPoints, ")" });
			}
			else
			{
				WeaponComponentData currentUsageItem2 = weapon.CurrentUsageItem;
				if (currentUsageItem2 != null && currentUsageItem2.IsConsumable && weapon.ModifiedMaxAmount > 1)
				{
					text = string.Concat(new object[] { text, " (", weapon.Amount, " / ", weapon.ModifiedMaxAmount, ")" });
				}
			}
			return text;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001993A File Offset: 0x00017B3A
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x00019942 File Offset: 0x00017B42
		[DataSourceProperty]
		public bool IsDropControllerActive
		{
			get
			{
				return this._isDropControllerActive;
			}
			set
			{
				if (value != this._isDropControllerActive)
				{
					this._isDropControllerActive = value;
					base.OnPropertyChangedWithValue(value, "IsDropControllerActive");
				}
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00019960 File Offset: 0x00017B60
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x00019968 File Offset: 0x00017B68
		[DataSourceProperty]
		public bool IsEquipControllerActive
		{
			get
			{
				return this._isEquipControllerActive;
			}
			set
			{
				if (value != this._isEquipControllerActive)
				{
					this._isEquipControllerActive = value;
					base.OnPropertyChangedWithValue(value, "IsEquipControllerActive");
				}
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00019986 File Offset: 0x00017B86
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0001998E File Offset: 0x00017B8E
		[DataSourceProperty]
		public string DropText
		{
			get
			{
				return this._dropText;
			}
			set
			{
				if (value != this._dropText)
				{
					this._dropText = value;
					base.OnPropertyChangedWithValue<string>(value, "DropText");
				}
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000199B1 File Offset: 0x00017BB1
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x000199B9 File Offset: 0x00017BB9
		[DataSourceProperty]
		public string EquipText
		{
			get
			{
				return this._equipText;
			}
			set
			{
				if (value != this._equipText)
				{
					this._equipText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipText");
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x000199DC File Offset: 0x00017BDC
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x000199E4 File Offset: 0x00017BE4
		[DataSourceProperty]
		public string FocusedItemText
		{
			get
			{
				return this._focusedItemText;
			}
			set
			{
				if (value != this._focusedItemText)
				{
					this._focusedItemText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusedItemText");
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00019A07 File Offset: 0x00017C07
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x00019A0F File Offset: 0x00017C0F
		[DataSourceProperty]
		public string SelectedItemText
		{
			get
			{
				return this._selectedItemText;
			}
			set
			{
				if (value != this._selectedItemText)
				{
					this._selectedItemText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedItemText");
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00019A32 File Offset: 0x00017C32
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00019A3A File Offset: 0x00017C3A
		[DataSourceProperty]
		public MBBindingList<EquipmentActionItemVM> DropActions
		{
			get
			{
				return this._dropActions;
			}
			set
			{
				if (value != this._dropActions)
				{
					this._dropActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<EquipmentActionItemVM>>(value, "DropActions");
				}
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00019A58 File Offset: 0x00017C58
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00019A60 File Offset: 0x00017C60
		[DataSourceProperty]
		public MBBindingList<EquipmentActionItemVM> EquipActions
		{
			get
			{
				return this._equipActions;
			}
			set
			{
				if (value != this._equipActions)
				{
					this._equipActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<EquipmentActionItemVM>>(value, "EquipActions");
				}
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00019A80 File Offset: 0x00017C80
		public static string GetItemTypeAsString(ItemObject item)
		{
			if (item.ItemComponent is WeaponComponent)
			{
				switch ((item.ItemComponent as WeaponComponent).PrimaryWeapon.WeaponClass)
				{
				case WeaponClass.Dagger:
				case WeaponClass.OneHandedSword:
				case WeaponClass.TwoHandedSword:
					return "Sword";
				case WeaponClass.OneHandedAxe:
				case WeaponClass.TwoHandedAxe:
					return "Axe";
				case WeaponClass.Mace:
				case WeaponClass.TwoHandedMace:
					return "Mace";
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					return "Spear";
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
				case WeaponClass.SlingStone:
				case WeaponClass.Cartridge:
				case WeaponClass.Musket:
					return "Ammo";
				case WeaponClass.Bow:
					return "Bow";
				case WeaponClass.Crossbow:
					return "Crossbow";
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					return "Stone";
				case WeaponClass.ThrowingAxe:
					return "ThrowingAxe";
				case WeaponClass.ThrowingKnife:
					return "ThrowingKnife";
				case WeaponClass.Javelin:
					return "Javelin";
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					return "Shield";
				case WeaponClass.Banner:
					return "Banner";
				}
				return "None";
			}
			if (item.ItemComponent is HorseComponent)
			{
				return "Mount";
			}
			return "None";
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00019BA4 File Offset: 0x00017DA4
		private bool DoesPlayerHaveAtLeastOneShield()
		{
			EquipmentIndex offhandWieldedItemIndex = Agent.Main.GetOffhandWieldedItemIndex();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (equipmentIndex != offhandWieldedItemIndex && !Agent.Main.Equipment[equipmentIndex].IsEmpty && Mission.Current.MainAgent.Equipment[equipmentIndex].Item.Type == ItemObject.ItemTypeEnum.Shield)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00019C0E File Offset: 0x00017E0E
		private bool IsWieldedWeaponAtIndex(EquipmentIndex index)
		{
			return index == Agent.Main.GetPrimaryWieldedItemIndex() || index == Agent.Main.GetOffhandWieldedItemIndex();
		}

		// Token: 0x04000318 RID: 792
		private TextObject _replaceWithLocalizedText;

		// Token: 0x04000319 RID: 793
		private TextObject _dropLocalizedText;

		// Token: 0x0400031A RID: 794
		private SpawnedItemEntity _focusedWeaponEntity;

		// Token: 0x0400031B RID: 795
		private readonly Action<EquipmentIndex> _onDropEquipment;

		// Token: 0x0400031C RID: 796
		private readonly Action<SpawnedItemEntity, EquipmentIndex> _onEquipItem;

		// Token: 0x0400031D RID: 797
		private readonly TextObject _pickText = new TextObject("{=d5SNB0HV}Pick {ITEM_NAME}", null);

		// Token: 0x0400031E RID: 798
		private bool _isDropControllerActive;

		// Token: 0x0400031F RID: 799
		private bool _isEquipControllerActive;

		// Token: 0x04000320 RID: 800
		private string _selectedItemText;

		// Token: 0x04000321 RID: 801
		private string _dropText;

		// Token: 0x04000322 RID: 802
		private string _equipText;

		// Token: 0x04000323 RID: 803
		private string _focusedItemText;

		// Token: 0x04000324 RID: 804
		private MBBindingList<EquipmentActionItemVM> _dropActions;

		// Token: 0x04000325 RID: 805
		private MBBindingList<EquipmentActionItemVM> _equipActions;

		// Token: 0x020000E9 RID: 233
		public enum ItemGroup
		{
			// Token: 0x0400066D RID: 1645
			None,
			// Token: 0x0400066E RID: 1646
			Spear,
			// Token: 0x0400066F RID: 1647
			Javelin,
			// Token: 0x04000670 RID: 1648
			Bow,
			// Token: 0x04000671 RID: 1649
			Crossbow,
			// Token: 0x04000672 RID: 1650
			Sword,
			// Token: 0x04000673 RID: 1651
			Axe,
			// Token: 0x04000674 RID: 1652
			Mace,
			// Token: 0x04000675 RID: 1653
			ThrowingAxe,
			// Token: 0x04000676 RID: 1654
			ThrowingKnife,
			// Token: 0x04000677 RID: 1655
			Ammo,
			// Token: 0x04000678 RID: 1656
			Shield,
			// Token: 0x04000679 RID: 1657
			Mount,
			// Token: 0x0400067A RID: 1658
			Banner,
			// Token: 0x0400067B RID: 1659
			Stone
		}
	}
}
