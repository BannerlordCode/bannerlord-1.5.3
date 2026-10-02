using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C0 RID: 448
	[ScriptingInterfaceBase]
	internal interface IMBMapScene
	{
		// Token: 0x06001958 RID: 6488
		[EngineMethod("get_accessible_point_near_position", false, null, false)]
		Vec3 GetAccessiblePointNearPosition(UIntPtr scenePointer, Vec2 position, bool isRegionMap0, float radius);

		// Token: 0x06001959 RID: 6489
		[EngineMethod("get_nearest_nav_mesh_face_center_position_for_position", false, null, false)]
		Vec2 GetNearestFaceCenterPositionForPosition(UIntPtr scenePointer, Vec3 position, bool isRegionMap0, int[] excludedFaceIds, int excludedFaceIdCount, float heightLimit);

		// Token: 0x0600195A RID: 6490
		[EngineMethod("get_nearest_nav_mesh_face_center_position_between_regions_using_path", false, null, false)]
		Vec2 GetNearestFaceCenterForPositionWithPath(UIntPtr scenePointer, int startFaceIndex, bool targetRegionMap0, float distMax, int[] excludedFaceIds, int excludedFaceIdCount);

		// Token: 0x0600195B RID: 6491
		[EngineMethod("remove_zero_corner_bodies", false, null, false)]
		void RemoveZeroCornerBodies(UIntPtr scenePointer);

		// Token: 0x0600195C RID: 6492
		[EngineMethod("load_atmosphere_data", false, null, false)]
		void LoadAtmosphereData(UIntPtr scenePointer);

		// Token: 0x0600195D RID: 6493
		[EngineMethod("tick_step_sound", false, null, false)]
		void TickStepSound(UIntPtr scenePointer, UIntPtr visualsPointer, int faceIndexTerrainType, TerrainTypeSoundSlot soundType, int partySize);

		// Token: 0x0600195E RID: 6494
		[EngineMethod("tick_ambient_sounds", false, null, false)]
		void TickAmbientSounds(UIntPtr scenePointer, int terrainType);

		// Token: 0x0600195F RID: 6495
		[EngineMethod("tick_visuals", false, null, false)]
		void TickVisuals(UIntPtr scenePointer, float tod, UIntPtr[] ticked_map_meshes, int tickedMapMeshesCount);

		// Token: 0x06001960 RID: 6496
		[EngineMethod("validate_terrain_sound_ids", false, null, false)]
		void ValidateTerrainSoundIds();

		// Token: 0x06001961 RID: 6497
		[EngineMethod("set_political_color", false, null, false)]
		void SetPoliticalColor(UIntPtr scenePointer, string value);

		// Token: 0x06001962 RID: 6498
		[EngineMethod("set_frame_for_atmosphere", false, null, false)]
		void SetFrameForAtmosphere(UIntPtr scenePointer, float tod, float cameraElevation, bool forceLoadTextures);

		// Token: 0x06001963 RID: 6499
		[EngineMethod("get_color_grade_grid_data", false, null, false)]
		void GetColorGradeGridData(UIntPtr scenePointer, byte[] snowData, string textureName);

		// Token: 0x06001964 RID: 6500
		[EngineMethod("get_battle_scene_index_map_resolution", false, null, false)]
		void GetBattleSceneIndexMapResolution(UIntPtr scenePointer, ref int width, ref int height);

		// Token: 0x06001965 RID: 6501
		[EngineMethod("get_battle_scene_index_map", false, null, false)]
		void GetBattleSceneIndexMap(UIntPtr scenePointer, byte[] indexData);

		// Token: 0x06001966 RID: 6502
		[EngineMethod("set_terrain_dynamic_params", false, null, false)]
		void SetTerrainDynamicParams(UIntPtr scenePointer, Vec3 dynamic_params);

		// Token: 0x06001967 RID: 6503
		[EngineMethod("set_season_time_factor", false, null, false)]
		void SetSeasonTimeFactor(UIntPtr scenePointer, float seasonTimeFactor);

		// Token: 0x06001968 RID: 6504
		[EngineMethod("get_season_time_factor", false, null, false)]
		float GetSeasonTimeFactor(UIntPtr scenePointer);

		// Token: 0x06001969 RID: 6505
		[EngineMethod("get_mouse_visible", false, null, false)]
		bool GetMouseVisible();

		// Token: 0x0600196A RID: 6506
		[EngineMethod("send_mouse_key_down_event", false, null, false)]
		void SendMouseKeyEvent(int keyId, bool isDown);

		// Token: 0x0600196B RID: 6507
		[EngineMethod("set_mouse_visible", false, null, false)]
		void SetMouseVisible(bool value);

		// Token: 0x0600196C RID: 6508
		[EngineMethod("set_mouse_pos", false, null, false)]
		void SetMousePos(int posX, int posY);
	}
}
