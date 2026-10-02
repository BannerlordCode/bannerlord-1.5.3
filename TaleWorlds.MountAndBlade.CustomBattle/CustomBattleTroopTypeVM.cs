using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000C RID: 12
	public class CustomBattleTroopTypeVM : ViewModel
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00006518 File Offset: 0x00004718
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00006520 File Offset: 0x00004720
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x0600007D RID: 125 RVA: 0x0000652C File Offset: 0x0000472C
		public CustomBattleTroopTypeVM(BasicCharacterObject character, Action<CustomBattleTroopTypeVM> onSelectionToggled, StringItemWithHintVM typeIconData, MBReadOnlyList<SkillObject> allSkills, bool isDefault)
		{
			this.Character = character;
			this.IsDefault = isDefault;
			this._onSelectionToggled = onSelectionToggled;
			this._allSkills = allSkills;
			if (character != null)
			{
				this.Visual = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(character));
				this.NameHint = new HintViewModel(character.Name, null);
				this.TroopSkillsHint = new BasicTooltipViewModel(() => this.GetTroopSkillsTooltip(this.Character));
				this.TierIconData = CustomBattleTroopTypeVM.GetCharacterTierData(this.Character, false);
				this.TypeIconData = typeIconData;
			}
			else
			{
				Debug.FailedAssert("Character shouldn't be null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.CustomBattle\\CustomBattle\\CustomBattleTroopTypeVM.cs", ".ctor", 40);
			}
			this.RefreshValues();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000065D1 File Offset: 0x000047D1
		public override void RefreshValues()
		{
			base.RefreshValues();
			BasicCharacterObject character = this.Character;
			this.Name = ((character != null) ? character.Name.ToString() : null);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000065F6 File Offset: 0x000047F6
		public void ExecuteToggleSelection()
		{
			Action<CustomBattleTroopTypeVM> onSelectionToggled = this._onSelectionToggled;
			if (onSelectionToggled == null)
			{
				return;
			}
			onSelectionToggled(this);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006609 File Offset: 0x00004809
		public void ExecuteRandomize()
		{
			this.IsSelected = MBRandom.RandomInt(2) == 1;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000661C File Offset: 0x0000481C
		private List<TooltipProperty> GetTroopSkillsTooltip(BasicCharacterObject character)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", character.Name.ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.Title));
			list.Add(new TooltipProperty(GameTexts.FindText("str_skills", null).ToString(), " ", 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty("", "", 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
			foreach (SkillObject skillObject in this._allSkills)
			{
				int skillValue = character.GetSkillValue(skillObject);
				if (skillValue > 0)
				{
					list.Add(new TooltipProperty(skillObject.Name.ToString(), skillValue.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000066FC File Offset: 0x000048FC
		public static StringItemWithHintVM GetCharacterTierData(BasicCharacterObject character, bool isBig = false)
		{
			int characterTier = CustomBattleTroopTypeVM.GetCharacterTier(character);
			if (characterTier <= 0 || characterTier > 7)
			{
				return new StringItemWithHintVM("", null);
			}
			string text = (isBig ? (characterTier.ToString() + "_big") : characterTier.ToString());
			string text2 = "General\\TroopTierIcons\\icon_tier_" + text;
			GameTexts.SetVariable("TIER_LEVEL", characterTier);
			TextObject textObject = new TextObject("{=!}" + GameTexts.FindText("str_party_troop_tier", null).ToString(), null);
			return new StringItemWithHintVM(text2, textObject);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000677F File Offset: 0x0000497F
		public static int GetCharacterTier(BasicCharacterObject character)
		{
			if (character.IsHero)
			{
				return 0;
			}
			return MathF.Min(MathF.Max(MathF.Ceiling(((float)character.Level - 5f) / 5f), 0), 7);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000067AF File Offset: 0x000049AF
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000067B7 File Offset: 0x000049B7
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000067D5 File Offset: 0x000049D5
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000067DD File Offset: 0x000049DD
		[DataSourceProperty]
		public BasicTooltipViewModel TroopSkillsHint
		{
			get
			{
				return this._troopSkillsHint;
			}
			set
			{
				if (value != this._troopSkillsHint)
				{
					this._troopSkillsHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TroopSkillsHint");
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000067FB File Offset: 0x000049FB
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00006803 File Offset: 0x00004A03
		[DataSourceProperty]
		public HintViewModel NameHint
		{
			get
			{
				return this._nameHint;
			}
			set
			{
				if (value != this._nameHint)
				{
					this._nameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NameHint");
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00006821 File Offset: 0x00004A21
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00006829 File Offset: 0x00004A29
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00006847 File Offset: 0x00004A47
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000684F File Offset: 0x00004A4F
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000686D File Offset: 0x00004A6D
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00006875 File Offset: 0x00004A75
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00006898 File Offset: 0x00004A98
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000068A0 File Offset: 0x00004AA0
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

		// Token: 0x0400005B RID: 91
		public bool IsDefault;

		// Token: 0x0400005C RID: 92
		private readonly Action<CustomBattleTroopTypeVM> _onSelectionToggled;

		// Token: 0x0400005D RID: 93
		private readonly MBReadOnlyList<SkillObject> _allSkills;

		// Token: 0x0400005E RID: 94
		private CharacterImageIdentifierVM _visual;

		// Token: 0x0400005F RID: 95
		private BasicTooltipViewModel _troopSkillsHint;

		// Token: 0x04000060 RID: 96
		private HintViewModel _nameHint;

		// Token: 0x04000061 RID: 97
		private StringItemWithHintVM _tierIconData;

		// Token: 0x04000062 RID: 98
		private StringItemWithHintVM _typeIconData;

		// Token: 0x04000063 RID: 99
		private string _name;

		// Token: 0x04000064 RID: 100
		private bool _isSelected;
	}
}
