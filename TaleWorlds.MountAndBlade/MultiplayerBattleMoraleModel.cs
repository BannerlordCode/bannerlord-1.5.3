using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020B RID: 523
	public class MultiplayerBattleMoraleModel : BattleMoraleModel
	{
		// Token: 0x06001E7B RID: 7803 RVA: 0x00068A25 File Offset: 0x00066C25
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow)
		{
			return new ValueTuple<float, float>(0f, 0f);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x00068A36 File Offset: 0x00066C36
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent)
		{
			return new ValueTuple<float, float>(0f, 0f);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x00068A47 File Offset: 0x00066C47
		public override float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange)
		{
			return 0f;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x00068A4E File Offset: 0x00066C4E
		public override float GetEffectiveInitialMorale(Agent agent, float baseMorale)
		{
			return baseMorale;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00068A51 File Offset: 0x00066C51
		public override bool CanPanicDueToMorale(Agent agent)
		{
			return true;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x00068A54 File Offset: 0x00066C54
		public override float CalculateCasualtiesFactor(BattleSideEnum battleSide)
		{
			return 1f;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x00068A5B File Offset: 0x00066C5B
		public override float GetAverageMorale(Formation formation)
		{
			return 0f;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x00068A62 File Offset: 0x00066C62
		public override float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin)
		{
			return 0f;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x00068A69 File Offset: 0x00066C69
		public override float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip)
		{
			return agent.GetMorale();
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x00068A71 File Offset: 0x00066C71
		public override float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip)
		{
			return agent.GetMorale();
		}
	}
}
