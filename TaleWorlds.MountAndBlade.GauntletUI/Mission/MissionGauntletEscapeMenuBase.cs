using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000030 RID: 48
	public abstract class MissionGauntletEscapeMenuBase : MissionEscapeMenuView
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000BC42 File Offset: 0x00009E42
		protected MissionGauntletEscapeMenuBase(string viewFile)
		{
			this._viewFile = viewFile;
			this.ViewOrderPriority = 50;
			Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000BC74 File Offset: 0x00009E74
		protected virtual List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			return null;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000BC78 File Offset: 0x00009E78
		public override void OnMissionScreenFinalize()
		{
			Game.Current.EventManager.UnregisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
			this.DataSource.OnFinalize();
			this.DataSource = null;
			this._gauntletLayer = null;
			this._movie = null;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000BCC6 File Offset: 0x00009EC6
		public override bool OnEscape()
		{
			if (!this._isRenderingStarted)
			{
				return false;
			}
			if (!base.IsActive)
			{
				this.DataSource.RefreshItems(this.GetEscapeMenuItems());
			}
			return this.OnEscapeMenuToggled(!base.IsActive);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000BCFC File Offset: 0x00009EFC
		protected bool OnEscapeMenuToggled(bool isOpened)
		{
			if (base.IsActive == isOpened)
			{
				return false;
			}
			base.IsActive = isOpened;
			if (isOpened)
			{
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.EscapeMenu));
				this.DataSource.RefreshValues();
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.PauseGameEngine();
					Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
				}
				this._gauntletLayer = new GauntletLayer("MissionEscapeMenu", this.ViewOrderPriority, false);
				this._gauntletLayer.IsFocusLayer = true;
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				this._movie = this._gauntletLayer.LoadMovie(this._viewFile, this.DataSource);
				base.MissionScreen.AddLayer(this._gauntletLayer);
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
			else
			{
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(this._escapeMenuPrevTutorialContext));
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.UnPauseGameEngine();
					Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
				}
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._movie = null;
				this._gauntletLayer = null;
			}
			return true;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000BE50 File Offset: 0x0000A050
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsActive && (this._gauntletLayer.Input.IsHotKeyReleased("ToggleEscapeMenu") || this._gauntletLayer.Input.IsHotKeyReleased("Exit")))
			{
				this.OnEscapeMenuToggled(false);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000BEA2 File Offset: 0x0000A0A2
		public override void OnSceneRenderingStarted()
		{
			base.OnSceneRenderingStarted();
			this._isRenderingStarted = true;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000BEB1 File Offset: 0x0000A0B1
		private void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			if (obj.NewContext != TutorialContexts.EscapeMenu)
			{
				this._escapeMenuPrevTutorialContext = obj.NewContext;
			}
		}

		// Token: 0x040000FD RID: 253
		protected EscapeMenuVM DataSource;

		// Token: 0x040000FE RID: 254
		private GauntletLayer _gauntletLayer;

		// Token: 0x040000FF RID: 255
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000100 RID: 256
		private string _viewFile;

		// Token: 0x04000101 RID: 257
		private bool _isRenderingStarted;

		// Token: 0x04000102 RID: 258
		private TutorialContexts _escapeMenuPrevTutorialContext;
	}
}
