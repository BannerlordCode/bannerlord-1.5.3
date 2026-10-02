using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012D RID: 301
	public class DefaultLocationModel : LocationModel
	{
		// Token: 0x060018FC RID: 6396 RVA: 0x000797AB File Offset: 0x000779AB
		public override int GetSettlementUpgradeLevel(LocationEncounter locationEncounter)
		{
			return locationEncounter.Settlement.Town.GetWallLevel();
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x000797C0 File Offset: 0x000779C0
		public override string GetCivilianSceneLevel(Settlement settlement)
		{
			string text = "civilian";
			if (settlement.IsFortification)
			{
				string upgradeLevelTag = this.GetUpgradeLevelTag(settlement.Town.GetWallLevel());
				if (!upgradeLevelTag.IsEmpty<char>())
				{
					text = text + " " + upgradeLevelTag;
				}
			}
			return text;
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00079804 File Offset: 0x00077A04
		public override string GetCivilianUpgradeLevelTag(int upgradeLevel)
		{
			if (upgradeLevel == 0)
			{
				return "";
			}
			string text = "civilian";
			string upgradeLevelTag = this.GetUpgradeLevelTag(upgradeLevel);
			if (!upgradeLevelTag.IsEmpty<char>())
			{
				text = text + " " + upgradeLevelTag;
			}
			return text;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0007983E File Offset: 0x00077A3E
		public override string GetUpgradeLevelTag(int upgradeLevel)
		{
			switch (upgradeLevel)
			{
			case 1:
				return "level_1";
			case 2:
				return "level_2";
			case 3:
				return "level_3";
			default:
				return "";
			}
		}
	}
}
