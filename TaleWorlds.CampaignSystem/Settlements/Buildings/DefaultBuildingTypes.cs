using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements.Buildings
{
	// Token: 0x020003E7 RID: 999
	public class DefaultBuildingTypes
	{
		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06003C20 RID: 15392 RVA: 0x000F3DAD File Offset: 0x000F1FAD
		private static DefaultBuildingTypes Instance
		{
			get
			{
				return Campaign.Current.DefaultBuildingTypes;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06003C21 RID: 15393 RVA: 0x000F3DB9 File Offset: 0x000F1FB9
		public static BuildingType SettlementFortifications
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementFortifications;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x000F3DC5 File Offset: 0x000F1FC5
		public static BuildingType SettlementBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementBarracks;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06003C23 RID: 15395 RVA: 0x000F3DD1 File Offset: 0x000F1FD1
		public static BuildingType SettlementTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementTrainingFields;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x000F3DDD File Offset: 0x000F1FDD
		public static BuildingType SettlementGuardHouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementGuardHouse;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06003C25 RID: 15397 RVA: 0x000F3DE9 File Offset: 0x000F1FE9
		public static BuildingType SettlementTaxOffice
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementTaxOffice;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06003C26 RID: 15398 RVA: 0x000F3DF5 File Offset: 0x000F1FF5
		public static BuildingType SettlementWarehouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementWarehouse;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x000F3E01 File Offset: 0x000F2001
		public static BuildingType SettlementMason
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMason;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x000F3E0D File Offset: 0x000F200D
		public static BuildingType SettlementSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementSiegeWorkshop;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x000F3E19 File Offset: 0x000F2019
		public static BuildingType SettlementWaterworks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementWaterworks;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x000F3E25 File Offset: 0x000F2025
		public static BuildingType SettlementCourthouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementCourthouse;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x000F3E31 File Offset: 0x000F2031
		public static BuildingType SettlementMarketplace
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMarketplace;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x000F3E3D File Offset: 0x000F203D
		public static BuildingType SettlementRoadsAndPaths
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementRoadsAndPaths;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x000F3E49 File Offset: 0x000F2049
		public static BuildingType CastleFortifications
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleFortifications;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x000F3E55 File Offset: 0x000F2055
		public static BuildingType CastleBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleBarracks;
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x000F3E61 File Offset: 0x000F2061
		public static BuildingType CastleTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleTrainingFields;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x000F3E6D File Offset: 0x000F206D
		public static BuildingType CastleGuardHouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGuardHouse;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x000F3E79 File Offset: 0x000F2079
		public static BuildingType CastleCastallansOffice
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleCastallansOffice;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x000F3E85 File Offset: 0x000F2085
		public static BuildingType CastleSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleSiegeWorkshop;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06003C33 RID: 15411 RVA: 0x000F3E91 File Offset: 0x000F2091
		public static BuildingType CastleCraftmansQuarters
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleCraftmansQuarters;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x000F3E9D File Offset: 0x000F209D
		public static BuildingType CastleFarmlands
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleFarmlands;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x000F3EA9 File Offset: 0x000F20A9
		public static BuildingType CastleGranary
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGranary;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x000F3EB5 File Offset: 0x000F20B5
		public static BuildingType CastleMason
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleMason;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06003C37 RID: 15415 RVA: 0x000F3EC1 File Offset: 0x000F20C1
		public static BuildingType CastleRoadsAndPaths
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleRoadsAndPaths;
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x000F3ECD File Offset: 0x000F20CD
		public static BuildingType SettlementDailyHousing
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyHousing;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06003C39 RID: 15417 RVA: 0x000F3ED9 File Offset: 0x000F20D9
		public static BuildingType SettlementDailyTrainMilitia
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyTrainMilitia;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x000F3EE5 File Offset: 0x000F20E5
		public static BuildingType SettlementDailyFestivalAndGames
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyFestivalAndGames;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x000F3EF1 File Offset: 0x000F20F1
		public static BuildingType SettlementDailyIrrigation
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyIrrigation;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x000F3EFD File Offset: 0x000F20FD
		public static BuildingType CastleDailySlackenGarrison
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailySlackenGarrison;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000F3F09 File Offset: 0x000F2109
		public static BuildingType CastleDailyRaiseTroops
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyRaiseTroops;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x000F3F15 File Offset: 0x000F2115
		public static BuildingType CastleDailyDrills
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyDrills;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000F3F21 File Offset: 0x000F2121
		public static BuildingType CastleDailyIrrigation
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyIrrigation;
			}
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x000F3F2D File Offset: 0x000F212D
		public DefaultBuildingTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x000F3F3C File Offset: 0x000F213C
		private void RegisterAll()
		{
			this._buildingSettlementFortifications = this.Create("building_settlement_fortifications");
			this._buildingSettlementBarracks = this.Create("building_settlement_barracks");
			this._buildingSettlementTrainingFields = this.Create("building_settlement_training_fields");
			this._buildingSettlementGuardHouse = this.Create("building_settlement_guard_house");
			this._buildingSettlementSiegeWorkshop = this.Create("building_settlement_siege_workshop");
			this._buildingSettlementTaxOffice = this.Create("building_settlement_tax_office");
			this._buildingSettlementMarketplace = this.Create("building_settlement_marketplace");
			this._buildingSettlementWarehouse = this.Create("building_settlement_warehouse");
			this._buildingSettlementMason = this.Create("building_settlement_mason");
			this._buildingSettlementWaterworks = this.Create("building_settlement_waterworks");
			this._buildingSettlementCourthouse = this.Create("building_settlement_courthouse");
			this._buildingSettlementRoadsAndPaths = this.Create("building_settlement_roads_and_paths");
			this._buildingCastleFortifications = this.Create("building_castle_fortifications");
			this._buildingCastleBarracks = this.Create("building_castle_barracks");
			this._buildingCastleTrainingFields = this.Create("building_castle_training_fields");
			this._buildingCastleGuardHouse = this.Create("building_castle_guard_house");
			this._buildingCastleSiegeWorkshop = this.Create("building_castle_siege_workshop");
			this._buildingCastleCastallansOffice = this.Create("building_castle_castallans_office");
			this._buildingCastleGranary = this.Create("building_castle_granary");
			this._buildingCastleCraftmansQuarters = this.Create("building_castle_craftmans_quarters");
			this._buildingCastleFarmlands = this.Create("building_castle_farmlands");
			this._buildingCastleMason = this.Create("building_castle_mason");
			this._buildingCastleRoadsAndPaths = this.Create("building_castle_roads_and_paths");
			this._buildingSettlementDailyHousing = this.Create("building_settlement_daily_housing");
			this._buildingSettlementDailyTrainMilitia = this.Create("building_settlement_daily_train_militia");
			this._buildingSettlementDailyFestivalAndGames = this.Create("building_settlement_daily_festival_and_games");
			this._buildingSettlementDailyIrrigation = this.Create("building_settlement_daily_irrigation");
			this._buildingCastleDailySlackenGarrison = this.Create("building_castle_daily_slacken_garrison");
			this._buildingCastleDailyRaiseTroops = this.Create("building_castle_daily_raise_troops");
			this._buildingCastleDailyDrills = this.Create("building_castle_daily_drills");
			this._buildingCastleDailyIrrigation = this.Create("building_castle_daily_irrigation");
			this.InitializeAll();
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x000F415E File Offset: 0x000F235E
		private BuildingType Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BuildingType>(new BuildingType(stringId));
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x000F4178 File Offset: 0x000F2378
		private void InitializeAll()
		{
			this._buildingSettlementFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=dIM6xa2O}Better fortifications and higher walls around town, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 6000, 12000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 60f, 90f, 120f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 50f, 75f, 100f)
			}, true, 0f, 1);
			this._buildingSettlementBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=JalrbDBC}Lodgings for garrison troops. Each level increases garrison limit and decreases garrison wage.", null), new int[] { 1800, 3000, 4200 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 60f, 90f, 120f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.05f, -0.1f, -0.15f)
			}, true, 0f, 0);
			this._buildingSettlementTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=NYzORuQm}Provides experience for garrison troops and increases militia veterancy.", null), new int[] { 1500, 2100, 2700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.MilitiaVeterancyChance, BuildingEffectIncrementType.Add, 0.1f, 0.15f, 0.2f)
			}, true, 0f, 0);
			this._buildingSettlementGuardHouse.Initialize(new TextObject("{=OHEiwoHC}Guard House", null), new TextObject("{=doojtAwr}Increases prisoner limit and provides a patrol party that improves security.", null), new int[] { 1500, 2100, 2700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PatrolPartyStrength, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 30f, 60f, 90f)
			}, true, 0f, 0);
			this._buildingSettlementSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}Builds and maintains siege engines for defense of the settlement.", null), new int[] { 1200, 1800, 3000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.BallistaOnSiegeStart, BuildingEffectIncrementType.Add, 1f, 1f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CatapultOnSiegeStart, BuildingEffectIncrementType.Add, 0f, 1f, 1f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, BuildingEffectIncrementType.AddFactor, 0.3f, 0.6f, 1f)
			}, false, 0f, 0);
			this._buildingSettlementTaxOffice.Initialize(new TextObject("{=LG84byW0}Tax Office", null), new TextObject("{=nQ6ytZeF}Increases tax income.", null), new int[] { 1800, 3000, 4200 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.TaxPerDay, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f)
			}, false, 0f, 0);
			this._buildingSettlementMarketplace.Initialize(new TextObject("{=zLdXCpne}Marketplace", null), new TextObject("{=Z0xf3Bbd}Increases the tariff collected from trades made in town", null), new int[] { 2400, 3600, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.TariffIncome, BuildingEffectIncrementType.AddFactor, 0.1f, 0.2f, 0.3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CaravanAccessibility, BuildingEffectIncrementType.AddFactor, 1.02f, 1.04f, 1.06f)
			}, false, 0f, 0);
			this._buildingSettlementWarehouse.Initialize(new TextObject("{=anTRftmb}Warehouse", null), new TextObject("{=hhKDZJeM}Increases Food storage limits and improves workshop productivity.", null), new int[] { 1800, 2400, 3000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodStock, BuildingEffectIncrementType.Add, 100f, 300f, 500f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WorkshopProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f)
			}, false, 0f, 0);
			this._buildingSettlementMason.Initialize(new TextObject("{=R7ssoDHW}Mason", null), new TextObject("{=hqUPvnaj}Increase bricks per day, increasing building and repair speed.", null), new int[] { 2400, 3000, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ConstructionPerDay, BuildingEffectIncrementType.Add, 3f, 6f, 9f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WallRepairSpeed, BuildingEffectIncrementType.AddFactor, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingSettlementWaterworks.Initialize(new TextObject("{=DA0y7B3S}Waterworks", null), new TextObject("{=SfbwSASh}Waterways and sanitation, decrease food consumption.", null), new int[] { 1800, 3600, 5400 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodConsumption, BuildingEffectIncrementType.AddFactor, -0.05f, -0.1f, -0.15f)
			}, false, 0f, 0);
			this._buildingSettlementCourthouse.Initialize(new TextObject("{=Bw8kAvGY}Courthouse", null), new TextObject("{=tmLJvPlz}Local judges manage disputes and maintain law and order. Provides influence and loyalty per day.", null), new int[] { 2400, 3600, 5400 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Loyalty, BuildingEffectIncrementType.Add, 0.3f, 0.6f, 1f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Influence, BuildingEffectIncrementType.Add, 0.2f, 0.5f, 1f)
			}, false, 0f, 0);
			this._buildingSettlementRoadsAndPaths.Initialize(new TextObject("{=maEmutDP}Roads and Paths", null), new TextObject("{=YPFDiwuy}Increase village production and village hearth growth.", null), new int[] { 2400, 3600, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingCastleFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=oS5Nesmi}Better fortifications and higher walls around the keep, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 1400, 2800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 50f, 75f, 100f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 30f, 45f, 60f)
			}, true, 0f, 1);
			this._buildingCastleBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=JalrbDBC}Lodgings for garrison troops. Each level increases garrison limit and decreases garrison wage.", null), new int[] { 420, 700, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 20f, 40f, 80f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.1f, -0.2f, -0.3f)
			}, true, 0f, 0);
			this._buildingCastleTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[] { 420, 560, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 3f, 4f, 5f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.MilitiaVeterancyChance, BuildingEffectIncrementType.Add, 0.1f, 0.15f, 0.2f)
			}, true, 0f, 0);
			this._buildingCastleGuardHouse.Initialize(new TextObject("{=OHEiwoHC}Guard House", null), new TextObject("{=K0cbj7o3}Increase militia recruitment, and prisoner limit.", null), new int[] { 350, 490, 630 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 10f, 30f, 50f)
			}, true, 0f, 0);
			this._buildingCastleSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=YRCW0oFd}Builds and maintains siege engines for defense of the settlement.", null), new int[] { 280, 420, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.BallistaOnSiegeStart, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CatapultOnSiegeStart, BuildingEffectIncrementType.Add, 0f, 1f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, BuildingEffectIncrementType.AddFactor, 0.2f, 0.4f, 0.8f)
			}, true, 0f, 0);
			this._buildingCastleCastallansOffice.Initialize(new TextObject("{=kLNnFMR9}Castellan's Office", null), new TextObject("{=GDsI6daq}Increases auto recruitment, and decreases garrison wage.", null), new int[] { 560, 840, 1260 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.1f, -0.2f, -0.3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 1f, 2f, 3f)
			}, true, 0f, 0);
			this._buildingCastleGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=iazij7fO}Increases food storage limits.", null), new int[] { 420, 560, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodStock, BuildingEffectIncrementType.Add, 100f, 200f, 300f)
			}, false, 0f, 0);
			this._buildingCastleCraftmansQuarters.Initialize(new TextObject("{=KE1KUayw}Craftmans Quarters", null), new TextObject("{=2qZ14G9p}Provides income based on bound village hearts", null), new int[] { 350, 490, 630 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.DenarByBoundVillageHeartPerDay, BuildingEffectIncrementType.Add, 0.2f, 0.4f, 0.6f)
			}, false, 0f, 0);
			this._buildingCastleFarmlands.Initialize(new TextObject("{=l4eZqegY}Farmlands", null), new TextObject("{=tajCl8Bg}Provides daily food.", null), new int[] { 420, 630, 840 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodProduction, BuildingEffectIncrementType.Add, 6f, 12f, 18f)
			}, false, 0f, 0);
			this._buildingCastleMason.Initialize(new TextObject("{=R7ssoDHW}Mason", null), new TextObject("{=hqUPvnaj}Increase bricks per day, increasing building and repair speed.", null), new int[] { 560, 700, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ConstructionPerDay, BuildingEffectIncrementType.Add, 2f, 4f, 6f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WallRepairSpeed, BuildingEffectIncrementType.AddFactor, 0.1f, 0.3f, 0.6f)
			}, false, 0f, 0);
			this._buildingCastleRoadsAndPaths.Initialize(new TextObject("{=maEmutDP}Roads and Paths", null), new TextObject("{=YPFDiwuy}Increase village production and village hearth growth.", null), new int[] { 560, 840, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingSettlementDailyHousing.InitializeDailyProject(new TextObject("{=F4V7oaVx}Housing", null), new TextObject("{=yWXtcxqb}Construct housing so that more folks can settle, increasing population.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Prosperity, BuildingEffectIncrementType.Add, 2f, 2f, 2f)
			});
			this._buildingSettlementDailyTrainMilitia.InitializeDailyProject(new TextObject("{=p1Y3EU5O}Train Militia", null), new TextObject("{=61J1wa6k}Schedule drills for commoners, increasing militia recruitment and auto recruitment.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 2f, 2f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 1f, 1f, 1f)
			});
			this._buildingSettlementDailyFestivalAndGames.InitializeDailyProject(new TextObject("{=aEmYZadz}Festival and Games", null), new TextObject("{=ovDbQIo9}Organize festivals and games in the settlement, increasing loyalty.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Loyalty, BuildingEffectIncrementType.Add, 3f, 3f, 3f)
			});
			this._buildingSettlementDailyIrrigation.InitializeDailyProject(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing hearth growth in bound villages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 1f, 1f, 1f)
			});
			this._buildingCastleDailySlackenGarrison.InitializeDailyProject(new TextObject("{=cHIa0Xty}Slacken Garrison", null), new TextObject("{=5VBbLVBt}Decrease garrison wages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.05f, -0.05f, -0.05f)
			});
			this._buildingCastleDailyRaiseTroops.InitializeDailyProject(new TextObject("{=jm1ScaoK}Raise Troops", null), new TextObject("{=UsHhePdk}Increase militia recruitment, and auto recruitment.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 3f, 3f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 2f, 2f, 2f)
			});
			this._buildingCastleDailyDrills.InitializeDailyProject(new TextObject("{=JpiQagYa}Drills", null), new TextObject("{=e9V1W7nW}Provides experience to garrison.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 8f, 8f, 8f)
			});
			this._buildingCastleDailyIrrigation.InitializeDailyProject(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing hearth growth in bound villages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.AddFactor, 0.5f, 0.5f, 0.5f)
			});
		}

		// Token: 0x0400127E RID: 4734
		public const int MaxBuildingLevel = 3;

		// Token: 0x0400127F RID: 4735
		private BuildingType _buildingSettlementFortifications;

		// Token: 0x04001280 RID: 4736
		private BuildingType _buildingSettlementMarketplace;

		// Token: 0x04001281 RID: 4737
		private BuildingType _buildingSettlementTrainingFields;

		// Token: 0x04001282 RID: 4738
		private BuildingType _buildingSettlementBarracks;

		// Token: 0x04001283 RID: 4739
		private BuildingType _buildingSettlementSiegeWorkshop;

		// Token: 0x04001284 RID: 4740
		private BuildingType _buildingSettlementGuardHouse;

		// Token: 0x04001285 RID: 4741
		private BuildingType _buildingSettlementTaxOffice;

		// Token: 0x04001286 RID: 4742
		private BuildingType _buildingSettlementWarehouse;

		// Token: 0x04001287 RID: 4743
		private BuildingType _buildingSettlementMason;

		// Token: 0x04001288 RID: 4744
		private BuildingType _buildingSettlementCourthouse;

		// Token: 0x04001289 RID: 4745
		private BuildingType _buildingSettlementWaterworks;

		// Token: 0x0400128A RID: 4746
		private BuildingType _buildingSettlementRoadsAndPaths;

		// Token: 0x0400128B RID: 4747
		private BuildingType _buildingCastleFortifications;

		// Token: 0x0400128C RID: 4748
		private BuildingType _buildingCastleBarracks;

		// Token: 0x0400128D RID: 4749
		private BuildingType _buildingCastleTrainingFields;

		// Token: 0x0400128E RID: 4750
		private BuildingType _buildingCastleGranary;

		// Token: 0x0400128F RID: 4751
		private BuildingType _buildingCastleGuardHouse;

		// Token: 0x04001290 RID: 4752
		private BuildingType _buildingCastleCastallansOffice;

		// Token: 0x04001291 RID: 4753
		private BuildingType _buildingCastleSiegeWorkshop;

		// Token: 0x04001292 RID: 4754
		private BuildingType _buildingCastleCraftmansQuarters;

		// Token: 0x04001293 RID: 4755
		private BuildingType _buildingCastleFarmlands;

		// Token: 0x04001294 RID: 4756
		private BuildingType _buildingSettlementDailyHousing;

		// Token: 0x04001295 RID: 4757
		private BuildingType _buildingCastleMason;

		// Token: 0x04001296 RID: 4758
		private BuildingType _buildingCastleRoadsAndPaths;

		// Token: 0x04001297 RID: 4759
		private BuildingType _buildingSettlementDailyIrrigation;

		// Token: 0x04001298 RID: 4760
		private BuildingType _buildingSettlementDailyTrainMilitia;

		// Token: 0x04001299 RID: 4761
		private BuildingType _buildingCastleDailySlackenGarrison;

		// Token: 0x0400129A RID: 4762
		private BuildingType _buildingSettlementDailyFestivalAndGames;

		// Token: 0x0400129B RID: 4763
		private BuildingType _buildingCastleDailyRaiseTroops;

		// Token: 0x0400129C RID: 4764
		private BuildingType _buildingCastleDailyDrills;

		// Token: 0x0400129D RID: 4765
		private BuildingType _buildingCastleDailyIrrigation;
	}
}
