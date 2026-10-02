using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000095 RID: 149
	public interface IAgentBehaviorManager
	{
		// Token: 0x060012D0 RID: 4816
		void AddQuestCharacterBehaviors(IAgent agent);

		// Token: 0x060012D1 RID: 4817
		void AddWandererBehaviors(IAgent agent);

		// Token: 0x060012D2 RID: 4818
		void AddOutdoorWandererBehaviors(IAgent agent);

		// Token: 0x060012D3 RID: 4819
		void AddIndoorWandererBehaviors(IAgent agent);

		// Token: 0x060012D4 RID: 4820
		void AddFixedCharacterBehaviors(IAgent agent);

		// Token: 0x060012D5 RID: 4821
		void AddPatrollingThugBehaviors(IAgent agent);

		// Token: 0x060012D6 RID: 4822
		void AddStandGuardBehaviors(IAgent agent);

		// Token: 0x060012D7 RID: 4823
		void AddFixedGuardBehaviors(IAgent agent);

		// Token: 0x060012D8 RID: 4824
		void AddStealthAgentBehaviors(IAgent agent);

		// Token: 0x060012D9 RID: 4825
		void AddPatrollingGuardBehaviors(IAgent agent);

		// Token: 0x060012DA RID: 4826
		void AddCompanionBehaviors(IAgent agent);

		// Token: 0x060012DB RID: 4827
		void AddBodyguardBehaviors(IAgent agent);

		// Token: 0x060012DC RID: 4828
		void AddFirstCompanionBehavior(IAgent agent);
	}
}
