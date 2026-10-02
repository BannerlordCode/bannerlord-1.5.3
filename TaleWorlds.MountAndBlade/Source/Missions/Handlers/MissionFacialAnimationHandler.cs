using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003E5 RID: 997
	public class MissionFacialAnimationHandler : MissionLogic
	{
		// Token: 0x0600377E RID: 14206 RVA: 0x000E692D File Offset: 0x000E4B2D
		public override void EarlyStart()
		{
			this._animRefreshTimer = new Timer(base.Mission.CurrentTime, 5f, true);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x000E694B File Offset: 0x000E4B4B
		public override void AfterStart()
		{
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x000E694D File Offset: 0x000E4B4D
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000E6950 File Offset: 0x000E4B50
		private void SetDefaultFacialAnimationsForAllAgents()
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsActive() && agent.IsHuman)
				{
					agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.Low, "idle_tired", true);
				}
			}
		}

		// Token: 0x040017F9 RID: 6137
		private Timer _animRefreshTimer;
	}
}
