using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C2 RID: 962
	public class DefaultPersonalityTraitEffects
	{
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003800 RID: 14336 RVA: 0x000E86D5 File Offset: 0x000E68D5
		private static DefaultPersonalityTraitEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultPersonalityTraitEffects;
			}
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000E86E1 File Offset: 0x000E68E1
		public DefaultPersonalityTraitEffects()
		{
			this.RegisterAll();
			this.InitializeAll();
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000E86F8 File Offset: 0x000E68F8
		private void RegisterAll()
		{
			this._calculatingWorkshop = DefaultPersonalityTraitEffects.Create("calculating_workshop");
			this._calculatingSiegePrep = DefaultPersonalityTraitEffects.Create("calculating_siege_prep");
			this._calculatingNotableRelationEffect = DefaultPersonalityTraitEffects.Create("calculating_notable_relation");
			this._calculatingChargeDamage = DefaultPersonalityTraitEffects.Create("calculating_charge_damage");
			this._calculatingCombatMorale = DefaultPersonalityTraitEffects.Create("calculating_combat_morale");
			this._calculatingLongerDisorganizeEffect = DefaultPersonalityTraitEffects.Create("longer_disorganize");
			this._generosityMercenaryRecruitment = DefaultPersonalityTraitEffects.Create("generosity_mercenary_recruitment");
			this._generosityMoraleGain = DefaultPersonalityTraitEffects.Create("generosity_morale_gain");
			this._generosityFoodCost = DefaultPersonalityTraitEffects.Create("generosity_food_cost");
			this._generosityTownProject = DefaultPersonalityTraitEffects.Create("generosity_town_project");
			this._generosityUpkeepReduction = DefaultPersonalityTraitEffects.Create("generosity_upkeep_reduction");
			this._generosityFlatMorale = DefaultPersonalityTraitEffects.Create("generosity_flat_morale");
			this._honorRelationGain = DefaultPersonalityTraitEffects.Create("honor_relation_gain");
			this._honorLoyaltyGain = DefaultPersonalityTraitEffects.Create("honor_loyalty_gain");
			this._honorCrimeDecaySlow = DefaultPersonalityTraitEffects.Create("honor_crime_decay_slow");
			this._honorCrimeIncreaseSlow = DefaultPersonalityTraitEffects.Create("honor_crime_increase_slow");
			this._honorRecruitPenaltyReduction = DefaultPersonalityTraitEffects.Create("honor_recruit_penalty_reduction");
			this._honorClanLeaderRelation = DefaultPersonalityTraitEffects.Create("honor_clan_leader_relation");
			this._mercyPrisonerCaptureMerciful = DefaultPersonalityTraitEffects.Create("mercy_prisoner_capture_merciful");
			this._mercyPrisonerCaptureCruel = DefaultPersonalityTraitEffects.Create("mercy_prisoner_capture_cruel");
			this._mercyHearthGrowth = DefaultPersonalityTraitEffects.Create("mercy_hearth_growth");
			this._mercyLordRansom = DefaultPersonalityTraitEffects.Create("mercy_lord_ransom");
			this._mercyTroopRansom = DefaultPersonalityTraitEffects.Create("mercy_troop_ransom");
			this._mercyRaidLoot = DefaultPersonalityTraitEffects.Create("mercy_raid_loot");
			this._mercyRebellionChance = DefaultPersonalityTraitEffects.Create("mercy_rebellion_chance");
			this._valorLossMoraleResist = DefaultPersonalityTraitEffects.Create("valor_loss_morale_resist");
			this._valorBattleRenown = DefaultPersonalityTraitEffects.Create("valor_battle_renown");
			this._valorInjuryRecovery = DefaultPersonalityTraitEffects.Create("valor_injury_recovery");
			this._valorPrisonerEscape = DefaultPersonalityTraitEffects.Create("valor_prisoner_escape");
			this._valorSiegeCasualty = DefaultPersonalityTraitEffects.Create("valor_siege_casualty");
			this._valorSiegePrep = DefaultPersonalityTraitEffects.Create("valor_siege_prep");
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000E88F5 File Offset: 0x000E6AF5
		private static TraitEffectObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<TraitEffectObject>(new TraitEffectObject(stringId));
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000E890C File Offset: 0x000E6B0C
		private void InitializeAll()
		{
			this._calculatingWorkshop.Initialize("{=haDJ8Paw}{VALUE}% workshop income when governing a town.", DefaultTraits.Calculating, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._calculatingSiegePrep.Initialize("{=2142Jj2H}{VALUE}% siege engine construction progress when leading a siege.", DefaultTraits.Calculating, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._calculatingNotableRelationEffect.Initialize("{=3fSxlWXU}{VALUE} relation, when meeting a town notable for the first time.", DefaultTraits.Calculating, new float[] { 0f, 0f, 0f, -3f, -6f }, false, EffectIncrementType.Add);
			TraitEffectObject calculatingChargeDamage = this._calculatingChargeDamage;
			string text = "{=Pd62h7RS}{VALUE}% damage for troops that have been given a charge order while leading a party or army.";
			TraitObject calculating = DefaultTraits.Calculating;
			float[] array = new float[5];
			array[0] = 0.1f;
			array[1] = 0.05f;
			calculatingChargeDamage.Initialize(text, calculating, array, true, EffectIncrementType.AddFactor);
			TraitEffectObject calculatingCombatMorale = this._calculatingCombatMorale;
			string text2 = "{=TL0iokvY}{VALUE}% combat starting morale while leading a party or army.";
			TraitObject calculating2 = DefaultTraits.Calculating;
			float[] array2 = new float[5];
			array2[0] = 0.2f;
			array2[1] = 0.1f;
			calculatingCombatMorale.Initialize(text2, calculating2, array2, true, EffectIncrementType.AddFactor);
			TraitEffectObject calculatingLongerDisorganizeEffect = this._calculatingLongerDisorganizeEffect;
			string text3 = "{=IX2vGfRk}{VALUE}% longer disorganized state.";
			TraitObject calculating3 = DefaultTraits.Calculating;
			float[] array3 = new float[5];
			array3[0] = 1f;
			array3[1] = 0.5f;
			calculatingLongerDisorganizeEffect.Initialize(text3, calculating3, array3, false, EffectIncrementType.AddFactor);
			this._generosityMercenaryRecruitment.Initialize("{=bOoId7Q4}More mercenaries join during recruitment.", DefaultTraits.Generosity, new float[] { 0f, 0f, 0f, 0.25f, 0.5f }, true, EffectIncrementType.AddFactor);
			this._generosityMoraleGain.Initialize("{=Fa5Fr1ja}{VALUE}% morale gain while leading a party.", DefaultTraits.Generosity, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._generosityFoodCost.Initialize("{=OazFrWxX}{VALUE}% food consumption while leading a party or governing a settlement.", DefaultTraits.Generosity, new float[] { 0f, 0f, 0f, 0.1f, 0.1f }, false, EffectIncrementType.AddFactor);
			TraitEffectObject generosityTownProject = this._generosityTownProject;
			string text4 = "{=IMggsnvm}{VALUE}% effectiveness of the settlement project reserve while governing a settlement.";
			TraitObject generosity = DefaultTraits.Generosity;
			float[] array4 = new float[5];
			array4[0] = 0.2f;
			array4[1] = 0.1f;
			generosityTownProject.Initialize(text4, generosity, array4, true, EffectIncrementType.AddFactor);
			TraitEffectObject generosityUpkeepReduction = this._generosityUpkeepReduction;
			string text5 = "{=vlVRCsMz}{VALUE}% troop wage while leading a party or governing a settlement.";
			TraitObject generosity2 = DefaultTraits.Generosity;
			float[] array5 = new float[5];
			array5[0] = -0.1f;
			array5[1] = -0.05f;
			generosityUpkeepReduction.Initialize(text5, generosity2, array5, true, EffectIncrementType.AddFactor);
			TraitEffectObject generosityFlatMorale = this._generosityFlatMorale;
			string text6 = "{=mJP8l22X}{VALUE} daily morale while leading a party.";
			TraitObject generosity3 = DefaultTraits.Generosity;
			float[] array6 = new float[5];
			array6[0] = -1f;
			array6[1] = -1f;
			generosityFlatMorale.Initialize(text6, generosity3, array6, false, EffectIncrementType.Add);
			this._honorRelationGain.Initialize("{=7fgbNLOh}{VALUE}% relationship gain rate.", DefaultTraits.Honor, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._honorLoyaltyGain.Initialize("{=QRxTjbME}{VALUE}% loyalty gain while governing a settlement.", DefaultTraits.Honor, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._honorCrimeDecaySlow.Initialize("{=wRdK7ZuF}{VALUE}% crime rate decay.", DefaultTraits.Honor, new float[] { 0f, 0f, 0f, -0.4f, -0.8f }, false, EffectIncrementType.AddFactor);
			TraitEffectObject honorCrimeIncreaseSlow = this._honorCrimeIncreaseSlow;
			string text7 = "{=ke5bwVnN}{VALUE}% crime rate gain.";
			TraitObject honor = DefaultTraits.Honor;
			float[] array7 = new float[5];
			array7[0] = -0.8f;
			array7[1] = -0.4f;
			honorCrimeIncreaseSlow.Initialize(text7, honor, array7, true, EffectIncrementType.AddFactor);
			TraitEffectObject honorRecruitPenaltyReduction = this._honorRecruitPenaltyReduction;
			string text8 = "{=CxrrPIit}{VALUE}% morale penalty when recruiting bandits and prisoners.";
			TraitObject honor2 = DefaultTraits.Honor;
			float[] array8 = new float[5];
			array8[0] = -0.25f;
			array8[1] = -0.15f;
			honorRecruitPenaltyReduction.Initialize(text8, honor2, array8, true, EffectIncrementType.AddFactor);
			TraitEffectObject honorClanLeaderRelation = this._honorClanLeaderRelation;
			string text9 = "{=cAWZWxwv}{VALUE}% relationship gain with clan leaders.";
			TraitObject honor3 = DefaultTraits.Honor;
			float[] array9 = new float[5];
			array9[0] = -0.1f;
			array9[1] = -0.05f;
			honorClanLeaderRelation.Initialize(text9, honor3, array9, false, EffectIncrementType.AddFactor);
			this._mercyPrisonerCaptureMerciful.Initialize("{=rU1e1aMt}{VALUE}% prisoner capture chance while leading a party.", DefaultTraits.Mercy, new float[] { 0f, 0f, 0f, 0.25f, 0.5f }, true, EffectIncrementType.AddFactor);
			this._mercyHearthGrowth.Initialize("{=Tkakcp7P}{VALUE}% hearth growth rate while governing a settlement.", DefaultTraits.Mercy, new float[] { 0f, 0f, 0f, 0.05f, 0.1f }, true, EffectIncrementType.AddFactor);
			this._mercyLordRansom.Initialize("{=H0RztUZo}{VALUE}% lord ransom income.", DefaultTraits.Mercy, new float[] { 0f, 0f, 0f, 0f, -0.2f }, false, EffectIncrementType.AddFactor);
			TraitEffectObject mercyTroopRansom = this._mercyTroopRansom;
			string text10 = "{=KAq7Ucf4}{VALUE}% troop ransom income.";
			TraitObject mercy = DefaultTraits.Mercy;
			float[] array10 = new float[5];
			array10[3] = -0.2f;
			mercyTroopRansom.Initialize(text10, mercy, array10, false, EffectIncrementType.AddFactor);
			TraitEffectObject mercyRaidLoot = this._mercyRaidLoot;
			string text11 = "{=QSNzULSk}{VALUE}% loot from raids while leading a party or army.";
			TraitObject mercy2 = DefaultTraits.Mercy;
			float[] array11 = new float[5];
			array11[0] = 0.4f;
			array11[1] = 0.2f;
			mercyRaidLoot.Initialize(text11, mercy2, array11, true, EffectIncrementType.AddFactor);
			TraitEffectObject mercyRebellionChance = this._mercyRebellionChance;
			string text12 = "{=da7VQ6lb}{VALUE}% rebellion chance while governing a settlement.";
			TraitObject mercy3 = DefaultTraits.Mercy;
			float[] array12 = new float[5];
			array12[0] = -0.4f;
			array12[1] = -0.2f;
			mercyRebellionChance.Initialize(text12, mercy3, array12, true, EffectIncrementType.AddFactor);
			TraitEffectObject mercyPrisonerCaptureCruel = this._mercyPrisonerCaptureCruel;
			string text13 = "{=rU1e1aMt}{VALUE}% prisoner capture chance while leading a party.";
			TraitObject mercy4 = DefaultTraits.Mercy;
			float[] array13 = new float[5];
			array13[0] = -0.5f;
			array13[1] = -0.25f;
			mercyPrisonerCaptureCruel.Initialize(text13, mercy4, array13, false, EffectIncrementType.AddFactor);
			this._valorLossMoraleResist.Initialize("{=ZIo1Bach}{VALUE}% morale loss from combat casualties as a party or army leader.", DefaultTraits.Valor, new float[] { 0f, 0f, 0f, -0.1f, -0.2f }, true, EffectIncrementType.AddFactor);
			this._valorBattleRenown.Initialize("{=M9gdyeIG}{VALUE}% battle renown gain while leading a party.", DefaultTraits.Valor, new float[] { 0f, 0f, 0f, 0.1f, 0.2f }, true, EffectIncrementType.AddFactor);
			this._valorInjuryRecovery.Initialize("{=yPPwZ3H1}{VALUE}% personal injury recovery rate.", DefaultTraits.Valor, new float[] { 0f, 0f, 0f, -0.05f, -0.1f }, false, EffectIncrementType.AddFactor);
			TraitEffectObject valorPrisonerEscape = this._valorPrisonerEscape;
			string text14 = "{=aMPNdnb1}{VALUE}% prisoner escape chance while leading a party.";
			TraitObject valor = DefaultTraits.Valor;
			float[] array14 = new float[5];
			array14[0] = -0.5f;
			array14[1] = -0.25f;
			valorPrisonerEscape.Initialize(text14, valor, array14, true, EffectIncrementType.AddFactor);
			TraitEffectObject valorSiegeCasualty = this._valorSiegeCasualty;
			string text15 = "{=kc2zU4wx}{VALUE}% damage to troops in the party or army during siege bombardment and simulation.";
			TraitObject valor2 = DefaultTraits.Valor;
			float[] array15 = new float[5];
			array15[0] = -0.2f;
			array15[1] = -0.1f;
			valorSiegeCasualty.Initialize(text15, valor2, array15, true, EffectIncrementType.AddFactor);
			TraitEffectObject valorSiegePrep = this._valorSiegePrep;
			string text16 = "{=UaYAhb42}{ABS(VALUE)}% siege preparation speed.";
			TraitObject valor3 = DefaultTraits.Valor;
			float[] array16 = new float[5];
			array16[0] = -0.3f;
			array16[1] = -0.15f;
			valorSiegePrep.Initialize(text16, valor3, array16, false, EffectIncrementType.AddFactor);
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x000E8E7C File Offset: 0x000E707C
		public static TraitEffectObject CalculatingWorkshopEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingWorkshop;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003806 RID: 14342 RVA: 0x000E8E88 File Offset: 0x000E7088
		public static TraitEffectObject CalculatingSiegePrepEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingSiegePrep;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x000E8E94 File Offset: 0x000E7094
		public static TraitEffectObject CalculatingNotableRelationEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingNotableRelationEffect;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x000E8EA0 File Offset: 0x000E70A0
		public static TraitEffectObject CalculatingChargeDamageEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingChargeDamage;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x000E8EAC File Offset: 0x000E70AC
		public static TraitEffectObject CalculatingCombatMoraleEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingCombatMorale;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x0600380A RID: 14346 RVA: 0x000E8EB8 File Offset: 0x000E70B8
		public static TraitEffectObject CalculatingLongerDisorganizeEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._calculatingLongerDisorganizeEffect;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x000E8EC4 File Offset: 0x000E70C4
		public static TraitEffectObject GenerosityMercenaryRecruitmentEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityMercenaryRecruitment;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x0600380C RID: 14348 RVA: 0x000E8ED0 File Offset: 0x000E70D0
		public static TraitEffectObject GenerosityMoraleGainEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityMoraleGain;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x000E8EDC File Offset: 0x000E70DC
		public static TraitEffectObject GenerosityFoodCostEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityFoodCost;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x0600380E RID: 14350 RVA: 0x000E8EE8 File Offset: 0x000E70E8
		public static TraitEffectObject GenerosityTownProjectEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityTownProject;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x0600380F RID: 14351 RVA: 0x000E8EF4 File Offset: 0x000E70F4
		public static TraitEffectObject GenerosityUpkeepReductionEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityUpkeepReduction;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x000E8F00 File Offset: 0x000E7100
		public static TraitEffectObject GenerosityFlatMoraleEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._generosityFlatMorale;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x000E8F0C File Offset: 0x000E710C
		public static TraitEffectObject HonorRelationGainEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorRelationGain;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003812 RID: 14354 RVA: 0x000E8F18 File Offset: 0x000E7118
		public static TraitEffectObject HonorLoyaltyGainEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorLoyaltyGain;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06003813 RID: 14355 RVA: 0x000E8F24 File Offset: 0x000E7124
		public static TraitEffectObject HonorCrimeDecaySlowEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorCrimeDecaySlow;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06003814 RID: 14356 RVA: 0x000E8F30 File Offset: 0x000E7130
		public static TraitEffectObject HonorCrimeIncreaseSlowEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorCrimeIncreaseSlow;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06003815 RID: 14357 RVA: 0x000E8F3C File Offset: 0x000E713C
		public static TraitEffectObject HonorRecruitPenaltyReductionEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorRecruitPenaltyReduction;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06003816 RID: 14358 RVA: 0x000E8F48 File Offset: 0x000E7148
		public static TraitEffectObject HonorClanLeaderRelationEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._honorClanLeaderRelation;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06003817 RID: 14359 RVA: 0x000E8F54 File Offset: 0x000E7154
		public static TraitEffectObject MercyPrisonerCaptureMercifulEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyPrisonerCaptureMerciful;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x000E8F60 File Offset: 0x000E7160
		public static TraitEffectObject MercyPrisonerCaptureCruelEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyPrisonerCaptureCruel;
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x000E8F6C File Offset: 0x000E716C
		public static TraitEffectObject MercyHearthGrowthEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyHearthGrowth;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x0600381A RID: 14362 RVA: 0x000E8F78 File Offset: 0x000E7178
		public static TraitEffectObject MercyLordRansomEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyLordRansom;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x000E8F84 File Offset: 0x000E7184
		public static TraitEffectObject MercyTroopRansomEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyTroopRansom;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x000E8F90 File Offset: 0x000E7190
		public static TraitEffectObject MercyRaidLootEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyRaidLoot;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x0600381D RID: 14365 RVA: 0x000E8F9C File Offset: 0x000E719C
		public static TraitEffectObject MercyRebellionChanceEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._mercyRebellionChance;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x000E8FA8 File Offset: 0x000E71A8
		public static TraitEffectObject ValorLossMoraleResistEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorLossMoraleResist;
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x0600381F RID: 14367 RVA: 0x000E8FB4 File Offset: 0x000E71B4
		public static TraitEffectObject ValorBattleRenownEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorBattleRenown;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003820 RID: 14368 RVA: 0x000E8FC0 File Offset: 0x000E71C0
		public static TraitEffectObject ValorInjuryRecoveryEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorInjuryRecovery;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x000E8FCC File Offset: 0x000E71CC
		public static TraitEffectObject ValorPrisonerEscapeEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorPrisonerEscape;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003822 RID: 14370 RVA: 0x000E8FD8 File Offset: 0x000E71D8
		public static TraitEffectObject ValorSiegeCasualtyEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorSiegeCasualty;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x000E8FE4 File Offset: 0x000E71E4
		public static TraitEffectObject ValorSiegePrepEffect
		{
			get
			{
				return DefaultPersonalityTraitEffects.Instance._valorSiegePrep;
			}
		}

		// Token: 0x04001133 RID: 4403
		private TraitEffectObject _calculatingWorkshop;

		// Token: 0x04001134 RID: 4404
		private TraitEffectObject _calculatingSiegePrep;

		// Token: 0x04001135 RID: 4405
		private TraitEffectObject _calculatingNotableRelationEffect;

		// Token: 0x04001136 RID: 4406
		private TraitEffectObject _calculatingChargeDamage;

		// Token: 0x04001137 RID: 4407
		private TraitEffectObject _calculatingCombatMorale;

		// Token: 0x04001138 RID: 4408
		private TraitEffectObject _calculatingLongerDisorganizeEffect;

		// Token: 0x04001139 RID: 4409
		private TraitEffectObject _generosityMercenaryRecruitment;

		// Token: 0x0400113A RID: 4410
		private TraitEffectObject _generosityMoraleGain;

		// Token: 0x0400113B RID: 4411
		private TraitEffectObject _generosityFoodCost;

		// Token: 0x0400113C RID: 4412
		private TraitEffectObject _generosityTownProject;

		// Token: 0x0400113D RID: 4413
		private TraitEffectObject _generosityUpkeepReduction;

		// Token: 0x0400113E RID: 4414
		private TraitEffectObject _generosityFlatMorale;

		// Token: 0x0400113F RID: 4415
		private TraitEffectObject _honorRelationGain;

		// Token: 0x04001140 RID: 4416
		private TraitEffectObject _honorLoyaltyGain;

		// Token: 0x04001141 RID: 4417
		private TraitEffectObject _honorCrimeDecaySlow;

		// Token: 0x04001142 RID: 4418
		private TraitEffectObject _honorCrimeIncreaseSlow;

		// Token: 0x04001143 RID: 4419
		private TraitEffectObject _honorRecruitPenaltyReduction;

		// Token: 0x04001144 RID: 4420
		private TraitEffectObject _honorClanLeaderRelation;

		// Token: 0x04001145 RID: 4421
		private TraitEffectObject _mercyPrisonerCaptureMerciful;

		// Token: 0x04001146 RID: 4422
		private TraitEffectObject _mercyPrisonerCaptureCruel;

		// Token: 0x04001147 RID: 4423
		private TraitEffectObject _mercyHearthGrowth;

		// Token: 0x04001148 RID: 4424
		private TraitEffectObject _mercyLordRansom;

		// Token: 0x04001149 RID: 4425
		private TraitEffectObject _mercyTroopRansom;

		// Token: 0x0400114A RID: 4426
		private TraitEffectObject _mercyRaidLoot;

		// Token: 0x0400114B RID: 4427
		private TraitEffectObject _mercyRebellionChance;

		// Token: 0x0400114C RID: 4428
		private TraitEffectObject _valorLossMoraleResist;

		// Token: 0x0400114D RID: 4429
		private TraitEffectObject _valorBattleRenown;

		// Token: 0x0400114E RID: 4430
		private TraitEffectObject _valorInjuryRecovery;

		// Token: 0x0400114F RID: 4431
		private TraitEffectObject _valorPrisonerEscape;

		// Token: 0x04001150 RID: 4432
		private TraitEffectObject _valorSiegeCasualty;

		// Token: 0x04001151 RID: 4433
		private TraitEffectObject _valorSiegePrep;
	}
}
