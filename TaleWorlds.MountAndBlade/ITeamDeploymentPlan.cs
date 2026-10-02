using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021E RID: 542
	public interface ITeamDeploymentPlan
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001FA3 RID: 8099
		Team Team { get; }

		// Token: 0x06001FA4 RID: 8100
		void MakeDeploymentPlan(float spawnPathOffset = 0f, float targetOffset = 0f, FormationSceneSpawnEntry[,] formationSceneSpawnEntries = null, bool isReinforcement = false);

		// Token: 0x06001FA5 RID: 8101
		void ClearPlan(bool isReinforcement = false);

		// Token: 0x06001FA6 RID: 8102
		bool IsFirstPlan(bool isReinforcement = false);

		// Token: 0x06001FA7 RID: 8103
		bool IsPlanMade(bool isReinforcement = false);

		// Token: 0x06001FA8 RID: 8104
		[return: TupleElementNames(new string[] { "id", "points" })]
		MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries();

		// Token: 0x06001FA9 RID: 8105
		float GetSpawnPathOffset(bool isReinforcement = false);

		// Token: 0x06001FAA RID: 8106
		float GetTargetOffset(bool isReinforcement = false);

		// Token: 0x06001FAB RID: 8107
		MatrixFrame GetDeploymentZoneFrame();

		// Token: 0x06001FAC RID: 8108
		bool HasDeploymentBoundaries();

		// Token: 0x06001FAD RID: 8109
		IFormationDeploymentPlan GetFormationPlan(FormationClass formationIndex, bool isReinforcement = false);

		// Token: 0x06001FAE RID: 8110
		Vec3 GetMeanPosition(bool isReinforcement = false);

		// Token: 0x06001FAF RID: 8111
		bool IsPositionInsideDeploymentBoundaries(in Vec2 position, [TupleElementNames(new string[] { "id", "points" })] out ValueTuple<string, MBList<Vec2>> containingBoundaryTuple);

		// Token: 0x06001FB0 RID: 8112
		Vec2 GetClosestDeploymentBoundaryPosition(in Vec2 position);
	}
}
