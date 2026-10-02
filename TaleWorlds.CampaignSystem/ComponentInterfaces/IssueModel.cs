using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F4 RID: 500
	public abstract class IssueModel : MBGameModel<IssueModel>
	{
		// Token: 0x06001FB6 RID: 8118
		public abstract float GetIssueDifficultyMultiplier();

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001FB7 RID: 8119
		public abstract int IssueOwnerCoolDownInDays { get; }

		// Token: 0x06001FB8 RID: 8120
		public abstract void GetIssueEffectsOfSettlement(IssueEffect issueEffect, Settlement settlement, ref ExplainedNumber explainedNumber);

		// Token: 0x06001FB9 RID: 8121
		public abstract void GetIssueEffectOfHero(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber);

		// Token: 0x06001FBA RID: 8122
		public abstract void GetIssueEffectOfClan(IssueEffect issueEffect, Clan clan, ref ExplainedNumber explainedNumber);

		// Token: 0x06001FBB RID: 8123
		public abstract ValueTuple<int, int> GetCausalityForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001FBC RID: 8124
		public abstract float GetFailureRiskForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001FBD RID: 8125
		public abstract CampaignTime GetDurationOfResolutionForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001FBE RID: 8126
		public abstract int GetTroopsRequiredForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001FBF RID: 8127
		public abstract bool CanTroopsReturnFromAlternativeSolution();

		// Token: 0x06001FC0 RID: 8128
		public abstract ValueTuple<SkillObject, int> GetIssueAlternativeSolutionSkill(Hero hero, IssueBase issue);
	}
}
