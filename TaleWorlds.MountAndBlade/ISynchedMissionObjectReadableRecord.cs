using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036E RID: 878
	public interface ISynchedMissionObjectReadableRecord
	{
		// Token: 0x060032A5 RID: 12965
		bool ReadFromNetwork(ref bool bufferReadValid);
	}
}
