using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000375 RID: 885
	public class ClearHandInverseKinematicsOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x0600330D RID: 13069 RVA: 0x000D13D5 File Offset: 0x000CF5D5
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			userAgent.ClearHandInverseKinematics();
		}
	}
}
