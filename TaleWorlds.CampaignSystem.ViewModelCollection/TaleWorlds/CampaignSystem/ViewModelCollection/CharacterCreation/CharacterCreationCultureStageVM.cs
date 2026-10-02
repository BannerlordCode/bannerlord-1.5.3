using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000153 RID: 339
	public class CharacterCreationCultureStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x060020D6 RID: 8406 RVA: 0x00075CF0 File Offset: 0x00073EF0
		public CharacterCreationCultureStageVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText, Action<CultureObject> onCultureSelected)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this._onCultureSelected = onCultureSelected;
			CharacterCreationContent currentContent = (GameStateManager.Current.ActiveState as CharacterCreationState).CharacterCreationManager.CharacterCreationContent;
			this.Cultures = new MBBindingList<CharacterCreationCultureVM>();
			base.Title = GameTexts.FindText("str_culture", null).ToString();
			base.Description = new TextObject("{=fz2kQjFS}Choose your character's culture:", null).ToString();
			base.SelectionText = new TextObject("{=MaHMOzL2}Character Culture", null).ToString();
			foreach (CultureObject cultureObject in currentContent.GetCultures())
			{
				CharacterCreationCultureVM characterCreationCultureVM = new CharacterCreationCultureVM(cultureObject, new Action<CharacterCreationCultureVM>(this.OnCultureSelection));
				this.Cultures.Add(characterCreationCultureVM);
			}
			this.SortCultureList(this.Cultures);
			if (currentContent.SelectedCulture != null)
			{
				CharacterCreationCultureVM characterCreationCultureVM2 = this.Cultures.FirstOrDefault<CharacterCreationCultureVM>((CharacterCreationCultureVM c) => c.Culture == currentContent.SelectedCulture);
				if (characterCreationCultureVM2 != null)
				{
					this.OnCultureSelection(characterCreationCultureVM2);
				}
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00075E1C File Offset: 0x0007401C
		private void SortCultureList(MBBindingList<CharacterCreationCultureVM> listToWorkOn)
		{
			int num = listToWorkOn.IndexOf(listToWorkOn.Single<CharacterCreationCultureVM>((CharacterCreationCultureVM i) => i.CultureID.Contains("vlan")));
			this.Swap(listToWorkOn, num, 0);
			int num2 = listToWorkOn.IndexOf(listToWorkOn.Single<CharacterCreationCultureVM>((CharacterCreationCultureVM i) => i.CultureID.Contains("stur")));
			this.Swap(listToWorkOn, num2, 1);
			int num3 = listToWorkOn.IndexOf(listToWorkOn.Single<CharacterCreationCultureVM>((CharacterCreationCultureVM i) => i.CultureID.Contains("empi")));
			this.Swap(listToWorkOn, num3, 2);
			int num4 = listToWorkOn.IndexOf(listToWorkOn.Single<CharacterCreationCultureVM>((CharacterCreationCultureVM i) => i.CultureID.Contains("aser")));
			this.Swap(listToWorkOn, num4, 3);
			int num5 = listToWorkOn.IndexOf(listToWorkOn.Single<CharacterCreationCultureVM>((CharacterCreationCultureVM i) => i.CultureID.Contains("khuz")));
			this.Swap(listToWorkOn, num5, 4);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00075F34 File Offset: 0x00074134
		public void OnCultureSelection(CharacterCreationCultureVM selectedCulture)
		{
			this.InitializePlayersFaceKeyAccordingToCultureSelection(selectedCulture);
			foreach (CharacterCreationCultureVM characterCreationCultureVM in this.Cultures.Where<CharacterCreationCultureVM>((CharacterCreationCultureVM c) => c.IsSelected))
			{
				characterCreationCultureVM.IsSelected = false;
			}
			selectedCulture.IsSelected = true;
			this.CurrentSelectedCulture = selectedCulture;
			base.AnyItemSelected = true;
			base.CanAdvance = this.CanAdvanceToNextStage();
			Action<CultureObject> onCultureSelected = this._onCultureSelected;
			if (onCultureSelected == null)
			{
				return;
			}
			onCultureSelected(selectedCulture.Culture);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00075FE4 File Offset: 0x000741E4
		private void InitializePlayersFaceKeyAccordingToCultureSelection(CharacterCreationCultureVM selectedCulture)
		{
			if (selectedCulture.Culture.DefaultCharacterCreationBodyProperty != null)
			{
				CharacterObject.PlayerCharacter.UpdatePlayerCharacterBodyProperties(selectedCulture.Culture.DefaultCharacterCreationBodyProperty.BodyPropertyMax, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
				Hero.MainHero.Culture = selectedCulture.Culture;
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0007603C File Offset: 0x0007423C
		private void Swap(MBBindingList<CharacterCreationCultureVM> listToWorkOn, int swapFromIndex, int swapToIndex)
		{
			if (swapFromIndex != swapToIndex)
			{
				CharacterCreationCultureVM characterCreationCultureVM = listToWorkOn[swapToIndex];
				listToWorkOn[swapToIndex] = listToWorkOn[swapFromIndex];
				listToWorkOn[swapFromIndex] = characterCreationCultureVM;
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x0007606C File Offset: 0x0007426C
		public override void OnNextStage()
		{
			if (this.CurrentSelectedCulture == null)
			{
				Debug.FailedAssert("Selected culture can't be null at this stage", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterCreation\\CharacterCreationCultureStageVM.cs", "OnNextStage", 111);
				return;
			}
			(GameStateManager.Current.ActiveState as CharacterCreationState).CharacterCreationManager.CharacterCreationContent.SetSelectedCulture(this.CurrentSelectedCulture.Culture, this.CharacterCreationManager);
			this._affirmativeAction();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000760D2 File Offset: 0x000742D2
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000760DF File Offset: 0x000742DF
		public override bool CanAdvanceToNextStage()
		{
			return this.Cultures.Any<CharacterCreationCultureVM>((CharacterCreationCultureVM s) => s.IsSelected);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x0007610B File Offset: 0x0007430B
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00076134 File Offset: 0x00074334
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00076143 File Offset: 0x00074343
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060020E1 RID: 8417 RVA: 0x00076152 File Offset: 0x00074352
		// (set) Token: 0x060020E2 RID: 8418 RVA: 0x0007615A File Offset: 0x0007435A
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

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00076178 File Offset: 0x00074378
		// (set) Token: 0x060020E4 RID: 8420 RVA: 0x00076180 File Offset: 0x00074380
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

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x0007619E File Offset: 0x0007439E
		// (set) Token: 0x060020E6 RID: 8422 RVA: 0x000761A6 File Offset: 0x000743A6
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

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x000761C4 File Offset: 0x000743C4
		// (set) Token: 0x060020E8 RID: 8424 RVA: 0x000761CC File Offset: 0x000743CC
		[DataSourceProperty]
		public MBBindingList<CharacterCreationCultureVM> Cultures
		{
			get
			{
				return this._cultures;
			}
			set
			{
				if (value != this._cultures)
				{
					this._cultures = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationCultureVM>>(value, "Cultures");
				}
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060020E9 RID: 8425 RVA: 0x000761EA File Offset: 0x000743EA
		// (set) Token: 0x060020EA RID: 8426 RVA: 0x000761F2 File Offset: 0x000743F2
		[DataSourceProperty]
		public CharacterCreationCultureVM CurrentSelectedCulture
		{
			get
			{
				return this._currentSelectedCulture;
			}
			set
			{
				if (value != this._currentSelectedCulture)
				{
					this._currentSelectedCulture = value;
					base.OnPropertyChangedWithValue<CharacterCreationCultureVM>(value, "CurrentSelectedCulture");
				}
			}
		}

		// Token: 0x04000F08 RID: 3848
		private Action<CultureObject> _onCultureSelected;

		// Token: 0x04000F09 RID: 3849
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000F0A RID: 3850
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F0B RID: 3851
		private bool _isActive;

		// Token: 0x04000F0C RID: 3852
		private MBBindingList<CharacterCreationCultureVM> _cultures;

		// Token: 0x04000F0D RID: 3853
		private CharacterCreationCultureVM _currentSelectedCulture;
	}
}
