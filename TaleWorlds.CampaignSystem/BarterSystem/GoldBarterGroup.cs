using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200049A RID: 1178
	public class GoldBarterGroup : BarterGroup
	{
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x0018144C File Offset: 0x0017F64C
		public override float AIDecisionWeight
		{
			get
			{
				return 0.6f;
			}
		}
	}
}
