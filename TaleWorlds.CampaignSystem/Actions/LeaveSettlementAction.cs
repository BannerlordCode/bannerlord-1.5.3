using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E2 RID: 1250
	public static class LeaveSettlementAction
	{
		// Token: 0x06004DC1 RID: 19905 RVA: 0x001892BC File Offset: 0x001874BC
		public static void ApplyForParty(MobileParty mobileParty)
		{
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
			{
				foreach (MobileParty mobileParty2 in mobileParty.Army.LeaderParty.AttachedParties)
				{
					if (mobileParty2 == MobileParty.MainParty && PlayerEncounter.Current != null)
					{
						PlayerEncounter.Finish(true);
					}
					else if (mobileParty2.CurrentSettlement == currentSettlement)
					{
						LeaveSettlementAction.ApplyForParty(mobileParty2);
					}
				}
			}
			mobileParty.CurrentSettlement = null;
			if (mobileParty.IsCurrentlyAtSea)
			{
				mobileParty.Anchor.ResetPosition();
			}
			currentSettlement.SettlementComponent.OnPartyLeft(mobileParty);
			CampaignEventDispatcher.Instance.OnSettlementLeft(mobileParty, currentSettlement);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00189388 File Offset: 0x00187588
		public static void ApplyForCharacterOnly(Hero hero)
		{
			Settlement currentSettlement = hero.CurrentSettlement;
			hero.StayingInSettlement = null;
			LocationComplex locationComplex = currentSettlement.LocationComplex;
			Location location = ((locationComplex != null) ? locationComplex.GetLocationOfCharacter(hero) : null);
			if (location != null && location.GetLocationCharacter(hero) != null)
			{
				currentSettlement.LocationComplex.RemoveCharacterIfExists(hero);
				LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
				if (locationEncounter == null)
				{
					return;
				}
				locationEncounter.RemoveAccompanyingCharacter(hero);
			}
		}
	}
}
