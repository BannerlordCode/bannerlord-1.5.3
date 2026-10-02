using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000154 RID: 340
	public class DefaultSettlementLoyaltyModel : SettlementLoyaltyModel
	{
		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x0008679E File Offset: 0x0008499E
		public override float HighLoyaltyProsperityEffect
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000867A5 File Offset: 0x000849A5
		public override int LowLoyaltyProsperityEffect
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000867A8 File Offset: 0x000849A8
		public override int ThresholdForTaxBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x000867AC File Offset: 0x000849AC
		public override int ThresholdForTaxCorruption
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000867B0 File Offset: 0x000849B0
		public override int ThresholdForHigherTaxCorruption
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x000867B4 File Offset: 0x000849B4
		public override int ThresholdForProsperityBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000867B8 File Offset: 0x000849B8
		public override int ThresholdForProsperityPenalty
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x000867BC File Offset: 0x000849BC
		public override int AdditionalStarvationPenaltyStartDay
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000867C0 File Offset: 0x000849C0
		public override int AdditionalStarvationLoyaltyEffect
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x000867C3 File Offset: 0x000849C3
		public override int RebellionStartLoyaltyThreshold
		{
			get
			{
				if (!Campaign.Current.Options.IsHighRebellionEnabled)
				{
					return 15;
				}
				return 50;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000867DB File Offset: 0x000849DB
		public override int RebelliousStateStartLoyaltyThreshold
		{
			get
			{
				if (!Campaign.Current.Options.IsHighRebellionEnabled)
				{
					return 25;
				}
				return 60;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x000867F3 File Offset: 0x000849F3
		public override int LoyaltyBoostAfterRebellionStartValue
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x000867F6 File Offset: 0x000849F6
		public override int MilitiaBoostPercentage
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x000867FD File Offset: 0x000849FD
		public override float ThresholdForNotableRelationBonus
		{
			get
			{
				return 75f;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00086804 File Offset: 0x00084A04
		public override int DailyNotableRelationBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x00086807 File Offset: 0x00084A07
		public override int SettlementLoyaltyChangeDueToSecurityThreshold
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x0008680B File Offset: 0x00084A0B
		public override int MaximumLoyaltyInSettlement
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x0008680F File Offset: 0x00084A0F
		public override int LoyaltyDriftMedium
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x00086813 File Offset: 0x00084A13
		public override float HighSecurityLoyaltyEffect
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x0008681A File Offset: 0x00084A1A
		public override float LowSecurityLoyaltyEffect
		{
			get
			{
				return -2f;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x00086821 File Offset: 0x00084A21
		public override float SettlementOwnerDifferentCultureLoyaltyEffect
		{
			get
			{
				return -3f;
			}
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00086828 File Offset: 0x00084A28
		public override ExplainedNumber CalculateLoyaltyChange(Town town, bool includeDescriptions = false)
		{
			return this.CalculateLoyaltyChangeInternal(town, includeDescriptions);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00086834 File Offset: 0x00084A34
		public override void CalculateGoldGainDueToHighLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Loyalty, (float)this.ThresholdForTaxBoost, 100f, 0f, 0.2f);
			explainedNumber.AddFactor(num, this.LoyaltyText);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00086870 File Offset: 0x00084A70
		public override void CalculateGoldCutDueToLowLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Loyalty, (float)this.ThresholdForHigherTaxCorruption, (float)this.ThresholdForTaxCorruption, -0.5f, 0f);
			explainedNumber.AddFactor(num, this.CorruptionText);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000868B0 File Offset: 0x00084AB0
		private ExplainedNumber CalculateLoyaltyChangeInternal(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.GetSettlementLoyaltyChangeDueToFoodStocks(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToOwnerCulture(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToPolicies(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToProjects(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToIssues(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToSecurity(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToNotableRelations(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToGovernorPerks(town, ref explainedNumber);
			this.GetSettlementLoyaltyChangeDueToLoyaltyDrift(town, ref explainedNumber);
			if (town.Governor != null)
			{
				Settlement currentSettlement = town.Governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town && explainedNumber.ResultNumber > 0f)
				{
					TraitEffectHelper.ApplyTraitEffect(town.Governor, DefaultPersonalityTraitEffects.HonorLoyaltyGainEffect, ref explainedNumber);
				}
			}
			return explainedNumber;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00086960 File Offset: 0x00084B60
		private void GetSettlementLoyaltyChangeDueToGovernorPerks(Town town, ref ExplainedNumber explainedNumber)
		{
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.HeroicLeader, town, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PhysicianOfPeople, town, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.Durable, town, false, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.Discipline, town, false, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.WellStraped, town, false, ref explainedNumber);
			float num = 0f;
			for (int i = 0; i < town.Settlement.Parties.Count; i++)
			{
				MobileParty mobileParty = town.Settlement.Parties[i];
				if (mobileParty.ActualClan == town.OwnerClan)
				{
					if (mobileParty.IsMainParty)
					{
						for (int j = 0; j < mobileParty.MemberRoster.Count; j++)
						{
							CharacterObject characterAtIndex = mobileParty.MemberRoster.GetCharacterAtIndex(j);
							if (characterAtIndex.IsHero && characterAtIndex.HeroObject.GetPerkValue(DefaultPerks.Charm.Parade))
							{
								num += DefaultPerks.Charm.Parade.PrimaryBonus;
							}
						}
					}
					else if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Charm.Parade))
					{
						num += DefaultPerks.Charm.Parade.PrimaryBonus;
					}
				}
			}
			foreach (Hero hero in town.Settlement.HeroesWithoutParty)
			{
				if (hero.Clan == town.OwnerClan && hero.GetPerkValue(DefaultPerks.Charm.Parade))
				{
					num += DefaultPerks.Charm.Parade.PrimaryBonus;
				}
			}
			if (num > 0f)
			{
				explainedNumber.Add(num, DefaultPerks.Charm.Parade.Name, null);
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00086B04 File Offset: 0x00084D04
		private void GetSettlementLoyaltyChangeDueToNotableRelations(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			foreach (Hero hero in town.Settlement.Notables)
			{
				if (hero.SupporterOf != null)
				{
					if (hero.SupporterOf == town.Settlement.OwnerClan)
					{
						num += 0.5f;
					}
					else if (town.MapFaction.IsAtWarWith(hero.SupporterOf.MapFaction))
					{
						num += -0.5f;
					}
				}
			}
			if (!num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				explainedNumber.Add(num, this.NotableText, null);
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00086BC0 File Offset: 0x00084DC0
		private void GetSettlementLoyaltyChangeDueToOwnerCulture(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.OwnerClan.Culture != town.Settlement.Culture)
			{
				explainedNumber.Add(this.SettlementOwnerDifferentCultureLoyaltyEffect, DefaultSettlementLoyaltyModel.CultureText, null);
			}
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00086BF4 File Offset: 0x00084DF4
		private void GetSettlementLoyaltyChangeDueToPolicies(Town town, ref ExplainedNumber explainedNumber)
		{
			Kingdom kingdom = town.Owner.Settlement.OwnerClan.Kingdom;
			if (kingdom != null)
			{
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Citizenship))
				{
					if (town.Settlement.OwnerClan.Culture == town.Settlement.Culture)
					{
						explainedNumber.Add(0.5f, DefaultPolicies.Citizenship.Name, null);
					}
					else
					{
						explainedNumber.Add(-0.5f, DefaultPolicies.Citizenship.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.HuntingRights))
				{
					explainedNumber.Add(-0.2f, DefaultPolicies.HuntingRights.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.GrazingRights))
				{
					explainedNumber.Add(0.5f, DefaultPolicies.GrazingRights.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TrialByJury))
				{
					explainedNumber.Add(0.5f, DefaultPolicies.TrialByJury.Name, null);
				}
				if (town.IsTown && kingdom.ActivePolicies.Contains(DefaultPolicies.ImperialTowns))
				{
					if (kingdom.RulingClan == town.Settlement.OwnerClan)
					{
						explainedNumber.Add(1f, DefaultPolicies.ImperialTowns.Name, null);
					}
					else
					{
						explainedNumber.Add(-0.3f, DefaultPolicies.ImperialTowns.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.ForgivenessOfDebts))
				{
					explainedNumber.Add(2f, DefaultPolicies.ForgivenessOfDebts.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TribunesOfThePeople) && town.IsTown)
				{
					explainedNumber.Add(1f, DefaultPolicies.TribunesOfThePeople.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.DebasementOfTheCurrency))
				{
					explainedNumber.Add(-1f, DefaultPolicies.DebasementOfTheCurrency.Name, null);
				}
			}
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00086DD0 File Offset: 0x00084FD0
		private void GetSettlementLoyaltyChangeDueToFoodStocks(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.IsStarving)
			{
				float num = -1f;
				if (town.Settlement.Party.DaysStarving > 14f)
				{
					num += -1f;
				}
				explainedNumber.Add(num, this.StarvingText, null);
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00086E20 File Offset: 0x00085020
		private void GetSettlementLoyaltyChangeDueToSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = ((town.Security > (float)this.SettlementLoyaltyChangeDueToSecurityThreshold) ? MBMath.Map(town.Security, (float)this.SettlementLoyaltyChangeDueToSecurityThreshold, (float)this.MaximumLoyaltyInSettlement, 0f, this.HighSecurityLoyaltyEffect) : MBMath.Map(town.Security, 0f, (float)this.SettlementLoyaltyChangeDueToSecurityThreshold, this.LowSecurityLoyaltyEffect, 0f));
			explainedNumber.Add(num, this.SecurityText, null);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00086E93 File Offset: 0x00085093
		private void GetSettlementLoyaltyChangeDueToProjects(Town town, ref ExplainedNumber explainedNumber)
		{
			town.AddEffectOfBuildings(BuildingEffectEnum.Loyalty, ref explainedNumber);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00086E9D File Offset: 0x0008509D
		private void GetSettlementLoyaltyChangeDueToIssues(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementLoyalty, town.Settlement, ref explainedNumber);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00086EBF File Offset: 0x000850BF
		private void GetSettlementLoyaltyChangeDueToLoyaltyDrift(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(-0.1f * (town.Loyalty - (float)this.LoyaltyDriftMedium), this.LoyaltyDriftText, null);
		}

		// Token: 0x040008C3 RID: 2243
		private const float StarvationLoyaltyEffect = -1f;

		// Token: 0x040008C4 RID: 2244
		private const int AdditionalStarvationLoyaltyEffectAfterDays = 14;

		// Token: 0x040008C5 RID: 2245
		private const float NotableSupportsOwnerLoyaltyEffect = 0.5f;

		// Token: 0x040008C6 RID: 2246
		private const float NotableSupportsEnemyLoyaltyEffect = -0.5f;

		// Token: 0x040008C7 RID: 2247
		private readonly TextObject StarvingText = GameTexts.FindText("str_starving", null);

		// Token: 0x040008C8 RID: 2248
		private static readonly TextObject CultureText = new TextObject("{=YjoXyFDX}Owner Culture", null);

		// Token: 0x040008C9 RID: 2249
		private readonly TextObject NotableText = GameTexts.FindText("str_notable_relations", null);

		// Token: 0x040008CA RID: 2250
		private readonly TextObject SecurityText = GameTexts.FindText("str_security", null);

		// Token: 0x040008CB RID: 2251
		private readonly TextObject LoyaltyText = GameTexts.FindText("str_loyalty", null);

		// Token: 0x040008CC RID: 2252
		private readonly TextObject LoyaltyDriftText = GameTexts.FindText("str_loyalty_drift", null);

		// Token: 0x040008CD RID: 2253
		private readonly TextObject CorruptionText = GameTexts.FindText("str_corruption", null);

		// Token: 0x040008CE RID: 2254
		private readonly TextObject GovernorText = GameTexts.FindText("str_notable_governor", null);
	}
}
