using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000264 RID: 612
	public interface IMissionAgentSpawnLogic : IMissionBehavior
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060022D9 RID: 8921
		BattleSideEnum PlayerSide { get; }

		// Token: 0x060022DA RID: 8922
		void StartSpawner(BattleSideEnum side);

		// Token: 0x060022DB RID: 8923
		void StopSpawner(BattleSideEnum side);

		// Token: 0x060022DC RID: 8924
		bool IsSideSpawnEnabled(BattleSideEnum side);

		// Token: 0x060022DD RID: 8925
		bool IsSideDepleted(BattleSideEnum side);

		// Token: 0x060022DE RID: 8926
		float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None);

		// Token: 0x060022DF RID: 8927
		IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side);

		// Token: 0x060022E0 RID: 8928
		bool GetSpawnHorses(BattleSideEnum side);

		// Token: 0x060022E1 RID: 8929
		int GetNumberOfPlayerControllableTroops();
	}
}
