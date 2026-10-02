using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026D RID: 621
	public interface IVehicleHandler : IMissionBehavior
	{
		// Token: 0x060022F8 RID: 8952
		bool IsAgentInVehicle(Agent agent, out WeakGameEntity vehicleEntity);
	}
}
