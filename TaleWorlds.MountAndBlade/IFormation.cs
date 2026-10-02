using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000146 RID: 326
	public interface IFormation
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001020 RID: 4128
		float Interval { get; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001021 RID: 4129
		float Distance { get; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001022 RID: 4130
		float UnitDiameter { get; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001023 RID: 4131
		float MinimumInterval { get; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001024 RID: 4132
		float MaximumInterval { get; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001025 RID: 4133
		float MinimumDistance { get; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001026 RID: 4134
		float MaximumDistance { get; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001027 RID: 4135
		int? OverridenUnitCount { get; }

		// Token: 0x06001028 RID: 4136
		bool GetIsLocalPositionAvailable(Vec2 localPosition, Vec2? nearestAvailableUnitPositionLocal);

		// Token: 0x06001029 RID: 4137
		bool BatchUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, int fileCount, int rankCount);

		// Token: 0x0600102A RID: 4138
		IFormationUnit GetClosestUnitTo(Vec2 localPosition, MBList<IFormationUnit> unitsWithSpaces = null, float? maxDistance = null);

		// Token: 0x0600102B RID: 4139
		IFormationUnit GetClosestUnitTo(IFormationUnit targetUnit, MBList<IFormationUnit> unitsWithSpaces = null, float? maxDistance = null);

		// Token: 0x0600102C RID: 4140
		void OnUnitAddedOrRemoved();

		// Token: 0x0600102D RID: 4141
		void SetUnitToFollow(IFormationUnit unit, IFormationUnit toFollow, Vec2 vector);
	}
}
