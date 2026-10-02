using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x0200000F RID: 15
	[GameStateScreen(typeof(QuestsState))]
	public class GauntletQuestsScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00007549 File Offset: 0x00005749
		public GauntletQuestsScreen(QuestsState questsState)
		{
			this._questsState = questsState;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00007558 File Offset: 0x00005758
		protected override void OnInitialize()
		{
			base.OnInitialize();
			InformationManager.HideAllMessages();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00007568 File Offset: 0x00005768
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit") || this._gauntletLayer.Input.IsHotKeyReleased("Confirm") || this._gauntletLayer.Input.IsGameKeyReleased(42))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteClose();
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000075D8 File Offset: 0x000057D8
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
			this._questCategory = UIResourceManager.LoadSpriteCategory("ui_quest");
			this._dataSource = new QuestsVM(new Action(this.CloseQuestsScreen));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._gauntletLayer = new GauntletLayer("QuestScreen", 1, true);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._gauntletLayer.LoadMovie("QuestsScreen", this._dataSource);
			this._gauntletLayer.IsFocusLayer = true;
			base.AddLayer(this._gauntletLayer);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.QuestsScreen));
			if (this._questsState.InitialSelectedIssue != null)
			{
				this._dataSource.SetSelectedIssue(this._questsState.InitialSelectedIssue);
			}
			else if (this._questsState.InitialSelectedQuest != null)
			{
				this._dataSource.SetSelectedQuest(this._questsState.InitialSelectedQuest);
			}
			else if (this._questsState.InitialSelectedLog != null)
			{
				this._dataSource.SetSelectedLog(this._questsState.InitialSelectedLog);
			}
			UISoundsHelper.PlayUISound("event:/ui/panels/panel_quest_open");
			this._gauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(2, null);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00007764 File Offset: 0x00005964
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
			this._questCategory.Unload();
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			base.RemoveLayer(this._gauntletLayer);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000077BA File Offset: 0x000059BA
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000077BC File Offset: 0x000059BC
		void IGameStateListener.OnFinalize()
		{
			QuestsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._gauntletLayer = null;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000077DD File Offset: 0x000059DD
		private void CloseQuestsScreen()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x0400004F RID: 79
		protected QuestsVM _dataSource;

		// Token: 0x04000050 RID: 80
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000051 RID: 81
		private SpriteCategory _questCategory;

		// Token: 0x04000052 RID: 82
		private readonly QuestsState _questsState;
	}
}
