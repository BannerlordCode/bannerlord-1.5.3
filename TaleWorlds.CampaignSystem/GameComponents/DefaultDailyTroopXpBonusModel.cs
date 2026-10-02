using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000113 RID: 275
	public class DefaultDailyTroopXpBonusModel : DailyTroopXpBonusModel
	{
		// Token: 0x060017F7 RID: 6135 RVA: 0x00071732 File Offset: 0x0006F932
		public override int CalculateDailyTroopXpBonus(Town town)
		{
			return this.CalculateTroopXpBonusInternal(town);
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0007173C File Offset: 0x0006F93C
		private int CalculateTroopXpBonusInternal(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			town.AddEffectOfBuildings(BuildingEffectEnum.ExperiencePerDay, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.RaiseTheMeek, town, false, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.ProjectileDeflection, town, false, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00071787 File Offset: 0x0006F987
		public override float CalculateGarrisonXpBonusMultiplier(Town town)
		{
			return 1f;
		}
	}
}
