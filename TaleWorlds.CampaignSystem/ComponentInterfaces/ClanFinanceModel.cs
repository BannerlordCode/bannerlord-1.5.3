using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DE RID: 478
	public abstract class ClanFinanceModel : MBGameModel<ClanFinanceModel>
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001F26 RID: 7974
		public abstract int PartyGoldLowerThreshold { get; }

		// Token: 0x06001F27 RID: 7975
		public abstract ExplainedNumber CalculateClanGoldChange(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001F28 RID: 7976
		public abstract ExplainedNumber CalculateClanIncome(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001F29 RID: 7977
		public abstract ExplainedNumber CalculateClanExpenses(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001F2A RID: 7978
		public abstract ExplainedNumber CalculateTownIncomeFromTariffs(Clan clan, Town town, bool applyWithdrawals = false);

		// Token: 0x06001F2B RID: 7979
		public abstract int CalculateTownIncomeFromProjects(Town town);

		// Token: 0x06001F2C RID: 7980
		public abstract int CalculateNotableDailyGoldChange(Hero hero, bool applyWithdrawals);

		// Token: 0x06001F2D RID: 7981
		public abstract int CalculateVillageIncome(Clan clan, Village village, bool applyWithdrawals = false);

		// Token: 0x06001F2E RID: 7982
		public abstract int CalculateOwnerIncomeFromCaravan(MobileParty caravan);

		// Token: 0x06001F2F RID: 7983
		public abstract int CalculateOwnerIncomeFromWorkshop(Workshop workshop);

		// Token: 0x06001F30 RID: 7984
		public abstract float RevenueSmoothenFraction();
	}
}
