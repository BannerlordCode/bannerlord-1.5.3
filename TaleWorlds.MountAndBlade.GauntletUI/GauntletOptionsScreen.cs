using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000015 RID: 21
	[OverrideView(typeof(OptionsScreen))]
	public class GauntletOptionsScreen : ScreenBase
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00005A59 File Offset: 0x00003C59
		public GauntletOptionsScreen(bool isFromMainMenu)
		{
			this._isFromMainMenu = isFromMainMenu;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005A68 File Offset: 0x00003C68
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._optionsSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_options");
			OptionsVM.OptionsMode optionsMode = (this._isFromMainMenu ? OptionsVM.OptionsMode.MainMenu : OptionsVM.OptionsMode.Singleplayer);
			this._dataSource = new OptionsVM(true, optionsMode, new Action<KeyOptionVM>(this.OnKeybindRequest), null, null);
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetPreviousTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"));
			this._dataSource.SetNextTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.ExposurePopUp.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.ExposurePopUp.SetConfirmInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.BrightnessPopUp.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.BrightnessPopUp.SetConfirmInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._gauntletLayer = new GauntletLayer("OptionsScreen", 4000, false);
			this._gauntletMovie = this._gauntletLayer.LoadMovie("Options", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			this._keybindingPopup = new KeybindingPopup(new Action<Key>(this.SetHotKey), this);
			base.AddLayer(this._gauntletLayer);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			if (BannerlordConfig.ForceVSyncInMenus)
			{
				Utilities.SetForceVsync(true);
			}
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.OptionsScreen));
			}
			InformationManager.HideAllMessages();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005CBF File Offset: 0x00003EBF
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
			this._optionsSpriteCategory.Unload();
			Utilities.SetForceVsync(false);
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			game.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005CFD File Offset: 0x00003EFD
		protected override void OnDeactivate()
		{
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005D04 File Offset: 0x00003F04
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
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
			this._keybindingPopup.Tick();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005E90 File Offset: 0x00004090
		private void OnKeybindRequest(KeyOptionVM requestedHotKeyToChange)
		{
			this._currentKey = requestedHotKeyToChange;
			this._keybindingPopup.OnToggle(true);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005EA8 File Offset: 0x000040A8
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
					Debug.FailedAssert("Could not find GameKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletOptionsScreen.cs", "SetHotKey", 169);
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
						Debug.FailedAssert("Could not find AuxiliaryKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletOptionsScreen.cs", "SetHotKey", 192);
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

		// Token: 0x04000071 RID: 113
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000072 RID: 114
		private OptionsVM _dataSource;

		// Token: 0x04000073 RID: 115
		private GauntletMovieIdentifier _gauntletMovie;

		// Token: 0x04000074 RID: 116
		private KeybindingPopup _keybindingPopup;

		// Token: 0x04000075 RID: 117
		private KeyOptionVM _currentKey;

		// Token: 0x04000076 RID: 118
		private SpriteCategory _optionsSpriteCategory;

		// Token: 0x04000077 RID: 119
		private bool _isFromMainMenu;
	}
}
