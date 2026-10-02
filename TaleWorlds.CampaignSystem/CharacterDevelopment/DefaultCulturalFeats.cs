using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C0 RID: 960
	public class DefaultCulturalFeats
	{
		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x000E0D99 File Offset: 0x000DEF99
		private static DefaultCulturalFeats Instance
		{
			get
			{
				return Campaign.Current.DefaultFeats;
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000E0DA5 File Offset: 0x000DEFA5
		public DefaultCulturalFeats()
		{
			this.RegisterAll();
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000E0DB4 File Offset: 0x000DEFB4
		private void RegisterAll()
		{
			this._aseraiTraderFeat = this.Create("aserai_cheaper_caravans");
			this._aseraiDesertSpeedFeat = this.Create("aserai_desert_speed");
			this._aseraiWageFeat = this.Create("aserai_increased_wages");
			this._battaniaForestSpeedFeat = this.Create("battanian_forest_speed");
			this._battaniaMilitiaFeat = this.Create("battanian_militia_production");
			this._battaniaConstructionFeat = this.Create("battanian_slower_construction");
			this._empireGarrisonWageFeat = this.Create("empire_decreased_garrison_wage");
			this._empireArmyInfluenceFeat = this.Create("empire_army_influence");
			this._empireVillageHearthFeat = this.Create("empire_slower_hearth_production");
			this._khuzaitCheaperRecruitsFeat = this.Create("khuzait_cheaper_recruits_mounted");
			this._khuzaitAnimalProductionFeat = this.Create("khuzait_increased_animal_production");
			this._khuzaitDecreasedTaxFeat = this.Create("khuzait_decreased_town_tax");
			this._sturgianGrainProductionFeat = this.Create("sturgian_increased_grain_production");
			this._sturgianArmyInfluenceCostFeat = this.Create("sturgian_decreased_army_influence_cost");
			this._sturgianDecisionPenaltyFeat = this.Create("sturgian_increased_decision_penalty");
			this._vlandianRenownIncomeFeat = this.Create("vlandian_renown_mercenary_income");
			this._vlandianVillageProductionFeat = this.Create("vlandian_villages_production_bonus");
			this._vlandianArmyInfluenceCostFeat = this.Create("vlandian_increased_army_influence_cost");
			this.InitializeAll();
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000E0EF9 File Offset: 0x000DF0F9
		private FeatObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<FeatObject>(new FeatObject(stringId));
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x000E0F10 File Offset: 0x000DF110
		private void InitializeAll()
		{
			this._aseraiTraderFeat.Initialize("{=!}aserai_cheaper_caravans", "{=7kGGgkro}Caravans are 30% cheaper to build. 10% less trade penalty.", 0.7f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiDesertSpeedFeat.Initialize("{=!}aserai_desert_speed", "{=6aFTN1Nb}No speed penalty on desert.", 1f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiWageFeat.Initialize("{=!}aserai_increased_wages", "{=GacrZ1Jl}Daily wages of troops in the party are increased by 5%.", 0.05f, false, FeatObject.AdditionType.AddFactor);
			this._battaniaForestSpeedFeat.Initialize("{=!}battanian_forest_speed", "{=38W2WloI}50% less speed penalty and 15% sight range bonus in forests.", 0.5f, true, FeatObject.AdditionType.AddFactor);
			this._battaniaMilitiaFeat.Initialize("{=!}battanian_militia_production", "{=HLI5zAMV}Towns owned by Battanian rulers will have +20% chance of militias to spawn as veteran militias.", 0.2f, true, FeatObject.AdditionType.Add);
			this._battaniaConstructionFeat.Initialize("{=!}battanian_slower_construction", "{=ruP9jbSq}10% slower build rate for town projects in settlements.", -0.1f, false, FeatObject.AdditionType.AddFactor);
			this._empireGarrisonWageFeat.Initialize("{=!}empire_decreased_garrison_wage", "{=a2eM0QUb}20% less garrison troop wage.", -0.2f, true, FeatObject.AdditionType.AddFactor);
			this._empireArmyInfluenceFeat.Initialize("{=!}empire_army_influence", "{=xgPNGOa8}Being in army brings 25% more influence.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._empireVillageHearthFeat.Initialize("{=!}empire_slower_hearth_production", "{=UWiqIFUb}Village hearths increase 20% less.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._khuzaitCheaperRecruitsFeat.Initialize("{=!}khuzait_cheaper_recruits_mounted", "{=JUpZuals}Recruiting and upgrading mounted troops are 10% cheaper.", -0.1f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitAnimalProductionFeat.Initialize("{=!}khuzait_increased_animal_production", "{=Xaw2CoCG}25% production bonus to horse, mule, cow and sheep in villages owned by Khuzait rulers.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitDecreasedTaxFeat.Initialize("{=!}khuzait_decreased_town_tax", "{=8PsaGhI8}20% less tax income from towns.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._sturgianGrainProductionFeat.Initialize("{=!}sturgian_increased_grain_production", "{=5BabRyaa}Villages grain production is increased by 10%.", 0.1f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianArmyInfluenceCostFeat.Initialize("{=!}sturgian_decreased_army_influence_cost", "{=Lmjm5Q9D}Armies are gathered with 50% less influence.", -0.5f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianDecisionPenaltyFeat.Initialize("{=!}sturgian_increased_decision_penalty", "{=fB7kS9Cx}20% more relationship penalty from kingdom decisions.", 0.2f, false, FeatObject.AdditionType.AddFactor);
			this._vlandianRenownIncomeFeat.Initialize("{=!}vlandian_renown_mercenary_income", "{=ppdrgOL8}5% more renown from the battles, 15% more income while serving as a mercenary.", 0.05f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianVillageProductionFeat.Initialize("{=!}vlandian_villages_production_bonus", "{=3GsZXXOi}10% production bonus to villages that are bound to castles.", 0.1f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianArmyInfluenceCostFeat.Initialize("{=!}vlandian_increased_army_influence_cost", "{=O1XCNeZr}Recruiting lords to armies costs 20% more influence.", 0.2f, false, FeatObject.AdditionType.AddFactor);
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x060037E7 RID: 14311 RVA: 0x000E1115 File Offset: 0x000DF315
		public static FeatObject AseraiTraderFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiTraderFeat;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x060037E8 RID: 14312 RVA: 0x000E1121 File Offset: 0x000DF321
		public static FeatObject AseraiDesertFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiDesertSpeedFeat;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000E112D File Offset: 0x000DF32D
		public static FeatObject AseraiIncreasedWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiWageFeat;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x000E1139 File Offset: 0x000DF339
		public static FeatObject BattanianForestSpeedFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaForestSpeedFeat;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x060037EB RID: 14315 RVA: 0x000E1145 File Offset: 0x000DF345
		public static FeatObject BattanianMilitiaFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaMilitiaFeat;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x060037EC RID: 14316 RVA: 0x000E1151 File Offset: 0x000DF351
		public static FeatObject BattanianConstructionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaConstructionFeat;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x060037ED RID: 14317 RVA: 0x000E115D File Offset: 0x000DF35D
		public static FeatObject EmpireGarrisonWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireGarrisonWageFeat;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x000E1169 File Offset: 0x000DF369
		public static FeatObject EmpireArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireArmyInfluenceFeat;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x000E1175 File Offset: 0x000DF375
		public static FeatObject EmpireVillageHearthFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireVillageHearthFeat;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000E1181 File Offset: 0x000DF381
		public static FeatObject KhuzaitRecruitUpgradeFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitCheaperRecruitsFeat;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x000E118D File Offset: 0x000DF38D
		public static FeatObject KhuzaitAnimalProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitAnimalProductionFeat;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x000E1199 File Offset: 0x000DF399
		public static FeatObject KhuzaitDecreasedTaxFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitDecreasedTaxFeat;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060037F3 RID: 14323 RVA: 0x000E11A5 File Offset: 0x000DF3A5
		public static FeatObject SturgianGrainProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianGrainProductionFeat;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x000E11B1 File Offset: 0x000DF3B1
		public static FeatObject SturgianArmyInfluenceCostFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianArmyInfluenceCostFeat;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000E11BD File Offset: 0x000DF3BD
		public static FeatObject SturgianDecisionPenaltyFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianDecisionPenaltyFeat;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x000E11C9 File Offset: 0x000DF3C9
		public static FeatObject VlandianRenownMercenaryFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianRenownIncomeFeat;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x060037F7 RID: 14327 RVA: 0x000E11D5 File Offset: 0x000DF3D5
		public static FeatObject VlandianCastleVillageProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianVillageProductionFeat;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x000E11E1 File Offset: 0x000DF3E1
		public static FeatObject VlandianArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianArmyInfluenceCostFeat;
			}
		}

		// Token: 0x04000FAA RID: 4010
		private FeatObject _aseraiTraderFeat;

		// Token: 0x04000FAB RID: 4011
		private FeatObject _aseraiDesertSpeedFeat;

		// Token: 0x04000FAC RID: 4012
		private FeatObject _aseraiWageFeat;

		// Token: 0x04000FAD RID: 4013
		private FeatObject _battaniaForestSpeedFeat;

		// Token: 0x04000FAE RID: 4014
		private FeatObject _battaniaMilitiaFeat;

		// Token: 0x04000FAF RID: 4015
		private FeatObject _battaniaConstructionFeat;

		// Token: 0x04000FB0 RID: 4016
		private FeatObject _empireGarrisonWageFeat;

		// Token: 0x04000FB1 RID: 4017
		private FeatObject _empireArmyInfluenceFeat;

		// Token: 0x04000FB2 RID: 4018
		private FeatObject _empireVillageHearthFeat;

		// Token: 0x04000FB3 RID: 4019
		private FeatObject _khuzaitCheaperRecruitsFeat;

		// Token: 0x04000FB4 RID: 4020
		private FeatObject _khuzaitAnimalProductionFeat;

		// Token: 0x04000FB5 RID: 4021
		private FeatObject _khuzaitDecreasedTaxFeat;

		// Token: 0x04000FB6 RID: 4022
		private FeatObject _sturgianGrainProductionFeat;

		// Token: 0x04000FB7 RID: 4023
		private FeatObject _sturgianArmyInfluenceCostFeat;

		// Token: 0x04000FB8 RID: 4024
		private FeatObject _sturgianDecisionPenaltyFeat;

		// Token: 0x04000FB9 RID: 4025
		private FeatObject _vlandianRenownIncomeFeat;

		// Token: 0x04000FBA RID: 4026
		private FeatObject _vlandianVillageProductionFeat;

		// Token: 0x04000FBB RID: 4027
		private FeatObject _vlandianArmyInfluenceCostFeat;
	}
}
