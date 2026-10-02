using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace StoryMode.GameComponents
{
	// Token: 0x02000049 RID: 73
	public class StoryModePrisonerRecruitmentCalculationModel : PrisonerRecruitmentCalculationModel
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00019853 File Offset: 0x00017A53
		public override int CalculateRecruitableNumber(PartyBase party, CharacterObject character)
		{
			return base.BaseModel.CalculateRecruitableNumber(party, character);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019862 File Offset: 0x00017A62
		public override ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject character)
		{
			if (party == PartyBase.MainParty && !StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return new ExplainedNumber(0f, false, null);
			}
			return base.BaseModel.GetConformityChangePerHour(party, character);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001989C File Offset: 0x00017A9C
		public override int GetConformityNeededToRecruitPrisoner(CharacterObject character)
		{
			return base.BaseModel.GetConformityNeededToRecruitPrisoner(character);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000198AA File Offset: 0x00017AAA
		public override float GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num)
		{
			return base.BaseModel.GetPrisonerRecruitmentMoraleEffect(party, character, num);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000198BA File Offset: 0x00017ABA
		public override bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded)
		{
			return base.BaseModel.IsPrisonerRecruitable(party, character, out conformityNeeded);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000198CA File Offset: 0x00017ACA
		public override bool ShouldPartyRecruitPrisoners(PartyBase party)
		{
			return base.BaseModel.ShouldPartyRecruitPrisoners(party);
		}
	}
}
