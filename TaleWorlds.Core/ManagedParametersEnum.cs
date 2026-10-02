using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009A RID: 154
	public enum ManagedParametersEnum
	{
		// Token: 0x040004BB RID: 1211
		EnableCampaignTutorials,
		// Token: 0x040004BC RID: 1212
		ReducedMouseSensitivityMultiplier,
		// Token: 0x040004BD RID: 1213
		MeleeAddedElevationForCrosshair,
		// Token: 0x040004BE RID: 1214
		BipedalRadius,
		// Token: 0x040004BF RID: 1215
		QuadrupedalRadius,
		// Token: 0x040004C0 RID: 1216
		BipedalCombatSpeedMinMultiplier,
		// Token: 0x040004C1 RID: 1217
		BipedalCombatSpeedMaxMultiplier,
		// Token: 0x040004C2 RID: 1218
		BipedalRangedReadySpeedMultiplier,
		// Token: 0x040004C3 RID: 1219
		BipedalRangedReloadSpeedMultiplier,
		// Token: 0x040004C4 RID: 1220
		DamageInterruptAttackThresholdPierce,
		// Token: 0x040004C5 RID: 1221
		DamageInterruptAttackThresholdCut,
		// Token: 0x040004C6 RID: 1222
		DamageInterruptAttackThresholdBlunt,
		// Token: 0x040004C7 RID: 1223
		MakesRearAttackDamageThreshold,
		// Token: 0x040004C8 RID: 1224
		MissileMinimumDamageToStick,
		// Token: 0x040004C9 RID: 1225
		BreakableProjectileMinimumBreakSpeed,
		// Token: 0x040004CA RID: 1226
		FistFightDamageMultiplier,
		// Token: 0x040004CB RID: 1227
		FallDamageMultiplier,
		// Token: 0x040004CC RID: 1228
		FallDamageAbsorption,
		// Token: 0x040004CD RID: 1229
		FallSpeedReductionMultiplierForRiderDamage,
		// Token: 0x040004CE RID: 1230
		SwingHitWithArmDamageMultiplier,
		// Token: 0x040004CF RID: 1231
		ThrustHitWithArmDamageMultiplier,
		// Token: 0x040004D0 RID: 1232
		NonTipThrustHitDamageMultiplier,
		// Token: 0x040004D1 RID: 1233
		SwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004D2 RID: 1234
		SwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004D3 RID: 1235
		SwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004D4 RID: 1236
		SwingCombatSpeedGraphOneProgressValue,
		// Token: 0x040004D5 RID: 1237
		OverSwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004D6 RID: 1238
		OverSwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004D7 RID: 1239
		OverSwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004D8 RID: 1240
		OverSwingCombatSpeedGraphOneProgressValue,
		// Token: 0x040004D9 RID: 1241
		ThrustCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004DA RID: 1242
		ThrustCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004DB RID: 1243
		ThrustCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004DC RID: 1244
		ThrustCombatSpeedGraphOneProgressValue,
		// Token: 0x040004DD RID: 1245
		StunPeriodAttackerSwing,
		// Token: 0x040004DE RID: 1246
		StunPeriodAttackerThrust,
		// Token: 0x040004DF RID: 1247
		StunDefendWeaponWeightOffsetShield,
		// Token: 0x040004E0 RID: 1248
		StunDefendWeaponWeightMultiplierWeaponWeight,
		// Token: 0x040004E1 RID: 1249
		StunDefendWeaponWeightBonusTwoHanded,
		// Token: 0x040004E2 RID: 1250
		StunDefendWeaponWeightBonusPolearm,
		// Token: 0x040004E3 RID: 1251
		StunMomentumTransferFactor,
		// Token: 0x040004E4 RID: 1252
		StunDefendWeaponWeightParryMultiplier,
		// Token: 0x040004E5 RID: 1253
		StunDefendWeaponWeightBonusRightStance,
		// Token: 0x040004E6 RID: 1254
		StunDefendWeaponWeightBonusActiveBlocked,
		// Token: 0x040004E7 RID: 1255
		StunDefendWeaponWeightBonusChamberBlocked,
		// Token: 0x040004E8 RID: 1256
		StunPeriodAttackerFriendlyFire,
		// Token: 0x040004E9 RID: 1257
		StunPeriodMax,
		// Token: 0x040004EA RID: 1258
		ProjectileMaxPenetrationSpeed,
		// Token: 0x040004EB RID: 1259
		ObjectMinPenetration,
		// Token: 0x040004EC RID: 1260
		ObjectMaxPenetration,
		// Token: 0x040004ED RID: 1261
		ProjectileMinPenetration,
		// Token: 0x040004EE RID: 1262
		ProjectileMaxPenetration,
		// Token: 0x040004EF RID: 1263
		RotatingProjectileMinPenetration,
		// Token: 0x040004F0 RID: 1264
		RotatingProjectileMaxPenetration,
		// Token: 0x040004F1 RID: 1265
		ShieldRightStanceBlockDamageMultiplier,
		// Token: 0x040004F2 RID: 1266
		ShieldCorrectSideBlockDamageMultiplier,
		// Token: 0x040004F3 RID: 1267
		AgentProjectileNormalWeight,
		// Token: 0x040004F4 RID: 1268
		ProjectileNormalWeight,
		// Token: 0x040004F5 RID: 1269
		ShieldPenetrationOffset,
		// Token: 0x040004F6 RID: 1270
		ShieldPenetrationFactor,
		// Token: 0x040004F7 RID: 1271
		AirFrictionJavelin,
		// Token: 0x040004F8 RID: 1272
		AirFrictionArrow,
		// Token: 0x040004F9 RID: 1273
		AirFrictionBallistaBolt,
		// Token: 0x040004FA RID: 1274
		AirFrictionBullet,
		// Token: 0x040004FB RID: 1275
		AirFrictionKnife,
		// Token: 0x040004FC RID: 1276
		AirFrictionAxe,
		// Token: 0x040004FD RID: 1277
		AirFrictionStone,
		// Token: 0x040004FE RID: 1278
		AirFrictionBoulder,
		// Token: 0x040004FF RID: 1279
		AirFrictionBallistaStone,
		// Token: 0x04000500 RID: 1280
		AirFrictionBallistaBoulder,
		// Token: 0x04000501 RID: 1281
		HeavyAttackMomentumMultiplier,
		// Token: 0x04000502 RID: 1282
		ActivateHeroTest,
		// Token: 0x04000503 RID: 1283
		Count
	}
}
