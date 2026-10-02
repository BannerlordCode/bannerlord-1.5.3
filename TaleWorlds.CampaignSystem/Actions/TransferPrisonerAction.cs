using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F5 RID: 1269
	public static class TransferPrisonerAction
	{
		// Token: 0x06004E07 RID: 19975 RVA: 0x0018B108 File Offset: 0x00189308
		private static void ApplyInternal(CharacterObject prisonerTroop, PartyBase prisonerOwnerParty, PartyBase newParty)
		{
			if (prisonerTroop.HeroObject == Hero.MainHero)
			{
				PlayerCaptivity.CaptorParty = newParty;
			}
			else
			{
				prisonerOwnerParty.PrisonRoster.AddToCounts(prisonerTroop, -1, false, 0, 0, true, -1);
				newParty.AddPrisoner(prisonerTroop, 1);
			}
			if (prisonerTroop.HeroObject != null && newParty.IsSettlement)
			{
				CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(newParty.Settlement, null, prisonerTroop.HeroObject, false);
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0018B16E File Offset: 0x0018936E
		public static void Apply(CharacterObject prisonerTroop, PartyBase prisonerOwnerParty, PartyBase newParty)
		{
			TransferPrisonerAction.ApplyInternal(prisonerTroop, prisonerOwnerParty, newParty);
		}
	}
}
