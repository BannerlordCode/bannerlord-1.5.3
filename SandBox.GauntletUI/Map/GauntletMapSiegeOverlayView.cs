using System;
using SandBox.View.Map;
using SandBox.View.Map.Managers;
using SandBox.View.Map.Visuals;
using SandBox.ViewModelCollection.MapSiege;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000042 RID: 66
	[OverrideView(typeof(MapSiegeOverlayView))]
	public class GauntletMapSiegeOverlayView : MapView
	{
		// Token: 0x0600030F RID: 783 RVA: 0x00012374 File Offset: 0x00010574
		protected override void CreateLayout()
		{
			base.CreateLayout();
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			SettlementVisual settlementVisual = SettlementVisualManager.Current.GetSettlementVisual(PlayerSiege.PlayerSiegeEvent.BesiegedSettlement);
			this._dataSource = new MapSiegeVM(base.MapScreen.MapCameraView.Camera, settlementVisual.GetAttackerBatteringRamSiegeEngineFrames(), settlementVisual.GetAttackerRangedSiegeEngineFrames(), settlementVisual.GetAttackerTowerSiegeEngineFrames(), settlementVisual.GetDefenderRangedSiegeEngineFrames(), settlementVisual.GetBreachableWallFrames());
			CampaignEvents.SiegeEngineBuiltEvent.AddNonSerializedListener(this, new Action<SiegeEvent, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineBuilt));
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapSiegeOverlay", this._dataSource);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00012431 File Offset: 0x00010631
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update(base.MapScreen.MapCameraView.CameraDistance);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0001245A File Offset: 0x0001065A
		protected override void OnFinalize()
		{
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._movie = null;
			this._dataSource = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			CampaignEvents.SiegeEngineBuiltEvent.ClearListeners(this);
			base.OnFinalize();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0001249A File Offset: 0x0001069A
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000124B6 File Offset: 0x000106B6
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000124D4 File Offset: 0x000106D4
		protected override void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			base.OnSiegeEngineClick(siegeEngineFrame);
			UISoundsHelper.PlayUISound("event:/ui/panels/siege/engine_click");
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.ProductionController.IsEnabled && this._dataSource.ProductionController.LatestSelectedPOI.MapSceneLocationFrame.NearlyEquals(siegeEngineFrame, 1E-05f))
			{
				this._dataSource.ProductionController.ExecuteDisable();
				return;
			}
			MapSiegeVM dataSource2 = this._dataSource;
			if (dataSource2 != null)
			{
				dataSource2.OnSelectionFromScene(siegeEngineFrame);
			}
			base.MapState.OnSiegeEngineClick(siegeEngineFrame);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0001255F File Offset: 0x0001075F
		protected override void OnMapTerrainClick()
		{
			base.OnMapTerrainClick();
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.ProductionController.ExecuteDisable();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0001257C File Offset: 0x0001077C
		private void OnSiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngineType)
		{
			if (siegeEvent.IsPlayerSiegeEvent && side == PlayerSiege.PlayerSide)
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/siege/engine_build_complete");
			}
		}

		// Token: 0x04000127 RID: 295
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000128 RID: 296
		private MapSiegeVM _dataSource;

		// Token: 0x04000129 RID: 297
		private GauntletMovieIdentifier _movie;
	}
}
