using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015F RID: 351
	public class DefaultSiegeEventModel : SiegeEventModel
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x0008969C File Offset: 0x0008789C
		public override string GetSiegeEngineMapPrefabName(SiegeEngineType type, int wallLevel, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon" : "ballista_b_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon" : "ballista_b_fire_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ram)
			{
				text = "batteringram_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.SiegeTower)
			{
				switch (wallLevel)
				{
				case 1:
					text = "siegetower_5m_mapicon";
					break;
				case 2:
					text = "siegetower_9m_mapicon";
					break;
				case 3:
					text = "siegetower_12m_mapicon";
					break;
				}
			}
			return text;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00089794 File Offset: 0x00087994
		public override string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType type)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.Trebuchet || type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "mangonel_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_fire_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = "ballista_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = "ballista_fire_mapicon_projectile";
			}
			return text;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00089804 File Offset: 0x00087A04
		public override string GetSiegeEngineMapReloadAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_reload" : "ballista_b_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_reload" : "ballista_b_fire_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon_reload";
			}
			return text;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000898AC File Offset: 0x00087AAC
		public override string GetSiegeEngineMapFireAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_fire" : "ballista_b_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_fire" : "ballista_b_fire_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon_fire";
			}
			return text;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00089954 File Offset: 0x00087B54
		public override sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType type, BattleSideEnum side)
		{
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.FireOnager)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Ballista || type == DefaultSiegeEngineTypes.FireBallista)
			{
				return 7;
			}
			if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				return 4;
			}
			if (type == DefaultSiegeEngineTypes.Bricole)
			{
				return 20;
			}
			return -1;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000899B0 File Offset: 0x00087BB0
		public override MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum battleSide)
		{
			MobileParty mobileParty = null;
			if (battleSide == BattleSideEnum.Attacker)
			{
				mobileParty = siegeEvent.BesiegerCamp.LeaderParty;
			}
			else
			{
				int num = 0;
				int num2 = -1;
				for (PartyBase partyBase = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege); partyBase != null; partyBase = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege))
				{
					if (partyBase.LeaderHero != null)
					{
						Hero effectiveEngineer = partyBase.MobileParty.EffectiveEngineer;
						int num3 = ((effectiveEngineer != null) ? effectiveEngineer.GetSkillValue(DefaultSkills.Engineering) : 0);
						if (num3 > num)
						{
							num = num3;
							mobileParty = partyBase.MobileParty;
						}
					}
				}
			}
			return mobileParty;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00089A30 File Offset: 0x00087C30
		public override float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side)
		{
			float num = 1f;
			Hero hero = null;
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Engineering.CampBuilding, out hero, true))
			{
				num += DefaultPerks.Engineering.CampBuilding.SecondaryBonus;
			}
			Hero hero2 = null;
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, out hero2, true))
			{
				num -= DefaultPerks.Medicine.SiegeMedic.SecondaryBonus;
			}
			if (side == BattleSideEnum.Defender)
			{
				Town town = siegeEvent.BesiegedSettlement.Town;
				if (((town != null) ? town.Governor : null) != null && siegeEvent.BesiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Medicine.BattleHardened))
				{
					num += DefaultPerks.Medicine.BattleHardened.SecondaryBonus;
				}
			}
			return num;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00089ACD File Offset: 0x00087CCD
		public override int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine)
		{
			return 2;
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00089AD0 File Offset: 0x00087CD0
		public override int GetColleteralDamageCasualties(SiegeEngineType siegeEngineType, MobileParty party)
		{
			int num = 1;
			Hero hero = null;
			if (party != null && party.HasPerk(DefaultPerks.Crossbow.Terror, out hero, false) && MBRandom.RandomFloat < DefaultPerks.Crossbow.Terror.PrimaryBonus)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00089B0C File Offset: 0x00087D0C
		public override float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town)
		{
			float num;
			if (target - SiegeBombardTargets.Wall > 1)
			{
				if (target != SiegeBombardTargets.People)
				{
					throw new ArgumentOutOfRangeException("target", target, null);
				}
				num = siegeEngineType.AntiPersonnelHitChance;
			}
			else
			{
				num = siegeEngineType.HitChance;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(num, false, null);
			if (battleSide == BattleSideEnum.Attacker && target == SiegeBombardTargets.RangedEngines)
			{
				float num2 = 0f;
				switch (town.GetWallLevel())
				{
				case 1:
					num2 = 0.05f;
					break;
				case 2:
					num2 = 0.1f;
					break;
				case 3:
					num2 = 0.15f;
					break;
				}
				explainedNumber.Add(-num2, new TextObject("{=b9NaTqyr}Extra Defender Defense", null), null);
			}
			if (battleSide == BattleSideEnum.Defender)
			{
				if (target == SiegeBombardTargets.RangedEngines && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.DreadfulSieger))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.DreadfulSieger.PrimaryBonus, DefaultPerks.Engineering.DreadfulSieger.Name);
				}
				if (siegeEngineType == DefaultSiegeEngineTypes.Ballista)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.Pavise, town, false, ref explainedNumber);
				}
			}
			SiegeEvent siegeEvent = town.Settlement.SiegeEvent;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			MobileParty effectiveSiegePartyForSide2 = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide.GetOppositeSide());
			if (effectiveSiegePartyForSide != null)
			{
				if (siegeEngineType == DefaultSiegeEngineTypes.Trebuchet || siegeEngineType == DefaultSiegeEngineTypes.Onager || siegeEngineType == DefaultSiegeEngineTypes.FireOnager)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.Foreman, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
				if (siegeEngineType == DefaultSiegeEngineTypes.Ballista || siegeEngineType == DefaultSiegeEngineTypes.FireBallista)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.Salvager, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
			}
			if (battleSide == BattleSideEnum.Defender && effectiveSiegePartyForSide2 != null && target == SiegeBombardTargets.RangedEngines)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.DungeonArchitect, effectiveSiegePartyForSide2, true, ref explainedNumber);
			}
			if (explainedNumber.ResultNumber < 0f)
			{
				explainedNumber = new ExplainedNumber(0f, false, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00089CBC File Offset: 0x00087EBC
		public override float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy)
		{
			if (strategy == DefaultSiegeStrategies.PreserveStrength)
			{
				return -9000f;
			}
			if (strategy != DefaultSiegeStrategies.Custom)
			{
				return MBRandom.RandomFloat;
			}
			if (siege == PlayerSiege.PlayerSiegeEvent && side == PlayerSiege.PlayerSide && siege.BesiegerCamp != null && siege.BesiegerCamp.LeaderParty == MobileParty.MainParty)
			{
				return 9000f;
			}
			return -100f;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00089D1C File Offset: 0x00087F1C
		public override float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			float availableManDayPower = this.GetAvailableManDayPower(side);
			float num = (float)type.ManDayCost;
			explainedNumber.Add(1f / (num / availableManDayPower * (float)CampaignTime.HoursInDay), this._baseConstructionSpeedText, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side.BattleSide);
			if (effectiveSiegePartyForSide != null)
			{
				int? num2;
				if (effectiveSiegePartyForSide == null)
				{
					num2 = null;
				}
				else
				{
					Hero effectiveEngineer = effectiveSiegePartyForSide.EffectiveEngineer;
					num2 = ((effectiveEngineer != null) ? new int?(effectiveEngineer.GetSkillValue(DefaultSkills.Engineering)) : null);
				}
				if ((num2 ?? 0) > 0)
				{
					SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.SiegeEngineProductionBonus, effectiveSiegePartyForSide, ref explainedNumber);
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				siegeEvent.BesiegedSettlement.Town.AddEffectOfBuildings(BuildingEffectEnum.SiegeEngineSpeed, ref explainedNumber);
				Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
				if (((governor != null) ? governor.CurrentSettlement : null) != null && governor.CurrentSettlement == siegeEvent.BesiegedSettlement)
				{
					SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.SiegeEngineProductionBonus, siegeEvent.BesiegedSettlement.Town, ref explainedNumber);
				}
			}
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty : null) != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Sweatshops, siegeEvent.BesiegerCamp.LeaderParty, false, ref explainedNumber);
			}
			if (effectiveSiegePartyForSide != null)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegePreparations = side.SiegeEngines.SiegePreparations;
				if (siegePreparations != null && !siegePreparations.IsConstructed)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.ImprovedTools, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
				else
				{
					PerkHelper.AddPerkBonusForParty(type.IsRanged ? DefaultPerks.Engineering.TorsionEngines : DefaultPerks.Engineering.Scaffolds, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
				PerkObject salvager = DefaultPerks.Engineering.Salvager;
				if (PerkHelper.GetPerkValueForTown(salvager, besiegedSettlement.Town))
				{
					explainedNumber.AddFactor(salvager.SecondaryBonus * besiegedSettlement.Militia, salvager.Name);
				}
			}
			if (side.BattleSide == BattleSideEnum.Attacker)
			{
				BesiegerCamp besiegerCamp = siegeEvent.BesiegerCamp;
				MobileParty mobileParty = ((besiegerCamp != null) ? besiegerCamp.LeaderParty : null);
				Hero hero;
				if (mobileParty == null)
				{
					hero = null;
				}
				else
				{
					Army army = mobileParty.Army;
					if (army == null)
					{
						hero = null;
					}
					else
					{
						MobileParty leaderParty = army.LeaderParty;
						hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
					}
				}
				Hero hero2 = hero ?? ((mobileParty != null) ? mobileParty.LeaderHero : null);
				if (hero2 != null)
				{
					TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.CalculatingSiegePrepEffect, ref explainedNumber);
					if (type == DefaultSiegeEngineTypes.Preparations)
					{
						TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.ValorSiegePrepEffect, ref explainedNumber);
					}
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00089F78 File Offset: 0x00088178
		public override float GetAvailableManDayPower(ISiegeEventSide side)
		{
			int num = -1;
			PartyBase partyBase = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			int num2 = 0;
			while (partyBase != null)
			{
				num2 += partyBase.NumberOfHealthyMembers;
				partyBase = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			}
			return MathF.Sqrt((float)num2);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00089FB4 File Offset: 0x000881B4
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
				town.AddEffectOfBuildings(BuildingEffectEnum.BallistaOnSiegeStart, ref explainedNumber);
				int num = 0;
				while ((float)num < explainedNumber.ResultNumber)
				{
					list.Add(DefaultSiegeEngineTypes.Ballista);
					num++;
				}
				ExplainedNumber explainedNumber2 = new ExplainedNumber(0f, false, null);
				town.AddEffectOfBuildings(BuildingEffectEnum.CatapultOnSiegeStart, ref explainedNumber2);
				int num2 = 0;
				while ((float)num2 < explainedNumber2.ResultNumber)
				{
					list.Add(DefaultSiegeEngineTypes.Catapult);
					num2++;
				}
				if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeWorks))
				{
					list.Add(DefaultSiegeEngineTypes.Catapult);
				}
			}
			return list;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0008A074 File Offset: 0x00088274
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp besiegerCamp)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			Hero hero = null;
			if (besiegerCamp.LeaderParty.HasPerk(DefaultPerks.Engineering.Battlements, out hero, false))
			{
				list.Add(DefaultSiegeEngineTypes.Ballista);
			}
			return list;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0008A0AC File Offset: 0x000882AC
		public override float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.BaseHitPoints, false, null);
			Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (battleSide == BattleSideEnum.Defender && besiegedSettlement.Town.Governor != null && besiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeEngineer))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.SiegeEngineer.PrimaryBonus, DefaultPerks.Engineering.SiegeEngineer.Name);
			}
			if (siegeEngine.IsRanged)
			{
				if (effectiveSiegePartyForSide != null)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.SiegeWorks, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
			}
			else if (battleSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.Carpenters, effectiveSiegePartyForSide, true, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0008A154 File Offset: 0x00088354
		public override float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.Damage, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (effectiveSiegePartyForSide != null)
			{
				if (battleSide == BattleSideEnum.Attacker)
				{
					if (target == SiegeBombardTargets.Wall)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.WallBreaker, effectiveSiegePartyForSide, true, ref explainedNumber);
					}
					if (target == SiegeBombardTargets.RangedEngines)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.MakeThemPay, effectiveSiegePartyForSide, true, ref explainedNumber);
					}
				}
				if ((target == SiegeBombardTargets.RangedEngines || target == SiegeBombardTargets.Wall) && effectiveSiegePartyForSide.LeaderHero != null)
				{
					PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Engineering.Masterwork, BattleEnvironment.Any, effectiveSiegePartyForSide.LeaderHero.CharacterObject, DefaultSkills.Engineering, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
				}
			}
			if (battleSide == BattleSideEnum.Defender && target == SiegeBombardTargets.RangedEngines)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Tactics.MakeThemPay, siegeEvent.BesiegedSettlement.Town, false, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0008A218 File Offset: 0x00088418
		public override int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(siegeEngine.CampaignRateOfFirePerDay, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side);
			if (effectiveSiegePartyForSide != null)
			{
				if (siegeEngine == DefaultSiegeEngineTypes.Ballista || siegeEngine == DefaultSiegeEngineTypes.FireBallista)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.Clockwork, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
				else if (siegeEngine == DefaultSiegeEngineTypes.Onager || siegeEngine == DefaultSiegeEngineTypes.Trebuchet || siegeEngine == DefaultSiegeEngineTypes.FireOnager)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.ArchitecturalCommisions, effectiveSiegePartyForSide, true, ref explainedNumber);
				}
			}
			return MathF.Round((float)(CampaignTime.MinutesInHour * CampaignTime.HoursInDay) / explainedNumber.ResultNumber);
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0008A2A1 File Offset: 0x000884A1
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party)
		{
			Hero hero = null;
			bool flag = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, out hero, true);
			Hero hero2 = null;
			bool flag2 = party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, out hero2, true);
			bool hasFirePerks = flag || flag2;
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Onager;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireOnager;
			}
			yield return DefaultSiegeEngineTypes.Trebuchet;
			yield break;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0008A2B1 File Offset: 0x000884B1
		public override IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party)
		{
			Hero hero = null;
			bool flag = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, out hero, true);
			Hero hero2 = null;
			bool flag2 = party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, out hero2, true);
			bool hasFirePerks = flag || flag2;
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Catapult;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireCatapult;
			}
			yield break;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0008A2C1 File Offset: 0x000884C1
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.Ram;
			yield break;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0008A2CA File Offset: 0x000884CA
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.SiegeTower;
			yield break;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0008A2D4 File Offset: 0x000884D4
		public override FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush()
		{
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement))
				{
					flattenedTroopRoster.Add(troopRosterElement);
				}
			}
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			if (playerSiegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty != null && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.Count > 0)
			{
				foreach (TroopRosterElement troopRosterElement2 in playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement2))
					{
						flattenedTroopRoster.Add(troopRosterElement2);
					}
				}
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				foreach (PartyBase partyBase in playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
				{
					if (partyBase != PartyBase.MainParty)
					{
						foreach (TroopRosterElement troopRosterElement3 in partyBase.MemberRoster.GetTroopRoster())
						{
							if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement3))
							{
								flattenedTroopRoster.Add(troopRosterElement3);
							}
						}
					}
				}
			}
			return flattenedTroopRoster;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0008A4B4 File Offset: 0x000886B4
		private bool IsPriorityTroopForSallyOutAmbush(TroopRosterElement troop)
		{
			CharacterObject character = troop.Character;
			return character.IsHero || character.HasMount();
		}

		// Token: 0x04000906 RID: 2310
		private readonly TextObject _baseConstructionSpeedText = new TextObject("{=MhGbcXJ4}Base construction speed", null);

		// Token: 0x04000907 RID: 2311
		private readonly TextObject _constructionSpeedProjectBonusText = new TextObject("{=xoTWC8Sm}Project Bonus", null);

		// Token: 0x04000908 RID: 2312
		private readonly TextObject _weatherConstructionPenalty = new TextObject("{=J6RjCKbk}Weather", null);
	}
}
