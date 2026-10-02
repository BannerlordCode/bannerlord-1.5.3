using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026B RID: 619
	public interface IBattlePowerCalculationLogic : IMissionBehavior
	{
		// Token: 0x060022F6 RID: 8950
		float GetTotalTeamPower(Team team);
	}
}
