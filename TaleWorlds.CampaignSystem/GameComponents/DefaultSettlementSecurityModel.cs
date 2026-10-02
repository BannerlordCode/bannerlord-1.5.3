using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000158 RID: 344
	public class DefaultSettlementSecurityModel : SettlementSecurityModel
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00087ED5 File Offset: 0x000860D5
		public override int MaximumSecurityInSettlement
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x00087ED9 File Offset: 0x000860D9
		public override int SecurityDriftMedium
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x00087EDD File Offset: 0x000860DD
		public override float MapEventSecurityEffectRadius
		{
			get
			{
				return 50f;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x00087EE4 File Offset: 0x000860E4
		public override float HideoutClearedSecurityEffectRadius
		{
			get
			{
				return 100f;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x00087EEB File Offset: 0x000860EB
		public override int HideoutClearedSecurityGain
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x00087EEE File Offset: 0x000860EE
		public override int ThresholdForTaxCorruption
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x00087EF2 File Offset: 0x000860F2
		public override int ThresholdForHigherTaxCorruption
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x00087EF5 File Offset: 0x000860F5
		public override int ThresholdForTaxBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x00087EF9 File Offset: 0x000860F9
		public override int SettlementTaxBoostPercentage
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x00087EFC File Offset: 0x000860FC
		public override int SettlementTaxPenaltyPercentage
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00087F00 File Offset: 0x00086100
		public override int ThresholdForNotableRelationBonus
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00087F04 File Offset: 0x00086104
		public override int ThresholdForNotableRelationPenalty
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00087F08 File Offset: 0x00086108
		public override int DailyNotableRelationBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00087F0B File Offset: 0x0008610B
		public override int DailyNotableRelationPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x00087F0E File Offset: 0x0008610E
		public override int DailyNotablePowerBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00087F11 File Offset: 0x00086111
		public override int DailyNotablePowerPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00087F14 File Offset: 0x00086114
		public override ExplainedNumber CalculateSecurityChange(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateInfestedHideoutEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateRaidedVillageEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateUnderSiegeEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateProsperityEffectOnSecurity(town, ref explainedNumber);
			this.CalculateGarrisonEffectsOnSecurity(town, ref explainedNumber);
			this.CalculatePolicyEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateGovernorEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateProjectEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateIssueEffectsOnSecurity(town, ref explainedNumber);
			this.CalculatePerkEffectsOnSecurity(town, ref explainedNumber);
			this.CalculateSecurityDrift(town, ref explainedNumber);
			this.CalculateSettlementProjectSecurityBonuses(town, ref explainedNumber);
			this.CalculateSettlementPatrolPartiesBonuses(town, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00087FA8 File Offset: 0x000861A8
		private void CalculateSettlementPatrolPartiesBonuses(Town town, ref ExplainedNumber result)
		{
			if (town.Settlement.PatrolParty != null)
			{
				foreach (Building building in town.Buildings)
				{
					if (building.BuildingType == DefaultBuildingTypes.SettlementGuardHouse && building.CurrentLevel > 0)
					{
						result.Add((float)building.CurrentLevel * 0.5f + 0.5f, this.PatrolPartiesText, null);
						break;
					}
				}
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0008803C File Offset: 0x0008623C
		private void CalculateSettlementProjectSecurityBonuses(Town town, ref ExplainedNumber result)
		{
			town.AddEffectOfBuildings(BuildingEffectEnum.SecurityPerDay, ref result);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00088047 File Offset: 0x00086247
		private void CalculateProsperityEffectOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(MathF.Max(-5f, -0.0005f * town.Prosperity), this.ProsperityText, null);
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0008806C File Offset: 0x0008626C
		private void CalculateUnderSiegeEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.IsUnderSiege)
			{
				explainedNumber.Add(-3f, this.UnderSiegeText, null);
			}
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00088090 File Offset: 0x00086290
		private void CalculateRaidedVillageEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			using (List<Village>.Enumerator enumerator = town.Settlement.BoundVillages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.VillageState == Village.VillageStates.Looted)
					{
						num += -2f;
						break;
					}
				}
			}
			explainedNumber.Add(num, this.LootedVillagesText, null);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00088108 File Offset: 0x00086308
		private void CalculateInfestedHideoutEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay * 0.5f;
			foreach (Hideout hideout in Hideout.All)
			{
				if (hideout.IsInfested && Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, hideout.Settlement, false, false, MobileParty.NavigationType.Default) < num)
				{
					explainedNumber.Add(-2f, this.NearbyHideoutText, null);
					break;
				}
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x000881B0 File Offset: 0x000863B0
		private void CalculateSecurityDrift(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(-1f * (town.Security - (float)this.SecurityDriftMedium) / 15f, this.SecurityDriftText, null);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x000881DC File Offset: 0x000863DC
		private void CalculatePolicyEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			Kingdom kingdom = town.Settlement.OwnerClan.Kingdom;
			if (kingdom != null)
			{
				if (town.IsTown)
				{
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
					{
						explainedNumber.Add(1f, DefaultPolicies.Bailiffs.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom))
					{
						explainedNumber.Add(1f, DefaultPolicies.Serfdom.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Magistrates))
					{
						explainedNumber.Add(1f, DefaultPolicies.Magistrates.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TrialByJury))
				{
					explainedNumber.Add(-0.2f, DefaultPolicies.TrialByJury.Name, null);
				}
			}
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000882A8 File Offset: 0x000864A8
		private void CalculateGovernorEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000882AC File Offset: 0x000864AC
		private void CalculateGarrisonEffectsOnSecurity(Town town, ref ExplainedNumber result)
		{
			if (town.GarrisonParty != null && town.GarrisonParty.MemberRoster.Count != 0 && town.GarrisonParty.MemberRoster.TotalHealthyCount != 0)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(0.01f, false, null);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.StandUnited, town, false, ref explainedNumber);
				float num;
				float num2;
				float num3;
				this.CalculateStrengthOfGarrisonParty(town.GarrisonParty.Party, out num, out num2, out num3);
				float num4 = num * explainedNumber.ResultNumber;
				result.Add(num4, this.GarrisonText, null);
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Leadership.Authority, town))
				{
					float num5 = num4 * DefaultPerks.Leadership.Authority.PrimaryBonus;
					result.Add(num5, DefaultPerks.Leadership.Authority.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Riding.ReliefForce, town))
				{
					float num6 = num3 / num;
					float num7 = num4 * num6 * DefaultPerks.Riding.ReliefForce.SecondaryBonus;
					result.Add(num7, DefaultPerks.Riding.ReliefForce.Name, null);
				}
				float num8 = num2 / num;
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.MountedArchery, town))
				{
					float num9 = num4 * num8 * DefaultPerks.Bow.MountedArchery.SecondaryBonus;
					result.Add(num9, DefaultPerks.Bow.MountedArchery.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.RangersSwiftness, town))
				{
					float num10 = num4 * num8 * DefaultPerks.Bow.RangersSwiftness.SecondaryBonus;
					result.Add(num10, DefaultPerks.Bow.RangersSwiftness.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Crossbow.RenownMarksmen, town))
				{
					float num11 = num4 * num8 * DefaultPerks.Crossbow.RenownMarksmen.SecondaryBonus;
					result.Add(num11, DefaultPerks.Crossbow.RenownMarksmen.Name, null);
				}
			}
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x00088440 File Offset: 0x00086640
		private void CalculateStrengthOfGarrisonParty(PartyBase party, out float totalStrength, out float archerStrength, out float cavalryStrength)
		{
			totalStrength = 0f;
			archerStrength = 0f;
			cavalryStrength = 0f;
			float num = 0f;
			MapEvent.PowerCalculationContext powerCalculationContext = MapEvent.PowerCalculationContext.Siege;
			BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
			if (party.MapEvent != null)
			{
				battleSideEnum = party.Side;
				Hero leaderHero = party.LeaderHero;
				num = ((leaderHero != null) ? leaderHero.PowerModifier : 0f);
				powerCalculationContext = party.MapEvent.SimulationContext;
			}
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character != null)
				{
					float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(elementCopyAtIndex.Character, battleSideEnum, powerCalculationContext, num);
					float num2 = (float)(elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber) * troopPower;
					if (elementCopyAtIndex.Character.IsMounted)
					{
						cavalryStrength += num2;
					}
					if (elementCopyAtIndex.Character.IsRanged)
					{
						archerStrength += num2;
					}
					totalStrength += num2;
				}
			}
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x0008853C File Offset: 0x0008673C
		private void CalculatePerkEffectsOnSecurity(Town town, ref ExplainedNumber result)
		{
			float num = (float)town.Settlement.Parties.Where<MobileParty>(delegate(MobileParty x)
			{
				Clan actualClan = x.ActualClan;
				if (actualClan != null && !actualClan.IsAtWarWith(town.MapFaction))
				{
					Hero leaderHero = x.LeaderHero;
					return leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Leadership.Presence);
				}
				return false;
			}).Count<MobileParty>() * DefaultPerks.Leadership.Presence.PrimaryBonus;
			if (num > 0f)
			{
				result.Add(num, DefaultPerks.Leadership.Presence.Name, null);
			}
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Roguery.KnowHow))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.KnowHow, town, false, ref result);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.ToBeBlunt, town, false, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Throwing.Focus, town, false, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Skewer, town, false, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Tactics.Gensdarmes, town, false, ref result);
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00088626 File Offset: 0x00086826
		private void CalculateProjectEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00088628 File Offset: 0x00086828
		private void CalculateIssueEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementSecurity, town.Settlement, ref explainedNumber);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0008864A File Offset: 0x0008684A
		public override float GetLootedNearbyPartySecurityEffect(Town town, float sumOfAttackedPartyStrengths)
		{
			return -1f * sumOfAttackedPartyStrengths * 0.005f;
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00088659 File Offset: 0x00086859
		public override float GetNearbyBanditPartyDefeatedSecurityEffect(Town town, float sumOfAttackedPartyStrengths)
		{
			return sumOfAttackedPartyStrengths * 0.005f;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00088664 File Offset: 0x00086864
		public override void CalculateGoldGainDueToHighSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Security, (float)this.ThresholdForTaxBoost, (float)this.MaximumSecurityInSettlement, 0f, (float)this.SettlementTaxBoostPercentage);
			explainedNumber.AddFactor(num * 0.01f, this.Security);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000886AC File Offset: 0x000868AC
		public override void CalculateGoldCutDueToLowSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Security, (float)this.ThresholdForHigherTaxCorruption, (float)this.ThresholdForTaxCorruption, (float)this.SettlementTaxPenaltyPercentage, 0f);
			explainedNumber.AddFactor(-1f * num * 0.01f, this.CorruptionText);
		}

		// Token: 0x040008DF RID: 2271
		private const float GarrisonHighSecurityGain = 3f;

		// Token: 0x040008E0 RID: 2272
		private const float GarrisonLowSecurityPenalty = -3f;

		// Token: 0x040008E1 RID: 2273
		private const float NearbyHideoutPenalty = -2f;

		// Token: 0x040008E2 RID: 2274
		private const float VillageLootedSecurityEffect = -2f;

		// Token: 0x040008E3 RID: 2275
		private const float UnderSiegeSecurityEffect = -3f;

		// Token: 0x040008E4 RID: 2276
		private const float MaxProsperityEffect = -5f;

		// Token: 0x040008E5 RID: 2277
		private const float PerProsperityEffect = -0.0005f;

		// Token: 0x040008E6 RID: 2278
		private readonly TextObject GarrisonText = GameTexts.FindText("str_garrison", null);

		// Token: 0x040008E7 RID: 2279
		private readonly TextObject LootedVillagesText = GameTexts.FindText("str_looted_villages", null);

		// Token: 0x040008E8 RID: 2280
		private readonly TextObject CorruptionText = GameTexts.FindText("str_corruption", null);

		// Token: 0x040008E9 RID: 2281
		private readonly TextObject NearbyHideoutText = GameTexts.FindText("str_nearby_hideout", null);

		// Token: 0x040008EA RID: 2282
		private readonly TextObject UnderSiegeText = GameTexts.FindText("str_under_siege", null);

		// Token: 0x040008EB RID: 2283
		private readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x040008EC RID: 2284
		private readonly TextObject Security = GameTexts.FindText("str_security", null);

		// Token: 0x040008ED RID: 2285
		private readonly TextObject SecurityDriftText = GameTexts.FindText("str_security_drift", null);

		// Token: 0x040008EE RID: 2286
		private readonly TextObject PatrolPartiesText = GameTexts.FindText("str_patrol_parties", null);
	}
}
