using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Nameplate;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003E RID: 62
	[OverrideView(typeof(MapPartyNameplateView))]
	public class GauntletMapPartyNameplateView : MapView
	{
		// Token: 0x060002EB RID: 747 RVA: 0x00011A08 File Offset: 0x0000FC08
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new PartyNameplatesVM(base.MapScreen.MapCameraView.Camera, new Action(base.MapScreen.FastMoveCameraToMainParty));
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("PartyNameplate", this._dataSource);
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			this._dataSource.Initialize();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00011AAF File Offset: 0x0000FCAF
		private void OnCharacterCreationIsOver(int index)
		{
			if (index != 9)
			{
				return;
			}
			this._dataSource.Reset();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00011AC4 File Offset: 0x0000FCC4
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			this._dataSource.Update();
			bool flag = base.MapScreen.SceneLayer.Input.IsGameKeyDown(5);
			EncounterModel encounterModel = Campaign.Current.Models.EncounterModel;
			for (int i = 0; i < this._dataSource.Nameplates.Count; i++)
			{
				PartyNameplateVM partyNameplateVM = this._dataSource.Nameplates[i];
				partyNameplateVM.ShouldShowFullName = flag;
				TextObject textObject;
				partyNameplateVM.CanParley = partyNameplateVM.ShouldShowFullName && encounterModel.CanMainHeroDoParleyWithParty(partyNameplateVM.Party.Party, out textObject);
			}
			if (this._dataSource.PlayerNameplate != null)
			{
				this._dataSource.PlayerNameplate.ShouldShowFullName = flag;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00011B80 File Offset: 0x0000FD80
		protected override void OnResume()
		{
			base.OnResume();
			foreach (PartyNameplateVM partyNameplateVM in this._dataSource.Nameplates)
			{
				partyNameplateVM.RefreshDynamicProperties(true);
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		protected override void OnFinalize()
		{
			CampaignEvents.OnCharacterCreationIsOverEvent.ClearListeners(this);
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._dataSource.OnFinalize();
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			this._dataSource = null;
			base.OnFinalize();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00011C2E File Offset: 0x0000FE2E
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00011C4A File Offset: 0x0000FE4A
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x0400011D RID: 285
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400011E RID: 286
		private PartyNameplatesVM _dataSource;

		// Token: 0x0400011F RID: 287
		private GauntletMovieIdentifier _movie;
	}
}
