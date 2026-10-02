using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000147 RID: 327
	public interface IFormationArrangement
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600102E RID: 4142
		// (set) Token: 0x0600102F RID: 4143
		float Width { get; set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001030 RID: 4144
		float Depth { get; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001031 RID: 4145
		// (set) Token: 0x06001032 RID: 4146
		float FlankWidth { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001033 RID: 4147
		float RankDepth { get; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001034 RID: 4148
		float MinimumWidth { get; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001035 RID: 4149
		float MaximumWidth { get; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001036 RID: 4150
		float MinimumFlankWidth { get; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001037 RID: 4151
		bool? IsLoose { get; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001038 RID: 4152
		float IntervalMultiplier { get; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001039 RID: 4153
		float DistanceMultiplier { get; }

		// Token: 0x0600103A RID: 4154
		IFormationUnit GetPlayerUnit();

		// Token: 0x0600103B RID: 4155
		MBReadOnlyList<IFormationUnit> GetAllUnits();

		// Token: 0x0600103C RID: 4156
		void GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn);

		// Token: 0x0600103D RID: 4157
		MBList<IFormationUnit> GetUnpositionedUnits();

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600103E RID: 4158
		int UnitCount { get; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600103F RID: 4159
		int RankCount { get; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001040 RID: 4160
		int PositionedUnitCount { get; }

		// Token: 0x06001041 RID: 4161
		bool AddUnit(IFormationUnit unit);

		// Token: 0x06001042 RID: 4162
		void RemoveUnit(IFormationUnit unit);

		// Token: 0x06001043 RID: 4163
		IFormationUnit GetUnit(int fileIndex, int rankIndex);

		// Token: 0x06001044 RID: 4164
		void OnBatchRemoveStart();

		// Token: 0x06001045 RID: 4165
		void OnBatchRemoveEnd();

		// Token: 0x06001046 RID: 4166
		Vec2? GetLocalPositionOfUnitOrDefault(int unitIndex);

		// Token: 0x06001047 RID: 4167
		Vec2? GetLocalPositionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x06001048 RID: 4168
		Vec2? GetLocalPositionOfUnitOrDefaultWithAdjustment(IFormationUnit unit, float distanceBetweenAgentsAdjustment);

		// Token: 0x06001049 RID: 4169
		Vec2? GetLocalDirectionOfUnitOrDefault(int unitIndex);

		// Token: 0x0600104A RID: 4170
		Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x0600104B RID: 4171
		WorldPosition? GetWorldPositionOfUnitOrDefault(int unitIndex);

		// Token: 0x0600104C RID: 4172
		WorldPosition? GetWorldPositionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x0600104D RID: 4173
		List<IFormationUnit> GetUnitsToPop(int count);

		// Token: 0x0600104E RID: 4174
		List<IFormationUnit> GetUnitsToPop(int count, Vec3 targetPosition);

		// Token: 0x0600104F RID: 4175
		IEnumerable<IFormationUnit> GetUnitsToPopWithCondition(int count, Func<IFormationUnit, bool> conditionFunction);

		// Token: 0x06001050 RID: 4176
		void SwitchUnitLocations(IFormationUnit firstUnit, IFormationUnit secondUnit);

		// Token: 0x06001051 RID: 4177
		void SwitchUnitLocationsWithUnpositionedUnit(IFormationUnit firstUnit, IFormationUnit secondUnit);

		// Token: 0x06001052 RID: 4178
		void SwitchUnitLocationsWithBackMostUnit(IFormationUnit unit);

		// Token: 0x06001053 RID: 4179
		IFormationUnit GetNeighborUnitOfLeftSide(IFormationUnit unit);

		// Token: 0x06001054 RID: 4180
		IFormationUnit GetNeighborUnitOfRightSide(IFormationUnit unit);

		// Token: 0x06001055 RID: 4181
		Vec2? GetLocalWallDirectionOfRelativeFormationLocation(IFormationUnit unit);

		// Token: 0x06001056 RID: 4182
		IEnumerable<Vec2> GetUnavailableUnitPositions();

		// Token: 0x06001057 RID: 4183
		float GetOccupationWidth(int unitCount);

		// Token: 0x06001058 RID: 4184
		Vec2? CreateNewPosition(int unitIndex);

		// Token: 0x06001059 RID: 4185
		void BeforeFormationFrameChange();

		// Token: 0x0600105A RID: 4186
		void OnFormationFrameChanged(bool updateCachedOrderedLocalPositions = false);

		// Token: 0x0600105B RID: 4187
		bool IsTurnBackwardsNecessary(Vec2 previousPosition, WorldPosition? newPosition, Vec2 previousDirection, bool hasNewDirection, Vec2? newDirection);

		// Token: 0x0600105C RID: 4188
		void TurnBackwards();

		// Token: 0x0600105D RID: 4189
		void OnFormationDispersed();

		// Token: 0x0600105E RID: 4190
		void Reset();

		// Token: 0x0600105F RID: 4191
		IFormationArrangement Clone(IFormation formation);

		// Token: 0x06001060 RID: 4192
		void DeepCopyFrom(IFormationArrangement arrangement);

		// Token: 0x06001061 RID: 4193
		void RearrangeTo(IFormationArrangement arrangement);

		// Token: 0x06001062 RID: 4194
		void RearrangeFrom(IFormationArrangement arrangement);

		// Token: 0x06001063 RID: 4195
		void RearrangeTransferUnits(IFormationArrangement arrangement);

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001064 RID: 4196
		// (remove) Token: 0x06001065 RID: 4197
		event Action OnWidthChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06001066 RID: 4198
		// (remove) Token: 0x06001067 RID: 4199
		event Action OnShapeChanged;

		// Token: 0x06001068 RID: 4200
		void ReserveMiddleFrontUnitPosition(IFormationUnit vanguard);

		// Token: 0x06001069 RID: 4201
		void ReleaseMiddleFrontUnitPosition();

		// Token: 0x0600106A RID: 4202
		Vec2 GetLocalPositionOfReservedUnitPosition();

		// Token: 0x0600106B RID: 4203
		void OnUnitLostMount(IFormationUnit unit);

		// Token: 0x0600106C RID: 4204
		float GetDirectionChangeTendencyOfUnit(IFormationUnit unit);

		// Token: 0x0600106D RID: 4205
		void UpdateLocalPositionErrors(bool recalculateErrors = true);

		// Token: 0x0600106E RID: 4206
		void OnTickOccasionally();

		// Token: 0x170003B1 RID: 945
		// (set) Token: 0x0600106F RID: 4207
		bool AreLocalPositionsDirty { set; }
	}
}
