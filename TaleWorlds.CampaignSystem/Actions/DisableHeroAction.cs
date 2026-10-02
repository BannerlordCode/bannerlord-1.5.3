using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D3 RID: 1235
	public static class DisableHeroAction
	{
		// Token: 0x06004D70 RID: 19824 RVA: 0x00187974 File Offset: 0x00185B74
		private static void ApplyInternal(Hero hero)
		{
			if (hero.IsAlive)
			{
				if (hero.PartyBelongedTo != null)
				{
					if (hero.PartyBelongedTo.LeaderHero == hero)
					{
						DestroyPartyAction.Apply(null, hero.PartyBelongedTo);
					}
					else
					{
						hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
				}
				if (hero.StayingInSettlement != null)
				{
					hero.ChangeState(Hero.CharacterStates.Disabled);
					hero.StayingInSettlement = null;
				}
				if (hero.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(hero);
				}
				if (hero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByEscape(hero, null, true);
				}
				hero.ChangeState(Hero.CharacterStates.Disabled);
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00187A0C File Offset: 0x00185C0C
		public static void Apply(Hero hero)
		{
			DisableHeroAction.ApplyInternal(hero);
		}
	}
}
