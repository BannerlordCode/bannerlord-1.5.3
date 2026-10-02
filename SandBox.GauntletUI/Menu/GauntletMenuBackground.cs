using System;
using SandBox.View.Menu;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000025 RID: 37
	[OverrideView(typeof(MenuBackgroundView))]
	public class GauntletMenuBackground : MenuView
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._layerAsGauntletLayer = base.MenuViewContext.FindLayer<GauntletLayer>("MapMenuView");
			if (this._layerAsGauntletLayer == null)
			{
				this._layerAsGauntletLayer = new GauntletLayer("MapMenuView", 100, false);
				base.MenuViewContext.AddLayer(this._layerAsGauntletLayer);
			}
			base.Layer = this._layerAsGauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenuBackground", null);
			this._layerAsGauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000C886 File Offset: 0x0000AA86
		protected override void OnFinalize()
		{
			GauntletLayer layerAsGauntletLayer = this._layerAsGauntletLayer;
			if (layerAsGauntletLayer != null)
			{
				layerAsGauntletLayer.ReleaseMovie(this._movie);
			}
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			base.OnFinalize();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000C8BA File Offset: 0x0000AABA
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000C8D6 File Offset: 0x0000AAD6
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000A1 RID: 161
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000A2 RID: 162
		private GauntletMovieIdentifier _movie;
	}
}
