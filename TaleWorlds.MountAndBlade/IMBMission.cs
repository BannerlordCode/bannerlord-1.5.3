using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B1 RID: 433
	[ScriptingInterfaceBase]
	internal interface IMBMission
	{
		// Token: 0x06001856 RID: 6230
		[EngineMethod("clear_resources", false, null, false)]
		void ClearResources(UIntPtr missionPointer);

		// Token: 0x06001857 RID: 6231
		[EngineMethod("defrag_render_buffers", false, null, false)]
		void DefragRenderBuffers();

		// Token: 0x06001858 RID: 6232
		[EngineMethod("create_mission", false, null, false)]
		UIntPtr CreateMission(Mission mission);

		// Token: 0x06001859 RID: 6233
		[EngineMethod("set_close_proximity_wave_sounds_enabled", false, null, false)]
		void SetCloseProximityWaveSoundsEnabled(UIntPtr missionPointer, bool value);

		// Token: 0x0600185A RID: 6234
		[EngineMethod("force_disable_occlusion", false, null, false)]
		void ForceDisableOcclusion(UIntPtr missionPointer, bool value);

		// Token: 0x0600185B RID: 6235
		[EngineMethod("tick_agents_and_teams_async", false, null, false)]
		void TickAgentsAndTeamsAsync(UIntPtr missionPointer, float dt);

		// Token: 0x0600185C RID: 6236
		[EngineMethod("get_tick_debug_paused", false, null, false)]
		bool GetTickDebugPaused(UIntPtr missionPointer);

		// Token: 0x0600185D RID: 6237
		[EngineMethod("clear_agent_actions", false, null, false)]
		void ClearAgentActions(UIntPtr missionPointer);

		// Token: 0x0600185E RID: 6238
		[EngineMethod("clear_missiles", false, null, false)]
		void ClearMissiles(UIntPtr missionPointer);

		// Token: 0x0600185F RID: 6239
		[EngineMethod("clear_corpses", false, null, false)]
		void ClearCorpses(UIntPtr missionPointer, bool isMissionReset);

		// Token: 0x06001860 RID: 6240
		[EngineMethod("get_pause_ai_tick", false, null, false)]
		bool GetPauseAITick(UIntPtr missionPointer);

		// Token: 0x06001861 RID: 6241
		[EngineMethod("set_pause_ai_tick", false, null, false)]
		void SetPauseAITick(UIntPtr missionPointer, bool value);

		// Token: 0x06001862 RID: 6242
		[EngineMethod("get_clear_scene_timer_elapsed_time", false, null, false)]
		float GetClearSceneTimerElapsedTime(UIntPtr missionPointer);

		// Token: 0x06001863 RID: 6243
		[EngineMethod("reset_first_third_person_view", false, null, false)]
		void ResetFirstThirdPersonView(UIntPtr missionPointer);

		// Token: 0x06001864 RID: 6244
		[EngineMethod("set_camera_is_first_person", false, null, false)]
		void SetCameraIsFirstPerson(bool value);

		// Token: 0x06001865 RID: 6245
		[EngineMethod("set_camera_frame", false, null, false)]
		void SetCameraFrame(UIntPtr missionPointer, ref MatrixFrame cameraFrame, float zoomFactor, ref Vec3 attenuationPosition);

		// Token: 0x06001866 RID: 6246
		[EngineMethod("get_camera_frame", false, null, false)]
		MatrixFrame GetCameraFrame(UIntPtr missionPointer);

		// Token: 0x06001867 RID: 6247
		[EngineMethod("get_is_loading_finished", false, null, false)]
		bool GetIsLoadingFinished(UIntPtr missionPointer);

		// Token: 0x06001868 RID: 6248
		[EngineMethod("clear_scene", false, null, false)]
		void ClearScene(UIntPtr missionPointer);

		// Token: 0x06001869 RID: 6249
		[EngineMethod("initialize_mission", false, null, false)]
		void InitializeMission(UIntPtr missionPointer, ref MissionInitializerRecord rec);

		// Token: 0x0600186A RID: 6250
		[EngineMethod("finalize_mission", false, null, false)]
		void FinalizeMission(UIntPtr missionPointer);

		// Token: 0x0600186B RID: 6251
		[EngineMethod("get_time", false, null, false)]
		float GetTime(UIntPtr missionPointer);

		// Token: 0x0600186C RID: 6252
		[EngineMethod("get_average_fps", false, null, false)]
		float GetAverageFps(UIntPtr missionPointer);

		// Token: 0x0600186D RID: 6253
		[EngineMethod("get_combat_type", false, null, false)]
		int GetCombatType(UIntPtr missionPointer);

		// Token: 0x0600186E RID: 6254
		[EngineMethod("set_combat_type", false, null, false)]
		void SetCombatType(UIntPtr missionPointer, int combatType);

		// Token: 0x0600186F RID: 6255
		[EngineMethod("ray_cast_for_closest_agent", false, null, false)]
		Agent RayCastForClosestAgent(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int excludeAgentIndex, float rayThickness, out float collisionDistance);

		// Token: 0x06001870 RID: 6256
		[EngineMethod("ray_cast_for_closest_agents_limbs", false, null, false)]
		Agent RayCastForClosestAgentsLimbs(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int excludeAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex);

		// Token: 0x06001871 RID: 6257
		[EngineMethod("ray_cast_for_given_agents_limbs", false, null, false)]
		bool RayCastForGivenAgentsLimbs(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int givenAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex);

		// Token: 0x06001872 RID: 6258
		[EngineMethod("get_number_of_teams", false, null, false)]
		int GetNumberOfTeams(UIntPtr missionPointer);

		// Token: 0x06001873 RID: 6259
		[EngineMethod("reset_teams", false, null, false)]
		void ResetTeams(UIntPtr missionPointer);

		// Token: 0x06001874 RID: 6260
		[EngineMethod("add_team", false, null, false)]
		int AddTeam(UIntPtr missionPointer);

		// Token: 0x06001875 RID: 6261
		[EngineMethod("restart_record", false, null, false)]
		void RestartRecord(UIntPtr missionPointer);

		// Token: 0x06001876 RID: 6262
		[EngineMethod("is_position_inside_boundaries", false, null, false)]
		bool IsPositionInsideBoundaries(UIntPtr missionPointer, Vec2 position);

		// Token: 0x06001877 RID: 6263
		[EngineMethod("is_position_inside_hard_boundaries", false, null, false)]
		bool IsPositionInsideHardBoundaries(UIntPtr missionPointer, Vec2 position);

		// Token: 0x06001878 RID: 6264
		[EngineMethod("is_position_inside_any_blocker_nav_mesh_face_2d", false, null, false)]
		bool IsPositionInsideAnyBlockerNavMeshFace2D(UIntPtr missionPointer, Vec2 position);

		// Token: 0x06001879 RID: 6265
		[EngineMethod("is_position_on_any_blocker_nav_mesh_face", false, null, false)]
		bool IsPositionOnAnyBlockerNavMeshFace(UIntPtr missionPointer, Vec3 position);

		// Token: 0x0600187A RID: 6266
		[EngineMethod("get_alternate_position_for_navmeshless_or_out_of_bounds_position", false, null, false)]
		WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(UIntPtr ptr, ref Vec2 directionTowards, ref WorldPosition originalPosition, ref float positionPenalty);

		// Token: 0x0600187B RID: 6267
		[EngineMethod("add_missile", false, null, false)]
		int AddMissile(UIntPtr missionPointer, bool isPrediction, int shooterAgentIndex, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, UIntPtr entityPointer, int forcedMissileIndex, bool isPrimaryWeaponShot, out UIntPtr missileEntity);

		// Token: 0x0600187C RID: 6268
		[EngineMethod("add_missile_single_usage", false, null, false)]
		int AddMissileSingleUsage(UIntPtr missionPointer, bool isPrediction, int shooterAgentIndex, in WeaponData weaponData, in WeaponStatsData weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, UIntPtr entityPointer, int forcedMissileIndex, bool isPrimaryWeaponShot, out UIntPtr missileEntity);

		// Token: 0x0600187D RID: 6269
		[EngineMethod("get_missile_collision_point", false, null, false)]
		Vec3 GetMissileCollisionPoint(UIntPtr missionPointer, Vec3 missileStartingPosition, Vec3 missileDirection, float missileStartingSpeed, in WeaponData weaponData);

		// Token: 0x0600187E RID: 6270
		[EngineMethod("remove_missile", false, null, false)]
		void RemoveMissile(UIntPtr missionPointer, int missileIndex);

		// Token: 0x0600187F RID: 6271
		[EngineMethod("get_missile_vertical_aim_correction", false, null, false)]
		float GetMissileVerticalAimCorrection(Vec3 vecToTarget, float missileStartingSpeed, ref WeaponStatsData weaponStatsData, float airFrictionConstant);

		// Token: 0x06001880 RID: 6272
		[EngineMethod("get_missile_range", false, null, false)]
		float GetMissileRange(float missileStartingSpeed, float heightDifference);

		// Token: 0x06001881 RID: 6273
		[EngineMethod("compute_exact_missile_range_at_height_difference", false, null, false)]
		float ComputeExactMissileRangeAtHeightDifference(float targetHeightDifference, float initialSpeed, float airFrictionConstant, float maxDuration);

		// Token: 0x06001882 RID: 6274
		[EngineMethod("prepare_missile_weapon_for_drop", false, null, false)]
		void PrepareMissileWeaponForDrop(UIntPtr missionPointer, int missileIndex);

		// Token: 0x06001883 RID: 6275
		[EngineMethod("add_particle_system_burst_by_name", false, null, false)]
		void AddParticleSystemBurstByName(UIntPtr missionPointer, string particleSystem, ref MatrixFrame frame, bool synchThroughNetwork);

		// Token: 0x06001884 RID: 6276
		[EngineMethod("tick", false, null, false)]
		void Tick(UIntPtr missionPointer, float dt);

		// Token: 0x06001885 RID: 6277
		[EngineMethod("idle_tick", false, null, false)]
		void IdleTick(UIntPtr missionPointer, float dt);

		// Token: 0x06001886 RID: 6278
		[EngineMethod("make_sound", false, null, false)]
		void MakeSound(UIntPtr pointer, int nativeSoundCode, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2);

		// Token: 0x06001887 RID: 6279
		[EngineMethod("make_sound_with_parameter", false, null, false)]
		void MakeSoundWithParameter(UIntPtr pointer, int nativeSoundCode, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2, SoundEventParameter parameter);

		// Token: 0x06001888 RID: 6280
		[EngineMethod("make_sound_only_on_related_peer", false, null, false)]
		void MakeSoundOnlyOnRelatedPeer(UIntPtr pointer, int nativeSoundCode, Vec3 position, int relatedAgent);

		// Token: 0x06001889 RID: 6281
		[EngineMethod("create_agent", false, null, false)]
		Mission.AgentCreationResult CreateAgent(UIntPtr missionPointer, ulong monsterFlag, int forcedAgentIndex, bool isFemale, ref AgentSpawnData spawnData, ref CapsuleData bodyCapsule, ref CapsuleData crouchedBodyCapsule, ref AnimationSystemData animationSystemData, int instanceNo);

		// Token: 0x0600188A RID: 6282
		[EngineMethod("get_position_of_missile", false, null, false)]
		Vec3 GetPositionOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x0600188B RID: 6283
		[EngineMethod("get_old_position_of_missile", false, null, false)]
		Vec3 GetOldPositionOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x0600188C RID: 6284
		[EngineMethod("get_velocity_of_missile", false, null, false)]
		Vec3 GetVelocityOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x0600188D RID: 6285
		[EngineMethod("set_velocity_of_missile", false, null, false)]
		void SetVelocityOfMissile(UIntPtr missionPointer, int index, in Vec3 velocity);

		// Token: 0x0600188E RID: 6286
		[EngineMethod("get_missile_has_rigid_body", false, null, false)]
		bool GetMissileHasRigidBody(UIntPtr missionPointer, int index);

		// Token: 0x0600188F RID: 6287
		[EngineMethod("add_boundary", false, null, false)]
		bool AddBoundary(UIntPtr missionPointer, string name, Vec2[] boundaryPoints, int boundaryPointCount, bool isAllowanceInside);

		// Token: 0x06001890 RID: 6288
		[EngineMethod("remove_boundary", false, null, false)]
		bool RemoveBoundary(UIntPtr missionPointer, string name);

		// Token: 0x06001891 RID: 6289
		[EngineMethod("get_boundary_points", false, null, false)]
		void GetBoundaryPoints(UIntPtr missionPointer, string name, int boundaryPointOffset, Vec2[] boundaryPoints, int boundaryPointsSize, ref int retrievedPointCount);

		// Token: 0x06001892 RID: 6290
		[EngineMethod("get_boundary_count", false, null, false)]
		int GetBoundaryCount(UIntPtr missionPointer);

		// Token: 0x06001893 RID: 6291
		[EngineMethod("get_boundary_radius", false, null, false)]
		float GetBoundaryRadius(UIntPtr missionPointer, string name);

		// Token: 0x06001894 RID: 6292
		[EngineMethod("get_boundary_name", false, null, false)]
		string GetBoundaryName(UIntPtr missionPointer, int boundaryIndex);

		// Token: 0x06001895 RID: 6293
		[EngineMethod("get_closest_boundary_position", false, null, false)]
		Vec2 GetClosestBoundaryPosition(UIntPtr missionPointer, Vec2 position);

		// Token: 0x06001896 RID: 6294
		[EngineMethod("get_navigation_points", false, null, false)]
		bool GetNavigationPoints(UIntPtr missionPointer, ref NavigationData navigationData);

		// Token: 0x06001897 RID: 6295
		[EngineMethod("set_navigation_face_cost_with_id_around_position", false, null, false)]
		void SetNavigationFaceCostWithIdAroundPosition(UIntPtr missionPointer, int navigationFaceId, Vec3 position, float cost);

		// Token: 0x06001898 RID: 6296
		[EngineMethod("pause_mission_scene_sounds", false, null, false)]
		void PauseMissionSceneSounds(UIntPtr missionPointer);

		// Token: 0x06001899 RID: 6297
		[EngineMethod("resume_mission_scene_sounds", false, null, false)]
		void ResumeMissionSceneSounds(UIntPtr missionPointer);

		// Token: 0x0600189A RID: 6298
		[EngineMethod("process_record_until_time", false, null, false)]
		void ProcessRecordUntilTime(UIntPtr missionPointer, float time);

		// Token: 0x0600189B RID: 6299
		[EngineMethod("end_of_record", false, null, false)]
		bool EndOfRecord(UIntPtr missionPointer);

		// Token: 0x0600189C RID: 6300
		[EngineMethod("record_current_state", false, null, false)]
		void RecordCurrentState(UIntPtr missionPointer);

		// Token: 0x0600189D RID: 6301
		[EngineMethod("start_recording", false, null, false)]
		void StartRecording();

		// Token: 0x0600189E RID: 6302
		[EngineMethod("backup_record_to_file", false, null, false)]
		void BackupRecordToFile(UIntPtr missionPointer, string fileName, string gameType, string sceneLevels);

		// Token: 0x0600189F RID: 6303
		[EngineMethod("restore_record_from_file", false, null, false)]
		void RestoreRecordFromFile(UIntPtr missionPointer, string fileName);

		// Token: 0x060018A0 RID: 6304
		[EngineMethod("clear_record_buffers", false, null, false)]
		void ClearRecordBuffers(UIntPtr missionPointer);

		// Token: 0x060018A1 RID: 6305
		[EngineMethod("get_scene_name_for_replay", false, null, false)]
		string GetSceneNameForReplay(PlatformFilePath replayName);

		// Token: 0x060018A2 RID: 6306
		[EngineMethod("get_game_type_for_replay", false, null, false)]
		string GetGameTypeForReplay(PlatformFilePath replayName);

		// Token: 0x060018A3 RID: 6307
		[EngineMethod("get_scene_levels_for_replay", false, null, false)]
		string GetSceneLevelsForReplay(PlatformFilePath replayName);

		// Token: 0x060018A4 RID: 6308
		[EngineMethod("get_atmosphere_name_for_replay", false, null, false)]
		string GetAtmosphereNameForReplay(PlatformFilePath replayName);

		// Token: 0x060018A5 RID: 6309
		[EngineMethod("get_atmosphere_season_for_replay", false, null, false)]
		int GetAtmosphereSeasonForReplay(PlatformFilePath replayName);

		// Token: 0x060018A6 RID: 6310
		[EngineMethod("get_closest_enemy", false, null, false)]
		Agent GetClosestEnemy(UIntPtr missionPointer, int teamIndex, Vec3 position, float radius);

		// Token: 0x060018A7 RID: 6311
		[EngineMethod("get_closest_ally", false, null, false)]
		Agent GetClosestAlly(UIntPtr missionPointer, int teamIndex, Vec3 position, float radius);

		// Token: 0x060018A8 RID: 6312
		[EngineMethod("is_agent_in_proximity_map", false, null, false)]
		bool IsAgentInProximityMap(UIntPtr missionPointer, int agentIndex);

		// Token: 0x060018A9 RID: 6313
		[EngineMethod("has_any_agents_of_team_around", false, null, false)]
		bool HasAnyAgentsOfTeamAround(UIntPtr missionPointer, Vec3 origin, float radius, int teamNo);

		// Token: 0x060018AA RID: 6314
		[EngineMethod("get_agent_count_around_position", false, null, false)]
		void GetAgentCountAroundPosition(UIntPtr missionPointer, int teamIndex, Vec2 position, float radius, ref int allyCount, ref int enemyCount);

		// Token: 0x060018AB RID: 6315
		[EngineMethod("find_agent_with_index", false, null, false)]
		Agent FindAgentWithIndex(UIntPtr missionPointer, int index);

		// Token: 0x060018AC RID: 6316
		[EngineMethod("set_random_decide_time_of_agents", false, null, false)]
		void SetRandomDecideTimeOfAgents(UIntPtr missionPointer, int agentCount, int[] agentIndices, float minAIReactionTime, float maxAIReactionTime);

		// Token: 0x060018AD RID: 6317
		[EngineMethod("get_average_morale_of_agents", false, null, false)]
		float GetAverageMoraleOfAgents(UIntPtr missionPointer, int agentCount, int[] agentIndices);

		// Token: 0x060018AE RID: 6318
		[EngineMethod("get_best_slope_towards_direction", false, null, false)]
		WorldPosition GetBestSlopeTowardsDirection(UIntPtr missionPointer, ref WorldPosition centerPosition, float halfsize, ref WorldPosition referencePosition);

		// Token: 0x060018AF RID: 6319
		[EngineMethod("get_best_slope_angle_height_pos_for_defending", false, null, false)]
		WorldPosition GetBestSlopeAngleHeightPosForDefending(UIntPtr missionPointer, WorldPosition enemyPosition, WorldPosition defendingPosition, int sampleSize, float distanceRatioAllowedFromDefendedPos, float distanceSqrdAllowedFromBoundary, float cosinusOfBestSlope, float cosinusOfMaxAcceptedSlope, float minSlopeScore, float maxSlopeScore, float excessiveSlopePenalty, float nearConeCenterRatio, float nearConeCenterBonus, float heightDifferenceCeiling, float maxDisplacementPenalty);

		// Token: 0x060018B0 RID: 6320
		[EngineMethod("get_nearby_agents_aux", false, null, false)]
		void GetNearbyAgentsAux(UIntPtr missionPointer, Vec2 center, float radius, int teamIndex, int friendOrEnemyOrAll, int agentsArrayOffset, ref EngineStackArray.StackArray40Int agentIds, ref int retrievedAgentCount);

		// Token: 0x060018B1 RID: 6321
		[EngineMethod("get_weighted_point_of_enemies", false, null, false)]
		Vec2 GetWeightedPointOfEnemies(UIntPtr missionPointer, int agentIndex, Vec2 basePoint);

		// Token: 0x060018B2 RID: 6322
		[EngineMethod("is_formation_unit_position_available", false, null, false)]
		bool IsFormationUnitPositionAvailable(UIntPtr missionPointer, ref WorldPosition orderPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance);

		// Token: 0x060018B3 RID: 6323
		[EngineMethod("get_straight_path_to_target", false, null, false)]
		WorldPosition GetStraightPathToTarget(UIntPtr scenePointer, Vec2 targetPosition, WorldPosition startingPosition, float samplingDistance, bool stopAtObstacle);

		// Token: 0x060018B4 RID: 6324
		[EngineMethod("set_bow_missile_speed_modifier", false, null, false)]
		void SetBowMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x060018B5 RID: 6325
		[EngineMethod("set_crossbow_missile_speed_modifier", false, null, false)]
		void SetCrossbowMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x060018B6 RID: 6326
		[EngineMethod("set_throwing_missile_speed_modifier", false, null, false)]
		void SetThrowingMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x060018B7 RID: 6327
		[EngineMethod("set_missile_range_modifier", false, null, false)]
		void SetMissileRangeModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x060018B8 RID: 6328
		[EngineMethod("set_last_movement_key_pressed", false, null, false)]
		void SetLastMovementKeyPressed(UIntPtr missionPointer, Agent.MovementControlFlag lastMovementKeyPressed);

		// Token: 0x060018B9 RID: 6329
		[EngineMethod("skip_forward_mission_replay", false, null, false)]
		void SkipForwardMissionReplay(UIntPtr missionPointer, float startTime, float endTime);

		// Token: 0x060018BA RID: 6330
		[EngineMethod("get_debug_agent", false, null, false)]
		int GetDebugAgent(UIntPtr missionPointer);

		// Token: 0x060018BB RID: 6331
		[EngineMethod("set_debug_agent", false, null, false)]
		void SetDebugAgent(UIntPtr missionPointer, int index);

		// Token: 0x060018BC RID: 6332
		[EngineMethod("add_ai_debug_text", false, null, false)]
		void AddAiDebugText(UIntPtr missionPointer, string text);

		// Token: 0x060018BD RID: 6333
		[EngineMethod("agent_proximity_map_begin_search", false, null, false)]
		AgentProximityMap.ProximityMapSearchStructInternal ProximityMapBeginSearch(UIntPtr missionPointer, Vec2 searchPos, float searchRadius);

		// Token: 0x060018BE RID: 6334
		[EngineMethod("agent_proximity_map_find_next", false, null, false)]
		void ProximityMapFindNext(UIntPtr missionPointer, ref AgentProximityMap.ProximityMapSearchStructInternal searchStruct);

		// Token: 0x060018BF RID: 6335
		[EngineMethod("agent_proximity_map_get_max_search_radius", false, null, false)]
		float ProximityMapMaxSearchRadius(UIntPtr missionPointer);

		// Token: 0x060018C0 RID: 6336
		[EngineMethod("set_override_corpse_count", false, null, false)]
		void SetOverrideCorpseCount(UIntPtr missionPointer, int overrideCorpseCount);

		// Token: 0x060018C1 RID: 6337
		[EngineMethod("get_biggest_agent_collision_padding", false, null, false)]
		float GetBiggestAgentCollisionPadding(UIntPtr missionPointer);

		// Token: 0x060018C2 RID: 6338
		[EngineMethod("set_mission_corpse_fade_out_time_in_seconds", false, null, false)]
		void SetMissionCorpseFadeOutTimeInSeconds(UIntPtr missionPointer, float corpseFadeOutTimeInSeconds);

		// Token: 0x060018C3 RID: 6339
		[EngineMethod("set_report_stuck_agents_mode", false, null, false)]
		void SetReportStuckAgentsMode(UIntPtr missionPointer, bool value);

		// Token: 0x060018C4 RID: 6340
		[EngineMethod("batch_formation_unit_positions", false, null, false)]
		void BatchFormationUnitPositions(UIntPtr missionPointer, Vec2i[] orderedPositionIndices, Vec2[] orderedLocalPositions, int[] availabilityTable, WorldPosition[] globalPositionTable, WorldPosition orderPosition, Vec2 direction, int fileCount, int rankCount, bool fastCheckWithSameFaceGroupIdDigit);

		// Token: 0x060018C5 RID: 6341
		[EngineMethod("get_fall_avoid_system_active", false, null, false)]
		bool GetFallAvoidSystemActive(UIntPtr missionPointer);

		// Token: 0x060018C6 RID: 6342
		[EngineMethod("set_fall_avoid_system_active", false, null, false)]
		void SetFallAvoidSystemActive(UIntPtr missionPointer, bool fallAvoidActive);

		// Token: 0x060018C7 RID: 6343
		[EngineMethod("get_water_level_at_position", false, null, false)]
		float GetWaterLevelAtPosition(UIntPtr missionPointer, Vec2 position, bool useWaterRenderer);

		// Token: 0x060018C8 RID: 6344
		[EngineMethod("find_convex_hull", false, null, false)]
		void FindConvexHull(Vec2[] boundaryPoints, int boundaryPointCount, ref int convexPointCount);

		// Token: 0x060018C9 RID: 6345
		[EngineMethod("on_fast_forward_state_changed", false, null, false)]
		void OnFastForwardStateChanged(UIntPtr missionPointer, bool state);

		// Token: 0x060018CA RID: 6346
		[EngineMethod("get_current_volume_generator_version", false, null, false)]
		int GetCurrentVolumeGeneratorVersion();
	}
}
