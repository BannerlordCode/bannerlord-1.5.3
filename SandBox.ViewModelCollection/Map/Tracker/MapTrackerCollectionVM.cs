using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x0200004A RID: 74
	public class MapTrackerCollectionVM : ViewModel
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x000128B0 File Offset: 0x00010AB0
		public MapTrackerCollectionVM()
		{
			this.Trackers = new MBBindingList<MapTrackerItemVM>();
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000128C3 File Offset: 0x00010AC3
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Trackers.Clear();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000128D6 File Offset: 0x00010AD6
		public bool HasTrackerFor(ITrackableCampaignObject trackable)
		{
			return this.GetTrackerFor(trackable) != null;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000128E4 File Offset: 0x00010AE4
		public MapTrackerItemVM GetTrackerFor(ITrackableCampaignObject trackable)
		{
			for (int i = 0; i < this.Trackers.Count; i++)
			{
				if (this.Trackers[i].TrackedObject == trackable)
				{
					return this.Trackers[i];
				}
			}
			return null;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00012929 File Offset: 0x00010B29
		public void AddTracker(MapTrackerItemVM tracker)
		{
			if (this.HasTrackerFor(tracker.TrackedObject))
			{
				Debug.FailedAssert("Trying to add a tracker that was already added", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Map\\Tracker\\MapTrackerCollectionVM.cs", "AddTracker", 43);
				return;
			}
			this.Trackers.Add(tracker);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001295C File Offset: 0x00010B5C
		public void RemoveTrackerIfExists(ITrackableCampaignObject trackable)
		{
			MapTrackerItemVM trackerFor = this.GetTrackerFor(trackable);
			if (trackerFor != null)
			{
				this.Trackers.Remove(trackerFor);
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00012984 File Offset: 0x00010B84
		public void Update()
		{
			for (int i = 0; i < this.Trackers.Count; i++)
			{
				this.Trackers[i].RefreshBinding();
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000129B8 File Offset: 0x00010BB8
		public void UpdateProperties()
		{
			this.Trackers.ApplyActionOnAllItems(delegate(MapTrackerItemVM t)
			{
				t.UpdateProperties();
			});
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x000129E4 File Offset: 0x00010BE4
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x000129EC File Offset: 0x00010BEC
		public MBBindingList<MapTrackerItemVM> Trackers
		{
			get
			{
				return this._trackers;
			}
			set
			{
				if (value != this._trackers)
				{
					this._trackers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapTrackerItemVM>>(value, "Trackers");
				}
			}
		}

		// Token: 0x0400025B RID: 603
		private MBBindingList<MapTrackerItemVM> _trackers;
	}
}
