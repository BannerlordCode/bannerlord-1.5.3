using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E7 RID: 1255
	public static class MarriageAction
	{
		// Token: 0x06004DCC RID: 19916 RVA: 0x001895F8 File Offset: 0x001877F8
		private static void ApplyInternal(Hero firstHero, Hero secondHero, bool showNotification)
		{
			if (!Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(firstHero, secondHero))
			{
				Debug.Print("MarriageAction.Apply() called for not suitable couple: " + firstHero.StringId + " and " + secondHero.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			firstHero.Spouse = secondHero;
			secondHero.Spouse = firstHero;
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(firstHero, secondHero, Campaign.Current.Models.MarriageModel.GetEffectiveRelationIncrease(firstHero, secondHero), false);
			Clan clanAfterMarriage = Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(firstHero, secondHero);
			if (clanAfterMarriage != firstHero.Clan)
			{
				Hero hero = firstHero;
				firstHero = secondHero;
				secondHero = hero;
			}
			CampaignEventDispatcher.Instance.OnBeforeHeroesMarried(firstHero, secondHero, showNotification);
			if (firstHero.Clan != clanAfterMarriage)
			{
				MarriageAction.HandleClanChangeAfterMarriageForHero(firstHero, clanAfterMarriage);
			}
			if (secondHero.Clan != clanAfterMarriage)
			{
				MarriageAction.HandleClanChangeAfterMarriageForHero(secondHero, clanAfterMarriage);
			}
			Romance.EndAllCourtships(firstHero);
			Romance.EndAllCourtships(secondHero);
			ChangeRomanticStateAction.Apply(firstHero, secondHero, Romance.RomanceLevelEnum.Marriage);
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x001896E0 File Offset: 0x001878E0
		private static void HandleClanChangeAfterMarriageForHero(Hero hero, Clan clanAfterMarriage)
		{
			Clan clan = hero.Clan;
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			if (hero.PartyBelongedTo != null)
			{
				if (clan.Kingdom != clanAfterMarriage.Kingdom)
				{
					if (hero.PartyBelongedTo.Army != null)
					{
						if (hero.PartyBelongedTo.Army.LeaderParty == hero.PartyBelongedTo)
						{
							DisbandArmyAction.ApplyByUnknownReason(hero.PartyBelongedTo.Army);
						}
						else
						{
							hero.PartyBelongedTo.Army = null;
						}
					}
					IFaction kingdom = clanAfterMarriage.Kingdom;
					FactionHelper.FinishAllRelatedHostileActionsOfNobleToFaction(hero, kingdom ?? clanAfterMarriage);
				}
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				bool flag = hero.PartyBelongedTo.LeaderHero == hero;
				partyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				MakeHeroFugitiveAction.Apply(hero, false);
				if (flag && partyBelongedTo.IsLordParty)
				{
					DisbandPartyAction.StartDisband(partyBelongedTo);
				}
			}
			hero.Clan = clanAfterMarriage;
			foreach (Hero hero2 in clan.Heroes)
			{
				hero2.UpdateHomeSettlement();
			}
			foreach (Hero hero3 in clanAfterMarriage.Heroes)
			{
				hero3.UpdateHomeSettlement();
			}
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x00189844 File Offset: 0x00187A44
		public static void Apply(Hero firstHero, Hero secondHero, bool showNotification = true)
		{
			MarriageAction.ApplyInternal(firstHero, secondHero, showNotification);
		}
	}
}
