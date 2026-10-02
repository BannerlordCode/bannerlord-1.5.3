using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026C RID: 620
	public interface IPlayerInputEffector : IMissionBehavior
	{
		// Token: 0x060022F7 RID: 8951
		Agent.EventControlFlag OnCollectPlayerEventControlFlags();
	}
}
