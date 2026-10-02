using System;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000029 RID: 41
	[OverrideView(typeof(MenuTournamentLeaderboardView))]
	public class GauntletMenuTournamentLeaderboardView : MenuView
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000D274 File Offset: 0x0000B474
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new TournamentLeaderboardVM
			{
				IsEnabled = true
			};
			base.Layer = new GauntletLayer("MapTournamentLeaderboard", 206, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenuTournamentLeaderboard", this._dataSource);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			base.MenuViewContext.AddLayer(base.Layer);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000D350 File Offset: 0x0000B550
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
			base.OnFinalize();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("Exit") || base.Layer.Input.IsHotKeyReleased("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.IsEnabled = false;
			}
			if (!this._dataSource.IsEnabled)
			{
				base.MenuViewContext.CloseTournamentLeaderboard();
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000D42C File Offset: 0x0000B62C
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000D448 File Offset: 0x0000B648
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000AC RID: 172
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000AD RID: 173
		private TournamentLeaderboardVM _dataSource;

		// Token: 0x040000AE RID: 174
		private GauntletMovieIdentifier _movie;
	}
}
