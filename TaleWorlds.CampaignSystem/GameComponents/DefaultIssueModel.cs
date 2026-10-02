using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000129 RID: 297
	public class DefaultIssueModel : IssueModel
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00078E71 File Offset: 0x00077071
		public override int IssueOwnerCoolDownInDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00078E75 File Offset: 0x00077075
		public override float GetIssueDifficultyMultiplier()
		{
			return MBMath.ClampFloat(Campaign.Current.PlayerProgress, 0.1f, 1f);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00078E90 File Offset: 0x00077090
		public override void GetIssueEffectsOfSettlement(IssueEffect issueEffect, Settlement settlement, ref ExplainedNumber explainedNumber)
		{
			foreach (Hero hero in settlement.OwnerClan.AliveLords)
			{
				if (hero.Issue != null)
				{
					this.GetIssueEffectOfHeroInternal(issueEffect, hero, ref explainedNumber, DefaultIssueModel.SettlementIssuesText);
				}
			}
			foreach (Hero hero2 in settlement.HeroesWithoutParty)
			{
				if (hero2.Issue != null)
				{
					this.GetIssueEffectOfHeroInternal(issueEffect, hero2, ref explainedNumber, DefaultIssueModel.SettlementIssuesText);
				}
			}
			if (settlement.IsTown || settlement.IsCastle)
			{
				foreach (Village village in settlement.BoundVillages)
				{
					foreach (Hero hero3 in village.Settlement.Notables)
					{
						if (hero3.Issue != null)
						{
							this.GetIssueEffectOfHeroInternal(issueEffect, hero3, ref explainedNumber, DefaultIssueModel.RelatedSettlementIssuesText);
						}
					}
				}
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x00078FEC File Offset: 0x000771EC
		public override void GetIssueEffectOfHero(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber)
		{
			this.GetIssueEffectOfHeroInternal(issueEffect, hero, ref explainedNumber, DefaultIssueModel.HeroIssueText);
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00078FFC File Offset: 0x000771FC
		public override void GetIssueEffectOfClan(IssueEffect issueEffect, Clan clan, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			foreach (Hero hero in clan.AliveLords)
			{
				if (hero.Issue != null)
				{
					IssueBase issue = hero.Issue;
					num += issue.GetActiveIssueEffectAmount(issueEffect);
				}
			}
			explainedNumber.Add(num, DefaultIssueModel.ClanIssuesText, null);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00079074 File Offset: 0x00077274
		public override ValueTuple<int, int> GetCausalityForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 0.8f;
			if (skillValue != 0)
			{
				num = (float)(issueAlternativeSolutionSkill.Item2 / skillValue) * 0.1f;
			}
			num = MBMath.ClampFloat(num, 0.2f, 0.8f);
			int num2 = MathF.Ceiling((float)issue.GetTotalAlternativeSolutionNeededMenCount() * num);
			return new ValueTuple<int, int>(MBMath.ClampInt(2 * (num2 / 3), 1, num2), num2);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x000790E4 File Offset: 0x000772E4
		public override float GetFailureRiskForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			return MBMath.ClampFloat((float)(issueAlternativeSolutionSkill.Item2 - alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1)) * 0.5f / 100f, 0f, 0.9f);
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0007912C File Offset: 0x0007732C
		public override CampaignTime GetDurationOfResolutionForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 10f;
			if (skillValue != 0)
			{
				num = MBMath.ClampFloat((float)(issueAlternativeSolutionSkill.Item2 / skillValue), 0f, 10f);
			}
			return CampaignTime.Days((float)issue.GetBaseAlternativeSolutionDurationInDays() + 2f * num);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00079188 File Offset: 0x00077388
		public override int GetTroopsRequiredForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 1.2f;
			if (skillValue != 0)
			{
				num = (float)issueAlternativeSolutionSkill.Item2 / (float)skillValue;
			}
			num = MBMath.ClampFloat(num, 0.2f, 1.2f);
			return (int)((float)issue.AlternativeSolutionBaseNeededMenCount * num);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x000791DA File Offset: 0x000773DA
		public override ValueTuple<SkillObject, int> GetIssueAlternativeSolutionSkill(Hero hero, IssueBase issue)
		{
			return issue.GetAlternativeSolutionSkill(hero);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x000791E4 File Offset: 0x000773E4
		private void GetIssueEffectOfHeroInternal(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber, TextObject customText)
		{
			float activeIssueEffectAmount = hero.Issue.GetActiveIssueEffectAmount(issueEffect);
			if (!activeIssueEffectAmount.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				explainedNumber.Add(activeIssueEffectAmount, customText, null);
			}
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0007921A File Offset: 0x0007741A
		public override bool CanTroopsReturnFromAlternativeSolution()
		{
			return !Hero.MainHero.IsPrisoner && (!MobileParty.MainParty.IsCurrentlyAtSea || Settlement.CurrentSettlement != null) && MobileParty.MainParty.MapEvent == null;
		}

		// Token: 0x04000827 RID: 2087
		private static readonly TextObject SettlementIssuesText = new TextObject("{=EQLgVYk0}Settlement Issues", null);

		// Token: 0x04000828 RID: 2088
		private static readonly TextObject HeroIssueText = GameTexts.FindText("str_issues", null);

		// Token: 0x04000829 RID: 2089
		private static readonly TextObject RelatedSettlementIssuesText = new TextObject("{=umNyHc3A}Bound Village Issues", null);

		// Token: 0x0400082A RID: 2090
		private static readonly TextObject ClanIssuesText = new TextObject("{=jdl8G8JS}Clan Issues", null);
	}
}
