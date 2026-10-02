using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000179 RID: 377
	public static class TextObjectExtensions
	{
		// Token: 0x06001BE0 RID: 7136 RVA: 0x00090521 File Offset: 0x0008E721
		public static void SetCharacterProperties(this TextObject to, string tag, CharacterObject character, bool includeDetails = false)
		{
			StringHelpers.SetCharacterProperties(tag, character, to, includeDetails);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x00090530 File Offset: 0x0008E730
		public static void SetSettlementProperties(this TextObject to, Settlement settlement)
		{
			to.SetTextVariable("IS_SETTLEMENT", 1);
			to.SetTextVariable("IS_CASTLE", settlement.IsCastle ? 1 : 0);
			to.SetTextVariable("IS_TOWN", settlement.IsTown ? 1 : 0);
			to.SetTextVariable("IS_HIDEOUT", settlement.IsHideout ? 1 : 0);
		}
	}
}
