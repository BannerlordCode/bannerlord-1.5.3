using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Missions.NameMarkers
{
	// Token: 0x02000034 RID: 52
	public class StealthNameMarkerProvider : MissionNameMarkerProvider
	{
		// Token: 0x060001AA RID: 426 RVA: 0x00013013 File Offset: 0x00011213
		protected override void OnInitialize(Mission mission)
		{
			base.OnInitialize(mission);
			this._stealthAreaMissionLogic = mission.GetMissionBehavior<StealthAreaMissionLogic>();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00013028 File Offset: 0x00011228
		protected override void OnDestroy(Mission mission)
		{
			base.OnDestroy(mission);
			this._stealthAreaMissionLogic = null;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00013038 File Offset: 0x00011238
		public override void CreateMarkers(List<MissionNameMarkerTargetBaseVM> markers)
		{
			this.CreateStealthAreaMarkers(markers);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00013044 File Offset: 0x00011244
		private void CreateStealthAreaMarkers(List<MissionNameMarkerTargetBaseVM> markers)
		{
			if (this._stealthAreaMissionLogic == null)
			{
				return;
			}
			if (Mission.Current == null)
			{
				return;
			}
			if (Agent.Main != null)
			{
				foreach (StealthAreaUsePoint stealthAreaUsePoint in Mission.Current.ActiveMissionObjects.FindAllWithType<StealthAreaUsePoint>())
				{
					if (stealthAreaUsePoint.IsUsableByAgent(Agent.Main))
					{
						MissionStealthAreaUsePointNameMarkerTargetVM missionStealthAreaUsePointNameMarkerTargetVM = new MissionStealthAreaUsePointNameMarkerTargetVM(stealthAreaUsePoint);
						markers.Add(missionStealthAreaUsePointNameMarkerTargetVM);
					}
				}
			}
		}

		// Token: 0x040000F8 RID: 248
		private StealthAreaMissionLogic _stealthAreaMissionLogic;
	}
}
