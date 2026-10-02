using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014F RID: 335
	public interface IDetachment
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x0600114C RID: 4428
		MBReadOnlyList<Formation> UserFormations { get; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600114D RID: 4429
		bool IsLoose { get; }

		// Token: 0x0600114E RID: 4430
		bool IsAgentUsingOrInterested(Agent agent);

		// Token: 0x0600114F RID: 4431
		float? GetWeightOfNextSlot(BattleSideEnum side);

		// Token: 0x06001150 RID: 4432
		float GetDetachmentWeight(BattleSideEnum side);

		// Token: 0x06001151 RID: 4433
		float ComputeAndCacheDetachmentWeight(BattleSideEnum side);

		// Token: 0x06001152 RID: 4434
		float GetDetachmentWeightFromCache();

		// Token: 0x06001153 RID: 4435
		void GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples);

		// Token: 0x06001154 RID: 4436
		bool IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent);

		// Token: 0x06001155 RID: 4437
		bool IsAgentEligible(Agent agent);

		// Token: 0x06001156 RID: 4438
		void AddAgentAtSlotIndex(Agent agent, int slotIndex);

		// Token: 0x06001157 RID: 4439
		Agent GetMovingAgentAtSlotIndex(int slotIndex);

		// Token: 0x06001158 RID: 4440
		void MarkSlotAtIndex(int slotIndex);

		// Token: 0x06001159 RID: 4441
		bool IsDetachmentRecentlyEvaluated();

		// Token: 0x0600115A RID: 4442
		void UnmarkDetachment();

		// Token: 0x0600115B RID: 4443
		float? GetWeightOfAgentAtNextSlot(List<Agent> candidates, out Agent match);

		// Token: 0x0600115C RID: 4444
		float? GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> agentTemplateScores, out Agent match);

		// Token: 0x0600115D RID: 4445
		float GetTemplateWeightOfAgent(Agent candidate);

		// Token: 0x0600115E RID: 4446
		List<float> GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue);

		// Token: 0x0600115F RID: 4447
		float GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex);

		// Token: 0x06001160 RID: 4448
		float GetWeightOfOccupiedSlot(Agent detachedAgent);

		// Token: 0x06001161 RID: 4449
		float? GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> candidates, out Agent match);

		// Token: 0x06001162 RID: 4450
		bool IsStandingPointAvailableForAgent(Agent agent);

		// Token: 0x06001163 RID: 4451
		void AddAgent(Agent agent, int slotIndex = -1, Agent.AIScriptedFrameFlags customFlags = Agent.AIScriptedFrameFlags.None);

		// Token: 0x06001164 RID: 4452
		void RemoveAgent(Agent detachedAgent);

		// Token: 0x06001165 RID: 4453
		int GetNumberOfUsableSlots();

		// Token: 0x06001166 RID: 4454
		void FormationStartUsing(Formation formation);

		// Token: 0x06001167 RID: 4455
		void FormationStopUsing(Formation formation);

		// Token: 0x06001168 RID: 4456
		bool IsUsedByFormation(Formation formation);

		// Token: 0x06001169 RID: 4457
		WorldFrame? GetAgentFrame(Agent detachedAgent);

		// Token: 0x0600116A RID: 4458
		void ResetEvaluation();

		// Token: 0x0600116B RID: 4459
		bool IsEvaluated();

		// Token: 0x0600116C RID: 4460
		void SetAsEvaluated();

		// Token: 0x0600116D RID: 4461
		void OnFormationLeave(Formation formation);
	}
}
