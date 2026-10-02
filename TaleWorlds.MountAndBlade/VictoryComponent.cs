using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010E RID: 270
	public class VictoryComponent : AgentComponent
	{
		// Token: 0x06000DCB RID: 3531 RVA: 0x0001A60C File Offset: 0x0001880C
		public VictoryComponent(Agent agent, RandomTimer timer)
			: base(agent)
		{
			this._timer = timer;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0001A61C File Offset: 0x0001881C
		public bool CheckTimer()
		{
			return this._timer.Check(Mission.Current.CurrentTime);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0001A633 File Offset: 0x00018833
		public void ChangeTimerDuration(float min, float max)
		{
			this._timer.ChangeDuration(min, max);
		}

		// Token: 0x0400031D RID: 797
		private readonly RandomTimer _timer;
	}
}
