using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200015C RID: 348
	public class CharacterCreationReviewStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x06002153 RID: 8531 RVA: 0x000777D8 File Offset: 0x000759D8
		public CharacterCreationReviewStageVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText, bool isBannerAndClanNameSet)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this.ReviewList = new MBBindingList<CharacterCreationReviewStageItemVM>();
			base.Title = new TextObject("{=txjiykNa}Review", null).ToString();
			base.Description = characterCreationManager.CharacterCreationContent.ReviewPageDescription.ToString();
			this._isBannerAndClanNameSet = isBannerAndClanNameSet;
			this.CannotAdvanceReasonHint = new HintViewModel();
			this.ClanBanner = new BannerImageIdentifierVM(Clan.PlayerClan.Banner, false);
			this.GainedPropertiesController = new CharacterCreationGainedPropertiesVM(this.CharacterCreationManager);
			this.Name = characterCreationManager.CharacterCreationContent.MainCharacterName;
			this.NameTextQuestion = new TextObject("{=mHVmrwRQ}Enter your name", null).ToString();
			this.AddReviewedItems();
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000778B4 File Offset: 0x00075AB4
		private void AddReviewedItems()
		{
			string text = string.Empty;
			CultureObject selectedCulture = this.CharacterCreationManager.CharacterCreationContent.SelectedCulture;
			IEnumerable<FeatObject> culturalFeats = selectedCulture.GetCulturalFeats((FeatObject x) => x.IsPositive);
			IEnumerable<FeatObject> culturalFeats2 = selectedCulture.GetCulturalFeats((FeatObject x) => !x.IsPositive);
			foreach (FeatObject featObject in culturalFeats)
			{
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", featObject.Description);
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			foreach (FeatObject featObject2 in culturalFeats2)
			{
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", featObject2.Description);
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			CharacterCreationReviewStageItemVM characterCreationReviewStageItemVM = new CharacterCreationReviewStageItemVM(new TextObject("{=K6GYskvJ}Culture:", null).ToString(), this.CharacterCreationManager.CharacterCreationContent.SelectedCulture.Name.ToString(), text);
			this.ReviewList.Add(characterCreationReviewStageItemVM);
			foreach (KeyValuePair<NarrativeMenu, NarrativeMenuOption> keyValuePair in this.CharacterCreationManager.SelectedOptions)
			{
				NarrativeMenu key = keyValuePair.Key;
				NarrativeMenuOption value = keyValuePair.Value;
				characterCreationReviewStageItemVM = new CharacterCreationReviewStageItemVM(key.Title.ToString(), value.Text.ToString(), value.PositiveEffectText.ToString());
				this.ReviewList.Add(characterCreationReviewStageItemVM);
			}
			if (this._isBannerAndClanNameSet)
			{
				CharacterCreationReviewStageItemVM characterCreationReviewStageItemVM2 = new CharacterCreationReviewStageItemVM(new BannerImageIdentifierVM(Clan.PlayerClan.Banner, true), GameTexts.FindText("str_clan", null).ToString(), Clan.PlayerClan.Name.ToString(), null);
				this.ReviewList.Add(characterCreationReviewStageItemVM2);
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00077AFC File Offset: 0x00075CFC
		public void ExecuteRandomizeName()
		{
			this.Name = NameGenerator.Current.GenerateFirstNameForPlayer(this.CharacterCreationManager.CharacterCreationContent.SelectedCulture, Hero.MainHero.IsFemale).ToString();
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00077B30 File Offset: 0x00075D30
		private void OnRefresh()
		{
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(this.Name, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(this.Name, null));
			textObject2.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			textObject.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			Hero.MainHero.SetName(textObject2, textObject);
			base.CanAdvance = this.CanAdvanceToNextStage();
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00077BD5 File Offset: 0x00075DD5
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00077BE2 File Offset: 0x00075DE2
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00077BF0 File Offset: 0x00075DF0
		public override bool CanAdvanceToNextStage()
		{
			TextObject textObject = TextObject.GetEmpty();
			bool flag = true;
			if (string.IsNullOrEmpty(this.Name) || string.IsNullOrWhiteSpace(this.Name))
			{
				textObject = new TextObject("{=IRcy3pWJ}Name cannot be empty", null);
				flag = false;
			}
			Tuple<bool, string> tuple = CampaignUIHelper.IsStringApplicableForHeroName(this.Name);
			if (!tuple.Item1)
			{
				if (!string.IsNullOrEmpty(tuple.Item2))
				{
					textObject = new TextObject("{=!}" + tuple.Item2, null);
				}
				flag = false;
			}
			this.CannotAdvanceReasonHint.HintText = textObject;
			return flag;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00077C74 File Offset: 0x00075E74
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			foreach (InputKeyItemVM inputKeyItemVM in this.CameraControlKeys)
			{
				inputKeyItemVM.OnFinalize();
			}
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00077CE8 File Offset: 0x00075EE8
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00077CF7 File Offset: 0x00075EF7
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00077D08 File Offset: 0x00075F08
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x00077D2C File Offset: 0x00075F2C
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00077D50 File Offset: 0x00075F50
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x00077D7C File Offset: 0x00075F7C
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x00077D84 File Offset: 0x00075F84
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x00077DA2 File Offset: 0x00075FA2
		// (set) Token: 0x06002163 RID: 8547 RVA: 0x00077DAA File Offset: 0x00075FAA
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x00077DC8 File Offset: 0x00075FC8
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x00077DD0 File Offset: 0x00075FD0
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> CameraControlKeys
		{
			get
			{
				return this._cameraControlKeys;
			}
			set
			{
				if (value != this._cameraControlKeys)
				{
					this._cameraControlKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "CameraControlKeys");
				}
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x00077DEE File Offset: 0x00075FEE
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x00077DF6 File Offset: 0x00075FF6
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
					this.CharacterCreationManager.CharacterCreationContent.SetMainCharacterName(value);
					base.OnPropertyChangedWithValue<string>(value, "Name");
					this.OnRefresh();
				}
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x00077E30 File Offset: 0x00076030
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x00077E38 File Offset: 0x00076038
		[DataSourceProperty]
		public string NameTextQuestion
		{
			get
			{
				return this._nameTextQuestion;
			}
			set
			{
				if (value != this._nameTextQuestion)
				{
					this._nameTextQuestion = value;
					base.OnPropertyChangedWithValue<string>(value, "NameTextQuestion");
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x00077E5B File Offset: 0x0007605B
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x00077E63 File Offset: 0x00076063
		[DataSourceProperty]
		public MBBindingList<CharacterCreationReviewStageItemVM> ReviewList
		{
			get
			{
				return this._reviewList;
			}
			set
			{
				if (value != this._reviewList)
				{
					this._reviewList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationReviewStageItemVM>>(value, "ReviewList");
				}
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00077E81 File Offset: 0x00076081
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x00077E89 File Offset: 0x00076089
		[DataSourceProperty]
		public CharacterCreationGainedPropertiesVM GainedPropertiesController
		{
			get
			{
				return this._gainedPropertiesController;
			}
			set
			{
				if (value != this._gainedPropertiesController)
				{
					this._gainedPropertiesController = value;
					base.OnPropertyChangedWithValue<CharacterCreationGainedPropertiesVM>(value, "GainedPropertiesController");
				}
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x00077EA7 File Offset: 0x000760A7
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x00077EAF File Offset: 0x000760AF
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x00077ECD File Offset: 0x000760CD
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x00077ED5 File Offset: 0x000760D5
		[DataSourceProperty]
		public HintViewModel CannotAdvanceReasonHint
		{
			get
			{
				return this._cannotAdvanceReasonHint;
			}
			set
			{
				if (value != this._cannotAdvanceReasonHint)
				{
					this._cannotAdvanceReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CannotAdvanceReasonHint");
				}
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00077EF3 File Offset: 0x000760F3
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x00077EFB File Offset: 0x000760FB
		[DataSourceProperty]
		public bool CharacterGamepadControlsEnabled
		{
			get
			{
				return this._characterGamepadControlsEnabled;
			}
			set
			{
				if (value != this._characterGamepadControlsEnabled)
				{
					this._characterGamepadControlsEnabled = value;
					base.OnPropertyChangedWithValue(value, "CharacterGamepadControlsEnabled");
				}
			}
		}

		// Token: 0x04000F3C RID: 3900
		private bool _isBannerAndClanNameSet;

		// Token: 0x04000F3D RID: 3901
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000F3E RID: 3902
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F3F RID: 3903
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000F40 RID: 3904
		private string _name = "";

		// Token: 0x04000F41 RID: 3905
		private string _nameTextQuestion = "";

		// Token: 0x04000F42 RID: 3906
		private MBBindingList<CharacterCreationReviewStageItemVM> _reviewList;

		// Token: 0x04000F43 RID: 3907
		private CharacterCreationGainedPropertiesVM _gainedPropertiesController;

		// Token: 0x04000F44 RID: 3908
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000F45 RID: 3909
		private HintViewModel _cannotAdvanceReasonHint;

		// Token: 0x04000F46 RID: 3910
		private bool _characterGamepadControlsEnabled;
	}
}
