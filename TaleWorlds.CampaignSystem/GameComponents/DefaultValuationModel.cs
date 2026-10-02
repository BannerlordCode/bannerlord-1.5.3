using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200016A RID: 362
	public class DefaultValuationModel : ValuationModel
	{
		// Token: 0x06001B95 RID: 7061 RVA: 0x0008EE0F File Offset: 0x0008D00F
		public override float GetMilitaryValueOfParty(MobileParty party)
		{
			return party.Party.CalculateCurrentStrength() * 15f;
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0008EE22 File Offset: 0x0008D022
		public override float GetValueOfTroop(CharacterObject troop)
		{
			return troop.GetPower() * 15f;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0008EE30 File Offset: 0x0008D030
		public override float GetValueOfHero(Hero hero)
		{
			if (hero.Clan != null)
			{
				return ((float)hero.Clan.Gold * 0.15f + (float)((1 + hero.Clan.Tier * hero.Clan.Tier) * 500)) * ((hero.Clan.Leader == hero) ? 4f : 1f);
			}
			return 500f;
		}
	}
}
