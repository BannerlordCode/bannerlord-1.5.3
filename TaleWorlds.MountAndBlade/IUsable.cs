using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037F RID: 895
	public interface IUsable
	{
		// Token: 0x0600332B RID: 13099
		void OnUse(Agent userAgent, sbyte agentBoneIndex);

		// Token: 0x0600332C RID: 13100
		void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex);
	}
}
