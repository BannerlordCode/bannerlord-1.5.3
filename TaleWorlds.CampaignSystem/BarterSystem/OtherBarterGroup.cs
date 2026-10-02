using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200049E RID: 1182
	public class OtherBarterGroup : BarterGroup
	{
		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06004BB9 RID: 19385 RVA: 0x00181488 File Offset: 0x0017F688
		public override float AIDecisionWeight
		{
			get
			{
				return 0.25f;
			}
		}
	}
}
