using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000054 RID: 84
	public class MissionMainAgentControllerEquipDropVM : ViewModel
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x00018984 File Offset: 0x00016B84
		public MissionMainAgentControllerEquipDropVM(Action<EquipmentIndex> toggleItem)
		{
			this._toggleItem = toggleItem;
			this.EquippedWeapons = new MBBindingList<ControllerEquippedItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000189B5 File Offset: 0x00016BB5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PressToEquipText = new TextObject("{=HEEZhL90}Press to Equip", null).ToString();
			this.HoldToDropText = this._dropTextObject.ToString();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000189E4 File Offset: 0x00016BE4
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive() && !Agent.Main.IsUsingGameObject && !Agent.Main.IsInWater();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00018A14 File Offset: 0x00016C14
		public void InitializeMainAgentPropterties()
		{
			Mission.Current.OnMainAgentChanged += this.OnMainAgentChanged;
			this.OnMainAgentChanged(null);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00018A34 File Offset: 0x00016C34
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (oldAgent != null)
			{
				oldAgent.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(oldAgent.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
			}
			if (Agent.Main != null)
			{
				Agent main = Agent.Main;
				main.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Combine(main.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00018A93 File Offset: 0x00016C93
		private void OnMainAgentWeaponChange()
		{
			this.UpdateItemsWieldStatus();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00018A9C File Offset: 0x00016C9C
		public void OnToggle(bool isEnabled)
		{
			this.EquippedWeapons.ApplyActionOnAllItems(delegate(ControllerEquippedItemVM o)
			{
				o.OnFinalize();
			});
			this.EquippedWeapons.Clear();
			this.EquippedExtraWeapon = null;
			this.HaveExtraWeapon = false;
			if (isEnabled)
			{
				this.PressToEquipText = (this.IsMainAgentAvailable() ? new TextObject("{=HEEZhL90}Press to Equip", null).ToString() : string.Empty);
				this.EquippedWeapons.Add(new ControllerEquippedItemVM(GameTexts.FindText("str_cancel", null).ToString(), null, "None", null, new Action<EquipmentActionItemVM>(this.OnItemSelected)));
				int num = 0;
				int totalNumberOfWeaponsOnMainAgent = this.GetTotalNumberOfWeaponsOnMainAgent();
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
				{
					MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
					if (!missionWeapon.IsEmpty)
					{
						string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon.Item);
						string weaponName = this.GetWeaponName(missionWeapon);
						this.EquippedWeapons.Add(new ControllerEquippedItemVM(weaponName, itemTypeAsString, equipmentIndex, MissionMainAgentControllerEquipDropVM.GetWeaponHotKey(num, totalNumberOfWeaponsOnMainAgent), new Action<EquipmentActionItemVM>(this.OnItemSelected)));
						num++;
					}
				}
				MissionWeapon missionWeapon2 = Agent.Main.Equipment[EquipmentIndex.ExtraWeaponSlot];
				this.HaveExtraWeapon = !missionWeapon2.IsEmpty;
				if (this.HaveExtraWeapon)
				{
					string itemTypeAsString2 = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon2.Item);
					string weaponName2 = this.GetWeaponName(missionWeapon2);
					this.EquippedExtraWeapon = new ControllerEquippedItemVM(weaponName2, itemTypeAsString2, EquipmentIndex.ExtraWeaponSlot, MissionMainAgentControllerEquipDropVM.GetWeaponHotKey(4, totalNumberOfWeaponsOnMainAgent), new Action<EquipmentActionItemVM>(this.OnItemSelected));
					num++;
				}
				this.UpdateItemsWieldStatus();
			}
			else
			{
				if (this._lastSelectedItem != null && this._lastSelectedItem.Identifier is EquipmentIndex)
				{
					Action<EquipmentIndex> toggleItem = this._toggleItem;
					if (toggleItem != null)
					{
						toggleItem((EquipmentIndex)this._lastSelectedItem.Identifier);
					}
				}
				this._lastSelectedItem = null;
			}
			this.IsActive = isEnabled;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00018C80 File Offset: 0x00016E80
		private void OnItemSelected(EquipmentActionItemVM selectedItem)
		{
			if (this._lastSelectedItem != selectedItem)
			{
				this._lastSelectedItem = selectedItem;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00018C92 File Offset: 0x00016E92
		public void OnCancelHoldController()
		{
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00018C94 File Offset: 0x00016E94
		public void OnWeaponDroppedAtIndex(int droppedWeaponIndex)
		{
			this.OnToggle(true);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00018C9D File Offset: 0x00016E9D
		private bool IsWieldedWeaponAtIndex(EquipmentIndex index)
		{
			return index == Agent.Main.GetPrimaryWieldedItemIndex() || index == Agent.Main.GetOffhandWieldedItemIndex();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00018CBB File Offset: 0x00016EBB
		public void OnWeaponEquippedAtIndex(int equippedWeaponIndex)
		{
			this.UpdateItemsWieldStatus();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00018CC4 File Offset: 0x00016EC4
		public void SetDropProgressForIndex(EquipmentIndex eqIndex, float progress)
		{
			int i = 0;
			while (i < this.EquippedWeapons.Count)
			{
				object obj;
				if (!((obj = this.EquippedWeapons[i].Identifier) is EquipmentIndex))
				{
					goto IL_0031;
				}
				EquipmentIndex equipmentIndex = (EquipmentIndex)obj;
				if (equipmentIndex != eqIndex || progress <= 0.2f)
				{
					goto IL_0031;
				}
				float num = progress;
				IL_0039:
				float num2 = num;
				this.EquippedWeapons[i].DropProgress = num2;
				i++;
				continue;
				IL_0031:
				num = 0f;
				goto IL_0039;
			}
			if (this.HaveExtraWeapon)
			{
				object obj;
				float num3;
				if ((obj = this.EquippedExtraWeapon.Identifier) is EquipmentIndex)
				{
					EquipmentIndex equipmentIndex2 = (EquipmentIndex)obj;
					if (equipmentIndex2 == eqIndex && progress > 0.2f)
					{
						num3 = progress;
						goto IL_0097;
					}
				}
				num3 = 0f;
				IL_0097:
				float num4 = num3;
				this.EquippedExtraWeapon.DropProgress = num4;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00018D78 File Offset: 0x00016F78
		private void UpdateItemsWieldStatus()
		{
			for (int i = 0; i < this.EquippedWeapons.Count; i++)
			{
				object identifier;
				if ((identifier = this.EquippedWeapons[i].Identifier) is EquipmentIndex)
				{
					EquipmentIndex equipmentIndex = (EquipmentIndex)identifier;
					this.EquippedWeapons[i].IsWielded = this.IsWieldedWeaponAtIndex(equipmentIndex);
				}
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00018DD4 File Offset: 0x00016FD4
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

		// Token: 0x060006DF RID: 1759 RVA: 0x00018EB8 File Offset: 0x000170B8
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (Agent.Main != null)
			{
				Agent main = Agent.Main;
				main.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(main.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
			}
			Mission.Current.OnMainAgentChanged -= this.OnMainAgentChanged;
			this.EquippedWeapons.ApplyActionOnAllItems(delegate(ControllerEquippedItemVM o)
			{
				o.OnFinalize();
			});
			this.EquippedWeapons.Clear();
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00018F44 File Offset: 0x00017144
		private static HotKey GetWeaponHotKey(int currentIndexOfWeapon, int totalNumOfWeapons)
		{
			if (currentIndexOfWeapon == 0)
			{
				if (totalNumOfWeapons == 1)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon4");
				}
				if (totalNumOfWeapons > 1)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon1");
				}
				Debug.FailedAssert("Wrong number of total weapons!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentControllerEquipDropVM.cs", "GetWeaponHotKey", 222);
			}
			else if (currentIndexOfWeapon == 1)
			{
				if (totalNumOfWeapons == 2)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon3");
				}
				if (totalNumOfWeapons > 2)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon4");
				}
			}
			else
			{
				if (currentIndexOfWeapon == 2)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon3");
				}
				if (currentIndexOfWeapon == 3)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropWeapon2");
				}
				if (currentIndexOfWeapon == 4)
				{
					return HotKeyManager.GetCategory("CombatHotKeyCategory").GetHotKey("ControllerEquipDropExtraWeapon");
				}
				Debug.FailedAssert("Wrong index of current weapon. Cannot be higher than 3", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentControllerEquipDropVM.cs", "GetWeaponHotKey", 250);
			}
			return null;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001903F File Offset: 0x0001723F
		public void OnGamepadActiveChanged(bool isActive)
		{
			this.HoldToDropText = (isActive ? this._dropTextObject.ToString() : string.Empty);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001905C File Offset: 0x0001725C
		private int GetTotalNumberOfWeaponsOnMainAgent()
		{
			int num = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (!Agent.Main.Equipment[equipmentIndex].IsEmpty)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00019096 File Offset: 0x00017296
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x0001909E File Offset: 0x0001729E
		[DataSourceProperty]
		public MBBindingList<ControllerEquippedItemVM> EquippedWeapons
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
					base.OnPropertyChangedWithValue<MBBindingList<ControllerEquippedItemVM>>(value, "EquippedWeapons");
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000190BC File Offset: 0x000172BC
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x000190C4 File Offset: 0x000172C4
		[DataSourceProperty]
		public ControllerEquippedItemVM EquippedExtraWeapon
		{
			get
			{
				return this._equippedExtraWeapon;
			}
			set
			{
				if (value != this._equippedExtraWeapon)
				{
					this._equippedExtraWeapon = value;
					base.OnPropertyChangedWithValue<ControllerEquippedItemVM>(value, "EquippedExtraWeapon");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000190E2 File Offset: 0x000172E2
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000190EA File Offset: 0x000172EA
		[DataSourceProperty]
		public string HoldToDropText
		{
			get
			{
				return this._holdToDropText;
			}
			set
			{
				if (value != this._holdToDropText)
				{
					this._holdToDropText = value;
					base.OnPropertyChangedWithValue<string>(value, "HoldToDropText");
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001910D File Offset: 0x0001730D
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x00019115 File Offset: 0x00017315
		[DataSourceProperty]
		public string PressToEquipText
		{
			get
			{
				return this._pressToEquipText;
			}
			set
			{
				if (value != this._pressToEquipText)
				{
					this._pressToEquipText = value;
					base.OnPropertyChangedWithValue<string>(value, "PressToEquipText");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00019138 File Offset: 0x00017338
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x00019140 File Offset: 0x00017340
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0001915E File Offset: 0x0001735E
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x00019166 File Offset: 0x00017366
		[DataSourceProperty]
		public bool HaveExtraWeapon
		{
			get
			{
				return this._haveExtraWeapon;
			}
			set
			{
				if (value != this._haveExtraWeapon)
				{
					this._haveExtraWeapon = value;
					base.OnPropertyChangedWithValue(value, "HaveExtraWeapon");
				}
			}
		}

		// Token: 0x0400030D RID: 781
		private EquipmentActionItemVM _lastSelectedItem;

		// Token: 0x0400030E RID: 782
		private Action<EquipmentIndex> _toggleItem;

		// Token: 0x0400030F RID: 783
		private TextObject _dropTextObject = new TextObject("{=d1tCz15N}Hold to Drop", null);

		// Token: 0x04000310 RID: 784
		private MBBindingList<ControllerEquippedItemVM> _equipActions;

		// Token: 0x04000311 RID: 785
		private ControllerEquippedItemVM _equippedExtraWeapon;

		// Token: 0x04000312 RID: 786
		private bool _isActive;

		// Token: 0x04000313 RID: 787
		private bool _haveExtraWeapon;

		// Token: 0x04000314 RID: 788
		private string _holdToDropText;

		// Token: 0x04000315 RID: 789
		private string _pressToEquipText;
	}
}
