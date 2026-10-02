using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021D RID: 541
	public interface IMissionDeploymentPlan
	{
		// Token: 0x06001F8A RID: 8074
		void Initialize();

		// Token: 0x06001F8B RID: 8075
		void ClearAll();

		// Token: 0x06001F8C RID: 8076
		void MakeDefaultDeploymentPlans();

		// Token: 0x06001F8D RID: 8077
		void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetOffset = 0f);

		// Token: 0x06001F8E RID: 8078
		bool RemakeDeploymentPlan(Team team);

		// Token: 0x06001F8F RID: 8079
		void ClearDeploymentPlan(Team team);

		// Token: 0x06001F90 RID: 8080
		bool IsPlanMade(Team team);

		// Token: 0x06001F91 RID: 8081
		bool IsPlanMade(Team team, out bool isFirstPlan);

		// Token: 0x06001F92 RID: 8082
		bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position);

		// Token: 0x06001F93 RID: 8083
		bool HasDeploymentBoundaries(Team team);

		// Token: 0x06001F94 RID: 8084
		[return: TupleElementNames(new string[] { "id", "points" })]
		MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team);

		// Token: 0x06001F95 RID: 8085
		bool SupportsReinforcements();

		// Token: 0x06001F96 RID: 8086
		void UpdateReinforcementPlan(Team team);

		// Token: 0x06001F97 RID: 8087
		bool SupportsNavmesh(Team team);

		// Token: 0x06001F98 RID: 8088
		bool HasPlayerSpawnFrame(BattleSideEnum battleSide);

		// Token: 0x06001F99 RID: 8089
		bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction);

		// Token: 0x06001F9A RID: 8090
		Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position);

		// Token: 0x06001F9B RID: 8091
		void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition position);

		// Token: 0x06001F9C RID: 8092
		bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection);

		// Token: 0x06001F9D RID: 8093
		MatrixFrame GetDeploymentZoneFrame(Team team);

		// Token: 0x06001F9E RID: 8094
		IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement = false);

		// Token: 0x06001F9F RID: 8095
		MatrixFrame GetFormationsCenterFrameAndExtents(Team team, out Vec2 halfExtents, bool ignoreDimensionlessFormations = true);

		// Token: 0x06001FA0 RID: 8096
		float GetSpawnPathOffset(Team team);

		// Token: 0x06001FA1 RID: 8097
		MatrixFrame GetZoomFocusFrame(Team team);

		// Token: 0x06001FA2 RID: 8098
		float GetZoomOffset(Team team, float fovAngle);
	}
}
