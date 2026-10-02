using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004DA RID: 1242
	public static class GainKingdomInfluenceAction
	{
		// Token: 0x06004D93 RID: 19859 RVA: 0x00188110 File Offset: 0x00186310
		private static void ApplyInternal(Hero hero, MobileParty party, float gainedInfluence, GainKingdomInfluenceAction.InfluenceGainingReason detail)
		{
			Clan clan = null;
			if (hero != null)
			{
				if (hero.CompanionOf != null)
				{
					clan = hero.CompanionOf;
				}
				else if (hero.Clan != null)
				{
					clan = hero.Clan;
				}
			}
			else if (party.ActualClan != null)
			{
				clan = party.ActualClan;
			}
			else if (party.Owner != null)
			{
				clan = party.Owner.Clan;
			}
			if (clan == null || clan.Kingdom == null)
			{
				return;
			}
			MobileParty mobileParty = party ?? hero.PartyBelongedTo;
			if (detail != GainKingdomInfluenceAction.InfluenceGainingReason.BeingAtArmy && detail == GainKingdomInfluenceAction.InfluenceGainingReason.ClanSupport)
			{
				gainedInfluence = 0.5f;
			}
			if (detail != GainKingdomInfluenceAction.InfluenceGainingReason.Default && detail != GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood && detail != GainKingdomInfluenceAction.InfluenceGainingReason.JoinFaction && detail != GainKingdomInfluenceAction.InfluenceGainingReason.ClanSupport && ((Kingdom)clan.MapFaction).ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae))
			{
				gainedInfluence *= 1.2f;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(gainedInfluence, false, null);
			if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.Battle && gainedInfluence > 0f)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.PreBattleManeuvers, mobileParty, true, ref explainedNumber);
			}
			if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.CaptureSettlement && (hero != null || mobileParty.LeaderHero != null))
			{
				Hero hero2 = hero ?? mobileParty.LeaderHero;
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Tactics.Besieged, mobileParty.CurrentBattleEnvironment, hero2.CharacterObject, false, ref explainedNumber);
			}
			gainedInfluence = explainedNumber.ResultNumber;
			ChangeClanInfluenceAction.Apply(clan, gainedInfluence);
			int num = (int)gainedInfluence;
			if (MathF.Abs(num) > 0)
			{
				if ((detail == GainKingdomInfluenceAction.InfluenceGainingReason.DonatePrisoners && party == MobileParty.MainParty) || (detail == GainKingdomInfluenceAction.InfluenceGainingReason.Battle && hero == Hero.MainHero))
				{
					TextObject textObject = GameTexts.FindText("str_influence_gain_message", null);
					textObject.SetTextVariable("INFLUENCE", num);
					textObject.SetTextVariable("NEW_INFLUENCE", (int)clan.Influence);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				}
				if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.SiegeSafePassage && hero == Hero.MainHero)
				{
					TextObject textObject2 = GameTexts.FindText("str_leave_siege_lose_influence_message", null);
					textObject2.SetTextVariable("INFLUENCE", -num);
					InformationManager.DisplayMessage(new InformationMessage(textObject2.ToString()));
				}
			}
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x001882C9 File Offset: 0x001864C9
		public static void ApplyForBattle(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.Battle);
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x001882D4 File Offset: 0x001864D4
		public static void ApplyForGivingFood(Hero hero1, Hero hero2, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero1, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood);
			GainKingdomInfluenceAction.ApplyInternal(hero2, null, -value, GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood);
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x001882E9 File Offset: 0x001864E9
		public static void ApplyForDefault(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.Default);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x001882F4 File Offset: 0x001864F4
		public static void ApplyForJoiningFaction(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.JoinFaction);
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x001882FF File Offset: 0x001864FF
		public static void ApplyForDonatePrisoners(MobileParty donatingParty, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, donatingParty, value, GainKingdomInfluenceAction.InfluenceGainingReason.DonatePrisoners);
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0018830B File Offset: 0x0018650B
		public static void ApplyForRaidingEnemyVillage(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.Raiding);
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x00188316 File Offset: 0x00186516
		public static void ApplyForBesiegingEnemySettlement(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.Besieging);
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x00188321 File Offset: 0x00186521
		public static void ApplyForSiegeSafePassageBarter(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.SiegeSafePassage);
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0018832D File Offset: 0x0018652D
		public static void ApplyForCapturingEnemySettlement(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.CaptureSettlement);
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x00188338 File Offset: 0x00186538
		public static void ApplyForLeavingTroopToGarrison(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.LeaveGarrison);
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x00188343 File Offset: 0x00186543
		public static void ApplyForBoardGameWon(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.BoardGameWon);
		}

		// Token: 0x020008E6 RID: 2278
		private enum InfluenceGainingReason
		{
			// Token: 0x0400268D RID: 9869
			Default,
			// Token: 0x0400268E RID: 9870
			BeingAtArmy,
			// Token: 0x0400268F RID: 9871
			Battle,
			// Token: 0x04002690 RID: 9872
			Raiding,
			// Token: 0x04002691 RID: 9873
			Besieging,
			// Token: 0x04002692 RID: 9874
			CaptureSettlement,
			// Token: 0x04002693 RID: 9875
			JoinFaction,
			// Token: 0x04002694 RID: 9876
			GivingFood,
			// Token: 0x04002695 RID: 9877
			LeaveGarrison,
			// Token: 0x04002696 RID: 9878
			BoardGameWon,
			// Token: 0x04002697 RID: 9879
			ClanSupport,
			// Token: 0x04002698 RID: 9880
			DonatePrisoners,
			// Token: 0x04002699 RID: 9881
			SiegeSafePassage
		}
	}
}
