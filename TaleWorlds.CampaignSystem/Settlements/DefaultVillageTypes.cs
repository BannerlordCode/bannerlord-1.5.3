using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x020003D8 RID: 984
	public class DefaultVillageTypes
	{
		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000EFABA File Offset: 0x000EDCBA
		private static DefaultVillageTypes Instance
		{
			get
			{
				return Campaign.Current.DefaultVillageTypes;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000EFAC6 File Offset: 0x000EDCC6
		// (set) Token: 0x06003ADE RID: 15070 RVA: 0x000EFACE File Offset: 0x000EDCCE
		public IList<ItemObject> ConsumableRawItems { get; private set; }

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000EFAD7 File Offset: 0x000EDCD7
		public static VillageType EuropeHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeEuropeHorseRanch;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000EFAE3 File Offset: 0x000EDCE3
		public static VillageType BattanianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeBattanianHorseRanch;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x000EFAEF File Offset: 0x000EDCEF
		public static VillageType SturgianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSturgianHorseRanch;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x000EFAFB File Offset: 0x000EDCFB
		public static VillageType VlandianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeVlandianHorseRanch;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x000EFB07 File Offset: 0x000EDD07
		public static VillageType SteppeHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSteppeHorseRanch;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x000EFB13 File Offset: 0x000EDD13
		public static VillageType DesertHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeDesertHorseRanch;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x000EFB1F File Offset: 0x000EDD1F
		public static VillageType WheatFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeWheatFarm;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x000EFB2B File Offset: 0x000EDD2B
		public static VillageType Lumberjack
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeLumberjack;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x000EFB37 File Offset: 0x000EDD37
		public static VillageType ClayMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeClayMine;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x000EFB43 File Offset: 0x000EDD43
		public static VillageType SaltMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSaltMine;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x000EFB4F File Offset: 0x000EDD4F
		public static VillageType IronMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeIronMine;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000EFB5B File Offset: 0x000EDD5B
		public static VillageType Fisherman
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeFisherman;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x000EFB67 File Offset: 0x000EDD67
		public static VillageType CattleRange
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeCattleRange;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000EFB73 File Offset: 0x000EDD73
		public static VillageType SheepFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSheepFarm;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x000EFB7F File Offset: 0x000EDD7F
		public static VillageType HogFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeHogFarm;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x000EFB8B File Offset: 0x000EDD8B
		public static VillageType VineYard
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeVineYard;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x000EFB97 File Offset: 0x000EDD97
		public static VillageType FlaxPlant
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeFlaxPlant;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x000EFBA3 File Offset: 0x000EDDA3
		public static VillageType DateFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeDateFarm;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x000EFBAF File Offset: 0x000EDDAF
		public static VillageType OliveTrees
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeOliveTrees;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x000EFBBB File Offset: 0x000EDDBB
		public static VillageType SilkPlant
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSilkPlant;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x000EFBC7 File Offset: 0x000EDDC7
		public static VillageType SilverMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSilverMine;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x000EFBD3 File Offset: 0x000EDDD3
		// (set) Token: 0x06003AF5 RID: 15093 RVA: 0x000EFBDB File Offset: 0x000EDDDB
		internal VillageType VillageTypeEuropeHorseRanch { get; private set; }

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x000EFBE4 File Offset: 0x000EDDE4
		// (set) Token: 0x06003AF7 RID: 15095 RVA: 0x000EFBEC File Offset: 0x000EDDEC
		internal VillageType VillageTypeBattanianHorseRanch { get; private set; }

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000EFBF5 File Offset: 0x000EDDF5
		// (set) Token: 0x06003AF9 RID: 15097 RVA: 0x000EFBFD File Offset: 0x000EDDFD
		internal VillageType VillageTypeSturgianHorseRanch { get; private set; }

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003AFA RID: 15098 RVA: 0x000EFC06 File Offset: 0x000EDE06
		// (set) Token: 0x06003AFB RID: 15099 RVA: 0x000EFC0E File Offset: 0x000EDE0E
		internal VillageType VillageTypeVlandianHorseRanch { get; private set; }

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x000EFC17 File Offset: 0x000EDE17
		// (set) Token: 0x06003AFD RID: 15101 RVA: 0x000EFC1F File Offset: 0x000EDE1F
		internal VillageType VillageTypeSteppeHorseRanch { get; private set; }

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003AFE RID: 15102 RVA: 0x000EFC28 File Offset: 0x000EDE28
		// (set) Token: 0x06003AFF RID: 15103 RVA: 0x000EFC30 File Offset: 0x000EDE30
		internal VillageType VillageTypeDesertHorseRanch { get; private set; }

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06003B00 RID: 15104 RVA: 0x000EFC39 File Offset: 0x000EDE39
		// (set) Token: 0x06003B01 RID: 15105 RVA: 0x000EFC41 File Offset: 0x000EDE41
		internal VillageType VillageTypeWheatFarm { get; private set; }

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003B02 RID: 15106 RVA: 0x000EFC4A File Offset: 0x000EDE4A
		// (set) Token: 0x06003B03 RID: 15107 RVA: 0x000EFC52 File Offset: 0x000EDE52
		internal VillageType VillageTypeLumberjack { get; private set; }

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003B04 RID: 15108 RVA: 0x000EFC5B File Offset: 0x000EDE5B
		// (set) Token: 0x06003B05 RID: 15109 RVA: 0x000EFC63 File Offset: 0x000EDE63
		internal VillageType VillageTypeClayMine { get; private set; }

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003B06 RID: 15110 RVA: 0x000EFC6C File Offset: 0x000EDE6C
		// (set) Token: 0x06003B07 RID: 15111 RVA: 0x000EFC74 File Offset: 0x000EDE74
		internal VillageType VillageTypeSaltMine { get; private set; }

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003B08 RID: 15112 RVA: 0x000EFC7D File Offset: 0x000EDE7D
		// (set) Token: 0x06003B09 RID: 15113 RVA: 0x000EFC85 File Offset: 0x000EDE85
		internal VillageType VillageTypeIronMine { get; private set; }

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x000EFC8E File Offset: 0x000EDE8E
		// (set) Token: 0x06003B0B RID: 15115 RVA: 0x000EFC96 File Offset: 0x000EDE96
		internal VillageType VillageTypeFisherman { get; private set; }

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x000EFC9F File Offset: 0x000EDE9F
		// (set) Token: 0x06003B0D RID: 15117 RVA: 0x000EFCA7 File Offset: 0x000EDEA7
		internal VillageType VillageTypeCattleRange { get; private set; }

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003B0E RID: 15118 RVA: 0x000EFCB0 File Offset: 0x000EDEB0
		// (set) Token: 0x06003B0F RID: 15119 RVA: 0x000EFCB8 File Offset: 0x000EDEB8
		internal VillageType VillageTypeSheepFarm { get; private set; }

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x000EFCC1 File Offset: 0x000EDEC1
		// (set) Token: 0x06003B11 RID: 15121 RVA: 0x000EFCC9 File Offset: 0x000EDEC9
		internal VillageType VillageTypeHogFarm { get; private set; }

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x000EFCD2 File Offset: 0x000EDED2
		// (set) Token: 0x06003B13 RID: 15123 RVA: 0x000EFCDA File Offset: 0x000EDEDA
		internal VillageType VillageTypeTrapper { get; private set; }

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x000EFCE3 File Offset: 0x000EDEE3
		// (set) Token: 0x06003B15 RID: 15125 RVA: 0x000EFCEB File Offset: 0x000EDEEB
		internal VillageType VillageTypeVineYard { get; private set; }

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x000EFCF4 File Offset: 0x000EDEF4
		// (set) Token: 0x06003B17 RID: 15127 RVA: 0x000EFCFC File Offset: 0x000EDEFC
		internal VillageType VillageTypeFlaxPlant { get; private set; }

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x000EFD05 File Offset: 0x000EDF05
		// (set) Token: 0x06003B19 RID: 15129 RVA: 0x000EFD0D File Offset: 0x000EDF0D
		internal VillageType VillageTypeDateFarm { get; private set; }

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x000EFD16 File Offset: 0x000EDF16
		// (set) Token: 0x06003B1B RID: 15131 RVA: 0x000EFD1E File Offset: 0x000EDF1E
		internal VillageType VillageTypeOliveTrees { get; private set; }

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x000EFD27 File Offset: 0x000EDF27
		// (set) Token: 0x06003B1D RID: 15133 RVA: 0x000EFD2F File Offset: 0x000EDF2F
		internal VillageType VillageTypeSilkPlant { get; private set; }

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x000EFD38 File Offset: 0x000EDF38
		// (set) Token: 0x06003B1F RID: 15135 RVA: 0x000EFD40 File Offset: 0x000EDF40
		internal VillageType VillageTypeSilverMine { get; private set; }

		// Token: 0x06003B20 RID: 15136 RVA: 0x000EFD49 File Offset: 0x000EDF49
		public DefaultVillageTypes()
		{
			this.ConsumableRawItems = new List<ItemObject>();
			this.RegisterAll();
			this.AddProductions();
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000EFD68 File Offset: 0x000EDF68
		private void RegisterAll()
		{
			this.VillageTypeWheatFarm = this.Create("wheat_farm");
			this.VillageTypeEuropeHorseRanch = this.Create("europe_horse_ranch");
			this.VillageTypeSteppeHorseRanch = this.Create("steppe_horse_ranch");
			this.VillageTypeDesertHorseRanch = this.Create("desert_horse_ranch");
			this.VillageTypeBattanianHorseRanch = this.Create("battanian_horse_ranch");
			this.VillageTypeSturgianHorseRanch = this.Create("sturgian_horse_ranch");
			this.VillageTypeVlandianHorseRanch = this.Create("vlandian_horse_ranch");
			this.VillageTypeLumberjack = this.Create("lumberjack");
			this.VillageTypeClayMine = this.Create("clay_mine");
			this.VillageTypeSaltMine = this.Create("salt_mine");
			this.VillageTypeIronMine = this.Create("iron_mine");
			this.VillageTypeFisherman = this.Create("fisherman");
			this.VillageTypeCattleRange = this.Create("cattle_farm");
			this.VillageTypeSheepFarm = this.Create("sheep_farm");
			this.VillageTypeHogFarm = this.Create("swine_farm");
			this.VillageTypeVineYard = this.Create("vineyard");
			this.VillageTypeFlaxPlant = this.Create("flax_plant");
			this.VillageTypeDateFarm = this.Create("date_farm");
			this.VillageTypeOliveTrees = this.Create("olive_trees");
			this.VillageTypeSilkPlant = this.Create("silk_plant");
			this.VillageTypeSilverMine = this.Create("silver_mine");
			this.VillageTypeTrapper = this.Create("trapper");
			this.InitializeAll();
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x000EFEF1 File Offset: 0x000EE0F1
		private VillageType Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<VillageType>(new VillageType(stringId));
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x000EFF08 File Offset: 0x000EE108
		private void InitializeAll()
		{
			this.VillageTypeWheatFarm.Initialize(new TextObject("{=BPPG2XF7}Wheat Farm", null), "wheat_farm", "wheat_farm_ucon", "wheat_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 50f)
			});
			this.VillageTypeEuropeHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "europe_horse_ranch", "ranch_ucon", "europe_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSteppeHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "steppe_horse_ranch", "ranch_ucon", "steppe_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeDesertHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "desert_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeBattanianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "battanian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSturgianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "sturgian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeVlandianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "vlandian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeLumberjack.Initialize(new TextObject("{=YYl1W2jU}Forester", null), "lumberjack", "lumberjack_ucon", "lumberjack_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeClayMine.Initialize(new TextObject("{=myuzMhOn}Clay Pits", null), "clay_mine", "clay_mine_ucon", "clay_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSaltMine.Initialize(new TextObject("{=3aOIY6wl}Salt Mine", null), "salt_mine", "salt_mine_ucon", "salt_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeIronMine.Initialize(new TextObject("{=rHcVKSbA}Iron Mine", null), "iron_mine", "iron_mine_ucon", "iron_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeFisherman.Initialize(new TextObject("{=XpREJNHD}Fishers", null), "fisherman", "fisherman_ucon", "fisherman_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeCattleRange.Initialize(new TextObject("{=bW3csuSZ}Cattle Farms", null), "cattle_farm", "ranch_ucon", "cattle_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSheepFarm.Initialize(new TextObject("{=QbKbGu2h}Sheep Farms", null), "sheep_farm", "ranch_ucon", "sheep_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeHogFarm.Initialize(new TextObject("{=vqSHB7mJ}Swine Farm", null), "swine_farm", "swine_farm_ucon", "swine_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeVineYard.Initialize(new TextObject("{=ZtxWTS9V}Vineyard", null), "vineyard", "vineyard_ucon", "vineyard_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeFlaxPlant.Initialize(new TextObject("{=Z8ntYx0Y}Flax Field", null), "flax_plant", "flax_plant_ucon", "flax_plant_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeDateFarm.Initialize(new TextObject("{=2NR2E663}Palm Orchard", null), "date_farm", "date_farm_ucon", "date_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeOliveTrees.Initialize(new TextObject("{=ewrkbwI9}Olive Trees", null), "date_farm", "date_farm_ucon", "date_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSilkPlant.Initialize(new TextObject("{=wTyq7LaM}Silkworm Farm", null), "silk_plant", "silk_plant_ucon", "silk_plant_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSilverMine.Initialize(new TextObject("{=aJLQz9iZ}Silver Mine", null), "silver_mine", "silver_mine_ucon", "silver_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeTrapper.Initialize(new TextObject("{=RREyouKr}Trapper", null), "trapper", "trapper_ucon", "trapper_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x000F04C4 File Offset: 0x000EE6C4
		private void AddProductions()
		{
			this.AddProductions(this.VillageTypeWheatFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cow", 0.2f),
				new ValueTuple<string, float>("sheep", 0.4f),
				new ValueTuple<string, float>("hog", 0.8f)
			});
			this.AddProductions(this.VillageTypeEuropeHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("empire_horse", 2.1f),
				new ValueTuple<string, float>("t2_empire_horse", 0.5f),
				new ValueTuple<string, float>("t3_empire_horse", 0.07f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeSturgianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("sturgia_horse", 2.5f),
				new ValueTuple<string, float>("t2_sturgia_horse", 0.7f),
				new ValueTuple<string, float>("t3_sturgia_horse", 0.1f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeVlandianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("vlandia_horse", 2.1f),
				new ValueTuple<string, float>("t2_vlandia_horse", 0.4f),
				new ValueTuple<string, float>("t3_vlandia_horse", 0.08f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeBattanianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("battania_horse", 2.3f),
				new ValueTuple<string, float>("t2_battania_horse", 0.7f),
				new ValueTuple<string, float>("t3_battania_horse", 0.09f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeSteppeHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("khuzait_horse", 1.8f),
				new ValueTuple<string, float>("t2_khuzait_horse", 0.4f),
				new ValueTuple<string, float>("t3_khuzait_horse", 0.05f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f)
			});
			this.AddProductions(this.VillageTypeDesertHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("aserai_horse", 1.7f),
				new ValueTuple<string, float>("t2_aserai_horse", 0.3f),
				new ValueTuple<string, float>("t3_aserai_horse", 0.05f),
				new ValueTuple<string, float>("camel", 0.3f),
				new ValueTuple<string, float>("war_camel", 0.08f),
				new ValueTuple<string, float>("pack_camel", 0.3f),
				new ValueTuple<string, float>("sumpter_horse", 0.4f),
				new ValueTuple<string, float>("mule", 0.5f)
			});
			this.AddProductions(this.VillageTypeCattleRange, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cow", 2f),
				new ValueTuple<string, float>("butter", 4f),
				new ValueTuple<string, float>("cheese", 4f)
			});
			this.AddProductions(this.VillageTypeSheepFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("sheep", 4f),
				new ValueTuple<string, float>("wool", 10f),
				new ValueTuple<string, float>("butter", 2f),
				new ValueTuple<string, float>("cheese", 2f)
			});
			this.AddProductions(this.VillageTypeHogFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("hog", 8f),
				new ValueTuple<string, float>("butter", 2f),
				new ValueTuple<string, float>("cheese", 2f)
			});
			this.AddProductions(this.VillageTypeLumberjack, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("hardwood", 18f)
			});
			this.AddProductions(this.VillageTypeClayMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("clay", 10f)
			});
			this.AddProductions(this.VillageTypeSaltMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("salt", 15f)
			});
			this.AddProductions(this.VillageTypeIronMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("iron", 10f)
			});
			this.AddProductions(this.VillageTypeFisherman, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("fish", 28f)
			});
			this.AddProductions(this.VillageTypeVineYard, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("grape", 11f)
			});
			this.AddProductions(this.VillageTypeFlaxPlant, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("flax", 18f)
			});
			this.AddProductions(this.VillageTypeDateFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("date_fruit", 8f)
			});
			this.AddProductions(this.VillageTypeOliveTrees, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("olives", 12f)
			});
			this.AddProductions(this.VillageTypeSilkPlant, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cotton", 8f)
			});
			this.AddProductions(this.VillageTypeSilverMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("silver", 3f)
			});
			this.AddProductions(this.VillageTypeTrapper, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("fur", 1.4f)
			});
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("grain"));
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("cheese"));
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("butter"));
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000F0D1A File Offset: 0x000EEF1A
		private void AddProductions(VillageType villageType, ValueTuple<string, float>[] productions)
		{
			villageType.AddProductions(productions.Select<ValueTuple<string, float>, ValueTuple<ItemObject, float>>((ValueTuple<string, float> p) => new ValueTuple<ItemObject, float>(Game.Current.ObjectManager.GetObject<ItemObject>(p.Item1), p.Item2)));
		}
	}
}
