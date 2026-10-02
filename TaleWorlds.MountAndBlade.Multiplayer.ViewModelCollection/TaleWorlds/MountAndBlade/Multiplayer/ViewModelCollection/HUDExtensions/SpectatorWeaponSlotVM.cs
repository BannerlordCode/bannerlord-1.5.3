using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x0200009A RID: 154
	public class SpectatorWeaponSlotVM : ViewModel
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002F920 File Offset: 0x0002DB20
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0002F928 File Offset: 0x0002DB28
		public ItemObject Item { get; private set; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002F931 File Offset: 0x0002DB31
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x0002F939 File Offset: 0x0002DB39
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002F944 File Offset: 0x0002DB44
		public SpectatorWeaponSlotVM(ItemObject item, EquipmentIndex slotIndex, MissionEquipment equipment)
		{
			this.Item = item;
			this.SlotIndex = slotIndex;
			this._itemModifier = equipment[slotIndex].ItemModifier;
			SpectatorWeaponSlotVM.ResolveAmmoTotals(equipment, out this._ammoCount, out this._averageAmmoDamage);
			this.Icon = new ItemImageIdentifierVM(item, "");
			this.Hint = new BasicTooltipViewModel(() => this.GetTooltipProperties());
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0002F9BF File Offset: 0x0002DBBF
		public override void OnFinalize()
		{
			base.OnFinalize();
			ItemImageIdentifierVM icon = this.Icon;
			if (icon != null)
			{
				icon.OnFinalize();
			}
			this.Icon = null;
			this.Hint = null;
			this.Item = null;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0002F9F0 File Offset: 0x0002DBF0
		private static void ResolveAmmoTotals(MissionEquipment equipment, out int ammoCount, out int averageAmmoDamage)
		{
			ammoCount = 0;
			averageAmmoDamage = 0;
			if (equipment == null)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				MissionWeapon missionWeapon = equipment[equipmentIndex];
				ItemObject item = missionWeapon.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && item.PrimaryWeapon.IsAmmo)
				{
					ammoCount += (int)item.PrimaryWeapon.GetModifiedStackCount(missionWeapon.ItemModifier);
					num2 += item.PrimaryWeapon.GetModifiedThrustDamage(missionWeapon.ItemModifier);
					num++;
				}
			}
			if (num > 0)
			{
				averageAmmoDamage = MathF.Round((float)num2 / (float)num);
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0002FA88 File Offset: 0x0002DC88
		private List<TooltipProperty> GetTooltipProperties()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			if (this.Item == null)
			{
				return list;
			}
			list.Add(new TooltipProperty(string.Empty, this.GetDisplayName().ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.Title));
			WeaponComponentData primaryWeapon = this.Item.PrimaryWeapon;
			if (primaryWeapon == null)
			{
				return list;
			}
			ItemObject.ItemTypeEnum itemTypeFromWeaponClass = WeaponComponentData.GetItemTypeFromWeaponClass(primaryWeapon.WeaponClass);
			if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Polearm)
			{
				if (primaryWeapon.SwingDamageType != DamageTypes.Invalid)
				{
					SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=yJsE4Ayo}Swing Spd.", null), primaryWeapon.GetModifiedSwingSpeed(this._itemModifier));
					SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=RNgWFLIO}Swing Dmg.", null), primaryWeapon.GetModifiedSwingDamage(this._itemModifier));
				}
				if (primaryWeapon.ThrustDamageType != DamageTypes.Invalid)
				{
					SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=J0vjDOFO}Thrust Spd.", null), primaryWeapon.GetModifiedThrustSpeed(this._itemModifier));
					SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=Ie9I2Bha}Thrust Dmg.", null), primaryWeapon.GetModifiedThrustDamage(this._itemModifier));
				}
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=ftoSCQ0x}Length", null), primaryWeapon.WeaponLength);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=oibdTnXP}Handling", null), primaryWeapon.GetModifiedHandling(this._itemModifier));
			}
			else if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Thrown)
			{
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=ftoSCQ0x}Length", null), primaryWeapon.WeaponLength);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=s31DnnAf}Damage", null), primaryWeapon.GetModifiedThrustDamage(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=QfTt7YRB}Fire Rate", null), primaryWeapon.GetModifiedMissileSpeed(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=TAnabTdy}Accuracy", null), primaryWeapon.Accuracy);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=b31ITmm0}Stack Amnt.", null), (int)primaryWeapon.GetModifiedStackCount(this._itemModifier));
			}
			else if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Shield)
			{
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=6GSXsdeX}Speed", null), primaryWeapon.GetModifiedThrustSpeed(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=GGseMDd3}Durability", null), (int)primaryWeapon.GetModifiedMaximumHitPoints(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=ahiBhAqU}Armor", null), primaryWeapon.GetModifiedArmor(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=4Dd2xgPm}Weight", null), (int)this.Item.Weight);
			}
			else if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Bow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Sling)
			{
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=ftoSCQ0x}Length", null), primaryWeapon.WeaponLength);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=s31DnnAf}Damage", null), primaryWeapon.GetModifiedThrustDamage(this._itemModifier) + this._averageAmmoDamage);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=QfTt7YRB}Fire Rate", null), primaryWeapon.GetModifiedSwingSpeed(this._itemModifier));
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=TAnabTdy}Accuracy", null), primaryWeapon.Accuracy);
				SpectatorWeaponSlotVM.AddProperty(list, new TextObject("{=yUpH2mQ4}Ammo", null), this._ammoCount);
			}
			SpectatorWeaponSlotVM.AddWeaponFlagProperties(list, primaryWeapon.WeaponFlags);
			return list;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0002FD66 File Offset: 0x0002DF66
		private TextObject GetDisplayName()
		{
			if (this._itemModifier == null)
			{
				return this.Item.Name;
			}
			TextObject name = this._itemModifier.Name;
			name.SetTextVariable("ITEMNAME", this.Item.Name);
			return name;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0002FDA0 File Offset: 0x0002DFA0
		private static void AddWeaponFlagProperties(List<TooltipProperty> properties, WeaponFlags weaponFlags)
		{
			List<TextObject> weaponFlagTexts = SpectatorWeaponSlotVM.GetWeaponFlagTexts(weaponFlags);
			if (weaponFlagTexts.Count == 0)
			{
				return;
			}
			properties.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			for (int i = 0; i < weaponFlagTexts.Count; i++)
			{
				properties.Add(new TooltipProperty(string.Empty, weaponFlagTexts[i].ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0002FE0C File Offset: 0x0002E00C
		private static List<TextObject> GetWeaponFlagTexts(WeaponFlags weaponFlags)
		{
			List<TextObject> list = new List<TextObject>();
			if (weaponFlags.HasAnyFlag(WeaponFlags.BonusAgainstShield))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_bonus_against_shield", null));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_can_knockdown", null));
			}
			if (weaponFlags.HasAllFlags(WeaponFlags.CanDismount | WeaponFlags.CanHook))
			{
				list.Add(new TextObject("{=7HA99oUg}Both swing and thrust attacks can dismount riders", null));
			}
			else if (weaponFlags.HasAnyFlag(WeaponFlags.CanDismount))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_can_dismount", null));
			}
			else if (weaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_can_hook", null));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanCrushThrough))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_can_crush_through", null));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithTwoHand))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_not_usable_two_hand", null));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_not_usable_one_hand", null));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CantReloadOnHorseback))
			{
				list.Add(GameTexts.FindText("str_inventory_flag_cant_reload_on_horseback", null));
			}
			return list;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0002FF35 File Offset: 0x0002E135
		private static void AddProperty(List<TooltipProperty> properties, TextObject name, int value)
		{
			properties.Add(new TooltipProperty(name.ToString(), value.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002FF52 File Offset: 0x0002E152
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0002FF5A File Offset: 0x0002E15A
		[DataSourceProperty]
		public ItemImageIdentifierVM Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Icon");
				}
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002FF78 File Offset: 0x0002E178
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x0002FF80 File Offset: 0x0002E180
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0002FF9E File Offset: 0x0002E19E
		// (set) Token: 0x06000F65 RID: 3941 RVA: 0x0002FFA6 File Offset: 0x0002E1A6
		[DataSourceProperty]
		public bool IsEquipped
		{
			get
			{
				return this._isEquipped;
			}
			set
			{
				if (value != this._isEquipped)
				{
					this._isEquipped = value;
					base.OnPropertyChangedWithValue(value, "IsEquipped");
					this.RefreshSlotState();
				}
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0002FFCA File Offset: 0x0002E1CA
		public void SetEquipped(bool isEquipped)
		{
			this.IsEquipped = isEquipped;
			this.RefreshSlotState();
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0002FFD9 File Offset: 0x0002E1D9
		private void RefreshSlotState()
		{
			this.SlotState = (this._isEquipped ? "Equipped" : "Default");
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0002FFF5 File Offset: 0x0002E1F5
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0002FFFD File Offset: 0x0002E1FD
		[DataSourceProperty]
		public string SlotState
		{
			get
			{
				return this._slotState;
			}
			set
			{
				if (value != this._slotState)
				{
					this._slotState = value;
					base.OnPropertyChangedWithValue<string>(value, "SlotState");
				}
			}
		}

		// Token: 0x04000720 RID: 1824
		private const int TitleTextHeight = 1;

		// Token: 0x04000721 RID: 1825
		private const int PropertyTextHeight = 0;

		// Token: 0x04000722 RID: 1826
		private const string DefaultSlotState = "Default";

		// Token: 0x04000723 RID: 1827
		private const string EquippedSlotState = "Equipped";

		// Token: 0x04000726 RID: 1830
		private readonly ItemModifier _itemModifier;

		// Token: 0x04000727 RID: 1831
		private readonly int _ammoCount;

		// Token: 0x04000728 RID: 1832
		private readonly int _averageAmmoDamage;

		// Token: 0x04000729 RID: 1833
		private ItemImageIdentifierVM _icon;

		// Token: 0x0400072A RID: 1834
		private BasicTooltipViewModel _hint;

		// Token: 0x0400072B RID: 1835
		private bool _isEquipped;

		// Token: 0x0400072C RID: 1836
		private string _slotState = "Default";
	}
}
