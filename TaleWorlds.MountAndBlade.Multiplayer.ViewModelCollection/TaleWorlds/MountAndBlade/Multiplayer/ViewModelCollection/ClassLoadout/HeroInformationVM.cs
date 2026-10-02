using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A9 RID: 169
	public class HeroInformationVM : ViewModel
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x000329BD File Offset: 0x00030BBD
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x000329C5 File Offset: 0x00030BC5
		public MultiplayerClassDivisions.MPHeroClass HeroClass { get; private set; }

		// Token: 0x0600103F RID: 4159 RVA: 0x000329D0 File Offset: 0x00030BD0
		public HeroInformationVM()
		{
			this._latestSelectedItemGroup = ShallowItemVM.ItemGroup.None;
			this.Item1 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item2 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item3 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item4 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.ItemHorse = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.IsArmyAvailable = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0;
			this.SetFirstSelectedItem();
			this.RefreshValues();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00032A8C File Offset: 0x00030C8C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmySizeHint = new HintViewModel(GameTexts.FindText("str_army_size", null), null);
			this.MovementSpeedHint = new HintViewModel(GameTexts.FindText("str_movement_speed", null), null);
			this.HitPointsHint = new HintViewModel(GameTexts.FindText("str_hitpoints", null), null);
			this.ArmorHint = new HintViewModel(GameTexts.FindText("str_armor", null), null);
			this.EquipmentText = GameTexts.FindText("str_equipment", null).ToString();
			ShallowItemVM item = this._item1;
			if (item != null)
			{
				item.RefreshValues();
			}
			ShallowItemVM item2 = this._item2;
			if (item2 != null)
			{
				item2.RefreshValues();
			}
			ShallowItemVM item3 = this._item3;
			if (item3 != null)
			{
				item3.RefreshValues();
			}
			ShallowItemVM item4 = this._item4;
			if (item4 != null)
			{
				item4.RefreshValues();
			}
			ShallowItemVM itemHorse = this._itemHorse;
			if (itemHorse != null)
			{
				itemHorse.RefreshValues();
			}
			ShallowItemVM itemSelected = this._itemSelected;
			if (itemSelected != null)
			{
				itemSelected.RefreshValues();
			}
			if (this.HeroClass != null)
			{
				this.NameText = this.HeroClass.HeroName.ToString();
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00032B98 File Offset: 0x00030D98
		public void RefreshWith(MultiplayerClassDivisions.MPHeroClass heroClass, List<IReadOnlyPerkObject> perks)
		{
			this.HeroClass = heroClass;
			Equipment equipment = heroClass.HeroCharacter.Equipment.Clone(false);
			MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(perks);
			IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
			if (enumerable != null)
			{
				foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
				{
					equipment[valueTuple.Item1] = valueTuple.Item2;
				}
			}
			this.ItemHorse.RefreshWith(EquipmentIndex.ArmorItemEndSlot, equipment);
			this.Item1.RefreshWith(EquipmentIndex.WeaponItemBeginSlot, equipment);
			this.Item2.RefreshWith(EquipmentIndex.Weapon1, equipment);
			this.Item3.RefreshWith(EquipmentIndex.Weapon2, equipment);
			this.Item4.RefreshWith(EquipmentIndex.Weapon3, equipment);
			TextObject heroInformation = heroClass.HeroInformation;
			this.Information = ((heroInformation != null) ? heroInformation.ToString() : null);
			this.NameText = heroClass.HeroName.ToString();
			int num = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (num == 0)
			{
				num = 25;
				this._armySizeHintWithDefaultValue.SetTextVariable("OPTION_VALUE", 25);
				this.ArmySizeHint.HintText = this._armySizeHintWithDefaultValue;
			}
			else
			{
				this.ArmySizeHint.HintText = GameTexts.FindText("str_army_size", null);
			}
			this.ArmySize = MPPerkObject.GetTroopCount(heroClass, num, onSpawnPerkHandler);
			this.MovementSpeed = (int)(this.HeroClass.HeroMovementSpeedMultiplier * 100f);
			this.HitPoints = heroClass.Health;
			this.Armor = (int)((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetDrivenPropertyBonusOnSpawn(true, DrivenProperty.ArmorTorso, (float)this.HeroClass.ArmorValue) : 0f) + this.HeroClass.ArmorValue;
			if (!this.TrySetSelectedItemByType(this._latestSelectedItemGroup))
			{
				this.SetFirstSelectedItem();
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00032D54 File Offset: 0x00030F54
		private bool TrySetSelectedItemByType(ShallowItemVM.ItemGroup itemGroup)
		{
			if (this.Item1.IsValid && this.Item1.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item1);
				return true;
			}
			if (this.Item2.IsValid && this.Item2.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item2);
				return true;
			}
			if (this.Item3.IsValid && this.Item3.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item3);
				return true;
			}
			if (this.Item4.IsValid && this.Item4.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item4);
				return true;
			}
			if (this.ItemHorse.IsValid && this.ItemHorse.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.ItemHorse);
				return true;
			}
			return false;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00032E30 File Offset: 0x00031030
		private void SetFirstSelectedItem()
		{
			ShallowItemVM itemSelected = this.ItemSelected;
			if (itemSelected == null || !itemSelected.IsValid)
			{
				if (this.Item1.IsValid)
				{
					this.UpdateHighlightedItem(this.Item1);
					return;
				}
				if (this.Item2.IsValid)
				{
					this.UpdateHighlightedItem(this.Item2);
					return;
				}
				if (this.Item3.IsValid)
				{
					this.UpdateHighlightedItem(this.Item3);
					return;
				}
				if (this.Item4.IsValid)
				{
					this.UpdateHighlightedItem(this.Item4);
					return;
				}
				if (this.ItemHorse.IsValid)
				{
					this.UpdateHighlightedItem(this.ItemHorse);
				}
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00032ED8 File Offset: 0x000310D8
		public void UpdateHighlightedItem(ShallowItemVM item)
		{
			this.ItemSelected = item;
			this.Item1.IsSelected = false;
			this.Item2.IsSelected = false;
			this.Item3.IsSelected = false;
			this.Item4.IsSelected = false;
			this.ItemHorse.IsSelected = false;
			item.IsSelected = true;
			this._latestSelectedItemGroup = item.Type;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00032F3B File Offset: 0x0003113B
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00032F43 File Offset: 0x00031143
		[DataSourceProperty]
		public HintViewModel ArmySizeHint
		{
			get
			{
				return this._armySizeHint;
			}
			set
			{
				if (value != this._armySizeHint)
				{
					this._armySizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ArmySizeHint");
				}
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x00032F61 File Offset: 0x00031161
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x00032F69 File Offset: 0x00031169
		[DataSourceProperty]
		public HintViewModel MovementSpeedHint
		{
			get
			{
				return this._movementSpeedHint;
			}
			set
			{
				if (value != this._movementSpeedHint)
				{
					this._movementSpeedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MovementSpeedHint");
				}
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x00032F87 File Offset: 0x00031187
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x00032F8F File Offset: 0x0003118F
		[DataSourceProperty]
		public HintViewModel HitPointsHint
		{
			get
			{
				return this._hitPointsHint;
			}
			set
			{
				if (value != this._hitPointsHint)
				{
					this._hitPointsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HitPointsHint");
				}
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x00032FAD File Offset: 0x000311AD
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x00032FB5 File Offset: 0x000311B5
		[DataSourceProperty]
		public HintViewModel ArmorHint
		{
			get
			{
				return this._armorHint;
			}
			set
			{
				if (value != this._armorHint)
				{
					this._armorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ArmorHint");
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x00032FD3 File Offset: 0x000311D3
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x00032FDB File Offset: 0x000311DB
		[DataSourceProperty]
		public ShallowItemVM Item1
		{
			get
			{
				return this._item1;
			}
			set
			{
				if (value != this._item1)
				{
					this._item1 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item1");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x00032FF9 File Offset: 0x000311F9
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x00033001 File Offset: 0x00031201
		[DataSourceProperty]
		public ShallowItemVM Item2
		{
			get
			{
				return this._item2;
			}
			set
			{
				if (value != this._item2)
				{
					this._item2 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item2");
				}
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0003301F File Offset: 0x0003121F
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x00033027 File Offset: 0x00031227
		[DataSourceProperty]
		public ShallowItemVM Item3
		{
			get
			{
				return this._item3;
			}
			set
			{
				if (value != this._item3)
				{
					this._item3 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item3");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x00033045 File Offset: 0x00031245
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0003304D File Offset: 0x0003124D
		[DataSourceProperty]
		public ShallowItemVM Item4
		{
			get
			{
				return this._item4;
			}
			set
			{
				if (value != this._item4)
				{
					this._item4 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item4");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0003306B File Offset: 0x0003126B
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00033073 File Offset: 0x00031273
		[DataSourceProperty]
		public ShallowItemVM ItemHorse
		{
			get
			{
				return this._itemHorse;
			}
			set
			{
				if (value != this._itemHorse)
				{
					this._itemHorse = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "ItemHorse");
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00033091 File Offset: 0x00031291
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x00033099 File Offset: 0x00031299
		[DataSourceProperty]
		public ShallowItemVM ItemSelected
		{
			get
			{
				return this._itemSelected;
			}
			set
			{
				if (value != this._itemSelected)
				{
					this._itemSelected = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "ItemSelected");
				}
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x000330B7 File Offset: 0x000312B7
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x000330BF File Offset: 0x000312BF
		[DataSourceProperty]
		public string Information
		{
			get
			{
				return this._information;
			}
			set
			{
				if (value != this._information)
				{
					this._information = value;
					base.OnPropertyChangedWithValue<string>(value, "Information");
				}
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x000330E2 File Offset: 0x000312E2
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x000330EA File Offset: 0x000312EA
		[DataSourceProperty]
		public string EquipmentText
		{
			get
			{
				return this._equipmentText;
			}
			set
			{
				if (value != this._equipmentText)
				{
					this._equipmentText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipmentText");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0003310D File Offset: 0x0003130D
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x00033115 File Offset: 0x00031315
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00033138 File Offset: 0x00031338
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x00033140 File Offset: 0x00031340
		[DataSourceProperty]
		public int MovementSpeed
		{
			get
			{
				return this._movementSpeed;
			}
			set
			{
				if (value != this._movementSpeed)
				{
					this._movementSpeed = value;
					base.OnPropertyChangedWithValue(value, "MovementSpeed");
				}
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0003315E File Offset: 0x0003135E
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x00033166 File Offset: 0x00031366
		[DataSourceProperty]
		public int ArmySize
		{
			get
			{
				return this._armySize;
			}
			set
			{
				if (value != this._armySize)
				{
					this._armySize = value;
					base.OnPropertyChangedWithValue(value, "ArmySize");
				}
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x00033184 File Offset: 0x00031384
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x0003318C File Offset: 0x0003138C
		[DataSourceProperty]
		public int HitPoints
		{
			get
			{
				return this._hitPoints;
			}
			set
			{
				if (value != this._hitPoints)
				{
					this._hitPoints = value;
					base.OnPropertyChangedWithValue(value, "HitPoints");
				}
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x000331AA File Offset: 0x000313AA
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x000331B2 File Offset: 0x000313B2
		[DataSourceProperty]
		public int Armor
		{
			get
			{
				return this._armor;
			}
			set
			{
				if (value != this._armor)
				{
					this._armor = value;
					base.OnPropertyChangedWithValue(value, "Armor");
				}
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x000331D0 File Offset: 0x000313D0
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x000331D8 File Offset: 0x000313D8
		[DataSourceProperty]
		public bool IsArmyAvailable
		{
			get
			{
				return this._armyAvailable;
			}
			set
			{
				if (value != this._armyAvailable)
				{
					this._armyAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsArmyAvailable");
				}
			}
		}

		// Token: 0x0400079E RID: 1950
		private const int _defaultNumberOfBotsPerFormation = 25;

		// Token: 0x0400079F RID: 1951
		private TextObject _armySizeHintWithDefaultValue = new TextObject("{=aalbxe7z}Army Size", null);

		// Token: 0x040007A1 RID: 1953
		private ShallowItemVM.ItemGroup _latestSelectedItemGroup;

		// Token: 0x040007A2 RID: 1954
		private HintViewModel _armySizeHint;

		// Token: 0x040007A3 RID: 1955
		private HintViewModel _movementSpeedHint;

		// Token: 0x040007A4 RID: 1956
		private HintViewModel _hitPointsHint;

		// Token: 0x040007A5 RID: 1957
		private HintViewModel _armorHint;

		// Token: 0x040007A6 RID: 1958
		private ShallowItemVM _item1;

		// Token: 0x040007A7 RID: 1959
		private ShallowItemVM _item2;

		// Token: 0x040007A8 RID: 1960
		private ShallowItemVM _item3;

		// Token: 0x040007A9 RID: 1961
		private ShallowItemVM _item4;

		// Token: 0x040007AA RID: 1962
		private ShallowItemVM _itemHorse;

		// Token: 0x040007AB RID: 1963
		private ShallowItemVM _itemSelected;

		// Token: 0x040007AC RID: 1964
		private string _information;

		// Token: 0x040007AD RID: 1965
		private string _nameText;

		// Token: 0x040007AE RID: 1966
		private string _equipmentText;

		// Token: 0x040007AF RID: 1967
		private int _movementSpeed;

		// Token: 0x040007B0 RID: 1968
		private int _hitPoints;

		// Token: 0x040007B1 RID: 1969
		private int _armySize;

		// Token: 0x040007B2 RID: 1970
		private int _armor;

		// Token: 0x040007B3 RID: 1971
		private bool _armyAvailable;
	}
}
