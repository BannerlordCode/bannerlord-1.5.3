using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map
{
	// Token: 0x02000045 RID: 69
	public class MapEventVisualsVM : ViewModel
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x0001237D File Offset: 0x0001057D
		public MapEventVisualsVM(Camera mapCamera)
		{
			this._mapCamera = mapCamera;
			this.MapEvents = new MBBindingList<MapEventVisualItemVM>();
			this.UpdateMapEventsAuxPredicate = new TWParallel.ParallelForAuxPredicate(this.UpdateMapEventsAux);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000123B4 File Offset: 0x000105B4
		private void UpdateMapEventsAux(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.MapEvents[i].ParallelUpdatePosition();
				this.MapEvents[i].DetermineIsVisibleOnMap();
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000123F0 File Offset: 0x000105F0
		public void Update(float dt)
		{
			TWParallel.For(0, this.MapEvents.Count, this.UpdateMapEventsAuxPredicate, 16);
			for (int i = 0; i < this.MapEvents.Count; i++)
			{
				this.MapEvents[i].UpdateBindingProperties();
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001243D File Offset: 0x0001063D
		public void OnMapEventVisibilityChanged(MapEvent mapEvent)
		{
			if (this._eventToVisualMap.ContainsKey(mapEvent))
			{
				this._eventToVisualMap[mapEvent].UpdateProperties();
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00012460 File Offset: 0x00010660
		public void OnMapEventStarted(MapEvent mapEvent)
		{
			if (!this._eventToVisualMap.ContainsKey(mapEvent))
			{
				if (!this.IsMapEventSettlementRelated(mapEvent))
				{
					MapEventVisualItemVM mapEventVisualItemVM = new MapEventVisualItemVM(this._mapCamera, mapEvent);
					this._eventToVisualMap.Add(mapEvent, mapEventVisualItemVM);
					this.MapEvents.Add(mapEventVisualItemVM);
					mapEventVisualItemVM.UpdateProperties();
				}
				return;
			}
			if (!this.IsMapEventSettlementRelated(mapEvent))
			{
				this._eventToVisualMap[mapEvent].UpdateProperties();
				return;
			}
			MapEventVisualItemVM mapEventVisualItemVM2 = this._eventToVisualMap[mapEvent];
			this.MapEvents.Remove(mapEventVisualItemVM2);
			this._eventToVisualMap.Remove(mapEvent);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000124F4 File Offset: 0x000106F4
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (this._eventToVisualMap.ContainsKey(mapEvent))
			{
				MapEventVisualItemVM mapEventVisualItemVM = this._eventToVisualMap[mapEvent];
				this.MapEvents.Remove(mapEventVisualItemVM);
				this._eventToVisualMap.Remove(mapEvent);
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00012536 File Offset: 0x00010736
		private bool IsMapEventSettlementRelated(MapEvent mapEvent)
		{
			return mapEvent.MapEventSettlement != null;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00012541 File Offset: 0x00010741
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x00012549 File Offset: 0x00010749
		public MBBindingList<MapEventVisualItemVM> MapEvents
		{
			get
			{
				return this._mapEvents;
			}
			set
			{
				if (this._mapEvents != value)
				{
					this._mapEvents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapEventVisualItemVM>>(value, "MapEvents");
				}
			}
		}

		// Token: 0x0400024A RID: 586
		private readonly Camera _mapCamera;

		// Token: 0x0400024B RID: 587
		private readonly Dictionary<MapEvent, MapEventVisualItemVM> _eventToVisualMap = new Dictionary<MapEvent, MapEventVisualItemVM>();

		// Token: 0x0400024C RID: 588
		private readonly TWParallel.ParallelForAuxPredicate UpdateMapEventsAuxPredicate;

		// Token: 0x0400024D RID: 589
		private MBBindingList<MapEventVisualItemVM> _mapEvents;
	}
}
