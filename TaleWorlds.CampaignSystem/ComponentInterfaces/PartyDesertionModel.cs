using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C9 RID: 457
	public abstract class PartyDesertionModel : MBGameModel<PartyDesertionModel>
	{
		// Token: 0x06001E89 RID: 7817
		public abstract TroopRoster GetTroopsToDesert(MobileParty mobileParty);

		// Token: 0x06001E8A RID: 7818
		public abstract float GetDesertionChanceForTroop(MobileParty mobileParty, in TroopRosterElement troopRosterElement);

		// Token: 0x06001E8B RID: 7819
		public abstract int GetMoraleThresholdForTroopDesertion();
	}
}
