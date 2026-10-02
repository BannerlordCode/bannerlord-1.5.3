using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FF RID: 1023
	public abstract class MissionShipParametersModel : MBGameModel<MissionShipParametersModel>
	{
		// Token: 0x06003834 RID: 14388
		public abstract int CalculateMainDeckCrewSize(IShipOrigin shipOrigin, Agent formationUnit);

		// Token: 0x06003835 RID: 14389
		public abstract float CalculateWindBonus(IShipOrigin shipOrigin, Agent captain, float baseSailForceMagnitude);

		// Token: 0x06003836 RID: 14390
		public abstract float CalculateOarForceMultiplier(Agent pilotAgent, float baseOarForce);
	}
}
