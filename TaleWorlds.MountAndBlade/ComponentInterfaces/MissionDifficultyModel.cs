using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000403 RID: 1027
	public abstract class MissionDifficultyModel : MBGameModel<MissionDifficultyModel>
	{
		// Token: 0x06003862 RID: 14434
		public abstract float GetDamageMultiplierOfCombatDifficulty(Agent victimAgent, Agent attackerAgent = null);
	}
}
