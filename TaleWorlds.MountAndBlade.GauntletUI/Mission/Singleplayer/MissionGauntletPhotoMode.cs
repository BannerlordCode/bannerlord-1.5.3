using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003C RID: 60
	[OverrideView(typeof(PhotoModeView))]
	public class MissionGauntletPhotoMode : MissionView
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0001038A File Offset: 0x0000E58A
		private Scene _missionScene
		{
			get
			{
				return base.MissionScreen.Mission.Scene;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0001039C File Offset: 0x0000E59C
		private InputContext _input
		{
			get
			{
				return base.MissionScreen.SceneLayer.Input;
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000103B0 File Offset: 0x0000E5B0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._photoModeCategory = UIResourceManager.LoadSpriteCategory("ui_photomode");
			this._dataSource = new PhotoModeVM(this._missionScene, () => this._vignetteMode, () => this._hideAgentsMode);
			this._cameraRoll = 0f;
			this._photoModeOrbitState = this._missionScene.GetPhotoModeOrbit();
			this._vignetteMode = false;
			this._hideAgentsMode = false;
			this._saveAmbientOcclusionPass = false;
			this._saveObjectIdPass = false;
			this._saveShadowPass = false;
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(107));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(93));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(94));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(100));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(99));
			if (this._missionScene.ContainsTerrain)
			{
				this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(97));
			}
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(98));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(92));
			if (Utilities.EditModeEnabled)
			{
				this._dataSource.AddKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(96));
			}
			this._dataSource.AddTakePictureKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetGameKey(95));
			this._dataSource.AddFasterCameraKey(HotKeyManager.GetCategory("PhotoModeHotKeyCategory").GetHotKey("FasterCamera"));
			this._dataSource.AddHotkeyWithForcedName(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("ToggleEscapeMenu"), new TextObject("{=exitMenuOption}Exit", null));
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000105B0 File Offset: 0x0000E7B0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._takePhoto != -1)
			{
				if (Utilities.GetNumberOfShaderCompilationsInProgress() > 0)
				{
					this._takePhoto++;
				}
				else if (this._takePhoto > 6)
				{
					if (this._saveObjectIdPass)
					{
						string text = this._missionScene.TakePhotoModePicture(false, true, false);
						MBDebug.DisableAllUI = this._prevUIDisabled;
						this._screenShotTakenMessage.SetTextVariable("PATH", text);
						InformationManager.DisplayMessage(new InformationMessage(this._screenShotTakenMessage.ToString()));
						Utilities.SetForceDrawEntityID(false);
						Utilities.SetRenderMode(Utilities.EngineRenderDisplayMode.ShowNone);
					}
					this._takePhoto = -1;
				}
				else if (this._takePhoto == 2)
				{
					string text2 = this._missionScene.TakePhotoModePicture(this._saveAmbientOcclusionPass, false, this._saveShadowPass);
					this._screenShotTakenMessage.SetTextVariable("PATH", text2);
					InformationManager.DisplayMessage(new InformationMessage(this._screenShotTakenMessage.ToString()));
					if (this._saveObjectIdPass)
					{
						Utilities.SetForceDrawEntityID(true);
						Utilities.SetRenderMode(Utilities.EngineRenderDisplayMode.ShowMeshId);
						this._takePhoto++;
					}
					else
					{
						MBDebug.DisableAllUI = this._prevUIDisabled;
						this._takePhoto = -1;
					}
				}
				else
				{
					this._takePhoto++;
				}
			}
			if (base.MissionScreen.IsPhotoModeEnabled)
			{
				this._dataSource.UpdateTakePictureKeyVisibility(this.GetCanTakePicture());
				this._dataSource.UpdateFasterCameraKeyVisibility(this.GetCanMoveCamera());
				if (this._takePhoto == -1)
				{
					if (!this._registered)
					{
						GameKeyContext category = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
						if (!this._input.IsCategoryRegistered(category))
						{
							this._input.RegisterHotKeyCategory(category);
						}
						GameKeyContext category2 = HotKeyManager.GetCategory("PhotoModeHotKeyCategory");
						if (!this._input.IsCategoryRegistered(category2))
						{
							this._input.RegisterHotKeyCategory(category2);
						}
						this._registered = true;
					}
					if (this._suspended)
					{
						this._suspended = false;
						this._gauntletLayer = new GauntletLayer("MissionPhotoMode", 100000, false);
						this._dataSource.RefreshValues();
						this._gauntletLayer.LoadMovie("PhotoMode", this._dataSource);
						this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Mouse);
						base.MissionScreen.AddLayer(this._gauntletLayer);
						GauntletChatLogView.Current.SetCanFocusWhileInMission(false);
					}
					if (this._input.IsGameKeyPressed(95) && this.GetCanTakePicture())
					{
						this._prevUIDisabled = MBDebug.DisableAllUI;
						MBDebug.DisableAllUI = true;
						this._saveAmbientOcclusionPass = false;
						this._saveObjectIdPass = false;
						this._saveShadowPass = false;
						this._takePhoto = 0;
					}
					else if (Utilities.EditModeEnabled && this._input.IsGameKeyPressed(96))
					{
						this._prevUIDisabled = MBDebug.DisableAllUI;
						MBDebug.DisableAllUI = true;
						this._saveAmbientOcclusionPass = true;
						this._saveObjectIdPass = Utilities.EditModeEnabled;
						this._saveShadowPass = true;
						this._takePhoto = 0;
					}
					else if (this._input.IsGameKeyPressed(92))
					{
						MBDebug.DisableAllUI = !MBDebug.DisableAllUI;
					}
					else if (this._input.IsGameKeyPressed(97))
					{
						this._photoModeOrbitState = !this._photoModeOrbitState;
						this._missionScene.SetPhotoModeOrbit(this._photoModeOrbitState);
					}
					else if (this._input.IsGameKeyPressed(98))
					{
						base.MissionScreen.SetPhotoModeRequiresMouse(!base.MissionScreen.PhotoModeRequiresMouse);
					}
					else if (this._input.IsGameKeyPressed(99))
					{
						this._vignetteMode = !this._vignetteMode;
						this._missionScene.SetPhotoModeVignette(this._vignetteMode);
					}
					else if (this._input.IsGameKeyPressed(100))
					{
						this._hideAgentsMode = !this._hideAgentsMode;
						Utilities.SetRenderAgents(!this._hideAgentsMode);
					}
					else if (this._input.IsGameKeyPressed(107))
					{
						this.ResetChanges();
					}
					else if (base.MissionScreen.SceneLayer.Input.IsKeyPressed(InputKey.RightMouseButton))
					{
						this._prevMouseEnabled = base.MissionScreen.PhotoModeRequiresMouse;
						base.MissionScreen.SetPhotoModeRequiresMouse(false);
					}
					else if (base.MissionScreen.SceneLayer.Input.IsKeyReleased(InputKey.RightMouseButton))
					{
						base.MissionScreen.SetPhotoModeRequiresMouse(this._prevMouseEnabled);
					}
					if (this._input.IsGameKeyDown(93))
					{
						this._cameraRoll -= 0.1f;
						this._missionScene.SetPhotoModeRoll(this._cameraRoll);
						return;
					}
					if (this._input.IsGameKeyDown(94))
					{
						this._cameraRoll += 0.1f;
						this._missionScene.SetPhotoModeRoll(this._cameraRoll);
						return;
					}
				}
			}
			else if (!this._suspended)
			{
				this._suspended = true;
				if (this._gauntletLayer != null)
				{
					base.MissionScreen.RemoveLayer(this._gauntletLayer);
					this._gauntletLayer = null;
				}
				GauntletChatLogView.Current.SetCanFocusWhileInMission(true);
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00010A89 File Offset: 0x0000EC89
		private bool GetCanTakePicture()
		{
			return !TaleWorlds.InputSystem.Input.IsGamepadActive || !base.MissionScreen.PhotoModeRequiresMouse;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00010AA2 File Offset: 0x0000ECA2
		private bool GetCanMoveCamera()
		{
			return !this._photoModeOrbitState && (!TaleWorlds.InputSystem.Input.IsGamepadActive || !base.MissionScreen.PhotoModeRequiresMouse);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00010AC8 File Offset: 0x0000ECC8
		private void ResetChanges()
		{
			this._photoModeOrbitState = false;
			this._missionScene.SetPhotoModeOrbit(this._photoModeOrbitState);
			this._vignetteMode = false;
			this._hideAgentsMode = false;
			this._saveAmbientOcclusionPass = false;
			this._saveObjectIdPass = false;
			this._saveShadowPass = false;
			this._missionScene.SetPhotoModeFocus(0f, 0f, 0f, 0f);
			this._missionScene.SetPhotoModeVignette(this._vignetteMode);
			Utilities.SetRenderAgents(!this._hideAgentsMode);
			this._cameraRoll = 0f;
			this._missionScene.SetPhotoModeRoll(this._cameraRoll);
			this._dataSource.Reset();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00010B75 File Offset: 0x0000ED75
		public override bool OnEscape()
		{
			if (base.MissionScreen.IsPhotoModeEnabled)
			{
				base.MissionScreen.SetPhotoModeEnabled(false);
				base.Mission.IsInPhotoMode = false;
				MBDebug.DisableAllUI = false;
				this.ResetChanges();
				return true;
			}
			return false;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00010BAB File Offset: 0x0000EDAB
		public override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return !base.MissionScreen.IsPhotoModeEnabled;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00010BBB File Offset: 0x0000EDBB
		public override void OnMissionScreenFinalize()
		{
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._photoModeCategory.Unload();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x04000167 RID: 359
		private readonly TextObject _screenShotTakenMessage = new TextObject("{=1e12bdjj}Screenshot has been saved in {PATH}", null);

		// Token: 0x04000168 RID: 360
		private const float _cameraRollAmount = 0.1f;

		// Token: 0x04000169 RID: 361
		private const float _cameraFocusAmount = 0.1f;

		// Token: 0x0400016A RID: 362
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400016B RID: 363
		private PhotoModeVM _dataSource;

		// Token: 0x0400016C RID: 364
		private bool _registered;

		// Token: 0x0400016D RID: 365
		private SpriteCategory _photoModeCategory;

		// Token: 0x0400016E RID: 366
		private float _cameraRoll;

		// Token: 0x0400016F RID: 367
		private bool _photoModeOrbitState;

		// Token: 0x04000170 RID: 368
		private bool _suspended = true;

		// Token: 0x04000171 RID: 369
		private bool _vignetteMode;

		// Token: 0x04000172 RID: 370
		private bool _hideAgentsMode;

		// Token: 0x04000173 RID: 371
		private int _takePhoto = -1;

		// Token: 0x04000174 RID: 372
		private bool _saveAmbientOcclusionPass;

		// Token: 0x04000175 RID: 373
		private bool _saveObjectIdPass;

		// Token: 0x04000176 RID: 374
		private bool _saveShadowPass;

		// Token: 0x04000177 RID: 375
		private bool _prevUIDisabled;

		// Token: 0x04000178 RID: 376
		private bool _prevMouseEnabled;
	}
}
