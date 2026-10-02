using System;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000026 RID: 38
	[OverrideView(typeof(MenuBaseView))]
	public class GauntletMenuBaseView : MenuView
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000C8FA File Offset: 0x0000AAFA
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000C902 File Offset: 0x0000AB02
		public GameMenuVM GameMenuDataSource { get; private set; }

		// Token: 0x060001EF RID: 495 RVA: 0x0000C90C File Offset: 0x0000AB0C
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.GameMenuDataSource = new GameMenuVM(base.MenuContext);
			GameKey gameKey = HotKeyManager.GetCategory("Generic").GetGameKey(4);
			this.GameMenuDataSource.SetLeaveHotKey(gameKey);
			base.Layer = base.MenuViewContext.FindLayer<GauntletLayer>("MapMenuView");
			if (base.Layer == null)
			{
				base.Layer = new GauntletLayer("MapMenuView", 100, false);
				base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				base.MenuViewContext.AddLayer(base.Layer);
			}
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenu", this.GameMenuDataSource);
			ScreenManager.TrySetFocus(base.Layer);
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 1f;
			MBInformationManager.HideInformations();
			this.GainGamepadNavigationAfterSeconds(0.25f);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000C9FE File Offset: 0x0000ABFE
		protected override void OnActivate()
		{
			base.OnActivate();
			this.GameMenuDataSource.Refresh(true);
			this.GameMenuDataSource.SetIdleMode(false);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000CA1E File Offset: 0x0000AC1E
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.GameMenuDataSource.SetIdleMode(true);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CA32 File Offset: 0x0000AC32
		protected override void OnResume()
		{
			base.OnResume();
			this.GameMenuDataSource.Refresh(true);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000CA46 File Offset: 0x0000AC46
		protected override void OnMenuContextRefreshed()
		{
			base.OnMenuContextRefreshed();
			this.GameMenuDataSource.Refresh(true);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		protected override void OnFinalize()
		{
			this.GameMenuDataSource.OnFinalize();
			this.GameMenuDataSource = null;
			ScreenManager.TryLoseFocus(base.Layer);
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			base.OnFinalize();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000CAB2 File Offset: 0x0000ACB2
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.GameMenuDataSource.OnFrameTick();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000CAC6 File Offset: 0x0000ACC6
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000CAE2 File Offset: 0x0000ACE2
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000CAFE File Offset: 0x0000ACFE
		protected override void OnMenuContextUpdated(MenuContext newMenuContext)
		{
			base.OnMenuContextUpdated(newMenuContext);
			this.GameMenuDataSource.UpdateMenuContext(newMenuContext);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000CB13 File Offset: 0x0000AD13
		protected override void OnBackgroundMeshNameSet(string name)
		{
			base.OnBackgroundMeshNameSet(name);
			this.GameMenuDataSource.Background = name;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000CB28 File Offset: 0x0000AD28
		private void GainGamepadNavigationAfterSeconds(float seconds)
		{
			this._layerAsGauntletLayer.UIContext.GamepadNavigation.GainNavigationAfterTime(seconds, () => this.GameMenuDataSource.ItemList.Count > 0);
		}

		// Token: 0x040000A4 RID: 164
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000A5 RID: 165
		private GauntletMovieIdentifier _movie;
	}
}
