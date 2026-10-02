using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000405 RID: 1029
	public abstract class BattleMoraleModel : MBGameModel<BattleMoraleModel>
	{
		// Token: 0x06003868 RID: 14440
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public abstract ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow);

		// Token: 0x06003869 RID: 14441
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public abstract ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent);

		// Token: 0x0600386A RID: 14442
		public abstract float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange);

		// Token: 0x0600386B RID: 14443
		public abstract float GetEffectiveInitialMorale(Agent agent, float baseMorale);

		// Token: 0x0600386C RID: 14444
		public abstract bool CanPanicDueToMorale(Agent agent);

		// Token: 0x0600386D RID: 14445
		public abstract float CalculateCasualtiesFactor(BattleSideEnum battleSide);

		// Token: 0x0600386E RID: 14446
		public abstract float GetAverageMorale(Formation formation);

		// Token: 0x0600386F RID: 14447
		public abstract float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin);

		// Token: 0x06003870 RID: 14448
		public abstract float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip);

		// Token: 0x06003871 RID: 14449
		public abstract float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip);

		// Token: 0x04001843 RID: 6211
		public const float BaseMoraleGainOnKill = 3f;

		// Token: 0x04001844 RID: 6212
		public const float BaseMoraleLossOnKill = 4f;

		// Token: 0x04001845 RID: 6213
		public const float BaseMoraleGainOnPanic = 2f;

		// Token: 0x04001846 RID: 6214
		public const float BaseMoraleLossOnPanic = 1.1f;

		// Token: 0x04001847 RID: 6215
		public const float MeleeWeaponMoraleMultiplier = 0.75f;

		// Token: 0x04001848 RID: 6216
		public const float RangedWeaponMoraleMultiplier = 0.5f;

		// Token: 0x04001849 RID: 6217
		public const float SiegeWeaponMoraleMultiplier = 0.25f;

		// Token: 0x0400184A RID: 6218
		public const float BurningSiegeWeaponMoraleBonus = 0.25f;

		// Token: 0x0400184B RID: 6219
		public const float CasualtyFactorRate = 2f;
	}
}
