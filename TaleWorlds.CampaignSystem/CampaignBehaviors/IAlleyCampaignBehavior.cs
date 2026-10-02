using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000419 RID: 1049
	public interface IAlleyCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x06004307 RID: 17159
		bool GetIsPlayerAlleyUnderAttack(Alley alley);

		// Token: 0x06004308 RID: 17160
		int GetPlayerOwnedAlleyTroopCount(Alley alley);

		// Token: 0x06004309 RID: 17161
		int GetResponseTimeLeftForAttackInDays(Alley alley);

		// Token: 0x0600430A RID: 17162
		void AbandonAlleyFromClanMenu(Alley alley);

		// Token: 0x0600430B RID: 17163
		Hero GetAssignedClanMemberOfAlley(Alley alley);

		// Token: 0x0600430C RID: 17164
		bool IsHeroAlleyLeaderOfAnyPlayerAlley(Hero hero);

		// Token: 0x0600430D RID: 17165
		List<Hero> GetAllAssignedClanMembersForOwnedAlleys();

		// Token: 0x0600430E RID: 17166
		void ChangeAlleyMember(Alley alley, Hero newAlleyLead);

		// Token: 0x0600430F RID: 17167
		void OnPlayerRetreatedFromMission();

		// Token: 0x06004310 RID: 17168
		void OnPlayerDiedInMission();
	}
}
