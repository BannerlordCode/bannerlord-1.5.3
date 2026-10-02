using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F1 RID: 1009
	public class BuildingsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003D61 RID: 15713 RVA: 0x000FEC30 File Offset: 0x000FCE30
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnBuildingLevelChangedEvent.AddNonSerializedListener(this, new Action<Town, Building, int>(this.OnBuildingLevelChanged));
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x000FEC99 File Offset: 0x000FCE99
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.Town != null && newOwner.Clan != Clan.PlayerClan)
			{
				settlement.Town.BuildingsInProgress.Clear();
			}
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x000FECC0 File Offset: 0x000FCEC0
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x000FECC2 File Offset: 0x000FCEC2
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			BuildingsCampaignBehavior.BuildDevelopmentsAtGameStart();
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x000FECCC File Offset: 0x000FCECC
		private static void DecideDailyProject(Town town)
		{
			Building nextDailyBuilding = Campaign.Current.Models.BuildingScoreCalculationModel.GetNextDailyBuilding(town);
			if (nextDailyBuilding != null && nextDailyBuilding != town.CurrentDefaultBuilding)
			{
				BuildingHelper.ChangeDefaultBuilding(nextDailyBuilding, town);
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x000FED04 File Offset: 0x000FCF04
		private static void DecideBuildingQueue(Town town)
		{
			if (town.BuildingsInProgress.IsEmpty<Building>())
			{
				Building nextBuilding = Campaign.Current.Models.BuildingScoreCalculationModel.GetNextBuilding(town);
				if (nextBuilding != null)
				{
					town.BuildingsInProgress.Enqueue(nextBuilding);
				}
			}
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x000FED44 File Offset: 0x000FCF44
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				foreach (Building building in town.Buildings)
				{
					if (town.Owner.Settlement.SiegeEvent == null)
					{
						building.HitPointChanged(10f);
					}
				}
				if (town.Owner.Settlement.OwnerClan != Clan.PlayerClan)
				{
					if (MBRandom.RandomFloat < 0.1f)
					{
						BuildingsCampaignBehavior.DecideBuildingQueue(town);
					}
					if (MBRandom.RandomFloat < 0.01f)
					{
						BuildingsCampaignBehavior.DecideDailyProject(town);
					}
				}
				if (!town.CurrentBuilding.BuildingType.IsDailyProject)
				{
					this.TickCurrentBuildingForTown(town);
					return;
				}
				if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Charm.Virile) && MBRandom.RandomFloat <= DefaultPerks.Charm.Virile.SecondaryBonus)
				{
					Hero randomElement = settlement.Notables.GetRandomElement<Hero>();
					if (randomElement != null)
					{
						int num = 1;
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(town.Governor.Clan.Leader, randomElement, num, false);
					}
				}
			}
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x000FEE6C File Offset: 0x000FD06C
		private void TickCurrentBuildingForTown(Town town)
		{
			if (town.BuildingsInProgress.Peek().CurrentLevel == 3)
			{
				town.BuildingsInProgress.Dequeue();
			}
			if (!town.Owner.Settlement.IsUnderSiege && !town.BuildingsInProgress.IsEmpty<Building>())
			{
				BuildingConstructionModel buildingConstructionModel = Campaign.Current.Models.BuildingConstructionModel;
				Building building = town.BuildingsInProgress.Peek();
				building.BuildingProgress += town.Construction;
				int boostCost = buildingConstructionModel.GetBoostCost(town);
				if (town.BoostBuildingProcess > 0)
				{
					town.BoostBuildingProcess -= boostCost;
					if (town.BoostBuildingProcess < 0)
					{
						town.BoostBuildingProcess = 0;
					}
				}
				BuildingHelper.CheckIfBuildingIsComplete(building);
			}
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x000FEF1C File Offset: 0x000FD11C
		private void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			if (building.BuildingType.HasEffect(BuildingEffectEnum.GarrisonCapacity))
			{
				MobileParty garrisonParty = building.Town.Settlement.Town.GarrisonParty;
				if (garrisonParty != null)
				{
					garrisonParty.Party.MemberRoster.UpdateVersion();
				}
			}
			if (building.BuildingType.HasEffect(BuildingEffectEnum.PrisonCapacity))
			{
				building.Town.Settlement.Party.PrisonRoster.UpdateVersion();
			}
			if (levelChange > 0)
			{
				if (town.Governor != null)
				{
					if ((town.IsTown || town.IsCastle) && town.Governor.GetPerkValue(DefaultPerks.Charm.MoralLeader))
					{
						foreach (Hero hero in town.Settlement.Notables)
						{
							int num = MathF.Round(DefaultPerks.Charm.MoralLeader.SecondaryBonus);
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(town.Settlement.OwnerClan.Leader, hero, num, true);
						}
					}
					if (town.Governor.GetPerkValue(DefaultPerks.Engineering.Foreman))
					{
						town.Prosperity += DefaultPerks.Engineering.Foreman.SecondaryBonus;
					}
				}
				SkillLevelingManager.OnSettlementProjectFinished(town.Settlement);
			}
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x000FF060 File Offset: 0x000FD260
		private static void BuildDevelopmentsAtGameStart()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsFortification)
				{
					Town town = settlement.Town;
					using (List<BuildingType>.Enumerator enumerator2 = BuildingType.All.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BuildingType buildingType = enumerator2.Current;
							if (town.Buildings.All<Building>((Building b) => b.BuildingType != buildingType) && Campaign.Current.Models.BuildingModel.CanAddBuildingTypeToTown(buildingType, town))
							{
								town.Buildings.Add(new Building(buildingType, town, 0f, buildingType.StartLevel));
							}
						}
					}
					foreach (Building building in town.Buildings)
					{
						BuildingType buildingType2 = building.BuildingType;
						if (building.CurrentLevel < 3 && settlement.RandomFloat(1f) < buildingType2.VarianceChance)
						{
							Debug.Print(string.Concat(new object[] { "Building variance roll success! SettlementId: ", settlement.StringId, " BuildingId: ", buildingType2.StringId, "Level: ", building.CurrentLevel }), 0, Debug.DebugColor.White, 17592186044416UL);
							building.LevelUp();
							Debug.Print("Building level increased to " + building.CurrentLevel + ".", 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
					BuildingsCampaignBehavior.DecideDailyProject(town);
					BuildingsCampaignBehavior.DecideBuildingQueue(town);
				}
			}
		}
	}
}
