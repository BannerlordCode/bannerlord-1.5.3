using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FC RID: 252
	public class DefaultBattleRewardModel : BattleRewardModel
	{
		// Token: 0x060016F8 RID: 5880 RVA: 0x0006AB8C File Offset: 0x00068D8C
		public override int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero)
		{
			float playerBattleContributionRate = mapEvent.GetPlayerBattleContributionRate();
			float num = (mapEvent.StrengthOfSide[(int)PartyBase.MainParty.Side] - PlayerEncounter.Current.PlayerPartyInitialStrength) / (mapEvent.StrengthOfSide[(int)PartyBase.MainParty.OpponentSide] + 1f);
			float num2 = ((num < 1f) ? (1f + (1f - num)) : ((num < 3f) ? (0.5f * (3f - num)) : 0f));
			float renownValue = (mapEvent.AttackerSide.IsMainPartyAmongParties() ? mapEvent.AttackerSide : mapEvent.DefenderSide).RenownValue;
			ExplainedNumber explainedNumber = new ExplainedNumber(0.75f + MathF.Pow(playerBattleContributionRate * 1.3f * (num2 + renownValue), 0.67f), false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.Camaraderie, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0006AC70 File Offset: 0x00068E70
		public override ExplainedNumber CalculateRenownGain(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float renownMultiplierForWinnerSide, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(contributionShareOfWinnerParty * renownValueOfBattleForWinnerSide * renownMultiplierForWinnerSide, includeDescriptions, null);
			if (winnerParty.IsMobile)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.LongReach, winnerParty.MobileParty, false, ref explainedNumber);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Charm.PublicSpeaker, winnerParty.MobileParty, true, ref explainedNumber);
				if (winnerParty.LeaderHero != null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Leadership.FamousCommander, winnerParty.MobileParty.CurrentBattleEnvironment, winnerParty.LeaderHero.CharacterObject, true, ref explainedNumber);
					TraitEffectHelper.ApplyTraitEffect(winnerParty.LeaderHero, DefaultPersonalityTraitEffects.ValorBattleRenownEffect, ref explainedNumber);
				}
				FeatHelper.ApplyCultureFeat(winnerParty, DefaultCulturalFeats.VlandianRenownMercenaryFeat, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x0006AD0C File Offset: 0x00068F0C
		public override ExplainedNumber CalculateInfluenceGain(PartyBase winnerParty, float influenceValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float influenceMultiplierForWinnerSide, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (winnerParty.MapFaction.IsKingdomFaction)
			{
				explainedNumber = new ExplainedNumber(influenceValueOfBattleForWinnerSide * contributionShareOfWinnerParty * influenceMultiplierForWinnerSide, includeDescriptions, null);
				if (winnerParty.LeaderHero != null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.Warlord, winnerParty.MobileParty.CurrentBattleEnvironment, winnerParty.LeaderHero.CharacterObject, true, ref explainedNumber);
				}
			}
			return explainedNumber;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0006AD74 File Offset: 0x00068F74
		public override ExplainedNumber CalculateMoraleGainVictory(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0.5f + renownValueOfBattleForWinnerSide * contributionShareOfWinnerParty * 0.5f, includeDescriptions, null);
			if (winnerParty.IsMobile)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.LongReach, winnerParty.MobileParty, false, ref explainedNumber);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.CitizenMilitia, winnerParty.MobileParty, false, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0006ADCB File Offset: 0x00068FCB
		public override int CalculateGoldLossAfterDefeat(Hero partyLeaderHero)
		{
			return (int)Math.Min((float)partyLeaderHero.Gold * 0.05f, 10000f);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x0006ADE8 File Offset: 0x00068FE8
		public override EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue)
		{
			Hero hero = null;
			bool flag = MobileParty.MainParty.HasPerk(DefaultPerks.Engineering.Metallurgy, out hero, false);
			EquipmentElement randomItem = DefaultBattleRewardModel.GetRandomItem(character.BattleEquipments.GetRandomElementInefficiently<Equipment>(), targetValue);
			if (flag && randomItem.ItemModifier != null && randomItem.ItemModifier.PriceMultiplier < 1f && MBRandom.RandomFloat < DefaultPerks.Engineering.Metallurgy.PrimaryBonus)
			{
				randomItem = new EquipmentElement(randomItem.Item, null, null, false);
			}
			return randomItem;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x0006AE5C File Offset: 0x0006905C
		private static EquipmentElement GetRandomItem(Equipment equipment, float targetValue = 0f)
		{
			int num = 0;
			for (int i = 0; i < 12; i++)
			{
				if (equipment[i].Item != null && !equipment[i].Item.NotMerchandise)
				{
					DefaultBattleRewardModel._indices[num] = i;
					num++;
				}
			}
			for (int j = 0; j < num - 1; j++)
			{
				int num2 = j;
				int num3 = equipment[DefaultBattleRewardModel._indices[j]].Item.Value;
				for (int k = j + 1; k < num; k++)
				{
					if (equipment[DefaultBattleRewardModel._indices[k]].Item.Value > num3)
					{
						num2 = k;
						num3 = equipment[DefaultBattleRewardModel._indices[k]].Item.Value;
					}
				}
				int num4 = DefaultBattleRewardModel._indices[j];
				DefaultBattleRewardModel._indices[j] = DefaultBattleRewardModel._indices[num2];
				DefaultBattleRewardModel._indices[num2] = num4;
			}
			if (num > 0)
			{
				for (int l = 0; l < num; l++)
				{
					int num5 = DefaultBattleRewardModel._indices[l];
					EquipmentElement equipmentElement = equipment[num5];
					if (equipmentElement.Item != null && !equipment[num5].Item.NotMerchandise)
					{
						float num6 = (float)equipmentElement.Item.Value + 0.1f;
						float num7 = 0.325f * (targetValue / (MathF.Max(targetValue, num6) * (float)(num - l)));
						if (MBRandom.RandomFloat < num7)
						{
							ItemComponent itemComponent = equipmentElement.Item.ItemComponent;
							ItemModifier itemModifier;
							if (itemComponent == null)
							{
								itemModifier = null;
							}
							else
							{
								ItemModifierGroup itemModifierGroup = itemComponent.ItemModifierGroup;
								itemModifier = ((itemModifierGroup != null) ? itemModifierGroup.GetRandomItemModifierLootScoreBased() : null);
							}
							ItemModifier itemModifier2 = itemModifier;
							if (itemModifier2 != null)
							{
								equipmentElement = new EquipmentElement(equipmentElement.Item, itemModifier2, null, false);
							}
							return equipmentElement;
						}
					}
				}
			}
			return default(EquipmentElement);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0006B024 File Offset: 0x00069224
		public override float GetExpectedLootedItemValueFromCasualty(Hero winnerPartyLeaderHero, CharacterObject casualtyCharacter)
		{
			float num = 7.25f * (float)(casualtyCharacter.Level * casualtyCharacter.Level);
			if (winnerPartyLeaderHero != Hero.MainHero)
			{
				return num;
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.ForceHideoutSendTroops)
			{
				return 0f;
			}
			return num * MBRandom.RandomFloatRanged(0.85f, 1.15f);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0006B07A File Offset: 0x0006927A
		public override float GetAITradePenalty()
		{
			return 0.018181818f;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0006B081 File Offset: 0x00069281
		public override float GetMainPartyMemberScatterChance()
		{
			return 0.1f;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x0006B088 File Offset: 0x00069288
		public override int CalculatePlunderedGoldAmountFromDefeatedParty(PartyBase defeatedParty)
		{
			int num = 0;
			if (defeatedParty.LeaderHero != null)
			{
				num = Campaign.Current.Models.BattleRewardModel.CalculateGoldLossAfterDefeat(defeatedParty.LeaderHero);
			}
			else if (defeatedParty.IsMobile && defeatedParty.MobileParty.IsPartyTradeActive)
			{
				MobileParty mobileParty = defeatedParty.MobileParty;
				num = (int)((float)mobileParty.PartyTradeGold * (mobileParty.IsBandit ? 0.5f : 0.1f));
			}
			return num;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0006B0F8 File Offset: 0x000692F8
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootGoldChances(MBReadOnlyList<MapEventParty> winnerParties)
		{
			MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
			float num = 0f;
			foreach (MapEventParty mapEventParty in winnerParties)
			{
				if (mapEventParty.ContributionToBattle > 0 && (!mapEventParty.Party.IsMobile || !mapEventParty.Party.MobileParty.IsPatrolParty))
				{
					mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, (float)mapEventParty.ContributionToBattle));
					num += (float)mapEventParty.ContributionToBattle;
				}
			}
			for (int i = 0; i < mblist.Count; i++)
			{
				mblist[i] = new KeyValuePair<MapEventParty, float>(mblist[i].Key, mblist[i].Value / num);
			}
			return mblist;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0006B1D8 File Offset: 0x000693D8
		public override void GetCaptureMemberChancesForWinnerParties(MapEvent endedMapEvent, MBReadOnlyList<MapEventParty> winnerParties, out MBList<KeyValuePair<MapEventParty, float>> woundedMemberChances, out MBList<KeyValuePair<MapEventParty, float>> healthyMemberChances)
		{
			woundedMemberChances = new MBList<KeyValuePair<MapEventParty, float>>();
			healthyMemberChances = new MBList<KeyValuePair<MapEventParty, float>>();
			MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
			MBList<KeyValuePair<MapEventParty, float>> mblist2 = new MBList<KeyValuePair<MapEventParty, float>>();
			float num = 0f;
			float num2 = 0.25f;
			if (endedMapEvent.GetMapEventSide(endedMapEvent.DefeatedSide).IsSurrendered)
			{
				num2 = 1f;
			}
			foreach (MapEventParty mapEventParty in winnerParties)
			{
				MobileParty mobileParty = mapEventParty.Party.MobileParty;
				if (mapEventParty.ContributionToBattle > 0 && mapEventParty.Party.MemberRoster.Count > 0 && (mobileParty == null || (!mobileParty.IsVillager && !mobileParty.IsCaravan && !mobileParty.IsPatrolParty && ((!mobileParty.IsGarrison && !mobileParty.IsMilitia) || !mobileParty.CurrentSettlement.IsVillage))))
				{
					mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, (float)mapEventParty.ContributionToBattle));
					num += (float)mapEventParty.ContributionToBattle;
					ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
					if (((mobileParty != null) ? mobileParty.LeaderHero : null) != null)
					{
						TraitEffectHelper.ApplyTraitEffect(mobileParty.LeaderHero, DefaultPersonalityTraitEffects.MercyPrisonerCaptureMercifulEffect, ref explainedNumber);
						TraitEffectHelper.ApplyTraitEffect((mobileParty != null) ? mobileParty.LeaderHero : null, DefaultPersonalityTraitEffects.MercyPrisonerCaptureCruelEffect, ref explainedNumber);
					}
					mblist2.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, explainedNumber.ResultNumber));
				}
			}
			for (int i = 0; i < mblist.Count; i++)
			{
				MapEventParty key = mblist[i].Key;
				float num3 = mblist[i].Value / num;
				float value = mblist2[i].Value;
				woundedMemberChances.Add(new KeyValuePair<MapEventParty, float>(key, num3 * 1f * value));
				healthyMemberChances.Add(new KeyValuePair<MapEventParty, float>(key, num3 * num2 * value));
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x0006B3EC File Offset: 0x000695EC
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootPrisonerChances(MBReadOnlyList<MapEventParty> winnerParties, TroopRosterElement prisonerElement)
		{
			MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
			CharacterObject character = prisonerElement.Character;
			if (character.HeroObject == null || !character.HeroObject.IsReleased)
			{
				float num = 0f;
				Occupation occupation = character.Occupation;
				foreach (MapEventParty mapEventParty in winnerParties)
				{
					MobileParty mobileParty = mapEventParty.Party.MobileParty;
					if (mapEventParty.ContributionToBattle > 0 && mapEventParty.Party.MemberRoster.Count > 0 && ((mobileParty == null && occupation != Occupation.Bandit) || (mobileParty != null && !mobileParty.IsVillager && !mobileParty.IsCaravan && !mobileParty.IsMilitia && !mobileParty.IsPatrolParty && (!mobileParty.IsBandit || occupation == Occupation.Bandit) && (!mobileParty.IsGarrison || occupation != Occupation.Bandit))))
					{
						mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, (float)mapEventParty.ContributionToBattle));
						num += (float)mapEventParty.ContributionToBattle;
					}
				}
				for (int i = 0; i < mblist.Count; i++)
				{
					mblist[i] = new KeyValuePair<MapEventParty, float>(mblist[i].Key, mblist[i].Value / num * 1f);
				}
			}
			return mblist;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0006B558 File Offset: 0x00069758
		public override MBList<KeyValuePair<MapEventParty, float>> GetLootItemChancesForWinnerParties(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.ForceHideoutSendTroops)
			{
				if (winnerParties.Any<MapEventParty>((MapEventParty x) => x.Party == PartyBase.MainParty))
				{
					return mblist;
				}
			}
			if (!defeatedParty.IsSettlement)
			{
				MBList<KeyValuePair<MapEventParty, float>> mblist2 = new MBList<KeyValuePair<MapEventParty, float>>();
				float num = 0f;
				foreach (MapEventParty mapEventParty in winnerParties)
				{
					MobileParty mobileParty = mapEventParty.Party.MobileParty;
					PartyBase party = mapEventParty.Party;
					BattleEnvironment battleEnvironment = ((mobileParty != null) ? mobileParty.CurrentBattleEnvironment : BattleEnvironment.Land);
					if (mapEventParty.ContributionToBattle > 0 && mapEventParty.Party.MemberRoster.Count > 0 && (mobileParty == null || (!mobileParty.IsGarrison && !mobileParty.IsMilitia)))
					{
						mblist2.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, (float)mapEventParty.ContributionToBattle));
						num += (float)mapEventParty.ContributionToBattle;
						ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
						SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.RogueryLootBonus, party.MobileParty, ref explainedNumber);
						if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Roguery.RogueExtraordinaire))
						{
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Roguery.RogueExtraordinaire, battleEnvironment, party.LeaderHero.CharacterObject, DefaultSkills.Roguery, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
						}
						float num2 = explainedNumber.ResultNumber;
						Hero hero = null;
						if (party.MobileParty.HasPerk(DefaultPerks.Roguery.KnowHow, out hero, false) && (defeatedParty.MobileParty.IsCaravan || defeatedParty.MobileParty.IsVillager))
						{
							num2 *= 1f + DefaultPerks.Roguery.KnowHow.PrimaryBonus;
						}
						mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, num2));
					}
				}
				for (int i = 0; i < mblist2.Count; i++)
				{
					mblist[i] = new KeyValuePair<MapEventParty, float>(mblist2[i].Key, mblist2[i].Value / num * mblist[i].Value * 0.5f);
				}
			}
			return mblist;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0006B7C4 File Offset: 0x000699C4
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootCasualtyChances(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
			if (!defeatedParty.IsSettlement || !defeatedParty.Settlement.IsTown)
			{
				float num = 0f;
				foreach (MapEventParty mapEventParty in winnerParties)
				{
					MobileParty mobileParty = mapEventParty.Party.MobileParty;
					if (mapEventParty.ContributionToBattle > 0 && mapEventParty.Party.MemberRoster.Count > 0 && (mobileParty == null || (!mobileParty.IsGarrison && !mobileParty.IsMilitia)))
					{
						mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, (float)mapEventParty.ContributionToBattle));
						num += (float)mapEventParty.ContributionToBattle;
					}
				}
				for (int i = 0; i < mblist.Count; i++)
				{
					mblist[i] = new KeyValuePair<MapEventParty, float>(mblist[i].Key, mblist[i].Value / num * 1f);
				}
			}
			return mblist;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0006B8D8 File Offset: 0x00069AD8
		public override float CalculateShipDamageAfterDefeat(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0006B8DF File Offset: 0x00069ADF
		public override MBReadOnlyList<KeyValuePair<Ship, MapEventParty>> DistributeDefeatedPartyShipsAmongWinners(MapEvent mapEvent, MBReadOnlyList<Ship> shipsToLoot, MBReadOnlyList<MapEventParty> winnerParties)
		{
			return new MBReadOnlyList<KeyValuePair<Ship, MapEventParty>>();
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0006B8E8 File Offset: 0x00069AE8
		public override float GetBannerLootChanceFromDefeatedHero(Hero defeatedHero)
		{
			Clan clan = defeatedHero.Clan;
			Hero hero;
			if (clan == null)
			{
				hero = null;
			}
			else
			{
				Kingdom kingdom = clan.Kingdom;
				hero = ((kingdom != null) ? kingdom.RulingClan.Leader : null);
			}
			if (hero == defeatedHero)
			{
				return 0.1f;
			}
			Clan clan2 = defeatedHero.Clan;
			if (((clan2 != null) ? clan2.Leader : null) == defeatedHero)
			{
				return 0.25f;
			}
			return 0.5f;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0006B944 File Offset: 0x00069B44
		public override ItemObject GetBannerRewardForWinningMapEvent(MapEvent mapEvent)
		{
			if (mapEvent.IsHideoutBattle || (mapEvent.AttackerSide.MissionSide == mapEvent.PlayerSide && mapEvent.IsSiegeAssault))
			{
				bool isHideoutBattle = mapEvent.IsHideoutBattle;
				Settlement mapEventSettlement = mapEvent.MapEventSettlement;
				float num = (isHideoutBattle ? 0.1f : 0.5f);
				if (MBRandom.RandomFloat <= num)
				{
					MBList<ItemObject> mblist = Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems().ToMBList<ItemObject>();
					if (mblist.Count > 0)
					{
						mblist.Shuffle<ItemObject>();
						int num2 = (isHideoutBattle ? 1 : mapEventSettlement.Town.GetWallLevel());
						foreach (ItemObject itemObject in mblist)
						{
							if (((BannerComponent)itemObject.ItemComponent).BannerLevel == num2 && (itemObject.Culture == null || itemObject.Culture.StringId == "neutral_culture" || (!isHideoutBattle && itemObject.Culture == mapEventSettlement.Culture)))
							{
								return itemObject;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0006BA70 File Offset: 0x00069C70
		public override float GetSunkenShipMoraleEffect(PartyBase shipOwner, Ship ship)
		{
			return 0f;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0006BA78 File Offset: 0x00069C78
		public override float CalculateMoraleChangeOnRoundVictory(PartyBase party, MapEventSide partySide, BattleSideEnum roundWinner)
		{
			float num = 0f;
			if (partySide.MissionSide != roundWinner && roundWinner != BattleSideEnum.None)
			{
				if (partySide.MapEvent.RetreatingSide != BattleSideEnum.None)
				{
					num = -1f;
				}
				else
				{
					num = -3f;
				}
			}
			return num;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0006BAB5 File Offset: 0x00069CB5
		public override float GetShipSiegeEngineHitMoraleEffect(Ship ship, SiegeEngineType siegeEngineType)
		{
			return 0f;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0006BABC File Offset: 0x00069CBC
		public override Figurehead GetFigureheadLoot(MBReadOnlyList<MapEventParty> defeatedParties, PartyBase defeatedSideLeaderParty)
		{
			return null;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0006BABF File Offset: 0x00069CBF
		public override MBReadOnlyList<MapEventParty> GetWinnerPartiesThatCanPlunderGoldFromShips(MBReadOnlyList<MapEventParty> winnerParties)
		{
			return new MBReadOnlyList<MapEventParty>();
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0006BAC6 File Offset: 0x00069CC6
		public override bool CanTroopBeTakenPrisoner(CharacterObject troop)
		{
			return true;
		}

		// Token: 0x040007A7 RID: 1959
		private static readonly int[] _indices = new int[12];

		// Token: 0x040007A8 RID: 1960
		private const float DestroyHideoutBannerLootChance = 0.1f;

		// Token: 0x040007A9 RID: 1961
		private const float CaptureSettlementBannerLootChance = 0.5f;

		// Token: 0x040007AA RID: 1962
		private const float DefeatRegularHeroBannerLootChance = 0.5f;

		// Token: 0x040007AB RID: 1963
		private const float DefeatClanLeaderBannerLootChance = 0.25f;

		// Token: 0x040007AC RID: 1964
		private const float DefeatKingdomRulerBannerLootChance = 0.1f;

		// Token: 0x040007AD RID: 1965
		private const float MainPartyMemberScatterChance = 0.1f;
	}
}
