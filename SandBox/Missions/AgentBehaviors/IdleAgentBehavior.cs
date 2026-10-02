using System;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AE RID: 174
	public class IdleAgentBehavior : AgentBehavior
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x00031A65 File Offset: 0x0002FC65
		public IdleAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00031A6E File Offset: 0x0002FC6E
		public override float GetAvailability(bool isSimulation)
		{
			return 1f;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00031A78 File Offset: 0x0002FC78
		protected override void OnActivate()
		{
			base.OwnerAgent.SetIsAIPaused(true);
			base.OwnerAgent.SetTargetPosition(base.OwnerAgent.GetWorldPosition().AsVec2);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00031AAF File Offset: 0x0002FCAF
		protected override void OnDeactivate()
		{
			base.OwnerAgent.SetIsAIPaused(false);
			base.OwnerAgent.ClearTargetFrame();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00031AC8 File Offset: 0x0002FCC8
		public override string GetDebugInfo()
		{
			return "Idle Behavior";
		}
	}
}
