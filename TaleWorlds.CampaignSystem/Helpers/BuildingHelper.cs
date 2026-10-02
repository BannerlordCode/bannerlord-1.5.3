using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x02000020 RID: 32
	public static class BuildingHelper
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		public static void CheckIfBuildingIsComplete(Building building)
		{
			if ((float)building.GetConstructionCost() <= building.BuildingProgress)
			{
				if (building.CurrentLevel < 3)
				{
					building.LevelUp();
				}
				if (building.CurrentLevel == 3)
				{
					building.BuildingProgress = (float)building.GetConstructionCost();
				}
				building.Town.BuildingsInProgress.Dequeue();
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000E028 File Offset: 0x0000C228
		public static void ChangeDefaultBuilding(Building newDefault, Town town)
		{
			foreach (Building building in town.Buildings)
			{
				if (building.IsCurrentlyDefault)
				{
					building.IsCurrentlyDefault = false;
				}
				if (building == newDefault)
				{
					building.IsCurrentlyDefault = true;
				}
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000E090 File Offset: 0x0000C290
		public static void ChangeCurrentBuildingQueue(List<Building> buildings, Town town)
		{
			town.BuildingsInProgress.Clear();
			foreach (Building building in buildings)
			{
				if (!building.BuildingType.IsDailyProject)
				{
					town.BuildingsInProgress.Enqueue(building);
				}
				else
				{
					Debug.FailedAssert("DefaultProject in building queue", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "ChangeCurrentBuildingQueue", 7822);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000E118 File Offset: 0x0000C318
		public static float GetProgressOfBuilding(Building building, Town town)
		{
			using (List<Building>.Enumerator enumerator = town.Buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == building)
					{
						return building.BuildingProgress / (float)building.GetConstructionCost();
					}
				}
			}
			Debug.FailedAssert(building.Name + "is not a project of" + town.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetProgressOfBuilding", 7837);
			return 0f;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
		public static int GetDaysToComplete(Building building, Town town)
		{
			BuildingConstructionModel buildingConstructionModel = Campaign.Current.Models.BuildingConstructionModel;
			using (List<Building>.Enumerator enumerator = town.Buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == building)
					{
						float num = (float)building.GetConstructionCost() - building.BuildingProgress;
						int num2 = (int)town.Construction;
						if (num2 != 0)
						{
							int num3 = (int)(num / (float)num2);
							int boostCost = buildingConstructionModel.GetBoostCost(town);
							if (town.BoostBuildingProcess >= boostCost)
							{
								int num4 = town.BoostBuildingProcess / boostCost;
								if (num3 > num4)
								{
									int num5 = num4 * num2;
									int num6 = Campaign.Current.Models.BuildingConstructionModel.CalculateDailyConstructionPowerWithoutBoost(town);
									return num4 + MathF.Max((int)((num - (float)num5) / (float)num6), 1);
								}
							}
							return MathF.Max(num3, 1);
						}
						return -1;
					}
				}
			}
			Debug.FailedAssert(building.Name + "is not a project of" + town.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetDaysToComplete", 7879);
			return 0;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		public static int GetTierOfBuilding(BuildingType buildingType, Town town)
		{
			foreach (Building building in town.Buildings)
			{
				if (building.BuildingType == buildingType)
				{
					return building.CurrentLevel;
				}
			}
			Debug.FailedAssert(buildingType.Name + "is not a project of" + town.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetTierOfBuilding", 7893);
			return 0;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000E354 File Offset: 0x0000C554
		public static void BoostBuildingProcessWithGold(int gold, Town town)
		{
			if (gold < town.BoostBuildingProcess)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, town.BoostBuildingProcess - gold, false);
			}
			else if (gold > town.BoostBuildingProcess)
			{
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, gold - town.BoostBuildingProcess, false);
			}
			town.BoostBuildingProcess = gold;
		}
	}
}
