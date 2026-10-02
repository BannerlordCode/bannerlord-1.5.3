using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D9 RID: 473
	public abstract class HeroAgentLocationModel : MBGameModel<HeroAgentLocationModel>
	{
		// Token: 0x06001F06 RID: 7942
		public abstract bool WillBeListedInOverlay(LocationCharacter locationCharacter);

		// Token: 0x06001F07 RID: 7943
		public abstract Location GetLocationForHero(Hero hero, Settlement settlement, out HeroAgentLocationModel.HeroLocationDetail heroSpawnDetail);

		// Token: 0x02000631 RID: 1585
		public enum HeroLocationDetail
		{
			// Token: 0x04001A40 RID: 6720
			None,
			// Token: 0x04001A41 RID: 6721
			SettlementKingQueen,
			// Token: 0x04001A42 RID: 6722
			NobleBelongingToNoParty,
			// Token: 0x04001A43 RID: 6723
			Prisoner,
			// Token: 0x04001A44 RID: 6724
			PlayerClanMember,
			// Token: 0x04001A45 RID: 6725
			MainPartyCompanion,
			// Token: 0x04001A46 RID: 6726
			Notable,
			// Token: 0x04001A47 RID: 6727
			Wanderer,
			// Token: 0x04001A48 RID: 6728
			PartyLeader,
			// Token: 0x04001A49 RID: 6729
			PartylessHeroInsideVillage
		}
	}
}
