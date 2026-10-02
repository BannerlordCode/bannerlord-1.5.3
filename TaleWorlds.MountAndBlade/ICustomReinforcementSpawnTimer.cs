using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028D RID: 653
	public interface ICustomReinforcementSpawnTimer
	{
		// Token: 0x06002492 RID: 9362
		bool Check(BattleSideEnum side);

		// Token: 0x06002493 RID: 9363
		void ResetTimer(BattleSideEnum side);
	}
}
