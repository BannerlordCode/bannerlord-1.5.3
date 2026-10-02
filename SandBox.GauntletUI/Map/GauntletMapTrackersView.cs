using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Map.Tracker;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000043 RID: 67
	[OverrideView(typeof(MapTrackersView))]
	public class GauntletMapTrackersView : MapTrackersView, IMapTrackersHandler
	{
		// Token: 0x06000318 RID: 792 RVA: 0x000125A0 File Offset: 0x000107A0
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new MapTrackerCollectionVM();
			MapTrackerItemVM.OnFastMoveCameraToPosition = new Action<CampaignVec2>(this.FastMoveCameraToPosition);
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapTrackers", this._dataSource);
			Campaign.Current.MapTrackerManager.Handler = this;
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			((IMapTrackersHandler)this).ResetTrackers();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00012641 File Offset: 0x00010841
		private void OnCharacterCreationIsOver(int index)
		{
			if (index != 9)
			{
				return;
			}
			((IMapTrackersHandler)this).ResetTrackers();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00012650 File Offset: 0x00010850
		private void AddTrackerForObject(ITrackableCampaignObject trackable)
		{
			if (this._dataSource.HasTrackerFor(trackable))
			{
				return;
			}
			MobileParty mobileParty;
			if ((mobileParty = trackable as MobileParty) != null)
			{
				this._dataSource.AddTracker(new MapMobilePartyTrackItemVM(mobileParty));
				return;
			}
			Army army;
			if ((army = trackable as Army) != null)
			{
				this._dataSource.AddTracker(new MapArmyTrackItemVM(army));
				return;
			}
			MapMarker mapMarker;
			if ((mapMarker = trackable as MapMarker) != null)
			{
				this._dataSource.AddTracker(new MapMarkerTrackerItemVM(mapMarker));
				return;
			}
			Debug.FailedAssert("Unsupported trackable object type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.GauntletUI\\Map\\GauntletMapTrackersView.cs", "AddTrackerForObject", 77);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000126D6 File Offset: 0x000108D6
		protected override void OnResume()
		{
			base.OnResume();
			this._dataSource.UpdateProperties();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000126EC File Offset: 0x000108EC
		private void UpdateTrackerPropertiesAux(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MapTrackerItemVM mapTrackerItemVM = this._dataSource.Trackers[i];
				mapTrackerItemVM.UpdateProperties();
				float num;
				float num2;
				float num3;
				this.GetScreenPosition(mapTrackerItemVM.TrackedObject, out num, out num2, out num3);
				mapTrackerItemVM.UpdatePosition(num, num2, num3);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00012739 File Offset: 0x00010939
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			TWParallel.For(0, this._dataSource.Trackers.Count, new TWParallel.ParallelForAuxPredicate(this.UpdateTrackerPropertiesAux), 32);
			this._dataSource.Update();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00012774 File Offset: 0x00010974
		protected override void OnFinalize()
		{
			Campaign.Current.MapTrackerManager.Handler = null;
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			MapTrackerItemVM.OnFastMoveCameraToPosition = null;
			this._dataSource.OnFinalize();
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			this._dataSource = null;
			base.OnFinalize();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000127E0 File Offset: 0x000109E0
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000127FC File Offset: 0x000109FC
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00012818 File Offset: 0x00010A18
		private void GetScreenPosition(ITrackableCampaignObject trackable, out float screenX, out float screenY, out float screenW)
		{
			float num = 0f;
			Vec3 position = trackable.GetPosition();
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(position.AsVec2, true);
			mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num);
			position.z = MathF.Max(num, 0f);
			screenX = -5000f;
			screenY = -5000f;
			screenW = -1f;
			MBWindowManager.WorldToScreenInsideUsableArea(base.MapScreen.MapCameraView.Camera, position, ref screenX, ref screenY, ref screenW);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00012897 File Offset: 0x00010A97
		private void FastMoveCameraToPosition(CampaignVec2 target)
		{
			base.MapScreen.FastMoveCameraToPosition(target);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000128A5 File Offset: 0x00010AA5
		void IMapTrackersHandler.OnTrackerAdded(ITrackableCampaignObject trackable)
		{
			this.AddTrackerForObject(trackable);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000128AE File Offset: 0x00010AAE
		void IMapTrackersHandler.OnTrackerRemoved(ITrackableCampaignObject trackable)
		{
			this._dataSource.RemoveTrackerIfExists(trackable);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000128BC File Offset: 0x00010ABC
		void IMapTrackersHandler.ResetTrackers()
		{
			this._dataSource.Trackers.Clear();
			foreach (ITrackableCampaignObject trackableCampaignObject in Campaign.Current.MapTrackerManager.GetAllTrackers())
			{
				this.AddTrackerForObject(trackableCampaignObject);
			}
		}

		// Token: 0x0400012A RID: 298
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400012B RID: 299
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400012C RID: 300
		private MapTrackerCollectionVM _dataSource;
	}
}
