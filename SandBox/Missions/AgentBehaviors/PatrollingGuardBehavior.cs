using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B2 RID: 178
	public class PatrollingGuardBehavior : AgentBehavior
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x00032727 File Offset: 0x00030927
		public PatrollingGuardBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00032744 File Offset: 0x00030944
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._target == null)
			{
				UsableMachine usableMachine = ((base.Navigator.SpecialTargetTag == null || base.Navigator.SpecialTargetTag.IsEmpty<char>()) ? this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, "npc_common") : this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, base.Navigator.SpecialTargetTag));
				if (usableMachine != null)
				{
					this._target = usableMachine;
					base.Navigator.SetTarget(this._target, false, Agent.AIScriptedFrameFlags.None);
					return;
				}
			}
			else if (base.Navigator.TargetUsableMachine == null)
			{
				base.Navigator.SetTarget(this._target, false, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000327EB File Offset: 0x000309EB
		public override float GetAvailability(bool isSimulation)
		{
			if (this._missionAgentHandler.GetAllUsablePointsWithTag(base.Navigator.SpecialTargetTag).Count <= 0)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00032816 File Offset: 0x00030A16
		protected override void OnDeactivate()
		{
			this._target = null;
			base.Navigator.ClearTarget();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0003282A File Offset: 0x00030A2A
		public override string GetDebugInfo()
		{
			return "Guard patrol";
		}

		// Token: 0x040003F3 RID: 1011
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003F4 RID: 1012
		private UsableMachine _target;
	}
}
