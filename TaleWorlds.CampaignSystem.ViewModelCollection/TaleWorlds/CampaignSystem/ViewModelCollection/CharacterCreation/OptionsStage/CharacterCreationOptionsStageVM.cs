using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.OptionsStage
{
	// Token: 0x0200015E RID: 350
	public class CharacterCreationOptionsStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x0600218C RID: 8588 RVA: 0x00078150 File Offset: 0x00076350
		public CharacterCreationOptionsStageVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			base.Title = GameTexts.FindText("str_difficulty", null).ToString();
			base.Description = GameTexts.FindText("str_determine_difficulty", null).ToString();
			MBBindingList<CampaignOptionItemVM> mbbindingList = new MBBindingList<CampaignOptionItemVM>();
			List<ICampaignOptionData> characterCreationCampaignOptions = CampaignOptionsManager.GetCharacterCreationCampaignOptions();
			for (int i = 0; i < characterCreationCampaignOptions.Count; i++)
			{
				mbbindingList.Add(new CampaignOptionItemVM(characterCreationCampaignOptions[i]));
			}
			this.OptionsController = new CampaignOptionsControllerVM(mbbindingList);
			base.CanAdvance = this.CanAdvanceToNextStage();
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000781E8 File Offset: 0x000763E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OptionsController.RefreshValues();
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000781FB File Offset: 0x000763FB
		private void OnOptionChange(string identifier)
		{
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000781FD File Offset: 0x000763FD
		public override bool CanAdvanceToNextStage()
		{
			return true;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x00078200 File Offset: 0x00076400
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0007820D File Offset: 0x0007640D
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0007821C File Offset: 0x0007641C
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

		// Token: 0x06002193 RID: 8595 RVA: 0x00078290 File Offset: 0x00076490
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0007829F File Offset: 0x0007649F
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000782B0 File Offset: 0x000764B0
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000782D4 File Offset: 0x000764D4
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000782F8 File Offset: 0x000764F8
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x00078324 File Offset: 0x00076524
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x0007832C File Offset: 0x0007652C
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

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x0007834A File Offset: 0x0007654A
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x00078352 File Offset: 0x00076552
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

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x00078370 File Offset: 0x00076570
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x00078378 File Offset: 0x00076578
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

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x00078396 File Offset: 0x00076596
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x0007839E File Offset: 0x0007659E
		[DataSourceProperty]
		public CampaignOptionsControllerVM OptionsController
		{
			get
			{
				return this._optionsController;
			}
			set
			{
				if (value != this._optionsController)
				{
					this._optionsController = value;
					base.OnPropertyChangedWithValue<CampaignOptionsControllerVM>(value, "OptionsController");
				}
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x000783BC File Offset: 0x000765BC
		// (set) Token: 0x060021A1 RID: 8609 RVA: 0x000783C4 File Offset: 0x000765C4
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

		// Token: 0x04000F56 RID: 3926
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000F57 RID: 3927
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F58 RID: 3928
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000F59 RID: 3929
		private CampaignOptionsControllerVM _optionsController;

		// Token: 0x04000F5A RID: 3930
		private bool _characterGamepadControlsEnabled;
	}
}
