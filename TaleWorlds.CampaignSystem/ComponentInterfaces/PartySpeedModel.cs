using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019D RID: 413
	public abstract class PartySpeedModel : MBGameModel<PartySpeedModel>
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001CFC RID: 7420
		public abstract float BaseSpeed { get; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001CFD RID: 7421
		public abstract float MinimumSpeed { get; }

		// Token: 0x06001CFE RID: 7422
		public abstract ExplainedNumber CalculateBaseSpeed(MobileParty party, bool includeDescriptions = false, int additionalTroopOnFootCount = 0, int additionalTroopOnHorseCount = 0);

		// Token: 0x06001CFF RID: 7423
		public abstract ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed);

		// Token: 0x06001D00 RID: 7424
		public abstract int GetSkeletalCrewCount(MobileParty party);
	}
}
