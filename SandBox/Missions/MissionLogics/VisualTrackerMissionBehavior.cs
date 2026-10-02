using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200008B RID: 139
	public class VisualTrackerMissionBehavior : MissionLogic
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x00023F05 File Offset: 0x00022105
		public override void OnAgentCreated(Agent agent)
		{
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00023F07 File Offset: 0x00022107
		public override void AfterStart()
		{
			this.Refresh();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00023F0F File Offset: 0x0002210F
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._visualTrackerManager.TrackedObjectsVersion != this._trackedObjectsVersion)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00023F31 File Offset: 0x00022131
		private void Refresh()
		{
			if (PlayerEncounter.LocationEncounter != null)
			{
				this.RefreshCommonAreas();
			}
			this._trackedObjectsVersion = this._visualTrackerManager.TrackedObjectsVersion;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00023F54 File Offset: 0x00022154
		public void RegisterLocalOnlyObject(ITrackableBase obj)
		{
			using (List<TrackedObject>.Enumerator enumerator = this._currentTrackedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Object == obj)
					{
						return;
					}
				}
			}
			this._currentTrackedObjects.Add(new TrackedObject(obj));
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00023FBC File Offset: 0x000221BC
		private void RefreshCommonAreas()
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			foreach (CommonAreaMarker commonAreaMarker in base.Mission.ActiveMissionObjects.FindAllWithType<CommonAreaMarker>().ToList<CommonAreaMarker>())
			{
				if (settlement.Alleys.Count >= commonAreaMarker.AreaIndex)
				{
					this.RegisterLocalOnlyObject(commonAreaMarker);
				}
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0002403C File Offset: 0x0002223C
		public override List<CompassItemUpdateParams> GetCompassTargets()
		{
			List<CompassItemUpdateParams> list = new List<CompassItemUpdateParams>();
			foreach (TrackedObject trackedObject in this._currentTrackedObjects)
			{
				list.Add(new CompassItemUpdateParams(trackedObject.Object, TargetIconType.Flag_A, trackedObject.Position, 4288256409U, uint.MaxValue));
			}
			return list;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000240B0 File Offset: 0x000222B0
		private void RemoveLocalObject(ITrackableBase obj)
		{
			this._currentTrackedObjects.RemoveAll((TrackedObject x) => x.Object == obj);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000240E2 File Offset: 0x000222E2
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			this.RemoveLocalObject(affectedAgent);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000240EB File Offset: 0x000222EB
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			this.RemoveLocalObject(affectedAgent);
		}

		// Token: 0x040002D0 RID: 720
		private List<TrackedObject> _currentTrackedObjects = new List<TrackedObject>();

		// Token: 0x040002D1 RID: 721
		private int _trackedObjectsVersion = -1;

		// Token: 0x040002D2 RID: 722
		private readonly VisualTrackerManager _visualTrackerManager = Campaign.Current.VisualTrackerManager;

		// Token: 0x02000196 RID: 406
		public enum AgentTrackTypes
		{
			// Token: 0x0400077C RID: 1916
			AvailableIssue,
			// Token: 0x0400077D RID: 1917
			ActiveIssue,
			// Token: 0x0400077E RID: 1918
			ActiveStoryQuest,
			// Token: 0x0400077F RID: 1919
			TrackedIssue,
			// Token: 0x04000780 RID: 1920
			TrackedStoryQuest
		}
	}
}
