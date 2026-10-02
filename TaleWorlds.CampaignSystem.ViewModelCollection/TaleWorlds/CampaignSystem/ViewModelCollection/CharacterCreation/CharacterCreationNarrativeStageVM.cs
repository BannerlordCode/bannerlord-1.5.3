using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000159 RID: 345
	public class CharacterCreationNarrativeStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x06002127 RID: 8487 RVA: 0x000771BC File Offset: 0x000753BC
		public CharacterCreationNarrativeStageVM(CharacterCreationManager characterCreationManagerMenu, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText, Action onMenuChanged)
			: base(characterCreationManagerMenu, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this._onMenuChanged = onMenuChanged;
			this.SelectionList = new MBBindingList<CharacterCreationOptionVM>();
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			this.GainedPropertiesController = new CharacterCreationGainedPropertiesVM(this.CharacterCreationManager);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0007720C File Offset: 0x0007540C
		public void RefreshMenu()
		{
			this.SelectionList.Clear();
			foreach (NarrativeMenuOption narrativeMenuOption in this.CharacterCreationManager.GetSuitableNarrativeMenuOptions())
			{
				CharacterCreationOptionVM characterCreationOptionVM = new CharacterCreationOptionVM(new Action<CharacterCreationOptionVM>(this.OnOptionSelected), narrativeMenuOption);
				this.SelectionList.Add(characterCreationOptionVM);
			}
			NarrativeMenuOption narrativeMenuOption2;
			if (this.CharacterCreationManager.SelectedOptions.TryGetValue(this.CharacterCreationManager.CurrentMenu, out narrativeMenuOption2))
			{
				for (int i = 0; i < this.SelectionList.Count; i++)
				{
					if (this.SelectionList[i].Option == narrativeMenuOption2)
					{
						this.SelectionList[i].ExecuteSelect();
					}
				}
			}
			base.Title = this.CharacterCreationManager.CurrentMenu.Title.ToString();
			base.Description = this.CharacterCreationManager.CurrentMenu.Description.ToString();
			GameTexts.SetVariable("SELECTION", base.Title);
			base.SelectionText = GameTexts.FindText("str_char_creation_generic_selection", null).ToString();
			base.CanAdvance = this.CanAdvanceToNextStage();
			Action onMenuChanged = this._onMenuChanged;
			if (onMenuChanged == null)
			{
				return;
			}
			onMenuChanged();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0007735C File Offset: 0x0007555C
		public void OnOptionSelected(CharacterCreationOptionVM option)
		{
			if (this.SelectedOption != null)
			{
				this.SelectedOption.IsSelected = false;
			}
			this.SelectedOption = option;
			if (this.SelectedOption != null)
			{
				this.SelectedOption.IsSelected = true;
				this.CharacterCreationManager.OnNarrativeMenuOptionSelected(this._selectedOption.Option);
				this.SelectedOption.RefreshValues();
			}
			Action onOptionSelection = this.OnOptionSelection;
			if (onOptionSelection != null)
			{
				onOptionSelection();
			}
			base.CanAdvance = this.CanAdvanceToNextStage();
			this.GainedPropertiesController.UpdateValues();
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000773E1 File Offset: 0x000755E1
		public override void OnNextStage()
		{
			this.CanAdvanceToNextStage();
			if (this.CharacterCreationManager.TrySwitchToNextMenu())
			{
				this.RefreshMenu();
				return;
			}
			this._affirmativeAction();
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00077409 File Offset: 0x00075609
		public override void OnPreviousStage()
		{
			if (this.CharacterCreationManager.TrySwitchToPreviousMenu())
			{
				this.RefreshMenu();
				return;
			}
			this._negativeAction();
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0007742A File Offset: 0x0007562A
		public override bool CanAdvanceToNextStage()
		{
			if (this.SelectionList.Count != 0)
			{
				return this.SelectionList.Any<CharacterCreationOptionVM>((CharacterCreationOptionVM s) => s.IsSelected);
			}
			return true;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00077465 File Offset: 0x00075665
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

		// Token: 0x0600212E RID: 8494 RVA: 0x0007748E File Offset: 0x0007568E
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0007749D File Offset: 0x0007569D
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000774AC File Offset: 0x000756AC
		// (set) Token: 0x06002131 RID: 8497 RVA: 0x000774B4 File Offset: 0x000756B4
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

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000774D2 File Offset: 0x000756D2
		// (set) Token: 0x06002133 RID: 8499 RVA: 0x000774DA File Offset: 0x000756DA
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

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x000774F8 File Offset: 0x000756F8
		// (set) Token: 0x06002135 RID: 8501 RVA: 0x00077500 File Offset: 0x00075700
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

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x0007751E File Offset: 0x0007571E
		// (set) Token: 0x06002137 RID: 8503 RVA: 0x00077526 File Offset: 0x00075726
		[DataSourceProperty]
		public CharacterCreationOptionVM SelectedOption
		{
			get
			{
				return this._selectedOption;
			}
			set
			{
				if (value != this._selectedOption)
				{
					this._selectedOption = value;
					base.OnPropertyChangedWithValue<CharacterCreationOptionVM>(value, "SelectedOption");
					base.AnyItemSelected = this.SelectedOption != null;
				}
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06002138 RID: 8504 RVA: 0x00077553 File Offset: 0x00075753
		// (set) Token: 0x06002139 RID: 8505 RVA: 0x0007755B File Offset: 0x0007575B
		[DataSourceProperty]
		public MBBindingList<CharacterCreationOptionVM> SelectionList
		{
			get
			{
				return this._selectionList;
			}
			set
			{
				if (value != this._selectionList)
				{
					this._selectionList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationOptionVM>>(value, "SelectionList");
				}
			}
		}

		// Token: 0x04000F29 RID: 3881
		public Action OnOptionSelection;

		// Token: 0x04000F2A RID: 3882
		private readonly Action _onMenuChanged;

		// Token: 0x04000F2B RID: 3883
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000F2C RID: 3884
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F2D RID: 3885
		private CharacterCreationGainedPropertiesVM _gainedPropertiesController;

		// Token: 0x04000F2E RID: 3886
		private CharacterCreationOptionVM _selectedOption;

		// Token: 0x04000F2F RID: 3887
		private MBBindingList<CharacterCreationOptionVM> _selectionList;
	}
}
