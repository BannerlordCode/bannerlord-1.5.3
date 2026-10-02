using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000039 RID: 57
	[OverrideView(typeof(MapEventVisualsView))]
	public class GauntletMapEventVisualsView : MapView, IGauntletMapEventVisualHandler
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0001041C File Offset: 0x0000E61C
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new MapEventVisualsVM(base.MapScreen.MapCameraView.Camera);
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapEventVisuals", this._dataSource);
			GauntletMapEventVisualCreator gauntletMapEventVisualCreator;
			if ((gauntletMapEventVisualCreator = Campaign.Current.VisualCreator.MapEventVisualCreator as GauntletMapEventVisualCreator) != null)
			{
				gauntletMapEventVisualCreator.Handlers.Add(this);
				foreach (GauntletMapEventVisual gauntletMapEventVisual in gauntletMapEventVisualCreator.GetCurrentEvents())
				{
					this._dataSource.OnMapEventStarted(gauntletMapEventVisual.MapEvent);
				}
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00010500 File Offset: 0x0000E700
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			this._dataSource.Update(dt);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00010518 File Offset: 0x0000E718
		protected override void OnFinalize()
		{
			GauntletMapEventVisualCreator gauntletMapEventVisualCreator;
			if ((gauntletMapEventVisualCreator = Campaign.Current.VisualCreator.MapEventVisualCreator as GauntletMapEventVisualCreator) != null)
			{
				gauntletMapEventVisualCreator.Handlers.Remove(this);
			}
			this._dataSource.OnFinalize();
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			this._movie = null;
			this._dataSource = null;
			base.OnFinalize();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00010588 File Offset: 0x0000E788
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000105A4 File Offset: 0x0000E7A4
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000105C0 File Offset: 0x0000E7C0
		void IGauntletMapEventVisualHandler.OnNewEventStarted(GauntletMapEventVisual newEvent)
		{
			this._dataSource.OnMapEventStarted(newEvent.MapEvent);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000105D3 File Offset: 0x0000E7D3
		void IGauntletMapEventVisualHandler.OnInitialized(GauntletMapEventVisual newEvent)
		{
			this._dataSource.OnMapEventStarted(newEvent.MapEvent);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000105E6 File Offset: 0x0000E7E6
		void IGauntletMapEventVisualHandler.OnEventEnded(GauntletMapEventVisual newEvent)
		{
			this._dataSource.OnMapEventEnded(newEvent.MapEvent);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000105F9 File Offset: 0x0000E7F9
		void IGauntletMapEventVisualHandler.OnEventVisibilityChanged(GauntletMapEventVisual visibilityChangedEvent)
		{
			this._dataSource.OnMapEventVisibilityChanged(visibilityChangedEvent.MapEvent);
		}

		// Token: 0x040000FB RID: 251
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000FC RID: 252
		private GauntletMovieIdentifier _movie;

		// Token: 0x040000FD RID: 253
		private MapEventVisualsVM _dataSource;
	}
}
