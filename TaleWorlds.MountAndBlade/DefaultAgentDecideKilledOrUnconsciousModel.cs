using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000201 RID: 513
	public class DefaultAgentDecideKilledOrUnconsciousModel : AgentDecideKilledOrUnconsciousModel
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x00066911 File Offset: 0x00064B11
		public override float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability)
		{
			useSurgeryProbability = 0f;
			return 1f;
		}
	}
}
