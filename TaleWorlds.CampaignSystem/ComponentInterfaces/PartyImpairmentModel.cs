using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B6 RID: 438
	public abstract class PartyImpairmentModel : MBGameModel<PartyImpairmentModel>
	{
		// Token: 0x06001DDC RID: 7644
		public abstract ExplainedNumber GetDisorganizedStateDuration(MobileParty party);

		// Token: 0x06001DDD RID: 7645
		public abstract float GetVulnerabilityStateDuration(PartyBase party);

		// Token: 0x06001DDE RID: 7646
		public abstract float GetSiegeExpectedVulnerabilityTime();

		// Token: 0x06001DDF RID: 7647
		public abstract bool CanGetDisorganized(PartyBase partyBase);
	}
}
