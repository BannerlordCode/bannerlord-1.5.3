using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000030 RID: 48
	[ApplicationInterfaceBase]
	internal interface ISceneView
	{
		// Token: 0x0600051A RID: 1306
		[EngineMethod("create_scene_view", false, null, false)]
		SceneView CreateSceneView();

		// Token: 0x0600051B RID: 1307
		[EngineMethod("set_scene", false, null, false)]
		void SetScene(UIntPtr ptr, UIntPtr scenePtr);

		// Token: 0x0600051C RID: 1308
		[EngineMethod("set_accept_global_debug_render_objects", false, null, false)]
		void SetAcceptGlobalDebugRenderObjects(UIntPtr ptr, bool value);

		// Token: 0x0600051D RID: 1309
		[EngineMethod("set_render_with_postfx", false, null, false)]
		void SetRenderWithPostfx(UIntPtr ptr, bool value);

		// Token: 0x0600051E RID: 1310
		[EngineMethod("set_force_shader_compilation", false, null, false)]
		void SetForceShaderCompilation(UIntPtr ptr, bool value);

		// Token: 0x0600051F RID: 1311
		[EngineMethod("check_scene_ready_to_render", false, null, false)]
		bool CheckSceneReadyToRender(UIntPtr ptr);

		// Token: 0x06000520 RID: 1312
		[EngineMethod("set_do_quick_exposure", false, null, false)]
		void SetDoQuickExposure(UIntPtr ptr, bool value);

		// Token: 0x06000521 RID: 1313
		[EngineMethod("set_postfx_config_params", false, null, false)]
		void SetPostfxConfigParams(UIntPtr ptr, int value);

		// Token: 0x06000522 RID: 1314
		[EngineMethod("set_camera", false, null, false)]
		void SetCamera(UIntPtr ptr, UIntPtr cameraPtr);

		// Token: 0x06000523 RID: 1315
		[EngineMethod("set_resolution_scaling", false, null, false)]
		void SetResolutionScaling(UIntPtr ptr, bool value);

		// Token: 0x06000524 RID: 1316
		[EngineMethod("set_postfx_from_config", false, null, false)]
		void SetPostfxFromConfig(UIntPtr ptr);

		// Token: 0x06000525 RID: 1317
		[EngineMethod("world_point_to_screen_point", false, null, false)]
		Vec2 WorldPointToScreenPoint(UIntPtr ptr, Vec3 position);

		// Token: 0x06000526 RID: 1318
		[EngineMethod("screen_point_to_viewport_point", false, null, false)]
		Vec2 ScreenPointToViewportPoint(UIntPtr ptr, float position_x, float position_y);

		// Token: 0x06000527 RID: 1319
		[EngineMethod("projected_mouse_position_on_ground", false, null, false)]
		bool ProjectedMousePositionOnGround(UIntPtr pointer, out Vec3 groundPosition, out Vec3 groundNormal, bool mouseVisible, BodyFlags excludeBodyOwnerFlags, bool checkOccludedSurface);

		// Token: 0x06000528 RID: 1320
		[EngineMethod("projected_mouse_position_on_water", false, null, false)]
		bool ProjectedMousePositionOnWater(UIntPtr pointer, out Vec3 groundPosition, bool mouseVisible);

		// Token: 0x06000529 RID: 1321
		[EngineMethod("translate_mouse", false, null, false)]
		void TranslateMouse(UIntPtr pointer, ref Vec3 worldMouseNear, ref Vec3 worldMouseFar, float maxDistance);

		// Token: 0x0600052A RID: 1322
		[EngineMethod("set_scene_uses_skybox", false, null, false)]
		void SetSceneUsesSkybox(UIntPtr pointer, bool value);

		// Token: 0x0600052B RID: 1323
		[EngineMethod("set_scene_uses_shadows", false, null, false)]
		void SetSceneUsesShadows(UIntPtr pointer, bool value);

		// Token: 0x0600052C RID: 1324
		[EngineMethod("set_scene_uses_contour", false, null, false)]
		void SetSceneUsesContour(UIntPtr pointer, bool value);

		// Token: 0x0600052D RID: 1325
		[EngineMethod("do_not_clear", false, null, false)]
		void DoNotClear(UIntPtr pointer, bool value);

		// Token: 0x0600052E RID: 1326
		[EngineMethod("add_clear_task", false, null, false)]
		void AddClearTask(UIntPtr ptr, bool clearOnlySceneview);

		// Token: 0x0600052F RID: 1327
		[EngineMethod("ready_to_render", false, null, false)]
		bool ReadyToRender(UIntPtr pointer);

		// Token: 0x06000530 RID: 1328
		[EngineMethod("set_clear_and_disable_after_succesfull_render", false, null, false)]
		void SetClearAndDisableAfterSucessfullRender(UIntPtr pointer, bool value);

		// Token: 0x06000531 RID: 1329
		[EngineMethod("set_clear_gbuffer", false, null, false)]
		void SetClearGbuffer(UIntPtr pointer, bool value);

		// Token: 0x06000532 RID: 1330
		[EngineMethod("set_shadowmap_resolution_multiplier", false, null, false)]
		void SetShadowmapResolutionMultiplier(UIntPtr pointer, float value);

		// Token: 0x06000533 RID: 1331
		[EngineMethod("set_pointlight_resolution_multiplier", false, null, false)]
		void SetPointlightResolutionMultiplier(UIntPtr pointer, float value);

		// Token: 0x06000534 RID: 1332
		[EngineMethod("set_clean_screen_until_loading_done", false, null, false)]
		void SetCleanScreenUntilLoadingDone(UIntPtr pointer, bool value);

		// Token: 0x06000535 RID: 1333
		[EngineMethod("clear_all", false, null, false)]
		void ClearAll(UIntPtr pointer, bool clear_scene, bool remove_terrain);

		// Token: 0x06000536 RID: 1334
		[EngineMethod("set_focused_shadowmap", false, null, false)]
		void SetFocusedShadowmap(UIntPtr ptr, bool enable, ref Vec3 center, float radius);

		// Token: 0x06000537 RID: 1335
		[EngineMethod("get_scene", false, null, false)]
		Scene GetScene(UIntPtr ptr);

		// Token: 0x06000538 RID: 1336
		[EngineMethod("ray_cast_for_closest_entity_or_terrain", false, null, false)]
		bool RayCastForClosestEntityOrTerrain(UIntPtr ptr, ref Vec3 sourcePoint, ref Vec3 targetPoint, float rayThickness, ref float collisionDistance, ref Vec3 closestPoint, ref UIntPtr entityIndex, BodyFlags bodyExcludeFlags);
	}
}
