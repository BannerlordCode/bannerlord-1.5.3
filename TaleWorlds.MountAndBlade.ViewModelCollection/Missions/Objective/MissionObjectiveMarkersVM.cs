using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003D RID: 61
	public class MissionObjectiveMarkersVM : ViewModel
	{
		// Token: 0x06000550 RID: 1360 RVA: 0x00014822 File Offset: 0x00012A22
		public MissionObjectiveMarkersVM(MissionObjectiveLogic objectiveLogic, Camera missionCamera)
		{
			this.Targets = new MBBindingList<MissionObjectiveMarkerVM>();
			this._distanceComparer = new MissionObjectiveMarkersVM.MarkerDistanceComparer();
			this._objectiveLogic = objectiveLogic;
			this._missionCamera = missionCamera;
			this.IsEnabled = true;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00014855 File Offset: 0x00012A55
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.IsEnabled)
			{
				this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM x)
				{
					x.RefreshValues();
				});
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001488F File Offset: 0x00012A8F
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000148C4 File Offset: 0x00012AC4
		public void UpdateObjective(MissionObjective objective)
		{
			if (this._latestObjective == objective)
			{
				return;
			}
			this._latestObjective = objective;
			this.Targets.Clear();
			if (this._latestObjective != null)
			{
				MBReadOnlyList<MissionObjectiveTarget> targetsCopy = this._latestObjective.GetTargetsCopy();
				if (targetsCopy != null)
				{
					for (int i = 0; i < targetsCopy.Count; i++)
					{
						MissionObjectiveMarkerVM missionObjectiveMarkerVM = new MissionObjectiveMarkerVM(targetsCopy[i]);
						this.Targets.Add(missionObjectiveMarkerVM);
					}
				}
			}
			this.RefreshValues();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00014934 File Offset: 0x00012B34
		public void Tick(float dt)
		{
			this.UpdateTargets();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001493C File Offset: 0x00012B3C
		private void UpdateTargets()
		{
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionObjectiveMarkerVM missionObjectiveMarkerVM = this.Targets[i];
				missionObjectiveMarkerVM.UpdateActiveState();
				if (missionObjectiveMarkerVM.IsActive)
				{
					missionObjectiveMarkerVM.UpdatePosition(this._missionCamera);
				}
			}
			this.Targets.Sort(this._distanceComparer);
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00014997 File Offset: 0x00012B97
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0001499F File Offset: 0x00012B9F
		[DataSourceProperty]
		public MBBindingList<MissionObjectiveMarkerVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionObjectiveMarkerVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x000149BD File Offset: 0x00012BBD
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x000149C8 File Offset: 0x00012BC8
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.Targets.ApplyActionOnAllItems(delegate(MissionObjectiveMarkerVM t)
					{
						t.IsEnabled = value;
					});
				}
			}
		}

		// Token: 0x0400026E RID: 622
		private readonly Camera _missionCamera;

		// Token: 0x0400026F RID: 623
		private readonly MissionObjectiveMarkersVM.MarkerDistanceComparer _distanceComparer;

		// Token: 0x04000270 RID: 624
		private readonly MissionObjectiveLogic _objectiveLogic;

		// Token: 0x04000271 RID: 625
		private MissionObjective _latestObjective;

		// Token: 0x04000272 RID: 626
		private MBBindingList<MissionObjectiveMarkerVM> _targets;

		// Token: 0x04000273 RID: 627
		private bool _isEnabled;

		// Token: 0x020000DB RID: 219
		private class MarkerDistanceComparer : IComparer<MissionObjectiveMarkerVM>
		{
			// Token: 0x06000CCE RID: 3278 RVA: 0x00029BBC File Offset: 0x00027DBC
			public int Compare(MissionObjectiveMarkerVM x, MissionObjectiveMarkerVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
