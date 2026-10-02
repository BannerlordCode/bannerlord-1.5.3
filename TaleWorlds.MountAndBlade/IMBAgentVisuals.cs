using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AB RID: 427
	[ScriptingInterfaceBase]
	internal interface IMBAgentVisuals
	{
		// Token: 0x060016DC RID: 5852
		[EngineMethod("validate_agent_visuals_reseted", false, null, false)]
		void ValidateAgentVisualsReseted(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer);

		// Token: 0x060016DD RID: 5853
		[EngineMethod("create_agent_renderer_scene_controller", false, null, false)]
		UIntPtr CreateAgentRendererSceneController(UIntPtr scenePointer);

		// Token: 0x060016DE RID: 5854
		[EngineMethod("destruct_agent_renderer_scene_controller", false, null, false)]
		void DestructAgentRendererSceneController(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer, bool deleteThisFrame);

		// Token: 0x060016DF RID: 5855
		[EngineMethod("set_do_timer_based_skeleton_forced_updates", false, null, false)]
		void SetDoTimerBasedForcedSkeletonUpdates(UIntPtr agentRendererSceneControllerPointer, bool value);

		// Token: 0x060016E0 RID: 5856
		[EngineMethod("set_enforced_visibility_for_all_agents", false, null, false)]
		void SetEnforcedVisibilityForAllAgents(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer);

		// Token: 0x060016E1 RID: 5857
		[EngineMethod("create_agent_visuals", false, null, false)]
		MBAgentVisuals CreateAgentVisuals(UIntPtr scenePtr, string ownerName, Vec3 eyeOffset);

		// Token: 0x060016E2 RID: 5858
		[EngineMethod("tick", false, null, false)]
		void Tick(UIntPtr agentVisualsId, UIntPtr parentAgentVisualsId, float dt, bool entityMoving, float speed);

		// Token: 0x060016E3 RID: 5859
		[EngineMethod("set_entity", false, null, false)]
		void SetEntity(UIntPtr agentVisualsId, UIntPtr entityPtr);

		// Token: 0x060016E4 RID: 5860
		[EngineMethod("set_skeleton", false, null, false)]
		void SetSkeleton(UIntPtr agentVisualsId, UIntPtr skeletonPtr);

		// Token: 0x060016E5 RID: 5861
		[EngineMethod("fill_entity_with_body_meshes_without_agent_visuals", false, null, false)]
		void FillEntityWithBodyMeshesWithoutAgentVisuals(UIntPtr entityPoinbter, ref SkinGenerationParams skinParams, ref BodyProperties bodyProperties, MetaMesh glovesMesh);

		// Token: 0x060016E6 RID: 5862
		[EngineMethod("add_skin_meshes_to_agent_visuals", false, null, false)]
		void AddSkinMeshesToAgentEntity(UIntPtr agentVisualsId, ref SkinGenerationParams skinParams, ref BodyProperties bodyProperties, bool useGPUMorph, bool useFaceCache);

		// Token: 0x060016E7 RID: 5863
		[EngineMethod("set_lod_atlas_shading_index", false, null, false)]
		void SetLodAtlasShadingIndex(UIntPtr agentVisualsId, int index, bool useTeamColor, uint teamColor1, uint teamColor2);

		// Token: 0x060016E8 RID: 5864
		[EngineMethod("set_face_generation_params", false, null, false)]
		void SetFaceGenerationParams(UIntPtr agentVisualsId, FaceGenerationParams faceGenerationParams);

		// Token: 0x060016E9 RID: 5865
		[EngineMethod("start_rhubarb_record", false, null, false)]
		void StartRhubarbRecord(UIntPtr agentVisualsId, string path, int soundId);

		// Token: 0x060016EA RID: 5866
		[EngineMethod("clear_visual_components", false, null, false)]
		void ClearVisualComponents(UIntPtr agentVisualsId, bool removeSkeleton, bool removeLabel);

		// Token: 0x060016EB RID: 5867
		[EngineMethod("lazy_update_agent_renderer_data", false, null, false)]
		void LazyUpdateAgentRendererData(UIntPtr agentVisualsId);

		// Token: 0x060016EC RID: 5868
		[EngineMethod("add_mesh", false, null, false)]
		void AddMesh(UIntPtr agentVisualsId, UIntPtr meshPointer);

		// Token: 0x060016ED RID: 5869
		[EngineMethod("remove_mesh", false, null, false)]
		void RemoveMesh(UIntPtr agentVisualsPtr, UIntPtr meshPointer);

		// Token: 0x060016EE RID: 5870
		[EngineMethod("add_multi_mesh", false, null, false)]
		void AddMultiMesh(UIntPtr agentVisualsPtr, UIntPtr multiMeshPointer, int bodyMeshIndex);

		// Token: 0x060016EF RID: 5871
		[EngineMethod("add_horse_reins_cloth_mesh", false, null, false)]
		void AddHorseReinsClothMesh(UIntPtr agentVisualsPtr, UIntPtr reinMeshPointer, UIntPtr ropeMeshPointer);

		// Token: 0x060016F0 RID: 5872
		[EngineMethod("update_skeleton_scale", false, null, false)]
		void UpdateSkeletonScale(UIntPtr agentVisualsId, int bodyDeformType);

		// Token: 0x060016F1 RID: 5873
		[EngineMethod("apply_skeleton_scale", false, null, false)]
		void ApplySkeletonScale(UIntPtr agentVisualsId, Vec3 mountSitBoneScale, float mountRadiusAdder, byte boneCount, sbyte[] boneIndices, Vec3[] boneScales);

		// Token: 0x060016F2 RID: 5874
		[EngineMethod("batch_last_lod_meshes", false, null, false)]
		void BatchLastLodMeshes(UIntPtr agentVisualsPtr);

		// Token: 0x060016F3 RID: 5875
		[EngineMethod("remove_multi_mesh", false, null, false)]
		void RemoveMultiMesh(UIntPtr agentVisualsPtr, UIntPtr multiMeshPointer, int bodyMeshIndex);

		// Token: 0x060016F4 RID: 5876
		[EngineMethod("add_weapon_to_agent_entity", false, null, false)]
		void AddWeaponToAgentEntity(UIntPtr agentVisualsPtr, int slotIndex, in WeaponData agentEntityData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, in WeaponData agentEntityAmmoData, WeaponStatsData[] ammoWeaponStatsData, int ammoWeaponStatsDataLength, GameEntity cachedEntity);

		// Token: 0x060016F5 RID: 5877
		[EngineMethod("update_quiver_mesh_of_weapon_in_slot", false, null, false)]
		void UpdateQuiverMeshesWithoutAgent(UIntPtr agentVisualsId, int weaponIndex, int ammoCountToShow);

		// Token: 0x060016F6 RID: 5878
		[EngineMethod("set_wielded_weapon_indices", false, null, false)]
		void SetWieldedWeaponIndices(UIntPtr agentVisualsId, int slotIndexRightHand, int slotIndexLeftHand);

		// Token: 0x060016F7 RID: 5879
		[EngineMethod("clear_all_weapon_meshes", false, null, false)]
		void ClearAllWeaponMeshes(UIntPtr agentVisualsPtr);

		// Token: 0x060016F8 RID: 5880
		[EngineMethod("clear_weapon_meshes", false, null, false)]
		void ClearWeaponMeshes(UIntPtr agentVisualsPtr, int weaponVisualIndex);

		// Token: 0x060016F9 RID: 5881
		[EngineMethod("make_voice", false, null, false)]
		void MakeVoice(UIntPtr agentVisualsPtr, int voiceId, ref Vec3 position);

		// Token: 0x060016FA RID: 5882
		[EngineMethod("set_setup_morph_node", false, null, false)]
		void SetSetupMorphNode(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x060016FB RID: 5883
		[EngineMethod("use_scaled_weapons", false, null, false)]
		void UseScaledWeapons(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x060016FC RID: 5884
		[EngineMethod("set_cloth_component_keep_state_of_all_meshes", false, null, false)]
		void SetClothComponentKeepStateOfAllMeshes(UIntPtr agentVisualsPtr, bool keepState);

		// Token: 0x060016FD RID: 5885
		[EngineMethod("get_current_helmet_scaling_factor", false, null, false)]
		Vec3 GetCurrentHelmetScalingFactor(UIntPtr agentVisualsPtr);

		// Token: 0x060016FE RID: 5886
		[EngineMethod("set_voice_definition_index", false, null, false)]
		void SetVoiceDefinitionIndex(UIntPtr agentVisualsPtr, int voiceDefinitionIndex, float voicePitch);

		// Token: 0x060016FF RID: 5887
		[EngineMethod("set_agent_lod_make_zero_or_max", false, null, false)]
		void SetAgentLodMakeZeroOrMax(UIntPtr agentVisualsPtr, bool makeZero);

		// Token: 0x06001700 RID: 5888
		[EngineMethod("set_agent_local_speed", false, null, false)]
		void SetAgentLocalSpeed(UIntPtr agentVisualsPtr, Vec2 speed);

		// Token: 0x06001701 RID: 5889
		[EngineMethod("set_look_direction", false, null, false)]
		void SetLookDirection(UIntPtr agentVisualsPtr, Vec3 direction);

		// Token: 0x06001702 RID: 5890
		[EngineMethod("get_bone_entitial_frame_at_animation_progress", false, null, true)]
		MatrixFrame GetBoneEntitialFrameAtAnimationProgress(UIntPtr agentVisualsPtr, sbyte boneIndex, int animationIndex, float progress);

		// Token: 0x06001703 RID: 5891
		[EngineMethod("reset", false, null, false)]
		void Reset(UIntPtr agentVisualsPtr);

		// Token: 0x06001704 RID: 5892
		[EngineMethod("reset_next_frame", false, null, false)]
		void ResetNextFrame(UIntPtr agentVisualsPtr);

		// Token: 0x06001705 RID: 5893
		[EngineMethod("set_frame", false, null, false)]
		void SetFrame(UIntPtr agentVisualsPtr, ref MatrixFrame frame);

		// Token: 0x06001706 RID: 5894
		[EngineMethod("get_frame", false, null, true)]
		void GetFrame(UIntPtr agentVisualsPtr, ref MatrixFrame outFrame);

		// Token: 0x06001707 RID: 5895
		[EngineMethod("get_global_frame", false, null, true)]
		void GetGlobalFrame(UIntPtr agentVisualsPtr, ref MatrixFrame outFrame);

		// Token: 0x06001708 RID: 5896
		[EngineMethod("set_visible", false, null, false)]
		void SetVisible(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x06001709 RID: 5897
		[EngineMethod("get_visible", false, null, false)]
		bool GetVisible(UIntPtr agentVisualsPtr);

		// Token: 0x0600170A RID: 5898
		[EngineMethod("get_skeleton", false, null, false)]
		Skeleton GetSkeleton(UIntPtr agentVisualsPtr);

		// Token: 0x0600170B RID: 5899
		[EngineMethod("get_entity", false, null, false)]
		GameEntity GetEntity(UIntPtr agentVisualsPtr);

		// Token: 0x0600170C RID: 5900
		[EngineMethod("get_entity_pointer", false, null, false)]
		UIntPtr GetEntityPointer(UIntPtr agentVisualsPtr);

		// Token: 0x0600170D RID: 5901
		[EngineMethod("is_valid", false, null, false)]
		bool IsValid(UIntPtr agentVisualsPtr);

		// Token: 0x0600170E RID: 5902
		[EngineMethod("get_global_stable_eye_point", false, null, false)]
		Vec3 GetGlobalStableEyePoint(UIntPtr agentVisualsPtr, bool isHumanoid);

		// Token: 0x0600170F RID: 5903
		[EngineMethod("get_global_stable_neck_point", false, null, false)]
		Vec3 GetGlobalStableNeckPoint(UIntPtr agentVisualsPtr, bool isHumanoid);

		// Token: 0x06001710 RID: 5904
		[EngineMethod("get_quick_bone_entitial_frame", false, null, false)]
		void GetBoneEntitialFrame(UIntPtr agentVisualsPtr, sbyte bone, bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x06001711 RID: 5905
		[EngineMethod("set_attached_position_for_rope_entity_after_animation_post_integrate", false, null, false)]
		void SetAttachedPositionForRopeEntityAfterAnimationPostIntegrate(UIntPtr agentVisualsPtr, UIntPtr ropeEntity, sbyte bone);

		// Token: 0x06001712 RID: 5906
		[EngineMethod("get_current_head_look_direction", false, null, false)]
		Vec3 GetCurrentHeadLookDirection(UIntPtr agentVisualsPtr);

		// Token: 0x06001713 RID: 5907
		[EngineMethod("get_current_ragdoll_state", false, null, false)]
		RagdollState GetCurrentRagdollState(UIntPtr agentVisualsPtr);

		// Token: 0x06001714 RID: 5908
		[EngineMethod("get_real_bone_index", false, null, false)]
		sbyte GetRealBoneIndex(UIntPtr agentVisualsPtr, HumanBone boneType);

		// Token: 0x06001715 RID: 5909
		[EngineMethod("add_prefab_to_agent_visual_bone_by_bone_type", false, null, false)]
		CompositeComponent AddPrefabToAgentVisualBoneByBoneType(UIntPtr agentVisualsPtr, string prefabName, HumanBone boneType);

		// Token: 0x06001716 RID: 5910
		[EngineMethod("add_prefab_to_agent_visual_bone_by_real_bone_index", false, null, false)]
		CompositeComponent AddPrefabToAgentVisualBoneByRealBoneIndex(UIntPtr agentVisualsPtr, string prefabName, sbyte realBoneIndex);

		// Token: 0x06001717 RID: 5911
		[EngineMethod("get_attached_weapon_entity", false, null, false)]
		GameEntity GetAttachedWeaponEntity(UIntPtr agentVisualsPtr, int attachedWeaponIndex);

		// Token: 0x06001718 RID: 5912
		[EngineMethod("create_particle_system_attached_to_bone", false, null, false)]
		void CreateParticleSystemAttachedToBone(UIntPtr agentVisualsPtr, int runtimeParticleindex, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame);

		// Token: 0x06001719 RID: 5913
		[EngineMethod("check_resources", false, null, false)]
		bool CheckResources(UIntPtr agentVisualsPtr, bool addToQueue);

		// Token: 0x0600171A RID: 5914
		[EngineMethod("add_child_entity", false, null, false)]
		bool AddChildEntity(UIntPtr agentVisualsPtr, UIntPtr EntityId);

		// Token: 0x0600171B RID: 5915
		[EngineMethod("set_cloth_wind_to_weapon_at_index", false, null, false)]
		void SetClothWindToWeaponAtIndex(UIntPtr agentVisualsPtr, Vec3 windVector, bool isLocal, int index);

		// Token: 0x0600171C RID: 5916
		[EngineMethod("remove_child_entity", false, null, false)]
		void RemoveChildEntity(UIntPtr agentVisualsPtr, UIntPtr EntityId, int removeReason);

		// Token: 0x0600171D RID: 5917
		[EngineMethod("disable_contour", false, null, false)]
		void DisableContour(UIntPtr agentVisualsPtr);

		// Token: 0x0600171E RID: 5918
		[EngineMethod("set_as_contour_entity", false, null, false)]
		void SetAsContourEntity(UIntPtr agentVisualsPtr, uint color);

		// Token: 0x0600171F RID: 5919
		[EngineMethod("set_contour_state", false, null, false)]
		void SetContourState(UIntPtr agentVisualsPtr, bool alwaysVisible);

		// Token: 0x06001720 RID: 5920
		[EngineMethod("set_enable_occlusion_culling", false, null, false)]
		void SetEnableOcclusionCulling(UIntPtr agentVisualsPtr, bool enable);

		// Token: 0x06001721 RID: 5921
		[EngineMethod("get_bone_type_data", false, null, false)]
		void GetBoneTypeData(UIntPtr pointer, sbyte boneIndex, ref BoneBodyTypeData boneBodyTypeData);

		// Token: 0x06001722 RID: 5922
		[EngineMethod("get_movement_mode", false, null, false)]
		int GetMovementMode(UIntPtr agentVisualsPtr);

		// Token: 0x06001723 RID: 5923
		[EngineMethod("get_visual_strength_of_agent_visual", false, null, false)]
		float GetVisualStrengthOfAgentVisual(UIntPtr agentVisualsPtr, UIntPtr targetagentVisualsPtr, UIntPtr missionPointer, float ambientLightStrength, float sunMoonLightStrength, int agentIndexToIgnore);
	}
}
