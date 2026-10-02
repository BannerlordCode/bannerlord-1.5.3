using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x0200000F RID: 15
	public static class FeatHelper
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00007C5C File Offset: 0x00005E5C
		public static void ApplyCultureFeat(CultureObject culture, FeatObject feat, ref ExplainedNumber result)
		{
			if (!culture.HasFeat(feat))
			{
				return;
			}
			if (feat.IncrementType == FeatObject.AdditionType.Add)
			{
				result.Add(feat.EffectBonus, GameTexts.FindText("str_culture", null), null);
				return;
			}
			if (feat.IncrementType == FeatObject.AdditionType.AddFactor)
			{
				result.AddFactor(feat.EffectBonus, GameTexts.FindText("str_culture", null));
				return;
			}
			Debug.FailedAssert("feat.IncrementType is out of range!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "ApplyCultureFeat", 3477);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public static void ApplyCultureFeat(PartyBase party, FeatObject feat, ref ExplainedNumber result)
		{
			if (!PartyBaseHelper.HasFeat(party, feat))
			{
				return;
			}
			CultureObject cultureObject = null;
			if (party.LeaderHero != null)
			{
				cultureObject = party.LeaderHero.Culture;
			}
			else if (party.Culture != null)
			{
				cultureObject = party.Culture;
			}
			else if (party.Owner != null)
			{
				cultureObject = party.Owner.Culture;
			}
			else if (party.Settlement != null)
			{
				cultureObject = party.Settlement.Culture;
			}
			FeatHelper.ApplyCultureFeat(cultureObject, feat, ref result);
		}
	}
}
