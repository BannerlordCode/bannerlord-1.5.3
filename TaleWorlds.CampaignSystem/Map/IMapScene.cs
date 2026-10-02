using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000228 RID: 552
	public interface IMapScene
	{
		// Token: 0x0600214E RID: 8526
		void Load();

		// Token: 0x0600214F RID: 8527
		void AfterLoad();

		// Token: 0x06002150 RID: 8528
		void Destroy();

		// Token: 0x06002151 RID: 8529
		PathFaceRecord GetFaceIndex(in CampaignVec2 vec2);

		// Token: 0x06002152 RID: 8530
		TerrainType GetTerrainTypeAtPosition(in CampaignVec2 vec2);

		// Token: 0x06002153 RID: 8531
		List<TerrainType> GetEnvironmentTerrainTypes(in CampaignVec2 vec2);

		// Token: 0x06002154 RID: 8532
		List<TerrainType> GetEnvironmentTerrainTypesCount(in CampaignVec2 vec2, out TerrainType currentPositionTerrainType);

		// Token: 0x06002155 RID: 8533
		MapPatchData GetMapPatchAtPosition(in CampaignVec2 position);

		// Token: 0x06002156 RID: 8534
		TerrainType GetFaceTerrainType(PathFaceRecord faceIndex);

		// Token: 0x06002157 RID: 8535
		CampaignVec2 GetNearestFaceCenterForPosition(in CampaignVec2 vec2, int[] excludedFaceIds);

		// Token: 0x06002158 RID: 8536
		CampaignVec2 GetNearestFaceCenterForPositionWithPath(PathFaceRecord pathFaceRecord, bool targetIsLand, float maxDist, int[] excludedFaceIds);

		// Token: 0x06002159 RID: 8537
		CampaignVec2 GetAccessiblePointNearPosition(in CampaignVec2 vec2, float radius);

		// Token: 0x0600215A RID: 8538
		bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds, float extraCostMultiplier, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand);

		// Token: 0x0600215B RID: 8539
		bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds, float extraCostMultiplier, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand, int excludedFaceIndex);

		// Token: 0x0600215C RID: 8540
		bool GetPathDistanceBetweenAIFaces(PathFaceRecord startingAiFace, PathFaceRecord endingAiFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, float distanceLimit, out float distance, int[] excludedFaceIds, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand);

		// Token: 0x0600215D RID: 8541
		bool IsLineToPointClear(PathFaceRecord startingFace, Vec2 position, Vec2 destination, float agentRadius);

		// Token: 0x0600215E RID: 8542
		Vec2 GetLastPointOnNavigationMeshFromPositionToDestination(PathFaceRecord startingFace, Vec2 position, Vec2 destination, int[] excludedFaceIds = null);

		// Token: 0x0600215F RID: 8543
		Vec2 GetLastPositionOnNavMeshFaceForPointAndDirection(PathFaceRecord startingFace, Vec2 position, Vec2 destination);

		// Token: 0x06002160 RID: 8544
		Vec2 GetNavigationMeshCenterPosition(PathFaceRecord face);

		// Token: 0x06002161 RID: 8545
		Vec2 GetNavigationMeshCenterPosition(int faceIndex);

		// Token: 0x06002162 RID: 8546
		PathFaceRecord GetFaceAtIndex(int faceIndex);

		// Token: 0x06002163 RID: 8547
		int GetNumberOfNavigationMeshFaces();

		// Token: 0x06002164 RID: 8548
		bool GetHeightAtPoint(in CampaignVec2 point, ref float height);

		// Token: 0x06002165 RID: 8549
		float GetWinterTimeFactor();

		// Token: 0x06002166 RID: 8550
		void GetTerrainHeightAndNormal(Vec2 position, out float height, out Vec3 normal);

		// Token: 0x06002167 RID: 8551
		float GetFaceVertexZ(PathFaceRecord navMeshFace);

		// Token: 0x06002168 RID: 8552
		Vec3 GetGroundNormal(Vec2 position);

		// Token: 0x06002169 RID: 8553
		void GetSiegeCampFrames(Settlement settlement, out List<MatrixFrame> siegeCamp1GlobalFrames, out List<MatrixFrame> siegeCamp2GlobalFrames);

		// Token: 0x0600216A RID: 8554
		string GetTerrainTypeName(TerrainType type);

		// Token: 0x0600216B RID: 8555
		Vec2 GetTerrainSize();

		// Token: 0x0600216C RID: 8556
		uint GetSceneLevel(string name);

		// Token: 0x0600216D RID: 8557
		void SetSceneLevels(List<string> levels);

		// Token: 0x0600216E RID: 8558
		List<AtmosphereState> GetAtmosphereStates();

		// Token: 0x0600216F RID: 8559
		void SetAtmosphereColorgrade(TerrainType terrainType);

		// Token: 0x06002170 RID: 8560
		void AddNewEntityToMapScene(string entityId, in CampaignVec2 position);

		// Token: 0x06002171 RID: 8561
		void GetMapBorders(out Vec2 minimumPosition, out Vec2 maximumPosition, out float maximumHeight);

		// Token: 0x06002172 RID: 8562
		uint GetSceneXmlCrc();

		// Token: 0x06002173 RID: 8563
		uint GetSceneNavigationMeshCrc();

		// Token: 0x06002174 RID: 8564
		float GetSnowAmountAtPosition(Vec2 position);

		// Token: 0x06002175 RID: 8565
		float GetRainAmountAtPosition(Vec2 position);
	}
}
