using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000086 RID: 134
	public class DefaultSkillEffects
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00053141 File Offset: 0x00051341
		private static DefaultSkillEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultSkillEffects;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x0005314D File Offset: 0x0005134D
		public static SkillEffect OneHandedSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectOneHandedSpeed;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x00053159 File Offset: 0x00051359
		public static SkillEffect OneHandedDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectOneHandedDamage;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00053165 File Offset: 0x00051365
		public static SkillEffect TwoHandedSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTwoHandedSpeed;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x00053171 File Offset: 0x00051371
		public static SkillEffect TwoHandedDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTwoHandedDamage;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x0005317D File Offset: 0x0005137D
		public static SkillEffect PolearmSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectPolearmSpeed;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x00053189 File Offset: 0x00051389
		public static SkillEffect PolearmDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectPolearmDamage;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00053195 File Offset: 0x00051395
		public static SkillEffect BowDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectBowDamage;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x000531A1 File Offset: 0x000513A1
		public static SkillEffect BowAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectBowAccuracy;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x000531AD File Offset: 0x000513AD
		public static SkillEffect ThrowingSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingSpeed;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x000531B9 File Offset: 0x000513B9
		public static SkillEffect ThrowingDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingDamage;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x000531C5 File Offset: 0x000513C5
		public static SkillEffect ThrowingAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingAccuracy;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x000531D1 File Offset: 0x000513D1
		public static SkillEffect CrossbowReloadSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCrossbowReloadSpeed;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x000531DD File Offset: 0x000513DD
		public static SkillEffect CrossbowAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCrossbowAccuracy;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x000531E9 File Offset: 0x000513E9
		public static SkillEffect HorseSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHorseSpeed;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x000531F5 File Offset: 0x000513F5
		public static SkillEffect HorseManeuver
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHorseManeuver;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x00053201 File Offset: 0x00051401
		public static SkillEffect MountedWeaponDamagePenalty
		{
			get
			{
				return DefaultSkillEffects.Instance._effectMountedWeaponDamagePenalty;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x0005320D File Offset: 0x0005140D
		public static SkillEffect MountedWeaponSpeedPenalty
		{
			get
			{
				return DefaultSkillEffects.Instance._effectMountedWeaponSpeedPenalty;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00053219 File Offset: 0x00051419
		public static SkillEffect DismountResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectDismountResistance;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00053225 File Offset: 0x00051425
		public static SkillEffect AthleticsSpeedFactor
		{
			get
			{
				return DefaultSkillEffects.Instance._effectAthleticsSpeedFactor;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00053231 File Offset: 0x00051431
		public static SkillEffect AthleticsWeightFactor
		{
			get
			{
				return DefaultSkillEffects.Instance._effectAthleticsWeightFactor;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x0005323D File Offset: 0x0005143D
		public static SkillEffect KnockBackResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectKnockBackResistance;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00053249 File Offset: 0x00051449
		public static SkillEffect KnockDownResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectKnockDownResistance;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00053255 File Offset: 0x00051455
		public static SkillEffect SmithingLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSmithingLevel;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00053261 File Offset: 0x00051461
		public static SkillEffect TacticsAdvantage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTacticsAdvantage;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x0005326D File Offset: 0x0005146D
		public static SkillEffect TacticsTroopSacrificeReduction
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTacticsTroopSacrificeReduction;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x00053279 File Offset: 0x00051479
		public static SkillEffect TrackingRadius
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingRadius;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x00053285 File Offset: 0x00051485
		public static SkillEffect TrackingSpottingDistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingSpottingDistance;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x00053291 File Offset: 0x00051491
		public static SkillEffect TrackingTrackInformation
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingTrackInformation;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0005329D File Offset: 0x0005149D
		public static SkillEffect RogueryLootBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectRogueryLootBonus;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x000532A9 File Offset: 0x000514A9
		public static SkillEffect CharmRelationBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCharmRelationBonus;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x000532B5 File Offset: 0x000514B5
		public static SkillEffect TradePenaltyReduction
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTradePenaltyReduction;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000532C1 File Offset: 0x000514C1
		public static SkillEffect SurgeonSurvivalBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSurgeonSurvivalBonus;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x000532CD File Offset: 0x000514CD
		public static SkillEffect SiegeEngineProductionBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSiegeEngineProductionBonus;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x000532D9 File Offset: 0x000514D9
		public static SkillEffect TownProjectBuildingBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTownProjectBuildingBonus;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000532E5 File Offset: 0x000514E5
		public static SkillEffect HealingRateBonusForHeroes
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHealingRateBonusForHeroes;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x000532F1 File Offset: 0x000514F1
		public static SkillEffect HealingRateBonusForRegulars
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHealingRateBonusForRegulars;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000532FD File Offset: 0x000514FD
		public static SkillEffect GovernorHealingRateBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectGovernorHealingRateBonus;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00053309 File Offset: 0x00051509
		public static SkillEffect LeadershipMoraleBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectLeadershipMoraleBonus;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00053315 File Offset: 0x00051515
		public static SkillEffect LeadershipGarrisonSizeBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectLeadershipGarrisonSizeBonus;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00053321 File Offset: 0x00051521
		public static SkillEffect StewardPartySizeBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectStewardPartySizeBonus;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0005332D File Offset: 0x0005152D
		public static SkillEffect SneakDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSneakDamage;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00053339 File Offset: 0x00051539
		public static SkillEffect CrouchedSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCrouchedSpeed;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x00053345 File Offset: 0x00051545
		public static SkillEffect NoiseSuppression
		{
			get
			{
				return DefaultSkillEffects.Instance._effectNoiseSuppression;
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00053351 File Offset: 0x00051551
		public DefaultSkillEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00053360 File Offset: 0x00051560
		private void RegisterAll()
		{
			this._effectOneHandedSpeed = this.Create("OneHandedSpeed");
			this._effectOneHandedDamage = this.Create("OneHandedDamage");
			this._effectTwoHandedSpeed = this.Create("TwoHandedSpeed");
			this._effectTwoHandedDamage = this.Create("TwoHandedDamage");
			this._effectPolearmSpeed = this.Create("PolearmSpeed");
			this._effectPolearmDamage = this.Create("PolearmDamage");
			this._effectBowDamage = this.Create("BowDamage");
			this._effectBowAccuracy = this.Create("BowAccuracy");
			this._effectThrowingSpeed = this.Create("ThrowingSpeed");
			this._effectThrowingDamage = this.Create("ThrowingDamage");
			this._effectThrowingAccuracy = this.Create("ThrowingAccuracy");
			this._effectCrossbowReloadSpeed = this.Create("CrossbowReloadSpeed");
			this._effectCrossbowAccuracy = this.Create("CrossbowAccuracy");
			this._effectHorseSpeed = this.Create("HorseSpeed");
			this._effectHorseManeuver = this.Create("HorseManeuver");
			this._effectMountedWeaponDamagePenalty = this.Create("MountedWeaponDamagePenalty");
			this._effectMountedWeaponSpeedPenalty = this.Create("MountedWeaponSpeedPenalty");
			this._effectDismountResistance = this.Create("DismountResistance");
			this._effectAthleticsSpeedFactor = this.Create("AthleticsSpeedFactor");
			this._effectAthleticsWeightFactor = this.Create("AthleticsWeightFactor");
			this._effectKnockBackResistance = this.Create("KnockBackResistance");
			this._effectKnockDownResistance = this.Create("KnockDownResistance");
			this._effectSmithingLevel = this.Create("SmithingLevel");
			this._effectTacticsAdvantage = this.Create("TacticsAdvantage");
			this._effectTacticsTroopSacrificeReduction = this.Create("TacticsTroopSacrificeReduction");
			this._effectTrackingRadius = this.Create("TrackingRadius");
			this._effectTrackingSpottingDistance = this.Create("TrackingSpottingDistance");
			this._effectTrackingTrackInformation = this.Create("TrackingTrackInformation");
			this._effectRogueryLootBonus = this.Create("RogueryLootBonus");
			this._effectCharmRelationBonus = this.Create("CharmRelationBonus");
			this._effectTradePenaltyReduction = this.Create("TradePenaltyReduction");
			this._effectLeadershipMoraleBonus = this.Create("LeadershipMoraleBonus");
			this._effectLeadershipGarrisonSizeBonus = this.Create("LeadershipGarrisonSizeBonus");
			this._effectSurgeonSurvivalBonus = this.Create("SurgeonSurvivalBonus");
			this._effectHealingRateBonusForHeroes = this.Create("HealingRateBonusForHeroes");
			this._effectHealingRateBonusForRegulars = this.Create("HealingRateBonusForRegulars");
			this._effectGovernorHealingRateBonus = this.Create("GovernorHealingRateBonus");
			this._effectSiegeEngineProductionBonus = this.Create("SiegeEngineProductionBonus");
			this._effectTownProjectBuildingBonus = this.Create("TownProjectBuildingBonus");
			this._effectStewardPartySizeBonus = this.Create("StewardPartySizeBonus");
			this._effectSneakDamage = this.Create("SneakDamage");
			this._effectCrouchedSpeed = this.Create("CrouchedSpeed");
			this._effectNoiseSuppression = this.Create("NoiseSuppression");
			this.InitializeAll();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0005364E File Offset: 0x0005184E
		private SkillEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<SkillEffect>(new SkillEffect(stringId));
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00053668 File Offset: 0x00051868
		private void InitializeAll()
		{
			this._effectOneHandedSpeed.Initialize(new TextObject("{=hjxRvb9l}One handed weapon speed: +{a0}%", null), DefaultSkills.OneHanded, PartyRole.Personal, 0.0007f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectOneHandedDamage.Initialize(new TextObject("{=baUFKAbd}One handed weapon damage: +{a0}%", null), DefaultSkills.OneHanded, PartyRole.Personal, 0.0015f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTwoHandedSpeed.Initialize(new TextObject("{=Np94rYMz}Two handed weapon speed: +{a0}%", null), DefaultSkills.TwoHanded, PartyRole.Personal, 0.0006f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTwoHandedDamage.Initialize(new TextObject("{=QkbbLb4v}Two handed weapon damage: +{a0}%", null), DefaultSkills.TwoHanded, PartyRole.Personal, 0.0016f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectPolearmSpeed.Initialize(new TextObject("{=2ATI9qVM}Polearm weapon speed: +{a0}%", null), DefaultSkills.Polearm, PartyRole.Personal, 0.0006f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectPolearmDamage.Initialize(new TextObject("{=17cIGVQE}Polearm weapon damage: +{a0}%", null), DefaultSkills.Polearm, PartyRole.Personal, 0.0007f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectBowDamage.Initialize(new TextObject("{=RUZHJMQO}Bow Damage: +{a0}%", null), DefaultSkills.Bow, PartyRole.Personal, 0.0011f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectBowAccuracy.Initialize(new TextObject("{=sQCS90Wq}Bow Accuracy: +{a0}%", null), DefaultSkills.Bow, PartyRole.Personal, -0.0009f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectThrowingSpeed.Initialize(new TextObject("{=Z0CoeojG}Thrown weapon speed: +{a0}%", null), DefaultSkills.Throwing, PartyRole.Personal, 0.0007f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectThrowingDamage.Initialize(new TextObject("{=TQMGppEk}Thrown weapon damage: +{a0}%", null), DefaultSkills.Throwing, PartyRole.Personal, 0.0006f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectThrowingAccuracy.Initialize(new TextObject("{=SfKrjKuO}Thrown weapon accuracy: +{a0}%", null), DefaultSkills.Throwing, PartyRole.Personal, -0.0006f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectCrossbowReloadSpeed.Initialize(new TextObject("{=W0Zu4iDz}Crossbow reload speed: +{a0}%", null), DefaultSkills.Crossbow, PartyRole.Personal, 0.0007f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectCrossbowAccuracy.Initialize(new TextObject("{=JwWnpD40}Crossbow accuracy: +{a0}%", null), DefaultSkills.Crossbow, PartyRole.Personal, -0.0005f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectHorseSpeed.Initialize(new TextObject("{=Y07OcP1T}Horse speed: +{a0}", null), DefaultSkills.Riding, PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectHorseManeuver.Initialize(new TextObject("{=AahNTeXY}Horse maneuver: +{a0}", null), DefaultSkills.Riding, PartyRole.Personal, 0.0004f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectMountedWeaponDamagePenalty.Initialize(new TextObject("{=0dbwEczK}Mounted weapon damage penalty: {a0}%", null), DefaultSkills.Riding, PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, -0.2f, float.MinValue, 0f);
			this._effectMountedWeaponSpeedPenalty.Initialize(new TextObject("{=oE5etyy0}Mounted weapon speed & reload penalty: {a0}%", null), DefaultSkills.Riding, PartyRole.Personal, 0.003f, EffectIncrementType.AddFactor, -0.3f, float.MinValue, 0f);
			this._effectDismountResistance.Initialize(new TextObject("{=kbHJVxAo}Dismount resistance: {a0}% of max. hitpoints", null), DefaultSkills.Riding, PartyRole.Personal, 0.001f, EffectIncrementType.AddFactor, 0.4f, float.MinValue, float.MaxValue);
			this._effectAthleticsSpeedFactor.Initialize(new TextObject("{=rgb6vdon}Running speed increased by {a0}%", null), DefaultSkills.Athletics, PartyRole.Personal, 0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectAthleticsWeightFactor.Initialize(new TextObject("{=WaUuhxwv}Weight penalty reduced by: {a0}%", null), DefaultSkills.Athletics, PartyRole.Personal, -0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectKnockBackResistance.Initialize(new TextObject("{=TyjDHQUv}Knock back resistance: {a0}% of max. hitpoints", null), DefaultSkills.Athletics, PartyRole.Personal, 0.001f, EffectIncrementType.AddFactor, 0.15f, float.MinValue, float.MaxValue);
			this._effectKnockDownResistance.Initialize(new TextObject("{=tlNZIH3l}Knock down resistance: {a0}% of max. hitpoints", null), DefaultSkills.Athletics, PartyRole.Personal, 0.001f, EffectIncrementType.AddFactor, 0.4f, float.MinValue, float.MaxValue);
			this._effectSmithingLevel.Initialize(new TextObject("{=ImN8Cfk6}Max difficulty of weapon that can be smithed without penalty: {a0}", null), DefaultSkills.Crafting, PartyRole.Personal, 1f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectTacticsAdvantage.Initialize(new TextObject("{=XO3SOlZx}Simulation advantage: +{a0}%", null), DefaultSkills.Tactics, PartyRole.Personal, 0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTacticsTroopSacrificeReduction.Initialize(new TextObject("{=VHdyQYKI}Decrease the sacrificed troop number when trying to get away +{a0}%", null), DefaultSkills.Tactics, PartyRole.Personal, -0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTrackingRadius.Initialize(new TextObject("{=kqJipMqc}Track detection radius +{a0}%", null), DefaultSkills.Scouting, PartyRole.Scout, 0.1f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectTrackingSpottingDistance.Initialize(new TextObject("{=lbrOAvKj}Spotting distance +{a0}%", null), DefaultSkills.Scouting, PartyRole.Scout, 0.06f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectTrackingTrackInformation.Initialize(new TextObject("{=uNls3bOP}Track information level: {a0}", null), DefaultSkills.Scouting, PartyRole.Scout, 0.04f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectRogueryLootBonus.Initialize(new TextObject("{=bN3bLDb2}Battle Loot +{a0}%", null), DefaultSkills.Roguery, PartyRole.PartyLeader, 0.0025f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectCharmRelationBonus.Initialize(new TextObject("{=c5dsio8Q}Relation increase with NPCs +{a0}%", null), DefaultSkills.Charm, PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTradePenaltyReduction.Initialize(new TextObject("{=uq7JwT1Z}Trade penalty Reduction +{a0}%", null), DefaultSkills.Trade, PartyRole.PartyLeader, 0.002f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectLeadershipMoraleBonus.Initialize(new TextObject("{=n3bFiuVu}Increase morale of the parties under your command +{a0}", null), DefaultSkills.Leadership, PartyRole.Personal, 0.1f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectLeadershipGarrisonSizeBonus.Initialize(new TextObject("{=cSt26auo}Increase garrison size by +{a0}", null), DefaultSkills.Leadership, PartyRole.Personal, 0.2f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectSurgeonSurvivalBonus.Initialize(new TextObject("{=w4BzNJYl}Casualty survival chance +{a0}%", null), DefaultSkills.Medicine, PartyRole.Surgeon, 0.01f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectHealingRateBonusForHeroes.Initialize(new TextObject("{=fUvs4g40}Healing rate increase for heroes +{a0}%", null), DefaultSkills.Medicine, PartyRole.Surgeon, 0.005f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectHealingRateBonusForRegulars.Initialize(new TextObject("{=A310vHqJ}Healing rate increase for troops +{a0}%", null), DefaultSkills.Medicine, PartyRole.Surgeon, 0.01f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectGovernorHealingRateBonus.Initialize(new TextObject("{=6mQGst9s}Healing rate increase +{a0}%", null), DefaultSkills.Medicine, PartyRole.Governor, 0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectSiegeEngineProductionBonus.Initialize(new TextObject("{=spbYlf0y}Faster siege engine production +{a0}%", null), DefaultSkills.Engineering, PartyRole.Engineer, 0.001f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectTownProjectBuildingBonus.Initialize(new TextObject("{=2paRqO8u}Faster building production +{a0}%", null), DefaultSkills.Engineering, PartyRole.Governor, 0.0025f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectStewardPartySizeBonus.Initialize(new TextObject("{=jNDUXetG}Increase party size by +{a0}", null), DefaultSkills.Steward, PartyRole.Quartermaster, 0.25f, EffectIncrementType.Add, 0f, float.MinValue, float.MaxValue);
			this._effectSneakDamage.Initialize(new TextObject("{=vDieFIKM}Sneak attack damage +{a0}%", null), DefaultSkills.Roguery, PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, 0.5f, float.MinValue, float.MaxValue);
			this._effectCrouchedSpeed.Initialize(new TextObject("{=sTgjLrPX}Crouched speed +{a0}%", null), DefaultSkills.Roguery, PartyRole.Personal, 0.0005f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
			this._effectNoiseSuppression.Initialize(new TextObject("{=GzLd3ca9}Noise suppression -{a0}%", null), DefaultSkills.Roguery, PartyRole.Personal, 0.0025f, EffectIncrementType.AddFactor, 0f, float.MinValue, float.MaxValue);
		}

		// Token: 0x0400052E RID: 1326
		private SkillEffect _effectOneHandedSpeed;

		// Token: 0x0400052F RID: 1327
		private SkillEffect _effectOneHandedDamage;

		// Token: 0x04000530 RID: 1328
		private SkillEffect _effectTwoHandedSpeed;

		// Token: 0x04000531 RID: 1329
		private SkillEffect _effectTwoHandedDamage;

		// Token: 0x04000532 RID: 1330
		private SkillEffect _effectPolearmSpeed;

		// Token: 0x04000533 RID: 1331
		private SkillEffect _effectPolearmDamage;

		// Token: 0x04000534 RID: 1332
		private SkillEffect _effectBowDamage;

		// Token: 0x04000535 RID: 1333
		private SkillEffect _effectBowAccuracy;

		// Token: 0x04000536 RID: 1334
		private SkillEffect _effectThrowingSpeed;

		// Token: 0x04000537 RID: 1335
		private SkillEffect _effectThrowingDamage;

		// Token: 0x04000538 RID: 1336
		private SkillEffect _effectThrowingAccuracy;

		// Token: 0x04000539 RID: 1337
		private SkillEffect _effectCrossbowReloadSpeed;

		// Token: 0x0400053A RID: 1338
		private SkillEffect _effectCrossbowAccuracy;

		// Token: 0x0400053B RID: 1339
		private SkillEffect _effectHorseSpeed;

		// Token: 0x0400053C RID: 1340
		private SkillEffect _effectHorseManeuver;

		// Token: 0x0400053D RID: 1341
		private SkillEffect _effectMountedWeaponDamagePenalty;

		// Token: 0x0400053E RID: 1342
		private SkillEffect _effectMountedWeaponSpeedPenalty;

		// Token: 0x0400053F RID: 1343
		private SkillEffect _effectDismountResistance;

		// Token: 0x04000540 RID: 1344
		private SkillEffect _effectAthleticsSpeedFactor;

		// Token: 0x04000541 RID: 1345
		private SkillEffect _effectAthleticsWeightFactor;

		// Token: 0x04000542 RID: 1346
		private SkillEffect _effectKnockBackResistance;

		// Token: 0x04000543 RID: 1347
		private SkillEffect _effectKnockDownResistance;

		// Token: 0x04000544 RID: 1348
		private SkillEffect _effectSmithingLevel;

		// Token: 0x04000545 RID: 1349
		private SkillEffect _effectTacticsAdvantage;

		// Token: 0x04000546 RID: 1350
		private SkillEffect _effectTacticsTroopSacrificeReduction;

		// Token: 0x04000547 RID: 1351
		private SkillEffect _effectTrackingRadius;

		// Token: 0x04000548 RID: 1352
		private SkillEffect _effectTrackingSpottingDistance;

		// Token: 0x04000549 RID: 1353
		private SkillEffect _effectTrackingTrackInformation;

		// Token: 0x0400054A RID: 1354
		private SkillEffect _effectRogueryLootBonus;

		// Token: 0x0400054B RID: 1355
		private SkillEffect _effectCharmRelationBonus;

		// Token: 0x0400054C RID: 1356
		private SkillEffect _effectTradePenaltyReduction;

		// Token: 0x0400054D RID: 1357
		private SkillEffect _effectSurgeonSurvivalBonus;

		// Token: 0x0400054E RID: 1358
		private SkillEffect _effectSiegeEngineProductionBonus;

		// Token: 0x0400054F RID: 1359
		private SkillEffect _effectTownProjectBuildingBonus;

		// Token: 0x04000550 RID: 1360
		private SkillEffect _effectHealingRateBonusForHeroes;

		// Token: 0x04000551 RID: 1361
		private SkillEffect _effectHealingRateBonusForRegulars;

		// Token: 0x04000552 RID: 1362
		private SkillEffect _effectGovernorHealingRateBonus;

		// Token: 0x04000553 RID: 1363
		private SkillEffect _effectLeadershipMoraleBonus;

		// Token: 0x04000554 RID: 1364
		private SkillEffect _effectLeadershipGarrisonSizeBonus;

		// Token: 0x04000555 RID: 1365
		private SkillEffect _effectStewardPartySizeBonus;

		// Token: 0x04000556 RID: 1366
		private SkillEffect _effectSneakDamage;

		// Token: 0x04000557 RID: 1367
		private SkillEffect _effectCrouchedSpeed;

		// Token: 0x04000558 RID: 1368
		private SkillEffect _effectNoiseSuppression;
	}
}
