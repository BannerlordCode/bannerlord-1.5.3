using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace StoryMode.GameComponents
{
	// Token: 0x02000048 RID: 72
	public class StoryModePartyWageModel : PartyWageModel
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000197C1 File Offset: 0x000179C1
		public override int MaxWagePaymentLimit
		{
			get
			{
				return base.BaseModel.MaxWagePaymentLimit;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000197CE File Offset: 0x000179CE
		public override int GetCharacterWage(CharacterObject character)
		{
			return base.BaseModel.GetCharacterWage(character);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000197DC File Offset: 0x000179DC
		public override ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false)
		{
			return base.BaseModel.GetTotalWage(mobileParty, troopRoster, includeDescriptions);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000197EC File Offset: 0x000179EC
		public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
		{
			if (StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return base.BaseModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
			}
			if (!(troop.StringId == "tutorial_placeholder_volunteer"))
			{
				return base.BaseModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
			}
			return new ExplainedNumber(50f, false, null);
		}

		// Token: 0x04000195 RID: 405
		private const int StoryModeTutorialTroopCost = 50;
	}
}
