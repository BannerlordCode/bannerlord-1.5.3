using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D1 RID: 1233
	public static class DestroyPartyAction
	{
		// Token: 0x06004D6A RID: 19818 RVA: 0x00187810 File Offset: 0x00185A10
		private static void ApplyInternal(PartyBase destroyerParty, MobileParty destroyedParty)
		{
			if (destroyedParty != MobileParty.MainParty)
			{
				if (!destroyedParty.IsActive)
				{
					Debug.Print("Trying to destroy an inactive party with id: " + destroyedParty.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert("destroyedParty.IsActive", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\DestroyPartyAction.cs", "ApplyInternal", 20);
				}
				if (destroyedParty.IsCaravan && destroyedParty.Party.Owner != null && destroyedParty.Party.Owner.GetPerkValue(DefaultPerks.Trade.InsurancePlans))
				{
					GiveGoldAction.ApplyBetweenCharacters(null, destroyedParty.Party.Owner, (int)DefaultPerks.Trade.InsurancePlans.PrimaryBonus, false);
				}
				CampaignEventDispatcher.Instance.OnMobilePartyDestroyed(destroyedParty, destroyerParty);
				CampaignEventDispatcher.Instance.OnMapInteractableDestroyed(destroyedParty.Party);
				if (destroyedParty.MapEvent != null && !destroyedParty.MapEvent.IsFinalized && !destroyedParty.IsCurrentlyUsedByAQuest)
				{
					destroyedParty.MapEventSide = null;
				}
				destroyedParty.RemoveParty();
			}
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x001878F6 File Offset: 0x00185AF6
		public static void Apply(PartyBase destroyerParty, MobileParty destroyedParty)
		{
			DestroyPartyAction.ApplyInternal(destroyerParty, destroyedParty);
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x001878FF File Offset: 0x00185AFF
		public static void ApplyForDisbanding(MobileParty disbandedParty, Settlement relatedSettlement)
		{
			if (disbandedParty.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForParty(disbandedParty);
			}
			CampaignEventDispatcher.Instance.OnPartyDisbanded(disbandedParty, relatedSettlement);
			DestroyPartyAction.ApplyInternal(null, disbandedParty);
		}
	}
}
