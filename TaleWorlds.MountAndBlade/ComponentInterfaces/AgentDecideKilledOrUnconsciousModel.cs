using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000402 RID: 1026
	public abstract class AgentDecideKilledOrUnconsciousModel : MBGameModel<AgentDecideKilledOrUnconsciousModel>
	{
		// Token: 0x06003860 RID: 14432
		public abstract float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability);
	}
}
