using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000141 RID: 321
	public class DefaultPartyTradeModel : PartyTradeModel
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x000828F9 File Offset: 0x00080AF9
		public override int CaravanTransactionHighestValueItemCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000828FC File Offset: 0x00080AFC
		public override float GetTradePenaltyFactor(MobileParty party)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.TradePenaltyReduction, party, ref explainedNumber);
			return 1f / explainedNumber.ResultNumber;
		}
	}
}
