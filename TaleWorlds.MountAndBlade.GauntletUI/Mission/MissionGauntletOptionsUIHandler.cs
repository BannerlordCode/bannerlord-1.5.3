using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000035 RID: 53
	[OverrideView(typeof(MissionOptionsUIHandler))]
	public class MissionGauntletOptionsUIHandler : MissionView
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		public bool IsEnabled { get; private set; }

		// Token: 0x06000263 RID: 611 RVA: 0x0000DDC1 File Offset: 0x0000BFC1
		public MissionGauntletOptionsUIHandler()
		{
			this.ViewOrderPriority = 49;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.Mission.GetMissionBehavior<MissionOptionsComponent>().OnOptionsAdded += this.OnShowOptions;
			this._keybindingPopup = new KeybindingPopup(new Action<Key>(this.SetHotKey), base.MissionScreen);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000DE20 File Offset: 0x0000C020
		public override void OnMissionScreenFinalize()
		{
			base.Mission.GetMissionBehavior<MissionOptionsComponent>().OnOptionsAdded -= this.OnShowOptions;
			base.OnMissionScreenFinalize();
			OptionsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._movie = null;
			KeybindingPopup keybindingPopup = this._keybindingPopup;
			if (keybindingPopup != null)
			{
				keybindingPopup.OnToggle(false);
			}
			this._keybindingPopup = null;
			this._gauntletLayer = null;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000DE90 File Offset: 0x0000C090
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._gauntletLayer != null && !this._keybindingPopup.IsActive)
			{
				if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					if (this._dataSource.ExposurePopUp.Visible)
					{
						this._dataSource.ExposurePopUp.ExecuteCancel();
					}
					else if (this._dataSource.BrightnessPopUp.Visible)
					{
						this._dataSource.BrightnessPopUp.ExecuteCancel();
					}
					else
					{
						this._dataSource.ExecuteCancel();
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					if (this._dataSource.ExposurePopUp.Visible)
					{
						this._dataSource.ExposurePopUp.ExecuteConfirm();
					}
					else if (this._dataSource.BrightnessPopUp.Visible)
					{
						this._dataSource.BrightnessPopUp.ExecuteConfirm();
					}
					else
					{
						this._dataSource.ExecuteDone();
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("SwitchToPreviousTab"))
				{
					UISoundsHelper.PlayUISound("event:/ui/tab");
					this._dataSource.SelectPreviousCategory();
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("SwitchToNextTab"))
				{
					UISoundsHelper.PlayUISound("event:/ui/tab");
					this._dataSource.SelectNextCategory();
				}
			}
			KeybindingPopup keybindingPopup = this._keybindingPopup;
			if (keybindingPopup == null)
			{
				return;
			}
			keybindingPopup.Tick();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000E021 File Offset: 0x0000C221
		public override bool OnEscape()
		{
			if (this._dataSource != null)
			{
				this._dataSource.ExecuteCloseOptions();
				return true;
			}
			return base.OnEscape();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000E03E File Offset: 0x0000C23E
		private void OnShowOptions()
		{
			this.IsEnabled = true;
			this.OnEscapeMenuToggled(true);
			this._initialClothSimValue = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ClothSimulation) == 0f;
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000E068 File Offset: 0x0000C268
		private void OnCloseOptions()
		{
			this.IsEnabled = false;
			this.OnEscapeMenuToggled(false);
			bool flag = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ClothSimulation) == 0f;
			if (this._initialClothSimValue != flag)
			{
				InformationManager.ShowInquiry(new InquiryData(Module.CurrentModule.GlobalTextManager.FindText("str_option_wont_take_effect_mission_title", null).ToString(), Module.CurrentModule.GlobalTextManager.FindText("str_option_wont_take_effect_mission_desc", null).ToString(), true, false, Module.CurrentModule.GlobalTextManager.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), true, false);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000E110 File Offset: 0x0000C310
		public override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return this._gauntletLayer == null;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000E11C File Offset: 0x0000C31C
		private void OnEscapeMenuToggled(bool isOpened)
		{
			if (isOpened)
			{
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.PauseGameEngine();
				}
			}
			else
			{
				MBCommon.UnPauseGameEngine();
			}
			if (isOpened)
			{
				OptionsVM.OptionsMode optionsMode = (GameNetwork.IsMultiplayer ? OptionsVM.OptionsMode.Multiplayer : OptionsVM.OptionsMode.Singleplayer);
				this._dataSource = new OptionsVM(optionsMode, new Action(this.OnCloseOptions), new Action<KeyOptionVM>(this.OnKeybindRequest), null, null);
				this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
				this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
				this._dataSource.SetPreviousTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"));
				this._dataSource.SetNextTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"));
				this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
				string text = "MissionOptions";
				int num = this.ViewOrderPriority + 1;
				this.ViewOrderPriority = num;
				this._gauntletLayer = new GauntletLayer(text, num, false);
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				this._optionsSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_options");
				this._movie = this._gauntletLayer.LoadMovie("Options", this._dataSource);
				base.MissionScreen.AddLayer(this._gauntletLayer);
				this._gauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._gauntletLayer);
				Game game = Game.Current;
				if (game == null)
				{
					return;
				}
				game.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.OptionsScreen));
				return;
			}
			else
			{
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				KeybindingPopup keybindingPopup = this._keybindingPopup;
				if (keybindingPopup != null)
				{
					keybindingPopup.OnToggle(false);
				}
				this._optionsSpriteCategory.Unload();
				this._gauntletLayer = null;
				this._dataSource.OnFinalize();
				this._dataSource = null;
				this._gauntletLayer = null;
				Game game2 = Game.Current;
				if (game2 == null)
				{
					return;
				}
				game2.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.Mission));
				return;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E35F File Offset: 0x0000C55F
		private void OnKeybindRequest(KeyOptionVM requestedHotKeyToChange)
		{
			this._currentKey = requestedHotKeyToChange;
			this._keybindingPopup.OnToggle(true);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000E374 File Offset: 0x0000C574
		private void SetHotKey(Key key)
		{
			if (key.IsControllerInput)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=B41vvGuo}Invalid key", null), 0, null, null, "");
				this._keybindingPopup.OnToggle(false);
				return;
			}
			GameKeyOptionVM gameKey;
			if ((gameKey = this._currentKey as GameKeyOptionVM) != null)
			{
				if (this._dataSource.GameKeyOptionGroups.GameKeyGroups.FirstOrDefault<GameKeyGroupVM>((GameKeyGroupVM g) => g.GameKeys.Contains(gameKey)) == null)
				{
					Debug.FailedAssert("Could not find GameKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\MissionGauntletOptionsUIHandler.cs", "SetHotKey", 246);
					MBInformationManager.AddQuickInformation(new TextObject("{=oZrVNUOk}Error", null), 0, null, null, "");
					this._keybindingPopup.OnToggle(false);
					return;
				}
				if (this._gauntletLayer.Input.IsHotKeyReleased("Exit") || key.InputKey == gameKey.CurrentKey.InputKey)
				{
					this._keybindingPopup.OnToggle(false);
				}
				else
				{
					GameKeyOptionVM gameKey2 = gameKey;
					if (gameKey2 != null)
					{
						gameKey2.Set(key.InputKey);
					}
					gameKey = null;
					this._keybindingPopup.OnToggle(false);
				}
			}
			else
			{
				AuxiliaryKeyOptionVM auxiliaryKey;
				if ((auxiliaryKey = this._currentKey as AuxiliaryKeyOptionVM) != null)
				{
					if (this._dataSource.GameKeyOptionGroups.AuxiliaryKeyGroups.FirstOrDefault<AuxiliaryKeyGroupVM>((AuxiliaryKeyGroupVM g) => g.HotKeys.Contains(auxiliaryKey)) == null)
					{
						Debug.FailedAssert("Could not find AuxiliaryKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\MissionGauntletOptionsUIHandler.cs", "SetHotKey", 269);
						MBInformationManager.AddQuickInformation(new TextObject("{=oZrVNUOk}Error", null), 0, null, null, "");
						this._keybindingPopup.OnToggle(false);
						return;
					}
					if (this._gauntletLayer.Input.IsHotKeyReleased("Exit") || key.InputKey == auxiliaryKey.CurrentKey.InputKey)
					{
						this._keybindingPopup.OnToggle(false);
					}
					else
					{
						AuxiliaryKeyOptionVM auxiliaryKey2 = auxiliaryKey;
						if (auxiliaryKey2 != null)
						{
							auxiliaryKey2.Set(key.InputKey);
						}
						auxiliaryKey = null;
						this._keybindingPopup.OnToggle(false);
					}
				}
			}
			OptionsVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			GameKeyOptionCategoryVM gameKeyOptionGroups = dataSource.GameKeyOptionGroups;
			if (gameKeyOptionGroups == null)
			{
				return;
			}
			gameKeyOptionGroups.RefreshValues();
		}

		// Token: 0x0400013B RID: 315
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400013C RID: 316
		private OptionsVM _dataSource;

		// Token: 0x0400013D RID: 317
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400013E RID: 318
		private KeybindingPopup _keybindingPopup;

		// Token: 0x0400013F RID: 319
		private KeyOptionVM _currentKey;

		// Token: 0x04000140 RID: 320
		private SpriteCategory _optionsSpriteCategory;

		// Token: 0x04000141 RID: 321
		private bool _initialClothSimValue;
	}
}
