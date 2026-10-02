using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004EE RID: 1262
	public static class SellPrisonersAction
	{
		// Token: 0x06004DE3 RID: 19939 RVA: 0x0018A104 File Offset: 0x00188304
		private static void ApplyInternal(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners, bool applyConsequences)
		{
			Settlement settlement = sellerParty.Settlement ?? ((buyerParty != null) ? buyerParty.Settlement : null);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			int num = 0;
			bool flag = false;
			foreach (TroopRosterElement troopRosterElement in prisoners.GetTroopRoster())
			{
				CharacterObject character = troopRosterElement.Character;
				if (!character.IsHero)
				{
					if (applyConsequences)
					{
						sellerParty.PrisonRoster.AddToCounts(character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, 0, true, -1);
					}
				}
				else if (character.HeroObject != Hero.MainHero)
				{
					if (buyerParty != null)
					{
						if (!buyerParty.MapFaction.IsAtWarWith(character.HeroObject.MapFaction))
						{
							if (character.HeroObject.Clan == Clan.PlayerClan)
							{
								EndCaptivityAction.ApplyByReleasedByCompensation(character.HeroObject);
							}
							else
							{
								EndCaptivityAction.ApplyByRansom(character.HeroObject, null);
							}
						}
						else
						{
							if (sellerParty.MapFaction == buyerParty.MapFaction && sellerParty != PartyBase.MainParty)
							{
								flag = true;
								troopRoster.Add(troopRosterElement);
							}
							TransferPrisonerAction.Apply(character, sellerParty, buyerParty);
						}
					}
					else
					{
						EndCaptivityAction.ApplyByRansom(character.HeroObject, null);
					}
					if (settlement != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, character.HeroObject, false);
					}
				}
				if (applyConsequences && !flag && character != Hero.MainHero.CharacterObject)
				{
					int num2 = Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(character, (sellerParty != null) ? sellerParty.LeaderHero : null);
					num += troopRosterElement.Number * num2;
				}
			}
			if (applyConsequences && !flag && num > 0)
			{
				if (sellerParty.IsMobile)
				{
					Hero hero = null;
					if (sellerParty.LeaderHero != null && sellerParty.LeaderHero.HeroState == Hero.CharacterStates.Active)
					{
						hero = sellerParty.LeaderHero;
					}
					else if (sellerParty.Owner != null && sellerParty.Owner.HeroState == Hero.CharacterStates.Active)
					{
						hero = sellerParty.Owner;
					}
					else
					{
						Clan actualClan = sellerParty.MobileParty.ActualClan;
						if (((actualClan != null) ? actualClan.Leader : null) != null)
						{
							hero = sellerParty.MobileParty.ActualClan.Leader;
						}
					}
					GiveGoldAction.ApplyBetweenCharacters(null, hero, num, false);
				}
				else
				{
					Clan clan;
					if (buyerParty == null)
					{
						clan = null;
					}
					else
					{
						Settlement settlement2 = buyerParty.Settlement;
						clan = ((settlement2 != null) ? settlement2.OwnerClan : null);
					}
					bool flag2 = clan != Clan.PlayerClan;
					GiveGoldAction.ApplyForPartyToSettlement(null, sellerParty.Settlement, num, flag2);
				}
			}
			if (sellerParty.IsMobile)
			{
				SkillLevelingManager.OnPrisonerSell(sellerParty.MobileParty, in prisoners);
			}
			CampaignEventDispatcher.Instance.OnPrisonerSold(sellerParty, buyerParty, prisoners);
			if (settlement != null && troopRoster.Count > 0)
			{
				CampaignEventDispatcher.Instance.OnPrisonerDonatedToSettlement(sellerParty.MobileParty, troopRoster.ToFlattenedRoster(), settlement);
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x0018A3C4 File Offset: 0x001885C4
		public static void ApplyForAllPrisoners(PartyBase sellerParty, PartyBase buyerParty)
		{
			SellPrisonersAction.ApplyInternal(sellerParty, buyerParty, sellerParty.PrisonRoster.CloneRosterData(), true);
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0018A3D9 File Offset: 0x001885D9
		public static void ApplyForSelectedPrisoners(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			SellPrisonersAction.ApplyInternal(sellerParty, buyerParty, prisoners, true);
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0018A3E4 File Offset: 0x001885E4
		public static void ApplyByPartyScreen(TroopRoster prisoners)
		{
			SellPrisonersAction.ApplyInternal(PartyBase.MainParty, Hero.MainHero.CurrentSettlement.Party, prisoners, false);
		}
	}
}
