using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A1 RID: 417
	public abstract class PartyHealingModel : MBGameModel<PartyHealingModel>
	{
		// Token: 0x06001D12 RID: 7442
		public abstract float GetSurgeryChance(PartyBase party);

		// Token: 0x06001D13 RID: 7443
		public abstract float GetSurvivalChance(PartyBase party, CharacterObject agentCharacter, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null);

		// Token: 0x06001D14 RID: 7444
		public abstract int GetSkillXpFromHealingTroop(PartyBase party);

		// Token: 0x06001D15 RID: 7445
		public abstract ExplainedNumber GetDailyHealingForRegulars(PartyBase partyBase, bool isPrisoner, bool includeDescriptions = false);

		// Token: 0x06001D16 RID: 7446
		public abstract ExplainedNumber GetDailyHealingHpForHeroes(PartyBase partyBase, bool isPrisoners, bool includeDescriptions = false);

		// Token: 0x06001D17 RID: 7447
		public abstract float GetSiegeBombardmentHitSurgeryChance(PartyBase party);

		// Token: 0x06001D18 RID: 7448
		public abstract ExplainedNumber GetBattleEndHealingAmount(PartyBase partyBase, Hero hero);
	}
}
