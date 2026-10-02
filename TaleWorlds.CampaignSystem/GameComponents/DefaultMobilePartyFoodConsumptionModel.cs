using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011F RID: 287
	public class DefaultMobilePartyFoodConsumptionModel : MobilePartyFoodConsumptionModel
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x00076C71 File Offset: 0x00074E71
		public override int NumberOfMenOnMapToEatOneFood
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00076C78 File Offset: 0x00074E78
		public override ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false)
		{
			int num = party.Party.NumberOfAllMembers + party.Party.NumberOfPrisoners / 2;
			num = ((num < 1) ? 1 : num);
			return new ExplainedNumber(-(float)num / (float)this.NumberOfMenOnMapToEatOneFood, includeDescription, null);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00076CBA File Offset: 0x00074EBA
		public override ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption)
		{
			this.CalculatePerkEffects(party, ref baseConsumption);
			baseConsumption.LimitMax(-0.01f, null);
			return baseConsumption;
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00076CD4 File Offset: 0x00074ED4
		private void CalculatePerkEffects(MobileParty party, ref ExplainedNumber result)
		{
			int num = 0;
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				if (party.MemberRoster.GetCharacterAtIndex(i).Culture.IsBandit)
				{
					num += party.MemberRoster.GetElementNumber(i);
				}
			}
			for (int j = 0; j < party.PrisonRoster.Count; j++)
			{
				if (party.PrisonRoster.GetCharacterAtIndex(j).Culture.IsBandit)
				{
					num += party.PrisonRoster.GetElementNumber(j);
				}
			}
			if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Roguery.Promises) && num > 0)
			{
				float num2 = (float)num / (float)party.MemberRoster.TotalManCount * DefaultPerks.Roguery.Promises.PrimaryBonus;
				result.AddFactor(num2, DefaultPerks.Roguery.Promises.Name);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.Spartan, party, false, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.WarriorsDiet, party, true, ref result);
			if (party.EffectiveQuartermaster != null)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, party.CurrentBattleEnvironment, party.EffectiveQuartermaster.CharacterObject, DefaultSkills.Steward, true, ref result, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
			}
			TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
			if (faceTerrainType == TerrainType.Forest || faceTerrainType == TerrainType.Steppe)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Foragers, party, true, ref result);
			}
			if (party.IsGarrison && party.CurrentSettlement != null && party.CurrentSettlement.Town.IsUnderSiege)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.StrongLegs, party.CurrentSettlement.Town, false, ref result);
			}
			if (party.Army != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.StiffUpperLip, party, true, ref result);
			}
			SiegeEvent siegeEvent = party.SiegeEvent;
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp : null) != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.SoundReserves, party, false, ref result);
				if (party.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(party.Party, MapEvent.BattleTypes.Siege))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.MasterOfPlanning, party, true, ref result);
				}
			}
			if (party.LeaderHero != null)
			{
				TraitEffectHelper.ApplyTraitEffect(party.LeaderHero, DefaultPersonalityTraitEffects.GenerosityFoodCostEffect, ref result);
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00076EE4 File Offset: 0x000750E4
		public override bool DoesPartyConsumeFood(MobileParty mobileParty)
		{
			return mobileParty.IsActive && (mobileParty.LeaderHero == null || mobileParty.LeaderHero.IsLord || mobileParty.LeaderHero.Clan == Clan.PlayerClan || mobileParty.LeaderHero.IsMinorFactionHero) && !mobileParty.IsGarrison && !mobileParty.IsCaravan && !mobileParty.IsBandit && !mobileParty.IsMilitia && !mobileParty.IsPatrolParty && !mobileParty.IsVillager;
		}

		// Token: 0x04000810 RID: 2064
		private const float MinimumDailyFoodConsumption = 0.01f;

		// Token: 0x04000811 RID: 2065
		private static readonly TextObject _partyConsumption = new TextObject("{=UrFzdy4z}Daily Consumption", null);
	}
}
