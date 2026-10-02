using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AF RID: 175
	public class ShallowItemVM : ViewModel
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00034932 File Offset: 0x00032B32
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x0003493A File Offset: 0x00032B3A
		public ShallowItemVM.ItemGroup Type { get; private set; }

		// Token: 0x060010D4 RID: 4308 RVA: 0x00034944 File Offset: 0x00032B44
		public ShallowItemVM(Action<ShallowItemVM> onSelect)
		{
			this.ItemInformationList = new MBBindingList<ShallowItemVM.ArmoryItemFlagVM>();
			this.PropertyList = new MBBindingList<ShallowItemPropertyVM>();
			this.AlternativeUsageSelector = new SelectorVM<AlternativeUsageItemOptionVM>(new List<string>(), 0, new Action<SelectorVM<AlternativeUsageItemOptionVM>>(this.OnAlternativeUsageChanged));
			this._onSelect = onSelect;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00034994 File Offset: 0x00032B94
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshWith(this._equipmentIndex, this._equipment);
			this.PropertyList.ApplyActionOnAllItems(delegate(ShallowItemPropertyVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000349E3 File Offset: 0x00032BE3
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._equipment = null;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000349F4 File Offset: 0x00032BF4
		public void RefreshWith(EquipmentIndex equipmentIndex, Equipment equipment)
		{
			this._equipment = equipment;
			this._equipmentIndex = equipmentIndex;
			ItemObject itemObject = ((equipment != null) ? equipment[equipmentIndex].Item : null);
			if (itemObject == null || (equipmentIndex == EquipmentIndex.ArmorItemEndSlot && !itemObject.HasHorseComponent) || (equipmentIndex != EquipmentIndex.ArmorItemEndSlot && (itemObject.PrimaryWeapon == null || itemObject.PrimaryWeapon.IsAmmo)))
			{
				this.IsValid = false;
				this.Icon = new ItemImageIdentifierVM(null, "");
				return;
			}
			this.IsValid = true;
			this.Name = itemObject.Name.ToString();
			this.Icon = new ItemImageIdentifierVM(itemObject, "");
			this.Type = ShallowItemVM.GetItemGroupType(itemObject);
			this.TypeAsString = ((this.Type == ShallowItemVM.ItemGroup.None) ? "" : this.Type.ToString());
			this.HasAnyAlternativeUsage = false;
			this.AlternativeUsageSelector.ItemList.Clear();
			if (itemObject.PrimaryWeapon != null)
			{
				for (int i = 0; i < itemObject.Weapons.Count; i++)
				{
					WeaponComponentData weaponComponentData = itemObject.Weapons[i];
					if (ShallowItemVM.IsItemUsageApplicable(weaponComponentData))
					{
						TextObject textObject = GameTexts.FindText("str_weapon_usage", weaponComponentData.WeaponDescriptionId);
						this.AlternativeUsageSelector.AddItem(new AlternativeUsageItemOptionVM(weaponComponentData.WeaponDescriptionId, textObject, textObject, this.AlternativeUsageSelector, i));
						this.HasAnyAlternativeUsage = true;
					}
				}
			}
			this.AlternativeUsageSelector.SelectedIndex = -1;
			this.AlternativeUsageSelector.SelectedIndex = 0;
			this._latestUsageOption = this.AlternativeUsageSelector.ItemList.FirstOrDefault<AlternativeUsageItemOptionVM>();
			if (this._latestUsageOption != null)
			{
				this._latestUsageOption.IsSelected = true;
			}
			this.AlternativeUsageSelector.SetOnChangeAction(new Action<SelectorVM<AlternativeUsageItemOptionVM>>(this.OnAlternativeUsageChanged));
			this.RefreshItemPropertyList(this._equipmentIndex, this._equipment, this.AlternativeUsageSelector.SelectedIndex);
			this._isInitialized = true;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00034BCC File Offset: 0x00032DCC
		private void OnAlternativeUsageChanged(SelectorVM<AlternativeUsageItemOptionVM> selector)
		{
			if (this._isInitialized && selector.SelectedIndex >= 0)
			{
				if (this._latestUsageOption != null)
				{
					this._latestUsageOption.IsSelected = false;
				}
				this.RefreshItemPropertyList(this._equipmentIndex, this._equipment, selector.SelectedIndex);
				if (selector.SelectedItem != null)
				{
					selector.SelectedItem.IsSelected = true;
				}
			}
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00034C2C File Offset: 0x00032E2C
		private void RefreshItemPropertyList(EquipmentIndex equipmentIndex, Equipment equipment, int alternativeIndex)
		{
			ItemObject item = equipment[equipmentIndex].Item;
			ItemModifier itemModifier = equipment[equipmentIndex].ItemModifier;
			this.PropertyList.Clear();
			if (item.PrimaryWeapon != null)
			{
				WeaponComponentData weaponComponentData = item.Weapons[alternativeIndex];
				ItemObject.ItemTypeEnum itemTypeFromWeaponClass = WeaponComponentData.GetItemTypeFromWeaponClass(weaponComponentData.WeaponClass);
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Polearm)
				{
					if (weaponComponentData.SwingDamageType != DamageTypes.Invalid)
					{
						this.AddProperty(new TextObject("{=yJsE4Ayo}Swing Spd.", null), (float)weaponComponentData.GetModifiedSwingSpeed(itemModifier) / 145f, weaponComponentData.GetModifiedSwingSpeed(itemModifier));
						this.AddProperty(new TextObject("{=RNgWFLIO}Swing Dmg.", null), (float)weaponComponentData.GetModifiedSwingDamage(itemModifier) / 143f, weaponComponentData.GetModifiedSwingDamage(itemModifier));
					}
					if (weaponComponentData.ThrustDamageType != DamageTypes.Invalid)
					{
						this.AddProperty(new TextObject("{=J0vjDOFO}Thrust Spd.", null), (float)weaponComponentData.GetModifiedThrustSpeed(itemModifier) / 114f, weaponComponentData.GetModifiedThrustSpeed(itemModifier));
						this.AddProperty(new TextObject("{=Ie9I2Bha}Thrust Dmg.", null), (float)weaponComponentData.GetModifiedThrustDamage(itemModifier) / 86f, weaponComponentData.GetModifiedThrustDamage(itemModifier));
					}
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 315f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=oibdTnXP}Handling", null), (float)weaponComponentData.GetModifiedHandling(itemModifier) / 120f, weaponComponentData.GetModifiedHandling(itemModifier));
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Thrown)
				{
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 147f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=s31DnnAf}Damage", null), (float)weaponComponentData.GetModifiedThrustDamage(itemModifier) / 94f, weaponComponentData.GetModifiedThrustDamage(itemModifier));
					this.AddProperty(new TextObject("{=QfTt7YRB}Fire Rate", null), (float)weaponComponentData.GetModifiedMissileSpeed(itemModifier) / 115f, weaponComponentData.GetModifiedMissileSpeed(itemModifier));
					this.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null), (float)weaponComponentData.Accuracy / 300f, weaponComponentData.Accuracy);
					this.AddProperty(new TextObject("{=b31ITmm0}Stack Amnt.", null), (float)weaponComponentData.GetModifiedStackCount(itemModifier) / 40f, (int)weaponComponentData.GetModifiedStackCount(itemModifier));
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Shield)
				{
					this.AddProperty(new TextObject("{=6GSXsdeX}Speed", null), (float)weaponComponentData.GetModifiedThrustSpeed(itemModifier) / 120f, weaponComponentData.GetModifiedThrustSpeed(itemModifier));
					this.AddProperty(new TextObject("{=GGseMDd3}Durability", null), (float)weaponComponentData.GetModifiedMaximumHitPoints(itemModifier) / 500f, (int)weaponComponentData.GetModifiedMaximumHitPoints(itemModifier));
					this.AddProperty(new TextObject("{=ahiBhAqU}Armor", null), (float)weaponComponentData.GetModifiedArmor(itemModifier) / 40f, weaponComponentData.GetModifiedArmor(itemModifier));
					this.AddProperty(new TextObject("{=4Dd2xgPm}Weight", null), item.Weight / 40f, (int)item.Weight);
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Bow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Sling)
				{
					int num = 0;
					float num2 = 0f;
					int num3 = 0;
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
					{
						ItemObject item2 = equipment[equipmentIndex2].Item;
						ItemModifier itemModifier2 = equipment[equipmentIndex2].ItemModifier;
						if (item2 != null && item2.PrimaryWeapon.IsAmmo)
						{
							num += (int)item2.PrimaryWeapon.GetModifiedStackCount(itemModifier2);
							num3 += item2.PrimaryWeapon.GetModifiedThrustDamage(itemModifier2);
							num2 += 1f;
						}
					}
					num3 = MathF.Round((float)num3 / num2);
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 123f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=s31DnnAf}Damage", null), (float)(weaponComponentData.GetModifiedThrustDamage(itemModifier) + num3) / 70f, weaponComponentData.GetModifiedThrustDamage(itemModifier) + num3);
					this.AddProperty(new TextObject("{=QfTt7YRB}Fire Rate", null), (float)weaponComponentData.GetModifiedSwingSpeed(itemModifier) / 120f, weaponComponentData.GetModifiedSwingSpeed(itemModifier));
					this.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null), (float)weaponComponentData.Accuracy / 105f, weaponComponentData.Accuracy);
					this.AddProperty(new TextObject("{=yUpH2mQ4}Ammo", null), (float)num / 90f, num);
				}
				this.ItemInformationList.Clear();
				List<ValueTuple<string, TextObject>> weaponFlagDetails = ShallowItemVM.GetWeaponFlagDetails(weaponComponentData.WeaponFlags);
				for (int i = 0; i < weaponFlagDetails.Count; i++)
				{
					ShallowItemVM.ArmoryItemFlagVM armoryItemFlagVM = new ShallowItemVM.ArmoryItemFlagVM(weaponFlagDetails[i].Item1, weaponFlagDetails[i].Item2);
					this.ItemInformationList.Add(armoryItemFlagVM);
				}
			}
			if (item.HorseComponent != null)
			{
				EquipmentElement equipmentElement = equipment[EquipmentIndex.ArmorItemEndSlot];
				EquipmentElement equipmentElement2 = equipment[EquipmentIndex.HorseHarness];
				int modifiedMountCharge = equipmentElement.GetModifiedMountCharge(in equipmentElement2);
				int num4 = (int)(4.33f * (float)equipmentElement.GetModifiedMountSpeed(in equipmentElement2));
				int modifiedMountManeuver = equipmentElement.GetModifiedMountManeuver(in equipmentElement2);
				int modifiedMountHitPoints = equipmentElement.GetModifiedMountHitPoints();
				int modifiedMountBodyArmor = equipmentElement2.GetModifiedMountBodyArmor();
				this.AddProperty(new TextObject("{=DAVb2Pzg}Charge Dmg.", null), (float)modifiedMountCharge / 35f, modifiedMountCharge);
				this.AddProperty(new TextObject("{=6GSXsdeX}Speed", null), (float)num4 / 303.1f, num4);
				this.AddProperty(new TextObject("{=rg7OuWS2}Maneuver", null), (float)modifiedMountManeuver / 70f, modifiedMountManeuver);
				this.AddProperty(new TextObject("{=oBbiVeKE}Hit Points", null), (float)modifiedMountHitPoints / 300f, modifiedMountHitPoints);
				this.AddProperty(new TextObject("{=kftE5nvv}Horse Armor", null), (float)modifiedMountBodyArmor / 100f, modifiedMountBodyArmor);
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00035190 File Offset: 0x00033390
		private static List<ValueTuple<string, TextObject>> GetWeaponFlagDetails(WeaponFlags weaponFlags)
		{
			List<ValueTuple<string, TextObject>> list = new List<ValueTuple<string, TextObject>>();
			if (weaponFlags.HasAnyFlag(WeaponFlags.BonusAgainstShield))
			{
				string text = "WeaponFlagIcons\\bonus_against_shield";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_bonus_against_shield", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown))
			{
				string text = "WeaponFlagIcons\\can_knock_down";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_knockdown", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanDismount) && !weaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_dismount", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanHook) && !weaponFlags.HasAnyFlag(WeaponFlags.CanDismount))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_hook", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAllFlags(WeaponFlags.CanDismount | WeaponFlags.CanHook))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = new TextObject("{=7HA99oUg}Both swing and thrust attacks can dismount riders", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanCrushThrough))
			{
				string text = "WeaponFlagIcons\\can_crush_through";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_crush_through", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithTwoHand))
			{
				string text = "WeaponFlagIcons\\not_usable_with_two_hand";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_not_usable_two_hand", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
			{
				string text = "WeaponFlagIcons\\not_usable_with_one_hand";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_not_usable_one_hand", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CantReloadOnHorseback))
			{
				string text = "WeaponFlagIcons\\cant_reload_on_horseback";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_cant_reload_on_horseback", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			return list;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0003534F File Offset: 0x0003354F
		private void AddProperty(TextObject name, float fraction, int value)
		{
			this.PropertyList.Add(new ShallowItemPropertyVM(name, MathF.Round(fraction * 1000f), value));
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00035370 File Offset: 0x00033570
		private static ShallowItemVM.ItemGroup GetItemGroupType(ItemObject item)
		{
			if (item.WeaponComponent != null)
			{
				switch (item.WeaponComponent.PrimaryWeapon.WeaponClass)
				{
				case WeaponClass.OneHandedSword:
				case WeaponClass.TwoHandedSword:
					return ShallowItemVM.ItemGroup.Sword;
				case WeaponClass.OneHandedAxe:
				case WeaponClass.TwoHandedAxe:
					return ShallowItemVM.ItemGroup.Axe;
				case WeaponClass.Mace:
				case WeaponClass.TwoHandedMace:
					return ShallowItemVM.ItemGroup.Mace;
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					return ShallowItemVM.ItemGroup.Spear;
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
				case WeaponClass.SlingStone:
				case WeaponClass.Cartridge:
				case WeaponClass.Musket:
					return ShallowItemVM.ItemGroup.Ammo;
				case WeaponClass.Bow:
					return ShallowItemVM.ItemGroup.Bow;
				case WeaponClass.Crossbow:
					return ShallowItemVM.ItemGroup.Crossbow;
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					return ShallowItemVM.ItemGroup.Stone;
				case WeaponClass.ThrowingAxe:
					return ShallowItemVM.ItemGroup.ThrowingAxe;
				case WeaponClass.ThrowingKnife:
					return ShallowItemVM.ItemGroup.ThrowingKnife;
				case WeaponClass.Javelin:
					return ShallowItemVM.ItemGroup.Javelin;
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					return ShallowItemVM.ItemGroup.Shield;
				}
				return ShallowItemVM.ItemGroup.None;
			}
			if (item.HasHorseComponent)
			{
				return ShallowItemVM.ItemGroup.Mount;
			}
			return ShallowItemVM.ItemGroup.None;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0003543D File Offset: 0x0003363D
		[UsedImplicitly]
		public void OnSelect()
		{
			this._onSelect(this);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0003544B File Offset: 0x0003364B
		public static bool IsItemUsageApplicable(WeaponComponentData weapon)
		{
			WeaponDescription weaponDescription = ((weapon != null && weapon.WeaponDescriptionId != null) ? MBObjectManager.Instance.GetObject<WeaponDescription>(weapon.WeaponDescriptionId) : null);
			return weaponDescription != null && !weaponDescription.IsHiddenFromUI;
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00035479 File Offset: 0x00033679
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x00035481 File Offset: 0x00033681
		[DataSourceProperty]
		public MBBindingList<ShallowItemVM.ArmoryItemFlagVM> ItemInformationList
		{
			get
			{
				return this._itemInformationList;
			}
			set
			{
				if (value != this._itemInformationList)
				{
					this._itemInformationList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ShallowItemVM.ArmoryItemFlagVM>>(value, "ItemInformationList");
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0003549F File Offset: 0x0003369F
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x000354A7 File Offset: 0x000336A7
		[DataSourceProperty]
		public MBBindingList<ShallowItemPropertyVM> PropertyList
		{
			get
			{
				return this._propertyList;
			}
			set
			{
				if (value != this._propertyList)
				{
					this._propertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ShallowItemPropertyVM>>(value, "PropertyList");
				}
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x000354C5 File Offset: 0x000336C5
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x000354CD File Offset: 0x000336CD
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

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x000354F0 File Offset: 0x000336F0
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x000354F8 File Offset: 0x000336F8
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

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00035516 File Offset: 0x00033716
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x0003551E File Offset: 0x0003371E
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00035541 File Offset: 0x00033741
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x00035549 File Offset: 0x00033749
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

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00035567 File Offset: 0x00033767
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x0003556F File Offset: 0x0003376F
		[DataSourceProperty]
		public bool HasAnyAlternativeUsage
		{
			get
			{
				return this._hasAnyAlternativeUsage;
			}
			set
			{
				if (value != this._hasAnyAlternativeUsage)
				{
					this._hasAnyAlternativeUsage = value;
					base.OnPropertyChangedWithValue(value, "HasAnyAlternativeUsage");
				}
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0003558D File Offset: 0x0003378D
		// (set) Token: 0x060010EE RID: 4334 RVA: 0x00035595 File Offset: 0x00033795
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x000355B3 File Offset: 0x000337B3
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x000355BB File Offset: 0x000337BB
		[DataSourceProperty]
		public SelectorVM<AlternativeUsageItemOptionVM> AlternativeUsageSelector
		{
			get
			{
				return this._alternativeUsageSelector;
			}
			set
			{
				if (value != this._alternativeUsageSelector)
				{
					this._alternativeUsageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<AlternativeUsageItemOptionVM>>(value, "AlternativeUsageSelector");
				}
			}
		}

		// Token: 0x040007E9 RID: 2025
		private readonly Action<ShallowItemVM> _onSelect;

		// Token: 0x040007EB RID: 2027
		private AlternativeUsageItemOptionVM _latestUsageOption;

		// Token: 0x040007EC RID: 2028
		private Equipment _equipment;

		// Token: 0x040007ED RID: 2029
		private EquipmentIndex _equipmentIndex;

		// Token: 0x040007EE RID: 2030
		private bool _isInitialized;

		// Token: 0x040007EF RID: 2031
		private ItemImageIdentifierVM _icon;

		// Token: 0x040007F0 RID: 2032
		private string _name;

		// Token: 0x040007F1 RID: 2033
		private string _typeAsString;

		// Token: 0x040007F2 RID: 2034
		private bool _isValid;

		// Token: 0x040007F3 RID: 2035
		private bool _isSelected;

		// Token: 0x040007F4 RID: 2036
		private bool _hasAnyAlternativeUsage;

		// Token: 0x040007F5 RID: 2037
		private MBBindingList<ShallowItemVM.ArmoryItemFlagVM> _itemInformationList;

		// Token: 0x040007F6 RID: 2038
		private MBBindingList<ShallowItemPropertyVM> _propertyList;

		// Token: 0x040007F7 RID: 2039
		private SelectorVM<AlternativeUsageItemOptionVM> _alternativeUsageSelector;

		// Token: 0x0200019B RID: 411
		public enum ItemGroup
		{
			// Token: 0x04000AC8 RID: 2760
			None,
			// Token: 0x04000AC9 RID: 2761
			Spear,
			// Token: 0x04000ACA RID: 2762
			Javelin,
			// Token: 0x04000ACB RID: 2763
			Bow,
			// Token: 0x04000ACC RID: 2764
			Crossbow,
			// Token: 0x04000ACD RID: 2765
			Sword,
			// Token: 0x04000ACE RID: 2766
			Axe,
			// Token: 0x04000ACF RID: 2767
			Mace,
			// Token: 0x04000AD0 RID: 2768
			ThrowingAxe,
			// Token: 0x04000AD1 RID: 2769
			ThrowingKnife,
			// Token: 0x04000AD2 RID: 2770
			Ammo,
			// Token: 0x04000AD3 RID: 2771
			Shield,
			// Token: 0x04000AD4 RID: 2772
			Mount,
			// Token: 0x04000AD5 RID: 2773
			Stone
		}

		// Token: 0x0200019C RID: 412
		public class ArmoryItemFlagVM : ViewModel
		{
			// Token: 0x060013A4 RID: 5028 RVA: 0x0003E71A File Offset: 0x0003C91A
			public ArmoryItemFlagVM(string icon, TextObject hintText)
			{
				this.Icon = "SPGeneral\\" + icon;
				this.Hint = new HintViewModel(hintText, null);
			}

			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x060013A5 RID: 5029 RVA: 0x0003E740 File Offset: 0x0003C940
			// (set) Token: 0x060013A6 RID: 5030 RVA: 0x0003E748 File Offset: 0x0003C948
			[DataSourceProperty]
			public string Icon
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
						base.OnPropertyChangedWithValue<string>(value, "Icon");
					}
				}
			}

			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x060013A7 RID: 5031 RVA: 0x0003E76B File Offset: 0x0003C96B
			// (set) Token: 0x060013A8 RID: 5032 RVA: 0x0003E773 File Offset: 0x0003C973
			[DataSourceProperty]
			public HintViewModel Hint
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
						base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
					}
				}
			}

			// Token: 0x04000AD6 RID: 2774
			private string _icon;

			// Token: 0x04000AD7 RID: 2775
			private HintViewModel _hint;
		}
	}
}
