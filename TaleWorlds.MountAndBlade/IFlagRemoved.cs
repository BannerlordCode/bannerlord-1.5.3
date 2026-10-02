using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000268 RID: 616
	public interface IFlagRemoved : IMissionBehavior
	{
		// Token: 0x060022EA RID: 8938
		void OnFlagsRemoved(int remainingFlagIndex);
	}
}
