using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F7 RID: 503
	public abstract class PartyTroopUpgradeModel : MBGameModel<PartyTroopUpgradeModel>
	{
		// Token: 0x06001FCB RID: 8139
		public abstract bool CanPartyUpgradeTroopToTarget(PartyBase party, CharacterObject character, CharacterObject target);

		// Token: 0x06001FCC RID: 8140
		public abstract bool IsTroopUpgradeable(PartyBase party, CharacterObject character);

		// Token: 0x06001FCD RID: 8141
		public abstract bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget);

		// Token: 0x06001FCE RID: 8142
		public abstract bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk);

		// Token: 0x06001FCF RID: 8143
		public abstract ExplainedNumber GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001FD0 RID: 8144
		public abstract int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001FD1 RID: 8145
		public abstract int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops);

		// Token: 0x06001FD2 RID: 8146
		public abstract float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex);
	}
}
