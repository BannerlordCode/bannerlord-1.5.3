using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200049D RID: 1181
	public class PrisonerBarterGroup : BarterGroup
	{
		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x00181479 File Offset: 0x0017F679
		public override float AIDecisionWeight
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
