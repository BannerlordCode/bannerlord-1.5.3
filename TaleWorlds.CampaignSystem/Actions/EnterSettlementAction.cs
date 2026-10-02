using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D9 RID: 1241
	public static class EnterSettlementAction
	{
		// Token: 0x06004D8E RID: 19854 RVA: 0x00187E50 File Offset: 0x00186050
		private static void ApplyInternal(Hero hero, MobileParty mobileParty, Settlement settlement, EnterSettlementAction.EnterSettlementDetail detail, object subject = null, bool isPlayerInvolved = false)
		{
			if (mobileParty != null && mobileParty.IsDisbanding && mobileParty.TargetSettlement == settlement)
			{
				DestroyPartyAction.ApplyForDisbanding(mobileParty, settlement);
			}
			else
			{
				CampaignEventDispatcher.Instance.OnBeforeSettlementEntered(mobileParty, settlement, hero);
				CampaignEventDispatcher.Instance.OnSettlementEntered(mobileParty, settlement, hero);
				CampaignEventDispatcher.Instance.OnAfterSettlementEntered(mobileParty, settlement, hero);
				if (detail == EnterSettlementAction.EnterSettlementDetail.Prisoner)
				{
					if (hero != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, hero, false);
					}
					if (mobileParty != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, mobileParty.PrisonRoster.ToFlattenedRoster(), null, false);
					}
				}
				Hero hero2 = ((mobileParty != null) ? mobileParty.LeaderHero : hero);
				if (hero2 != null)
				{
					float currentTime = Campaign.CurrentTime;
					if (hero2.Clan == settlement.OwnerClan)
					{
						Clan clan = hero2.Clan;
						if (((clan != null) ? clan.Leader : null) == hero2)
						{
							settlement.LastVisitTimeOfOwner = currentTime;
						}
					}
				}
				if (mobileParty == MobileParty.MainParty && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
				{
					foreach (MobileParty mobileParty2 in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
					{
						EnterSettlementAction.ApplyForParty(mobileParty2, settlement);
					}
				}
				if (hero != null && mobileParty == null && hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null && hero.Clan == Clan.PlayerClan && hero.GovernorOf == null)
				{
					CampaignEventDispatcher.Instance.OnHeroGetsBusy(hero, HeroGetsBusyReasons.BecomeEmissary);
				}
			}
			if (mobileParty != null && mobileParty.IsFleeing())
			{
				mobileParty.Ai.DisableForHours(5);
			}
			if (hero == Hero.MainHero || mobileParty == MobileParty.MainParty)
			{
				Debug.Print(string.Format("Player has entered {0}: {1}", settlement.StringId, settlement), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x00188010 File Offset: 0x00186210
		public static void ApplyForParty(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != null && mobileParty.Army != null && mobileParty.Army.LeaderParty != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.Army.LeaderParty.CurrentSettlement == settlement && mobileParty.AttachedTo == null)
			{
				mobileParty.Army.AddPartyToMergedParties(mobileParty);
			}
			bool isCurrentlyAtSea = mobileParty.IsCurrentlyAtSea;
			if (!settlement.IsVillage && mobileParty.HasLandNavigationCapability)
			{
				mobileParty.IsCurrentlyAtSea = false;
			}
			mobileParty.CurrentSettlement = settlement;
			if (isCurrentlyAtSea && !mobileParty.IsCurrentlyAtSea && mobileParty.Ships.Any<Ship>() && !mobileParty.Anchor.IsAtSettlement(settlement))
			{
				mobileParty.Anchor.Settlement = settlement;
			}
			settlement.SettlementComponent.OnPartyEntered(mobileParty);
			EnterSettlementAction.ApplyInternal(mobileParty.LeaderHero, mobileParty, settlement, EnterSettlementAction.EnterSettlementDetail.WarParty, null, false);
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x001880DB File Offset: 0x001862DB
		public static void ApplyForPartyEntersAlley(MobileParty party, Settlement settlement, Alley alley, bool isPlayerInvolved = false)
		{
			EnterSettlementAction.ApplyInternal(null, party, settlement, EnterSettlementAction.EnterSettlementDetail.PartyEntersAlley, alley, isPlayerInvolved);
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x001880E8 File Offset: 0x001862E8
		public static void ApplyForCharacterOnly(Hero hero, Settlement settlement)
		{
			hero.StayingInSettlement = settlement;
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Character, null, false);
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x001880FC File Offset: 0x001862FC
		public static void ApplyForPrisoner(Hero hero, Settlement settlement)
		{
			hero.ChangeState(Hero.CharacterStates.Prisoner);
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Prisoner, null, false);
		}

		// Token: 0x020008E5 RID: 2277
		private enum EnterSettlementDetail
		{
			// Token: 0x04002688 RID: 9864
			WarParty,
			// Token: 0x04002689 RID: 9865
			PartyEntersAlley,
			// Token: 0x0400268A RID: 9866
			Character,
			// Token: 0x0400268B RID: 9867
			Prisoner
		}
	}
}
