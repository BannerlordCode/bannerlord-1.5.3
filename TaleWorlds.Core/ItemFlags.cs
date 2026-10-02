using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000095 RID: 149
	[Flags]
	public enum ItemFlags : uint
	{
		// Token: 0x0400046E RID: 1134
		ForceAttachOffHandPrimaryItemBone = 256U,
		// Token: 0x0400046F RID: 1135
		ForceAttachOffHandSecondaryItemBone = 512U,
		// Token: 0x04000470 RID: 1136
		AttachmentMask = 768U,
		// Token: 0x04000471 RID: 1137
		NotUsableByFemale = 1024U,
		// Token: 0x04000472 RID: 1138
		NotUsableByMale = 2048U,
		// Token: 0x04000473 RID: 1139
		DropOnWeaponChange = 4096U,
		// Token: 0x04000474 RID: 1140
		DropOnAnyAction = 8192U,
		// Token: 0x04000475 RID: 1141
		CannotBePickedUp = 16384U,
		// Token: 0x04000476 RID: 1142
		CanBePickedUpFromCorpse = 32768U,
		// Token: 0x04000477 RID: 1143
		QuickFadeOut = 65536U,
		// Token: 0x04000478 RID: 1144
		WoodenAttack = 131072U,
		// Token: 0x04000479 RID: 1145
		WoodenParry = 262144U,
		// Token: 0x0400047A RID: 1146
		HeldInOffHand = 524288U,
		// Token: 0x0400047B RID: 1147
		HasToBeHeldUp = 1048576U,
		// Token: 0x0400047C RID: 1148
		UseTeamColor = 2097152U,
		// Token: 0x0400047D RID: 1149
		Civilian = 4194304U,
		// Token: 0x0400047E RID: 1150
		DoNotScaleBodyAccordingToWeaponLength = 8388608U,
		// Token: 0x0400047F RID: 1151
		DoesNotHideChest = 16777216U,
		// Token: 0x04000480 RID: 1152
		NotStackable = 33554432U,
		// Token: 0x04000481 RID: 1153
		Stealth = 67108864U,
		// Token: 0x04000482 RID: 1154
		DoesNotSpawnWhenDropped = 134217728U
	}
}
