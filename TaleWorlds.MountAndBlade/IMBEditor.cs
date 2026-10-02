using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B0 RID: 432
	[ScriptingInterfaceBase]
	internal interface IMBEditor
	{
		// Token: 0x06001839 RID: 6201
		[EngineMethod("is_edit_mode", false, null, false)]
		bool IsEditMode();

		// Token: 0x0600183A RID: 6202
		[EngineMethod("is_edit_mode_enabled", false, null, false)]
		bool IsEditModeEnabled();

		// Token: 0x0600183B RID: 6203
		[EngineMethod("update_scene_tree", false, null, false)]
		void UpdateSceneTree(bool do_next_frame);

		// Token: 0x0600183C RID: 6204
		[EngineMethod("is_entity_selected", false, null, false)]
		bool IsEntitySelected(UIntPtr entityId);

		// Token: 0x0600183D RID: 6205
		[EngineMethod("add_editor_warning", false, null, false)]
		void AddEditorWarning(string msg);

		// Token: 0x0600183E RID: 6206
		[EngineMethod("render_editor_mesh", false, null, false)]
		void RenderEditorMesh(UIntPtr metaMeshId, ref MatrixFrame frame);

		// Token: 0x0600183F RID: 6207
		[EngineMethod("apply_delta_to_editor_camera", false, null, false)]
		void ApplyDeltaToEditorCamera(in Vec3 delta);

		// Token: 0x06001840 RID: 6208
		[EngineMethod("enter_edit_mode", false, null, false)]
		void EnterEditMode(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing);

		// Token: 0x06001841 RID: 6209
		[EngineMethod("tick_edit_mode", false, null, false)]
		void TickEditMode(float dt);

		// Token: 0x06001842 RID: 6210
		[EngineMethod("leave_edit_mode", false, null, false)]
		void LeaveEditMode();

		// Token: 0x06001843 RID: 6211
		[EngineMethod("enter_edit_mission_mode", false, null, false)]
		void EnterEditMissionMode(UIntPtr missionPointer);

		// Token: 0x06001844 RID: 6212
		[EngineMethod("leave_edit_mission_mode", false, null, false)]
		void LeaveEditMissionMode();

		// Token: 0x06001845 RID: 6213
		[EngineMethod("activate_scene_editor_presentation", false, null, false)]
		void ActivateSceneEditorPresentation();

		// Token: 0x06001846 RID: 6214
		[EngineMethod("deactivate_scene_editor_presentation", false, null, false)]
		void DeactivateSceneEditorPresentation();

		// Token: 0x06001847 RID: 6215
		[EngineMethod("tick_scene_editor_presentation", false, null, false)]
		void TickSceneEditorPresentation(float dt);

		// Token: 0x06001848 RID: 6216
		[EngineMethod("get_editor_scene_view", false, null, false)]
		SceneView GetEditorSceneView();

		// Token: 0x06001849 RID: 6217
		[EngineMethod("helpers_enabled", false, null, false)]
		bool HelpersEnabled();

		// Token: 0x0600184A RID: 6218
		[EngineMethod("border_helpers_enabled", false, null, false)]
		bool BorderHelpersEnabled();

		// Token: 0x0600184B RID: 6219
		[EngineMethod("zoom_to_position", false, null, false)]
		void ZoomToPosition(Vec3 pos);

		// Token: 0x0600184C RID: 6220
		[EngineMethod("add_entity_warning", false, null, false)]
		void AddEntityWarning(UIntPtr entityId, string msg);

		// Token: 0x0600184D RID: 6221
		[EngineMethod("add_nav_mesh_warning", false, null, false)]
		void AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg);

		// Token: 0x0600184E RID: 6222
		[EngineMethod("get_all_prefabs_and_child_with_tag", false, null, false)]
		string GetAllPrefabsAndChildWithTag(string tag);

		// Token: 0x0600184F RID: 6223
		[EngineMethod("set_upgrade_level_visibility", false, null, false)]
		void SetUpgradeLevelVisibility(string cumulated_string);

		// Token: 0x06001850 RID: 6224
		[EngineMethod("set_level_visibility", false, null, false)]
		void SetLevelVisibility(string cumulated_string);

		// Token: 0x06001851 RID: 6225
		[EngineMethod("toggle_enable_editor_physics", false, null, false)]
		void ToggleEnableEditorPhysics();

		// Token: 0x06001852 RID: 6226
		[EngineMethod("exit_edit_mode", false, null, false)]
		void ExitEditMode();

		// Token: 0x06001853 RID: 6227
		[EngineMethod("is_replay_manager_recording", false, null, false)]
		bool IsReplayManagerRecording();

		// Token: 0x06001854 RID: 6228
		[EngineMethod("is_replay_manager_rendering", false, null, false)]
		bool IsReplayManagerRendering();

		// Token: 0x06001855 RID: 6229
		[EngineMethod("is_replay_manager_replaying", false, null, false)]
		bool IsReplayManagerReplaying();
	}
}
