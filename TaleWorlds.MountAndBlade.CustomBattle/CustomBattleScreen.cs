using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000A RID: 10
	[GameStateScreen(typeof(CustomBattleState))]
	public class CustomBattleScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005ADB File Offset: 0x00003CDB
		public CustomBattleScreen(CustomBattleState customBattleState)
		{
			this._customBattleState = customBattleState;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005AEA File Offset: 0x00003CEA
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005AEC File Offset: 0x00003CEC
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005AEE File Offset: 0x00003CEE
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005AF0 File Offset: 0x00003CF0
		void IGameStateListener.OnFinalize()
		{
			this._dataSource.OnFinalize();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005B00 File Offset: 0x00003D00
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new CustomBattleVM(this._customBattleState);
			this._dataSource.SetStartInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.SetRandomizeInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Randomize"));
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this._dataSource.TroopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp != null)
			{
				troopTypeSelectionPopUp.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			}
			this._gauntletLayer = new GauntletLayer("CustomBattle", 1, true);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this.LoadMovie();
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._dataSource.SetActiveState(true);
			base.AddLayer(this._gauntletLayer);
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005C48 File Offset: 0x00003E48
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._isFirstFrameCounter >= 0)
			{
				if (this._isFirstFrameCounter == 0)
				{
					LoadingWindow.DisableGlobalLoadingWindow();
				}
				this._isFirstFrameCounter--;
			}
			if (!this._gauntletLayer.IsFocusedOnInput())
			{
				TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this._dataSource.TroopTypeSelectionPopUp;
				if (troopTypeSelectionPopUp != null && troopTypeSelectionPopUp.IsOpen)
				{
					if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.TroopTypeSelectionPopUp.ExecuteCancel();
						return;
					}
					if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.TroopTypeSelectionPopUp.ExecuteDone();
						return;
					}
					if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.TroopTypeSelectionPopUp.ExecuteReset();
						return;
					}
				}
				else
				{
					if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.ExecuteBack();
						return;
					}
					if (this._gauntletLayer.Input.IsHotKeyReleased("Randomize"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.ExecuteRandomize();
						return;
					}
					if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.ExecuteStart();
					}
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005DCB File Offset: 0x00003FCB
		protected override void OnFinalize()
		{
			this.UnloadMovie();
			base.RemoveLayer(this._gauntletLayer);
			this._dataSource = null;
			this._gauntletLayer = null;
			base.OnFinalize();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005DF3 File Offset: 0x00003FF3
		protected override void OnActivate()
		{
			this.LoadMovie();
			CustomBattleVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetActiveState(true);
			}
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._isFirstFrameCounter = 2;
			base.OnActivate();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005E31 File Offset: 0x00004031
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.UnloadMovie();
			CustomBattleVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.SetActiveState(false);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005E50 File Offset: 0x00004050
		public override void UpdateLayout()
		{
			base.UpdateLayout();
			if (!this._isMovieLoaded)
			{
				CustomBattleVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.RefreshValues();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005E70 File Offset: 0x00004070
		private void LoadMovie()
		{
			if (!this._isMovieLoaded)
			{
				this._gauntletMovie = this._gauntletLayer.LoadMovie("CustomBattleScreen", this._dataSource);
				this._isMovieLoaded = true;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005E9D File Offset: 0x0000409D
		private void UnloadMovie()
		{
			if (this._isMovieLoaded)
			{
				this._gauntletLayer.ReleaseMovie(this._gauntletMovie);
				this._gauntletMovie = null;
				this._isMovieLoaded = false;
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
			}
		}

		// Token: 0x04000047 RID: 71
		private CustomBattleState _customBattleState;

		// Token: 0x04000048 RID: 72
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000049 RID: 73
		private GauntletMovieIdentifier _gauntletMovie;

		// Token: 0x0400004A RID: 74
		private CustomBattleVM _dataSource;

		// Token: 0x0400004B RID: 75
		private bool _isMovieLoaded;

		// Token: 0x0400004C RID: 76
		private int _isFirstFrameCounter;
	}
}
