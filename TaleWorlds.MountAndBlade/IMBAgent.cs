using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AC RID: 428
	[ScriptingInterfaceBase]
	internal interface IMBAgent
	{
		// Token: 0x06001724 RID: 5924
		[EngineMethod("get_movement_flags", false, null, false)]
		Agent.MovementControlFlag GetMovementFlags(UIntPtr agentPointer);

		// Token: 0x06001725 RID: 5925
		[EngineMethod("set_movement_flags", false, null, false)]
		void SetMovementFlags(UIntPtr agentPointer, Agent.MovementControlFlag value);

		// Token: 0x06001726 RID: 5926
		[EngineMethod("get_movement_input_vector", false, null, false)]
		Vec2 GetMovementInputVector(UIntPtr agentPointer);

		// Token: 0x06001727 RID: 5927
		[EngineMethod("set_movement_input_vector", false, null, false)]
		void SetMovementInputVector(UIntPtr agentPointer, Vec2 value);

		// Token: 0x06001728 RID: 5928
		[EngineMethod("get_collision_capsule", false, null, false)]
		void GetCollisionCapsule(UIntPtr agentPointer, ref CapsuleData value);

		// Token: 0x06001729 RID: 5929
		[EngineMethod("set_attack_state", false, null, false)]
		void SetAttackState(UIntPtr agentPointer, int attackState);

		// Token: 0x0600172A RID: 5930
		[EngineMethod("get_agent_visuals", false, null, false)]
		MBAgentVisuals GetAgentVisuals(UIntPtr agentPointer);

		// Token: 0x0600172B RID: 5931
		[EngineMethod("get_event_control_flags", false, null, false)]
		Agent.EventControlFlag GetEventControlFlags(UIntPtr agentPointer);

		// Token: 0x0600172C RID: 5932
		[EngineMethod("set_event_control_flags", false, null, false)]
		void SetEventControlFlags(UIntPtr agentPointer, Agent.EventControlFlag eventflag);

		// Token: 0x0600172D RID: 5933
		[EngineMethod("get_has_on_ai_input_set_callback", false, null, false)]
		bool GetHasOnAiInputSetCallback(UIntPtr agentPointer);

		// Token: 0x0600172E RID: 5934
		[EngineMethod("set_has_on_ai_input_set_callback", false, null, false)]
		void SetHasOnAiInputSetCallback(UIntPtr agentPointer, bool value);

		// Token: 0x0600172F RID: 5935
		[EngineMethod("set_average_ping_in_milliseconds", false, null, false)]
		void SetAveragePingInMilliseconds(UIntPtr agentPointer, double averagePingInMilliseconds);

		// Token: 0x06001730 RID: 5936
		[EngineMethod("set_look_agent", false, null, false)]
		void SetLookAgent(UIntPtr agentPointer, UIntPtr lookAtAgentPointer);

		// Token: 0x06001731 RID: 5937
		[EngineMethod("get_look_agent", false, null, false)]
		Agent GetLookAgent(UIntPtr agentPointer);

		// Token: 0x06001732 RID: 5938
		[EngineMethod("get_target_agent", false, null, false)]
		Agent GetTargetAgent(UIntPtr agentPointer);

		// Token: 0x06001733 RID: 5939
		[EngineMethod("set_target_agent", false, null, false)]
		void SetTargetAgent(UIntPtr agentPointer, int targetAgentIndex);

		// Token: 0x06001734 RID: 5940
		[EngineMethod("set_is_physics_force_closed", false, null, false)]
		void SetIsPhysicsForceClosed(UIntPtr agentPointer, bool isPhysicsForceClosed);

		// Token: 0x06001735 RID: 5941
		[EngineMethod("set_interaction_agent", false, null, false)]
		void SetInteractionAgent(UIntPtr agentPointer, UIntPtr interactionAgentPointer);

		// Token: 0x06001736 RID: 5942
		[EngineMethod("set_look_to_point_of_interest", false, null, false)]
		void SetLookToPointOfInterest(UIntPtr agentPointer, Vec3 point);

		// Token: 0x06001737 RID: 5943
		[EngineMethod("disable_look_to_point_of_interest", false, null, false)]
		void DisableLookToPointOfInterest(UIntPtr agentPointer);

		// Token: 0x06001738 RID: 5944
		[EngineMethod("is_enemy", false, null, false)]
		bool IsEnemy(UIntPtr agentPointer1, UIntPtr agentPointer2);

		// Token: 0x06001739 RID: 5945
		[EngineMethod("is_friend", false, null, false)]
		bool IsFriend(UIntPtr agentPointer1, UIntPtr agentPointer2);

		// Token: 0x0600173A RID: 5946
		[EngineMethod("set_agent_flags", false, null, false)]
		void SetAgentFlags(UIntPtr agentPointer, uint agentFlags);

		// Token: 0x0600173B RID: 5947
		[EngineMethod("set_selected_mount_index", false, null, false)]
		void SetSelectedMountIndex(UIntPtr agentPointer, int mount_index);

		// Token: 0x0600173C RID: 5948
		[EngineMethod("get_selected_mount_index", false, null, false)]
		int GetSelectedMountIndex(UIntPtr agentPointer);

		// Token: 0x0600173D RID: 5949
		[EngineMethod("get_firing_order", false, null, false)]
		int GetFiringOrder(UIntPtr agentPointer);

		// Token: 0x0600173E RID: 5950
		[EngineMethod("get_riding_order", false, null, false)]
		int GetRidingOrder(UIntPtr agentPointer);

		// Token: 0x0600173F RID: 5951
		[EngineMethod("get_stepped_body_flags", false, null, false)]
		BodyFlags GetSteppedBodyFlags(UIntPtr agentPointer);

		// Token: 0x06001740 RID: 5952
		[EngineMethod("get_stepped_entity_id", false, null, true)]
		UIntPtr GetSteppedEntityId(UIntPtr agentPointer0);

		// Token: 0x06001741 RID: 5953
		[EngineMethod("get_stepped_root_entity_id", false, null, true)]
		UIntPtr GetSteppedRootEntity(UIntPtr agentPointer0);

		// Token: 0x06001742 RID: 5954
		[EngineMethod("set_network_peer", false, null, false)]
		void SetNetworkPeer(UIntPtr agentPointer, int networkPeerIndex);

		// Token: 0x06001743 RID: 5955
		[EngineMethod("die", false, null, false)]
		void Die(UIntPtr agentPointer, ref Blow b, sbyte overrideKillInfo);

		// Token: 0x06001744 RID: 5956
		[EngineMethod("make_dead", false, null, false)]
		void MakeDead(UIntPtr agentPointer, bool isKilled, int actionIndex, int corpsesToFadeIndex);

		// Token: 0x06001745 RID: 5957
		[EngineMethod("set_formation_frame_disabled", false, null, true)]
		void SetFormationFrameDisabled(UIntPtr agentPointer);

		// Token: 0x06001746 RID: 5958
		[EngineMethod("set_formation_frame_enabled", false, null, true)]
		bool SetFormationFrameEnabled(UIntPtr agentPointer, WorldPosition position, Vec2 direction, Vec2 positionVelocity, float formationDirectionEnforcingFactor, bool teleportAgents);

		// Token: 0x06001747 RID: 5959
		[EngineMethod("set_should_catch_up_with_formation", false, null, false)]
		void SetShouldCatchUpWithFormation(UIntPtr agentPointer, bool value);

		// Token: 0x06001748 RID: 5960
		[EngineMethod("set_formation_integrity_data", false, null, true)]
		void SetFormationIntegrityData(UIntPtr agentPointer, in Vec2 position, in Vec2 currentFormationDirection, in Vec2 averageVelocityOfCloseAgents, float averageMaxUnlimitedSpeedOfCloseAgents, float deviationOfPositions, bool shouldKeepWithFormationInsteadOfMovingToAgent);

		// Token: 0x06001749 RID: 5961
		[EngineMethod("set_formation_info", false, null, false)]
		void SetFormationInfo(UIntPtr agentPointer, int fileIndex, int rankIndex, int fileCount, int rankCount, int unitCount, Vec2 wallDir, int unitSpacing, bool isUnderRangedAttack);

		// Token: 0x0600174A RID: 5962
		[EngineMethod("set_retreat_mode", false, null, false)]
		void SetRetreatMode(UIntPtr agentPointer, WorldPosition retreatPos, bool retreat);

		// Token: 0x0600174B RID: 5963
		[EngineMethod("is_retreating", false, null, true)]
		bool IsRetreating(UIntPtr agentPointer);

		// Token: 0x0600174C RID: 5964
		[EngineMethod("is_fading_out", false, null, true)]
		bool IsFadingOut(UIntPtr agentPointer);

		// Token: 0x0600174D RID: 5965
		[EngineMethod("is_wandering", false, null, true)]
		bool IsWandering(UIntPtr agentPointer);

		// Token: 0x0600174E RID: 5966
		[EngineMethod("start_fading_out", false, null, false)]
		void StartFadingOut(UIntPtr agentPointer);

		// Token: 0x0600174F RID: 5967
		[EngineMethod("set_render_check_enabled", false, null, false)]
		void SetRenderCheckEnabled(UIntPtr agentPointer, bool value);

		// Token: 0x06001750 RID: 5968
		[EngineMethod("get_render_check_enabled", false, null, false)]
		bool GetRenderCheckEnabled(UIntPtr agentPointer);

		// Token: 0x06001751 RID: 5969
		[EngineMethod("get_retreat_pos", false, null, false)]
		WorldPosition GetRetreatPos(UIntPtr agentPointer);

		// Token: 0x06001752 RID: 5970
		[EngineMethod("get_team", false, null, false)]
		int GetTeam(UIntPtr agentPointer);

		// Token: 0x06001753 RID: 5971
		[EngineMethod("set_team", false, null, false)]
		void SetTeam(UIntPtr agentPointer, int teamIndex);

		// Token: 0x06001754 RID: 5972
		[EngineMethod("set_courage", false, null, false)]
		void SetCourage(UIntPtr agentPointer, float courage);

		// Token: 0x06001755 RID: 5973
		[EngineMethod("update_driven_properties", false, null, false)]
		void UpdateDrivenProperties(UIntPtr agentPointer, float[] values);

		// Token: 0x06001756 RID: 5974
		[EngineMethod("get_look_direction", false, null, false)]
		Vec3 GetLookDirection(UIntPtr agentPointer);

		// Token: 0x06001757 RID: 5975
		[EngineMethod("set_look_direction", false, null, false)]
		void SetLookDirection(UIntPtr agentPointer, Vec3 lookDirection);

		// Token: 0x06001758 RID: 5976
		[EngineMethod("get_look_down_limit", false, null, false)]
		float GetLookDownLimit(UIntPtr agentPointer);

		// Token: 0x06001759 RID: 5977
		[EngineMethod("get_position", false, null, false)]
		Vec3 GetPosition(UIntPtr agentPointer);

		// Token: 0x0600175A RID: 5978
		[EngineMethod("set_position", false, null, false)]
		void SetPosition(UIntPtr agentPointer, ref Vec3 position);

		// Token: 0x0600175B RID: 5979
		[EngineMethod("get_rotation_frame", false, null, false)]
		void GetRotationFrame(UIntPtr agentPointer, ref MatrixFrame outFrame);

		// Token: 0x0600175C RID: 5980
		[EngineMethod("get_eye_global_height", false, null, false)]
		float GetEyeGlobalHeight(UIntPtr agentPointer);

		// Token: 0x0600175D RID: 5981
		[EngineMethod("get_movement_velocity", false, null, false)]
		Vec2 GetMovementVelocity(UIntPtr agentPointer);

		// Token: 0x0600175E RID: 5982
		[EngineMethod("get_average_velocity", false, null, false)]
		Vec3 GetAverageVelocity(UIntPtr agentPointer);

		// Token: 0x0600175F RID: 5983
		[EngineMethod("set_weapon_guard", false, null, false)]
		void SetWeaponGuard(UIntPtr agentPointer, Agent.UsageDirection direction);

		// Token: 0x06001760 RID: 5984
		[EngineMethod("get_is_left_stance", false, null, false)]
		bool GetIsLeftStance(UIntPtr agentPointer);

		// Token: 0x06001761 RID: 5985
		[EngineMethod("invalidate_target_agent", false, null, false)]
		void InvalidateTargetAgent(UIntPtr agentPointer);

		// Token: 0x06001762 RID: 5986
		[EngineMethod("invalidate_ai_weapon_selections", false, null, false)]
		void InvalidateAIWeaponSelections(UIntPtr agentPointer);

		// Token: 0x06001763 RID: 5987
		[EngineMethod("reset_enemy_caches", false, null, false)]
		void ResetEnemyCaches(UIntPtr agentPointer);

		// Token: 0x06001764 RID: 5988
		[EngineMethod("get_ai_state_flags", false, null, true)]
		Agent.AIStateFlag GetAIStateFlags(UIntPtr agentPointer);

		// Token: 0x06001765 RID: 5989
		[EngineMethod("set_ai_alarm_state", false, null, false)]
		void SetAIAlarmState(UIntPtr agentPointer, Agent.AIStateFlag aiStateFlags);

		// Token: 0x06001766 RID: 5990
		[EngineMethod("set_ai_state_flags", false, null, false)]
		void SetAIStateFlags(UIntPtr agentPointer, Agent.AIStateFlag aiStateFlags);

		// Token: 0x06001767 RID: 5991
		[EngineMethod("set_automatic_target_agent_selection", false, null, false)]
		void SetAutomaticTargetSelection(UIntPtr agentPointer, bool enable);

		// Token: 0x06001768 RID: 5992
		[EngineMethod("start_ragdoll_as_corpse", false, null, false)]
		void StartRagdollAsCorpse(UIntPtr agentPointer);

		// Token: 0x06001769 RID: 5993
		[EngineMethod("end_ragdoll_as_corpse", false, null, false)]
		void EndRagdollAsCorpse(UIntPtr agentPointer);

		// Token: 0x0600176A RID: 5994
		[EngineMethod("is_added_as_corpse", false, null, false)]
		bool IsAddedAsCorpse(UIntPtr agentPointer);

		// Token: 0x0600176B RID: 5995
		[EngineMethod("add_as_corpse", false, null, false)]
		void AddAsCorpse(UIntPtr agentPointer);

		// Token: 0x0600176C RID: 5996
		[EngineMethod("set_overriden_strike_and_death_action", false, null, false)]
		void SetOverridenStrikeAndDeathAction(UIntPtr agentPointer, int strikeActionIndex, int deathActionIndex);

		// Token: 0x0600176D RID: 5997
		[EngineMethod("apply_force_on_ragdoll", false, null, false)]
		void ApplyForceOnRagdoll(UIntPtr agentPointer, sbyte boneIndex, in Vec3 force);

		// Token: 0x0600176E RID: 5998
		[EngineMethod("set_velocity_limits_on_ragdoll", false, null, false)]
		void SetVelocityLimitsOnRagdoll(UIntPtr agentPointer, float linearVelocityLimit, float angularVelocityLimit);

		// Token: 0x0600176F RID: 5999
		[EngineMethod("get_state_flags", false, null, false)]
		AgentState GetStateFlags(UIntPtr agentPointer);

		// Token: 0x06001770 RID: 6000
		[EngineMethod("set_state_flags", false, null, false)]
		void SetStateFlags(UIntPtr agentPointer, AgentState StateFlags);

		// Token: 0x06001771 RID: 6001
		[EngineMethod("get_ai_last_suspicious_position", false, null, false)]
		WorldPosition GetAILastSuspiciousPosition(UIntPtr agentPointer);

		// Token: 0x06001772 RID: 6002
		[EngineMethod("set_ai_last_suspicious_position", false, null, false)]
		void SetAILastSuspiciousPosition(UIntPtr agentPointer, in WorldPosition lastSuspiciousPosition);

		// Token: 0x06001773 RID: 6003
		[EngineMethod("get_mount_agent", false, null, false)]
		Agent GetMountAgent(UIntPtr agentPointer);

		// Token: 0x06001774 RID: 6004
		[EngineMethod("set_mount_agent", false, null, false)]
		void SetMountAgent(UIntPtr agentPointer, int mountAgentIndex);

		// Token: 0x06001775 RID: 6005
		[EngineMethod("get_rider_agent", false, null, false)]
		Agent GetRiderAgent(UIntPtr agentPointer);

		// Token: 0x06001776 RID: 6006
		[EngineMethod("set_controller", false, null, false)]
		void SetController(UIntPtr agentPointer, AgentControllerType controller);

		// Token: 0x06001777 RID: 6007
		[EngineMethod("set_initial_frame", false, null, false)]
		void SetInitialFrame(UIntPtr agentPointer, in Vec3 initialPosition, in Vec2 initialDirection, bool canSpawnOutsideOfMissionBoundary);

		// Token: 0x06001778 RID: 6008
		[EngineMethod("weapon_equipped", false, null, false)]
		void WeaponEquipped(UIntPtr agentPointer, int equipmentSlot, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, int ammoWeaponStatsDataLength, UIntPtr weaponEntity, bool removeOldWeaponFromScene, bool isWieldedOnSpawn);

		// Token: 0x06001779 RID: 6009
		[EngineMethod("drop_item", false, null, false)]
		void DropItem(UIntPtr agentPointer, int itemIndex, int pickedUpItemType);

		// Token: 0x0600177A RID: 6010
		[EngineMethod("set_weapon_amount_in_slot", false, null, false)]
		void SetWeaponAmountInSlot(UIntPtr agentPointer, int equipmentSlot, short amount, bool enforcePrimaryItem);

		// Token: 0x0600177B RID: 6011
		[EngineMethod("clear_equipment", false, null, false)]
		void ClearEquipment(UIntPtr agentPointer);

		// Token: 0x0600177C RID: 6012
		[EngineMethod("set_wielded_item_index_as_client", false, null, false)]
		void SetWieldedItemIndexAsClient(UIntPtr agentPointer, int handIndex, int wieldedItemIndex, bool isWieldedInstantly, bool isWieldedOnSpawn, int mainHandCurrentUsageIndex);

		// Token: 0x0600177D RID: 6013
		[EngineMethod("set_usage_index_of_weapon_in_slot_as_client", false, null, false)]
		void SetUsageIndexOfWeaponInSlotAsClient(UIntPtr agentPointer, int slotIndex, int usageIndex);

		// Token: 0x0600177E RID: 6014
		[EngineMethod("set_weapon_hit_points_in_slot", false, null, false)]
		void SetWeaponHitPointsInSlot(UIntPtr agentPointer, int wieldedItemIndex, short hitPoints);

		// Token: 0x0600177F RID: 6015
		[EngineMethod("set_weapon_ammo_as_client", false, null, false)]
		void SetWeaponAmmoAsClient(UIntPtr agentPointer, int equipmentIndex, int ammoEquipmentIndex, short ammo);

		// Token: 0x06001780 RID: 6016
		[EngineMethod("set_weapon_reload_phase_as_client", false, null, false)]
		void SetWeaponReloadPhaseAsClient(UIntPtr agentPointer, int wieldedItemIndex, short reloadPhase);

		// Token: 0x06001781 RID: 6017
		[EngineMethod("set_reload_ammo_in_slot", false, null, false)]
		void SetReloadAmmoInSlot(UIntPtr agentPointer, int slotIndex, int ammoSlotIndex, short reloadedAmmo);

		// Token: 0x06001782 RID: 6018
		[EngineMethod("start_switching_weapon_usage_index_as_client", false, null, false)]
		void StartSwitchingWeaponUsageIndexAsClient(UIntPtr agentPointer, int wieldedItemIndex, int usageIndex, Agent.UsageDirection currentMovementFlagUsageDirection);

		// Token: 0x06001783 RID: 6019
		[EngineMethod("try_to_wield_weapon_in_slot", false, null, false)]
		void TryToWieldWeaponInSlot(UIntPtr agentPointer, int equipmentSlot, int type, bool isWieldedOnSpawn);

		// Token: 0x06001784 RID: 6020
		[EngineMethod("get_weapon_entity_from_equipment_slot", false, null, false)]
		UIntPtr GetWeaponEntityFromEquipmentSlot(UIntPtr agentPointer, int equipmentSlot);

		// Token: 0x06001785 RID: 6021
		[EngineMethod("prepare_weapon_for_drop_in_equipment_slot", false, null, false)]
		void PrepareWeaponForDropInEquipmentSlot(UIntPtr agentPointer, int equipmentSlot, bool dropWithHolster);

		// Token: 0x06001786 RID: 6022
		[EngineMethod("try_to_sheath_weapon_in_hand", false, null, false)]
		void TryToSheathWeaponInHand(UIntPtr agentPointer, int handIndex, int type);

		// Token: 0x06001787 RID: 6023
		[EngineMethod("update_weapons", false, null, false)]
		void UpdateWeapons(UIntPtr agentPointer);

		// Token: 0x06001788 RID: 6024
		[EngineMethod("attach_weapon_to_bone", false, null, false)]
		void AttachWeaponToBone(UIntPtr agentPointer, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, UIntPtr weaponEntity, sbyte boneIndex, ref MatrixFrame attachLocalFrame);

		// Token: 0x06001789 RID: 6025
		[EngineMethod("delete_attached_weapon_from_bone", false, null, false)]
		void DeleteAttachedWeaponFromBone(UIntPtr agentPointer, int attachedWeaponIndex);

		// Token: 0x0600178A RID: 6026
		[EngineMethod("attach_weapon_to_weapon_in_slot", false, null, false)]
		void AttachWeaponToWeaponInSlot(UIntPtr agentPointer, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, UIntPtr weaponEntity, int slotIndex, ref MatrixFrame attachLocalFrame);

		// Token: 0x0600178B RID: 6027
		[EngineMethod("build", false, null, false)]
		void Build(UIntPtr agentPointer, Vec3 eyeOffsetWrtHead);

		// Token: 0x0600178C RID: 6028
		[EngineMethod("lock_agent_replication_table_with_current_reliable_sequence_no", false, null, false)]
		void LockAgentReplicationTableDataWithCurrentReliableSequenceNo(UIntPtr agentPointer, int peerIndex);

		// Token: 0x0600178D RID: 6029
		[EngineMethod("set_agent_exclude_state_for_face_group_id", false, null, false)]
		void SetAgentExcludeStateForFaceGroupId(UIntPtr agentPointer, int faceGroupId, bool isExcluded);

		// Token: 0x0600178E RID: 6030
		[EngineMethod("set_agent_scale", false, null, false)]
		void SetAgentScale(UIntPtr agentPointer, float scale);

		// Token: 0x0600178F RID: 6031
		[EngineMethod("initialize_agent_record", false, null, false)]
		void InitializeAgentRecord(UIntPtr agentPointer);

		// Token: 0x06001790 RID: 6032
		[EngineMethod("get_current_velocity", false, null, false)]
		Vec2 GetCurrentVelocity(UIntPtr agentPointer);

		// Token: 0x06001791 RID: 6033
		[EngineMethod("get_turn_speed", false, null, false)]
		float GetTurnSpeed(UIntPtr agentPointer);

		// Token: 0x06001792 RID: 6034
		[EngineMethod("set_movement_direction", false, null, false)]
		void SetMovementDirection(UIntPtr agentPointer, in Vec2 direction);

		// Token: 0x06001793 RID: 6035
		[EngineMethod("get_current_speed_limit", false, null, false)]
		float GetCurrentSpeedLimit(UIntPtr agentPointer);

		// Token: 0x06001794 RID: 6036
		[EngineMethod("set_maximum_speed_limit", false, null, true)]
		void SetMaximumSpeedLimit(UIntPtr agentPointer, float maximumSpeedLimit, bool isMultiplier);

		// Token: 0x06001795 RID: 6037
		[EngineMethod("get_maximum_speed_limit", false, null, false)]
		float GetMaximumSpeedLimit(UIntPtr agentPointer);

		// Token: 0x06001796 RID: 6038
		[EngineMethod("get_real_global_velocity", false, null, false)]
		Vec3 GetRealGlobalVelocity(UIntPtr agentPointer);

		// Token: 0x06001797 RID: 6039
		[EngineMethod("get_average_real_global_velocity", false, null, false)]
		Vec3 GetAverageRealGlobalVelocity(UIntPtr agentPointer);

		// Token: 0x06001798 RID: 6040
		[EngineMethod("fade_out", false, null, false)]
		void FadeOut(UIntPtr agentPointer, bool hideInstantly);

		// Token: 0x06001799 RID: 6041
		[EngineMethod("fade_in", false, null, false)]
		void FadeIn(UIntPtr agentPointer);

		// Token: 0x0600179A RID: 6042
		[EngineMethod("get_scripted_flags", false, null, false)]
		int GetScriptedFlags(UIntPtr agentPointer);

		// Token: 0x0600179B RID: 6043
		[EngineMethod("set_scripted_flags", false, null, false)]
		void SetScriptedFlags(UIntPtr agentPointer, int flags);

		// Token: 0x0600179C RID: 6044
		[EngineMethod("get_scripted_combat_flags", false, null, false)]
		int GetScriptedCombatFlags(UIntPtr agentPointer);

		// Token: 0x0600179D RID: 6045
		[EngineMethod("set_scripted_combat_flags", false, null, false)]
		void SetScriptedCombatFlags(UIntPtr agentPointer, int flags);

		// Token: 0x0600179E RID: 6046
		[EngineMethod("set_scripted_position_and_direction", false, null, false)]
		bool SetScriptedPositionAndDirection(UIntPtr agentPointer, ref WorldPosition targetPosition, float targetDirection, bool addHumanLikeDelay, int additionalFlags);

		// Token: 0x0600179F RID: 6047
		[EngineMethod("set_scripted_position", false, null, false)]
		bool SetScriptedPosition(UIntPtr agentPointer, ref WorldPosition targetPosition, bool addHumanLikeDelay, int additionalFlags);

		// Token: 0x060017A0 RID: 6048
		[EngineMethod("set_scripted_target_entity", false, null, false)]
		void SetScriptedTargetEntity(UIntPtr agentPointer, UIntPtr entityId, int additionalFlags, bool ignoreIfAlreadyAttacking);

		// Token: 0x060017A1 RID: 6049
		[EngineMethod("disable_scripted_movement", false, null, false)]
		void DisableScriptedMovement(UIntPtr agentPointer);

		// Token: 0x060017A2 RID: 6050
		[EngineMethod("disable_scripted_combat_movement", false, null, false)]
		void DisableScriptedCombatMovement(UIntPtr agentPointer);

		// Token: 0x060017A3 RID: 6051
		[EngineMethod("force_ai_behavior_selection", false, null, false)]
		void ForceAiBehaviorSelection(UIntPtr agentPointer);

		// Token: 0x060017A4 RID: 6052
		[EngineMethod("has_path_through_navigation_face_id_from_direction", false, null, false)]
		bool HasPathThroughNavigationFaceIdFromDirection(UIntPtr agentPointer, int navigationFaceId, ref Vec2 direction);

		// Token: 0x060017A5 RID: 6053
		[EngineMethod("has_path_through_navigation_faces_id_from_direction", false, null, false)]
		bool HasPathThroughNavigationFacesIDFromDirection(UIntPtr agentPointer, int navigationFaceID_1, int navigationFaceID_2, int navigationFaceID_3, ref Vec2 direction);

		// Token: 0x060017A6 RID: 6054
		[EngineMethod("can_move_directly_to_position", false, null, false)]
		bool CanMoveDirectlyToPosition(UIntPtr agentPointer, in Vec2 position);

		// Token: 0x060017A7 RID: 6055
		[EngineMethod("check_path_to_ai_target_agent_passes_through_navigation_face_id_from_direction", false, null, false)]
		bool CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirection(UIntPtr agentPointer, int navigationFaceId, in Vec3 direction, float overridenCostForFaceId);

		// Token: 0x060017A8 RID: 6056
		[EngineMethod("is_target_navigation_face_id_between", false, null, false)]
		bool IsTargetNavigationFaceIdBetween(UIntPtr agentPointer, int navigationFaceIdStart, int navigationFaceIdEnd);

		// Token: 0x060017A9 RID: 6057
		[EngineMethod("get_path_distance_to_point", false, null, false)]
		float GetPathDistanceToPoint(UIntPtr agentPointer, ref Vec3 direction);

		// Token: 0x060017AA RID: 6058
		[EngineMethod("get_current_navigation_face_id", false, null, true)]
		int GetCurrentNavigationFaceId(UIntPtr agentPointer);

		// Token: 0x060017AB RID: 6059
		[EngineMethod("get_world_position", false, null, false)]
		WorldPosition GetWorldPosition(UIntPtr agentPointer);

		// Token: 0x060017AC RID: 6060
		[EngineMethod("set_agent_facial_animation", false, null, false)]
		void SetAgentFacialAnimation(UIntPtr agentPointer, int channel, string animationName, bool loop);

		// Token: 0x060017AD RID: 6061
		[EngineMethod("get_agent_facial_animation", false, null, false)]
		string GetAgentFacialAnimation(UIntPtr agentPointer);

		// Token: 0x060017AE RID: 6062
		[EngineMethod("get_agent_voice_definiton", false, null, false)]
		string GetAgentVoiceDefinition(UIntPtr agentPointer);

		// Token: 0x060017AF RID: 6063
		[EngineMethod("get_current_animation_flags", false, null, false)]
		ulong GetCurrentAnimationFlags(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B0 RID: 6064
		[EngineMethod("get_current_action_type", false, null, true)]
		int GetCurrentActionType(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B1 RID: 6065
		[EngineMethod("get_current_action_stage", false, null, true)]
		int GetCurrentActionStage(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B2 RID: 6066
		[EngineMethod("get_current_action_direction", false, null, true)]
		int GetCurrentActionDirection(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B3 RID: 6067
		[EngineMethod("compute_animation_displacement", false, null, false)]
		Vec3 ComputeAnimationDisplacement(UIntPtr agentPointer, float dt);

		// Token: 0x060017B4 RID: 6068
		[EngineMethod("get_current_action_priority", false, null, true)]
		int GetCurrentActionPriority(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B5 RID: 6069
		[EngineMethod("get_current_action_progress", false, null, true)]
		float GetCurrentActionProgress(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017B6 RID: 6070
		[EngineMethod("set_current_action_progress", false, null, true)]
		void SetCurrentActionProgress(UIntPtr agentPointer, int channelNo, float progress);

		// Token: 0x060017B7 RID: 6071
		[EngineMethod("set_action_channel", false, null, true)]
		bool SetActionChannel(UIntPtr agentPointer, int channelNo, int actionNo, ulong additionalFlags, bool ignorePriority, float blendWithNextActionFactor, float actionSpeed, float blendInPeriod, float blendOutPeriodToNoAnim, float startProgress, bool useLinearSmoothing, float blendOutPeriod, bool forceFaceMorphRestart);

		// Token: 0x060017B8 RID: 6072
		[EngineMethod("set_current_action_speed", false, null, false)]
		void SetCurrentActionSpeed(UIntPtr agentPointer, int channelNo, float actionSpeed);

		// Token: 0x060017B9 RID: 6073
		[EngineMethod("tick_action_channels", false, null, false)]
		void TickActionChannels(UIntPtr agentPointer, float dt);

		// Token: 0x060017BA RID: 6074
		[EngineMethod("get_action_channel_weight", false, null, false)]
		float GetActionChannelWeight(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017BB RID: 6075
		[EngineMethod("get_action_channel_current_action_weight", false, null, false)]
		float GetActionChannelCurrentActionWeight(UIntPtr agentPointer, int channelNo);

		// Token: 0x060017BC RID: 6076
		[EngineMethod("set_action_set", false, null, false)]
		void SetActionSet(UIntPtr agentPointer, ref AnimationSystemData animationSystemData);

		// Token: 0x060017BD RID: 6077
		[EngineMethod("get_action_set_no", false, null, true)]
		int GetActionSetNo(UIntPtr agentPointer);

		// Token: 0x060017BE RID: 6078
		[EngineMethod("get_movement_locked_state", false, null, false)]
		AgentMovementLockedState GetMovementLockedState(UIntPtr agentPointer);

		// Token: 0x060017BF RID: 6079
		[EngineMethod("get_aiming_timer", false, null, false)]
		float GetAimingTimer(UIntPtr agentPointer);

		// Token: 0x060017C0 RID: 6080
		[EngineMethod("get_target_position", false, null, false)]
		Vec2 GetTargetPosition(UIntPtr agentPointer);

		// Token: 0x060017C1 RID: 6081
		[EngineMethod("set_target_position", false, null, false)]
		void SetTargetPosition(UIntPtr agentPointer, ref Vec2 targetPosition);

		// Token: 0x060017C2 RID: 6082
		[EngineMethod("set_target_z", false, null, false)]
		void SetTargetZ(UIntPtr agentPointer, float targetZ);

		// Token: 0x060017C3 RID: 6083
		[EngineMethod("clear_target_z", false, null, false)]
		void ClearTargetZ(UIntPtr agentPointer);

		// Token: 0x060017C4 RID: 6084
		[EngineMethod("set_target_up", false, null, false)]
		void SetTargetUp(UIntPtr agentPointer, in Vec3 targetUp);

		// Token: 0x060017C5 RID: 6085
		[EngineMethod("get_target_direction", false, null, false)]
		Vec3 GetTargetDirection(UIntPtr agentPointer);

		// Token: 0x060017C6 RID: 6086
		[EngineMethod("set_target_position_and_direction", false, null, true)]
		void SetTargetPositionAndDirection(UIntPtr agentPointer, in Vec2 targetPosition, in Vec3 targetDirection);

		// Token: 0x060017C7 RID: 6087
		[EngineMethod("add_acceleration", false, null, false)]
		void AddAcceleration(UIntPtr agentPointer, in Vec3 acceleration);

		// Token: 0x060017C8 RID: 6088
		[EngineMethod("clear_target_frame", false, null, false)]
		void ClearTargetFrame(UIntPtr agentPointer);

		// Token: 0x060017C9 RID: 6089
		[EngineMethod("get_is_look_direction_locked", false, null, false)]
		bool GetIsLookDirectionLocked(UIntPtr agentPointer);

		// Token: 0x060017CA RID: 6090
		[EngineMethod("set_is_look_direction_locked", false, null, false)]
		void SetIsLookDirectionLocked(UIntPtr agentPointer, bool isLocked);

		// Token: 0x060017CB RID: 6091
		[EngineMethod("set_mono_object", false, null, false)]
		void SetMonoObject(UIntPtr agentPointer, Agent monoObject);

		// Token: 0x060017CC RID: 6092
		[EngineMethod("get_eye_global_position", false, null, false)]
		Vec3 GetEyeGlobalPosition(UIntPtr agentPointer);

		// Token: 0x060017CD RID: 6093
		[EngineMethod("get_chest_global_position", false, null, false)]
		Vec3 GetChestGlobalPosition(UIntPtr agentPointer);

		// Token: 0x060017CE RID: 6094
		[EngineMethod("add_mesh_to_bone", false, null, false)]
		void AddMeshToBone(UIntPtr agentPointer, UIntPtr meshPointer, sbyte boneIndex);

		// Token: 0x060017CF RID: 6095
		[EngineMethod("remove_mesh_from_bone", false, null, false)]
		void RemoveMeshFromBone(UIntPtr agentPointer, UIntPtr meshPointer, sbyte boneIndex);

		// Token: 0x060017D0 RID: 6096
		[EngineMethod("add_prefab_to_agent_bone", false, null, false)]
		CompositeComponent AddPrefabToAgentBone(UIntPtr agentPointer, string prefabName, sbyte boneIndex);

		// Token: 0x060017D1 RID: 6097
		[EngineMethod("wield_next_weapon", false, null, false)]
		void WieldNextWeapon(UIntPtr agentPointer, int handIndex, int wieldActionType);

		// Token: 0x060017D2 RID: 6098
		[EngineMethod("preload_for_rendering", false, null, false)]
		void PreloadForRendering(UIntPtr agentPointer);

		// Token: 0x060017D3 RID: 6099
		[EngineMethod("get_ai_move_stop_tolerance", false, null, false)]
		float GetAIMoveStopTolerance(UIntPtr agentPointer);

		// Token: 0x060017D4 RID: 6100
		[EngineMethod("get_agent_scale", false, null, true)]
		float GetAgentScale(UIntPtr agentPointer);

		// Token: 0x060017D5 RID: 6101
		[EngineMethod("get_ai_move_destination", false, null, false)]
		WorldPosition GetAIMoveDestination(UIntPtr agentPointer);

		// Token: 0x060017D6 RID: 6102
		[EngineMethod("find_longest_direct_move_to_position", false, null, false)]
		Vec2 FindLongestDirectMoveToPosition(UIntPtr agentPointer, Vec2 targetPosition, bool checkBoundaries, bool checkFriendlyAgents, out bool isCollidedWithAgent);

		// Token: 0x060017D7 RID: 6103
		[EngineMethod("get_crouch_mode", false, null, false)]
		bool GetCrouchMode(UIntPtr agentPointer);

		// Token: 0x060017D8 RID: 6104
		[EngineMethod("get_walk_mode", false, null, false)]
		bool GetWalkMode(UIntPtr agentPointer);

		// Token: 0x060017D9 RID: 6105
		[EngineMethod("get_visual_position", false, null, false)]
		Vec3 GetVisualPosition(UIntPtr agentPointer);

		// Token: 0x060017DA RID: 6106
		[EngineMethod("is_look_rotation_in_slow_motion", false, null, false)]
		bool IsLookRotationInSlowMotion(UIntPtr agentPointer);

		// Token: 0x060017DB RID: 6107
		[EngineMethod("get_look_direction_as_angle", false, null, false)]
		float GetLookDirectionAsAngle(UIntPtr agentPointer);

		// Token: 0x060017DC RID: 6108
		[EngineMethod("set_look_direction_as_angle", false, null, false)]
		void SetLookDirectionAsAngle(UIntPtr agentPointer, float value);

		// Token: 0x060017DD RID: 6109
		[EngineMethod("attack_direction_to_movement_flag", false, null, false)]
		Agent.MovementControlFlag AttackDirectionToMovementFlag(UIntPtr agentPointer, Agent.UsageDirection direction);

		// Token: 0x060017DE RID: 6110
		[EngineMethod("defend_direction_to_movement_flag", false, null, false)]
		Agent.MovementControlFlag DefendDirectionToMovementFlag(UIntPtr agentPointer, Agent.UsageDirection direction);

		// Token: 0x060017DF RID: 6111
		[EngineMethod("get_head_camera_mode", false, null, false)]
		bool GetHeadCameraMode(UIntPtr agentPointer);

		// Token: 0x060017E0 RID: 6112
		[EngineMethod("set_head_camera_mode", false, null, false)]
		void SetHeadCameraMode(UIntPtr agentPointer, bool value);

		// Token: 0x060017E1 RID: 6113
		[EngineMethod("kick_clear", false, null, false)]
		bool KickClear(UIntPtr agentPointer);

		// Token: 0x060017E2 RID: 6114
		[EngineMethod("reset_guard", false, null, false)]
		void ResetGuard(UIntPtr agentPointer);

		// Token: 0x060017E3 RID: 6115
		[EngineMethod("get_current_guard_mode", false, null, false)]
		Agent.GuardMode GetCurrentGuardMode(UIntPtr agentPointer);

		// Token: 0x060017E4 RID: 6116
		[EngineMethod("get_defend_movement_flag", false, null, false)]
		Agent.MovementControlFlag GetDefendMovementFlag(UIntPtr agentPointer);

		// Token: 0x060017E5 RID: 6117
		[EngineMethod("get_attack_direction", false, null, false)]
		Agent.UsageDirection GetAttackDirection(UIntPtr agentPointer);

		// Token: 0x060017E6 RID: 6118
		[EngineMethod("player_attack_direction", false, null, false)]
		Agent.UsageDirection PlayerAttackDirection(UIntPtr agentPointer);

		// Token: 0x060017E7 RID: 6119
		[EngineMethod("get_wielded_weapon_info", false, null, false)]
		bool GetWieldedWeaponInfo(UIntPtr agentPointer, int handIndex, ref bool isMeleeWeapon, ref bool isRangedWeapon);

		// Token: 0x060017E8 RID: 6120
		[EngineMethod("get_immediate_enemy", false, null, false)]
		Agent GetImmediateEnemy(UIntPtr agentPointer);

		// Token: 0x060017E9 RID: 6121
		[EngineMethod("try_get_immediate_agent_movement_data", false, null, false)]
		bool TryGetImmediateEnemyAgentMovementData(UIntPtr agentPointer, out float maximumForwardUnlimitedSpeed, out Vec3 position);

		// Token: 0x060017EA RID: 6122
		[EngineMethod("get_is_doing_passive_attack", false, null, false)]
		bool GetIsDoingPassiveAttack(UIntPtr agentPointer);

		// Token: 0x060017EB RID: 6123
		[EngineMethod("get_is_passive_usage_conditions_are_met", false, null, false)]
		bool GetIsPassiveUsageConditionsAreMet(UIntPtr agentPointer);

		// Token: 0x060017EC RID: 6124
		[EngineMethod("get_current_aiming_turbulance", false, null, false)]
		float GetCurrentAimingTurbulance(UIntPtr agentPointer);

		// Token: 0x060017ED RID: 6125
		[EngineMethod("get_current_aiming_error", false, null, false)]
		float GetCurrentAimingError(UIntPtr agentPointer);

		// Token: 0x060017EE RID: 6126
		[EngineMethod("get_body_rotation_constraint", false, null, false)]
		Vec3 GetBodyRotationConstraint(UIntPtr agentPointer, int channelIndex);

		// Token: 0x060017EF RID: 6127
		[EngineMethod("get_total_mass", false, null, false)]
		float GetTotalMass(UIntPtr agentPointer);

		// Token: 0x060017F0 RID: 6128
		[EngineMethod("get_action_direction", false, null, false)]
		Agent.UsageDirection GetActionDirection(int actionIndex);

		// Token: 0x060017F1 RID: 6129
		[EngineMethod("get_attack_direction_usage", false, null, false)]
		Agent.UsageDirection GetAttackDirectionUsage(UIntPtr agentPointer);

		// Token: 0x060017F2 RID: 6130
		[EngineMethod("handle_blow_aux", false, null, false)]
		void HandleBlowAux(UIntPtr agentPointer, ref Blow blow);

		// Token: 0x060017F3 RID: 6131
		[EngineMethod("get_quick_bone_entitial_frame", false, null, false)]
		void GetBoneEntitialFrame(UIntPtr agentPointer, sbyte boneIndex, bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x060017F4 RID: 6132
		[EngineMethod("make_voice", false, null, false)]
		void MakeVoice(UIntPtr agentPointer, int voiceType, int predictionType);

		// Token: 0x060017F5 RID: 6133
		[EngineMethod("yell_after_delay", false, null, false)]
		void YellAfterDelay(UIntPtr agentPointer, float delayTimeInSecond);

		// Token: 0x060017F6 RID: 6134
		[EngineMethod("set_hand_inverse_kinematics_frame", false, null, false)]
		bool SetHandInverseKinematicsFrame(UIntPtr agentPointer, in MatrixFrame leftGlobalFrame, in MatrixFrame rightGlobalFrame);

		// Token: 0x060017F7 RID: 6135
		[EngineMethod("set_hand_inverse_kinematics_frame_for_mission_object_usage", false, null, false)]
		bool SetHandInverseKinematicsFrameForMissionObjectUsage(UIntPtr agentPointer, in MatrixFrame localIKFrame, in MatrixFrame boundEntityGlobalFrame, float animationHeightDifference);

		// Token: 0x060017F8 RID: 6136
		[EngineMethod("clear_hand_inverse_kinematics", false, null, false)]
		void ClearHandInverseKinematics(UIntPtr agentPointer);

		// Token: 0x060017F9 RID: 6137
		[EngineMethod("debug_more", false, null, false)]
		void DebugMore(UIntPtr agentPointer);

		// Token: 0x060017FA RID: 6138
		[EngineMethod("get_agent_parent_entity", false, null, false)]
		GameEntity GetAgentParentEntity(UIntPtr agentPointer);

		// Token: 0x060017FB RID: 6139
		[EngineMethod("set_excluded_from_gravity", false, null, false)]
		void SetExcludedFromGravity(UIntPtr agentPointer, bool exclude, bool applyAverageGlobalVelocity);

		// Token: 0x060017FC RID: 6140
		[EngineMethod("set_force_attached_entity", false, null, false)]
		void SetForceAttachedEntity(UIntPtr agentPointer, UIntPtr entityPointer);

		// Token: 0x060017FD RID: 6141
		[EngineMethod("is_sliding", false, null, false)]
		bool IsSliding(UIntPtr agentPointer);

		// Token: 0x060017FE RID: 6142
		[EngineMethod("is_running_away", false, null, false)]
		bool IsRunningAway(UIntPtr agentPointer);

		// Token: 0x060017FF RID: 6143
		[EngineMethod("is_crouching_allowed", false, null, false)]
		bool IsCrouchingAllowed(UIntPtr agentPointer);

		// Token: 0x06001800 RID: 6144
		[EngineMethod("get_cur_weapon_offset", false, null, false)]
		Vec3 GetCurWeaponOffset(UIntPtr agentPointer);

		// Token: 0x06001801 RID: 6145
		[EngineMethod("get_walking_speed_limit_of_mountable", false, null, false)]
		float GetWalkSpeedLimitOfMountable(UIntPtr agentPointer);

		// Token: 0x06001802 RID: 6146
		[EngineMethod("create_blood_burst_at_limb", false, null, false)]
		void CreateBloodBurstAtLimb(UIntPtr agentPointer, sbyte realBoneIndex, float scale);

		// Token: 0x06001803 RID: 6147
		[EngineMethod("get_native_action_index", false, null, false)]
		int GetNativeActionIndex(string actionName);

		// Token: 0x06001804 RID: 6148
		[EngineMethod("set_columnwise_follow_agent", false, null, false)]
		void SetColumnwiseFollowAgent(UIntPtr agentPointer, int followAgentIndex, ref Vec2 followPosition);

		// Token: 0x06001805 RID: 6149
		[EngineMethod("get_monster_usage_index", false, null, false)]
		int GetMonsterUsageIndex(string monsterUsage);

		// Token: 0x06001806 RID: 6150
		[EngineMethod("get_missile_range_with_height_difference", false, null, false)]
		float GetMissileRangeWithHeightDifference(UIntPtr agentPointer, float targetZ);

		// Token: 0x06001807 RID: 6151
		[EngineMethod("set_formation_no", false, null, false)]
		void SetFormationNo(UIntPtr agentPointer, int formationNo);

		// Token: 0x06001808 RID: 6152
		[EngineMethod("enforce_shield_usage", false, null, false)]
		void EnforceShieldUsage(UIntPtr agentPointer, Agent.UsageDirection direction);

		// Token: 0x06001809 RID: 6153
		[EngineMethod("set_firing_order", false, null, false)]
		void SetFiringOrder(UIntPtr agentPointer, int order);

		// Token: 0x0600180A RID: 6154
		[EngineMethod("set_riding_order", false, null, false)]
		void SetRidingOrder(UIntPtr agentPointer, int order);

		// Token: 0x0600180B RID: 6155
		[EngineMethod("get_target_formation_index", false, null, false)]
		int GetTargetFormationIndex(UIntPtr agentPointer);

		// Token: 0x0600180C RID: 6156
		[EngineMethod("set_target_formation_index", false, null, false)]
		void SetTargetFormationIndex(UIntPtr agentPointer, int targetFormationIndex);

		// Token: 0x0600180D RID: 6157
		[EngineMethod("set_direction_change_tendency", false, null, true)]
		void SetDirectionChangeTendency(UIntPtr agentPointer, float tendency);

		// Token: 0x0600180E RID: 6158
		[EngineMethod("set_ai_behavior_params", false, null, false)]
		void SetAIBehaviorParams(UIntPtr agentPointer, int behavior, float y1, float x2, float y2, float x3, float y3);

		// Token: 0x0600180F RID: 6159
		[EngineMethod("set_all_ai_behavior_params", false, null, false)]
		void SetAllAIBehaviorParams(UIntPtr agentPointer, HumanAIComponent.BehaviorValues[] behaviorParams);

		// Token: 0x06001810 RID: 6160
		[EngineMethod("set_body_armor_material_type", false, null, false)]
		void SetBodyArmorMaterialType(UIntPtr agentPointer, ArmorComponent.ArmorMaterialTypes bodyArmorMaterialType);

		// Token: 0x06001811 RID: 6161
		[EngineMethod("get_maximum_number_of_agents", false, null, false)]
		int GetMaximumNumberOfAgents();

		// Token: 0x06001812 RID: 6162
		[EngineMethod("get_running_simulation_data_until_maximum_speed_reached", false, null, false)]
		void GetRunningSimulationDataUntilMaximumSpeedReached(UIntPtr agentPointer, ref float combatAccelerationTime, ref float maxSpeed, float[] speedValues);

		// Token: 0x06001813 RID: 6163
		[EngineMethod("get_last_target_visibility_state", false, null, false)]
		int GetLastTargetVisibilityState(UIntPtr agentPointer);

		// Token: 0x06001814 RID: 6164
		[EngineMethod("get_missile_range", false, null, false)]
		float GetMissileRange(UIntPtr agentPointer);

		// Token: 0x06001815 RID: 6165
		[EngineMethod("set_agent_idle_animation_status", false, null, false)]
		void SetAgentIdleAnimationStatus(UIntPtr agentPointer, bool idleEnabled);

		// Token: 0x06001816 RID: 6166
		[EngineMethod("get_old_wielded_item_info", false, null, false)]
		void GetOldWieldedItemInfo(UIntPtr agentPointer, out int rightHandSlotIndex, out int rightHandUsageIndex, out int leftHandSlotIndex, out int leftHandUsageIndex);

		// Token: 0x06001817 RID: 6167
		[EngineMethod("get_ground_material_for_collision_effect", false, null, false)]
		int GetGroundMaterialForCollisionEffect(UIntPtr agentPointer);

		// Token: 0x06001818 RID: 6168
		[EngineMethod("get_bone_entitial_frame_at_animation_progress", false, null, true)]
		MatrixFrame GetBoneEntitialFrameAtAnimationProgress(UIntPtr agentPointer, sbyte boneIndex, int animationIndex, float progress);
	}
}
