using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000153 RID: 339
	public enum OrderType
	{
		// Token: 0x04000412 RID: 1042
		None,
		// Token: 0x04000413 RID: 1043
		Move,
		// Token: 0x04000414 RID: 1044
		MoveToLineSegment,
		// Token: 0x04000415 RID: 1045
		MoveToLineSegmentWithHorizontalLayout,
		// Token: 0x04000416 RID: 1046
		Charge,
		// Token: 0x04000417 RID: 1047
		ChargeWithTarget,
		// Token: 0x04000418 RID: 1048
		StandYourGround,
		// Token: 0x04000419 RID: 1049
		FollowMe,
		// Token: 0x0400041A RID: 1050
		FollowEntity,
		// Token: 0x0400041B RID: 1051
		Retreat,
		// Token: 0x0400041C RID: 1052
		AdvanceTenPaces,
		// Token: 0x0400041D RID: 1053
		FallBackTenPaces,
		// Token: 0x0400041E RID: 1054
		Advance,
		// Token: 0x0400041F RID: 1055
		FallBack,
		// Token: 0x04000420 RID: 1056
		LookAtEnemy,
		// Token: 0x04000421 RID: 1057
		LookAtDirection,
		// Token: 0x04000422 RID: 1058
		ArrangementLine,
		// Token: 0x04000423 RID: 1059
		ArrangementCloseOrder,
		// Token: 0x04000424 RID: 1060
		ArrangementLoose,
		// Token: 0x04000425 RID: 1061
		ArrangementCircular,
		// Token: 0x04000426 RID: 1062
		ArrangementSchiltron,
		// Token: 0x04000427 RID: 1063
		ArrangementVee,
		// Token: 0x04000428 RID: 1064
		ArrangementColumn,
		// Token: 0x04000429 RID: 1065
		ArrangementScatter,
		// Token: 0x0400042A RID: 1066
		FormCustom,
		// Token: 0x0400042B RID: 1067
		FormDeep,
		// Token: 0x0400042C RID: 1068
		FormWide,
		// Token: 0x0400042D RID: 1069
		FormWider,
		// Token: 0x0400042E RID: 1070
		CohesionHigh,
		// Token: 0x0400042F RID: 1071
		CohesionMedium,
		// Token: 0x04000430 RID: 1072
		CohesionLow,
		// Token: 0x04000431 RID: 1073
		HoldFire,
		// Token: 0x04000432 RID: 1074
		FireAtWill,
		// Token: 0x04000433 RID: 1075
		RideFree,
		// Token: 0x04000434 RID: 1076
		Mount,
		// Token: 0x04000435 RID: 1077
		Dismount,
		// Token: 0x04000436 RID: 1078
		AIControlOn,
		// Token: 0x04000437 RID: 1079
		AIControlOff,
		// Token: 0x04000438 RID: 1080
		Transfer,
		// Token: 0x04000439 RID: 1081
		Use,
		// Token: 0x0400043A RID: 1082
		AttackEntity,
		// Token: 0x0400043B RID: 1083
		PointDefence,
		// Token: 0x0400043C RID: 1084
		Count
	}
}
