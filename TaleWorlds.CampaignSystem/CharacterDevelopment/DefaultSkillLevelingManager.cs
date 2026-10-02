using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C3 RID: 963
	public class DefaultSkillLevelingManager : ISkillLevelingManager
	{
		// Token: 0x06003824 RID: 14372 RVA: 0x000E8FF0 File Offset: 0x000E71F0
		public void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack)
		{
			if (isTeamKill)
			{
				return;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (affectorCharacter.IsHero)
			{
				Hero heroObject = affectorCharacter.HeroObject;
				CombatXpModel combatXpModel = Campaign.Current.Models.CombatXpModel;
				CharacterObject characterObject = heroObject.CharacterObject;
				MobileParty partyBelongedTo = heroObject.PartyBelongedTo;
				explainedNumber = new ExplainedNumber(combatXpModel.GetXpFromHit(characterObject, captain, affectedCharacter, (partyBelongedTo != null) ? partyBelongedTo.Party : null, (int)damageAmount, isFatal, missionType).ResultNumber, false, null);
				SkillObject skillObject;
				if (affectorWeapon != null)
				{
					skillObject = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(affectorWeapon, isSiegeEngineHit);
					float num = ((skillObject == DefaultSkills.Bow) ? 0.5f : 1f);
					if (shotDifficulty > 0f)
					{
						explainedNumber.AddFactor(num * Campaign.Current.Models.CombatXpModel.GetXpMultiplierFromShotDifficulty(shotDifficulty), null);
					}
				}
				else
				{
					skillObject = (isHorseCharge ? DefaultSkills.Riding : DefaultSkills.Athletics);
				}
				heroObject.AddSkillXp(skillObject, (float)MBRandom.RoundRandomized((float)explainedNumber.RoundedResultNumber));
				if (!isSiegeEngineHit && !isHorseCharge)
				{
					float num2 = shotDifficulty * 0.15f;
					if (isAffectorMounted)
					{
						float num3 = 0.5f;
						if (num2 > 0f)
						{
							num3 += num2;
						}
						if (speedBonusFromMovement > 0f)
						{
							num3 *= 1f + speedBonusFromMovement;
						}
						if (num3 > 0f)
						{
							DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(num3 * (float)explainedNumber.RoundedResultNumber), heroObject.CharacterObject.Equipment.Horse.Item);
						}
					}
					else
					{
						float num4 = 1f;
						if (num2 > 0f)
						{
							num4 += num2;
						}
						if (speedBonusFromMovement > 0f)
						{
							num4 += 1.5f * speedBonusFromMovement;
						}
						if (num4 > 0f)
						{
							heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(num4 * explainedNumber.ResultNumber));
						}
					}
				}
				if (isSneakAttack)
				{
					heroObject.AddSkillXp(DefaultSkills.Roguery, 78f);
				}
			}
			if (commander != null && commander != affectorCharacter.HeroObject && commander.PartyBelongedTo != null)
			{
				this.OnTacticsUsed(commander.PartyBelongedTo, (float)MathF.Ceiling(0.02f * (float)explainedNumber.RoundedResultNumber));
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000E9214 File Offset: 0x000E7414
		public void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				float num = (float)destroyedSiegeEngine.ManDayCost * 20f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, num, PartyRole.Engineer);
			}
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000E924C File Offset: 0x000E744C
		public void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty)
		{
			int xpReward = Campaign.Current.Models.PartyTrainingModel.GetXpReward(affectedCharacter);
			if (affectorCharacter.IsHero)
			{
				ItemObject defaultWeapon = CharacterHelper.GetDefaultWeapon(affectorCharacter);
				Hero heroObject = affectorCharacter.HeroObject;
				if (defaultWeapon != null)
				{
					SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(defaultWeapon.GetWeaponWithUsageIndex(0), false);
					heroObject.AddSkillXp(skillForWeapon, (float)xpReward);
				}
				if (affectorCharacter.IsMounted)
				{
					float num = (float)xpReward * 0.3f;
					DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(num), heroObject.CharacterObject.Equipment.Horse.Item);
				}
				else
				{
					float num2 = (float)xpReward * 0.3f;
					heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(num2));
				}
			}
			if (commanderParty != null && commanderParty.IsMobile && !commanderParty.MapEvent.IsNavalMapEvent && commanderParty.LeaderHero != null && commanderParty.LeaderHero != affectedCharacter.HeroObject)
			{
				this.OnTacticsUsed(commanderParty.MobileParty, (float)MathF.Ceiling(0.02f * (float)xpReward));
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000E9358 File Offset: 0x000E7558
		public void OnTradeProfitMade(PartyBase party, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float num = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party.MobileParty, DefaultSkills.Trade, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000E9384 File Offset: 0x000E7584
		public void OnTradeProfitMade(Hero hero, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float num = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Trade, num, hero == Hero.MainHero);
			}
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000E93B2 File Offset: 0x000E75B2
		public void OnSettlementProjectFinished(Settlement settlement)
		{
			DefaultSkillLevelingManager.OnSettlementSkillExercised(settlement, DefaultSkills.Steward, 1000f);
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000E93C4 File Offset: 0x000E75C4
		public void OnSettlementGoverned(Hero governor, Settlement settlement)
		{
			float prosperityChange = settlement.Town.ProsperityChange;
			if (prosperityChange > 0f)
			{
				float num = prosperityChange * 30f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(governor, DefaultSkills.Steward, num, true);
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x000E93FC File Offset: 0x000E75FC
		public void OnInfluenceSpent(Hero hero, float amountSpent)
		{
			if (hero.PartyBelongedTo != null)
			{
				float num = 10f * amountSpent;
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Steward, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000E942C File Offset: 0x000E762C
		public void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default)
		{
			if ((hero.PartyBelongedTo == null && detail != ChangeRelationAction.ChangeRelationDetail.Emissary) || relationChange <= 0f)
			{
				return;
			}
			int charmExperienceFromRelationGain = Campaign.Current.Models.DiplomacyModel.GetCharmExperienceFromRelationGain(gainedRelationWith, relationChange, detail);
			if (hero.PartyBelongedTo != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, PartyRole.PartyLeader);
				return;
			}
			DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, true);
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000E9494 File Offset: 0x000E7694
		public void OnTroopRecruited(Hero hero, int amount, int tier)
		{
			if (amount > 0)
			{
				int num = amount * tier * 2;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Leadership, (float)num, true);
			}
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x000E94BC File Offset: 0x000E76BC
		public void OnBribeGiven(int amount)
		{
			if (amount > 0)
			{
				float num = (float)amount * 0.1f;
				DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x000E94E7 File Offset: 0x000E76E7
		public void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count)
		{
			if (count > 0)
			{
				DefaultSkillLevelingManager.OnPersonalSkillExercised(mobileParty.LeaderHero, DefaultSkills.Roguery, (float)(count * 2 * bandit.Tier), true);
			}
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x000E950C File Offset: 0x000E770C
		public void OnMainHeroReleasedFromCaptivity(float captivityTime)
		{
			float num = captivityTime * 0.5f;
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, num, true);
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x000E9534 File Offset: 0x000E7734
		public void OnMainHeroTortured()
		{
			float num = MBRandom.RandomFloatRanged(50f, 100f);
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, num, true);
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x000E9564 File Offset: 0x000E7764
		public void OnMainHeroDisguised(bool isNotCaught)
		{
			float num = (isNotCaught ? MBRandom.RandomFloatRanged(10f, 25f) : MBRandom.RandomFloatRanged(1f, 10f));
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, num, PartyRole.PartyLeader);
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x000E95A8 File Offset: 0x000E77A8
		public void OnRaid(MobileParty attackerParty, ItemRoster lootedItems)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = (float)lootedItems.TradeGoodsTotalValue * 0.5f + (float)(lootedItems.NumberOfMounts * 100) + (float)(lootedItems.NumberOfLivestockAnimals * 25) + (float)(lootedItems.NumberOfPackAnimals * 25);
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num, true);
			}
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x000E9600 File Offset: 0x000E7800
		public void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = 0f;
				if (forcedParty.IsVillager)
				{
					num = (attacked ? 0.75f : 0.5f);
				}
				else if (forcedParty.IsCaravan)
				{
					num = (attacked ? 0.15f : 0.1f);
				}
				float num2 = (float)(lootedItems.TradeGoodsTotalValue + lootedItems.NumberOfMounts * 200 + lootedItems.NumberOfLivestockAnimals * 50 + lootedItems.NumberOfPackAnimals * 50) * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num2, true);
			}
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x000E968C File Offset: 0x000E788C
		public void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			int num = 0;
			for (int i = 0; i < prisonerRoster.Count; i++)
			{
				num += prisonerRoster.data[i].Character.Tier * prisonerRoster.data[i].Number;
			}
			int num2 = num * 2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Roguery, (float)num2, PartyRole.PartyLeader);
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000E96EC File Offset: 0x000E78EC
		public void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier)
		{
			float num = (float)(surgerySuccess ? (10 * troopTier) : (5 * troopTier));
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Medicine, num, PartyRole.Surgeon);
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000E9714 File Offset: 0x000E7914
		public void OnTacticsUsed(MobileParty party, float xp)
		{
			if (xp > 0f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Tactics, xp, PartyRole.PartyLeader);
			}
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x000E972B File Offset: 0x000E792B
		public void OnHideoutSpotted(MobileParty party, PartyBase spottedParty)
		{
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Scouting, 100f, PartyRole.Scout);
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000E9740 File Offset: 0x000E7940
		public void OnTrackDetected(Track track)
		{
			float skillFromTrackDetected = Campaign.Current.Models.MapTrackModel.GetSkillFromTrackDetected(track);
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Scouting, skillFromTrackDetected, PartyRole.Scout);
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x000E9775 File Offset: 0x000E7975
		public void OnTravelOnFoot(Hero hero)
		{
			hero.AddSkillXp(DefaultSkills.Athletics, (float)(MBRandom.RoundRandomized(0.2f * hero.PartyBelongedTo.Speed) + 1));
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000E979C File Offset: 0x000E799C
		public void OnTravelOnHorse(Hero hero)
		{
			ItemObject item = hero.CharacterObject.Equipment.Horse.Item;
			DefaultSkillLevelingManager.OnGainingRidingExperience(hero, (float)MBRandom.RoundRandomized(0.3f * hero.PartyBelongedTo.Speed), item);
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000E97E0 File Offset: 0x000E79E0
		public void OnHeroHealedWhileWaiting(Hero hero, int healingAmount)
		{
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.EffectiveSurgeon != null)
			{
				float num = (float)Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(hero.PartyBelongedTo.Party);
				float num2 = ((hero.PartyBelongedTo.CurrentSettlement != null && !hero.PartyBelongedTo.CurrentSettlement.IsCastle) ? 0.2f : 0.1f);
				num *= (float)healingAmount * num2 * (1f + (float)hero.PartyBelongedTo.EffectiveSurgeon.Level * 0.1f);
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Medicine, num, PartyRole.Surgeon);
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000E988C File Offset: 0x000E7A8C
		public void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier)
		{
			float num = (float)(Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(mobileParty.Party) * healedTroopCount) * averageTier;
			float num2 = ((mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsCastle) ? 2f : 1f);
			num *= num2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Medicine, num, PartyRole.Surgeon);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000E98EC File Offset: 0x000E7AEC
		public void OnLeadingArmy(MobileParty mobileParty)
		{
			Army army = mobileParty.Army;
			float num = ((army != null) ? army.EstimatedStrength : mobileParty.Party.EstimatedStrength) * 0.0004f * mobileParty.Army.Morale;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Leadership, num, PartyRole.PartyLeader);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000E9938 File Offset: 0x000E7B38
		public void OnHighMorale(MobileParty mobileParty)
		{
			float num = mobileParty.Party.EstimatedStrength * 0.004f * (mobileParty.Morale - Campaign.Current.Models.PartyMoraleModel.HighMoraleValue + 1f);
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Leadership, num, PartyRole.PartyLeader);
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000E9988 File Offset: 0x000E7B88
		public void OnSieging(MobileParty mobileParty)
		{
			int num = mobileParty.MemberRoster.TotalManCount;
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
			{
				foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
				{
					if (mobileParty2 != mobileParty)
					{
						num += mobileParty2.MemberRoster.TotalManCount;
					}
				}
			}
			float num2 = 0.25f * MathF.Sqrt((float)num);
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, num2, PartyRole.Engineer);
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000E9A28 File Offset: 0x000E7C28
		public void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine)
		{
			float num = 30f + 2f * (float)siegeEngine.Difficulty;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, num, PartyRole.Engineer);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000E9A58 File Offset: 0x000E7C58
		public void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			if (hero != null)
			{
				SkillObject skillObject = DefaultSkills.Leadership;
				float num = 0.025f;
				if (troop.Occupation == Occupation.Bandit)
				{
					skillObject = DefaultSkills.Roguery;
					num = 0.05f;
				}
				float num2 = (float)Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(party, troop, upgrade) * num * (float)numberOfTroops;
				hero.AddSkillXp(skillObject, num2);
			}
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000E9AC4 File Offset: 0x000E7CC4
		public void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			float num = (float)Campaign.Current.Models.PersuasionModel.GetSkillXpFromPersuasion(difficulty, argumentDifficultyBonusCoefficient);
			if (num > 0f)
			{
				targetHero.AddSkillXp(skill, num);
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000E9AFC File Offset: 0x000E7CFC
		public void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded)
		{
			float rogueryRewardOnPrisonBreak = Campaign.Current.Models.PrisonBreakModel.GetRogueryRewardOnPrisonBreak(prisonerHero, isSucceeded);
			if (rogueryRewardOnPrisonBreak > 0f)
			{
				Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryRewardOnPrisonBreak);
			}
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000E9B38 File Offset: 0x000E7D38
		public void OnWallBreached(MobileParty party)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, 250f, PartyRole.Engineer);
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000E9B5C File Offset: 0x000E7D5C
		public void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty)
		{
			if (attackerParty.LeaderHero != null)
			{
				int num = MathF.Ceiling(forcedParty.Settlement.Village.Hearth / 10f);
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, (float)num, true);
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000E9BA0 File Offset: 0x000E7DA0
		public void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = (attacked ? 0.75f : 0.5f);
				float num2 = (float)(lootedItems.TradeGoodsTotalValue + lootedItems.NumberOfMounts * 200 + lootedItems.NumberOfLivestockAnimals * 50 + lootedItems.NumberOfPackAnimals * 50) * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num2, true);
			}
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000E9C04 File Offset: 0x000E7E04
		public void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType)
		{
			int num = ((currentTerrainType == TerrainType.Forest) ? MBRandom.RoundRandomized(5f) : MBRandom.RoundRandomized(3f));
			hero.AddSkillXp(DefaultSkills.Scouting, isCaravanParty ? ((float)num / 2f) : ((float)num));
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000E9C48 File Offset: 0x000E7E48
		public void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType)
		{
			float num = 0f;
			float lastCalculatedSpeed = mobileParty._lastCalculatedSpeed;
			if (lastCalculatedSpeed > 1f)
			{
				bool flag = currentTerrainType == TerrainType.Desert || currentTerrainType == TerrainType.Dune || currentTerrainType == TerrainType.Forest || currentTerrainType == TerrainType.Snow;
				num = lastCalculatedSpeed * (1f + MathF.Pow((float)mobileParty.MemberRoster.TotalManCount, 0.66f)) * (flag ? 0.25f : 0.15f);
			}
			if (mobileParty.IsCaravan)
			{
				num *= 0.5f;
			}
			if (num >= 5f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Scouting, num, PartyRole.Scout);
			}
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000E9CD4 File Offset: 0x000E7ED4
		public void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			float num = 0.025f;
			SkillObject skillObject = DefaultSkills.Leadership;
			if (troop.Occupation == Occupation.Bandit)
			{
				num = 0.05f;
				skillObject = DefaultSkills.Roguery;
			}
			float num2 = (float)excessXp * num;
			hero.AddSkillXp(skillObject, num2);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000E9D20 File Offset: 0x000E7F20
		public void OnFoodConsumed(MobileParty mobileParty, bool wasStarving)
		{
			if (!wasStarving && mobileParty.ItemRoster.FoodVariety > 3 && mobileParty.EffectiveQuartermaster != null)
			{
				float num = (float)MathF.Round(-mobileParty.BaseFoodChange * 100f) * ((float)mobileParty.ItemRoster.FoodVariety - 2f) / 3f;
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Steward, num, PartyRole.Quartermaster);
			}
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000E9D81 File Offset: 0x000E7F81
		public void OnAlleyCleared(Alley alley)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetInitialXpGainForMainHero());
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000E9DA8 File Offset: 0x000E7FA8
		public void OnDailyAlleyTick(Alley alley, Hero alleyLeader)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForMainHero());
			if (alleyLeader != null && !alleyLeader.IsDead)
			{
				alleyLeader.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForAssignedClanMember(alleyLeader));
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000E9E04 File Offset: 0x000E8004
		public void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain)
		{
			switch (difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20f);
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 100f);
				break;
			}
			if (extraXpGain)
			{
				lord.AddSkillXp(DefaultSkills.Steward, 100f);
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000E9E78 File Offset: 0x000E8078
		public void OnHideoutClearedAsGhost()
		{
			TextObject textObject = new TextObject("{=Obuhsttm}Ghost bonus: {XP} Roguery exp! (Base: {BASE_XP})", null);
			float rogueryXpGainAsGhost = Campaign.Current.Models.HideoutModel.GetRogueryXpGainAsGhost();
			float skillXp = Hero.MainHero.HeroDeveloper.GetSkillXp(DefaultSkills.Roguery);
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryXpGainAsGhost);
			float num = Hero.MainHero.HeroDeveloper.GetSkillXp(DefaultSkills.Roguery) - skillXp;
			textObject.SetTextVariable("BASE_XP", MathF.Floor(rogueryXpGainAsGhost));
			textObject.SetTextVariable("XP", MathF.Floor(num));
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), new Color(0f, 0f, 1f, 1f)));
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000E9F30 File Offset: 0x000E8130
		public void OnHideoutMissionEnd(bool isSucceeded)
		{
			float rogueryXpGainOnHideoutMissionEnd = Campaign.Current.Models.HideoutModel.GetRogueryXpGainOnHideoutMissionEnd(isSucceeded);
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryXpGainOnHideoutMissionEnd);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000E9F63 File Offset: 0x000E8163
		public void OnWarehouseProduction(EquipmentElement production)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Trade, Campaign.Current.Models.WorkshopModel.GetTradeXpPerWarehouseProduction(production));
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x000E9F8C File Offset: 0x000E818C
		public void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty)
		{
			if (defeatedParty.IsMobile)
			{
				float num = -1f;
				MobileParty mobileParty = defeatedParty.MobileParty;
				if (mobileParty.IsVillager)
				{
					num = 0.75f;
				}
				else if (mobileParty.IsCaravan)
				{
					num = 0.15f;
				}
				if (num > 0f)
				{
					float num2 = (float)goldAmount * num;
					winnerPartyLeader.HeroDeveloper.AddSkillXp(DefaultSkills.Roguery, num2, true, false);
				}
			}
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000E9FEC File Offset: 0x000E81EC
		public void OnShipDamaged(Ship ship, float rawDamage, float finalDamage)
		{
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x000E9FEE File Offset: 0x000E81EE
		public void OnShipRepaired(Ship ship, float repairedHitPoints)
		{
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000E9FF0 File Offset: 0x000E81F0
		public void OnTravelOnWater(MobileParty party)
		{
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000E9FF2 File Offset: 0x000E81F2
		private static void OnPersonalSkillExercised(Hero hero, SkillObject skill, float skillXp, bool shouldNotify = true)
		{
			if (hero != null)
			{
				hero.HeroDeveloper.AddSkillXp(skill, skillXp, true, shouldNotify);
			}
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x000EA008 File Offset: 0x000E8208
		private static void OnSettlementSkillExercised(Settlement settlement, SkillObject skill, float skillXp)
		{
			Town town = settlement.Town;
			Hero hero = ((town != null) ? town.Governor : null) ?? ((settlement.OwnerClan.Leader.CurrentSettlement == settlement) ? settlement.OwnerClan.Leader : null);
			if (hero == null)
			{
				return;
			}
			hero.AddSkillXp(skill, skillXp);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000EA058 File Offset: 0x000E8258
		private static void OnGainingRidingExperience(Hero hero, float baseXpAmount, ItemObject horse)
		{
			if (horse != null)
			{
				float num = 1f + (float)horse.Difficulty * 0.02f;
				hero.AddSkillXp(DefaultSkills.Riding, baseXpAmount * num);
			}
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x000EA08A File Offset: 0x000E828A
		private static void OnPartySkillExercised(MobileParty party, SkillObject skill, float skillXp, PartyRole partyRole = PartyRole.PartyLeader)
		{
			Hero effectiveRoleHolder = party.GetEffectiveRoleHolder(partyRole);
			if (effectiveRoleHolder == null)
			{
				return;
			}
			effectiveRoleHolder.AddSkillXp(skill, skillXp);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000EA0A7 File Offset: 0x000E82A7
		void ISkillLevelingManager.OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			this.OnPrisonerSell(mobileParty, in prisonerRoster);
		}

		// Token: 0x04001152 RID: 4434
		private const float TacticsXpCoefficient = 0.02f;

		// Token: 0x04001153 RID: 4435
		private const int RogueryXpGainOnSneakAttack = 78;
	}
}
