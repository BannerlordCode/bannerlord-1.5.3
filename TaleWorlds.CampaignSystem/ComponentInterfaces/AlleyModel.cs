using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000200 RID: 512
	public abstract class AlleyModel : MBGameModel<AlleyModel>
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002003 RID: 8195
		public abstract CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath { get; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002004 RID: 8196
		public abstract int MinimumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002005 RID: 8197
		public abstract int MaximumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002006 RID: 8198
		public abstract float GetDailyCrimeRatingOfAlley { get; }

		// Token: 0x06002007 RID: 8199
		public abstract float GetDailyXpGainForAssignedClanMember(Hero assignedHero);

		// Token: 0x06002008 RID: 8200
		public abstract float GetDailyXpGainForMainHero();

		// Token: 0x06002009 RID: 8201
		public abstract float GetInitialXpGainForMainHero();

		// Token: 0x0600200A RID: 8202
		public abstract float GetXpGainAfterSuccessfulAlleyDefenseForMainHero();

		// Token: 0x0600200B RID: 8203
		public abstract TroopRoster GetTroopsOfAIOwnedAlley(Alley alley);

		// Token: 0x0600200C RID: 8204
		public abstract TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley);

		// Token: 0x0600200D RID: 8205
		public abstract int GetDailyIncomeOfAlley(Alley alley);

		// Token: 0x0600200E RID: 8206
		public abstract List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley);

		// Token: 0x0600200F RID: 8207
		public abstract TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random);

		// Token: 0x06002010 RID: 8208
		public abstract TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail);

		// Token: 0x06002011 RID: 8209
		public abstract float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster);
	}
}
