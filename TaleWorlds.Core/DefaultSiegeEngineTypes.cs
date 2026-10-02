using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000055 RID: 85
	public class DefaultSiegeEngineTypes
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00017C2C File Offset: 0x00015E2C
		private static DefaultSiegeEngineTypes Instance
		{
			get
			{
				return Game.Current.DefaultSiegeEngineTypes;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00017C38 File Offset: 0x00015E38
		public static SiegeEngineType Preparations
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypePreparations;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00017C44 File Offset: 0x00015E44
		public static SiegeEngineType Ladder
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeLadder;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00017C50 File Offset: 0x00015E50
		public static SiegeEngineType Ballista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBallista;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00017C5C File Offset: 0x00015E5C
		public static SiegeEngineType FireBallista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireBallista;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00017C68 File Offset: 0x00015E68
		public static SiegeEngineType Ram
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeRam;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00017C74 File Offset: 0x00015E74
		public static SiegeEngineType ImprovedRam
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeImprovedRam;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00017C80 File Offset: 0x00015E80
		public static SiegeEngineType SiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeSiegeTower;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00017C8C File Offset: 0x00015E8C
		public static SiegeEngineType HeavySiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeHeavySiegeTower;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00017C98 File Offset: 0x00015E98
		public static SiegeEngineType Catapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeCatapult;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00017CA4 File Offset: 0x00015EA4
		public static SiegeEngineType FireCatapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireCatapult;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00017CB0 File Offset: 0x00015EB0
		public static SiegeEngineType Onager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeOnager;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00017CBC File Offset: 0x00015EBC
		public static SiegeEngineType FireOnager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireOnager;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00017CC8 File Offset: 0x00015EC8
		public static SiegeEngineType Bricole
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBricole;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00017CD4 File Offset: 0x00015ED4
		public static SiegeEngineType Trebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00017CE0 File Offset: 0x00015EE0
		public static SiegeEngineType FireTrebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00017CEC File Offset: 0x00015EEC
		public DefaultSiegeEngineTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00017CFC File Offset: 0x00015EFC
		private void RegisterAll()
		{
			Game.Current.ObjectManager.LoadXML("SiegeEngines", false);
			this._siegeEngineTypePreparations = this.GetSiegeEngine("preparations");
			this._siegeEngineTypeLadder = this.GetSiegeEngine("ladder");
			this._siegeEngineTypeSiegeTower = this.GetSiegeEngine("siege_tower_level1");
			this._siegeEngineTypeHeavySiegeTower = this.GetSiegeEngine("siege_tower_level2");
			this._siegeEngineTypeBallista = this.GetSiegeEngine("ballista");
			this._siegeEngineTypeFireBallista = this.GetSiegeEngine("fire_ballista");
			this._siegeEngineTypeCatapult = this.GetSiegeEngine("catapult");
			this._siegeEngineTypeFireCatapult = this.GetSiegeEngine("fire_catapult");
			this._siegeEngineTypeOnager = this.GetSiegeEngine("onager");
			this._siegeEngineTypeFireOnager = this.GetSiegeEngine("fire_onager");
			this._siegeEngineTypeBricole = this.GetSiegeEngine("bricole");
			this._siegeEngineTypeTrebuchet = this.GetSiegeEngine("trebuchet");
			this._siegeEngineTypeFireTrebuchet = this.GetSiegeEngine("fire_trebuchet");
			this._siegeEngineTypeRam = this.GetSiegeEngine("ram");
			this._siegeEngineTypeImprovedRam = this.GetSiegeEngine("improved_ram");
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00017E1D File Offset: 0x0001601D
		private SiegeEngineType GetSiegeEngine(string id)
		{
			return Game.Current.ObjectManager.GetObject<SiegeEngineType>(id);
		}

		// Token: 0x04000359 RID: 857
		private SiegeEngineType _siegeEngineTypePreparations;

		// Token: 0x0400035A RID: 858
		private SiegeEngineType _siegeEngineTypeLadder;

		// Token: 0x0400035B RID: 859
		private SiegeEngineType _siegeEngineTypeBallista;

		// Token: 0x0400035C RID: 860
		private SiegeEngineType _siegeEngineTypeFireBallista;

		// Token: 0x0400035D RID: 861
		private SiegeEngineType _siegeEngineTypeRam;

		// Token: 0x0400035E RID: 862
		private SiegeEngineType _siegeEngineTypeImprovedRam;

		// Token: 0x0400035F RID: 863
		private SiegeEngineType _siegeEngineTypeSiegeTower;

		// Token: 0x04000360 RID: 864
		private SiegeEngineType _siegeEngineTypeHeavySiegeTower;

		// Token: 0x04000361 RID: 865
		private SiegeEngineType _siegeEngineTypeCatapult;

		// Token: 0x04000362 RID: 866
		private SiegeEngineType _siegeEngineTypeFireCatapult;

		// Token: 0x04000363 RID: 867
		private SiegeEngineType _siegeEngineTypeOnager;

		// Token: 0x04000364 RID: 868
		private SiegeEngineType _siegeEngineTypeFireOnager;

		// Token: 0x04000365 RID: 869
		private SiegeEngineType _siegeEngineTypeBricole;

		// Token: 0x04000366 RID: 870
		private SiegeEngineType _siegeEngineTypeTrebuchet;

		// Token: 0x04000367 RID: 871
		private SiegeEngineType _siegeEngineTypeFireTrebuchet;
	}
}
