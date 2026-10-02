using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017D RID: 381
	public class FormationQuerySystem
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00049641 File Offset: 0x00047841
		public TeamQuerySystem Team
		{
			get
			{
				return this.Formation.Team.QuerySystem;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00049653 File Offset: 0x00047853
		public float FormationPower
		{
			get
			{
				return this._formationPower.Value;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x00049660 File Offset: 0x00047860
		public float FormationPowerReadOnly
		{
			get
			{
				return this._formationPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x0004966D File Offset: 0x0004786D
		public float FormationMeleeFightingPower
		{
			get
			{
				return this._formationMeleeFightingPower.Value;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0004967A File Offset: 0x0004787A
		public float FormationMeleeFightingPowerReadOnly
		{
			get
			{
				return this._formationMeleeFightingPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00049687 File Offset: 0x00047887
		public Vec2 EstimatedDirection
		{
			get
			{
				return this._estimatedDirection.Value;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00049694 File Offset: 0x00047894
		public Vec2 EstimatedDirectionReadOnly
		{
			get
			{
				return this._estimatedDirection.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x000496A1 File Offset: 0x000478A1
		public float EstimatedInterval
		{
			get
			{
				return this._estimatedInterval.Value;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x000496AE File Offset: 0x000478AE
		public float EstimatedIntervalReadOnly
		{
			get
			{
				return this._estimatedInterval.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x000496BB File Offset: 0x000478BB
		public Vec2 AverageAllyPosition
		{
			get
			{
				return this._averageAllyPosition.Value;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x000496C8 File Offset: 0x000478C8
		public Vec2 AverageAllyPositionReadOnly
		{
			get
			{
				return this._averageAllyPosition.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000496D5 File Offset: 0x000478D5
		public float IdealAverageDisplacement
		{
			get
			{
				return this._idealAverageDisplacement.Value;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x000496E2 File Offset: 0x000478E2
		public float IdealAverageDisplacementReadOnly
		{
			get
			{
				return this._idealAverageDisplacement.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x000496EF File Offset: 0x000478EF
		public MBList<Agent> LocalAllyUnits
		{
			get
			{
				return this._localAllyUnits.Value;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x000496FC File Offset: 0x000478FC
		public MBList<Agent> LocalAllyUnitsReadOnly
		{
			get
			{
				return this._localAllyUnits.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x00049709 File Offset: 0x00047909
		public MBList<Agent> LocalEnemyUnits
		{
			get
			{
				return this._localEnemyUnits.Value;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00049716 File Offset: 0x00047916
		public MBList<Agent> LocalEnemyUnitsReadOnly
		{
			get
			{
				return this._localEnemyUnits.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00049723 File Offset: 0x00047923
		public FormationClass MainClass
		{
			get
			{
				return this._mainClass.Value;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00049730 File Offset: 0x00047930
		public FormationClass MainClassReadOnly
		{
			get
			{
				return this._mainClass.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x0004973D File Offset: 0x0004793D
		public float InfantryUnitRatio
		{
			get
			{
				return this._infantryUnitRatio.Value;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0004974A File Offset: 0x0004794A
		public float InfantryUnitRatioReadOnly
		{
			get
			{
				return this._infantryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00049757 File Offset: 0x00047957
		public float HasShieldUnitRatio
		{
			get
			{
				return this._hasShieldUnitRatio.Value;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00049764 File Offset: 0x00047964
		public float HasShieldUnitRatioReadOnly
		{
			get
			{
				return this._hasShieldUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00049771 File Offset: 0x00047971
		public float HasThrowingUnitRatio
		{
			get
			{
				return this._hasThrowingUnitRatio.Value;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0004977E File Offset: 0x0004797E
		public float HasThrowingUnitRatioReadOnly
		{
			get
			{
				return this._hasThrowingUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0004978B File Offset: 0x0004798B
		public float RangedUnitRatio
		{
			get
			{
				return this._rangedUnitRatio.Value;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00049798 File Offset: 0x00047998
		public float RangedUnitRatioReadOnly
		{
			get
			{
				return this._rangedUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x000497A5 File Offset: 0x000479A5
		public int InsideCastleUnitCountIncludingUnpositioned
		{
			get
			{
				return this._insideCastleUnitCountIncludingUnpositioned.Value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x000497B2 File Offset: 0x000479B2
		public int InsideCastleUnitCountIncludingUnpositionedReadOnly
		{
			get
			{
				return this._insideCastleUnitCountIncludingUnpositioned.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x000497BF File Offset: 0x000479BF
		public int InsideCastleUnitCountPositioned
		{
			get
			{
				return this._insideCastleUnitCountPositioned.Value;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x000497CC File Offset: 0x000479CC
		public int InsideCastleUnitCountPositionedReadOnly
		{
			get
			{
				return this._insideCastleUnitCountPositioned.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x000497D9 File Offset: 0x000479D9
		public float CavalryUnitRatio
		{
			get
			{
				return this._cavalryUnitRatio.Value;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x000497E6 File Offset: 0x000479E6
		public float CavalryUnitRatioReadOnly
		{
			get
			{
				return this._cavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x000497F3 File Offset: 0x000479F3
		public float RangedCavalryUnitRatio
		{
			get
			{
				return this._rangedCavalryUnitRatio.Value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00049800 File Offset: 0x00047A00
		public float RangedCavalryUnitRatioReadOnly
		{
			get
			{
				return this._rangedCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x0004980D File Offset: 0x00047A0D
		public bool IsMeleeFormation
		{
			get
			{
				return this._isMeleeFormation.Value;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x0004981A File Offset: 0x00047A1A
		public bool IsMeleeFormationReadOnly
		{
			get
			{
				return this._isMeleeFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00049827 File Offset: 0x00047A27
		public bool IsInfantryFormation
		{
			get
			{
				return this._isInfantryFormation.Value;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00049834 File Offset: 0x00047A34
		public bool IsInfantryFormationReadOnly
		{
			get
			{
				return this._isInfantryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00049841 File Offset: 0x00047A41
		public bool HasShield
		{
			get
			{
				return this._hasShield.Value;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x0004984E File Offset: 0x00047A4E
		public bool HasShieldReadOnly
		{
			get
			{
				return this._hasShield.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0004985B File Offset: 0x00047A5B
		public bool HasThrowing
		{
			get
			{
				return this._hasThrowing.Value;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00049868 File Offset: 0x00047A68
		public bool HasThrowingReadOnly
		{
			get
			{
				return this._hasThrowing.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00049875 File Offset: 0x00047A75
		public bool IsRangedFormation
		{
			get
			{
				return this._isRangedFormation.Value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00049882 File Offset: 0x00047A82
		public bool IsRangedFormationReadOnly
		{
			get
			{
				return this._isRangedFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0004988F File Offset: 0x00047A8F
		public bool IsCavalryFormation
		{
			get
			{
				return this._isCavalryFormation.Value;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0004989C File Offset: 0x00047A9C
		public bool IsCavalryFormationReadOnly
		{
			get
			{
				return this._isCavalryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x000498A9 File Offset: 0x00047AA9
		public bool IsRangedCavalryFormation
		{
			get
			{
				return this._isRangedCavalryFormation.Value;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x000498B6 File Offset: 0x00047AB6
		public bool IsRangedCavalryFormationReadOnly
		{
			get
			{
				return this._isRangedCavalryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x000498C3 File Offset: 0x00047AC3
		public float MovementSpeedMaximum
		{
			get
			{
				return this._movementSpeedMaximum.Value;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x000498D0 File Offset: 0x00047AD0
		public float MovementSpeedMaximumReadOnly
		{
			get
			{
				return this._movementSpeedMaximum.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x000498DD File Offset: 0x00047ADD
		public float MaximumMissileRange
		{
			get
			{
				return this._maximumMissileRange.Value;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x000498EA File Offset: 0x00047AEA
		public float MaximumMissileRangeReadOnly
		{
			get
			{
				return this._maximumMissileRange.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x000498F7 File Offset: 0x00047AF7
		public float MissileRangeAdjusted
		{
			get
			{
				return this._missileRangeAdjusted.Value;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00049904 File Offset: 0x00047B04
		public float MissileRangeAdjustedReadOnly
		{
			get
			{
				return this._missileRangeAdjusted.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00049911 File Offset: 0x00047B11
		public float LocalInfantryUnitRatio
		{
			get
			{
				return this._localInfantryUnitRatio.Value;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0004991E File Offset: 0x00047B1E
		public float LocalInfantryUnitRatioReadOnly
		{
			get
			{
				return this._localInfantryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0004992B File Offset: 0x00047B2B
		public float LocalRangedUnitRatio
		{
			get
			{
				return this._localRangedUnitRatio.Value;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00049938 File Offset: 0x00047B38
		public float LocalRangedUnitRatioReadOnly
		{
			get
			{
				return this._localRangedUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x00049945 File Offset: 0x00047B45
		public float LocalCavalryUnitRatio
		{
			get
			{
				return this._localCavalryUnitRatio.Value;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00049952 File Offset: 0x00047B52
		public float LocalCavalryUnitRatioReadOnly
		{
			get
			{
				return this._localCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0004995F File Offset: 0x00047B5F
		public float LocalRangedCavalryUnitRatio
		{
			get
			{
				return this._localRangedCavalryUnitRatio.Value;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0004996C File Offset: 0x00047B6C
		public float LocalRangedCavalryUnitRatioReadOnly
		{
			get
			{
				return this._localRangedCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x00049979 File Offset: 0x00047B79
		public float LocalAllyPower
		{
			get
			{
				return this._localAllyPower.Value;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00049986 File Offset: 0x00047B86
		public float LocalAllyPowerReadOnly
		{
			get
			{
				return this._localAllyPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x00049993 File Offset: 0x00047B93
		public float LocalEnemyPower
		{
			get
			{
				return this._localEnemyPower.Value;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000499A0 File Offset: 0x00047BA0
		public float LocalEnemyPowerReadOnly
		{
			get
			{
				return this._localEnemyPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x000499AD File Offset: 0x00047BAD
		public float LocalPowerRatio
		{
			get
			{
				return this._localPowerRatio.Value;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x000499BA File Offset: 0x00047BBA
		public float LocalPowerRatioReadOnly
		{
			get
			{
				return this._localPowerRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x000499C7 File Offset: 0x00047BC7
		public float CasualtyRatio
		{
			get
			{
				return this._casualtyRatio.Value;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x000499D4 File Offset: 0x00047BD4
		public float CasualtyRatioReadOnly
		{
			get
			{
				return this._casualtyRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x000499E1 File Offset: 0x00047BE1
		public bool HasRecentlyTakenRangedDamage
		{
			get
			{
				return this._hasRecentlyTakenRangedDamage.Value;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x000499EE File Offset: 0x00047BEE
		public bool HasRecentlyTakenRangedDamageReadOnly
		{
			get
			{
				return this._hasRecentlyTakenRangedDamage.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x000499FB File Offset: 0x00047BFB
		public bool IsUnderRangedAttack
		{
			get
			{
				return this._isUnderRangedAttack.Value;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00049A08 File Offset: 0x00047C08
		public bool IsUnderRangedAttackReadOnly
		{
			get
			{
				return this._isUnderRangedAttack.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x00049A15 File Offset: 0x00047C15
		public float UnderRangedAttackRatio
		{
			get
			{
				return this._underRangedAttackRatio.Value;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00049A22 File Offset: 0x00047C22
		public float UnderRangedAttackRatioReadOnly
		{
			get
			{
				return this._underRangedAttackRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x00049A2F File Offset: 0x00047C2F
		public float MakingRangedAttackRatio
		{
			get
			{
				return this._makingRangedAttackRatio.Value;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00049A3C File Offset: 0x00047C3C
		public float MakingRangedAttackRatioReadOnly
		{
			get
			{
				return this._makingRangedAttackRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x00049A49 File Offset: 0x00047C49
		public Formation MainFormation
		{
			get
			{
				return this._mainFormation.Value;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00049A56 File Offset: 0x00047C56
		public Formation MainFormationReadOnly
		{
			get
			{
				return this._mainFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00049A63 File Offset: 0x00047C63
		public float MainFormationReliabilityFactor
		{
			get
			{
				return this._mainFormationReliabilityFactor.Value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x00049A70 File Offset: 0x00047C70
		public float MainFormationReliabilityFactorReadOnly
		{
			get
			{
				return this._mainFormationReliabilityFactor.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00049A7D File Offset: 0x00047C7D
		public Vec2 WeightedAverageEnemyPosition
		{
			get
			{
				return this._weightedAverageEnemyPosition.Value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x00049A8A File Offset: 0x00047C8A
		public Vec2 WeightedAverageEnemyPositionReadOnly
		{
			get
			{
				return this._weightedAverageEnemyPosition.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00049A97 File Offset: 0x00047C97
		public Agent ClosestEnemyAgent
		{
			get
			{
				return this._closestEnemyAgent.Value;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00049AA4 File Offset: 0x00047CA4
		public Agent ClosestEnemyAgentReadOnly
		{
			get
			{
				return this._closestEnemyAgent.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00049AB4 File Offset: 0x00047CB4
		public FormationQuerySystem ClosestSignificantlyLargeEnemyFormation
		{
			get
			{
				if (this._closestSignificantlyLargeEnemyFormation.Value == null || this._closestSignificantlyLargeEnemyFormation.Value.CountOfUnits == 0)
				{
					this._closestSignificantlyLargeEnemyFormation.Expire();
				}
				Formation value = this._closestSignificantlyLargeEnemyFormation.Value;
				if (value == null)
				{
					return null;
				}
				return value.QuerySystem;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00049B01 File Offset: 0x00047D01
		public FormationQuerySystem ClosestSignificantlyLargeEnemyFormationReadOnly
		{
			get
			{
				Formation cachedValueUnlessTooOld = this._closestSignificantlyLargeEnemyFormation.GetCachedValueUnlessTooOld();
				if (cachedValueUnlessTooOld == null)
				{
					return null;
				}
				return cachedValueUnlessTooOld.QuerySystem;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00049B1C File Offset: 0x00047D1C
		public FormationQuerySystem FastestSignificantlyLargeEnemyFormation
		{
			get
			{
				if (this._fastestSignificantlyLargeEnemyFormation.Value == null || this._fastestSignificantlyLargeEnemyFormation.Value.CountOfUnits == 0)
				{
					this._fastestSignificantlyLargeEnemyFormation.Expire();
				}
				Formation value = this._fastestSignificantlyLargeEnemyFormation.Value;
				if (value == null)
				{
					return null;
				}
				return value.QuerySystem;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00049B69 File Offset: 0x00047D69
		public FormationQuerySystem FastestSignificantlyLargeEnemyFormationReadOnly
		{
			get
			{
				Formation cachedValueUnlessTooOld = this._fastestSignificantlyLargeEnemyFormation.GetCachedValueUnlessTooOld();
				if (cachedValueUnlessTooOld == null)
				{
					return null;
				}
				return cachedValueUnlessTooOld.QuerySystem;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00049B81 File Offset: 0x00047D81
		public Vec2 HighGroundCloseToForeseenBattleGround
		{
			get
			{
				return this._highGroundCloseToForeseenBattleGround.Value;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00049B8E File Offset: 0x00047D8E
		public Vec2 HighGroundCloseToForeseenBattleGroundReadOnly
		{
			get
			{
				return this._highGroundCloseToForeseenBattleGround.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00049B9B File Offset: 0x00047D9B
		public bool IsUnderCavalryChargeFromFront
		{
			get
			{
				return this._isUnderCavalryChargeFromFront.Value;
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00049BA8 File Offset: 0x00047DA8
		public FormationQuerySystem(Formation formation)
		{
			FormationQuerySystem.<>c__DisplayClass236_0 CS$<>8__locals1 = new FormationQuerySystem.<>c__DisplayClass236_0();
			CS$<>8__locals1.formation = formation;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.Formation = CS$<>8__locals1.formation;
			Mission mission = Mission.Current;
			this._formationPower = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetFormationPower), 2.5f);
			this._formationMeleeFightingPower = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetFormationMeleeFightingPower), 2.5f);
			this._estimatedDirection = new QueryData<Vec2>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnitsWithoutDetachedOnes > 0)
				{
					Vec2 averagePositionOfUnits = CS$<>8__locals1.formation.GetAveragePositionOfUnits(true, true);
					float num = 0f;
					float num2 = 0f;
					Vec2 orderLocalAveragePosition = CS$<>8__locals1.formation.OrderLocalAveragePosition;
					int num3 = 0;
					foreach (IFormationUnit formationUnit in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
					{
						Agent agent = (Agent)formationUnit;
						Vec2? localPositionOfUnitOrDefault = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent);
						if (localPositionOfUnitOrDefault != null)
						{
							Vec2 value = localPositionOfUnitOrDefault.Value;
							Vec2 asVec = agent.Position.AsVec2;
							num += (value.x - orderLocalAveragePosition.x) * (asVec.x - averagePositionOfUnits.x) + (value.y - orderLocalAveragePosition.y) * (asVec.y - averagePositionOfUnits.y);
							num2 += (value.x - orderLocalAveragePosition.x) * (asVec.y - averagePositionOfUnits.y) - (value.y - orderLocalAveragePosition.y) * (asVec.x - averagePositionOfUnits.x);
							num3++;
						}
					}
					if (num3 > 0)
					{
						float num4 = 1f / (float)num3;
						num *= num4;
						num2 *= num4;
						float num5 = MathF.Sqrt(num * num + num2 * num2);
						if (num5 > 0f)
						{
							float num6 = MathF.Acos(MBMath.ClampFloat(num / num5, -1f, 1f));
							Vec2 vec = Vec2.FromRotation(num6);
							Vec2 vec2 = Vec2.FromRotation(-num6);
							float num7 = 0f;
							float num8 = 0f;
							foreach (IFormationUnit formationUnit2 in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
							{
								Agent agent2 = (Agent)formationUnit2;
								Vec2? localPositionOfUnitOrDefault2 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent2);
								if (localPositionOfUnitOrDefault2 != null)
								{
									Vec2 vec3 = vec.TransformToParentUnitF(localPositionOfUnitOrDefault2.Value - orderLocalAveragePosition);
									Vec2 vec4 = vec2.TransformToParentUnitF(localPositionOfUnitOrDefault2.Value - orderLocalAveragePosition);
									Vec2 asVec2 = agent2.Position.AsVec2;
									num7 += (vec3 - asVec2 + averagePositionOfUnits).LengthSquared;
									num8 += (vec4 - asVec2 + averagePositionOfUnits).LengthSquared;
								}
							}
							if (num7 >= num8)
							{
								return vec2;
							}
							return vec;
						}
					}
				}
				return new Vec2(0f, 1f);
			}, 0.2f);
			this._estimatedInterval = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnitsWithoutDetachedOnes > 0)
				{
					Vec2 estimatedDirection = CS$<>8__locals1.formation.QuerySystem.EstimatedDirection;
					Vec2 currentPosition = CS$<>8__locals1.formation.CurrentPosition;
					float num9 = 0f;
					float num10 = 0f;
					foreach (IFormationUnit formationUnit3 in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
					{
						Agent agent3 = (Agent)formationUnit3;
						Vec2? localPositionOfUnitOrDefault3 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent3);
						if (localPositionOfUnitOrDefault3 != null)
						{
							Vec2 vec5 = estimatedDirection.TransformToLocalUnitF(agent3.Position.AsVec2 - currentPosition);
							Vec2 vec6 = localPositionOfUnitOrDefault3.Value - vec5;
							Vec2 vec7 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent3, 1f).Value - localPositionOfUnitOrDefault3.Value;
							if (vec7.IsNonZero())
							{
								float num11 = vec7.Normalize();
								float num12 = Vec2.DotProduct(vec6, vec7);
								num9 += num12 * num11;
								num10 += num11 * num11;
							}
						}
					}
					if (num10 != 0f)
					{
						return Math.Max(0f, -num9 / num10 + CS$<>8__locals1.formation.Interval);
					}
				}
				return CS$<>8__locals1.formation.Interval;
			}, 0.2f);
			this._averageAllyPosition = new QueryData<Vec2>(delegate
			{
				int num13 = 0;
				Vec2 vec8 = Vec2.Zero;
				using (List<Team>.Enumerator enumerator3 = mission.Teams.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.IsFriendOf(CS$<>8__locals1.formation.Team))
						{
							foreach (Formation formation2 in CS$<>8__locals1.<>4__this.Formation.Team.FormationsIncludingSpecialAndEmpty)
							{
								if (formation2.CountOfUnits > 0 && formation2 != CS$<>8__locals1.formation)
								{
									num13 += formation2.CountOfUnits;
									vec8 += formation2.GetAveragePositionOfUnits(false, false) * (float)formation2.CountOfUnits;
								}
							}
						}
					}
				}
				if (num13 > 0)
				{
					return vec8 * (1f / (float)num13);
				}
				return CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition;
			}, 5f);
			this._idealAverageDisplacement = new QueryData<float>(() => MathF.Sqrt(CS$<>8__locals1.formation.Width * CS$<>8__locals1.formation.Width * 0.5f * 0.5f + CS$<>8__locals1.formation.Depth * CS$<>8__locals1.formation.Depth * 0.5f * 0.5f) / 2f, 5f);
			this._localAllyUnits = new QueryData<MBList<Agent>>(() => mission.GetNearbyAllyAgents(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, 30f, CS$<>8__locals1.formation.Team, CS$<>8__locals1.<>4__this._localAllyUnits.GetCachedValue()), 5f, new MBList<Agent>());
			this._localEnemyUnits = new QueryData<MBList<Agent>>(() => mission.GetNearbyEnemyAgents(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, 30f, CS$<>8__locals1.formation.Team, CS$<>8__locals1.<>4__this._localEnemyUnits.GetCachedValue()), 5f, new MBList<Agent>());
			this._infantryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Infantry, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._hasShieldUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsWithCondition(new Func<Agent, bool>(QueryLibrary.HasShield)) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._hasThrowingUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsWithCondition(new Func<Agent, bool>(QueryLibrary.HasThrown)) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._rangedUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Ranged, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._cavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Cavalry, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._rangedCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.HorseArcher, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._isMeleeFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.InfantryUnitRatio + CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio + CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isInfantryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.CavalryUnitRatio && CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._hasShield = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.HasShieldUnitRatio >= 0.4f, 5f);
			this._hasThrowing = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.HasThrowingUnitRatio >= 0.5f, 5f);
			this._isRangedFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.RangedUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.RangedUnitRatio >= CS$<>8__locals1.<>4__this.CavalryUnitRatio && CS$<>8__locals1.<>4__this.RangedUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isCavalryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.CavalryUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isRangedCavalryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.CavalryUnitRatio, 5f);
			QueryData<float>.SetupSyncGroup(new IQueryData[]
			{
				this._infantryUnitRatio, this._hasShieldUnitRatio, this._rangedUnitRatio, this._cavalryUnitRatio, this._rangedCavalryUnitRatio, this._isMeleeFormation, this._isInfantryFormation, this._hasShield, this._isRangedFormation, this._isCavalryFormation,
				this._isRangedCavalryFormation
			});
			this._movementSpeedMaximum = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetAverageMaximumMovementSpeedOfUnits), 10f);
			this._maximumMissileRange = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				float maximumRange = 0f;
				CS$<>8__locals1.formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					if (agent.MaximumMissileRange > maximumRange)
					{
						maximumRange = agent.MaximumMissileRange;
					}
				}, null);
				return maximumRange;
			}, 10f);
			this._missileRangeAdjusted = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				float sum = 0f;
				CS$<>8__locals1.formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					sum += agent.MissileRangeAdjusted;
				}, null);
				return sum / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 10f);
			this._localInfantryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsInfantry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localRangedUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsRanged)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsCavalry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localRangedCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsRangedCavalry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._localInfantryUnitRatio, this._localRangedUnitRatio, this._localCavalryUnitRatio, this._localRangedCavalryUnitRatio });
			this._localAllyPower = new QueryData<float>(() => CS$<>8__locals1.<>4__this.LocalAllyUnits.Sum<Agent>((Agent lau) => lau.CharacterPowerCached), 5f);
			this._localEnemyPower = new QueryData<float>(() => CS$<>8__locals1.<>4__this.LocalEnemyUnits.Sum<Agent>((Agent leu) => leu.CharacterPowerCached), 5f);
			this._localPowerRatio = new QueryData<float>(() => MBMath.ClampFloat(MathF.Sqrt((CS$<>8__locals1.<>4__this.LocalAllyUnits.Sum<Agent>((Agent lau) => lau.CharacterPowerCached) + 1f) * 1f / (CS$<>8__locals1.<>4__this.LocalEnemyUnits.Sum<Agent>((Agent leu) => leu.CharacterPowerCached) + 1f)), 0.5f, 1.75f), 5f);
			this._casualtyRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				CasualtyHandler missionBehavior = mission.GetMissionBehavior<CasualtyHandler>();
				int num14 = ((missionBehavior != null) ? missionBehavior.GetCasualtyCountOfFormation(CS$<>8__locals1.formation) : 0);
				return 1f - (float)num14 * 1f / (float)(num14 + CS$<>8__locals1.formation.CountOfUnits);
			}, 10f);
			this._isUnderRangedAttack = new QueryData<bool>(() => CS$<>8__locals1.formation.GetLastRecievedContactTypeOfUnits(5f) == Agent.LastRecievedAttackType.RangedContact, 3f);
			this._hasRecentlyTakenRangedDamage = new QueryData<bool>(() => CS$<>8__locals1.formation.HasUnitWithLastRecievedAttackType(Agent.LastRecievedAttackType.RangedHit, 10f), 3f);
			this._underRangedAttackRatio = new QueryData<float>(delegate
			{
				float currentTime = Mission.Current.CurrentTime;
				int countOfUnitsWithCondition = CS$<>8__locals1.formation.GetCountOfUnitsWithCondition((Agent agent) => currentTime - agent.LastRecievedRangedHitTime < 10f);
				if (CS$<>8__locals1.formation.CountOfUnits <= 0)
				{
					return 0f;
				}
				return (float)countOfUnitsWithCondition / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 3f);
			this._makingRangedAttackRatio = new QueryData<float>(delegate
			{
				float currentTime = Mission.Current.CurrentTime;
				int countOfUnitsWithCondition2 = CS$<>8__locals1.formation.GetCountOfUnitsWithCondition((Agent agent) => currentTime - agent.LastRangedHitTime < 10f);
				if (CS$<>8__locals1.formation.CountOfUnits <= 0)
				{
					return 0f;
				}
				return (float)countOfUnitsWithCondition2 / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 3f);
			this._closestEnemyAgent = new QueryData<Agent>(delegate
			{
				float num15 = float.MaxValue;
				Agent agent5 = null;
				foreach (Team team in mission.Teams)
				{
					if (team.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Agent agent6 in team.ActiveAgents)
						{
							float num16 = agent6.Position.DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f));
							if (num16 < num15)
							{
								num15 = num16;
								agent5 = agent6;
							}
						}
					}
				}
				return agent5;
			}, 1.5f);
			this._closestSignificantlyLargeEnemyFormation = new QueryData<Formation>(delegate
			{
				float num17 = float.MaxValue;
				Formation formation3 = null;
				float num18 = float.MaxValue;
				Formation formation4 = null;
				foreach (Team team2 in mission.Teams)
				{
					if (team2.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Formation formation5 in team2.FormationsIncludingSpecialAndEmpty)
						{
							if (formation5.CountOfUnits > 0)
							{
								if (formation5.QuerySystem.FormationPower / CS$<>8__locals1.<>4__this.FormationPower > 0.2f || formation5.QuerySystem.FormationPower * CS$<>8__locals1.<>4__this.Team.TeamPower / (formation5.Team.QuerySystem.TeamPower * CS$<>8__locals1.<>4__this.FormationPower) > 0.2f)
								{
									float num19 = formation5.CachedMedianPosition.GetNavMeshVec3MT().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZMT(), -1f));
									if (num19 < num17)
									{
										num17 = num19;
										formation3 = formation5;
									}
								}
								else if (formation3 == null)
								{
									float num20 = formation5.CachedMedianPosition.GetNavMeshVec3MT().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZMT(), -1f));
									if (num20 < num18)
									{
										num18 = num20;
										formation4 = formation5;
									}
								}
							}
						}
					}
				}
				return formation3 ?? formation4;
			}, 1.5f);
			this._fastestSignificantlyLargeEnemyFormation = new QueryData<Formation>(delegate
			{
				float num21 = float.MaxValue;
				Formation formation6 = null;
				float num22 = float.MaxValue;
				Formation formation7 = null;
				foreach (Team team3 in mission.Teams)
				{
					if (team3.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Formation formation8 in team3.FormationsIncludingSpecialAndEmpty)
						{
							if (formation8.CountOfUnits > 0)
							{
								if (formation8.QuerySystem.FormationPower / CS$<>8__locals1.<>4__this.FormationPower > 0.2f || formation8.QuerySystem.FormationPower * CS$<>8__locals1.<>4__this.Team.TeamPower / (formation8.Team.QuerySystem.TeamPower * CS$<>8__locals1.<>4__this.FormationPower) > 0.2f)
								{
									float num23 = formation8.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f)) / (formation8.CachedMovementSpeed * formation8.CachedMovementSpeed);
									if (num23 < num21)
									{
										num21 = num23;
										formation6 = formation8;
									}
								}
								else if (formation6 == null)
								{
									float num24 = formation8.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f)) / (formation8.CachedMovementSpeed * formation8.CachedMovementSpeed);
									if (num24 < num22)
									{
										num22 = num24;
										formation7 = formation8;
									}
								}
							}
						}
					}
				}
				return formation6 ?? formation7;
			}, 1.5f);
			this._mainClass = new QueryData<FormationClass>(delegate
			{
				FormationClass formationClass = FormationClass.Infantry;
				float num25 = CS$<>8__locals1.<>4__this.InfantryUnitRatio;
				if (CS$<>8__locals1.<>4__this.RangedUnitRatio > num25)
				{
					formationClass = FormationClass.Ranged;
					num25 = CS$<>8__locals1.<>4__this.RangedUnitRatio;
				}
				if (CS$<>8__locals1.<>4__this.CavalryUnitRatio > num25)
				{
					formationClass = FormationClass.Cavalry;
					num25 = CS$<>8__locals1.<>4__this.CavalryUnitRatio;
				}
				if (CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > num25)
				{
					formationClass = FormationClass.HorseArcher;
				}
				return formationClass;
			}, 15f);
			this._mainFormation = new QueryData<Formation>(delegate
			{
				IEnumerable<Formation> formationsIncludingSpecialAndEmpty = CS$<>8__locals1.formation.Team.FormationsIncludingSpecialAndEmpty;
				Func<Formation, bool> func;
				if ((func = CS$<>8__locals1.<>9__52) == null)
				{
					func = (CS$<>8__locals1.<>9__52 = (Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation && f != CS$<>8__locals1.formation);
				}
				return formationsIncludingSpecialAndEmpty.FirstOrDefault<Formation>(func);
			}, 15f);
			this._mainFormationReliabilityFactor = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.MainFormation == null)
				{
					return 0f;
				}
				float num26 = ((CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Charge || CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget || CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderRetreat) ? 0.5f : 1f);
				float num27 = (CS$<>8__locals1.<>4__this.MainFormation.HasUnitWithLastRecievedAttackType(Agent.LastRecievedAttackType.MeleeHit, 3f) ? 0.8f : 1f);
				return num26 * num27;
			}, 5f);
			this._weightedAverageEnemyPosition = new QueryData<Vec2>(() => CS$<>8__locals1.<>4__this.Formation.Team.GetWeightedAverageOfEnemies(CS$<>8__locals1.<>4__this.Formation.CurrentPosition), 0.5f);
			this._highGroundCloseToForeseenBattleGround = new QueryData<Vec2>(delegate
			{
				WorldPosition cachedMedianPosition = CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition);
				WorldPosition medianTargetFormationPosition = CS$<>8__locals1.<>4__this.Team.MedianTargetFormationPosition;
				return mission.FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref cachedMedianPosition, CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition.Distance(CS$<>8__locals1.<>4__this.Team.MedianTargetFormationPosition.AsVec2) * 0.5f, ref medianTargetFormationPosition).AsVec2;
			}, 10f);
			this._insideCastleUnitCountIncludingUnpositioned = new QueryData<int>(() => CS$<>8__locals1.<>4__this.Formation.CountUnitsOnNavMeshIDMod10(1, false), 3f);
			this._insideCastleUnitCountPositioned = new QueryData<int>(() => CS$<>8__locals1.<>4__this.Formation.CountUnitsOnNavMeshIDMod10(1, true), 3f);
			this._isUnderCavalryChargeFromFront = new QueryData<bool>(delegate
			{
				FormationQuerySystem closestSignificantlyLargeEnemyFormation = CS$<>8__locals1.<>4__this.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
				if (closestSignificantlyLargeEnemyFormation != null && closestSignificantlyLargeEnemyFormation.IsCavalryFormationReadOnly)
				{
					Vec2 cachedCurrentVelocity = closestSignificantlyLargeEnemyFormation.Formation.CachedCurrentVelocity;
					float num28 = cachedCurrentVelocity.Normalize();
					Vec2 vec9 = CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.AsVec2 - closestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2;
					float num29 = vec9.Normalize();
					bool flag = cachedCurrentVelocity.DotProduct(vec9) > 0.75f;
					bool flag2 = CS$<>8__locals1.<>4__this.Formation.Arrangement is CircularFormation || CS$<>8__locals1.<>4__this.Formation.Arrangement is SquareFormation || vec9.DotProduct(CS$<>8__locals1.<>4__this.Formation.Direction) < -0.75f;
					if (flag && flag2)
					{
						return num29 / num28 < 15f;
					}
				}
				return false;
			}, 2f);
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004A2A0 File Offset: 0x000484A0
		public void EvaluateAllPreliminaryQueryData()
		{
			float currentTime = Mission.Current.CurrentTime;
			this._infantryUnitRatio.Evaluate(currentTime);
			this._hasShieldUnitRatio.Evaluate(currentTime);
			this._rangedUnitRatio.Evaluate(currentTime);
			this._cavalryUnitRatio.Evaluate(currentTime);
			this._rangedCavalryUnitRatio.Evaluate(currentTime);
			this._isInfantryFormation.Evaluate(currentTime);
			this._hasShield.Evaluate(currentTime);
			this._isRangedFormation.Evaluate(currentTime);
			this._isCavalryFormation.Evaluate(currentTime);
			this._isRangedCavalryFormation.Evaluate(currentTime);
			this._isMeleeFormation.Evaluate(currentTime);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004A33C File Offset: 0x0004853C
		public void ForceExpireCavalryUnitRatio()
		{
			this._cavalryUnitRatio.Expire();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004A34C File Offset: 0x0004854C
		public void Expire()
		{
			this._formationPower.Expire();
			this._formationMeleeFightingPower.Expire();
			this._estimatedDirection.Expire();
			this._averageAllyPosition.Expire();
			this._idealAverageDisplacement.Expire();
			this._localAllyUnits.Expire();
			this._localEnemyUnits.Expire();
			this._mainClass.Expire();
			this._infantryUnitRatio.Expire();
			this._hasShieldUnitRatio.Expire();
			this._rangedUnitRatio.Expire();
			this._cavalryUnitRatio.Expire();
			this._rangedCavalryUnitRatio.Expire();
			this._isMeleeFormation.Expire();
			this._isInfantryFormation.Expire();
			this._hasShield.Expire();
			this._isRangedFormation.Expire();
			this._isCavalryFormation.Expire();
			this._isRangedCavalryFormation.Expire();
			this._movementSpeedMaximum.Expire();
			this._maximumMissileRange.Expire();
			this._missileRangeAdjusted.Expire();
			this._localInfantryUnitRatio.Expire();
			this._localRangedUnitRatio.Expire();
			this._localCavalryUnitRatio.Expire();
			this._localRangedCavalryUnitRatio.Expire();
			this._localAllyPower.Expire();
			this._localEnemyPower.Expire();
			this._localPowerRatio.Expire();
			this._casualtyRatio.Expire();
			this._isUnderRangedAttack.Expire();
			this._underRangedAttackRatio.Expire();
			this._makingRangedAttackRatio.Expire();
			this._mainFormation.Expire();
			this._mainFormationReliabilityFactor.Expire();
			this._weightedAverageEnemyPosition.Expire();
			this._closestSignificantlyLargeEnemyFormation.Expire();
			this._fastestSignificantlyLargeEnemyFormation.Expire();
			this._highGroundCloseToForeseenBattleGround.Expire();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0004A508 File Offset: 0x00048708
		public void ExpireAfterUnitAddRemove()
		{
			this._formationPower.Expire();
			float currentTime = Mission.Current.CurrentTime;
			this._infantryUnitRatio.Evaluate(currentTime);
			this._hasShieldUnitRatio.Evaluate(currentTime);
			this._rangedUnitRatio.Evaluate(currentTime);
			this._cavalryUnitRatio.Evaluate(currentTime);
			this._rangedCavalryUnitRatio.Evaluate(currentTime);
			this._isMeleeFormation.Evaluate(currentTime);
			this._isInfantryFormation.Evaluate(currentTime);
			this._hasShield.Evaluate(currentTime);
			this._isRangedFormation.Evaluate(currentTime);
			this._isCavalryFormation.Evaluate(currentTime);
			this._isRangedCavalryFormation.Evaluate(currentTime);
			this._mainClass.Evaluate(currentTime);
			if (this.Formation.CountOfUnits == 0)
			{
				this._infantryUnitRatio.SetValue(0f, currentTime);
				this._hasShieldUnitRatio.SetValue(0f, currentTime);
				this._rangedUnitRatio.SetValue(0f, currentTime);
				this._cavalryUnitRatio.SetValue(0f, currentTime);
				this._rangedCavalryUnitRatio.SetValue(0f, currentTime);
				this._isMeleeFormation.SetValue(false, currentTime);
				this._isInfantryFormation.SetValue(true, currentTime);
				this._hasShield.SetValue(false, currentTime);
				this._isRangedFormation.SetValue(false, currentTime);
				this._isCavalryFormation.SetValue(false, currentTime);
				this._isRangedCavalryFormation.SetValue(false, currentTime);
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0004A66E File Offset: 0x0004886E
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004A670 File Offset: 0x00048870
		public float GetClassWeightedFactor(float infantryWeight, float rangedWeight, float cavalryWeight, float rangedCavalryWeight)
		{
			return this.InfantryUnitRatio * infantryWeight + this.RangedUnitRatio * rangedWeight + this.CavalryUnitRatio * cavalryWeight + this.RangedCavalryUnitRatio * rangedCavalryWeight;
		}

		// Token: 0x04000510 RID: 1296
		public readonly Formation Formation;

		// Token: 0x04000511 RID: 1297
		private readonly QueryData<float> _formationPower;

		// Token: 0x04000512 RID: 1298
		private readonly QueryData<float> _formationMeleeFightingPower;

		// Token: 0x04000513 RID: 1299
		private readonly QueryData<Vec2> _estimatedDirection;

		// Token: 0x04000514 RID: 1300
		private readonly QueryData<float> _estimatedInterval;

		// Token: 0x04000515 RID: 1301
		private readonly QueryData<Vec2> _averageAllyPosition;

		// Token: 0x04000516 RID: 1302
		private readonly QueryData<float> _idealAverageDisplacement;

		// Token: 0x04000517 RID: 1303
		private readonly QueryData<MBList<Agent>> _localAllyUnits;

		// Token: 0x04000518 RID: 1304
		private readonly QueryData<MBList<Agent>> _localEnemyUnits;

		// Token: 0x04000519 RID: 1305
		private readonly QueryData<FormationClass> _mainClass;

		// Token: 0x0400051A RID: 1306
		private readonly QueryData<float> _infantryUnitRatio;

		// Token: 0x0400051B RID: 1307
		private readonly QueryData<float> _hasShieldUnitRatio;

		// Token: 0x0400051C RID: 1308
		private readonly QueryData<float> _hasThrowingUnitRatio;

		// Token: 0x0400051D RID: 1309
		private readonly QueryData<float> _rangedUnitRatio;

		// Token: 0x0400051E RID: 1310
		private readonly QueryData<int> _insideCastleUnitCountIncludingUnpositioned;

		// Token: 0x0400051F RID: 1311
		private readonly QueryData<int> _insideCastleUnitCountPositioned;

		// Token: 0x04000520 RID: 1312
		private readonly QueryData<float> _cavalryUnitRatio;

		// Token: 0x04000521 RID: 1313
		private readonly QueryData<float> _rangedCavalryUnitRatio;

		// Token: 0x04000522 RID: 1314
		private readonly QueryData<bool> _isMeleeFormation;

		// Token: 0x04000523 RID: 1315
		private readonly QueryData<bool> _isInfantryFormation;

		// Token: 0x04000524 RID: 1316
		private readonly QueryData<bool> _hasShield;

		// Token: 0x04000525 RID: 1317
		private readonly QueryData<bool> _hasThrowing;

		// Token: 0x04000526 RID: 1318
		private readonly QueryData<bool> _isRangedFormation;

		// Token: 0x04000527 RID: 1319
		private readonly QueryData<bool> _isCavalryFormation;

		// Token: 0x04000528 RID: 1320
		private readonly QueryData<bool> _isRangedCavalryFormation;

		// Token: 0x04000529 RID: 1321
		private readonly QueryData<float> _movementSpeedMaximum;

		// Token: 0x0400052A RID: 1322
		private readonly QueryData<float> _maximumMissileRange;

		// Token: 0x0400052B RID: 1323
		private readonly QueryData<float> _missileRangeAdjusted;

		// Token: 0x0400052C RID: 1324
		private readonly QueryData<float> _localInfantryUnitRatio;

		// Token: 0x0400052D RID: 1325
		private readonly QueryData<float> _localRangedUnitRatio;

		// Token: 0x0400052E RID: 1326
		private readonly QueryData<float> _localCavalryUnitRatio;

		// Token: 0x0400052F RID: 1327
		private readonly QueryData<float> _localRangedCavalryUnitRatio;

		// Token: 0x04000530 RID: 1328
		private readonly QueryData<float> _localAllyPower;

		// Token: 0x04000531 RID: 1329
		private readonly QueryData<float> _localEnemyPower;

		// Token: 0x04000532 RID: 1330
		private readonly QueryData<float> _localPowerRatio;

		// Token: 0x04000533 RID: 1331
		private readonly QueryData<float> _casualtyRatio;

		// Token: 0x04000534 RID: 1332
		private readonly QueryData<bool> _hasRecentlyTakenRangedDamage;

		// Token: 0x04000535 RID: 1333
		private readonly QueryData<bool> _isUnderRangedAttack;

		// Token: 0x04000536 RID: 1334
		private readonly QueryData<float> _underRangedAttackRatio;

		// Token: 0x04000537 RID: 1335
		private readonly QueryData<float> _makingRangedAttackRatio;

		// Token: 0x04000538 RID: 1336
		private readonly QueryData<Formation> _mainFormation;

		// Token: 0x04000539 RID: 1337
		private readonly QueryData<float> _mainFormationReliabilityFactor;

		// Token: 0x0400053A RID: 1338
		private readonly QueryData<Vec2> _weightedAverageEnemyPosition;

		// Token: 0x0400053B RID: 1339
		private readonly QueryData<Agent> _closestEnemyAgent;

		// Token: 0x0400053C RID: 1340
		private readonly QueryData<Formation> _closestSignificantlyLargeEnemyFormation;

		// Token: 0x0400053D RID: 1341
		private readonly QueryData<Formation> _fastestSignificantlyLargeEnemyFormation;

		// Token: 0x0400053E RID: 1342
		private readonly QueryData<Vec2> _highGroundCloseToForeseenBattleGround;

		// Token: 0x0400053F RID: 1343
		private readonly QueryData<bool> _isUnderCavalryChargeFromFront;
	}
}
