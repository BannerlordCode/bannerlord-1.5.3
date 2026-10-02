using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000170 RID: 368
	public class DefaultWallHitPointCalculationModel : WallHitPointCalculationModel
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x0008FFE1 File Offset: 0x0008E1E1
		public override float CalculateMaximumWallHitPoint(Town town)
		{
			if (town == null)
			{
				return 0f;
			}
			return this.CalculateMaximumWallHitPointInternal(town);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0008FFF4 File Offset: 0x0008E1F4
		private float CalculateMaximumWallHitPointInternal(Town town)
		{
			float num = 0f;
			int wallLevel = town.GetWallLevel();
			if (wallLevel == 1)
			{
				num += 30000f;
			}
			else if (wallLevel == 2)
			{
				num += 50000f;
			}
			else if (wallLevel == 3)
			{
				num += 67000f;
			}
			else
			{
				Debug.FailedAssert("Settlement \"" + town.Name + "\" has a wrong wall level set.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultWallHitPointCalculationModel.cs", "CalculateMaximumWallHitPointInternal", 37);
				num += -1f;
			}
			Hero governor = town.Governor;
			if (governor != null && governor.GetPerkValue(DefaultPerks.Engineering.EngineeringGuilds))
			{
				float num2 = num * DefaultPerks.Engineering.EngineeringGuilds.SecondaryBonus;
				num += num2;
			}
			return num;
		}
	}
}
