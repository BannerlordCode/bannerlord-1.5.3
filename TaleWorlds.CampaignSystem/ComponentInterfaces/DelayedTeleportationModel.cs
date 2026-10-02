using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FD RID: 509
	public abstract class DelayedTeleportationModel : MBGameModel<DelayedTeleportationModel>
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001FFA RID: 8186
		public abstract float MaximumDistanceForDelayAsDays { get; }

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06001FFB RID: 8187
		public abstract float DefaultTeleportationSpeed { get; }

		// Token: 0x06001FFC RID: 8188
		public abstract ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target);

		// Token: 0x06001FFD RID: 8189
		public abstract bool CanPerformImmediateTeleport(Hero hero, MobileParty targetMobileParty, Settlement targetSettlement);
	}
}
