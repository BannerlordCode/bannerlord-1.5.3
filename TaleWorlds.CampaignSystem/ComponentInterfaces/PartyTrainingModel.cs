using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019E RID: 414
	public abstract class PartyTrainingModel : MBGameModel<PartyTrainingModel>
	{
		// Token: 0x06001D02 RID: 7426
		public abstract int GenerateSharedXp(CharacterObject troop, int xp, MobileParty mobileParty);

		// Token: 0x06001D03 RID: 7427
		public abstract ExplainedNumber CalculateXpGainFromBattles(FlattenedTroopRosterElement troopRosterElement, PartyBase party);

		// Token: 0x06001D04 RID: 7428
		public abstract int GetXpReward(CharacterObject character);

		// Token: 0x06001D05 RID: 7429
		public abstract ExplainedNumber GetEffectiveDailyExperience(MobileParty party, TroopRosterElement troop);
	}
}
