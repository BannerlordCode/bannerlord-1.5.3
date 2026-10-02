using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000034 RID: 52
	public class MissionNameMarkerVM : ViewModel
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00010ECD File Offset: 0x0000F0CD
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x00010ED5 File Offset: 0x0000F0D5
		public bool IsTargetsAdded { get; private set; }

		// Token: 0x06000408 RID: 1032 RVA: 0x00010EDE File Offset: 0x0000F0DE
		public MissionNameMarkerVM(List<MissionNameMarkerProvider> providers, Camera missionCamera)
		{
			this.Targets = new MBBindingList<MissionNameMarkerTargetBaseVM>();
			this._providers = providers;
			this._distanceComparer = new MissionNameMarkerVM.MarkerDistanceComparer();
			this._missionCamera = missionCamera;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010F0A File Offset: 0x0000F10A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Targets.ApplyActionOnAllItems(delegate(MissionNameMarkerTargetBaseVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010F3C File Offset: 0x0000F13C
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Targets.ApplyActionOnAllItems(delegate(MissionNameMarkerTargetBaseVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010F70 File Offset: 0x0000F170
		public void Tick(float dt)
		{
			if (!this.IsTargetsAdded)
			{
				List<MissionNameMarkerTargetBaseVM> list = new List<MissionNameMarkerTargetBaseVM>();
				for (int i = 0; i < this._providers.Count; i++)
				{
					this._providers[i].CreateMarkers(list);
				}
				MBReadOnlyList<MissionNameMarkerTargetBaseVM> mbreadOnlyList;
				MBReadOnlyList<MissionNameMarkerTargetBaseVM> mbreadOnlyList2;
				MissionNameMarkerVM.GetTargetDifferences(this.Targets, list, out mbreadOnlyList, out mbreadOnlyList2);
				for (int j = 0; j < mbreadOnlyList.Count; j++)
				{
					this.Targets.Remove(mbreadOnlyList[j]);
				}
				for (int k = 0; k < mbreadOnlyList2.Count; k++)
				{
					this.Targets.Add(mbreadOnlyList2[k]);
				}
				this.IsTargetsAdded = true;
			}
			if (this.IsEnabled)
			{
				this.UpdateTargetScreenPositions(false);
				this._fadeOutTimerStarted = false;
				this._fadeOutTimer = 0f;
				this._prevEnabledState = this.IsEnabled;
			}
			else
			{
				if (this._prevEnabledState)
				{
					this._fadeOutTimerStarted = true;
				}
				if (this._fadeOutTimerStarted)
				{
					this._fadeOutTimer += dt;
				}
				if (this._fadeOutTimer >= 2f)
				{
					this._fadeOutTimerStarted = false;
				}
				this.UpdateTargetScreenPositions(this._fadeOutTimer < 2f);
			}
			this._prevEnabledState = this.IsEnabled;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000110A8 File Offset: 0x0000F2A8
		private static void GetTargetDifferences(IList<MissionNameMarkerTargetBaseVM> currentTargets, IList<MissionNameMarkerTargetBaseVM> newTargets, out MBReadOnlyList<MissionNameMarkerTargetBaseVM> removedTargets, out MBReadOnlyList<MissionNameMarkerTargetBaseVM> addedTargets)
		{
			MBList<MissionNameMarkerTargetBaseVM> mblist = new MBList<MissionNameMarkerTargetBaseVM>();
			MBList<MissionNameMarkerTargetBaseVM> mblist2 = new MBList<MissionNameMarkerTargetBaseVM>();
			for (int i = 0; i < currentTargets.Count; i++)
			{
				MissionNameMarkerTargetBaseVM missionNameMarkerTargetBaseVM = currentTargets[i];
				bool flag = true;
				for (int j = 0; j < newTargets.Count; j++)
				{
					MissionNameMarkerTargetBaseVM missionNameMarkerTargetBaseVM2 = newTargets[j];
					if (missionNameMarkerTargetBaseVM.Equals(missionNameMarkerTargetBaseVM2))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					mblist.Add(missionNameMarkerTargetBaseVM);
				}
			}
			for (int k = 0; k < newTargets.Count; k++)
			{
				MissionNameMarkerTargetBaseVM missionNameMarkerTargetBaseVM3 = newTargets[k];
				bool flag2 = true;
				for (int l = 0; l < currentTargets.Count; l++)
				{
					if (currentTargets[l].Equals(missionNameMarkerTargetBaseVM3))
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					mblist2.Add(missionNameMarkerTargetBaseVM3);
				}
			}
			removedTargets = mblist;
			addedTargets = mblist2;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00011175 File Offset: 0x0000F375
		public void SetTargetsDirty()
		{
			this.IsTargetsAdded = false;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00011180 File Offset: 0x0000F380
		private void UpdateTargetScreenPositions(bool forceUpdate)
		{
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionNameMarkerTargetBaseVM missionNameMarkerTargetBaseVM = this.Targets[i];
				if (missionNameMarkerTargetBaseVM.IsEnabled || forceUpdate)
				{
					missionNameMarkerTargetBaseVM.UpdatePosition(this._missionCamera);
				}
			}
			this.Targets.Sort(this._distanceComparer);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000111D8 File Offset: 0x0000F3D8
		private void UpdateTargetStates(bool state)
		{
			foreach (MissionNameMarkerTargetBaseVM missionNameMarkerTargetBaseVM in this.Targets)
			{
				missionNameMarkerTargetBaseVM.SetEnabledState(state);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00011224 File Offset: 0x0000F424
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0001122C File Offset: 0x0000F42C
		[DataSourceProperty]
		public MBBindingList<MissionNameMarkerTargetBaseVM> Targets
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
					base.OnPropertyChangedWithValue<MBBindingList<MissionNameMarkerTargetBaseVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001124A File Offset: 0x0000F44A
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00011252 File Offset: 0x0000F452
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
					this.UpdateTargetStates(value);
					Game.Current.EventManager.TriggerEvent<MissionNameMarkerToggleEvent>(new MissionNameMarkerToggleEvent(value));
				}
			}
		}

		// Token: 0x0400021B RID: 539
		private readonly Camera _missionCamera;

		// Token: 0x0400021C RID: 540
		private bool _prevEnabledState;

		// Token: 0x0400021D RID: 541
		private bool _fadeOutTimerStarted;

		// Token: 0x0400021E RID: 542
		private float _fadeOutTimer;

		// Token: 0x0400021F RID: 543
		private readonly MissionNameMarkerVM.MarkerDistanceComparer _distanceComparer;

		// Token: 0x04000220 RID: 544
		private readonly List<MissionNameMarkerProvider> _providers;

		// Token: 0x04000221 RID: 545
		private MBBindingList<MissionNameMarkerTargetBaseVM> _targets;

		// Token: 0x04000222 RID: 546
		private bool _isEnabled;

		// Token: 0x020000A3 RID: 163
		private class MarkerDistanceComparer : IComparer<MissionNameMarkerTargetBaseVM>
		{
			// Token: 0x0600072A RID: 1834 RVA: 0x0001885C File Offset: 0x00016A5C
			public int Compare(MissionNameMarkerTargetBaseVM x, MissionNameMarkerTargetBaseVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
