using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010F RID: 271
	public class DefaultCombatXpModel : CombatXpModel
	{
		// Token: 0x060017E0 RID: 6112 RVA: 0x00070F4C File Offset: 0x0006F14C
		public override SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit)
		{
			SkillObject skillObject = DefaultSkills.Athletics;
			if (isSiegeEngineHit)
			{
				skillObject = DefaultSkills.Engineering;
			}
			else if (weapon != null)
			{
				skillObject = weapon.RelevantSkill;
			}
			return skillObject;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00070F78 File Offset: 0x0006F178
		public override ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType)
		{
			int num = attackedTroop.MaxHitPoints();
			float num2 = 0f;
			BattleSideEnum battleSideEnum = BattleSideEnum.Attacker;
			MapEvent.PowerCalculationContext powerCalculationContext = MapEvent.PowerCalculationContext.PlainBattle;
			if (((attackerParty != null) ? attackerParty.MapEvent : null) != null)
			{
				num2 = attackerParty.MapEventSide.LeaderSimulationModifier;
				battleSideEnum = attackerParty.Side;
				powerCalculationContext = attackerParty.MapEvent.SimulationContext;
			}
			float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(attackedTroop, battleSideEnum.GetOppositeSide(), powerCalculationContext, num2);
			float num3 = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(attackerTroop, battleSideEnum, powerCalculationContext, num2) + 0.5f;
			float num4 = troopPower + 0.5f;
			int num5 = MathF.Min(damage, num) + (isFatal ? num : 0);
			float num6 = 0.4f * num3 * num4 * (float)num5;
			num6 *= DefaultCombatXpModel.GetXpfMultiplierForMissionType(missionType);
			ExplainedNumber explainedNumber = new ExplainedNumber(num6, false, null);
			if (attackerParty != null)
			{
				DefaultCombatXpModel.GetBattleXpBonusFromPerks(attackerParty, ref explainedNumber, attackerTroop);
			}
			bool flag = attackerParty == null || !attackerParty.IsMobile || attackerParty.MobileParty.IsCurrentlyAtSea;
			if (captain != null && captain.IsHero && !flag)
			{
				BattleEnvironment battleEnvironment = ((attackerParty != null && attackerParty.MobileParty != null) ? attackerParty.MobileParty.CurrentBattleEnvironment : BattleEnvironment.Any);
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Leadership.InspiringLeader, battleEnvironment, captain, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000710B4 File Offset: 0x0006F2B4
		private static float GetXpfMultiplierForMissionType(CombatXpModel.MissionTypeEnum missionType)
		{
			float num;
			if (missionType == CombatXpModel.MissionTypeEnum.NoXp)
			{
				num = 0f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.PracticeFight)
			{
				num = 0.0625f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.Tournament)
			{
				num = 0.33f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.SimulationBattle)
			{
				num = 0.9f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.Battle)
			{
				num = 1f;
			}
			else
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00071103 File Offset: 0x0006F303
		public override float GetXpMultiplierFromShotDifficulty(float shotDifficulty)
		{
			if (shotDifficulty > 14.4f)
			{
				shotDifficulty = 14.4f;
			}
			return MBMath.Lerp(0f, 2f, (shotDifficulty - 1f) / 13.4f, 1E-05f);
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x00071135 File Offset: 0x0006F335
		public override float CaptainRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0007113C File Offset: 0x0006F33C
		private static void GetBattleXpBonusFromPerks(PartyBase party, ref ExplainedNumber xpToGain, CharacterObject troop)
		{
			if (party.IsMobile && party.MobileParty.LeaderHero != null)
			{
				if (!troop.IsRanged)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.Trainer, party.MobileParty, false, ref xpToGain);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.BaptisedInBlood, party.MobileParty, false, ref xpToGain);
				}
				if (troop.HasThrowingWeapon())
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.Resourceful, party.MobileParty, false, ref xpToGain);
				}
				if (troop.IsInfantry)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.CorpsACorps, party.MobileParty, true, ref xpToGain);
				}
				PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.LeadByExample, party.MobileParty, true, ref xpToGain);
				if (troop.IsRanged)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.MountedCrossbowman, party.MobileParty, false, ref xpToGain);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.BullsEye, party.MobileParty, true, ref xpToGain);
				}
				if (troop.Culture.IsBandit)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.NoRestForTheWicked, party.MobileParty, true, ref xpToGain);
				}
			}
			if (party.IsMobile && party.MobileParty.IsGarrison)
			{
				Settlement currentSettlement = party.MobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town.Governor : null) != null)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.ProjectileDeflection, party.MobileParty.CurrentSettlement.Town, false, ref xpToGain);
					if (troop.IsMounted)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Guards, party.MobileParty.CurrentSettlement.Town, false, ref xpToGain);
					}
				}
			}
		}
	}
}
