using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B8 RID: 184
	public class RecruitVolunteerTroopVM : ViewModel
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x00046A9C File Offset: 0x00044C9C
		public RecruitVolunteerTroopVM(RecruitVolunteerVM owner, CharacterObject character, int index, Action<RecruitVolunteerTroopVM> onClick, Action<RecruitVolunteerTroopVM> onRemoveFromCart)
		{
			if (character != null)
			{
				this.NameText = character.Name.ToString();
				this._character = character;
				GameTexts.SetVariable("LEVEL", character.Level);
				this.Level = GameTexts.FindText("str_level_with_value", null).ToString();
				this.Character = character;
				this.Wage = this.Character.TroopWage;
				this.Cost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.Character, Hero.MainHero, false).RoundedResultNumber;
				this.IsTroopEmpty = false;
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(character, false);
				this.ImageIdentifier = new CharacterImageIdentifierVM(characterCode);
				this.TierIconData = CampaignUIHelper.GetCharacterTierData(character, false);
				this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(character, false);
			}
			else
			{
				this.IsTroopEmpty = true;
			}
			this.Owner = owner;
			if (this.Owner != null)
			{
				this._currentRelation = Hero.MainHero.GetRelation(this.Owner.OwnerHero);
			}
			this._maximumIndexCanBeRecruit = Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(Hero.MainHero, this.Owner.OwnerHero, -101);
			for (int i = -100; i < 100; i++)
			{
				if (index < Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(Hero.MainHero, this.Owner.OwnerHero, i))
				{
					this._requiredRelation = i;
					break;
				}
			}
			this._onClick = onClick;
			this.Index = index;
			this._onRemoveFromCart = onRemoveFromCart;
			this.RefreshValues();
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00046C2C File Offset: 0x00044E2C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._character != null)
			{
				this.NameText = this._character.Name.ToString();
				GameTexts.SetVariable("LEVEL", this._character.Level);
				this.Level = GameTexts.FindText("str_level_with_value", null).ToString();
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00046C88 File Offset: 0x00044E88
		public void ExecuteRecruit()
		{
			if (this.CanBeRecruited)
			{
				this._onClick(this);
				return;
			}
			if (this.IsInCart)
			{
				this._onRemoveFromCart(this);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00046CB3 File Offset: 0x00044EB3
		public void ExecuteOpenEncyclopedia()
		{
			if (this.Character != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Character.EncyclopediaLink);
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00046CD7 File Offset: 0x00044ED7
		public void ExecuteRemoveFromCart()
		{
			if (this.IsInCart)
			{
				this._onRemoveFromCart(this);
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00046CF0 File Offset: 0x00044EF0
		public virtual void ExecuteBeginHint()
		{
			if (this._character != null)
			{
				if (this.PlayerHasEnoughRelation)
				{
					InformationManager.ShowTooltip(typeof(CharacterObject), new object[] { this._character });
					return;
				}
				List<TooltipProperty> list = new List<TooltipProperty>();
				string text = "";
				list.Add(new TooltipProperty(text, this._character.Name.ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.None));
				list.Add(new TooltipProperty(text, text, -1, false, TooltipProperty.TooltipPropertyFlags.None));
				GameTexts.SetVariable("LEVEL", this._character.Level);
				GameTexts.SetVariable("newline", "\n");
				list.Add(new TooltipProperty(text, GameTexts.FindText("str_level_with_value", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				GameTexts.SetVariable("REL1", this._currentRelation);
				GameTexts.SetVariable("REL2", this._requiredRelation);
				list.Add(new TooltipProperty(text, GameTexts.FindText("str_recruit_volunteers_not_enough_relation", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { list });
				return;
			}
			else
			{
				if (this.PlayerHasEnoughRelation)
				{
					MBInformationManager.ShowHint(GameTexts.FindText("str_recruit_volunteers_new_troop", null).ToString());
					return;
				}
				GameTexts.SetVariable("newline", "\n");
				GameTexts.SetVariable("REL1", this._currentRelation);
				GameTexts.SetVariable("REL2", this._requiredRelation);
				GameTexts.SetVariable("STR1", GameTexts.FindText("str_recruit_volunteers_new_troop", null));
				GameTexts.SetVariable("STR2", GameTexts.FindText("str_recruit_volunteers_not_enough_relation", null));
				MBInformationManager.ShowHint(GameTexts.FindText("str_string_newline_string", null).ToString());
				return;
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00046E92 File Offset: 0x00045092
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00046E99 File Offset: 0x00045099
		public void ExecuteFocus()
		{
			if (!this.IsTroopEmpty)
			{
				Action<RecruitVolunteerTroopVM> onFocused = RecruitVolunteerTroopVM.OnFocused;
				if (onFocused == null)
				{
					return;
				}
				onFocused(this);
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00046EB3 File Offset: 0x000450B3
		public void ExecuteUnfocus()
		{
			Action<RecruitVolunteerTroopVM> onFocused = RecruitVolunteerTroopVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(null);
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00046EC5 File Offset: 0x000450C5
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x00046ECD File Offset: 0x000450CD
		[DataSourceProperty]
		public string Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue<string>(value, "Level");
				}
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x00046EF0 File Offset: 0x000450F0
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00046EF8 File Offset: 0x000450F8
		[DataSourceProperty]
		public bool CanBeRecruited
		{
			get
			{
				return this._canBeRecruited;
			}
			set
			{
				if (value != this._canBeRecruited)
				{
					this._canBeRecruited = value;
					base.OnPropertyChangedWithValue(value, "CanBeRecruited");
				}
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00046F16 File Offset: 0x00045116
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x00046F1E File Offset: 0x0004511E
		[DataSourceProperty]
		public bool IsHiglightEnabled
		{
			get
			{
				return this._isHiglightEnabled;
			}
			set
			{
				if (value != this._isHiglightEnabled)
				{
					this._isHiglightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHiglightEnabled");
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x00046F3C File Offset: 0x0004513C
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x00046F44 File Offset: 0x00045144
		[DataSourceProperty]
		public int Wage
		{
			get
			{
				return this._wage;
			}
			set
			{
				if (value != this._wage)
				{
					this._wage = value;
					base.OnPropertyChangedWithValue(value, "Wage");
				}
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00046F62 File Offset: 0x00045162
		// (set) Token: 0x060011B5 RID: 4533 RVA: 0x00046F6A File Offset: 0x0004516A
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00046F88 File Offset: 0x00045188
		// (set) Token: 0x060011B7 RID: 4535 RVA: 0x00046F90 File Offset: 0x00045190
		[DataSourceProperty]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (value != this._isInCart)
				{
					this._isInCart = value;
					base.OnPropertyChangedWithValue(value, "IsInCart");
				}
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00046FAE File Offset: 0x000451AE
		// (set) Token: 0x060011B9 RID: 4537 RVA: 0x00046FB6 File Offset: 0x000451B6
		[DataSourceProperty]
		public bool IsTroopEmpty
		{
			get
			{
				return this._isTroopEmpty;
			}
			set
			{
				if (value != this._isTroopEmpty)
				{
					this._isTroopEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsTroopEmpty");
				}
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00046FD4 File Offset: 0x000451D4
		// (set) Token: 0x060011BB RID: 4539 RVA: 0x00046FDC File Offset: 0x000451DC
		[DataSourceProperty]
		public bool PlayerHasEnoughRelation
		{
			get
			{
				return this._playerHasEnoughRelation;
			}
			set
			{
				if (value != this._playerHasEnoughRelation)
				{
					this._playerHasEnoughRelation = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasEnoughRelation");
				}
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x00046FFA File Offset: 0x000451FA
		// (set) Token: 0x060011BD RID: 4541 RVA: 0x00047002 File Offset: 0x00045202
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00047020 File Offset: 0x00045220
		// (set) Token: 0x060011BF RID: 4543 RVA: 0x00047028 File Offset: 0x00045228
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

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x0004704B File Offset: 0x0004524B
		// (set) Token: 0x060011C1 RID: 4545 RVA: 0x00047053 File Offset: 0x00045253
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

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00047071 File Offset: 0x00045271
		// (set) Token: 0x060011C3 RID: 4547 RVA: 0x00047079 File Offset: 0x00045279
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

		// Token: 0x040007FD RID: 2045
		public static Action<RecruitVolunteerTroopVM> OnFocused;

		// Token: 0x040007FE RID: 2046
		private readonly Action<RecruitVolunteerTroopVM> _onClick;

		// Token: 0x040007FF RID: 2047
		private readonly Action<RecruitVolunteerTroopVM> _onRemoveFromCart;

		// Token: 0x04000800 RID: 2048
		private CharacterObject _character;

		// Token: 0x04000801 RID: 2049
		public CharacterObject Character;

		// Token: 0x04000802 RID: 2050
		public int Index;

		// Token: 0x04000803 RID: 2051
		private int _maximumIndexCanBeRecruit;

		// Token: 0x04000804 RID: 2052
		private int _requiredRelation;

		// Token: 0x04000805 RID: 2053
		public RecruitVolunteerVM Owner;

		// Token: 0x04000806 RID: 2054
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x04000807 RID: 2055
		private string _nameText;

		// Token: 0x04000808 RID: 2056
		private string _level;

		// Token: 0x04000809 RID: 2057
		private bool _canBeRecruited;

		// Token: 0x0400080A RID: 2058
		private bool _isInCart;

		// Token: 0x0400080B RID: 2059
		private int _wage;

		// Token: 0x0400080C RID: 2060
		private int _cost;

		// Token: 0x0400080D RID: 2061
		private bool _isTroopEmpty;

		// Token: 0x0400080E RID: 2062
		private bool _playerHasEnoughRelation;

		// Token: 0x0400080F RID: 2063
		private int _currentRelation;

		// Token: 0x04000810 RID: 2064
		private bool _isHiglightEnabled;

		// Token: 0x04000811 RID: 2065
		private StringItemWithHintVM _tierIconData;

		// Token: 0x04000812 RID: 2066
		private StringItemWithHintVM _typeIconData;
	}
}
