using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D3 RID: 467
	public static class MBMapScene
	{
		// Token: 0x06001C13 RID: 7187 RVA: 0x00061373 File Offset: 0x0005F573
		public static Vec2 GetNearestFaceCenterForPosition(Scene mapScene, Vec2 position, bool isRegionMap0, int[] excludedFaceIds)
		{
			return MBAPI.IMBMapScene.GetNearestFaceCenterPositionForPosition(mapScene.Pointer, position.ToVec3(0f), isRegionMap0, excludedFaceIds, excludedFaceIds.Length, float.MaxValue);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0006139B File Offset: 0x0005F59B
		public static Vec2 GetNearestFaceCenterForPositionWithPath(Scene mapScene, PathFaceRecord pathFaceRecord, bool targetRegionMap0, float maxDist, int[] excludedFaceIds)
		{
			return MBAPI.IMBMapScene.GetNearestFaceCenterForPositionWithPath(mapScene.Pointer, pathFaceRecord.FaceIndex, targetRegionMap0, maxDist, excludedFaceIds, excludedFaceIds.Length);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000613BC File Offset: 0x0005F5BC
		public static Vec2 GetAccessiblePointNearPosition(Scene mapScene, Vec2 position, bool isRegionMap1, float radius)
		{
			return MBAPI.IMBMapScene.GetAccessiblePointNearPosition(mapScene.Pointer, position, isRegionMap1, radius).AsVec2;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000613E4 File Offset: 0x0005F5E4
		public static void RemoveZeroCornerBodies(Scene mapScene)
		{
			MBAPI.IMBMapScene.RemoveZeroCornerBodies(mapScene.Pointer);
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000613F6 File Offset: 0x0005F5F6
		public static void LoadAtmosphereData(Scene mapScene)
		{
			MBAPI.IMBMapScene.LoadAtmosphereData(mapScene.Pointer);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00061408 File Offset: 0x0005F608
		public static void TickStepSound(Scene mapScene, MBAgentVisuals visuals, int terrainType, TerrainTypeSoundSlot soundType, int partySize)
		{
			MBAPI.IMBMapScene.TickStepSound(mapScene.Pointer, visuals.Pointer, terrainType, soundType, partySize);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00061424 File Offset: 0x0005F624
		public static void TickAmbientSounds(Scene mapScene, int terrainType)
		{
			MBAPI.IMBMapScene.TickAmbientSounds(mapScene.Pointer, terrainType);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00061437 File Offset: 0x0005F637
		public static bool GetMouseVisible()
		{
			return MBAPI.IMBMapScene.GetMouseVisible();
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00061443 File Offset: 0x0005F643
		public static bool GetApplyRainColorGrade()
		{
			return MBMapScene.ApplyRainColorGrade;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0006144A File Offset: 0x0005F64A
		public static void SendMouseKeyEvent(int mouseKeyId, bool isDown)
		{
			MBAPI.IMBMapScene.SendMouseKeyEvent(mouseKeyId, isDown);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x00061458 File Offset: 0x0005F658
		public static void SetMousePos(int posX, int posY)
		{
			MBAPI.IMBMapScene.SetMousePos(posX, posY);
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00061468 File Offset: 0x0005F668
		public static void TickVisuals(Scene mapScene, float tod, Mesh[] tickedMapMeshes)
		{
			for (int i = 0; i < tickedMapMeshes.Length; i++)
			{
				MBMapScene._tickedMapMeshesCachedArray[i] = tickedMapMeshes[i].Pointer;
			}
			MBAPI.IMBMapScene.TickVisuals(mapScene.Pointer, tod, MBMapScene._tickedMapMeshesCachedArray, tickedMapMeshes.Length);
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000614AB File Offset: 0x0005F6AB
		public static void ValidateTerrainSoundIds()
		{
			MBAPI.IMBMapScene.ValidateTerrainSoundIds();
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000614B7 File Offset: 0x0005F6B7
		public static void GetGlobalIlluminationOfString(Scene mapScene, string value)
		{
			MBAPI.IMBMapScene.SetPoliticalColor(mapScene.Pointer, value);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000614CA File Offset: 0x0005F6CA
		public static void GetColorGradeGridData(Scene mapScene, byte[] gridData, string textureName)
		{
			MBAPI.IMBMapScene.GetColorGradeGridData(mapScene.Pointer, gridData, textureName);
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000614E0 File Offset: 0x0005F6E0
		public static void GetBattleSceneIndexMap(Scene mapScene, ref byte[] indexData, ref int width, ref int height)
		{
			MBAPI.IMBMapScene.GetBattleSceneIndexMapResolution(mapScene.Pointer, ref width, ref height);
			int num = width * height * 2;
			if (indexData == null || indexData.Length != num)
			{
				indexData = new byte[num];
			}
			MBAPI.IMBMapScene.GetBattleSceneIndexMap(mapScene.Pointer, indexData);
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0006152C File Offset: 0x0005F72C
		public static void SetFrameForAtmosphere(Scene mapScene, float tod, float cameraElevation, bool forceLoadTextures)
		{
			MBAPI.IMBMapScene.SetFrameForAtmosphere(mapScene.Pointer, tod, cameraElevation, forceLoadTextures);
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00061541 File Offset: 0x0005F741
		public static void SetTerrainDynamicParams(Scene mapScene, Vec3 dynamic_params)
		{
			MBAPI.IMBMapScene.SetTerrainDynamicParams(mapScene.Pointer, dynamic_params);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x00061554 File Offset: 0x0005F754
		public static void SetSeasonTimeFactor(Scene mapScene, float seasonTimeFactor)
		{
			MBAPI.IMBMapScene.SetSeasonTimeFactor(mapScene.Pointer, seasonTimeFactor);
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00061567 File Offset: 0x0005F767
		public static float GetSeasonTimeFactor(Scene mapScene)
		{
			return MBAPI.IMBMapScene.GetSeasonTimeFactor(mapScene.Pointer);
		}

		// Token: 0x0400092D RID: 2349
		public static bool ApplyRainColorGrade;

		// Token: 0x0400092E RID: 2350
		private static UIntPtr[] _tickedMapMeshesCachedArray = new UIntPtr[1024];
	}
}
