using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000126 RID: 294
	public class DefaultIncidentModel : IncidentModel
	{
		// Token: 0x060018C9 RID: 6345 RVA: 0x00078973 File Offset: 0x00076B73
		public override CampaignTime GetMinGlobalCooldownTime()
		{
			return CampaignTime.Days(8f);
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0007897F File Offset: 0x00076B7F
		public override CampaignTime GetMaxGlobalCooldownTime()
		{
			return CampaignTime.Days(15f);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0007898B File Offset: 0x00076B8B
		public override float GetIncidentTriggerGlobalProbability()
		{
			return 0.5f;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00078992 File Offset: 0x00076B92
		public override float GetIncidentTriggerProbabilityDuringSiege()
		{
			return 0.143f;
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00078999 File Offset: 0x00076B99
		public override float GetIncidentTriggerProbabilityDuringWait()
		{
			return 0.143f;
		}
	}
}
