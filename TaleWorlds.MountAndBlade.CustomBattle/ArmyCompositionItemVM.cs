using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000008 RID: 8
	public class ArmyCompositionItemVM : ViewModel
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000053F4 File Offset: 0x000035F4
		public ArmyCompositionItemVM(ArmyCompositionItemVM.CompositionType type, List<BasicCharacterObject> allCharacterObjects, MBReadOnlyList<SkillObject> allSkills, Action<int, int> onCompositionValueChanged, TroopTypeSelectionPopUpVM troopTypeSelectionPopUp, int[] compositionValues)
		{
			this._allCharacterObjects = allCharacterObjects;
			this._allSkills = allSkills;
			this._onCompositionValueChanged = onCompositionValueChanged;
			this._troopTypeSelectionPopUp = troopTypeSelectionPopUp;
			this._type = type;
			this._compositionValues = compositionValues;
			this.TroopTypes = new MBBindingList<CustomBattleTroopTypeVM>();
			this.InvalidHint = new HintViewModel(new TextObject("{=iSQTtNUD}This faction doesn't have this troop type.", null), null);
			this.AddTroopTypeHint = new HintViewModel(new TextObject("{=eMbuGGus}Select troops to spawn in formation.", null), null);
			this.UpdatePercentageText(this._compositionValues[(int)this._type]);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00005480 File Offset: 0x00003680
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00005488 File Offset: 0x00003688
		public void SetCurrentSelectedCulture(BasicCultureObject culture)
		{
			this.IsLocked = false;
			this._culture = culture;
			this.PopulateTroopTypes();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000054A0 File Offset: 0x000036A0
		public void ExecuteRandomize(int compositionValue)
		{
			this.IsValid = true;
			this.IsLocked = false;
			this.CompositionValue = compositionValue;
			this.IsValid = this.TroopTypes.Count > 0;
			this.TroopTypes.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.ExecuteRandomize();
			});
			if (!this.TroopTypes.Any<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected) && this.IsValid)
			{
				this.TroopTypes[0].IsSelected = true;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00005548 File Offset: 0x00003748
		public void ExecuteAddTroopTypes()
		{
			string text = GameTexts.FindText("str_custom_battle_choose_troop", this._type.ToString()).ToString();
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this._troopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.OpenPopUp(text, this.TroopTypes);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000558D File Offset: 0x0000378D
		public void RefreshCompositionValue()
		{
			base.OnPropertyChanged("CompositionValue");
			this.UpdatePercentageText(this._compositionValues[(int)this._type]);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000055B0 File Offset: 0x000037B0
		private void UpdatePercentageText(int percentage)
		{
			int num = (int)MathF.Clamp((float)percentage, 0f, 100f);
			this.CompositionValuePercentageText = GameTexts.FindText("str_NUMBER_percent", null).SetTextVariable("NUMBER", num).ToString();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000055F1 File Offset: 0x000037F1
		private void OnValidityChanged(bool value)
		{
			this.IsLocked = false;
			if (!value)
			{
				this.CompositionValue = 0;
			}
			this.IsLocked = !value;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00005610 File Offset: 0x00003810
		private void PopulateTroopTypes()
		{
			this.TroopTypes.Clear();
			MBReadOnlyList<BasicCharacterObject> defaultCharacters = this.GetDefaultCharacters();
			foreach (BasicCharacterObject basicCharacterObject in this._allCharacterObjects)
			{
				if (this.IsValidUnitItem(basicCharacterObject))
				{
					this.TroopTypes.Add(new CustomBattleTroopTypeVM(basicCharacterObject, new Action<CustomBattleTroopTypeVM>(this._troopTypeSelectionPopUp.OnItemSelectionToggled), ArmyCompositionItemVM.GetTroopTypeIconData(basicCharacterObject, this._type, false), this._allSkills, defaultCharacters.Contains(basicCharacterObject)));
				}
			}
			this.IsValid = this.TroopTypes.Count > 0;
			if (this.IsValid)
			{
				if (!this.TroopTypes.Any<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsDefault))
				{
					this.TroopTypes[0].IsDefault = true;
				}
			}
			this.TroopTypes.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.IsSelected = x.IsDefault;
			});
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000573C File Offset: 0x0000393C
		private bool IsValidUnitItem(BasicCharacterObject o)
		{
			if (o == null || this._culture != o.Culture)
			{
				return false;
			}
			switch (this._type)
			{
			case ArmyCompositionItemVM.CompositionType.MeleeInfantry:
				return o.DefaultFormationClass == FormationClass.Infantry || o.DefaultFormationClass == FormationClass.HeavyInfantry;
			case ArmyCompositionItemVM.CompositionType.RangedInfantry:
				return o.DefaultFormationClass == FormationClass.Ranged;
			case ArmyCompositionItemVM.CompositionType.MeleeCavalry:
				return o.DefaultFormationClass == FormationClass.Cavalry || o.DefaultFormationClass == FormationClass.HeavyCavalry || o.DefaultFormationClass == FormationClass.LightCavalry;
			case ArmyCompositionItemVM.CompositionType.RangedCavalry:
				return o.DefaultFormationClass == FormationClass.HorseArcher;
			default:
				return false;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000057C4 File Offset: 0x000039C4
		private MBReadOnlyList<BasicCharacterObject> GetDefaultCharacters()
		{
			MBList<BasicCharacterObject> mblist = new MBList<BasicCharacterObject>();
			FormationClass formationClass = FormationClass.NumberOfAllFormations;
			switch (this._type)
			{
			case ArmyCompositionItemVM.CompositionType.MeleeInfantry:
				formationClass = FormationClass.Infantry;
				break;
			case ArmyCompositionItemVM.CompositionType.RangedInfantry:
				formationClass = FormationClass.Ranged;
				break;
			case ArmyCompositionItemVM.CompositionType.MeleeCavalry:
				formationClass = FormationClass.Cavalry;
				break;
			case ArmyCompositionItemVM.CompositionType.RangedCavalry:
				formationClass = FormationClass.HorseArcher;
				break;
			}
			mblist.Add(CustomBattleHelper.GetDefaultTroopOfFormationForFaction(this._culture, formationClass));
			return mblist;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000581C File Offset: 0x00003A1C
		public static StringItemWithHintVM GetTroopTypeIconData(BasicCharacterObject basicCharacterObject, ArmyCompositionItemVM.CompositionType type, bool isBig = false)
		{
			bool flag = false;
			if (basicCharacterObject != null)
			{
				flag = basicCharacterObject.StringId.Contains("marine") || basicCharacterObject.Culture.StringId.Contains("nord");
			}
			TextObject textObject = new TextObject("{=!}{TYPENAME}{MARINER}{BIG}", null);
			TextObject textObject2;
			switch (type)
			{
			case ArmyCompositionItemVM.CompositionType.MeleeInfantry:
			{
				textObject.SetTextVariable("TYPENAME", "infantry");
				string text = (flag ? "Infantry_Mariner" : "Infantry");
				textObject2 = GameTexts.FindText("str_troop_type_name", text);
				break;
			}
			case ArmyCompositionItemVM.CompositionType.RangedInfantry:
			{
				textObject.SetTextVariable("TYPENAME", "bow");
				string text2 = (flag ? "Ranged_Mariner" : "Ranged");
				textObject2 = GameTexts.FindText("str_troop_type_name", text2);
				break;
			}
			case ArmyCompositionItemVM.CompositionType.MeleeCavalry:
				textObject.SetTextVariable("TYPENAME", "cavalry");
				textObject2 = GameTexts.FindText("str_troop_type_name", "Cavalry");
				break;
			case ArmyCompositionItemVM.CompositionType.RangedCavalry:
				textObject.SetTextVariable("TYPENAME", "horse_archer");
				textObject2 = GameTexts.FindText("str_troop_type_name", "HorseArcher");
				break;
			default:
				return new StringItemWithHintVM("", null);
			}
			textObject.SetTextVariable("MARINER", flag ? "_mariner" : "");
			textObject.SetTextVariable("BIG", isBig ? "_big" : "");
			return new StringItemWithHintVM("General\\TroopTypeIcons\\icon_troop_type_" + textObject.ToString(), new TextObject("{=!}" + textObject2.ToString(), null));
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00005996 File Offset: 0x00003B96
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000599E File Offset: 0x00003B9E
		[DataSourceProperty]
		public MBBindingList<CustomBattleTroopTypeVM> TroopTypes
		{
			get
			{
				return this._troopTypes;
			}
			set
			{
				if (value != this._troopTypes)
				{
					this._troopTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleTroopTypeVM>>(value, "TroopTypes");
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000059BC File Offset: 0x00003BBC
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000059C4 File Offset: 0x00003BC4
		[DataSourceProperty]
		public HintViewModel InvalidHint
		{
			get
			{
				return this._invalidHint;
			}
			set
			{
				if (value != this._invalidHint)
				{
					this._invalidHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InvalidHint");
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000059E2 File Offset: 0x00003BE2
		// (set) Token: 0x06000048 RID: 72 RVA: 0x000059EA File Offset: 0x00003BEA
		[DataSourceProperty]
		public HintViewModel AddTroopTypeHint
		{
			get
			{
				return this._addTroopTypeHint;
			}
			set
			{
				if (value != this._addTroopTypeHint)
				{
					this._addTroopTypeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddTroopTypeHint");
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00005A08 File Offset: 0x00003C08
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00005A10 File Offset: 0x00003C10
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
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00005A2E File Offset: 0x00003C2E
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00005A36 File Offset: 0x00003C36
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
				this.OnValidityChanged(value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00005A5B File Offset: 0x00003C5B
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00005A6A File Offset: 0x00003C6A
		[DataSourceProperty]
		public int CompositionValue
		{
			get
			{
				return this._compositionValues[(int)this._type];
			}
			set
			{
				if (value != this._compositionValues[(int)this._type])
				{
					this._onCompositionValueChanged(value, (int)this._type);
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00005A8E File Offset: 0x00003C8E
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00005A96 File Offset: 0x00003C96
		[DataSourceProperty]
		public string CompositionValuePercentageText
		{
			get
			{
				return this._compositionValuePercentageText;
			}
			set
			{
				if (value != this._compositionValuePercentageText)
				{
					this._compositionValuePercentageText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompositionValuePercentageText");
				}
			}
		}

		// Token: 0x0400003A RID: 58
		private readonly MBReadOnlyList<SkillObject> _allSkills;

		// Token: 0x0400003B RID: 59
		private readonly List<BasicCharacterObject> _allCharacterObjects;

		// Token: 0x0400003C RID: 60
		private readonly Action<int, int> _onCompositionValueChanged;

		// Token: 0x0400003D RID: 61
		private readonly TroopTypeSelectionPopUpVM _troopTypeSelectionPopUp;

		// Token: 0x0400003E RID: 62
		private BasicCultureObject _culture;

		// Token: 0x0400003F RID: 63
		private readonly ArmyCompositionItemVM.CompositionType _type;

		// Token: 0x04000040 RID: 64
		private readonly int[] _compositionValues;

		// Token: 0x04000041 RID: 65
		private MBBindingList<CustomBattleTroopTypeVM> _troopTypes;

		// Token: 0x04000042 RID: 66
		private HintViewModel _invalidHint;

		// Token: 0x04000043 RID: 67
		private HintViewModel _addTroopTypeHint;

		// Token: 0x04000044 RID: 68
		private bool _isLocked;

		// Token: 0x04000045 RID: 69
		private bool _isValid;

		// Token: 0x04000046 RID: 70
		private string _compositionValuePercentageText;

		// Token: 0x02000031 RID: 49
		public enum CompositionType
		{
			// Token: 0x04000149 RID: 329
			MeleeInfantry,
			// Token: 0x0400014A RID: 330
			RangedInfantry,
			// Token: 0x0400014B RID: 331
			MeleeCavalry,
			// Token: 0x0400014C RID: 332
			RangedCavalry
		}
	}
}
