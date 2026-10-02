using System;
using SandBox.View.Map;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000028 RID: 40
	[OverrideView(typeof(MenuRecruitVolunteersView))]
	public class GauntletMenuRecruitVolunteersView : MenuView
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
		public override bool ShouldUpdateMenuAfterRemoved
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new RecruitmentVM();
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.SetRecruitAllInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("TakeAll"));
			this._dataSource.SetGetKeyTextFromKeyIDFunc(new Func<string, TextObject>(Game.Current.GameTextManager.GetHotKeyGameTextFromKeyID));
			base.Layer = new GauntletLayer("MapRecruit", 206, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MenuViewContext.AddLayer(base.Layer);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._movie = this._layerAsGauntletLayer.LoadMovie("RecruitmentPopup", this._dataSource);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			this._dataSource.RefreshScreen();
			this._dataSource.Enabled = true;
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.RecruitmentWindow));
			MapScreen mapScreen;
			if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null)
			{
				mapScreen.SetIsInRecruitment(true);
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000D054 File Offset: 0x0000B254
		protected override void OnFinalize()
		{
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			base.MenuViewContext.RemoveLayer(base.Layer);
			this._movie = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.MapWindow));
			MapScreen mapScreen;
			if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null)
			{
				mapScreen.SetIsInRecruitment(false);
			}
			base.OnFinalize();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteForceQuit();
			}
			else if (base.Layer.Input.IsHotKeyReleased("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteDone();
			}
			else if (base.Layer.Input.IsHotKeyReleased("Reset"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteReset();
			}
			else if (base.Layer.Input.IsHotKeyReleased("TakeAll"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteRecruitAll();
			}
			else if (base.Layer.Input.IsGameKeyReleased(39))
			{
				if (this._dataSource.FocusedVolunteerOwner != null)
				{
					this._dataSource.FocusedVolunteerOwner.ExecuteOpenEncyclopedia();
				}
				else if (this._dataSource.FocusedVolunteerTroop != null)
				{
					this._dataSource.FocusedVolunteerTroop.ExecuteOpenEncyclopedia();
				}
			}
			if (!this._dataSource.Enabled)
			{
				base.MenuViewContext.CloseRecruitVolunteers();
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000D22E File Offset: 0x0000B42E
		protected override TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000D231 File Offset: 0x0000B431
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000D24D File Offset: 0x0000B44D
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000A9 RID: 169
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000AA RID: 170
		private RecruitmentVM _dataSource;

		// Token: 0x040000AB RID: 171
		private GauntletMovieIdentifier _movie;
	}
}
