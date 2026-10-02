using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A8 RID: 168
	public class ChangeLocationBehavior : AgentBehavior
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0002F488 File Offset: 0x0002D688
		public ChangeLocationBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._initializeTime = base.Mission.CurrentTime;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002F4B4 File Offset: 0x0002D6B4
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._selectedDoor == null)
			{
				Passage passage = this.SelectADoor();
				if (passage != null)
				{
					this._selectedDoor = passage;
					base.Navigator.SetTarget(this._selectedDoor, false, Agent.AIScriptedFrameFlags.None);
					return;
				}
			}
			else if (this._selectedDoor.ToLocation.CharacterCount >= this._selectedDoor.ToLocation.ProsperityMax)
			{
				base.Navigator.SetTarget(null, false, Agent.AIScriptedFrameFlags.None);
				base.Navigator.ForceThink(0f);
				this._selectedDoor = null;
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002F538 File Offset: 0x0002D738
		private Passage SelectADoor()
		{
			Passage passage = null;
			List<Passage> list = new List<Passage>();
			foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
			{
				Passage passage2 = (Passage)usableMachine;
				if (passage2.GetVacantStandingPointForAI(base.OwnerAgent) != null && passage2.ToLocation.CharacterCount < passage2.ToLocation.ProsperityMax)
				{
					list.Add(passage2);
				}
			}
			if (list.Count > 0)
			{
				passage = list[MBRandom.RandomInt(list.Count)];
			}
			return passage;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002F5E0 File Offset: 0x0002D7E0
		protected override void OnActivate()
		{
			base.OnActivate();
			this._selectedDoor = null;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002F5EF File Offset: 0x0002D7EF
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._selectedDoor = null;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002F5FE File Offset: 0x0002D7FE
		public override string GetDebugInfo()
		{
			if (this._selectedDoor != null)
			{
				return "Go to " + this._selectedDoor.ToLocation.StringId;
			}
			return "Change Location no target";
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002F628 File Offset: 0x0002D828
		public override float GetAvailability(bool isSimulation)
		{
			float num = 0f;
			bool flag = false;
			bool flag2 = false;
			LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.OwnerAgent.Origin);
			if (base.Mission.CurrentTime < 5f || locationCharacter.FixedLocation || !this._missionAgentHandler.HasPassages())
			{
				return 0f;
			}
			foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
			{
				Passage passage = usableMachine as Passage;
				if (passage.ToLocation.CanAIEnter(locationCharacter) && passage.ToLocation.CharacterCount < passage.ToLocation.ProsperityMax)
				{
					flag = true;
					if (passage.PilotStandingPoint.GameEntity.GetGlobalFrame().origin.Distance(base.OwnerAgent.Position) < 1f)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					num = (CampaignMission.Current.Location.IsIndoor ? 0.1f : 0.05f);
				}
				else if (base.Mission.CurrentTime - this._initializeTime > 10f)
				{
					num = 0.01f;
				}
			}
			return num;
		}

		// Token: 0x040003B2 RID: 946
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003B3 RID: 947
		private readonly float _initializeTime;

		// Token: 0x040003B4 RID: 948
		private Passage _selectedDoor;
	}
}
