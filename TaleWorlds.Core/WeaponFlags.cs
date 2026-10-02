using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000096 RID: 150
	[Flags]
	public enum WeaponFlags : ulong
	{
		// Token: 0x04000484 RID: 1156
		MeleeWeapon = 1UL,
		// Token: 0x04000485 RID: 1157
		RangedWeapon = 2UL,
		// Token: 0x04000486 RID: 1158
		WeaponMask = 3UL,
		// Token: 0x04000487 RID: 1159
		FirearmAmmo = 4UL,
		// Token: 0x04000488 RID: 1160
		NotUsableWithOneHand = 16UL,
		// Token: 0x04000489 RID: 1161
		NotUsableWithTwoHand = 32UL,
		// Token: 0x0400048A RID: 1162
		HandUsageMask = 48UL,
		// Token: 0x0400048B RID: 1163
		WideGrip = 64UL,
		// Token: 0x0400048C RID: 1164
		AttachAmmoToVisual = 128UL,
		// Token: 0x0400048D RID: 1165
		Consumable = 256UL,
		// Token: 0x0400048E RID: 1166
		HasHitPoints = 512UL,
		// Token: 0x0400048F RID: 1167
		DataValueMask = 768UL,
		// Token: 0x04000490 RID: 1168
		HasString = 1024UL,
		// Token: 0x04000491 RID: 1169
		StringHeldByHand = 3072UL,
		// Token: 0x04000492 RID: 1170
		UnloadWhenSheathed = 4096UL,
		// Token: 0x04000493 RID: 1171
		AffectsArea = 8192UL,
		// Token: 0x04000494 RID: 1172
		AffectsAreaBig = 16384UL,
		// Token: 0x04000495 RID: 1173
		Burning = 32768UL,
		// Token: 0x04000496 RID: 1174
		BonusAgainstShield = 65536UL,
		// Token: 0x04000497 RID: 1175
		CanPenetrateShield = 131072UL,
		// Token: 0x04000498 RID: 1176
		CantReloadOnHorseback = 262144UL,
		// Token: 0x04000499 RID: 1177
		AutoReload = 524288UL,
		// Token: 0x0400049A RID: 1178
		CanBeUsedWhileCrouched = 1048576UL,
		// Token: 0x0400049B RID: 1179
		TwoHandIdleOnMount = 2097152UL,
		// Token: 0x0400049C RID: 1180
		NoBlood = 4194304UL,
		// Token: 0x0400049D RID: 1181
		PenaltyWithShield = 8388608UL,
		// Token: 0x0400049E RID: 1182
		CanDismount = 16777216UL,
		// Token: 0x0400049F RID: 1183
		CanHook = 33554432UL,
		// Token: 0x040004A0 RID: 1184
		CanKnockDown = 67108864UL,
		// Token: 0x040004A1 RID: 1185
		CanCrushThrough = 134217728UL,
		// Token: 0x040004A2 RID: 1186
		CanBlockRanged = 268435456UL,
		// Token: 0x040004A3 RID: 1187
		MissileWithPhysics = 536870912UL,
		// Token: 0x040004A4 RID: 1188
		MultiplePenetration = 1073741824UL,
		// Token: 0x040004A5 RID: 1189
		LeavesTrail = 2147483648UL,
		// Token: 0x040004A6 RID: 1190
		UseHandAsThrowBase = 4294967296UL,
		// Token: 0x040004A7 RID: 1191
		HeldBackwards = 8589934592UL,
		// Token: 0x040004A8 RID: 1192
		CanKillEvenIfBlunt = 17179869184UL,
		// Token: 0x040004A9 RID: 1193
		AmmoBreaksOnBounceBack = 68719476736UL,
		// Token: 0x040004AA RID: 1194
		AmmoCanBreakOnBounceBack = 137438953472UL,
		// Token: 0x040004AB RID: 1195
		AmmoBreakOnBounceBackMask = 206158430208UL,
		// Token: 0x040004AC RID: 1196
		AmmoSticksWhenShot = 274877906944UL
	}
}
