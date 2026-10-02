using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B4 RID: 180
	public class StandGuardBehavior : AgentBehavior
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x00032F3B File Offset: 0x0003113B
		public StandGuardBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00032F58 File Offset: 0x00031158
		public override void Tick(float dt, bool isSimulation)
		{
			if (base.OwnerAgent.CurrentWatchState == Agent.WatchState.Patrolling)
			{
				if (this._standPoint == null || isSimulation)
				{
					UsableMachine usableMachine = this._oldStandPoint ?? this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, base.Navigator.SpecialTargetTag);
					if (usableMachine != null)
					{
						this._oldStandPoint = null;
						this._standPoint = usableMachine;
						base.Navigator.SetTarget(this._standPoint, false, Agent.AIScriptedFrameFlags.None);
						return;
					}
				}
			}
			else if (this._standPoint != null)
			{
				this._oldStandPoint = this._standPoint;
				base.Navigator.SetTarget(null, false, Agent.AIScriptedFrameFlags.None);
				this._standPoint = null;
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00032FF4 File Offset: 0x000311F4
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this._standPoint = null;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00033008 File Offset: 0x00031208
		public override float GetAvailability(bool isSimulation)
		{
			return 1f;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0003300F File Offset: 0x0003120F
		public override string GetDebugInfo()
		{
			return "Guard stand";
		}

		// Token: 0x04000404 RID: 1028
		private UsableMachine _oldStandPoint;

		// Token: 0x04000405 RID: 1029
		private UsableMachine _standPoint;

		// Token: 0x04000406 RID: 1030
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x020001C6 RID: 454
		private enum GuardState
		{
			// Token: 0x0400081D RID: 2077
			StandIdle,
			// Token: 0x0400081E RID: 2078
			StandAttention,
			// Token: 0x0400081F RID: 2079
			StandCautious,
			// Token: 0x04000820 RID: 2080
			GotToStandPoint
		}
	}
}
