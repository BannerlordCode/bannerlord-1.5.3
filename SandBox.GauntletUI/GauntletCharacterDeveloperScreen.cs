using System;
using SandBox.View;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x02000007 RID: 7
	[GameStateScreen(typeof(CharacterDeveloperState))]
	public class GauntletCharacterDeveloperScreen : ScreenBase, IGameStateListener, IChangeableScreen, ICharacterDeveloperStateHandler
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000028C9 File Offset: 0x00000AC9
		public GauntletCharacterDeveloperScreen(CharacterDeveloperState clanState)
		{
			this._characterDeveloperState = clanState;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028D8 File Offset: 0x00000AD8
		protected override void OnInitialize()
		{
			base.OnInitialize();
			InformationManager.HideAllMessages();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028E8 File Offset: 0x00000AE8
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit") || this._gauntletLayer.Input.IsGameKeyPressed(37))
			{
				if (this._dataSource.CurrentCharacter.IsInspectingAnAttribute)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.CurrentCharacter.ExecuteStopInspectingCurrentAttribute();
					return;
				}
				if (this._dataSource.CurrentCharacter.PerkSelection.IsActive)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.CurrentCharacter.PerkSelection.ExecuteDeactivate();
					return;
				}
				this.CloseCharacterDeveloperScreen();
				return;
			}
			else
			{
				if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
				{
					this.ExecuteConfirm();
					return;
				}
				if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
				{
					this.ExecuteReset();
					return;
				}
				if (this._gauntletLayer.Input.IsHotKeyPressed("SwitchToPreviousTab"))
				{
					this.ExecuteSwitchToPreviousTab();
					return;
				}
				if (this._gauntletLayer.Input.IsHotKeyPressed("SwitchToNextTab"))
				{
					this.ExecuteSwitchToNextTab();
				}
				return;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002A10 File Offset: 0x00000C10
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
			this._characterdeveloper = UIResourceManager.LoadSpriteCategory("ui_characterdeveloper");
			this._dataSource = new CharacterDeveloperVM(new Action(this.CloseCharacterDeveloperScreen));
			this._dataSource.SetGetKeyTextFromKeyIDFunc(new Func<string, TextObject>(Game.Current.GameTextManager.GetHotKeyGameTextFromKeyID));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.SetPreviousCharacterInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"));
			this._dataSource.SetNextCharacterInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"));
			if (this._characterDeveloperState.InitialSelectedHero != null)
			{
				this._dataSource.SelectHero(this._characterDeveloperState.InitialSelectedHero);
			}
			this._gauntletLayer = new GauntletLayer("CharacterDeveloper", 1, true);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._gauntletLayer.LoadMovie("CharacterDeveloper", this._dataSource);
			base.AddLayer(this._gauntletLayer);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.CharacterScreen));
			UISoundsHelper.PlayUISound("event:/ui/panels/panel_character_open");
			this._gauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(2, null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BEB File Offset: 0x00000DEB
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
			base.RemoveLayer(this._gauntletLayer);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C2B File Offset: 0x00000E2B
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C2D File Offset: 0x00000E2D
		void IGameStateListener.OnFinalize()
		{
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
			this._characterdeveloper.Unload();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C53 File Offset: 0x00000E53
		private void CloseCharacterDeveloperScreen()
		{
			UISoundsHelper.PlayUISound("event:/ui/default");
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C6F File Offset: 0x00000E6F
		private void ExecuteConfirm()
		{
			UISoundsHelper.PlayUISound("event:/ui/default");
			this._dataSource.ExecuteDone();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C86 File Offset: 0x00000E86
		private void ExecuteReset()
		{
			UISoundsHelper.PlayUISound("event:/ui/default");
			this._dataSource.ExecuteReset();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C9D File Offset: 0x00000E9D
		private void ExecuteSwitchToPreviousTab()
		{
			MBBindingList<SelectorItemVM> itemList = this._dataSource.CharacterList.ItemList;
			if (itemList != null && itemList.Count > 1)
			{
				UISoundsHelper.PlayUISound("event:/ui/checkbox");
			}
			this._dataSource.CharacterList.ExecuteSelectPreviousItem();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002CDA File Offset: 0x00000EDA
		private void ExecuteSwitchToNextTab()
		{
			MBBindingList<SelectorItemVM> itemList = this._dataSource.CharacterList.ItemList;
			if (itemList != null && itemList.Count > 1)
			{
				UISoundsHelper.PlayUISound("event:/ui/checkbox");
			}
			this._dataSource.CharacterList.ExecuteSelectNextItem();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D17 File Offset: 0x00000F17
		bool IChangeableScreen.AnyUnsavedChanges()
		{
			return this._dataSource.IsThereAnyChanges();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D24 File Offset: 0x00000F24
		bool IChangeableScreen.CanChangesBeApplied()
		{
			return true;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D27 File Offset: 0x00000F27
		void IChangeableScreen.ApplyChanges()
		{
			this._dataSource.ApplyAllChanges();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D34 File Offset: 0x00000F34
		void IChangeableScreen.ResetChanges()
		{
			this._dataSource.ExecuteReset();
		}

		// Token: 0x0400000D RID: 13
		private CharacterDeveloperVM _dataSource;

		// Token: 0x0400000E RID: 14
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400000F RID: 15
		private SpriteCategory _characterdeveloper;

		// Token: 0x04000010 RID: 16
		private readonly CharacterDeveloperState _characterDeveloperState;
	}
}
