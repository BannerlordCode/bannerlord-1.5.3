using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FB RID: 251
	public class DefaultBattleCaptainModel : BattleCaptainModel
	{
		// Token: 0x060016F6 RID: 5878 RVA: 0x0006AB08 File Offset: 0x00068D08
		public override float GetCaptainRatingForTroopUsages(Hero hero, TroopUsageFlags flag, BattleEnvironment battleEnvironment, out List<PerkObject> compatiblePerks)
		{
			float num = 0f;
			compatiblePerks = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkHelper.GetCaptainPerksForTroopUsages(flag, battleEnvironment))
			{
				if (hero.GetPerkValue(perkObject))
				{
					num += perkObject.RequiredSkillValue;
					compatiblePerks.Add(perkObject);
				}
			}
			num /= 1650f;
			return num;
		}
	}
}
