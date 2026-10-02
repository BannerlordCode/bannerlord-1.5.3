using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F5 RID: 501
	public abstract class PrisonerRecruitmentCalculationModel : MBGameModel<PrisonerRecruitmentCalculationModel>
	{
		// Token: 0x06001FC2 RID: 8130
		public abstract int GetConformityNeededToRecruitPrisoner(CharacterObject character);

		// Token: 0x06001FC3 RID: 8131
		public abstract ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject character);

		// Token: 0x06001FC4 RID: 8132
		public abstract float GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num);

		// Token: 0x06001FC5 RID: 8133
		public abstract bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded);

		// Token: 0x06001FC6 RID: 8134
		public abstract bool ShouldPartyRecruitPrisoners(PartyBase party);

		// Token: 0x06001FC7 RID: 8135
		public abstract int CalculateRecruitableNumber(PartyBase party, CharacterObject character);
	}
}
