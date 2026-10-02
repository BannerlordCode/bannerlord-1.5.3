using System;
using System.Collections.Generic;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.HeirSelectionPopup;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200002C RID: 44
	[OverrideView(typeof(HeirSelectionPopupView))]
	public class GauntletHeirSelectionPopupView : MapView
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		public GauntletHeirSelectionPopupView(Dictionary<Hero, int> heirApparents)
		{
			this._heirApparents = heirApparents;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._gameOverCategory = UIResourceManager.LoadSpriteCategory("ui_gameover");
			this._dataSource = new HeirSelectionPopupVM(this._heirApparents);
			this.InitializeKeyVisuals();
			base.Layer = new GauntletLayer("HeirSelectionPopup", 203, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			this._movie = this._layerAsGauntletLayer.LoadMovie("HeirSelectionPopup", this._dataSource);
			base.MapScreen.AddLayer(base.Layer);
			base.MapScreen.SetIsHeirSelectionPopupActive(true);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000DC59 File Offset: 0x0000BE59
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			HeirSelectionPopupVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.Update();
			}
			this.HandleInput();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000DC79 File Offset: 0x0000BE79
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			HeirSelectionPopupVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.Update();
			}
			this.HandleInput();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DC99 File Offset: 0x0000BE99
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			HeirSelectionPopupVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.Update();
			}
			this.HandleInput();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		protected override void OnFinalize()
		{
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._gameOverCategory.Unload();
			base.MapScreen.RemoveLayer(base.Layer);
			this._movie = null;
			this._dataSource = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			base.MapScreen.SetIsHeirSelectionPopupActive(false);
			Campaign.Current.SetTimeControlModeLock(false);
			base.OnFinalize();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000DD2F File Offset: 0x0000BF2F
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000DD4B File Offset: 0x0000BF4B
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DD67 File Offset: 0x0000BF67
		protected override bool IsEscaped()
		{
			return true;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000DD6A File Offset: 0x0000BF6A
		protected override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return false;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000DD6D File Offset: 0x0000BF6D
		private void HandleInput()
		{
			if (this._dataSource != null && base.Layer.Input.IsHotKeyReleased("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.ExecuteSelectHeir();
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DDA3 File Offset: 0x0000BFA3
		private void InitializeKeyVisuals()
		{
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
		}

		// Token: 0x040000BB RID: 187
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000BC RID: 188
		private HeirSelectionPopupVM _dataSource;

		// Token: 0x040000BD RID: 189
		private GauntletMovieIdentifier _movie;

		// Token: 0x040000BE RID: 190
		private readonly Dictionary<Hero, int> _heirApparents;

		// Token: 0x040000BF RID: 191
		private SpriteCategory _gameOverCategory;
	}
}
