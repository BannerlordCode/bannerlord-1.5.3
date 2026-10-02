using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000B RID: 11
	public class CustomBattleSideVM : ViewModel
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00005EDD File Offset: 0x000040DD
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00005EE5 File Offset: 0x000040E5
		public BasicCharacterObject SelectedCharacter { get; private set; }

		// Token: 0x06000062 RID: 98 RVA: 0x00005EF0 File Offset: 0x000040F0
		public CustomBattleSideVM(TextObject sideName, bool isPlayerSide, TroopTypeSelectionPopUpVM troopTypeSelectionPopUp, Action onCharacterSelected)
		{
			this._sideName = sideName;
			this._isPlayerSide = isPlayerSide;
			this._onCharacterSelected = onCharacterSelected;
			this.CompositionGroup = new ArmyCompositionGroupVM(troopTypeSelectionPopUp);
			this.FactionSelectionGroup = new CustomBattleFactionSelectionVM(new Action<BasicCultureObject>(this.OnCultureSelection));
			this.CharacterSelectionGroup = new SelectorVM<CharacterItemVM>(0, new Action<SelectorVM<CharacterItemVM>>(this.OnCharacterSelection));
			this.ArmorsList = new MBBindingList<CharacterEquipmentItemVM>();
			this.WeaponsList = new MBBindingList<CharacterEquipmentItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005F70 File Offset: 0x00004170
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._sideName.ToString();
			this.FactionText = GameTexts.FindText("str_faction", null).ToString();
			if (this._isPlayerSide)
			{
				this.TitleText = new TextObject("{=bLXleed8}Player Character", null).ToString();
			}
			else
			{
				this.TitleText = new TextObject("{=QAYngoNQ}Enemy Character", null).ToString();
			}
			this.CharacterSelectionGroup.ItemList.Clear();
			foreach (BasicCharacterObject basicCharacterObject in CustomBattleData.Characters)
			{
				this.CharacterSelectionGroup.AddItem(new CharacterItemVM(basicCharacterObject));
			}
			this.CharacterSelectionGroup.SelectedIndex = (this._isPlayerSide ? 0 : 1);
			this.UpdateCharacterVisual();
			Action onCharacterSelected = this._onCharacterSelected;
			if (onCharacterSelected != null)
			{
				onCharacterSelected();
			}
			this.CompositionGroup.RefreshValues();
			this.CharacterSelectionGroup.RefreshValues();
			this.FactionSelectionGroup.RefreshValues();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006088 File Offset: 0x00004288
		public void OnPlayerTypeChange(CustomBattlePlayerType playerType)
		{
			this.CompositionGroup.OnPlayerTypeChange(playerType);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006098 File Offset: 0x00004298
		private void OnCultureSelection(BasicCultureObject selectedCulture)
		{
			this.CompositionGroup.SetCurrentSelectedCulture(selectedCulture);
			if (this.CurrentSelectedCharacter != null)
			{
				this.CurrentSelectedCharacter.ArmorColor1 = selectedCulture.Color;
				this.CurrentSelectedCharacter.ArmorColor2 = selectedCulture.Color2;
				this.CurrentSelectedCharacter.BannerCodeText = selectedCulture.Banner.BannerCode;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000060F4 File Offset: 0x000042F4
		private void OnCharacterSelection(SelectorVM<CharacterItemVM> selector)
		{
			BasicCharacterObject character = selector.SelectedItem.Character;
			this.SelectedCharacter = character;
			this.UpdateCharacterVisual();
			Action onCharacterSelected = this._onCharacterSelected;
			if (onCharacterSelected == null)
			{
				return;
			}
			onCharacterSelected();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000612C File Offset: 0x0000432C
		public void UpdateCharacterVisual()
		{
			this.CurrentSelectedCharacter = new CharacterViewModel(CharacterViewModel.StanceTypes.EmphasizeFace);
			CharacterViewModel currentSelectedCharacter = this.CurrentSelectedCharacter;
			BasicCharacterObject selectedCharacter = this.SelectedCharacter;
			int num = -1;
			CustomBattleFactionSelectionVM factionSelectionGroup = this.FactionSelectionGroup;
			string text;
			if (factionSelectionGroup == null)
			{
				text = null;
			}
			else
			{
				FactionItemVM selectedItem = factionSelectionGroup.SelectedItem;
				text = ((selectedItem != null) ? selectedItem.Faction.Banner.BannerCode : null);
			}
			currentSelectedCharacter.FillFrom(selectedCharacter, num, text);
			CustomBattleFactionSelectionVM factionSelectionGroup2 = this.FactionSelectionGroup;
			if (((factionSelectionGroup2 != null) ? factionSelectionGroup2.SelectedItem : null) != null)
			{
				this.CurrentSelectedCharacter.ArmorColor1 = this.FactionSelectionGroup.SelectedItem.Faction.Color;
				this.CurrentSelectedCharacter.ArmorColor2 = this.FactionSelectionGroup.SelectedItem.Faction.Color2;
			}
			this.ArmorsList.Clear();
			this.ArmorsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.NumAllWeaponSlots].Item));
			this.ArmorsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Cape].Item));
			this.ArmorsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Body].Item));
			this.ArmorsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Gloves].Item));
			this.ArmorsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Leg].Item));
			this.WeaponsList.Clear();
			this.WeaponsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.WeaponItemBeginSlot].Item));
			this.WeaponsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Weapon1].Item));
			this.WeaponsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Weapon2].Item));
			this.WeaponsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.Weapon3].Item));
			this.WeaponsList.Add(new CharacterEquipmentItemVM(this.SelectedCharacter.Equipment[EquipmentIndex.ExtraWeaponSlot].Item));
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00006384 File Offset: 0x00004584
		public void Randomize(CustomBattleSideVM oppositeSide = null)
		{
			this.CharacterSelectionGroup.ExecuteRandomize();
			this.FactionSelectionGroup.ExecuteRandomize();
			this.CompositionGroup.ExecuteRandomize((oppositeSide != null) ? oppositeSide.CompositionGroup : null);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000063B3 File Offset: 0x000045B3
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000063BB File Offset: 0x000045BB
		[DataSourceProperty]
		public CharacterViewModel CurrentSelectedCharacter
		{
			get
			{
				return this._currentSelectedCharacter;
			}
			set
			{
				if (value != this._currentSelectedCharacter)
				{
					this._currentSelectedCharacter = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CurrentSelectedCharacter");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000063D9 File Offset: 0x000045D9
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000063E1 File Offset: 0x000045E1
		[DataSourceProperty]
		public MBBindingList<CharacterEquipmentItemVM> ArmorsList
		{
			get
			{
				return this._armorsList;
			}
			set
			{
				if (value != this._armorsList)
				{
					this._armorsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterEquipmentItemVM>>(value, "ArmorsList");
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000063FF File Offset: 0x000045FF
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00006407 File Offset: 0x00004607
		[DataSourceProperty]
		public MBBindingList<CharacterEquipmentItemVM> WeaponsList
		{
			get
			{
				return this._weaponsList;
			}
			set
			{
				if (value != this._weaponsList)
				{
					this._weaponsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterEquipmentItemVM>>(value, "WeaponsList");
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00006425 File Offset: 0x00004625
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000642D File Offset: 0x0000462D
		[DataSourceProperty]
		public string FactionText
		{
			get
			{
				return this._factionText;
			}
			set
			{
				if (value != this._factionText)
				{
					this._factionText = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionText");
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00006450 File Offset: 0x00004650
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00006458 File Offset: 0x00004658
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000647B File Offset: 0x0000467B
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00006483 File Offset: 0x00004683
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000064A6 File Offset: 0x000046A6
		// (set) Token: 0x06000076 RID: 118 RVA: 0x000064AE File Offset: 0x000046AE
		[DataSourceProperty]
		public SelectorVM<CharacterItemVM> CharacterSelectionGroup
		{
			get
			{
				return this._characterSelectionGroup;
			}
			set
			{
				if (value != this._characterSelectionGroup)
				{
					this._characterSelectionGroup = value;
					base.OnPropertyChangedWithValue<SelectorVM<CharacterItemVM>>(value, "CharacterSelectionGroup");
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000064CC File Offset: 0x000046CC
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000064D4 File Offset: 0x000046D4
		[DataSourceProperty]
		public ArmyCompositionGroupVM CompositionGroup
		{
			get
			{
				return this._compositionGroup;
			}
			set
			{
				if (value != this._compositionGroup)
				{
					this._compositionGroup = value;
					base.OnPropertyChangedWithValue<ArmyCompositionGroupVM>(value, "CompositionGroup");
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000064F2 File Offset: 0x000046F2
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000064FA File Offset: 0x000046FA
		[DataSourceProperty]
		public CustomBattleFactionSelectionVM FactionSelectionGroup
		{
			get
			{
				return this._factionSelectionGroup;
			}
			set
			{
				if (value != this._factionSelectionGroup)
				{
					this._factionSelectionGroup = value;
					base.OnPropertyChangedWithValue<CustomBattleFactionSelectionVM>(value, "FactionSelectionGroup");
				}
			}
		}

		// Token: 0x0400004E RID: 78
		private readonly TextObject _sideName;

		// Token: 0x0400004F RID: 79
		private readonly bool _isPlayerSide;

		// Token: 0x04000050 RID: 80
		private readonly Action _onCharacterSelected;

		// Token: 0x04000051 RID: 81
		private ArmyCompositionGroupVM _compositionGroup;

		// Token: 0x04000052 RID: 82
		private CustomBattleFactionSelectionVM _factionSelectionGroup;

		// Token: 0x04000053 RID: 83
		private SelectorVM<CharacterItemVM> _characterSelectionGroup;

		// Token: 0x04000054 RID: 84
		private CharacterViewModel _currentSelectedCharacter;

		// Token: 0x04000055 RID: 85
		private MBBindingList<CharacterEquipmentItemVM> _armorsList;

		// Token: 0x04000056 RID: 86
		private MBBindingList<CharacterEquipmentItemVM> _weaponsList;

		// Token: 0x04000057 RID: 87
		private string _name;

		// Token: 0x04000058 RID: 88
		private string _factionText;

		// Token: 0x04000059 RID: 89
		private string _titleText;
	}
}
