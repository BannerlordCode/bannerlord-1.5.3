using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000209 RID: 521
	public abstract class IncidentModel : MBGameModel<IncidentModel>
	{
		// Token: 0x06002045 RID: 8261
		public abstract CampaignTime GetMinGlobalCooldownTime();

		// Token: 0x06002046 RID: 8262
		public abstract CampaignTime GetMaxGlobalCooldownTime();

		// Token: 0x06002047 RID: 8263
		public abstract float GetIncidentTriggerGlobalProbability();

		// Token: 0x06002048 RID: 8264
		public abstract float GetIncidentTriggerProbabilityDuringSiege();

		// Token: 0x06002049 RID: 8265
		public abstract float GetIncidentTriggerProbabilityDuringWait();
	}
}
