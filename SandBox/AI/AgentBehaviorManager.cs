using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.AI
{
	// Token: 0x0200010E RID: 270
	public class AgentBehaviorManager : IAgentBehaviorManager
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x00061D00 File Offset: 0x0005FF00
		public void AddQuestCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddQuestCharacterBehaviors(agent);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00061D08 File Offset: 0x0005FF08
		void IAgentBehaviorManager.AddWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddWandererBehaviors(agent);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00061D10 File Offset: 0x0005FF10
		void IAgentBehaviorManager.AddOutdoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddOutdoorWandererBehaviors(agent);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00061D18 File Offset: 0x0005FF18
		void IAgentBehaviorManager.AddIndoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddIndoorWandererBehaviors(agent);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00061D20 File Offset: 0x0005FF20
		void IAgentBehaviorManager.AddFixedCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddFixedCharacterBehaviors(agent);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00061D28 File Offset: 0x0005FF28
		void IAgentBehaviorManager.AddPatrollingThugBehaviors(IAgent agent)
		{
			BehaviorSets.AddPatrollingThugBehaviors(agent);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00061D30 File Offset: 0x0005FF30
		void IAgentBehaviorManager.AddStandGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddStandGuardBehaviors(agent);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00061D38 File Offset: 0x0005FF38
		void IAgentBehaviorManager.AddFixedGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddFixedGuardBehaviors(agent);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00061D40 File Offset: 0x0005FF40
		void IAgentBehaviorManager.AddStealthAgentBehaviors(IAgent agent)
		{
			BehaviorSets.StealthAgentBehaviors(agent);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00061D48 File Offset: 0x0005FF48
		void IAgentBehaviorManager.AddPatrollingGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddPatrollingGuardBehaviors(agent);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00061D50 File Offset: 0x0005FF50
		void IAgentBehaviorManager.AddCompanionBehaviors(IAgent agent)
		{
			BehaviorSets.AddCompanionBehaviors(agent);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00061D58 File Offset: 0x0005FF58
		void IAgentBehaviorManager.AddBodyguardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBodyguardBehaviors(agent);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00061D60 File Offset: 0x0005FF60
		public void AddFirstCompanionBehavior(IAgent agent)
		{
			BehaviorSets.AddFirstCompanionBehavior(agent);
		}
	}
}
