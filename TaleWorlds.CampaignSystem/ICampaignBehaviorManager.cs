using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000098 RID: 152
	public interface ICampaignBehaviorManager
	{
		// Token: 0x060012EE RID: 4846
		void RegisterEvents();

		// Token: 0x060012EF RID: 4847
		T GetBehavior<T>();

		// Token: 0x060012F0 RID: 4848
		IEnumerable<T> GetBehaviors<T>();

		// Token: 0x060012F1 RID: 4849
		void AddBehavior(CampaignBehaviorBase campaignBehavior);

		// Token: 0x060012F2 RID: 4850
		void RemoveBehavior<T>() where T : CampaignBehaviorBase;

		// Token: 0x060012F3 RID: 4851
		void ClearBehaviors();

		// Token: 0x060012F4 RID: 4852
		void LoadBehaviorData();

		// Token: 0x060012F5 RID: 4853
		void InitializeCampaignBehaviors(IEnumerable<CampaignBehaviorBase> inputComponents);
	}
}
