using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedCallbacks
{
	// Token: 0x02000008 RID: 8
	internal static class ScriptingInterfaceObjects
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public static Dictionary<string, object> GetObjects()
		{
			return new Dictionary<string, object>
			{
				{
					"TaleWorlds.MountAndBlade.IMBActionSet",
					new ScriptingInterfaceOfIMBActionSet()
				},
				{
					"TaleWorlds.MountAndBlade.IMBAgent",
					new ScriptingInterfaceOfIMBAgent()
				},
				{
					"TaleWorlds.MountAndBlade.IMBAgentVisuals",
					new ScriptingInterfaceOfIMBAgentVisuals()
				},
				{
					"TaleWorlds.MountAndBlade.IMBAnimation",
					new ScriptingInterfaceOfIMBAnimation()
				},
				{
					"TaleWorlds.MountAndBlade.IMBBannerlordChecker",
					new ScriptingInterfaceOfIMBBannerlordChecker()
				},
				{
					"TaleWorlds.MountAndBlade.IMBBannerlordConfig",
					new ScriptingInterfaceOfIMBBannerlordConfig()
				},
				{
					"TaleWorlds.MountAndBlade.IMBBannerlordTableauManager",
					new ScriptingInterfaceOfIMBBannerlordTableauManager()
				},
				{
					"TaleWorlds.MountAndBlade.IMBDebugExtensions",
					new ScriptingInterfaceOfIMBDebugExtensions()
				},
				{
					"TaleWorlds.MountAndBlade.IMBDelegate",
					new ScriptingInterfaceOfIMBDelegate()
				},
				{
					"TaleWorlds.MountAndBlade.IMBEditor",
					new ScriptingInterfaceOfIMBEditor()
				},
				{
					"TaleWorlds.MountAndBlade.IMBFaceGen",
					new ScriptingInterfaceOfIMBFaceGen()
				},
				{
					"TaleWorlds.MountAndBlade.IMBGame",
					new ScriptingInterfaceOfIMBGame()
				},
				{
					"TaleWorlds.MountAndBlade.IMBGameEntityExtensions",
					new ScriptingInterfaceOfIMBGameEntityExtensions()
				},
				{
					"TaleWorlds.MountAndBlade.IMBItem",
					new ScriptingInterfaceOfIMBItem()
				},
				{
					"TaleWorlds.MountAndBlade.IMBMapScene",
					new ScriptingInterfaceOfIMBMapScene()
				},
				{
					"TaleWorlds.MountAndBlade.IMBMessageManager",
					new ScriptingInterfaceOfIMBMessageManager()
				},
				{
					"TaleWorlds.MountAndBlade.IMBMission",
					new ScriptingInterfaceOfIMBMission()
				},
				{
					"TaleWorlds.MountAndBlade.IMBMultiplayerData",
					new ScriptingInterfaceOfIMBMultiplayerData()
				},
				{
					"TaleWorlds.MountAndBlade.IMBNetwork",
					new ScriptingInterfaceOfIMBNetwork()
				},
				{
					"TaleWorlds.MountAndBlade.IMBPeer",
					new ScriptingInterfaceOfIMBPeer()
				},
				{
					"TaleWorlds.MountAndBlade.IMBScreen",
					new ScriptingInterfaceOfIMBScreen()
				},
				{
					"TaleWorlds.MountAndBlade.IMBSkeletonExtensions",
					new ScriptingInterfaceOfIMBSkeletonExtensions()
				},
				{
					"TaleWorlds.MountAndBlade.IMBSoundEvent",
					new ScriptingInterfaceOfIMBSoundEvent()
				},
				{
					"TaleWorlds.MountAndBlade.IMBTeam",
					new ScriptingInterfaceOfIMBTeam()
				},
				{
					"TaleWorlds.MountAndBlade.IMBTestRun",
					new ScriptingInterfaceOfIMBTestRun()
				},
				{
					"TaleWorlds.MountAndBlade.IMBVoiceManager",
					new ScriptingInterfaceOfIMBVoiceManager()
				},
				{
					"TaleWorlds.MountAndBlade.IMBWindowManager",
					new ScriptingInterfaceOfIMBWindowManager()
				},
				{
					"TaleWorlds.MountAndBlade.IMBWorld",
					new ScriptingInterfaceOfIMBWorld()
				}
			};
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public static void SetFunctionPointer(int id, IntPtr pointer)
		{
			switch (id)
			{
			case 0:
				ScriptingInterfaceOfIMBActionSet.call_AreActionsAlternativesDelegate = (ScriptingInterfaceOfIMBActionSet.AreActionsAlternativesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.AreActionsAlternativesDelegate));
				return;
			case 1:
				ScriptingInterfaceOfIMBActionSet.call_GetAnimationNameDelegate = (ScriptingInterfaceOfIMBActionSet.GetAnimationNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetAnimationNameDelegate));
				return;
			case 2:
				ScriptingInterfaceOfIMBActionSet.call_GetBoneHasParentBoneDelegate = (ScriptingInterfaceOfIMBActionSet.GetBoneHasParentBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetBoneHasParentBoneDelegate));
				return;
			case 3:
				ScriptingInterfaceOfIMBActionSet.call_GetBoneIndexWithIdDelegate = (ScriptingInterfaceOfIMBActionSet.GetBoneIndexWithIdDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetBoneIndexWithIdDelegate));
				return;
			case 4:
				ScriptingInterfaceOfIMBActionSet.call_GetIndexWithIDDelegate = (ScriptingInterfaceOfIMBActionSet.GetIndexWithIDDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetIndexWithIDDelegate));
				return;
			case 5:
				ScriptingInterfaceOfIMBActionSet.call_GetNameWithIndexDelegate = (ScriptingInterfaceOfIMBActionSet.GetNameWithIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetNameWithIndexDelegate));
				return;
			case 6:
				ScriptingInterfaceOfIMBActionSet.call_GetNumberOfActionSetsDelegate = (ScriptingInterfaceOfIMBActionSet.GetNumberOfActionSetsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetNumberOfActionSetsDelegate));
				return;
			case 7:
				ScriptingInterfaceOfIMBActionSet.call_GetNumberOfMonsterUsageSetsDelegate = (ScriptingInterfaceOfIMBActionSet.GetNumberOfMonsterUsageSetsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetNumberOfMonsterUsageSetsDelegate));
				return;
			case 8:
				ScriptingInterfaceOfIMBActionSet.call_GetSkeletonNameDelegate = (ScriptingInterfaceOfIMBActionSet.GetSkeletonNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBActionSet.GetSkeletonNameDelegate));
				return;
			case 9:
				ScriptingInterfaceOfIMBAgent.call_AddAccelerationDelegate = (ScriptingInterfaceOfIMBAgent.AddAccelerationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AddAccelerationDelegate));
				return;
			case 10:
				ScriptingInterfaceOfIMBAgent.call_AddAsCorpseDelegate = (ScriptingInterfaceOfIMBAgent.AddAsCorpseDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AddAsCorpseDelegate));
				return;
			case 11:
				ScriptingInterfaceOfIMBAgent.call_AddMeshToBoneDelegate = (ScriptingInterfaceOfIMBAgent.AddMeshToBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AddMeshToBoneDelegate));
				return;
			case 12:
				ScriptingInterfaceOfIMBAgent.call_AddPrefabToAgentBoneDelegate = (ScriptingInterfaceOfIMBAgent.AddPrefabToAgentBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AddPrefabToAgentBoneDelegate));
				return;
			case 13:
				ScriptingInterfaceOfIMBAgent.call_ApplyForceOnRagdollDelegate = (ScriptingInterfaceOfIMBAgent.ApplyForceOnRagdollDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ApplyForceOnRagdollDelegate));
				return;
			case 14:
				ScriptingInterfaceOfIMBAgent.call_AttachWeaponToBoneDelegate = (ScriptingInterfaceOfIMBAgent.AttachWeaponToBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AttachWeaponToBoneDelegate));
				return;
			case 15:
				ScriptingInterfaceOfIMBAgent.call_AttachWeaponToWeaponInSlotDelegate = (ScriptingInterfaceOfIMBAgent.AttachWeaponToWeaponInSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AttachWeaponToWeaponInSlotDelegate));
				return;
			case 16:
				ScriptingInterfaceOfIMBAgent.call_AttackDirectionToMovementFlagDelegate = (ScriptingInterfaceOfIMBAgent.AttackDirectionToMovementFlagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.AttackDirectionToMovementFlagDelegate));
				return;
			case 17:
				ScriptingInterfaceOfIMBAgent.call_BuildDelegate = (ScriptingInterfaceOfIMBAgent.BuildDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.BuildDelegate));
				return;
			case 18:
				ScriptingInterfaceOfIMBAgent.call_CanMoveDirectlyToPositionDelegate = (ScriptingInterfaceOfIMBAgent.CanMoveDirectlyToPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.CanMoveDirectlyToPositionDelegate));
				return;
			case 19:
				ScriptingInterfaceOfIMBAgent.call_CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirectionDelegate = (ScriptingInterfaceOfIMBAgent.CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.CheckPathToAITargetAgentPassesThroughNavigationFaceIdFromDirectionDelegate));
				return;
			case 20:
				ScriptingInterfaceOfIMBAgent.call_ClearEquipmentDelegate = (ScriptingInterfaceOfIMBAgent.ClearEquipmentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ClearEquipmentDelegate));
				return;
			case 21:
				ScriptingInterfaceOfIMBAgent.call_ClearHandInverseKinematicsDelegate = (ScriptingInterfaceOfIMBAgent.ClearHandInverseKinematicsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ClearHandInverseKinematicsDelegate));
				return;
			case 22:
				ScriptingInterfaceOfIMBAgent.call_ClearTargetFrameDelegate = (ScriptingInterfaceOfIMBAgent.ClearTargetFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ClearTargetFrameDelegate));
				return;
			case 23:
				ScriptingInterfaceOfIMBAgent.call_ClearTargetZDelegate = (ScriptingInterfaceOfIMBAgent.ClearTargetZDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ClearTargetZDelegate));
				return;
			case 24:
				ScriptingInterfaceOfIMBAgent.call_ComputeAnimationDisplacementDelegate = (ScriptingInterfaceOfIMBAgent.ComputeAnimationDisplacementDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ComputeAnimationDisplacementDelegate));
				return;
			case 25:
				ScriptingInterfaceOfIMBAgent.call_CreateBloodBurstAtLimbDelegate = (ScriptingInterfaceOfIMBAgent.CreateBloodBurstAtLimbDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.CreateBloodBurstAtLimbDelegate));
				return;
			case 26:
				ScriptingInterfaceOfIMBAgent.call_DebugMoreDelegate = (ScriptingInterfaceOfIMBAgent.DebugMoreDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DebugMoreDelegate));
				return;
			case 27:
				break;
			case 28:
				ScriptingInterfaceOfIMBAgent.call_DefendDirectionToMovementFlagDelegate = (ScriptingInterfaceOfIMBAgent.DefendDirectionToMovementFlagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DefendDirectionToMovementFlagDelegate));
				return;
			case 29:
				ScriptingInterfaceOfIMBAgent.call_DeleteAttachedWeaponFromBoneDelegate = (ScriptingInterfaceOfIMBAgent.DeleteAttachedWeaponFromBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DeleteAttachedWeaponFromBoneDelegate));
				return;
			case 30:
				ScriptingInterfaceOfIMBAgent.call_DieDelegate = (ScriptingInterfaceOfIMBAgent.DieDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DieDelegate));
				return;
			case 31:
				ScriptingInterfaceOfIMBAgent.call_DisableLookToPointOfInterestDelegate = (ScriptingInterfaceOfIMBAgent.DisableLookToPointOfInterestDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DisableLookToPointOfInterestDelegate));
				return;
			case 32:
				ScriptingInterfaceOfIMBAgent.call_DisableScriptedCombatMovementDelegate = (ScriptingInterfaceOfIMBAgent.DisableScriptedCombatMovementDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DisableScriptedCombatMovementDelegate));
				return;
			case 33:
				ScriptingInterfaceOfIMBAgent.call_DisableScriptedMovementDelegate = (ScriptingInterfaceOfIMBAgent.DisableScriptedMovementDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DisableScriptedMovementDelegate));
				return;
			case 34:
				ScriptingInterfaceOfIMBAgent.call_DropItemDelegate = (ScriptingInterfaceOfIMBAgent.DropItemDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.DropItemDelegate));
				return;
			case 35:
				ScriptingInterfaceOfIMBAgent.call_EndRagdollAsCorpseDelegate = (ScriptingInterfaceOfIMBAgent.EndRagdollAsCorpseDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.EndRagdollAsCorpseDelegate));
				return;
			case 36:
				ScriptingInterfaceOfIMBAgent.call_EnforceShieldUsageDelegate = (ScriptingInterfaceOfIMBAgent.EnforceShieldUsageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.EnforceShieldUsageDelegate));
				return;
			case 37:
				ScriptingInterfaceOfIMBAgent.call_FadeInDelegate = (ScriptingInterfaceOfIMBAgent.FadeInDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.FadeInDelegate));
				return;
			case 38:
				ScriptingInterfaceOfIMBAgent.call_FadeOutDelegate = (ScriptingInterfaceOfIMBAgent.FadeOutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.FadeOutDelegate));
				return;
			case 39:
				ScriptingInterfaceOfIMBAgent.call_FindLongestDirectMoveToPositionDelegate = (ScriptingInterfaceOfIMBAgent.FindLongestDirectMoveToPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.FindLongestDirectMoveToPositionDelegate));
				return;
			case 40:
				ScriptingInterfaceOfIMBAgent.call_ForceAiBehaviorSelectionDelegate = (ScriptingInterfaceOfIMBAgent.ForceAiBehaviorSelectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ForceAiBehaviorSelectionDelegate));
				return;
			case 41:
				ScriptingInterfaceOfIMBAgent.call_GetActionChannelCurrentActionWeightDelegate = (ScriptingInterfaceOfIMBAgent.GetActionChannelCurrentActionWeightDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetActionChannelCurrentActionWeightDelegate));
				return;
			case 42:
				ScriptingInterfaceOfIMBAgent.call_GetActionChannelWeightDelegate = (ScriptingInterfaceOfIMBAgent.GetActionChannelWeightDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetActionChannelWeightDelegate));
				return;
			case 43:
				ScriptingInterfaceOfIMBAgent.call_GetActionDirectionDelegate = (ScriptingInterfaceOfIMBAgent.GetActionDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetActionDirectionDelegate));
				return;
			case 44:
				ScriptingInterfaceOfIMBAgent.call_GetActionSetNoDelegate = (ScriptingInterfaceOfIMBAgent.GetActionSetNoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetActionSetNoDelegate));
				return;
			case 45:
				ScriptingInterfaceOfIMBAgent.call_GetAgentFacialAnimationDelegate = (ScriptingInterfaceOfIMBAgent.GetAgentFacialAnimationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAgentFacialAnimationDelegate));
				return;
			case 46:
				ScriptingInterfaceOfIMBAgent.call_GetAgentParentEntityDelegate = (ScriptingInterfaceOfIMBAgent.GetAgentParentEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAgentParentEntityDelegate));
				return;
			case 47:
				ScriptingInterfaceOfIMBAgent.call_GetAgentScaleDelegate = (ScriptingInterfaceOfIMBAgent.GetAgentScaleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAgentScaleDelegate));
				return;
			case 48:
				ScriptingInterfaceOfIMBAgent.call_GetAgentVisualsDelegate = (ScriptingInterfaceOfIMBAgent.GetAgentVisualsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAgentVisualsDelegate));
				return;
			case 49:
				ScriptingInterfaceOfIMBAgent.call_GetAgentVoiceDefinitionDelegate = (ScriptingInterfaceOfIMBAgent.GetAgentVoiceDefinitionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAgentVoiceDefinitionDelegate));
				return;
			case 50:
				ScriptingInterfaceOfIMBAgent.call_GetAILastSuspiciousPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetAILastSuspiciousPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAILastSuspiciousPositionDelegate));
				return;
			case 51:
				ScriptingInterfaceOfIMBAgent.call_GetAimingTimerDelegate = (ScriptingInterfaceOfIMBAgent.GetAimingTimerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAimingTimerDelegate));
				return;
			case 52:
				ScriptingInterfaceOfIMBAgent.call_GetAIMoveDestinationDelegate = (ScriptingInterfaceOfIMBAgent.GetAIMoveDestinationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAIMoveDestinationDelegate));
				return;
			case 53:
				ScriptingInterfaceOfIMBAgent.call_GetAIMoveStopToleranceDelegate = (ScriptingInterfaceOfIMBAgent.GetAIMoveStopToleranceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAIMoveStopToleranceDelegate));
				return;
			case 54:
				ScriptingInterfaceOfIMBAgent.call_GetAIStateFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetAIStateFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAIStateFlagsDelegate));
				return;
			case 55:
				ScriptingInterfaceOfIMBAgent.call_GetAttackDirectionDelegate = (ScriptingInterfaceOfIMBAgent.GetAttackDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAttackDirectionDelegate));
				return;
			case 56:
				ScriptingInterfaceOfIMBAgent.call_GetAttackDirectionUsageDelegate = (ScriptingInterfaceOfIMBAgent.GetAttackDirectionUsageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAttackDirectionUsageDelegate));
				return;
			case 57:
				ScriptingInterfaceOfIMBAgent.call_GetAverageRealGlobalVelocityDelegate = (ScriptingInterfaceOfIMBAgent.GetAverageRealGlobalVelocityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAverageRealGlobalVelocityDelegate));
				return;
			case 58:
				ScriptingInterfaceOfIMBAgent.call_GetAverageVelocityDelegate = (ScriptingInterfaceOfIMBAgent.GetAverageVelocityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetAverageVelocityDelegate));
				return;
			case 59:
				ScriptingInterfaceOfIMBAgent.call_GetBodyRotationConstraintDelegate = (ScriptingInterfaceOfIMBAgent.GetBodyRotationConstraintDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetBodyRotationConstraintDelegate));
				return;
			case 60:
				ScriptingInterfaceOfIMBAgent.call_GetBoneEntitialFrameDelegate = (ScriptingInterfaceOfIMBAgent.GetBoneEntitialFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetBoneEntitialFrameDelegate));
				return;
			case 61:
				ScriptingInterfaceOfIMBAgent.call_GetBoneEntitialFrameAtAnimationProgressDelegate = (ScriptingInterfaceOfIMBAgent.GetBoneEntitialFrameAtAnimationProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetBoneEntitialFrameAtAnimationProgressDelegate));
				return;
			case 62:
				ScriptingInterfaceOfIMBAgent.call_GetChestGlobalPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetChestGlobalPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetChestGlobalPositionDelegate));
				return;
			case 63:
				ScriptingInterfaceOfIMBAgent.call_GetCollisionCapsuleDelegate = (ScriptingInterfaceOfIMBAgent.GetCollisionCapsuleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCollisionCapsuleDelegate));
				return;
			case 64:
				ScriptingInterfaceOfIMBAgent.call_GetCrouchModeDelegate = (ScriptingInterfaceOfIMBAgent.GetCrouchModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCrouchModeDelegate));
				return;
			case 65:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentActionDirectionDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentActionDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentActionDirectionDelegate));
				return;
			case 66:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentActionPriorityDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentActionPriorityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentActionPriorityDelegate));
				return;
			case 67:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentActionProgressDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentActionProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentActionProgressDelegate));
				return;
			case 68:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentActionStageDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentActionStageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentActionStageDelegate));
				return;
			case 69:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentActionTypeDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentActionTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentActionTypeDelegate));
				return;
			case 70:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentAimingErrorDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentAimingErrorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentAimingErrorDelegate));
				return;
			case 71:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentAimingTurbulanceDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentAimingTurbulanceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentAimingTurbulanceDelegate));
				return;
			case 72:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentAnimationFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentAnimationFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentAnimationFlagsDelegate));
				return;
			case 73:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentGuardModeDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentGuardModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentGuardModeDelegate));
				return;
			case 74:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentNavigationFaceIdDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentNavigationFaceIdDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentNavigationFaceIdDelegate));
				return;
			case 75:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentSpeedLimitDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentSpeedLimitDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentSpeedLimitDelegate));
				return;
			case 76:
				ScriptingInterfaceOfIMBAgent.call_GetCurrentVelocityDelegate = (ScriptingInterfaceOfIMBAgent.GetCurrentVelocityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurrentVelocityDelegate));
				return;
			case 77:
				ScriptingInterfaceOfIMBAgent.call_GetCurWeaponOffsetDelegate = (ScriptingInterfaceOfIMBAgent.GetCurWeaponOffsetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetCurWeaponOffsetDelegate));
				return;
			case 78:
				ScriptingInterfaceOfIMBAgent.call_GetDefendMovementFlagDelegate = (ScriptingInterfaceOfIMBAgent.GetDefendMovementFlagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetDefendMovementFlagDelegate));
				return;
			case 79:
				ScriptingInterfaceOfIMBAgent.call_GetEventControlFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetEventControlFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetEventControlFlagsDelegate));
				return;
			case 80:
				ScriptingInterfaceOfIMBAgent.call_GetEyeGlobalHeightDelegate = (ScriptingInterfaceOfIMBAgent.GetEyeGlobalHeightDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetEyeGlobalHeightDelegate));
				return;
			case 81:
				ScriptingInterfaceOfIMBAgent.call_GetEyeGlobalPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetEyeGlobalPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetEyeGlobalPositionDelegate));
				return;
			case 82:
				ScriptingInterfaceOfIMBAgent.call_GetFiringOrderDelegate = (ScriptingInterfaceOfIMBAgent.GetFiringOrderDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetFiringOrderDelegate));
				return;
			case 83:
				ScriptingInterfaceOfIMBAgent.call_GetGroundMaterialForCollisionEffectDelegate = (ScriptingInterfaceOfIMBAgent.GetGroundMaterialForCollisionEffectDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetGroundMaterialForCollisionEffectDelegate));
				return;
			case 84:
				ScriptingInterfaceOfIMBAgent.call_GetHasOnAiInputSetCallbackDelegate = (ScriptingInterfaceOfIMBAgent.GetHasOnAiInputSetCallbackDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetHasOnAiInputSetCallbackDelegate));
				return;
			case 85:
				ScriptingInterfaceOfIMBAgent.call_GetHeadCameraModeDelegate = (ScriptingInterfaceOfIMBAgent.GetHeadCameraModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetHeadCameraModeDelegate));
				return;
			case 86:
				ScriptingInterfaceOfIMBAgent.call_GetImmediateEnemyDelegate = (ScriptingInterfaceOfIMBAgent.GetImmediateEnemyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetImmediateEnemyDelegate));
				return;
			case 87:
				ScriptingInterfaceOfIMBAgent.call_GetIsDoingPassiveAttackDelegate = (ScriptingInterfaceOfIMBAgent.GetIsDoingPassiveAttackDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetIsDoingPassiveAttackDelegate));
				return;
			case 88:
				ScriptingInterfaceOfIMBAgent.call_GetIsLeftStanceDelegate = (ScriptingInterfaceOfIMBAgent.GetIsLeftStanceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetIsLeftStanceDelegate));
				return;
			case 89:
				ScriptingInterfaceOfIMBAgent.call_GetIsLookDirectionLockedDelegate = (ScriptingInterfaceOfIMBAgent.GetIsLookDirectionLockedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetIsLookDirectionLockedDelegate));
				return;
			case 90:
				ScriptingInterfaceOfIMBAgent.call_GetIsPassiveUsageConditionsAreMetDelegate = (ScriptingInterfaceOfIMBAgent.GetIsPassiveUsageConditionsAreMetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetIsPassiveUsageConditionsAreMetDelegate));
				return;
			case 91:
				ScriptingInterfaceOfIMBAgent.call_GetLastTargetVisibilityStateDelegate = (ScriptingInterfaceOfIMBAgent.GetLastTargetVisibilityStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetLastTargetVisibilityStateDelegate));
				return;
			case 92:
				ScriptingInterfaceOfIMBAgent.call_GetLookAgentDelegate = (ScriptingInterfaceOfIMBAgent.GetLookAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetLookAgentDelegate));
				return;
			case 93:
				ScriptingInterfaceOfIMBAgent.call_GetLookDirectionDelegate = (ScriptingInterfaceOfIMBAgent.GetLookDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetLookDirectionDelegate));
				return;
			case 94:
				ScriptingInterfaceOfIMBAgent.call_GetLookDirectionAsAngleDelegate = (ScriptingInterfaceOfIMBAgent.GetLookDirectionAsAngleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetLookDirectionAsAngleDelegate));
				return;
			case 95:
				ScriptingInterfaceOfIMBAgent.call_GetLookDownLimitDelegate = (ScriptingInterfaceOfIMBAgent.GetLookDownLimitDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetLookDownLimitDelegate));
				return;
			case 96:
				ScriptingInterfaceOfIMBAgent.call_GetMaximumNumberOfAgentsDelegate = (ScriptingInterfaceOfIMBAgent.GetMaximumNumberOfAgentsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMaximumNumberOfAgentsDelegate));
				return;
			case 97:
				ScriptingInterfaceOfIMBAgent.call_GetMaximumSpeedLimitDelegate = (ScriptingInterfaceOfIMBAgent.GetMaximumSpeedLimitDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMaximumSpeedLimitDelegate));
				return;
			case 98:
				ScriptingInterfaceOfIMBAgent.call_GetMissileRangeDelegate = (ScriptingInterfaceOfIMBAgent.GetMissileRangeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMissileRangeDelegate));
				return;
			case 99:
				ScriptingInterfaceOfIMBAgent.call_GetMissileRangeWithHeightDifferenceDelegate = (ScriptingInterfaceOfIMBAgent.GetMissileRangeWithHeightDifferenceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMissileRangeWithHeightDifferenceDelegate));
				return;
			case 100:
				ScriptingInterfaceOfIMBAgent.call_GetMonsterUsageIndexDelegate = (ScriptingInterfaceOfIMBAgent.GetMonsterUsageIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMonsterUsageIndexDelegate));
				return;
			case 101:
				ScriptingInterfaceOfIMBAgent.call_GetMountAgentDelegate = (ScriptingInterfaceOfIMBAgent.GetMountAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMountAgentDelegate));
				return;
			case 102:
				ScriptingInterfaceOfIMBAgent.call_GetMovementFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetMovementFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMovementFlagsDelegate));
				return;
			case 103:
				ScriptingInterfaceOfIMBAgent.call_GetMovementInputVectorDelegate = (ScriptingInterfaceOfIMBAgent.GetMovementInputVectorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMovementInputVectorDelegate));
				return;
			case 104:
				ScriptingInterfaceOfIMBAgent.call_GetMovementLockedStateDelegate = (ScriptingInterfaceOfIMBAgent.GetMovementLockedStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMovementLockedStateDelegate));
				return;
			case 105:
				ScriptingInterfaceOfIMBAgent.call_GetMovementVelocityDelegate = (ScriptingInterfaceOfIMBAgent.GetMovementVelocityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetMovementVelocityDelegate));
				return;
			case 106:
				ScriptingInterfaceOfIMBAgent.call_GetNativeActionIndexDelegate = (ScriptingInterfaceOfIMBAgent.GetNativeActionIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetNativeActionIndexDelegate));
				return;
			case 107:
				ScriptingInterfaceOfIMBAgent.call_GetOldWieldedItemInfoDelegate = (ScriptingInterfaceOfIMBAgent.GetOldWieldedItemInfoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetOldWieldedItemInfoDelegate));
				return;
			case 108:
				ScriptingInterfaceOfIMBAgent.call_GetPathDistanceToPointDelegate = (ScriptingInterfaceOfIMBAgent.GetPathDistanceToPointDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetPathDistanceToPointDelegate));
				return;
			case 109:
				ScriptingInterfaceOfIMBAgent.call_GetPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetPositionDelegate));
				return;
			case 110:
				ScriptingInterfaceOfIMBAgent.call_GetRealGlobalVelocityDelegate = (ScriptingInterfaceOfIMBAgent.GetRealGlobalVelocityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRealGlobalVelocityDelegate));
				return;
			case 111:
				ScriptingInterfaceOfIMBAgent.call_GetRenderCheckEnabledDelegate = (ScriptingInterfaceOfIMBAgent.GetRenderCheckEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRenderCheckEnabledDelegate));
				return;
			case 112:
				ScriptingInterfaceOfIMBAgent.call_GetRetreatPosDelegate = (ScriptingInterfaceOfIMBAgent.GetRetreatPosDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRetreatPosDelegate));
				return;
			case 113:
				ScriptingInterfaceOfIMBAgent.call_GetRiderAgentDelegate = (ScriptingInterfaceOfIMBAgent.GetRiderAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRiderAgentDelegate));
				return;
			case 114:
				ScriptingInterfaceOfIMBAgent.call_GetRidingOrderDelegate = (ScriptingInterfaceOfIMBAgent.GetRidingOrderDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRidingOrderDelegate));
				return;
			case 115:
				ScriptingInterfaceOfIMBAgent.call_GetRotationFrameDelegate = (ScriptingInterfaceOfIMBAgent.GetRotationFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRotationFrameDelegate));
				return;
			case 116:
				ScriptingInterfaceOfIMBAgent.call_GetRunningSimulationDataUntilMaximumSpeedReachedDelegate = (ScriptingInterfaceOfIMBAgent.GetRunningSimulationDataUntilMaximumSpeedReachedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetRunningSimulationDataUntilMaximumSpeedReachedDelegate));
				return;
			case 117:
				ScriptingInterfaceOfIMBAgent.call_GetScriptedCombatFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetScriptedCombatFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetScriptedCombatFlagsDelegate));
				return;
			case 118:
				ScriptingInterfaceOfIMBAgent.call_GetScriptedFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetScriptedFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetScriptedFlagsDelegate));
				return;
			case 119:
				ScriptingInterfaceOfIMBAgent.call_GetSelectedMountIndexDelegate = (ScriptingInterfaceOfIMBAgent.GetSelectedMountIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetSelectedMountIndexDelegate));
				return;
			case 120:
				ScriptingInterfaceOfIMBAgent.call_GetStateFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetStateFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetStateFlagsDelegate));
				return;
			case 121:
				ScriptingInterfaceOfIMBAgent.call_GetSteppedBodyFlagsDelegate = (ScriptingInterfaceOfIMBAgent.GetSteppedBodyFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetSteppedBodyFlagsDelegate));
				return;
			case 122:
				ScriptingInterfaceOfIMBAgent.call_GetSteppedEntityIdDelegate = (ScriptingInterfaceOfIMBAgent.GetSteppedEntityIdDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetSteppedEntityIdDelegate));
				return;
			case 123:
				ScriptingInterfaceOfIMBAgent.call_GetSteppedRootEntityDelegate = (ScriptingInterfaceOfIMBAgent.GetSteppedRootEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetSteppedRootEntityDelegate));
				return;
			case 124:
				ScriptingInterfaceOfIMBAgent.call_GetTargetAgentDelegate = (ScriptingInterfaceOfIMBAgent.GetTargetAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTargetAgentDelegate));
				return;
			case 125:
				ScriptingInterfaceOfIMBAgent.call_GetTargetDirectionDelegate = (ScriptingInterfaceOfIMBAgent.GetTargetDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTargetDirectionDelegate));
				return;
			case 126:
				ScriptingInterfaceOfIMBAgent.call_GetTargetFormationIndexDelegate = (ScriptingInterfaceOfIMBAgent.GetTargetFormationIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTargetFormationIndexDelegate));
				return;
			case 127:
				ScriptingInterfaceOfIMBAgent.call_GetTargetPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetTargetPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTargetPositionDelegate));
				return;
			case 128:
				ScriptingInterfaceOfIMBAgent.call_GetTeamDelegate = (ScriptingInterfaceOfIMBAgent.GetTeamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTeamDelegate));
				return;
			case 129:
				ScriptingInterfaceOfIMBAgent.call_GetTotalMassDelegate = (ScriptingInterfaceOfIMBAgent.GetTotalMassDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTotalMassDelegate));
				return;
			case 130:
				ScriptingInterfaceOfIMBAgent.call_GetTurnSpeedDelegate = (ScriptingInterfaceOfIMBAgent.GetTurnSpeedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetTurnSpeedDelegate));
				return;
			case 131:
				ScriptingInterfaceOfIMBAgent.call_GetVisualPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetVisualPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetVisualPositionDelegate));
				return;
			case 132:
				ScriptingInterfaceOfIMBAgent.call_GetWalkModeDelegate = (ScriptingInterfaceOfIMBAgent.GetWalkModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetWalkModeDelegate));
				return;
			case 133:
				ScriptingInterfaceOfIMBAgent.call_GetWalkSpeedLimitOfMountableDelegate = (ScriptingInterfaceOfIMBAgent.GetWalkSpeedLimitOfMountableDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetWalkSpeedLimitOfMountableDelegate));
				return;
			case 134:
				ScriptingInterfaceOfIMBAgent.call_GetWeaponEntityFromEquipmentSlotDelegate = (ScriptingInterfaceOfIMBAgent.GetWeaponEntityFromEquipmentSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetWeaponEntityFromEquipmentSlotDelegate));
				return;
			case 135:
				ScriptingInterfaceOfIMBAgent.call_GetWieldedWeaponInfoDelegate = (ScriptingInterfaceOfIMBAgent.GetWieldedWeaponInfoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetWieldedWeaponInfoDelegate));
				return;
			case 136:
				ScriptingInterfaceOfIMBAgent.call_GetWorldPositionDelegate = (ScriptingInterfaceOfIMBAgent.GetWorldPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.GetWorldPositionDelegate));
				return;
			case 137:
				ScriptingInterfaceOfIMBAgent.call_HandleBlowAuxDelegate = (ScriptingInterfaceOfIMBAgent.HandleBlowAuxDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.HandleBlowAuxDelegate));
				return;
			case 138:
				ScriptingInterfaceOfIMBAgent.call_HasPathThroughNavigationFaceIdFromDirectionDelegate = (ScriptingInterfaceOfIMBAgent.HasPathThroughNavigationFaceIdFromDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.HasPathThroughNavigationFaceIdFromDirectionDelegate));
				return;
			case 139:
				ScriptingInterfaceOfIMBAgent.call_HasPathThroughNavigationFacesIDFromDirectionDelegate = (ScriptingInterfaceOfIMBAgent.HasPathThroughNavigationFacesIDFromDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.HasPathThroughNavigationFacesIDFromDirectionDelegate));
				return;
			case 140:
				ScriptingInterfaceOfIMBAgent.call_InitializeAgentRecordDelegate = (ScriptingInterfaceOfIMBAgent.InitializeAgentRecordDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.InitializeAgentRecordDelegate));
				return;
			case 141:
				ScriptingInterfaceOfIMBAgent.call_InvalidateAIWeaponSelectionsDelegate = (ScriptingInterfaceOfIMBAgent.InvalidateAIWeaponSelectionsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.InvalidateAIWeaponSelectionsDelegate));
				return;
			case 142:
				ScriptingInterfaceOfIMBAgent.call_InvalidateTargetAgentDelegate = (ScriptingInterfaceOfIMBAgent.InvalidateTargetAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.InvalidateTargetAgentDelegate));
				return;
			case 143:
				ScriptingInterfaceOfIMBAgent.call_IsAddedAsCorpseDelegate = (ScriptingInterfaceOfIMBAgent.IsAddedAsCorpseDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsAddedAsCorpseDelegate));
				return;
			case 144:
				ScriptingInterfaceOfIMBAgent.call_IsCrouchingAllowedDelegate = (ScriptingInterfaceOfIMBAgent.IsCrouchingAllowedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsCrouchingAllowedDelegate));
				return;
			case 145:
				ScriptingInterfaceOfIMBAgent.call_IsEnemyDelegate = (ScriptingInterfaceOfIMBAgent.IsEnemyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsEnemyDelegate));
				return;
			case 146:
				ScriptingInterfaceOfIMBAgent.call_IsFadingOutDelegate = (ScriptingInterfaceOfIMBAgent.IsFadingOutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsFadingOutDelegate));
				return;
			case 147:
				ScriptingInterfaceOfIMBAgent.call_IsFriendDelegate = (ScriptingInterfaceOfIMBAgent.IsFriendDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsFriendDelegate));
				return;
			case 148:
				ScriptingInterfaceOfIMBAgent.call_IsLookRotationInSlowMotionDelegate = (ScriptingInterfaceOfIMBAgent.IsLookRotationInSlowMotionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsLookRotationInSlowMotionDelegate));
				return;
			case 149:
				ScriptingInterfaceOfIMBAgent.call_IsRetreatingDelegate = (ScriptingInterfaceOfIMBAgent.IsRetreatingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsRetreatingDelegate));
				return;
			case 150:
				ScriptingInterfaceOfIMBAgent.call_IsRunningAwayDelegate = (ScriptingInterfaceOfIMBAgent.IsRunningAwayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsRunningAwayDelegate));
				return;
			case 151:
				ScriptingInterfaceOfIMBAgent.call_IsSlidingDelegate = (ScriptingInterfaceOfIMBAgent.IsSlidingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsSlidingDelegate));
				return;
			case 152:
				ScriptingInterfaceOfIMBAgent.call_IsTargetNavigationFaceIdBetweenDelegate = (ScriptingInterfaceOfIMBAgent.IsTargetNavigationFaceIdBetweenDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsTargetNavigationFaceIdBetweenDelegate));
				return;
			case 153:
				ScriptingInterfaceOfIMBAgent.call_IsWanderingDelegate = (ScriptingInterfaceOfIMBAgent.IsWanderingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.IsWanderingDelegate));
				return;
			case 154:
				ScriptingInterfaceOfIMBAgent.call_KickClearDelegate = (ScriptingInterfaceOfIMBAgent.KickClearDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.KickClearDelegate));
				return;
			case 155:
				ScriptingInterfaceOfIMBAgent.call_LockAgentReplicationTableDataWithCurrentReliableSequenceNoDelegate = (ScriptingInterfaceOfIMBAgent.LockAgentReplicationTableDataWithCurrentReliableSequenceNoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.LockAgentReplicationTableDataWithCurrentReliableSequenceNoDelegate));
				return;
			case 156:
				ScriptingInterfaceOfIMBAgent.call_MakeDeadDelegate = (ScriptingInterfaceOfIMBAgent.MakeDeadDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.MakeDeadDelegate));
				return;
			case 157:
				ScriptingInterfaceOfIMBAgent.call_MakeVoiceDelegate = (ScriptingInterfaceOfIMBAgent.MakeVoiceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.MakeVoiceDelegate));
				return;
			case 158:
				ScriptingInterfaceOfIMBAgent.call_PlayerAttackDirectionDelegate = (ScriptingInterfaceOfIMBAgent.PlayerAttackDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.PlayerAttackDirectionDelegate));
				return;
			case 159:
				ScriptingInterfaceOfIMBAgent.call_PreloadForRenderingDelegate = (ScriptingInterfaceOfIMBAgent.PreloadForRenderingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.PreloadForRenderingDelegate));
				return;
			case 160:
				ScriptingInterfaceOfIMBAgent.call_PrepareWeaponForDropInEquipmentSlotDelegate = (ScriptingInterfaceOfIMBAgent.PrepareWeaponForDropInEquipmentSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.PrepareWeaponForDropInEquipmentSlotDelegate));
				return;
			case 161:
				ScriptingInterfaceOfIMBAgent.call_RemoveMeshFromBoneDelegate = (ScriptingInterfaceOfIMBAgent.RemoveMeshFromBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.RemoveMeshFromBoneDelegate));
				return;
			case 162:
				ScriptingInterfaceOfIMBAgent.call_ResetEnemyCachesDelegate = (ScriptingInterfaceOfIMBAgent.ResetEnemyCachesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ResetEnemyCachesDelegate));
				return;
			case 163:
				ScriptingInterfaceOfIMBAgent.call_ResetGuardDelegate = (ScriptingInterfaceOfIMBAgent.ResetGuardDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.ResetGuardDelegate));
				return;
			case 164:
				ScriptingInterfaceOfIMBAgent.call_SetActionChannelDelegate = (ScriptingInterfaceOfIMBAgent.SetActionChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetActionChannelDelegate));
				return;
			case 165:
				ScriptingInterfaceOfIMBAgent.call_SetActionSetDelegate = (ScriptingInterfaceOfIMBAgent.SetActionSetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetActionSetDelegate));
				return;
			case 166:
				ScriptingInterfaceOfIMBAgent.call_SetAgentExcludeStateForFaceGroupIdDelegate = (ScriptingInterfaceOfIMBAgent.SetAgentExcludeStateForFaceGroupIdDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAgentExcludeStateForFaceGroupIdDelegate));
				return;
			case 167:
				ScriptingInterfaceOfIMBAgent.call_SetAgentFacialAnimationDelegate = (ScriptingInterfaceOfIMBAgent.SetAgentFacialAnimationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAgentFacialAnimationDelegate));
				return;
			case 168:
				ScriptingInterfaceOfIMBAgent.call_SetAgentFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetAgentFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAgentFlagsDelegate));
				return;
			case 169:
				ScriptingInterfaceOfIMBAgent.call_SetAgentIdleAnimationStatusDelegate = (ScriptingInterfaceOfIMBAgent.SetAgentIdleAnimationStatusDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAgentIdleAnimationStatusDelegate));
				return;
			case 170:
				ScriptingInterfaceOfIMBAgent.call_SetAgentScaleDelegate = (ScriptingInterfaceOfIMBAgent.SetAgentScaleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAgentScaleDelegate));
				return;
			case 171:
				ScriptingInterfaceOfIMBAgent.call_SetAIAlarmStateDelegate = (ScriptingInterfaceOfIMBAgent.SetAIAlarmStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAIAlarmStateDelegate));
				return;
			case 172:
				ScriptingInterfaceOfIMBAgent.call_SetAIBehaviorParamsDelegate = (ScriptingInterfaceOfIMBAgent.SetAIBehaviorParamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAIBehaviorParamsDelegate));
				return;
			case 173:
				ScriptingInterfaceOfIMBAgent.call_SetAILastSuspiciousPositionDelegate = (ScriptingInterfaceOfIMBAgent.SetAILastSuspiciousPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAILastSuspiciousPositionDelegate));
				return;
			case 174:
				ScriptingInterfaceOfIMBAgent.call_SetAIStateFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetAIStateFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAIStateFlagsDelegate));
				return;
			case 175:
				ScriptingInterfaceOfIMBAgent.call_SetAllAIBehaviorParamsDelegate = (ScriptingInterfaceOfIMBAgent.SetAllAIBehaviorParamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAllAIBehaviorParamsDelegate));
				return;
			case 176:
				ScriptingInterfaceOfIMBAgent.call_SetAttackStateDelegate = (ScriptingInterfaceOfIMBAgent.SetAttackStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAttackStateDelegate));
				return;
			case 177:
				ScriptingInterfaceOfIMBAgent.call_SetAutomaticTargetSelectionDelegate = (ScriptingInterfaceOfIMBAgent.SetAutomaticTargetSelectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAutomaticTargetSelectionDelegate));
				return;
			case 178:
				ScriptingInterfaceOfIMBAgent.call_SetAveragePingInMillisecondsDelegate = (ScriptingInterfaceOfIMBAgent.SetAveragePingInMillisecondsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetAveragePingInMillisecondsDelegate));
				return;
			case 179:
				ScriptingInterfaceOfIMBAgent.call_SetBodyArmorMaterialTypeDelegate = (ScriptingInterfaceOfIMBAgent.SetBodyArmorMaterialTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetBodyArmorMaterialTypeDelegate));
				return;
			case 180:
				ScriptingInterfaceOfIMBAgent.call_SetColumnwiseFollowAgentDelegate = (ScriptingInterfaceOfIMBAgent.SetColumnwiseFollowAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetColumnwiseFollowAgentDelegate));
				return;
			case 181:
				ScriptingInterfaceOfIMBAgent.call_SetControllerDelegate = (ScriptingInterfaceOfIMBAgent.SetControllerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetControllerDelegate));
				return;
			case 182:
				ScriptingInterfaceOfIMBAgent.call_SetCourageDelegate = (ScriptingInterfaceOfIMBAgent.SetCourageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetCourageDelegate));
				return;
			case 183:
				ScriptingInterfaceOfIMBAgent.call_SetCurrentActionProgressDelegate = (ScriptingInterfaceOfIMBAgent.SetCurrentActionProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetCurrentActionProgressDelegate));
				return;
			case 184:
				ScriptingInterfaceOfIMBAgent.call_SetCurrentActionSpeedDelegate = (ScriptingInterfaceOfIMBAgent.SetCurrentActionSpeedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetCurrentActionSpeedDelegate));
				return;
			case 185:
				ScriptingInterfaceOfIMBAgent.call_SetDirectionChangeTendencyDelegate = (ScriptingInterfaceOfIMBAgent.SetDirectionChangeTendencyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetDirectionChangeTendencyDelegate));
				return;
			case 186:
				ScriptingInterfaceOfIMBAgent.call_SetEventControlFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetEventControlFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetEventControlFlagsDelegate));
				return;
			case 187:
				ScriptingInterfaceOfIMBAgent.call_SetExcludedFromGravityDelegate = (ScriptingInterfaceOfIMBAgent.SetExcludedFromGravityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetExcludedFromGravityDelegate));
				return;
			case 188:
				ScriptingInterfaceOfIMBAgent.call_SetFiringOrderDelegate = (ScriptingInterfaceOfIMBAgent.SetFiringOrderDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFiringOrderDelegate));
				return;
			case 189:
				ScriptingInterfaceOfIMBAgent.call_SetForceAttachedEntityDelegate = (ScriptingInterfaceOfIMBAgent.SetForceAttachedEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetForceAttachedEntityDelegate));
				return;
			case 190:
				ScriptingInterfaceOfIMBAgent.call_SetFormationFrameDisabledDelegate = (ScriptingInterfaceOfIMBAgent.SetFormationFrameDisabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFormationFrameDisabledDelegate));
				return;
			case 191:
				ScriptingInterfaceOfIMBAgent.call_SetFormationFrameEnabledDelegate = (ScriptingInterfaceOfIMBAgent.SetFormationFrameEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFormationFrameEnabledDelegate));
				return;
			case 192:
				ScriptingInterfaceOfIMBAgent.call_SetFormationInfoDelegate = (ScriptingInterfaceOfIMBAgent.SetFormationInfoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFormationInfoDelegate));
				return;
			case 193:
				ScriptingInterfaceOfIMBAgent.call_SetFormationIntegrityDataDelegate = (ScriptingInterfaceOfIMBAgent.SetFormationIntegrityDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFormationIntegrityDataDelegate));
				return;
			case 194:
				ScriptingInterfaceOfIMBAgent.call_SetFormationNoDelegate = (ScriptingInterfaceOfIMBAgent.SetFormationNoDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetFormationNoDelegate));
				return;
			case 195:
				ScriptingInterfaceOfIMBAgent.call_SetHandInverseKinematicsFrameDelegate = (ScriptingInterfaceOfIMBAgent.SetHandInverseKinematicsFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetHandInverseKinematicsFrameDelegate));
				return;
			case 196:
				ScriptingInterfaceOfIMBAgent.call_SetHandInverseKinematicsFrameForMissionObjectUsageDelegate = (ScriptingInterfaceOfIMBAgent.SetHandInverseKinematicsFrameForMissionObjectUsageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetHandInverseKinematicsFrameForMissionObjectUsageDelegate));
				return;
			case 197:
				ScriptingInterfaceOfIMBAgent.call_SetHasOnAiInputSetCallbackDelegate = (ScriptingInterfaceOfIMBAgent.SetHasOnAiInputSetCallbackDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetHasOnAiInputSetCallbackDelegate));
				return;
			case 198:
				ScriptingInterfaceOfIMBAgent.call_SetHeadCameraModeDelegate = (ScriptingInterfaceOfIMBAgent.SetHeadCameraModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetHeadCameraModeDelegate));
				return;
			case 199:
				ScriptingInterfaceOfIMBAgent.call_SetInitialFrameDelegate = (ScriptingInterfaceOfIMBAgent.SetInitialFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetInitialFrameDelegate));
				return;
			case 200:
				ScriptingInterfaceOfIMBAgent.call_SetInteractionAgentDelegate = (ScriptingInterfaceOfIMBAgent.SetInteractionAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetInteractionAgentDelegate));
				return;
			case 201:
				ScriptingInterfaceOfIMBAgent.call_SetIsLookDirectionLockedDelegate = (ScriptingInterfaceOfIMBAgent.SetIsLookDirectionLockedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetIsLookDirectionLockedDelegate));
				return;
			case 202:
				ScriptingInterfaceOfIMBAgent.call_SetIsPhysicsForceClosedDelegate = (ScriptingInterfaceOfIMBAgent.SetIsPhysicsForceClosedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetIsPhysicsForceClosedDelegate));
				return;
			case 203:
				ScriptingInterfaceOfIMBAgent.call_SetLookAgentDelegate = (ScriptingInterfaceOfIMBAgent.SetLookAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetLookAgentDelegate));
				return;
			case 204:
				ScriptingInterfaceOfIMBAgent.call_SetLookDirectionDelegate = (ScriptingInterfaceOfIMBAgent.SetLookDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetLookDirectionDelegate));
				return;
			case 205:
				ScriptingInterfaceOfIMBAgent.call_SetLookDirectionAsAngleDelegate = (ScriptingInterfaceOfIMBAgent.SetLookDirectionAsAngleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetLookDirectionAsAngleDelegate));
				return;
			case 206:
				ScriptingInterfaceOfIMBAgent.call_SetLookToPointOfInterestDelegate = (ScriptingInterfaceOfIMBAgent.SetLookToPointOfInterestDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetLookToPointOfInterestDelegate));
				return;
			case 207:
				ScriptingInterfaceOfIMBAgent.call_SetMaximumSpeedLimitDelegate = (ScriptingInterfaceOfIMBAgent.SetMaximumSpeedLimitDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMaximumSpeedLimitDelegate));
				return;
			case 208:
				ScriptingInterfaceOfIMBAgent.call_SetMonoObjectDelegate = (ScriptingInterfaceOfIMBAgent.SetMonoObjectDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMonoObjectDelegate));
				return;
			case 209:
				ScriptingInterfaceOfIMBAgent.call_SetMountAgentDelegate = (ScriptingInterfaceOfIMBAgent.SetMountAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMountAgentDelegate));
				return;
			case 210:
				ScriptingInterfaceOfIMBAgent.call_SetMovementDirectionDelegate = (ScriptingInterfaceOfIMBAgent.SetMovementDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMovementDirectionDelegate));
				return;
			case 211:
				ScriptingInterfaceOfIMBAgent.call_SetMovementFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetMovementFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMovementFlagsDelegate));
				return;
			case 212:
				ScriptingInterfaceOfIMBAgent.call_SetMovementInputVectorDelegate = (ScriptingInterfaceOfIMBAgent.SetMovementInputVectorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetMovementInputVectorDelegate));
				return;
			case 213:
				ScriptingInterfaceOfIMBAgent.call_SetNetworkPeerDelegate = (ScriptingInterfaceOfIMBAgent.SetNetworkPeerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetNetworkPeerDelegate));
				return;
			case 214:
				ScriptingInterfaceOfIMBAgent.call_SetOverridenStrikeAndDeathActionDelegate = (ScriptingInterfaceOfIMBAgent.SetOverridenStrikeAndDeathActionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetOverridenStrikeAndDeathActionDelegate));
				return;
			case 215:
				ScriptingInterfaceOfIMBAgent.call_SetPositionDelegate = (ScriptingInterfaceOfIMBAgent.SetPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetPositionDelegate));
				return;
			case 216:
				ScriptingInterfaceOfIMBAgent.call_SetReloadAmmoInSlotDelegate = (ScriptingInterfaceOfIMBAgent.SetReloadAmmoInSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetReloadAmmoInSlotDelegate));
				return;
			case 217:
				ScriptingInterfaceOfIMBAgent.call_SetRenderCheckEnabledDelegate = (ScriptingInterfaceOfIMBAgent.SetRenderCheckEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetRenderCheckEnabledDelegate));
				return;
			case 218:
				ScriptingInterfaceOfIMBAgent.call_SetRetreatModeDelegate = (ScriptingInterfaceOfIMBAgent.SetRetreatModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetRetreatModeDelegate));
				return;
			case 219:
				ScriptingInterfaceOfIMBAgent.call_SetRidingOrderDelegate = (ScriptingInterfaceOfIMBAgent.SetRidingOrderDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetRidingOrderDelegate));
				return;
			case 220:
				ScriptingInterfaceOfIMBAgent.call_SetScriptedCombatFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetScriptedCombatFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetScriptedCombatFlagsDelegate));
				return;
			case 221:
				ScriptingInterfaceOfIMBAgent.call_SetScriptedFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetScriptedFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetScriptedFlagsDelegate));
				return;
			case 222:
				ScriptingInterfaceOfIMBAgent.call_SetScriptedPositionDelegate = (ScriptingInterfaceOfIMBAgent.SetScriptedPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetScriptedPositionDelegate));
				return;
			case 223:
				ScriptingInterfaceOfIMBAgent.call_SetScriptedPositionAndDirectionDelegate = (ScriptingInterfaceOfIMBAgent.SetScriptedPositionAndDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetScriptedPositionAndDirectionDelegate));
				return;
			case 224:
				ScriptingInterfaceOfIMBAgent.call_SetScriptedTargetEntityDelegate = (ScriptingInterfaceOfIMBAgent.SetScriptedTargetEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetScriptedTargetEntityDelegate));
				return;
			case 225:
				ScriptingInterfaceOfIMBAgent.call_SetSelectedMountIndexDelegate = (ScriptingInterfaceOfIMBAgent.SetSelectedMountIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetSelectedMountIndexDelegate));
				return;
			case 226:
				ScriptingInterfaceOfIMBAgent.call_SetShouldCatchUpWithFormationDelegate = (ScriptingInterfaceOfIMBAgent.SetShouldCatchUpWithFormationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetShouldCatchUpWithFormationDelegate));
				return;
			case 227:
				ScriptingInterfaceOfIMBAgent.call_SetStateFlagsDelegate = (ScriptingInterfaceOfIMBAgent.SetStateFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetStateFlagsDelegate));
				return;
			case 228:
				ScriptingInterfaceOfIMBAgent.call_SetTargetAgentDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetAgentDelegate));
				return;
			case 229:
				ScriptingInterfaceOfIMBAgent.call_SetTargetFormationIndexDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetFormationIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetFormationIndexDelegate));
				return;
			case 230:
				ScriptingInterfaceOfIMBAgent.call_SetTargetPositionDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetPositionDelegate));
				return;
			case 231:
				ScriptingInterfaceOfIMBAgent.call_SetTargetPositionAndDirectionDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetPositionAndDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetPositionAndDirectionDelegate));
				return;
			case 232:
				ScriptingInterfaceOfIMBAgent.call_SetTargetUpDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetUpDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetUpDelegate));
				return;
			case 233:
				ScriptingInterfaceOfIMBAgent.call_SetTargetZDelegate = (ScriptingInterfaceOfIMBAgent.SetTargetZDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTargetZDelegate));
				return;
			case 234:
				ScriptingInterfaceOfIMBAgent.call_SetTeamDelegate = (ScriptingInterfaceOfIMBAgent.SetTeamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetTeamDelegate));
				return;
			case 235:
				ScriptingInterfaceOfIMBAgent.call_SetUsageIndexOfWeaponInSlotAsClientDelegate = (ScriptingInterfaceOfIMBAgent.SetUsageIndexOfWeaponInSlotAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetUsageIndexOfWeaponInSlotAsClientDelegate));
				return;
			case 236:
				ScriptingInterfaceOfIMBAgent.call_SetVelocityLimitsOnRagdollDelegate = (ScriptingInterfaceOfIMBAgent.SetVelocityLimitsOnRagdollDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetVelocityLimitsOnRagdollDelegate));
				return;
			case 237:
				ScriptingInterfaceOfIMBAgent.call_SetWeaponAmmoAsClientDelegate = (ScriptingInterfaceOfIMBAgent.SetWeaponAmmoAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWeaponAmmoAsClientDelegate));
				return;
			case 238:
				ScriptingInterfaceOfIMBAgent.call_SetWeaponAmountInSlotDelegate = (ScriptingInterfaceOfIMBAgent.SetWeaponAmountInSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWeaponAmountInSlotDelegate));
				return;
			case 239:
				ScriptingInterfaceOfIMBAgent.call_SetWeaponGuardDelegate = (ScriptingInterfaceOfIMBAgent.SetWeaponGuardDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWeaponGuardDelegate));
				return;
			case 240:
				ScriptingInterfaceOfIMBAgent.call_SetWeaponHitPointsInSlotDelegate = (ScriptingInterfaceOfIMBAgent.SetWeaponHitPointsInSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWeaponHitPointsInSlotDelegate));
				return;
			case 241:
				ScriptingInterfaceOfIMBAgent.call_SetWeaponReloadPhaseAsClientDelegate = (ScriptingInterfaceOfIMBAgent.SetWeaponReloadPhaseAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWeaponReloadPhaseAsClientDelegate));
				return;
			case 242:
				ScriptingInterfaceOfIMBAgent.call_SetWieldedItemIndexAsClientDelegate = (ScriptingInterfaceOfIMBAgent.SetWieldedItemIndexAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.SetWieldedItemIndexAsClientDelegate));
				return;
			case 243:
				ScriptingInterfaceOfIMBAgent.call_StartFadingOutDelegate = (ScriptingInterfaceOfIMBAgent.StartFadingOutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.StartFadingOutDelegate));
				return;
			case 244:
				ScriptingInterfaceOfIMBAgent.call_StartRagdollAsCorpseDelegate = (ScriptingInterfaceOfIMBAgent.StartRagdollAsCorpseDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.StartRagdollAsCorpseDelegate));
				return;
			case 245:
				ScriptingInterfaceOfIMBAgent.call_StartSwitchingWeaponUsageIndexAsClientDelegate = (ScriptingInterfaceOfIMBAgent.StartSwitchingWeaponUsageIndexAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.StartSwitchingWeaponUsageIndexAsClientDelegate));
				return;
			case 246:
				ScriptingInterfaceOfIMBAgent.call_TickActionChannelsDelegate = (ScriptingInterfaceOfIMBAgent.TickActionChannelsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.TickActionChannelsDelegate));
				return;
			case 247:
				ScriptingInterfaceOfIMBAgent.call_TryGetImmediateEnemyAgentMovementDataDelegate = (ScriptingInterfaceOfIMBAgent.TryGetImmediateEnemyAgentMovementDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.TryGetImmediateEnemyAgentMovementDataDelegate));
				return;
			case 248:
				ScriptingInterfaceOfIMBAgent.call_TryToSheathWeaponInHandDelegate = (ScriptingInterfaceOfIMBAgent.TryToSheathWeaponInHandDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.TryToSheathWeaponInHandDelegate));
				return;
			case 249:
				ScriptingInterfaceOfIMBAgent.call_TryToWieldWeaponInSlotDelegate = (ScriptingInterfaceOfIMBAgent.TryToWieldWeaponInSlotDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.TryToWieldWeaponInSlotDelegate));
				return;
			case 250:
				ScriptingInterfaceOfIMBAgent.call_UpdateDrivenPropertiesDelegate = (ScriptingInterfaceOfIMBAgent.UpdateDrivenPropertiesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.UpdateDrivenPropertiesDelegate));
				return;
			case 251:
				ScriptingInterfaceOfIMBAgent.call_UpdateWeaponsDelegate = (ScriptingInterfaceOfIMBAgent.UpdateWeaponsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.UpdateWeaponsDelegate));
				return;
			case 252:
				ScriptingInterfaceOfIMBAgent.call_WeaponEquippedDelegate = (ScriptingInterfaceOfIMBAgent.WeaponEquippedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.WeaponEquippedDelegate));
				return;
			case 253:
				ScriptingInterfaceOfIMBAgent.call_WieldNextWeaponDelegate = (ScriptingInterfaceOfIMBAgent.WieldNextWeaponDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.WieldNextWeaponDelegate));
				return;
			case 254:
				ScriptingInterfaceOfIMBAgent.call_YellAfterDelayDelegate = (ScriptingInterfaceOfIMBAgent.YellAfterDelayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgent.YellAfterDelayDelegate));
				return;
			case 255:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddChildEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddChildEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddChildEntityDelegate));
				return;
			case 256:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddHorseReinsClothMeshDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddHorseReinsClothMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddHorseReinsClothMeshDelegate));
				return;
			case 257:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddMeshDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddMeshDelegate));
				return;
			case 258:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddMultiMeshDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddMultiMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddMultiMeshDelegate));
				return;
			case 259:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddPrefabToAgentVisualBoneByBoneTypeDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddPrefabToAgentVisualBoneByBoneTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddPrefabToAgentVisualBoneByBoneTypeDelegate));
				return;
			case 260:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddPrefabToAgentVisualBoneByRealBoneIndexDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndexDelegate));
				return;
			case 261:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddSkinMeshesToAgentEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddSkinMeshesToAgentEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddSkinMeshesToAgentEntityDelegate));
				return;
			case 262:
				ScriptingInterfaceOfIMBAgentVisuals.call_AddWeaponToAgentEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.AddWeaponToAgentEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.AddWeaponToAgentEntityDelegate));
				return;
			case 263:
				ScriptingInterfaceOfIMBAgentVisuals.call_ApplySkeletonScaleDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ApplySkeletonScaleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ApplySkeletonScaleDelegate));
				return;
			case 264:
				ScriptingInterfaceOfIMBAgentVisuals.call_BatchLastLodMeshesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.BatchLastLodMeshesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.BatchLastLodMeshesDelegate));
				return;
			case 265:
				ScriptingInterfaceOfIMBAgentVisuals.call_CheckResourcesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.CheckResourcesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.CheckResourcesDelegate));
				return;
			case 266:
				ScriptingInterfaceOfIMBAgentVisuals.call_ClearAllWeaponMeshesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ClearAllWeaponMeshesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ClearAllWeaponMeshesDelegate));
				return;
			case 267:
				ScriptingInterfaceOfIMBAgentVisuals.call_ClearVisualComponentsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ClearVisualComponentsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ClearVisualComponentsDelegate));
				return;
			case 268:
				ScriptingInterfaceOfIMBAgentVisuals.call_ClearWeaponMeshesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ClearWeaponMeshesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ClearWeaponMeshesDelegate));
				return;
			case 269:
				ScriptingInterfaceOfIMBAgentVisuals.call_CreateAgentRendererSceneControllerDelegate = (ScriptingInterfaceOfIMBAgentVisuals.CreateAgentRendererSceneControllerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.CreateAgentRendererSceneControllerDelegate));
				return;
			case 270:
				ScriptingInterfaceOfIMBAgentVisuals.call_CreateAgentVisualsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.CreateAgentVisualsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.CreateAgentVisualsDelegate));
				return;
			case 271:
				ScriptingInterfaceOfIMBAgentVisuals.call_CreateParticleSystemAttachedToBoneDelegate = (ScriptingInterfaceOfIMBAgentVisuals.CreateParticleSystemAttachedToBoneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.CreateParticleSystemAttachedToBoneDelegate));
				return;
			case 272:
				ScriptingInterfaceOfIMBAgentVisuals.call_DestructAgentRendererSceneControllerDelegate = (ScriptingInterfaceOfIMBAgentVisuals.DestructAgentRendererSceneControllerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.DestructAgentRendererSceneControllerDelegate));
				return;
			case 273:
				ScriptingInterfaceOfIMBAgentVisuals.call_DisableContourDelegate = (ScriptingInterfaceOfIMBAgentVisuals.DisableContourDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.DisableContourDelegate));
				return;
			case 274:
				ScriptingInterfaceOfIMBAgentVisuals.call_FillEntityWithBodyMeshesWithoutAgentVisualsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.FillEntityWithBodyMeshesWithoutAgentVisualsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.FillEntityWithBodyMeshesWithoutAgentVisualsDelegate));
				return;
			case 275:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetAttachedWeaponEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetAttachedWeaponEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetAttachedWeaponEntityDelegate));
				return;
			case 276:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetBoneEntitialFrameDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetBoneEntitialFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetBoneEntitialFrameDelegate));
				return;
			case 277:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetBoneEntitialFrameAtAnimationProgressDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetBoneEntitialFrameAtAnimationProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetBoneEntitialFrameAtAnimationProgressDelegate));
				return;
			case 278:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetBoneTypeDataDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetBoneTypeDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetBoneTypeDataDelegate));
				return;
			case 279:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetCurrentHeadLookDirectionDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetCurrentHeadLookDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetCurrentHeadLookDirectionDelegate));
				return;
			case 280:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetCurrentHelmetScalingFactorDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetCurrentHelmetScalingFactorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetCurrentHelmetScalingFactorDelegate));
				return;
			case 281:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetCurrentRagdollStateDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetCurrentRagdollStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetCurrentRagdollStateDelegate));
				return;
			case 282:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetEntityDelegate));
				return;
			case 283:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetEntityPointerDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetEntityPointerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetEntityPointerDelegate));
				return;
			case 284:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetFrameDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetFrameDelegate));
				return;
			case 285:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetGlobalFrameDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetGlobalFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetGlobalFrameDelegate));
				return;
			case 286:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetGlobalStableEyePointDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetGlobalStableEyePointDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetGlobalStableEyePointDelegate));
				return;
			case 287:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetGlobalStableNeckPointDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetGlobalStableNeckPointDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetGlobalStableNeckPointDelegate));
				return;
			case 288:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetMovementModeDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetMovementModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetMovementModeDelegate));
				return;
			case 289:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetRealBoneIndexDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetRealBoneIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetRealBoneIndexDelegate));
				return;
			case 290:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetSkeletonDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetSkeletonDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetSkeletonDelegate));
				return;
			case 291:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetVisibleDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetVisibleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetVisibleDelegate));
				return;
			case 292:
				ScriptingInterfaceOfIMBAgentVisuals.call_GetVisualStrengthOfAgentVisualDelegate = (ScriptingInterfaceOfIMBAgentVisuals.GetVisualStrengthOfAgentVisualDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.GetVisualStrengthOfAgentVisualDelegate));
				return;
			case 293:
				ScriptingInterfaceOfIMBAgentVisuals.call_IsValidDelegate = (ScriptingInterfaceOfIMBAgentVisuals.IsValidDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.IsValidDelegate));
				return;
			case 294:
				ScriptingInterfaceOfIMBAgentVisuals.call_LazyUpdateAgentRendererDataDelegate = (ScriptingInterfaceOfIMBAgentVisuals.LazyUpdateAgentRendererDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.LazyUpdateAgentRendererDataDelegate));
				return;
			case 295:
				ScriptingInterfaceOfIMBAgentVisuals.call_MakeVoiceDelegate = (ScriptingInterfaceOfIMBAgentVisuals.MakeVoiceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.MakeVoiceDelegate));
				return;
			case 296:
				ScriptingInterfaceOfIMBAgentVisuals.call_RemoveChildEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.RemoveChildEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.RemoveChildEntityDelegate));
				return;
			case 297:
				ScriptingInterfaceOfIMBAgentVisuals.call_RemoveMeshDelegate = (ScriptingInterfaceOfIMBAgentVisuals.RemoveMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.RemoveMeshDelegate));
				return;
			case 298:
				ScriptingInterfaceOfIMBAgentVisuals.call_RemoveMultiMeshDelegate = (ScriptingInterfaceOfIMBAgentVisuals.RemoveMultiMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.RemoveMultiMeshDelegate));
				return;
			case 299:
				ScriptingInterfaceOfIMBAgentVisuals.call_ResetDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ResetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ResetDelegate));
				return;
			case 300:
				ScriptingInterfaceOfIMBAgentVisuals.call_ResetNextFrameDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ResetNextFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ResetNextFrameDelegate));
				return;
			case 301:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetAgentLocalSpeedDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetAgentLocalSpeedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetAgentLocalSpeedDelegate));
				return;
			case 302:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetAgentLodMakeZeroOrMaxDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetAgentLodMakeZeroOrMaxDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetAgentLodMakeZeroOrMaxDelegate));
				return;
			case 303:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetAsContourEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetAsContourEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetAsContourEntityDelegate));
				return;
			case 304:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetAttachedPositionForRopeEntityAfterAnimationPostIntegrateDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetAttachedPositionForRopeEntityAfterAnimationPostIntegrateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetAttachedPositionForRopeEntityAfterAnimationPostIntegrateDelegate));
				return;
			case 305:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetClothComponentKeepStateOfAllMeshesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetClothComponentKeepStateOfAllMeshesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetClothComponentKeepStateOfAllMeshesDelegate));
				return;
			case 306:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetClothWindToWeaponAtIndexDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetClothWindToWeaponAtIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetClothWindToWeaponAtIndexDelegate));
				return;
			case 307:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetContourStateDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetContourStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetContourStateDelegate));
				return;
			case 308:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetDoTimerBasedForcedSkeletonUpdatesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetDoTimerBasedForcedSkeletonUpdatesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetDoTimerBasedForcedSkeletonUpdatesDelegate));
				return;
			case 309:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetEnableOcclusionCullingDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetEnableOcclusionCullingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetEnableOcclusionCullingDelegate));
				return;
			case 310:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetEnforcedVisibilityForAllAgentsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetEnforcedVisibilityForAllAgentsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetEnforcedVisibilityForAllAgentsDelegate));
				return;
			case 311:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetEntityDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetEntityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetEntityDelegate));
				return;
			case 312:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetFaceGenerationParamsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetFaceGenerationParamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetFaceGenerationParamsDelegate));
				return;
			case 313:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetFrameDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetFrameDelegate));
				return;
			case 314:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetLodAtlasShadingIndexDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetLodAtlasShadingIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetLodAtlasShadingIndexDelegate));
				return;
			case 315:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetLookDirectionDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetLookDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetLookDirectionDelegate));
				return;
			case 316:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetSetupMorphNodeDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetSetupMorphNodeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetSetupMorphNodeDelegate));
				return;
			case 317:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetSkeletonDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetSkeletonDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetSkeletonDelegate));
				return;
			case 318:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetVisibleDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetVisibleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetVisibleDelegate));
				return;
			case 319:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetVoiceDefinitionIndexDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetVoiceDefinitionIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetVoiceDefinitionIndexDelegate));
				return;
			case 320:
				ScriptingInterfaceOfIMBAgentVisuals.call_SetWieldedWeaponIndicesDelegate = (ScriptingInterfaceOfIMBAgentVisuals.SetWieldedWeaponIndicesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.SetWieldedWeaponIndicesDelegate));
				return;
			case 321:
				ScriptingInterfaceOfIMBAgentVisuals.call_StartRhubarbRecordDelegate = (ScriptingInterfaceOfIMBAgentVisuals.StartRhubarbRecordDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.StartRhubarbRecordDelegate));
				return;
			case 322:
				ScriptingInterfaceOfIMBAgentVisuals.call_TickDelegate = (ScriptingInterfaceOfIMBAgentVisuals.TickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.TickDelegate));
				return;
			case 323:
				ScriptingInterfaceOfIMBAgentVisuals.call_UpdateQuiverMeshesWithoutAgentDelegate = (ScriptingInterfaceOfIMBAgentVisuals.UpdateQuiverMeshesWithoutAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.UpdateQuiverMeshesWithoutAgentDelegate));
				return;
			case 324:
				ScriptingInterfaceOfIMBAgentVisuals.call_UpdateSkeletonScaleDelegate = (ScriptingInterfaceOfIMBAgentVisuals.UpdateSkeletonScaleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.UpdateSkeletonScaleDelegate));
				return;
			case 325:
				ScriptingInterfaceOfIMBAgentVisuals.call_UseScaledWeaponsDelegate = (ScriptingInterfaceOfIMBAgentVisuals.UseScaledWeaponsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.UseScaledWeaponsDelegate));
				return;
			case 326:
				ScriptingInterfaceOfIMBAgentVisuals.call_ValidateAgentVisualsResetedDelegate = (ScriptingInterfaceOfIMBAgentVisuals.ValidateAgentVisualsResetedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAgentVisuals.ValidateAgentVisualsResetedDelegate));
				return;
			case 327:
				ScriptingInterfaceOfIMBAnimation.call_AnimationIndexOfActionCodeDelegate = (ScriptingInterfaceOfIMBAnimation.AnimationIndexOfActionCodeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.AnimationIndexOfActionCodeDelegate));
				return;
			case 328:
				ScriptingInterfaceOfIMBAnimation.call_CheckAnimationClipExistsDelegate = (ScriptingInterfaceOfIMBAnimation.CheckAnimationClipExistsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.CheckAnimationClipExistsDelegate));
				return;
			case 329:
				ScriptingInterfaceOfIMBAnimation.call_GetActionAnimationDurationDelegate = (ScriptingInterfaceOfIMBAnimation.GetActionAnimationDurationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetActionAnimationDurationDelegate));
				return;
			case 330:
				ScriptingInterfaceOfIMBAnimation.call_GetActionBlendOutStartProgressDelegate = (ScriptingInterfaceOfIMBAnimation.GetActionBlendOutStartProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetActionBlendOutStartProgressDelegate));
				return;
			case 331:
				ScriptingInterfaceOfIMBAnimation.call_GetActionCodeWithNameDelegate = (ScriptingInterfaceOfIMBAnimation.GetActionCodeWithNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetActionCodeWithNameDelegate));
				return;
			case 332:
				ScriptingInterfaceOfIMBAnimation.call_GetActionNameWithCodeDelegate = (ScriptingInterfaceOfIMBAnimation.GetActionNameWithCodeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetActionNameWithCodeDelegate));
				return;
			case 333:
				ScriptingInterfaceOfIMBAnimation.call_GetActionTypeDelegate = (ScriptingInterfaceOfIMBAnimation.GetActionTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetActionTypeDelegate));
				return;
			case 334:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendInPeriodDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationBlendInPeriodDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationBlendInPeriodDelegate));
				return;
			case 335:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendsWithActionIndexDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationBlendsWithActionIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationBlendsWithActionIndexDelegate));
				return;
			case 336:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationContinueToActionDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationContinueToActionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationContinueToActionDelegate));
				return;
			case 337:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationDisplacementAtProgressDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationDisplacementAtProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationDisplacementAtProgressDelegate));
				return;
			case 338:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationDurationDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationDurationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationDurationDelegate));
				return;
			case 339:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationFlagsDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationFlagsDelegate));
				return;
			case 340:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationNameDelegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationNameDelegate));
				return;
			case 341:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter1Delegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationParameter1Delegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationParameter1Delegate));
				return;
			case 342:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter2Delegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationParameter2Delegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationParameter2Delegate));
				return;
			case 343:
				ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter3Delegate = (ScriptingInterfaceOfIMBAnimation.GetAnimationParameter3Delegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetAnimationParameter3Delegate));
				return;
			case 344:
				ScriptingInterfaceOfIMBAnimation.call_GetDisplacementVectorDelegate = (ScriptingInterfaceOfIMBAnimation.GetDisplacementVectorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetDisplacementVectorDelegate));
				return;
			case 345:
				ScriptingInterfaceOfIMBAnimation.call_GetIDWithIndexDelegate = (ScriptingInterfaceOfIMBAnimation.GetIDWithIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetIDWithIndexDelegate));
				return;
			case 346:
				ScriptingInterfaceOfIMBAnimation.call_GetIndexWithIDDelegate = (ScriptingInterfaceOfIMBAnimation.GetIndexWithIDDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetIndexWithIDDelegate));
				return;
			case 347:
				ScriptingInterfaceOfIMBAnimation.call_GetNumActionCodesDelegate = (ScriptingInterfaceOfIMBAnimation.GetNumActionCodesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetNumActionCodesDelegate));
				return;
			case 348:
				ScriptingInterfaceOfIMBAnimation.call_GetNumAnimationsDelegate = (ScriptingInterfaceOfIMBAnimation.GetNumAnimationsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.GetNumAnimationsDelegate));
				return;
			case 349:
				ScriptingInterfaceOfIMBAnimation.call_IsAnyAnimationLoadingFromDiskDelegate = (ScriptingInterfaceOfIMBAnimation.IsAnyAnimationLoadingFromDiskDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.IsAnyAnimationLoadingFromDiskDelegate));
				return;
			case 350:
				ScriptingInterfaceOfIMBAnimation.call_PrefetchAnimationClipDelegate = (ScriptingInterfaceOfIMBAnimation.PrefetchAnimationClipDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBAnimation.PrefetchAnimationClipDelegate));
				return;
			case 351:
				ScriptingInterfaceOfIMBBannerlordChecker.call_GetEngineStructMemberOffsetDelegate = (ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructMemberOffsetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructMemberOffsetDelegate));
				return;
			case 352:
				ScriptingInterfaceOfIMBBannerlordChecker.call_GetEngineStructSizeDelegate = (ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructSizeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructSizeDelegate));
				return;
			case 353:
				ScriptingInterfaceOfIMBBannerlordConfig.call_ValidateOptionsDelegate = (ScriptingInterfaceOfIMBBannerlordConfig.ValidateOptionsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordConfig.ValidateOptionsDelegate));
				return;
			case 354:
				ScriptingInterfaceOfIMBBannerlordTableauManager.call_GetNumberOfPendingTableauRequestsDelegate = (ScriptingInterfaceOfIMBBannerlordTableauManager.GetNumberOfPendingTableauRequestsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordTableauManager.GetNumberOfPendingTableauRequestsDelegate));
				return;
			case 355:
				ScriptingInterfaceOfIMBBannerlordTableauManager.call_InitializeCharacterTableauRenderSystemDelegate = (ScriptingInterfaceOfIMBBannerlordTableauManager.InitializeCharacterTableauRenderSystemDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordTableauManager.InitializeCharacterTableauRenderSystemDelegate));
				return;
			case 356:
				ScriptingInterfaceOfIMBBannerlordTableauManager.call_RequestCharacterTableauRenderDelegate = (ScriptingInterfaceOfIMBBannerlordTableauManager.RequestCharacterTableauRenderDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBBannerlordTableauManager.RequestCharacterTableauRenderDelegate));
				return;
			case 357:
				ScriptingInterfaceOfIMBDebugExtensions.call_OverrideNativeParameterDelegate = (ScriptingInterfaceOfIMBDebugExtensions.OverrideNativeParameterDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBDebugExtensions.OverrideNativeParameterDelegate));
				return;
			case 358:
				ScriptingInterfaceOfIMBDebugExtensions.call_ReloadNativeParametersDelegate = (ScriptingInterfaceOfIMBDebugExtensions.ReloadNativeParametersDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBDebugExtensions.ReloadNativeParametersDelegate));
				return;
			case 359:
				ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugArcOnTerrainDelegate = (ScriptingInterfaceOfIMBDebugExtensions.RenderDebugArcOnTerrainDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBDebugExtensions.RenderDebugArcOnTerrainDelegate));
				return;
			case 360:
				ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugCircleOnTerrainDelegate = (ScriptingInterfaceOfIMBDebugExtensions.RenderDebugCircleOnTerrainDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBDebugExtensions.RenderDebugCircleOnTerrainDelegate));
				return;
			case 361:
				ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugLineOnTerrainDelegate = (ScriptingInterfaceOfIMBDebugExtensions.RenderDebugLineOnTerrainDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBDebugExtensions.RenderDebugLineOnTerrainDelegate));
				return;
			case 362:
				ScriptingInterfaceOfIMBEditor.call_ActivateSceneEditorPresentationDelegate = (ScriptingInterfaceOfIMBEditor.ActivateSceneEditorPresentationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.ActivateSceneEditorPresentationDelegate));
				return;
			case 363:
				ScriptingInterfaceOfIMBEditor.call_AddEditorWarningDelegate = (ScriptingInterfaceOfIMBEditor.AddEditorWarningDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.AddEditorWarningDelegate));
				return;
			case 364:
				ScriptingInterfaceOfIMBEditor.call_AddEntityWarningDelegate = (ScriptingInterfaceOfIMBEditor.AddEntityWarningDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.AddEntityWarningDelegate));
				return;
			case 365:
				ScriptingInterfaceOfIMBEditor.call_AddNavMeshWarningDelegate = (ScriptingInterfaceOfIMBEditor.AddNavMeshWarningDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.AddNavMeshWarningDelegate));
				return;
			case 366:
				ScriptingInterfaceOfIMBEditor.call_ApplyDeltaToEditorCameraDelegate = (ScriptingInterfaceOfIMBEditor.ApplyDeltaToEditorCameraDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.ApplyDeltaToEditorCameraDelegate));
				return;
			case 367:
				ScriptingInterfaceOfIMBEditor.call_BorderHelpersEnabledDelegate = (ScriptingInterfaceOfIMBEditor.BorderHelpersEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.BorderHelpersEnabledDelegate));
				return;
			case 368:
				ScriptingInterfaceOfIMBEditor.call_DeactivateSceneEditorPresentationDelegate = (ScriptingInterfaceOfIMBEditor.DeactivateSceneEditorPresentationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.DeactivateSceneEditorPresentationDelegate));
				return;
			case 369:
				ScriptingInterfaceOfIMBEditor.call_EnterEditMissionModeDelegate = (ScriptingInterfaceOfIMBEditor.EnterEditMissionModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.EnterEditMissionModeDelegate));
				return;
			case 370:
				ScriptingInterfaceOfIMBEditor.call_EnterEditModeDelegate = (ScriptingInterfaceOfIMBEditor.EnterEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.EnterEditModeDelegate));
				return;
			case 371:
				ScriptingInterfaceOfIMBEditor.call_ExitEditModeDelegate = (ScriptingInterfaceOfIMBEditor.ExitEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.ExitEditModeDelegate));
				return;
			case 372:
				ScriptingInterfaceOfIMBEditor.call_GetAllPrefabsAndChildWithTagDelegate = (ScriptingInterfaceOfIMBEditor.GetAllPrefabsAndChildWithTagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.GetAllPrefabsAndChildWithTagDelegate));
				return;
			case 373:
				ScriptingInterfaceOfIMBEditor.call_GetEditorSceneViewDelegate = (ScriptingInterfaceOfIMBEditor.GetEditorSceneViewDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.GetEditorSceneViewDelegate));
				return;
			case 374:
				ScriptingInterfaceOfIMBEditor.call_HelpersEnabledDelegate = (ScriptingInterfaceOfIMBEditor.HelpersEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.HelpersEnabledDelegate));
				return;
			case 375:
				ScriptingInterfaceOfIMBEditor.call_IsEditModeDelegate = (ScriptingInterfaceOfIMBEditor.IsEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsEditModeDelegate));
				return;
			case 376:
				ScriptingInterfaceOfIMBEditor.call_IsEditModeEnabledDelegate = (ScriptingInterfaceOfIMBEditor.IsEditModeEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsEditModeEnabledDelegate));
				return;
			case 377:
				ScriptingInterfaceOfIMBEditor.call_IsEntitySelectedDelegate = (ScriptingInterfaceOfIMBEditor.IsEntitySelectedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsEntitySelectedDelegate));
				return;
			case 378:
				ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRecordingDelegate = (ScriptingInterfaceOfIMBEditor.IsReplayManagerRecordingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsReplayManagerRecordingDelegate));
				return;
			case 379:
				ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRenderingDelegate = (ScriptingInterfaceOfIMBEditor.IsReplayManagerRenderingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsReplayManagerRenderingDelegate));
				return;
			case 380:
				ScriptingInterfaceOfIMBEditor.call_IsReplayManagerReplayingDelegate = (ScriptingInterfaceOfIMBEditor.IsReplayManagerReplayingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.IsReplayManagerReplayingDelegate));
				return;
			case 381:
				ScriptingInterfaceOfIMBEditor.call_LeaveEditMissionModeDelegate = (ScriptingInterfaceOfIMBEditor.LeaveEditMissionModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.LeaveEditMissionModeDelegate));
				return;
			case 382:
				ScriptingInterfaceOfIMBEditor.call_LeaveEditModeDelegate = (ScriptingInterfaceOfIMBEditor.LeaveEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.LeaveEditModeDelegate));
				return;
			case 383:
				ScriptingInterfaceOfIMBEditor.call_RenderEditorMeshDelegate = (ScriptingInterfaceOfIMBEditor.RenderEditorMeshDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.RenderEditorMeshDelegate));
				return;
			case 384:
				ScriptingInterfaceOfIMBEditor.call_SetLevelVisibilityDelegate = (ScriptingInterfaceOfIMBEditor.SetLevelVisibilityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.SetLevelVisibilityDelegate));
				return;
			case 385:
				ScriptingInterfaceOfIMBEditor.call_SetUpgradeLevelVisibilityDelegate = (ScriptingInterfaceOfIMBEditor.SetUpgradeLevelVisibilityDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.SetUpgradeLevelVisibilityDelegate));
				return;
			case 386:
				ScriptingInterfaceOfIMBEditor.call_TickEditModeDelegate = (ScriptingInterfaceOfIMBEditor.TickEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.TickEditModeDelegate));
				return;
			case 387:
				ScriptingInterfaceOfIMBEditor.call_TickSceneEditorPresentationDelegate = (ScriptingInterfaceOfIMBEditor.TickSceneEditorPresentationDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.TickSceneEditorPresentationDelegate));
				return;
			case 388:
				ScriptingInterfaceOfIMBEditor.call_ToggleEnableEditorPhysicsDelegate = (ScriptingInterfaceOfIMBEditor.ToggleEnableEditorPhysicsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.ToggleEnableEditorPhysicsDelegate));
				return;
			case 389:
				ScriptingInterfaceOfIMBEditor.call_UpdateSceneTreeDelegate = (ScriptingInterfaceOfIMBEditor.UpdateSceneTreeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.UpdateSceneTreeDelegate));
				return;
			case 390:
				ScriptingInterfaceOfIMBEditor.call_ZoomToPositionDelegate = (ScriptingInterfaceOfIMBEditor.ZoomToPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBEditor.ZoomToPositionDelegate));
				return;
			case 391:
				ScriptingInterfaceOfIMBFaceGen.call_EnforceConstraintsDelegate = (ScriptingInterfaceOfIMBFaceGen.EnforceConstraintsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.EnforceConstraintsDelegate));
				return;
			case 392:
				ScriptingInterfaceOfIMBFaceGen.call_FlushFaceCacheDelegate = (ScriptingInterfaceOfIMBFaceGen.FlushFaceCacheDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.FlushFaceCacheDelegate));
				return;
			case 393:
				ScriptingInterfaceOfIMBFaceGen.call_GetDeformKeyDataDelegate = (ScriptingInterfaceOfIMBFaceGen.GetDeformKeyDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetDeformKeyDataDelegate));
				return;
			case 394:
				ScriptingInterfaceOfIMBFaceGen.call_GetFaceGenInstancesLengthDelegate = (ScriptingInterfaceOfIMBFaceGen.GetFaceGenInstancesLengthDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetFaceGenInstancesLengthDelegate));
				return;
			case 395:
				ScriptingInterfaceOfIMBFaceGen.call_GetFacialIndicesByTagDelegate = (ScriptingInterfaceOfIMBFaceGen.GetFacialIndicesByTagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetFacialIndicesByTagDelegate));
				return;
			case 396:
				ScriptingInterfaceOfIMBFaceGen.call_GetHairColorCountDelegate = (ScriptingInterfaceOfIMBFaceGen.GetHairColorCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetHairColorCountDelegate));
				return;
			case 397:
				ScriptingInterfaceOfIMBFaceGen.call_GetHairColorGradientPointsDelegate = (ScriptingInterfaceOfIMBFaceGen.GetHairColorGradientPointsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetHairColorGradientPointsDelegate));
				return;
			case 398:
				ScriptingInterfaceOfIMBFaceGen.call_GetHairIndicesByTagDelegate = (ScriptingInterfaceOfIMBFaceGen.GetHairIndicesByTagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetHairIndicesByTagDelegate));
				return;
			case 399:
				ScriptingInterfaceOfIMBFaceGen.call_GetMaturityTypeDelegate = (ScriptingInterfaceOfIMBFaceGen.GetMaturityTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetMaturityTypeDelegate));
				return;
			case 400:
				ScriptingInterfaceOfIMBFaceGen.call_GetNumEditableDeformKeysDelegate = (ScriptingInterfaceOfIMBFaceGen.GetNumEditableDeformKeysDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetNumEditableDeformKeysDelegate));
				return;
			case 401:
				ScriptingInterfaceOfIMBFaceGen.call_GetParamsFromKeyDelegate = (ScriptingInterfaceOfIMBFaceGen.GetParamsFromKeyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetParamsFromKeyDelegate));
				return;
			case 402:
				ScriptingInterfaceOfIMBFaceGen.call_GetParamsMaxDelegate = (ScriptingInterfaceOfIMBFaceGen.GetParamsMaxDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetParamsMaxDelegate));
				return;
			case 403:
				ScriptingInterfaceOfIMBFaceGen.call_GetRaceIdsDelegate = (ScriptingInterfaceOfIMBFaceGen.GetRaceIdsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetRaceIdsDelegate));
				return;
			case 404:
				ScriptingInterfaceOfIMBFaceGen.call_GetRandomBodyPropertiesDelegate = (ScriptingInterfaceOfIMBFaceGen.GetRandomBodyPropertiesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetRandomBodyPropertiesDelegate));
				return;
			case 405:
				ScriptingInterfaceOfIMBFaceGen.call_GetScaleFromKeyDelegate = (ScriptingInterfaceOfIMBFaceGen.GetScaleFromKeyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetScaleFromKeyDelegate));
				return;
			case 406:
				ScriptingInterfaceOfIMBFaceGen.call_GetSkinColorCountDelegate = (ScriptingInterfaceOfIMBFaceGen.GetSkinColorCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetSkinColorCountDelegate));
				return;
			case 407:
				ScriptingInterfaceOfIMBFaceGen.call_GetSkinColorGradientPointsDelegate = (ScriptingInterfaceOfIMBFaceGen.GetSkinColorGradientPointsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetSkinColorGradientPointsDelegate));
				return;
			case 408:
				ScriptingInterfaceOfIMBFaceGen.call_GetTatooColorCountDelegate = (ScriptingInterfaceOfIMBFaceGen.GetTatooColorCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetTatooColorCountDelegate));
				return;
			case 409:
				ScriptingInterfaceOfIMBFaceGen.call_GetTatooColorGradientPointsDelegate = (ScriptingInterfaceOfIMBFaceGen.GetTatooColorGradientPointsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetTatooColorGradientPointsDelegate));
				return;
			case 410:
				ScriptingInterfaceOfIMBFaceGen.call_GetTattooIndicesByTagDelegate = (ScriptingInterfaceOfIMBFaceGen.GetTattooIndicesByTagDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetTattooIndicesByTagDelegate));
				return;
			case 411:
				ScriptingInterfaceOfIMBFaceGen.call_GetVoiceRecordsCountDelegate = (ScriptingInterfaceOfIMBFaceGen.GetVoiceRecordsCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetVoiceRecordsCountDelegate));
				return;
			case 412:
				ScriptingInterfaceOfIMBFaceGen.call_GetVoiceTypeUsableForPlayerDataDelegate = (ScriptingInterfaceOfIMBFaceGen.GetVoiceTypeUsableForPlayerDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetVoiceTypeUsableForPlayerDataDelegate));
				return;
			case 413:
				ScriptingInterfaceOfIMBFaceGen.call_GetZeroProbabilitiesDelegate = (ScriptingInterfaceOfIMBFaceGen.GetZeroProbabilitiesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.GetZeroProbabilitiesDelegate));
				return;
			case 414:
				ScriptingInterfaceOfIMBFaceGen.call_ProduceNumericKeyWithDefaultValuesDelegate = (ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithDefaultValuesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithDefaultValuesDelegate));
				return;
			case 415:
				ScriptingInterfaceOfIMBFaceGen.call_ProduceNumericKeyWithParamsDelegate = (ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithParamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.ProduceNumericKeyWithParamsDelegate));
				return;
			case 416:
				ScriptingInterfaceOfIMBFaceGen.call_TransformFaceKeysToDefaultFaceDelegate = (ScriptingInterfaceOfIMBFaceGen.TransformFaceKeysToDefaultFaceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBFaceGen.TransformFaceKeysToDefaultFaceDelegate));
				return;
			case 417:
				ScriptingInterfaceOfIMBGame.call_LoadModuleDataDelegate = (ScriptingInterfaceOfIMBGame.LoadModuleDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGame.LoadModuleDataDelegate));
				return;
			case 418:
				ScriptingInterfaceOfIMBGame.call_StartNewDelegate = (ScriptingInterfaceOfIMBGame.StartNewDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGame.StartNewDelegate));
				return;
			case 419:
				ScriptingInterfaceOfIMBGameEntityExtensions.call_CreateFromWeaponDelegate = (ScriptingInterfaceOfIMBGameEntityExtensions.CreateFromWeaponDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGameEntityExtensions.CreateFromWeaponDelegate));
				return;
			case 420:
				ScriptingInterfaceOfIMBGameEntityExtensions.call_FadeInDelegate = (ScriptingInterfaceOfIMBGameEntityExtensions.FadeInDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGameEntityExtensions.FadeInDelegate));
				return;
			case 421:
				ScriptingInterfaceOfIMBGameEntityExtensions.call_FadeOutDelegate = (ScriptingInterfaceOfIMBGameEntityExtensions.FadeOutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGameEntityExtensions.FadeOutDelegate));
				return;
			case 422:
				ScriptingInterfaceOfIMBGameEntityExtensions.call_HideIfNotFadingOutDelegate = (ScriptingInterfaceOfIMBGameEntityExtensions.HideIfNotFadingOutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBGameEntityExtensions.HideIfNotFadingOutDelegate));
				return;
			case 423:
				ScriptingInterfaceOfIMBItem.call_GetHolsterFrameByIndexDelegate = (ScriptingInterfaceOfIMBItem.GetHolsterFrameByIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetHolsterFrameByIndexDelegate));
				return;
			case 424:
				ScriptingInterfaceOfIMBItem.call_GetItemHolsterIndexDelegate = (ScriptingInterfaceOfIMBItem.GetItemHolsterIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemHolsterIndexDelegate));
				return;
			case 425:
				ScriptingInterfaceOfIMBItem.call_GetItemIsPassiveUsageDelegate = (ScriptingInterfaceOfIMBItem.GetItemIsPassiveUsageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemIsPassiveUsageDelegate));
				return;
			case 426:
				ScriptingInterfaceOfIMBItem.call_GetItemUsageIndexDelegate = (ScriptingInterfaceOfIMBItem.GetItemUsageIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemUsageIndexDelegate));
				return;
			case 427:
				ScriptingInterfaceOfIMBItem.call_GetItemUsageReloadActionCodeDelegate = (ScriptingInterfaceOfIMBItem.GetItemUsageReloadActionCodeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemUsageReloadActionCodeDelegate));
				return;
			case 428:
				ScriptingInterfaceOfIMBItem.call_GetItemUsageSetFlagsDelegate = (ScriptingInterfaceOfIMBItem.GetItemUsageSetFlagsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemUsageSetFlagsDelegate));
				return;
			case 429:
				ScriptingInterfaceOfIMBItem.call_GetItemUsageStrikeTypeDelegate = (ScriptingInterfaceOfIMBItem.GetItemUsageStrikeTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetItemUsageStrikeTypeDelegate));
				return;
			case 430:
				ScriptingInterfaceOfIMBItem.call_GetMissileRangeDelegate = (ScriptingInterfaceOfIMBItem.GetMissileRangeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBItem.GetMissileRangeDelegate));
				return;
			case 431:
				ScriptingInterfaceOfIMBMapScene.call_GetAccessiblePointNearPositionDelegate = (ScriptingInterfaceOfIMBMapScene.GetAccessiblePointNearPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetAccessiblePointNearPositionDelegate));
				return;
			case 432:
				ScriptingInterfaceOfIMBMapScene.call_GetBattleSceneIndexMapDelegate = (ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapDelegate));
				return;
			case 433:
				ScriptingInterfaceOfIMBMapScene.call_GetBattleSceneIndexMapResolutionDelegate = (ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapResolutionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapResolutionDelegate));
				return;
			case 434:
				ScriptingInterfaceOfIMBMapScene.call_GetColorGradeGridDataDelegate = (ScriptingInterfaceOfIMBMapScene.GetColorGradeGridDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetColorGradeGridDataDelegate));
				return;
			case 435:
				ScriptingInterfaceOfIMBMapScene.call_GetMouseVisibleDelegate = (ScriptingInterfaceOfIMBMapScene.GetMouseVisibleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetMouseVisibleDelegate));
				return;
			case 436:
				ScriptingInterfaceOfIMBMapScene.call_GetNearestFaceCenterForPositionWithPathDelegate = (ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterForPositionWithPathDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterForPositionWithPathDelegate));
				return;
			case 437:
				ScriptingInterfaceOfIMBMapScene.call_GetNearestFaceCenterPositionForPositionDelegate = (ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterPositionForPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterPositionForPositionDelegate));
				return;
			case 438:
				ScriptingInterfaceOfIMBMapScene.call_GetSeasonTimeFactorDelegate = (ScriptingInterfaceOfIMBMapScene.GetSeasonTimeFactorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.GetSeasonTimeFactorDelegate));
				return;
			case 439:
				ScriptingInterfaceOfIMBMapScene.call_LoadAtmosphereDataDelegate = (ScriptingInterfaceOfIMBMapScene.LoadAtmosphereDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.LoadAtmosphereDataDelegate));
				return;
			case 440:
				ScriptingInterfaceOfIMBMapScene.call_RemoveZeroCornerBodiesDelegate = (ScriptingInterfaceOfIMBMapScene.RemoveZeroCornerBodiesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.RemoveZeroCornerBodiesDelegate));
				return;
			case 441:
				ScriptingInterfaceOfIMBMapScene.call_SendMouseKeyEventDelegate = (ScriptingInterfaceOfIMBMapScene.SendMouseKeyEventDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SendMouseKeyEventDelegate));
				return;
			case 442:
				ScriptingInterfaceOfIMBMapScene.call_SetFrameForAtmosphereDelegate = (ScriptingInterfaceOfIMBMapScene.SetFrameForAtmosphereDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetFrameForAtmosphereDelegate));
				return;
			case 443:
				ScriptingInterfaceOfIMBMapScene.call_SetMousePosDelegate = (ScriptingInterfaceOfIMBMapScene.SetMousePosDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetMousePosDelegate));
				return;
			case 444:
				ScriptingInterfaceOfIMBMapScene.call_SetMouseVisibleDelegate = (ScriptingInterfaceOfIMBMapScene.SetMouseVisibleDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetMouseVisibleDelegate));
				return;
			case 445:
				ScriptingInterfaceOfIMBMapScene.call_SetPoliticalColorDelegate = (ScriptingInterfaceOfIMBMapScene.SetPoliticalColorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetPoliticalColorDelegate));
				return;
			case 446:
				ScriptingInterfaceOfIMBMapScene.call_SetSeasonTimeFactorDelegate = (ScriptingInterfaceOfIMBMapScene.SetSeasonTimeFactorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetSeasonTimeFactorDelegate));
				return;
			case 447:
				ScriptingInterfaceOfIMBMapScene.call_SetTerrainDynamicParamsDelegate = (ScriptingInterfaceOfIMBMapScene.SetTerrainDynamicParamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.SetTerrainDynamicParamsDelegate));
				return;
			case 448:
				ScriptingInterfaceOfIMBMapScene.call_TickAmbientSoundsDelegate = (ScriptingInterfaceOfIMBMapScene.TickAmbientSoundsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.TickAmbientSoundsDelegate));
				return;
			case 449:
				ScriptingInterfaceOfIMBMapScene.call_TickStepSoundDelegate = (ScriptingInterfaceOfIMBMapScene.TickStepSoundDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.TickStepSoundDelegate));
				return;
			case 450:
				ScriptingInterfaceOfIMBMapScene.call_TickVisualsDelegate = (ScriptingInterfaceOfIMBMapScene.TickVisualsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.TickVisualsDelegate));
				return;
			case 451:
				ScriptingInterfaceOfIMBMapScene.call_ValidateTerrainSoundIdsDelegate = (ScriptingInterfaceOfIMBMapScene.ValidateTerrainSoundIdsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMapScene.ValidateTerrainSoundIdsDelegate));
				return;
			case 452:
				ScriptingInterfaceOfIMBMessageManager.call_DisplayMessageDelegate = (ScriptingInterfaceOfIMBMessageManager.DisplayMessageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMessageManager.DisplayMessageDelegate));
				return;
			case 453:
				ScriptingInterfaceOfIMBMessageManager.call_DisplayMessageWithColorDelegate = (ScriptingInterfaceOfIMBMessageManager.DisplayMessageWithColorDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMessageManager.DisplayMessageWithColorDelegate));
				return;
			case 454:
				ScriptingInterfaceOfIMBMessageManager.call_SetMessageManagerDelegate = (ScriptingInterfaceOfIMBMessageManager.SetMessageManagerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMessageManager.SetMessageManagerDelegate));
				return;
			case 455:
				ScriptingInterfaceOfIMBMission.call_AddAiDebugTextDelegate = (ScriptingInterfaceOfIMBMission.AddAiDebugTextDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddAiDebugTextDelegate));
				return;
			case 456:
				ScriptingInterfaceOfIMBMission.call_AddBoundaryDelegate = (ScriptingInterfaceOfIMBMission.AddBoundaryDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddBoundaryDelegate));
				return;
			case 457:
				ScriptingInterfaceOfIMBMission.call_AddMissileDelegate = (ScriptingInterfaceOfIMBMission.AddMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddMissileDelegate));
				return;
			case 458:
				ScriptingInterfaceOfIMBMission.call_AddMissileSingleUsageDelegate = (ScriptingInterfaceOfIMBMission.AddMissileSingleUsageDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddMissileSingleUsageDelegate));
				return;
			case 459:
				ScriptingInterfaceOfIMBMission.call_AddParticleSystemBurstByNameDelegate = (ScriptingInterfaceOfIMBMission.AddParticleSystemBurstByNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddParticleSystemBurstByNameDelegate));
				return;
			case 460:
				ScriptingInterfaceOfIMBMission.call_AddTeamDelegate = (ScriptingInterfaceOfIMBMission.AddTeamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.AddTeamDelegate));
				return;
			case 461:
				ScriptingInterfaceOfIMBMission.call_BackupRecordToFileDelegate = (ScriptingInterfaceOfIMBMission.BackupRecordToFileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.BackupRecordToFileDelegate));
				return;
			case 462:
				ScriptingInterfaceOfIMBMission.call_BatchFormationUnitPositionsDelegate = (ScriptingInterfaceOfIMBMission.BatchFormationUnitPositionsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.BatchFormationUnitPositionsDelegate));
				return;
			case 463:
				ScriptingInterfaceOfIMBMission.call_ClearAgentActionsDelegate = (ScriptingInterfaceOfIMBMission.ClearAgentActionsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearAgentActionsDelegate));
				return;
			case 464:
				ScriptingInterfaceOfIMBMission.call_ClearCorpsesDelegate = (ScriptingInterfaceOfIMBMission.ClearCorpsesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearCorpsesDelegate));
				return;
			case 465:
				ScriptingInterfaceOfIMBMission.call_ClearMissilesDelegate = (ScriptingInterfaceOfIMBMission.ClearMissilesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearMissilesDelegate));
				return;
			case 466:
				ScriptingInterfaceOfIMBMission.call_ClearRecordBuffersDelegate = (ScriptingInterfaceOfIMBMission.ClearRecordBuffersDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearRecordBuffersDelegate));
				return;
			case 467:
				ScriptingInterfaceOfIMBMission.call_ClearResourcesDelegate = (ScriptingInterfaceOfIMBMission.ClearResourcesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearResourcesDelegate));
				return;
			case 468:
				ScriptingInterfaceOfIMBMission.call_ClearSceneDelegate = (ScriptingInterfaceOfIMBMission.ClearSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ClearSceneDelegate));
				return;
			case 469:
				ScriptingInterfaceOfIMBMission.call_ComputeExactMissileRangeAtHeightDifferenceDelegate = (ScriptingInterfaceOfIMBMission.ComputeExactMissileRangeAtHeightDifferenceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ComputeExactMissileRangeAtHeightDifferenceDelegate));
				return;
			case 470:
				ScriptingInterfaceOfIMBMission.call_CreateAgentDelegate = (ScriptingInterfaceOfIMBMission.CreateAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.CreateAgentDelegate));
				return;
			case 471:
				ScriptingInterfaceOfIMBMission.call_CreateMissionDelegate = (ScriptingInterfaceOfIMBMission.CreateMissionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.CreateMissionDelegate));
				return;
			case 472:
				ScriptingInterfaceOfIMBMission.call_DefragRenderBuffersDelegate = (ScriptingInterfaceOfIMBMission.DefragRenderBuffersDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.DefragRenderBuffersDelegate));
				return;
			case 473:
				ScriptingInterfaceOfIMBMission.call_EndOfRecordDelegate = (ScriptingInterfaceOfIMBMission.EndOfRecordDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.EndOfRecordDelegate));
				return;
			case 474:
				ScriptingInterfaceOfIMBMission.call_FinalizeMissionDelegate = (ScriptingInterfaceOfIMBMission.FinalizeMissionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.FinalizeMissionDelegate));
				return;
			case 475:
				ScriptingInterfaceOfIMBMission.call_FindAgentWithIndexDelegate = (ScriptingInterfaceOfIMBMission.FindAgentWithIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.FindAgentWithIndexDelegate));
				return;
			case 476:
				ScriptingInterfaceOfIMBMission.call_FindConvexHullDelegate = (ScriptingInterfaceOfIMBMission.FindConvexHullDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.FindConvexHullDelegate));
				return;
			case 477:
				ScriptingInterfaceOfIMBMission.call_ForceDisableOcclusionDelegate = (ScriptingInterfaceOfIMBMission.ForceDisableOcclusionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ForceDisableOcclusionDelegate));
				return;
			case 478:
				ScriptingInterfaceOfIMBMission.call_GetAgentCountAroundPositionDelegate = (ScriptingInterfaceOfIMBMission.GetAgentCountAroundPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAgentCountAroundPositionDelegate));
				return;
			case 479:
				ScriptingInterfaceOfIMBMission.call_GetAlternatePositionForNavmeshlessOrOutOfBoundsPositionDelegate = (ScriptingInterfaceOfIMBMission.GetAlternatePositionForNavmeshlessOrOutOfBoundsPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAlternatePositionForNavmeshlessOrOutOfBoundsPositionDelegate));
				return;
			case 480:
				ScriptingInterfaceOfIMBMission.call_GetAtmosphereNameForReplayDelegate = (ScriptingInterfaceOfIMBMission.GetAtmosphereNameForReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAtmosphereNameForReplayDelegate));
				return;
			case 481:
				ScriptingInterfaceOfIMBMission.call_GetAtmosphereSeasonForReplayDelegate = (ScriptingInterfaceOfIMBMission.GetAtmosphereSeasonForReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAtmosphereSeasonForReplayDelegate));
				return;
			case 482:
				ScriptingInterfaceOfIMBMission.call_GetAverageFpsDelegate = (ScriptingInterfaceOfIMBMission.GetAverageFpsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAverageFpsDelegate));
				return;
			case 483:
				ScriptingInterfaceOfIMBMission.call_GetAverageMoraleOfAgentsDelegate = (ScriptingInterfaceOfIMBMission.GetAverageMoraleOfAgentsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetAverageMoraleOfAgentsDelegate));
				return;
			case 484:
				ScriptingInterfaceOfIMBMission.call_GetBestSlopeAngleHeightPosForDefendingDelegate = (ScriptingInterfaceOfIMBMission.GetBestSlopeAngleHeightPosForDefendingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBestSlopeAngleHeightPosForDefendingDelegate));
				return;
			case 485:
				ScriptingInterfaceOfIMBMission.call_GetBestSlopeTowardsDirectionDelegate = (ScriptingInterfaceOfIMBMission.GetBestSlopeTowardsDirectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBestSlopeTowardsDirectionDelegate));
				return;
			case 486:
				ScriptingInterfaceOfIMBMission.call_GetBiggestAgentCollisionPaddingDelegate = (ScriptingInterfaceOfIMBMission.GetBiggestAgentCollisionPaddingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBiggestAgentCollisionPaddingDelegate));
				return;
			case 487:
				ScriptingInterfaceOfIMBMission.call_GetBoundaryCountDelegate = (ScriptingInterfaceOfIMBMission.GetBoundaryCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBoundaryCountDelegate));
				return;
			case 488:
				ScriptingInterfaceOfIMBMission.call_GetBoundaryNameDelegate = (ScriptingInterfaceOfIMBMission.GetBoundaryNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBoundaryNameDelegate));
				return;
			case 489:
				ScriptingInterfaceOfIMBMission.call_GetBoundaryPointsDelegate = (ScriptingInterfaceOfIMBMission.GetBoundaryPointsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBoundaryPointsDelegate));
				return;
			case 490:
				ScriptingInterfaceOfIMBMission.call_GetBoundaryRadiusDelegate = (ScriptingInterfaceOfIMBMission.GetBoundaryRadiusDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetBoundaryRadiusDelegate));
				return;
			case 491:
				ScriptingInterfaceOfIMBMission.call_GetCameraFrameDelegate = (ScriptingInterfaceOfIMBMission.GetCameraFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetCameraFrameDelegate));
				return;
			case 492:
				ScriptingInterfaceOfIMBMission.call_GetClearSceneTimerElapsedTimeDelegate = (ScriptingInterfaceOfIMBMission.GetClearSceneTimerElapsedTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetClearSceneTimerElapsedTimeDelegate));
				return;
			case 493:
				ScriptingInterfaceOfIMBMission.call_GetClosestAllyDelegate = (ScriptingInterfaceOfIMBMission.GetClosestAllyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetClosestAllyDelegate));
				return;
			case 494:
				ScriptingInterfaceOfIMBMission.call_GetClosestBoundaryPositionDelegate = (ScriptingInterfaceOfIMBMission.GetClosestBoundaryPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetClosestBoundaryPositionDelegate));
				return;
			case 495:
				ScriptingInterfaceOfIMBMission.call_GetClosestEnemyDelegate = (ScriptingInterfaceOfIMBMission.GetClosestEnemyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetClosestEnemyDelegate));
				return;
			case 496:
				ScriptingInterfaceOfIMBMission.call_GetCombatTypeDelegate = (ScriptingInterfaceOfIMBMission.GetCombatTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetCombatTypeDelegate));
				return;
			case 497:
				ScriptingInterfaceOfIMBMission.call_GetCurrentVolumeGeneratorVersionDelegate = (ScriptingInterfaceOfIMBMission.GetCurrentVolumeGeneratorVersionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetCurrentVolumeGeneratorVersionDelegate));
				return;
			case 498:
				ScriptingInterfaceOfIMBMission.call_GetDebugAgentDelegate = (ScriptingInterfaceOfIMBMission.GetDebugAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetDebugAgentDelegate));
				return;
			case 499:
				ScriptingInterfaceOfIMBMission.call_GetFallAvoidSystemActiveDelegate = (ScriptingInterfaceOfIMBMission.GetFallAvoidSystemActiveDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetFallAvoidSystemActiveDelegate));
				return;
			case 500:
				ScriptingInterfaceOfIMBMission.call_GetGameTypeForReplayDelegate = (ScriptingInterfaceOfIMBMission.GetGameTypeForReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetGameTypeForReplayDelegate));
				return;
			case 501:
				ScriptingInterfaceOfIMBMission.call_GetIsLoadingFinishedDelegate = (ScriptingInterfaceOfIMBMission.GetIsLoadingFinishedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetIsLoadingFinishedDelegate));
				return;
			case 502:
				ScriptingInterfaceOfIMBMission.call_GetMissileCollisionPointDelegate = (ScriptingInterfaceOfIMBMission.GetMissileCollisionPointDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetMissileCollisionPointDelegate));
				return;
			case 503:
				ScriptingInterfaceOfIMBMission.call_GetMissileHasRigidBodyDelegate = (ScriptingInterfaceOfIMBMission.GetMissileHasRigidBodyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetMissileHasRigidBodyDelegate));
				return;
			case 504:
				ScriptingInterfaceOfIMBMission.call_GetMissileRangeDelegate = (ScriptingInterfaceOfIMBMission.GetMissileRangeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetMissileRangeDelegate));
				return;
			case 505:
				ScriptingInterfaceOfIMBMission.call_GetMissileVerticalAimCorrectionDelegate = (ScriptingInterfaceOfIMBMission.GetMissileVerticalAimCorrectionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetMissileVerticalAimCorrectionDelegate));
				return;
			case 506:
				ScriptingInterfaceOfIMBMission.call_GetNavigationPointsDelegate = (ScriptingInterfaceOfIMBMission.GetNavigationPointsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetNavigationPointsDelegate));
				return;
			case 507:
				ScriptingInterfaceOfIMBMission.call_GetNearbyAgentsAuxDelegate = (ScriptingInterfaceOfIMBMission.GetNearbyAgentsAuxDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetNearbyAgentsAuxDelegate));
				return;
			case 508:
				ScriptingInterfaceOfIMBMission.call_GetNumberOfTeamsDelegate = (ScriptingInterfaceOfIMBMission.GetNumberOfTeamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetNumberOfTeamsDelegate));
				return;
			case 509:
				ScriptingInterfaceOfIMBMission.call_GetOldPositionOfMissileDelegate = (ScriptingInterfaceOfIMBMission.GetOldPositionOfMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetOldPositionOfMissileDelegate));
				return;
			case 510:
				ScriptingInterfaceOfIMBMission.call_GetPauseAITickDelegate = (ScriptingInterfaceOfIMBMission.GetPauseAITickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetPauseAITickDelegate));
				return;
			case 511:
				ScriptingInterfaceOfIMBMission.call_GetPositionOfMissileDelegate = (ScriptingInterfaceOfIMBMission.GetPositionOfMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetPositionOfMissileDelegate));
				return;
			case 512:
				ScriptingInterfaceOfIMBMission.call_GetSceneLevelsForReplayDelegate = (ScriptingInterfaceOfIMBMission.GetSceneLevelsForReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetSceneLevelsForReplayDelegate));
				return;
			case 513:
				ScriptingInterfaceOfIMBMission.call_GetSceneNameForReplayDelegate = (ScriptingInterfaceOfIMBMission.GetSceneNameForReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetSceneNameForReplayDelegate));
				return;
			case 514:
				ScriptingInterfaceOfIMBMission.call_GetStraightPathToTargetDelegate = (ScriptingInterfaceOfIMBMission.GetStraightPathToTargetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetStraightPathToTargetDelegate));
				return;
			case 515:
				ScriptingInterfaceOfIMBMission.call_GetTickDebugPausedDelegate = (ScriptingInterfaceOfIMBMission.GetTickDebugPausedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetTickDebugPausedDelegate));
				return;
			case 516:
				ScriptingInterfaceOfIMBMission.call_GetTimeDelegate = (ScriptingInterfaceOfIMBMission.GetTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetTimeDelegate));
				return;
			case 517:
				ScriptingInterfaceOfIMBMission.call_GetVelocityOfMissileDelegate = (ScriptingInterfaceOfIMBMission.GetVelocityOfMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetVelocityOfMissileDelegate));
				return;
			case 518:
				ScriptingInterfaceOfIMBMission.call_GetWaterLevelAtPositionDelegate = (ScriptingInterfaceOfIMBMission.GetWaterLevelAtPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetWaterLevelAtPositionDelegate));
				return;
			case 519:
				ScriptingInterfaceOfIMBMission.call_GetWeightedPointOfEnemiesDelegate = (ScriptingInterfaceOfIMBMission.GetWeightedPointOfEnemiesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.GetWeightedPointOfEnemiesDelegate));
				return;
			case 520:
				ScriptingInterfaceOfIMBMission.call_HasAnyAgentsOfTeamAroundDelegate = (ScriptingInterfaceOfIMBMission.HasAnyAgentsOfTeamAroundDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.HasAnyAgentsOfTeamAroundDelegate));
				return;
			case 521:
				ScriptingInterfaceOfIMBMission.call_IdleTickDelegate = (ScriptingInterfaceOfIMBMission.IdleTickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IdleTickDelegate));
				return;
			case 522:
				ScriptingInterfaceOfIMBMission.call_InitializeMissionDelegate = (ScriptingInterfaceOfIMBMission.InitializeMissionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.InitializeMissionDelegate));
				return;
			case 523:
				ScriptingInterfaceOfIMBMission.call_IsAgentInProximityMapDelegate = (ScriptingInterfaceOfIMBMission.IsAgentInProximityMapDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsAgentInProximityMapDelegate));
				return;
			case 524:
				ScriptingInterfaceOfIMBMission.call_IsFormationUnitPositionAvailableDelegate = (ScriptingInterfaceOfIMBMission.IsFormationUnitPositionAvailableDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsFormationUnitPositionAvailableDelegate));
				return;
			case 525:
				ScriptingInterfaceOfIMBMission.call_IsPositionInsideAnyBlockerNavMeshFace2DDelegate = (ScriptingInterfaceOfIMBMission.IsPositionInsideAnyBlockerNavMeshFace2DDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsPositionInsideAnyBlockerNavMeshFace2DDelegate));
				return;
			case 526:
				ScriptingInterfaceOfIMBMission.call_IsPositionInsideBoundariesDelegate = (ScriptingInterfaceOfIMBMission.IsPositionInsideBoundariesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsPositionInsideBoundariesDelegate));
				return;
			case 527:
				ScriptingInterfaceOfIMBMission.call_IsPositionInsideHardBoundariesDelegate = (ScriptingInterfaceOfIMBMission.IsPositionInsideHardBoundariesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsPositionInsideHardBoundariesDelegate));
				return;
			case 528:
				ScriptingInterfaceOfIMBMission.call_IsPositionOnAnyBlockerNavMeshFaceDelegate = (ScriptingInterfaceOfIMBMission.IsPositionOnAnyBlockerNavMeshFaceDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.IsPositionOnAnyBlockerNavMeshFaceDelegate));
				return;
			case 529:
				ScriptingInterfaceOfIMBMission.call_MakeSoundDelegate = (ScriptingInterfaceOfIMBMission.MakeSoundDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.MakeSoundDelegate));
				return;
			case 530:
				ScriptingInterfaceOfIMBMission.call_MakeSoundOnlyOnRelatedPeerDelegate = (ScriptingInterfaceOfIMBMission.MakeSoundOnlyOnRelatedPeerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.MakeSoundOnlyOnRelatedPeerDelegate));
				return;
			case 531:
				ScriptingInterfaceOfIMBMission.call_MakeSoundWithParameterDelegate = (ScriptingInterfaceOfIMBMission.MakeSoundWithParameterDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.MakeSoundWithParameterDelegate));
				return;
			case 532:
				ScriptingInterfaceOfIMBMission.call_OnFastForwardStateChangedDelegate = (ScriptingInterfaceOfIMBMission.OnFastForwardStateChangedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.OnFastForwardStateChangedDelegate));
				return;
			case 533:
				ScriptingInterfaceOfIMBMission.call_PauseMissionSceneSoundsDelegate = (ScriptingInterfaceOfIMBMission.PauseMissionSceneSoundsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.PauseMissionSceneSoundsDelegate));
				return;
			case 534:
				ScriptingInterfaceOfIMBMission.call_PrepareMissileWeaponForDropDelegate = (ScriptingInterfaceOfIMBMission.PrepareMissileWeaponForDropDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.PrepareMissileWeaponForDropDelegate));
				return;
			case 535:
				ScriptingInterfaceOfIMBMission.call_ProcessRecordUntilTimeDelegate = (ScriptingInterfaceOfIMBMission.ProcessRecordUntilTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ProcessRecordUntilTimeDelegate));
				return;
			case 536:
				ScriptingInterfaceOfIMBMission.call_ProximityMapBeginSearchDelegate = (ScriptingInterfaceOfIMBMission.ProximityMapBeginSearchDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ProximityMapBeginSearchDelegate));
				return;
			case 537:
				ScriptingInterfaceOfIMBMission.call_ProximityMapFindNextDelegate = (ScriptingInterfaceOfIMBMission.ProximityMapFindNextDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ProximityMapFindNextDelegate));
				return;
			case 538:
				ScriptingInterfaceOfIMBMission.call_ProximityMapMaxSearchRadiusDelegate = (ScriptingInterfaceOfIMBMission.ProximityMapMaxSearchRadiusDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ProximityMapMaxSearchRadiusDelegate));
				return;
			case 539:
				ScriptingInterfaceOfIMBMission.call_RayCastForClosestAgentDelegate = (ScriptingInterfaceOfIMBMission.RayCastForClosestAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RayCastForClosestAgentDelegate));
				return;
			case 540:
				ScriptingInterfaceOfIMBMission.call_RayCastForClosestAgentsLimbsDelegate = (ScriptingInterfaceOfIMBMission.RayCastForClosestAgentsLimbsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RayCastForClosestAgentsLimbsDelegate));
				return;
			case 541:
				ScriptingInterfaceOfIMBMission.call_RayCastForGivenAgentsLimbsDelegate = (ScriptingInterfaceOfIMBMission.RayCastForGivenAgentsLimbsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RayCastForGivenAgentsLimbsDelegate));
				return;
			case 542:
				ScriptingInterfaceOfIMBMission.call_RecordCurrentStateDelegate = (ScriptingInterfaceOfIMBMission.RecordCurrentStateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RecordCurrentStateDelegate));
				return;
			case 543:
				ScriptingInterfaceOfIMBMission.call_RemoveBoundaryDelegate = (ScriptingInterfaceOfIMBMission.RemoveBoundaryDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RemoveBoundaryDelegate));
				return;
			case 544:
				ScriptingInterfaceOfIMBMission.call_RemoveMissileDelegate = (ScriptingInterfaceOfIMBMission.RemoveMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RemoveMissileDelegate));
				return;
			case 545:
				ScriptingInterfaceOfIMBMission.call_ResetFirstThirdPersonViewDelegate = (ScriptingInterfaceOfIMBMission.ResetFirstThirdPersonViewDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ResetFirstThirdPersonViewDelegate));
				return;
			case 546:
				ScriptingInterfaceOfIMBMission.call_ResetTeamsDelegate = (ScriptingInterfaceOfIMBMission.ResetTeamsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ResetTeamsDelegate));
				return;
			case 547:
				ScriptingInterfaceOfIMBMission.call_RestartRecordDelegate = (ScriptingInterfaceOfIMBMission.RestartRecordDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RestartRecordDelegate));
				return;
			case 548:
				ScriptingInterfaceOfIMBMission.call_RestoreRecordFromFileDelegate = (ScriptingInterfaceOfIMBMission.RestoreRecordFromFileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.RestoreRecordFromFileDelegate));
				return;
			case 549:
				ScriptingInterfaceOfIMBMission.call_ResumeMissionSceneSoundsDelegate = (ScriptingInterfaceOfIMBMission.ResumeMissionSceneSoundsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.ResumeMissionSceneSoundsDelegate));
				return;
			case 550:
				ScriptingInterfaceOfIMBMission.call_SetBowMissileSpeedModifierDelegate = (ScriptingInterfaceOfIMBMission.SetBowMissileSpeedModifierDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetBowMissileSpeedModifierDelegate));
				return;
			case 551:
				ScriptingInterfaceOfIMBMission.call_SetCameraFrameDelegate = (ScriptingInterfaceOfIMBMission.SetCameraFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetCameraFrameDelegate));
				return;
			case 552:
				ScriptingInterfaceOfIMBMission.call_SetCameraIsFirstPersonDelegate = (ScriptingInterfaceOfIMBMission.SetCameraIsFirstPersonDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetCameraIsFirstPersonDelegate));
				return;
			case 553:
				ScriptingInterfaceOfIMBMission.call_SetCloseProximityWaveSoundsEnabledDelegate = (ScriptingInterfaceOfIMBMission.SetCloseProximityWaveSoundsEnabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetCloseProximityWaveSoundsEnabledDelegate));
				return;
			case 554:
				ScriptingInterfaceOfIMBMission.call_SetCombatTypeDelegate = (ScriptingInterfaceOfIMBMission.SetCombatTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetCombatTypeDelegate));
				return;
			case 555:
				ScriptingInterfaceOfIMBMission.call_SetCrossbowMissileSpeedModifierDelegate = (ScriptingInterfaceOfIMBMission.SetCrossbowMissileSpeedModifierDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetCrossbowMissileSpeedModifierDelegate));
				return;
			case 556:
				ScriptingInterfaceOfIMBMission.call_SetDebugAgentDelegate = (ScriptingInterfaceOfIMBMission.SetDebugAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetDebugAgentDelegate));
				return;
			case 557:
				ScriptingInterfaceOfIMBMission.call_SetFallAvoidSystemActiveDelegate = (ScriptingInterfaceOfIMBMission.SetFallAvoidSystemActiveDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetFallAvoidSystemActiveDelegate));
				return;
			case 558:
				ScriptingInterfaceOfIMBMission.call_SetLastMovementKeyPressedDelegate = (ScriptingInterfaceOfIMBMission.SetLastMovementKeyPressedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetLastMovementKeyPressedDelegate));
				return;
			case 559:
				ScriptingInterfaceOfIMBMission.call_SetMissileRangeModifierDelegate = (ScriptingInterfaceOfIMBMission.SetMissileRangeModifierDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetMissileRangeModifierDelegate));
				return;
			case 560:
				ScriptingInterfaceOfIMBMission.call_SetMissionCorpseFadeOutTimeInSecondsDelegate = (ScriptingInterfaceOfIMBMission.SetMissionCorpseFadeOutTimeInSecondsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetMissionCorpseFadeOutTimeInSecondsDelegate));
				return;
			case 561:
				ScriptingInterfaceOfIMBMission.call_SetNavigationFaceCostWithIdAroundPositionDelegate = (ScriptingInterfaceOfIMBMission.SetNavigationFaceCostWithIdAroundPositionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetNavigationFaceCostWithIdAroundPositionDelegate));
				return;
			case 562:
				ScriptingInterfaceOfIMBMission.call_SetOverrideCorpseCountDelegate = (ScriptingInterfaceOfIMBMission.SetOverrideCorpseCountDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetOverrideCorpseCountDelegate));
				return;
			case 563:
				ScriptingInterfaceOfIMBMission.call_SetPauseAITickDelegate = (ScriptingInterfaceOfIMBMission.SetPauseAITickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetPauseAITickDelegate));
				return;
			case 564:
				ScriptingInterfaceOfIMBMission.call_SetRandomDecideTimeOfAgentsDelegate = (ScriptingInterfaceOfIMBMission.SetRandomDecideTimeOfAgentsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetRandomDecideTimeOfAgentsDelegate));
				return;
			case 565:
				ScriptingInterfaceOfIMBMission.call_SetRenderParallelLogicInProgressDelegate = (ScriptingInterfaceOfIMBMission.SetRenderParallelLogicInProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetRenderParallelLogicInProgressDelegate));
				return;
			case 566:
				ScriptingInterfaceOfIMBMission.call_SetReportStuckAgentsModeDelegate = (ScriptingInterfaceOfIMBMission.SetReportStuckAgentsModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetReportStuckAgentsModeDelegate));
				return;
			case 567:
				ScriptingInterfaceOfIMBMission.call_SetThrowingMissileSpeedModifierDelegate = (ScriptingInterfaceOfIMBMission.SetThrowingMissileSpeedModifierDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetThrowingMissileSpeedModifierDelegate));
				return;
			case 568:
				ScriptingInterfaceOfIMBMission.call_SetVelocityOfMissileDelegate = (ScriptingInterfaceOfIMBMission.SetVelocityOfMissileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SetVelocityOfMissileDelegate));
				return;
			case 569:
				ScriptingInterfaceOfIMBMission.call_SkipForwardMissionReplayDelegate = (ScriptingInterfaceOfIMBMission.SkipForwardMissionReplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.SkipForwardMissionReplayDelegate));
				return;
			case 570:
				ScriptingInterfaceOfIMBMission.call_StartRecordingDelegate = (ScriptingInterfaceOfIMBMission.StartRecordingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.StartRecordingDelegate));
				return;
			case 571:
				ScriptingInterfaceOfIMBMission.call_TickDelegate = (ScriptingInterfaceOfIMBMission.TickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.TickDelegate));
				return;
			case 572:
				ScriptingInterfaceOfIMBMission.call_TickAgentsAndTeamsAsyncDelegate = (ScriptingInterfaceOfIMBMission.TickAgentsAndTeamsAsyncDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBMission.TickAgentsAndTeamsAsyncDelegate));
				return;
			case 573:
				ScriptingInterfaceOfIMBNetwork.call_AddNewBotOnServerDelegate = (ScriptingInterfaceOfIMBNetwork.AddNewBotOnServerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.AddNewBotOnServerDelegate));
				return;
			case 574:
				ScriptingInterfaceOfIMBNetwork.call_AddNewPlayerOnServerDelegate = (ScriptingInterfaceOfIMBNetwork.AddNewPlayerOnServerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.AddNewPlayerOnServerDelegate));
				return;
			case 575:
				ScriptingInterfaceOfIMBNetwork.call_AddPeerToDisconnectDelegate = (ScriptingInterfaceOfIMBNetwork.AddPeerToDisconnectDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.AddPeerToDisconnectDelegate));
				return;
			case 576:
				ScriptingInterfaceOfIMBNetwork.call_BeginBroadcastModuleEventDelegate = (ScriptingInterfaceOfIMBNetwork.BeginBroadcastModuleEventDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.BeginBroadcastModuleEventDelegate));
				return;
			case 577:
				ScriptingInterfaceOfIMBNetwork.call_BeginModuleEventAsClientDelegate = (ScriptingInterfaceOfIMBNetwork.BeginModuleEventAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.BeginModuleEventAsClientDelegate));
				return;
			case 578:
				ScriptingInterfaceOfIMBNetwork.call_CanAddNewPlayersOnServerDelegate = (ScriptingInterfaceOfIMBNetwork.CanAddNewPlayersOnServerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.CanAddNewPlayersOnServerDelegate));
				return;
			case 579:
				ScriptingInterfaceOfIMBNetwork.call_ClearReplicationTableStatisticsDelegate = (ScriptingInterfaceOfIMBNetwork.ClearReplicationTableStatisticsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ClearReplicationTableStatisticsDelegate));
				return;
			case 580:
				ScriptingInterfaceOfIMBNetwork.call_ElapsedTimeSinceLastUdpPacketArrivedDelegate = (ScriptingInterfaceOfIMBNetwork.ElapsedTimeSinceLastUdpPacketArrivedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ElapsedTimeSinceLastUdpPacketArrivedDelegate));
				return;
			case 581:
				ScriptingInterfaceOfIMBNetwork.call_EndBroadcastModuleEventDelegate = (ScriptingInterfaceOfIMBNetwork.EndBroadcastModuleEventDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.EndBroadcastModuleEventDelegate));
				return;
			case 582:
				ScriptingInterfaceOfIMBNetwork.call_EndModuleEventAsClientDelegate = (ScriptingInterfaceOfIMBNetwork.EndModuleEventAsClientDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.EndModuleEventAsClientDelegate));
				return;
			case 583:
				ScriptingInterfaceOfIMBNetwork.call_GetActiveUdpSessionsIpAddressDelegate = (ScriptingInterfaceOfIMBNetwork.GetActiveUdpSessionsIpAddressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.GetActiveUdpSessionsIpAddressDelegate));
				return;
			case 584:
				ScriptingInterfaceOfIMBNetwork.call_GetAveragePacketLossRatioDelegate = (ScriptingInterfaceOfIMBNetwork.GetAveragePacketLossRatioDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.GetAveragePacketLossRatioDelegate));
				return;
			case 585:
				ScriptingInterfaceOfIMBNetwork.call_GetDebugUploadsInBitsDelegate = (ScriptingInterfaceOfIMBNetwork.GetDebugUploadsInBitsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.GetDebugUploadsInBitsDelegate));
				return;
			case 586:
				ScriptingInterfaceOfIMBNetwork.call_GetMultiplayerDisabledDelegate = (ScriptingInterfaceOfIMBNetwork.GetMultiplayerDisabledDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.GetMultiplayerDisabledDelegate));
				return;
			case 587:
				ScriptingInterfaceOfIMBNetwork.call_InitializeClientSideDelegate = (ScriptingInterfaceOfIMBNetwork.InitializeClientSideDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.InitializeClientSideDelegate));
				return;
			case 588:
				ScriptingInterfaceOfIMBNetwork.call_InitializeServerSideDelegate = (ScriptingInterfaceOfIMBNetwork.InitializeServerSideDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.InitializeServerSideDelegate));
				return;
			case 589:
				ScriptingInterfaceOfIMBNetwork.call_IsDedicatedServerDelegate = (ScriptingInterfaceOfIMBNetwork.IsDedicatedServerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.IsDedicatedServerDelegate));
				return;
			case 590:
				ScriptingInterfaceOfIMBNetwork.call_PrepareNewUdpSessionDelegate = (ScriptingInterfaceOfIMBNetwork.PrepareNewUdpSessionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.PrepareNewUdpSessionDelegate));
				return;
			case 591:
				ScriptingInterfaceOfIMBNetwork.call_PrintDebugStatsDelegate = (ScriptingInterfaceOfIMBNetwork.PrintDebugStatsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.PrintDebugStatsDelegate));
				return;
			case 592:
				ScriptingInterfaceOfIMBNetwork.call_PrintReplicationTableStatisticsDelegate = (ScriptingInterfaceOfIMBNetwork.PrintReplicationTableStatisticsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.PrintReplicationTableStatisticsDelegate));
				return;
			case 593:
				ScriptingInterfaceOfIMBNetwork.call_ReadByteArrayFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadByteArrayFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadByteArrayFromPacketDelegate));
				return;
			case 594:
				ScriptingInterfaceOfIMBNetwork.call_ReadFloatFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadFloatFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadFloatFromPacketDelegate));
				return;
			case 595:
				ScriptingInterfaceOfIMBNetwork.call_ReadIntFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadIntFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadIntFromPacketDelegate));
				return;
			case 596:
				ScriptingInterfaceOfIMBNetwork.call_ReadLongFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadLongFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadLongFromPacketDelegate));
				return;
			case 597:
				ScriptingInterfaceOfIMBNetwork.call_ReadStringFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadStringFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadStringFromPacketDelegate));
				return;
			case 598:
				ScriptingInterfaceOfIMBNetwork.call_ReadUintFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadUintFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadUintFromPacketDelegate));
				return;
			case 599:
				ScriptingInterfaceOfIMBNetwork.call_ReadUlongFromPacketDelegate = (ScriptingInterfaceOfIMBNetwork.ReadUlongFromPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ReadUlongFromPacketDelegate));
				return;
			case 600:
				ScriptingInterfaceOfIMBNetwork.call_RemoveBotOnServerDelegate = (ScriptingInterfaceOfIMBNetwork.RemoveBotOnServerDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.RemoveBotOnServerDelegate));
				return;
			case 601:
				ScriptingInterfaceOfIMBNetwork.call_ResetDebugUploadsDelegate = (ScriptingInterfaceOfIMBNetwork.ResetDebugUploadsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ResetDebugUploadsDelegate));
				return;
			case 602:
				ScriptingInterfaceOfIMBNetwork.call_ResetDebugVariablesDelegate = (ScriptingInterfaceOfIMBNetwork.ResetDebugVariablesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ResetDebugVariablesDelegate));
				return;
			case 603:
				ScriptingInterfaceOfIMBNetwork.call_ResetMissionDataDelegate = (ScriptingInterfaceOfIMBNetwork.ResetMissionDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ResetMissionDataDelegate));
				return;
			case 604:
				ScriptingInterfaceOfIMBNetwork.call_ServerPingDelegate = (ScriptingInterfaceOfIMBNetwork.ServerPingDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.ServerPingDelegate));
				return;
			case 605:
				ScriptingInterfaceOfIMBNetwork.call_SetServerBandwidthLimitInMbpsDelegate = (ScriptingInterfaceOfIMBNetwork.SetServerBandwidthLimitInMbpsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.SetServerBandwidthLimitInMbpsDelegate));
				return;
			case 606:
				ScriptingInterfaceOfIMBNetwork.call_SetServerFrameRateDelegate = (ScriptingInterfaceOfIMBNetwork.SetServerFrameRateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.SetServerFrameRateDelegate));
				return;
			case 607:
				ScriptingInterfaceOfIMBNetwork.call_SetServerTickRateDelegate = (ScriptingInterfaceOfIMBNetwork.SetServerTickRateDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.SetServerTickRateDelegate));
				return;
			case 608:
				ScriptingInterfaceOfIMBNetwork.call_TerminateClientSideDelegate = (ScriptingInterfaceOfIMBNetwork.TerminateClientSideDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.TerminateClientSideDelegate));
				return;
			case 609:
				ScriptingInterfaceOfIMBNetwork.call_TerminateServerSideDelegate = (ScriptingInterfaceOfIMBNetwork.TerminateServerSideDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.TerminateServerSideDelegate));
				return;
			case 610:
				ScriptingInterfaceOfIMBNetwork.call_WriteByteArrayToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteByteArrayToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteByteArrayToPacketDelegate));
				return;
			case 611:
				ScriptingInterfaceOfIMBNetwork.call_WriteFloatToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteFloatToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteFloatToPacketDelegate));
				return;
			case 612:
				ScriptingInterfaceOfIMBNetwork.call_WriteIntToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteIntToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteIntToPacketDelegate));
				return;
			case 613:
				ScriptingInterfaceOfIMBNetwork.call_WriteLongToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteLongToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteLongToPacketDelegate));
				return;
			case 614:
				ScriptingInterfaceOfIMBNetwork.call_WriteStringToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteStringToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteStringToPacketDelegate));
				return;
			case 615:
				ScriptingInterfaceOfIMBNetwork.call_WriteUintToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteUintToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteUintToPacketDelegate));
				return;
			case 616:
				ScriptingInterfaceOfIMBNetwork.call_WriteUlongToPacketDelegate = (ScriptingInterfaceOfIMBNetwork.WriteUlongToPacketDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBNetwork.WriteUlongToPacketDelegate));
				return;
			case 617:
				ScriptingInterfaceOfIMBPeer.call_BeginModuleEventDelegate = (ScriptingInterfaceOfIMBPeer.BeginModuleEventDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.BeginModuleEventDelegate));
				return;
			case 618:
				ScriptingInterfaceOfIMBPeer.call_DebugRefreshDisconnectTimeoutDelegate = (ScriptingInterfaceOfIMBPeer.DebugRefreshDisconnectTimeoutDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.DebugRefreshDisconnectTimeoutDelegate));
				return;
			case 619:
				ScriptingInterfaceOfIMBPeer.call_EndModuleEventDelegate = (ScriptingInterfaceOfIMBPeer.EndModuleEventDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.EndModuleEventDelegate));
				return;
			case 620:
				ScriptingInterfaceOfIMBPeer.call_GetAverageLossPercentDelegate = (ScriptingInterfaceOfIMBPeer.GetAverageLossPercentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetAverageLossPercentDelegate));
				return;
			case 621:
				ScriptingInterfaceOfIMBPeer.call_GetAveragePingInMillisecondsDelegate = (ScriptingInterfaceOfIMBPeer.GetAveragePingInMillisecondsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetAveragePingInMillisecondsDelegate));
				return;
			case 622:
				ScriptingInterfaceOfIMBPeer.call_GetHostDelegate = (ScriptingInterfaceOfIMBPeer.GetHostDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetHostDelegate));
				return;
			case 623:
				ScriptingInterfaceOfIMBPeer.call_GetIsSynchronizedDelegate = (ScriptingInterfaceOfIMBPeer.GetIsSynchronizedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetIsSynchronizedDelegate));
				return;
			case 624:
				ScriptingInterfaceOfIMBPeer.call_GetPortDelegate = (ScriptingInterfaceOfIMBPeer.GetPortDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetPortDelegate));
				return;
			case 625:
				ScriptingInterfaceOfIMBPeer.call_GetReversedHostDelegate = (ScriptingInterfaceOfIMBPeer.GetReversedHostDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.GetReversedHostDelegate));
				return;
			case 626:
				ScriptingInterfaceOfIMBPeer.call_IsActiveDelegate = (ScriptingInterfaceOfIMBPeer.IsActiveDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.IsActiveDelegate));
				return;
			case 627:
				ScriptingInterfaceOfIMBPeer.call_SendExistingObjectsDelegate = (ScriptingInterfaceOfIMBPeer.SendExistingObjectsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SendExistingObjectsDelegate));
				return;
			case 628:
				ScriptingInterfaceOfIMBPeer.call_SetControlledAgentDelegate = (ScriptingInterfaceOfIMBPeer.SetControlledAgentDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SetControlledAgentDelegate));
				return;
			case 629:
				ScriptingInterfaceOfIMBPeer.call_SetIsSynchronizedDelegate = (ScriptingInterfaceOfIMBPeer.SetIsSynchronizedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SetIsSynchronizedDelegate));
				return;
			case 630:
				ScriptingInterfaceOfIMBPeer.call_SetRelevantGameOptionsDelegate = (ScriptingInterfaceOfIMBPeer.SetRelevantGameOptionsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SetRelevantGameOptionsDelegate));
				return;
			case 631:
				ScriptingInterfaceOfIMBPeer.call_SetTeamDelegate = (ScriptingInterfaceOfIMBPeer.SetTeamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SetTeamDelegate));
				return;
			case 632:
				ScriptingInterfaceOfIMBPeer.call_SetUserDataDelegate = (ScriptingInterfaceOfIMBPeer.SetUserDataDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBPeer.SetUserDataDelegate));
				return;
			case 633:
				ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterPressDelegate = (ScriptingInterfaceOfIMBScreen.OnEditModeEnterPressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBScreen.OnEditModeEnterPressDelegate));
				return;
			case 634:
				ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterReleaseDelegate = (ScriptingInterfaceOfIMBScreen.OnEditModeEnterReleaseDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBScreen.OnEditModeEnterReleaseDelegate));
				return;
			case 635:
				ScriptingInterfaceOfIMBScreen.call_OnExitButtonClickDelegate = (ScriptingInterfaceOfIMBScreen.OnExitButtonClickDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBScreen.OnExitButtonClickDelegate));
				return;
			case 636:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateAgentSkeletonDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.CreateAgentSkeletonDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.CreateAgentSkeletonDelegate));
				return;
			case 637:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateSimpleSkeletonDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.CreateSimpleSkeletonDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.CreateSimpleSkeletonDelegate));
				return;
			case 638:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateWithActionSetDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.CreateWithActionSetDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.CreateWithActionSetDelegate));
				return;
			case 639:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_DoesActionContinueWithCurrentActionAtChannelDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.DoesActionContinueWithCurrentActionAtChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.DoesActionContinueWithCurrentActionAtChannelDelegate));
				return;
			case 640:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_GetActionAtChannelDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.GetActionAtChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.GetActionAtChannelDelegate));
				return;
			case 641:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameDelegate));
				return;
			case 642:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameAtAnimationProgressDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameAtAnimationProgressDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameAtAnimationProgressDelegate));
				return;
			case 643:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationNameDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationNameDelegate));
				return;
			case 644:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationTimeDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationTimeDelegate));
				return;
			case 645:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAgentActionChannelDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.SetAgentActionChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.SetAgentActionChannelDelegate));
				return;
			case 646:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAnimationAtChannelDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.SetAnimationAtChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.SetAnimationAtChannelDelegate));
				return;
			case 647:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_SetFacialAnimationOfChannelDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.SetFacialAnimationOfChannelDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.SetFacialAnimationOfChannelDelegate));
				return;
			case 648:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_SetSkeletonFaceAnimationTimeDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.SetSkeletonFaceAnimationTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.SetSkeletonFaceAnimationTimeDelegate));
				return;
			case 649:
				ScriptingInterfaceOfIMBSkeletonExtensions.call_TickActionChannelsDelegate = (ScriptingInterfaceOfIMBSkeletonExtensions.TickActionChannelsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSkeletonExtensions.TickActionChannelsDelegate));
				return;
			case 650:
				ScriptingInterfaceOfIMBSoundEvent.call_CreateEventFromExternalFileDelegate = (ScriptingInterfaceOfIMBSoundEvent.CreateEventFromExternalFileDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.CreateEventFromExternalFileDelegate));
				return;
			case 651:
				ScriptingInterfaceOfIMBSoundEvent.call_CreateEventFromSoundBufferDelegate = (ScriptingInterfaceOfIMBSoundEvent.CreateEventFromSoundBufferDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.CreateEventFromSoundBufferDelegate));
				return;
			case 652:
				ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundDelegate = (ScriptingInterfaceOfIMBSoundEvent.PlaySoundDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.PlaySoundDelegate));
				return;
			case 653:
				ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithIntParamDelegate = (ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithIntParamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithIntParamDelegate));
				return;
			case 654:
				ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithParamDelegate = (ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithParamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithParamDelegate));
				return;
			case 655:
				ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithStrParamDelegate = (ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithStrParamDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithStrParamDelegate));
				return;
			case 656:
				ScriptingInterfaceOfIMBTeam.call_IsEnemyDelegate = (ScriptingInterfaceOfIMBTeam.IsEnemyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTeam.IsEnemyDelegate));
				return;
			case 657:
				ScriptingInterfaceOfIMBTeam.call_SetIsEnemyDelegate = (ScriptingInterfaceOfIMBTeam.SetIsEnemyDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTeam.SetIsEnemyDelegate));
				return;
			case 658:
				ScriptingInterfaceOfIMBTestRun.call_AutoContinueDelegate = (ScriptingInterfaceOfIMBTestRun.AutoContinueDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.AutoContinueDelegate));
				return;
			case 659:
				ScriptingInterfaceOfIMBTestRun.call_CloseSceneDelegate = (ScriptingInterfaceOfIMBTestRun.CloseSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.CloseSceneDelegate));
				return;
			case 660:
				ScriptingInterfaceOfIMBTestRun.call_EnterEditModeDelegate = (ScriptingInterfaceOfIMBTestRun.EnterEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.EnterEditModeDelegate));
				return;
			case 661:
				ScriptingInterfaceOfIMBTestRun.call_GetFPSDelegate = (ScriptingInterfaceOfIMBTestRun.GetFPSDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.GetFPSDelegate));
				return;
			case 662:
				ScriptingInterfaceOfIMBTestRun.call_LeaveEditModeDelegate = (ScriptingInterfaceOfIMBTestRun.LeaveEditModeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.LeaveEditModeDelegate));
				return;
			case 663:
				ScriptingInterfaceOfIMBTestRun.call_NewSceneDelegate = (ScriptingInterfaceOfIMBTestRun.NewSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.NewSceneDelegate));
				return;
			case 664:
				ScriptingInterfaceOfIMBTestRun.call_OpenDefaultSceneDelegate = (ScriptingInterfaceOfIMBTestRun.OpenDefaultSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.OpenDefaultSceneDelegate));
				return;
			case 665:
				ScriptingInterfaceOfIMBTestRun.call_OpenSceneDelegate = (ScriptingInterfaceOfIMBTestRun.OpenSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.OpenSceneDelegate));
				return;
			case 666:
				ScriptingInterfaceOfIMBTestRun.call_SaveSceneDelegate = (ScriptingInterfaceOfIMBTestRun.SaveSceneDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.SaveSceneDelegate));
				return;
			case 667:
				ScriptingInterfaceOfIMBTestRun.call_StartMissionDelegate = (ScriptingInterfaceOfIMBTestRun.StartMissionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBTestRun.StartMissionDelegate));
				return;
			case 668:
				ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate = (ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate));
				return;
			case 669:
				ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate = (ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate));
				return;
			case 670:
				ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceTypeIndexDelegate = (ScriptingInterfaceOfIMBVoiceManager.GetVoiceTypeIndexDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBVoiceManager.GetVoiceTypeIndexDelegate));
				return;
			case 671:
				ScriptingInterfaceOfIMBWindowManager.call_DontChangeCursorPosDelegate = (ScriptingInterfaceOfIMBWindowManager.DontChangeCursorPosDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.DontChangeCursorPosDelegate));
				return;
			case 672:
				ScriptingInterfaceOfIMBWindowManager.call_EraseMessageLinesDelegate = (ScriptingInterfaceOfIMBWindowManager.EraseMessageLinesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.EraseMessageLinesDelegate));
				return;
			case 673:
				ScriptingInterfaceOfIMBWindowManager.call_GetScreenResolutionDelegate = (ScriptingInterfaceOfIMBWindowManager.GetScreenResolutionDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.GetScreenResolutionDelegate));
				return;
			case 674:
				ScriptingInterfaceOfIMBWindowManager.call_PreDisplayDelegate = (ScriptingInterfaceOfIMBWindowManager.PreDisplayDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.PreDisplayDelegate));
				return;
			case 675:
				ScriptingInterfaceOfIMBWindowManager.call_ScreenToWorldDelegate = (ScriptingInterfaceOfIMBWindowManager.ScreenToWorldDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.ScreenToWorldDelegate));
				return;
			case 676:
				ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenDelegate = (ScriptingInterfaceOfIMBWindowManager.WorldToScreenDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.WorldToScreenDelegate));
				return;
			case 677:
				ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenWithFixedZDelegate = (ScriptingInterfaceOfIMBWindowManager.WorldToScreenWithFixedZDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWindowManager.WorldToScreenWithFixedZDelegate));
				return;
			case 678:
				ScriptingInterfaceOfIMBWorld.call_CheckResourceModificationsDelegate = (ScriptingInterfaceOfIMBWorld.CheckResourceModificationsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.CheckResourceModificationsDelegate));
				return;
			case 679:
				ScriptingInterfaceOfIMBWorld.call_FixSkeletonsDelegate = (ScriptingInterfaceOfIMBWorld.FixSkeletonsDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.FixSkeletonsDelegate));
				return;
			case 680:
				ScriptingInterfaceOfIMBWorld.call_GetGameTypeDelegate = (ScriptingInterfaceOfIMBWorld.GetGameTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.GetGameTypeDelegate));
				return;
			case 681:
				ScriptingInterfaceOfIMBWorld.call_GetGlobalTimeDelegate = (ScriptingInterfaceOfIMBWorld.GetGlobalTimeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.GetGlobalTimeDelegate));
				return;
			case 682:
				ScriptingInterfaceOfIMBWorld.call_GetLastMessagesDelegate = (ScriptingInterfaceOfIMBWorld.GetLastMessagesDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.GetLastMessagesDelegate));
				return;
			case 683:
				ScriptingInterfaceOfIMBWorld.call_PauseGameDelegate = (ScriptingInterfaceOfIMBWorld.PauseGameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.PauseGameDelegate));
				return;
			case 684:
				ScriptingInterfaceOfIMBWorld.call_SetBodyUsedDelegate = (ScriptingInterfaceOfIMBWorld.SetBodyUsedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.SetBodyUsedDelegate));
				return;
			case 685:
				ScriptingInterfaceOfIMBWorld.call_SetGameTypeDelegate = (ScriptingInterfaceOfIMBWorld.SetGameTypeDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.SetGameTypeDelegate));
				return;
			case 686:
				ScriptingInterfaceOfIMBWorld.call_SetMaterialUsedDelegate = (ScriptingInterfaceOfIMBWorld.SetMaterialUsedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.SetMaterialUsedDelegate));
				return;
			case 687:
				ScriptingInterfaceOfIMBWorld.call_SetMeshUsedDelegate = (ScriptingInterfaceOfIMBWorld.SetMeshUsedDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.SetMeshUsedDelegate));
				return;
			case 688:
				ScriptingInterfaceOfIMBWorld.call_UnpauseGameDelegate = (ScriptingInterfaceOfIMBWorld.UnpauseGameDelegate)Marshal.GetDelegateForFunctionPointer(pointer, typeof(ScriptingInterfaceOfIMBWorld.UnpauseGameDelegate));
				break;
			default:
				return;
			}
		}

		// Token: 0x02000094 RID: 148
		private enum CoreInterfaceGeneratedEnum
		{
			// Token: 0x040002F1 RID: 753
			enm_IMono_MBActionSet_are_actions_alternatives,
			// Token: 0x040002F2 RID: 754
			enm_IMono_MBActionSet_get_animation_name,
			// Token: 0x040002F3 RID: 755
			enm_IMono_MBActionSet_get_bone_has_parent_bone,
			// Token: 0x040002F4 RID: 756
			enm_IMono_MBActionSet_get_bone_index_with_id,
			// Token: 0x040002F5 RID: 757
			enm_IMono_MBActionSet_get_index_with_id,
			// Token: 0x040002F6 RID: 758
			enm_IMono_MBActionSet_get_name_with_index,
			// Token: 0x040002F7 RID: 759
			enm_IMono_MBActionSet_get_number_of_action_sets,
			// Token: 0x040002F8 RID: 760
			enm_IMono_MBActionSet_get_number_of_monster_usage_sets,
			// Token: 0x040002F9 RID: 761
			enm_IMono_MBActionSet_get_skeleton_name,
			// Token: 0x040002FA RID: 762
			enm_IMono_MBAgent_add_acceleration,
			// Token: 0x040002FB RID: 763
			enm_IMono_MBAgent_add_as_corpse,
			// Token: 0x040002FC RID: 764
			enm_IMono_MBAgent_add_mesh_to_bone,
			// Token: 0x040002FD RID: 765
			enm_IMono_MBAgent_add_prefab_to_agent_bone,
			// Token: 0x040002FE RID: 766
			enm_IMono_MBAgent_apply_force_on_ragdoll,
			// Token: 0x040002FF RID: 767
			enm_IMono_MBAgent_attach_weapon_to_bone,
			// Token: 0x04000300 RID: 768
			enm_IMono_MBAgent_attach_weapon_to_weapon_in_slot,
			// Token: 0x04000301 RID: 769
			enm_IMono_MBAgent_attack_direction_to_movement_flag,
			// Token: 0x04000302 RID: 770
			enm_IMono_MBAgent_build,
			// Token: 0x04000303 RID: 771
			enm_IMono_MBAgent_can_move_directly_to_position,
			// Token: 0x04000304 RID: 772
			enm_IMono_MBAgent_check_path_to_ai_target_agent_passes_through_navigation_face_id_from_direction,
			// Token: 0x04000305 RID: 773
			enm_IMono_MBAgent_clear_equipment,
			// Token: 0x04000306 RID: 774
			enm_IMono_MBAgent_clear_hand_inverse_kinematics,
			// Token: 0x04000307 RID: 775
			enm_IMono_MBAgent_clear_target_frame,
			// Token: 0x04000308 RID: 776
			enm_IMono_MBAgent_clear_target_z,
			// Token: 0x04000309 RID: 777
			enm_IMono_MBAgent_compute_animation_displacement,
			// Token: 0x0400030A RID: 778
			enm_IMono_MBAgent_create_blood_burst_at_limb,
			// Token: 0x0400030B RID: 779
			enm_IMono_MBAgent_debug_more,
			// Token: 0x0400030C RID: 780
			enm_IMono_MBAgent_defend_direction_to_movement_flag = 28,
			// Token: 0x0400030D RID: 781
			enm_IMono_MBAgent_delete_attached_weapon_from_bone,
			// Token: 0x0400030E RID: 782
			enm_IMono_MBAgent_die,
			// Token: 0x0400030F RID: 783
			enm_IMono_MBAgent_disable_look_to_point_of_interest,
			// Token: 0x04000310 RID: 784
			enm_IMono_MBAgent_disable_scripted_combat_movement,
			// Token: 0x04000311 RID: 785
			enm_IMono_MBAgent_disable_scripted_movement,
			// Token: 0x04000312 RID: 786
			enm_IMono_MBAgent_drop_item,
			// Token: 0x04000313 RID: 787
			enm_IMono_MBAgent_end_ragdoll_as_corpse,
			// Token: 0x04000314 RID: 788
			enm_IMono_MBAgent_enforce_shield_usage,
			// Token: 0x04000315 RID: 789
			enm_IMono_MBAgent_fade_in,
			// Token: 0x04000316 RID: 790
			enm_IMono_MBAgent_fade_out,
			// Token: 0x04000317 RID: 791
			enm_IMono_MBAgent_find_longest_direct_move_to_position,
			// Token: 0x04000318 RID: 792
			enm_IMono_MBAgent_force_ai_behavior_selection,
			// Token: 0x04000319 RID: 793
			enm_IMono_MBAgent_get_action_channel_current_action_weight,
			// Token: 0x0400031A RID: 794
			enm_IMono_MBAgent_get_action_channel_weight,
			// Token: 0x0400031B RID: 795
			enm_IMono_MBAgent_get_action_direction,
			// Token: 0x0400031C RID: 796
			enm_IMono_MBAgent_get_action_set_no,
			// Token: 0x0400031D RID: 797
			enm_IMono_MBAgent_get_agent_facial_animation,
			// Token: 0x0400031E RID: 798
			enm_IMono_MBAgent_get_agent_parent_entity,
			// Token: 0x0400031F RID: 799
			enm_IMono_MBAgent_get_agent_scale,
			// Token: 0x04000320 RID: 800
			enm_IMono_MBAgent_get_agent_visuals,
			// Token: 0x04000321 RID: 801
			enm_IMono_MBAgent_get_agent_voice_definiton,
			// Token: 0x04000322 RID: 802
			enm_IMono_MBAgent_get_ai_last_suspicious_position,
			// Token: 0x04000323 RID: 803
			enm_IMono_MBAgent_get_aiming_timer,
			// Token: 0x04000324 RID: 804
			enm_IMono_MBAgent_get_ai_move_destination,
			// Token: 0x04000325 RID: 805
			enm_IMono_MBAgent_get_ai_move_stop_tolerance,
			// Token: 0x04000326 RID: 806
			enm_IMono_MBAgent_get_ai_state_flags,
			// Token: 0x04000327 RID: 807
			enm_IMono_MBAgent_get_attack_direction,
			// Token: 0x04000328 RID: 808
			enm_IMono_MBAgent_get_attack_direction_usage,
			// Token: 0x04000329 RID: 809
			enm_IMono_MBAgent_get_average_real_global_velocity,
			// Token: 0x0400032A RID: 810
			enm_IMono_MBAgent_get_average_velocity,
			// Token: 0x0400032B RID: 811
			enm_IMono_MBAgent_get_body_rotation_constraint,
			// Token: 0x0400032C RID: 812
			enm_IMono_MBAgent_get_quick_bone_entitial_frame,
			// Token: 0x0400032D RID: 813
			enm_IMono_MBAgent_get_bone_entitial_frame_at_animation_progress,
			// Token: 0x0400032E RID: 814
			enm_IMono_MBAgent_get_chest_global_position,
			// Token: 0x0400032F RID: 815
			enm_IMono_MBAgent_get_collision_capsule,
			// Token: 0x04000330 RID: 816
			enm_IMono_MBAgent_get_crouch_mode,
			// Token: 0x04000331 RID: 817
			enm_IMono_MBAgent_get_current_action_direction,
			// Token: 0x04000332 RID: 818
			enm_IMono_MBAgent_get_current_action_priority,
			// Token: 0x04000333 RID: 819
			enm_IMono_MBAgent_get_current_action_progress,
			// Token: 0x04000334 RID: 820
			enm_IMono_MBAgent_get_current_action_stage,
			// Token: 0x04000335 RID: 821
			enm_IMono_MBAgent_get_current_action_type,
			// Token: 0x04000336 RID: 822
			enm_IMono_MBAgent_get_current_aiming_error,
			// Token: 0x04000337 RID: 823
			enm_IMono_MBAgent_get_current_aiming_turbulance,
			// Token: 0x04000338 RID: 824
			enm_IMono_MBAgent_get_current_animation_flags,
			// Token: 0x04000339 RID: 825
			enm_IMono_MBAgent_get_current_guard_mode,
			// Token: 0x0400033A RID: 826
			enm_IMono_MBAgent_get_current_navigation_face_id,
			// Token: 0x0400033B RID: 827
			enm_IMono_MBAgent_get_current_speed_limit,
			// Token: 0x0400033C RID: 828
			enm_IMono_MBAgent_get_current_velocity,
			// Token: 0x0400033D RID: 829
			enm_IMono_MBAgent_get_cur_weapon_offset,
			// Token: 0x0400033E RID: 830
			enm_IMono_MBAgent_get_defend_movement_flag,
			// Token: 0x0400033F RID: 831
			enm_IMono_MBAgent_get_event_control_flags,
			// Token: 0x04000340 RID: 832
			enm_IMono_MBAgent_get_eye_global_height,
			// Token: 0x04000341 RID: 833
			enm_IMono_MBAgent_get_eye_global_position,
			// Token: 0x04000342 RID: 834
			enm_IMono_MBAgent_get_firing_order,
			// Token: 0x04000343 RID: 835
			enm_IMono_MBAgent_get_ground_material_for_collision_effect,
			// Token: 0x04000344 RID: 836
			enm_IMono_MBAgent_get_has_on_ai_input_set_callback,
			// Token: 0x04000345 RID: 837
			enm_IMono_MBAgent_get_head_camera_mode,
			// Token: 0x04000346 RID: 838
			enm_IMono_MBAgent_get_immediate_enemy,
			// Token: 0x04000347 RID: 839
			enm_IMono_MBAgent_get_is_doing_passive_attack,
			// Token: 0x04000348 RID: 840
			enm_IMono_MBAgent_get_is_left_stance,
			// Token: 0x04000349 RID: 841
			enm_IMono_MBAgent_get_is_look_direction_locked,
			// Token: 0x0400034A RID: 842
			enm_IMono_MBAgent_get_is_passive_usage_conditions_are_met,
			// Token: 0x0400034B RID: 843
			enm_IMono_MBAgent_get_last_target_visibility_state,
			// Token: 0x0400034C RID: 844
			enm_IMono_MBAgent_get_look_agent,
			// Token: 0x0400034D RID: 845
			enm_IMono_MBAgent_get_look_direction,
			// Token: 0x0400034E RID: 846
			enm_IMono_MBAgent_get_look_direction_as_angle,
			// Token: 0x0400034F RID: 847
			enm_IMono_MBAgent_get_look_down_limit,
			// Token: 0x04000350 RID: 848
			enm_IMono_MBAgent_get_maximum_number_of_agents,
			// Token: 0x04000351 RID: 849
			enm_IMono_MBAgent_get_maximum_speed_limit,
			// Token: 0x04000352 RID: 850
			enm_IMono_MBAgent_get_missile_range,
			// Token: 0x04000353 RID: 851
			enm_IMono_MBAgent_get_missile_range_with_height_difference,
			// Token: 0x04000354 RID: 852
			enm_IMono_MBAgent_get_monster_usage_index,
			// Token: 0x04000355 RID: 853
			enm_IMono_MBAgent_get_mount_agent,
			// Token: 0x04000356 RID: 854
			enm_IMono_MBAgent_get_movement_flags,
			// Token: 0x04000357 RID: 855
			enm_IMono_MBAgent_get_movement_input_vector,
			// Token: 0x04000358 RID: 856
			enm_IMono_MBAgent_get_movement_locked_state,
			// Token: 0x04000359 RID: 857
			enm_IMono_MBAgent_get_movement_velocity,
			// Token: 0x0400035A RID: 858
			enm_IMono_MBAgent_get_native_action_index,
			// Token: 0x0400035B RID: 859
			enm_IMono_MBAgent_get_old_wielded_item_info,
			// Token: 0x0400035C RID: 860
			enm_IMono_MBAgent_get_path_distance_to_point,
			// Token: 0x0400035D RID: 861
			enm_IMono_MBAgent_get_position,
			// Token: 0x0400035E RID: 862
			enm_IMono_MBAgent_get_real_global_velocity,
			// Token: 0x0400035F RID: 863
			enm_IMono_MBAgent_get_render_check_enabled,
			// Token: 0x04000360 RID: 864
			enm_IMono_MBAgent_get_retreat_pos,
			// Token: 0x04000361 RID: 865
			enm_IMono_MBAgent_get_rider_agent,
			// Token: 0x04000362 RID: 866
			enm_IMono_MBAgent_get_riding_order,
			// Token: 0x04000363 RID: 867
			enm_IMono_MBAgent_get_rotation_frame,
			// Token: 0x04000364 RID: 868
			enm_IMono_MBAgent_get_running_simulation_data_until_maximum_speed_reached,
			// Token: 0x04000365 RID: 869
			enm_IMono_MBAgent_get_scripted_combat_flags,
			// Token: 0x04000366 RID: 870
			enm_IMono_MBAgent_get_scripted_flags,
			// Token: 0x04000367 RID: 871
			enm_IMono_MBAgent_get_selected_mount_index,
			// Token: 0x04000368 RID: 872
			enm_IMono_MBAgent_get_state_flags,
			// Token: 0x04000369 RID: 873
			enm_IMono_MBAgent_get_stepped_body_flags,
			// Token: 0x0400036A RID: 874
			enm_IMono_MBAgent_get_stepped_entity_id,
			// Token: 0x0400036B RID: 875
			enm_IMono_MBAgent_get_stepped_root_entity_id,
			// Token: 0x0400036C RID: 876
			enm_IMono_MBAgent_get_target_agent,
			// Token: 0x0400036D RID: 877
			enm_IMono_MBAgent_get_target_direction,
			// Token: 0x0400036E RID: 878
			enm_IMono_MBAgent_get_target_formation_index,
			// Token: 0x0400036F RID: 879
			enm_IMono_MBAgent_get_target_position,
			// Token: 0x04000370 RID: 880
			enm_IMono_MBAgent_get_team,
			// Token: 0x04000371 RID: 881
			enm_IMono_MBAgent_get_total_mass,
			// Token: 0x04000372 RID: 882
			enm_IMono_MBAgent_get_turn_speed,
			// Token: 0x04000373 RID: 883
			enm_IMono_MBAgent_get_visual_position,
			// Token: 0x04000374 RID: 884
			enm_IMono_MBAgent_get_walk_mode,
			// Token: 0x04000375 RID: 885
			enm_IMono_MBAgent_get_walking_speed_limit_of_mountable,
			// Token: 0x04000376 RID: 886
			enm_IMono_MBAgent_get_weapon_entity_from_equipment_slot,
			// Token: 0x04000377 RID: 887
			enm_IMono_MBAgent_get_wielded_weapon_info,
			// Token: 0x04000378 RID: 888
			enm_IMono_MBAgent_get_world_position,
			// Token: 0x04000379 RID: 889
			enm_IMono_MBAgent_handle_blow_aux,
			// Token: 0x0400037A RID: 890
			enm_IMono_MBAgent_has_path_through_navigation_face_id_from_direction,
			// Token: 0x0400037B RID: 891
			enm_IMono_MBAgent_has_path_through_navigation_faces_id_from_direction,
			// Token: 0x0400037C RID: 892
			enm_IMono_MBAgent_initialize_agent_record,
			// Token: 0x0400037D RID: 893
			enm_IMono_MBAgent_invalidate_ai_weapon_selections,
			// Token: 0x0400037E RID: 894
			enm_IMono_MBAgent_invalidate_target_agent,
			// Token: 0x0400037F RID: 895
			enm_IMono_MBAgent_is_added_as_corpse,
			// Token: 0x04000380 RID: 896
			enm_IMono_MBAgent_is_crouching_allowed,
			// Token: 0x04000381 RID: 897
			enm_IMono_MBAgent_is_enemy,
			// Token: 0x04000382 RID: 898
			enm_IMono_MBAgent_is_fading_out,
			// Token: 0x04000383 RID: 899
			enm_IMono_MBAgent_is_friend,
			// Token: 0x04000384 RID: 900
			enm_IMono_MBAgent_is_look_rotation_in_slow_motion,
			// Token: 0x04000385 RID: 901
			enm_IMono_MBAgent_is_retreating,
			// Token: 0x04000386 RID: 902
			enm_IMono_MBAgent_is_running_away,
			// Token: 0x04000387 RID: 903
			enm_IMono_MBAgent_is_sliding,
			// Token: 0x04000388 RID: 904
			enm_IMono_MBAgent_is_target_navigation_face_id_between,
			// Token: 0x04000389 RID: 905
			enm_IMono_MBAgent_is_wandering,
			// Token: 0x0400038A RID: 906
			enm_IMono_MBAgent_kick_clear,
			// Token: 0x0400038B RID: 907
			enm_IMono_MBAgent_lock_agent_replication_table_with_current_reliable_sequence_no,
			// Token: 0x0400038C RID: 908
			enm_IMono_MBAgent_make_dead,
			// Token: 0x0400038D RID: 909
			enm_IMono_MBAgent_make_voice,
			// Token: 0x0400038E RID: 910
			enm_IMono_MBAgent_player_attack_direction,
			// Token: 0x0400038F RID: 911
			enm_IMono_MBAgent_preload_for_rendering,
			// Token: 0x04000390 RID: 912
			enm_IMono_MBAgent_prepare_weapon_for_drop_in_equipment_slot,
			// Token: 0x04000391 RID: 913
			enm_IMono_MBAgent_remove_mesh_from_bone,
			// Token: 0x04000392 RID: 914
			enm_IMono_MBAgent_reset_enemy_caches,
			// Token: 0x04000393 RID: 915
			enm_IMono_MBAgent_reset_guard,
			// Token: 0x04000394 RID: 916
			enm_IMono_MBAgent_set_action_channel,
			// Token: 0x04000395 RID: 917
			enm_IMono_MBAgent_set_action_set,
			// Token: 0x04000396 RID: 918
			enm_IMono_MBAgent_set_agent_exclude_state_for_face_group_id,
			// Token: 0x04000397 RID: 919
			enm_IMono_MBAgent_set_agent_facial_animation,
			// Token: 0x04000398 RID: 920
			enm_IMono_MBAgent_set_agent_flags,
			// Token: 0x04000399 RID: 921
			enm_IMono_MBAgent_set_agent_idle_animation_status,
			// Token: 0x0400039A RID: 922
			enm_IMono_MBAgent_set_agent_scale,
			// Token: 0x0400039B RID: 923
			enm_IMono_MBAgent_set_ai_alarm_state,
			// Token: 0x0400039C RID: 924
			enm_IMono_MBAgent_set_ai_behavior_params,
			// Token: 0x0400039D RID: 925
			enm_IMono_MBAgent_set_ai_last_suspicious_position,
			// Token: 0x0400039E RID: 926
			enm_IMono_MBAgent_set_ai_state_flags,
			// Token: 0x0400039F RID: 927
			enm_IMono_MBAgent_set_all_ai_behavior_params,
			// Token: 0x040003A0 RID: 928
			enm_IMono_MBAgent_set_attack_state,
			// Token: 0x040003A1 RID: 929
			enm_IMono_MBAgent_set_automatic_target_agent_selection,
			// Token: 0x040003A2 RID: 930
			enm_IMono_MBAgent_set_average_ping_in_milliseconds,
			// Token: 0x040003A3 RID: 931
			enm_IMono_MBAgent_set_body_armor_material_type,
			// Token: 0x040003A4 RID: 932
			enm_IMono_MBAgent_set_columnwise_follow_agent,
			// Token: 0x040003A5 RID: 933
			enm_IMono_MBAgent_set_controller,
			// Token: 0x040003A6 RID: 934
			enm_IMono_MBAgent_set_courage,
			// Token: 0x040003A7 RID: 935
			enm_IMono_MBAgent_set_current_action_progress,
			// Token: 0x040003A8 RID: 936
			enm_IMono_MBAgent_set_current_action_speed,
			// Token: 0x040003A9 RID: 937
			enm_IMono_MBAgent_set_direction_change_tendency,
			// Token: 0x040003AA RID: 938
			enm_IMono_MBAgent_set_event_control_flags,
			// Token: 0x040003AB RID: 939
			enm_IMono_MBAgent_set_excluded_from_gravity,
			// Token: 0x040003AC RID: 940
			enm_IMono_MBAgent_set_firing_order,
			// Token: 0x040003AD RID: 941
			enm_IMono_MBAgent_set_force_attached_entity,
			// Token: 0x040003AE RID: 942
			enm_IMono_MBAgent_set_formation_frame_disabled,
			// Token: 0x040003AF RID: 943
			enm_IMono_MBAgent_set_formation_frame_enabled,
			// Token: 0x040003B0 RID: 944
			enm_IMono_MBAgent_set_formation_info,
			// Token: 0x040003B1 RID: 945
			enm_IMono_MBAgent_set_formation_integrity_data,
			// Token: 0x040003B2 RID: 946
			enm_IMono_MBAgent_set_formation_no,
			// Token: 0x040003B3 RID: 947
			enm_IMono_MBAgent_set_hand_inverse_kinematics_frame,
			// Token: 0x040003B4 RID: 948
			enm_IMono_MBAgent_set_hand_inverse_kinematics_frame_for_mission_object_usage,
			// Token: 0x040003B5 RID: 949
			enm_IMono_MBAgent_set_has_on_ai_input_set_callback,
			// Token: 0x040003B6 RID: 950
			enm_IMono_MBAgent_set_head_camera_mode,
			// Token: 0x040003B7 RID: 951
			enm_IMono_MBAgent_set_initial_frame,
			// Token: 0x040003B8 RID: 952
			enm_IMono_MBAgent_set_interaction_agent,
			// Token: 0x040003B9 RID: 953
			enm_IMono_MBAgent_set_is_look_direction_locked,
			// Token: 0x040003BA RID: 954
			enm_IMono_MBAgent_set_is_physics_force_closed,
			// Token: 0x040003BB RID: 955
			enm_IMono_MBAgent_set_look_agent,
			// Token: 0x040003BC RID: 956
			enm_IMono_MBAgent_set_look_direction,
			// Token: 0x040003BD RID: 957
			enm_IMono_MBAgent_set_look_direction_as_angle,
			// Token: 0x040003BE RID: 958
			enm_IMono_MBAgent_set_look_to_point_of_interest,
			// Token: 0x040003BF RID: 959
			enm_IMono_MBAgent_set_maximum_speed_limit,
			// Token: 0x040003C0 RID: 960
			enm_IMono_MBAgent_set_mono_object,
			// Token: 0x040003C1 RID: 961
			enm_IMono_MBAgent_set_mount_agent,
			// Token: 0x040003C2 RID: 962
			enm_IMono_MBAgent_set_movement_direction,
			// Token: 0x040003C3 RID: 963
			enm_IMono_MBAgent_set_movement_flags,
			// Token: 0x040003C4 RID: 964
			enm_IMono_MBAgent_set_movement_input_vector,
			// Token: 0x040003C5 RID: 965
			enm_IMono_MBAgent_set_network_peer,
			// Token: 0x040003C6 RID: 966
			enm_IMono_MBAgent_set_overriden_strike_and_death_action,
			// Token: 0x040003C7 RID: 967
			enm_IMono_MBAgent_set_position,
			// Token: 0x040003C8 RID: 968
			enm_IMono_MBAgent_set_reload_ammo_in_slot,
			// Token: 0x040003C9 RID: 969
			enm_IMono_MBAgent_set_render_check_enabled,
			// Token: 0x040003CA RID: 970
			enm_IMono_MBAgent_set_retreat_mode,
			// Token: 0x040003CB RID: 971
			enm_IMono_MBAgent_set_riding_order,
			// Token: 0x040003CC RID: 972
			enm_IMono_MBAgent_set_scripted_combat_flags,
			// Token: 0x040003CD RID: 973
			enm_IMono_MBAgent_set_scripted_flags,
			// Token: 0x040003CE RID: 974
			enm_IMono_MBAgent_set_scripted_position,
			// Token: 0x040003CF RID: 975
			enm_IMono_MBAgent_set_scripted_position_and_direction,
			// Token: 0x040003D0 RID: 976
			enm_IMono_MBAgent_set_scripted_target_entity,
			// Token: 0x040003D1 RID: 977
			enm_IMono_MBAgent_set_selected_mount_index,
			// Token: 0x040003D2 RID: 978
			enm_IMono_MBAgent_set_should_catch_up_with_formation,
			// Token: 0x040003D3 RID: 979
			enm_IMono_MBAgent_set_state_flags,
			// Token: 0x040003D4 RID: 980
			enm_IMono_MBAgent_set_target_agent,
			// Token: 0x040003D5 RID: 981
			enm_IMono_MBAgent_set_target_formation_index,
			// Token: 0x040003D6 RID: 982
			enm_IMono_MBAgent_set_target_position,
			// Token: 0x040003D7 RID: 983
			enm_IMono_MBAgent_set_target_position_and_direction,
			// Token: 0x040003D8 RID: 984
			enm_IMono_MBAgent_set_target_up,
			// Token: 0x040003D9 RID: 985
			enm_IMono_MBAgent_set_target_z,
			// Token: 0x040003DA RID: 986
			enm_IMono_MBAgent_set_team,
			// Token: 0x040003DB RID: 987
			enm_IMono_MBAgent_set_usage_index_of_weapon_in_slot_as_client,
			// Token: 0x040003DC RID: 988
			enm_IMono_MBAgent_set_velocity_limits_on_ragdoll,
			// Token: 0x040003DD RID: 989
			enm_IMono_MBAgent_set_weapon_ammo_as_client,
			// Token: 0x040003DE RID: 990
			enm_IMono_MBAgent_set_weapon_amount_in_slot,
			// Token: 0x040003DF RID: 991
			enm_IMono_MBAgent_set_weapon_guard,
			// Token: 0x040003E0 RID: 992
			enm_IMono_MBAgent_set_weapon_hit_points_in_slot,
			// Token: 0x040003E1 RID: 993
			enm_IMono_MBAgent_set_weapon_reload_phase_as_client,
			// Token: 0x040003E2 RID: 994
			enm_IMono_MBAgent_set_wielded_item_index_as_client,
			// Token: 0x040003E3 RID: 995
			enm_IMono_MBAgent_start_fading_out,
			// Token: 0x040003E4 RID: 996
			enm_IMono_MBAgent_start_ragdoll_as_corpse,
			// Token: 0x040003E5 RID: 997
			enm_IMono_MBAgent_start_switching_weapon_usage_index_as_client,
			// Token: 0x040003E6 RID: 998
			enm_IMono_MBAgent_tick_action_channels,
			// Token: 0x040003E7 RID: 999
			enm_IMono_MBAgent_try_get_immediate_agent_movement_data,
			// Token: 0x040003E8 RID: 1000
			enm_IMono_MBAgent_try_to_sheath_weapon_in_hand,
			// Token: 0x040003E9 RID: 1001
			enm_IMono_MBAgent_try_to_wield_weapon_in_slot,
			// Token: 0x040003EA RID: 1002
			enm_IMono_MBAgent_update_driven_properties,
			// Token: 0x040003EB RID: 1003
			enm_IMono_MBAgent_update_weapons,
			// Token: 0x040003EC RID: 1004
			enm_IMono_MBAgent_weapon_equipped,
			// Token: 0x040003ED RID: 1005
			enm_IMono_MBAgent_wield_next_weapon,
			// Token: 0x040003EE RID: 1006
			enm_IMono_MBAgent_yell_after_delay,
			// Token: 0x040003EF RID: 1007
			enm_IMono_MBAgentVisuals_add_child_entity,
			// Token: 0x040003F0 RID: 1008
			enm_IMono_MBAgentVisuals_add_horse_reins_cloth_mesh,
			// Token: 0x040003F1 RID: 1009
			enm_IMono_MBAgentVisuals_add_mesh,
			// Token: 0x040003F2 RID: 1010
			enm_IMono_MBAgentVisuals_add_multi_mesh,
			// Token: 0x040003F3 RID: 1011
			enm_IMono_MBAgentVisuals_add_prefab_to_agent_visual_bone_by_bone_type,
			// Token: 0x040003F4 RID: 1012
			enm_IMono_MBAgentVisuals_add_prefab_to_agent_visual_bone_by_real_bone_index,
			// Token: 0x040003F5 RID: 1013
			enm_IMono_MBAgentVisuals_add_skin_meshes_to_agent_visuals,
			// Token: 0x040003F6 RID: 1014
			enm_IMono_MBAgentVisuals_add_weapon_to_agent_entity,
			// Token: 0x040003F7 RID: 1015
			enm_IMono_MBAgentVisuals_apply_skeleton_scale,
			// Token: 0x040003F8 RID: 1016
			enm_IMono_MBAgentVisuals_batch_last_lod_meshes,
			// Token: 0x040003F9 RID: 1017
			enm_IMono_MBAgentVisuals_check_resources,
			// Token: 0x040003FA RID: 1018
			enm_IMono_MBAgentVisuals_clear_all_weapon_meshes,
			// Token: 0x040003FB RID: 1019
			enm_IMono_MBAgentVisuals_clear_visual_components,
			// Token: 0x040003FC RID: 1020
			enm_IMono_MBAgentVisuals_clear_weapon_meshes,
			// Token: 0x040003FD RID: 1021
			enm_IMono_MBAgentVisuals_create_agent_renderer_scene_controller,
			// Token: 0x040003FE RID: 1022
			enm_IMono_MBAgentVisuals_create_agent_visuals,
			// Token: 0x040003FF RID: 1023
			enm_IMono_MBAgentVisuals_create_particle_system_attached_to_bone,
			// Token: 0x04000400 RID: 1024
			enm_IMono_MBAgentVisuals_destruct_agent_renderer_scene_controller,
			// Token: 0x04000401 RID: 1025
			enm_IMono_MBAgentVisuals_disable_contour,
			// Token: 0x04000402 RID: 1026
			enm_IMono_MBAgentVisuals_fill_entity_with_body_meshes_without_agent_visuals,
			// Token: 0x04000403 RID: 1027
			enm_IMono_MBAgentVisuals_get_attached_weapon_entity,
			// Token: 0x04000404 RID: 1028
			enm_IMono_MBAgentVisuals_get_quick_bone_entitial_frame,
			// Token: 0x04000405 RID: 1029
			enm_IMono_MBAgentVisuals_get_bone_entitial_frame_at_animation_progress,
			// Token: 0x04000406 RID: 1030
			enm_IMono_MBAgentVisuals_get_bone_type_data,
			// Token: 0x04000407 RID: 1031
			enm_IMono_MBAgentVisuals_get_current_head_look_direction,
			// Token: 0x04000408 RID: 1032
			enm_IMono_MBAgentVisuals_get_current_helmet_scaling_factor,
			// Token: 0x04000409 RID: 1033
			enm_IMono_MBAgentVisuals_get_current_ragdoll_state,
			// Token: 0x0400040A RID: 1034
			enm_IMono_MBAgentVisuals_get_entity,
			// Token: 0x0400040B RID: 1035
			enm_IMono_MBAgentVisuals_get_entity_pointer,
			// Token: 0x0400040C RID: 1036
			enm_IMono_MBAgentVisuals_get_frame,
			// Token: 0x0400040D RID: 1037
			enm_IMono_MBAgentVisuals_get_global_frame,
			// Token: 0x0400040E RID: 1038
			enm_IMono_MBAgentVisuals_get_global_stable_eye_point,
			// Token: 0x0400040F RID: 1039
			enm_IMono_MBAgentVisuals_get_global_stable_neck_point,
			// Token: 0x04000410 RID: 1040
			enm_IMono_MBAgentVisuals_get_movement_mode,
			// Token: 0x04000411 RID: 1041
			enm_IMono_MBAgentVisuals_get_real_bone_index,
			// Token: 0x04000412 RID: 1042
			enm_IMono_MBAgentVisuals_get_skeleton,
			// Token: 0x04000413 RID: 1043
			enm_IMono_MBAgentVisuals_get_visible,
			// Token: 0x04000414 RID: 1044
			enm_IMono_MBAgentVisuals_get_visual_strength_of_agent_visual,
			// Token: 0x04000415 RID: 1045
			enm_IMono_MBAgentVisuals_is_valid,
			// Token: 0x04000416 RID: 1046
			enm_IMono_MBAgentVisuals_lazy_update_agent_renderer_data,
			// Token: 0x04000417 RID: 1047
			enm_IMono_MBAgentVisuals_make_voice,
			// Token: 0x04000418 RID: 1048
			enm_IMono_MBAgentVisuals_remove_child_entity,
			// Token: 0x04000419 RID: 1049
			enm_IMono_MBAgentVisuals_remove_mesh,
			// Token: 0x0400041A RID: 1050
			enm_IMono_MBAgentVisuals_remove_multi_mesh,
			// Token: 0x0400041B RID: 1051
			enm_IMono_MBAgentVisuals_reset,
			// Token: 0x0400041C RID: 1052
			enm_IMono_MBAgentVisuals_reset_next_frame,
			// Token: 0x0400041D RID: 1053
			enm_IMono_MBAgentVisuals_set_agent_local_speed,
			// Token: 0x0400041E RID: 1054
			enm_IMono_MBAgentVisuals_set_agent_lod_make_zero_or_max,
			// Token: 0x0400041F RID: 1055
			enm_IMono_MBAgentVisuals_set_as_contour_entity,
			// Token: 0x04000420 RID: 1056
			enm_IMono_MBAgentVisuals_set_attached_position_for_rope_entity_after_animation_post_integrate,
			// Token: 0x04000421 RID: 1057
			enm_IMono_MBAgentVisuals_set_cloth_component_keep_state_of_all_meshes,
			// Token: 0x04000422 RID: 1058
			enm_IMono_MBAgentVisuals_set_cloth_wind_to_weapon_at_index,
			// Token: 0x04000423 RID: 1059
			enm_IMono_MBAgentVisuals_set_contour_state,
			// Token: 0x04000424 RID: 1060
			enm_IMono_MBAgentVisuals_set_do_timer_based_skeleton_forced_updates,
			// Token: 0x04000425 RID: 1061
			enm_IMono_MBAgentVisuals_set_enable_occlusion_culling,
			// Token: 0x04000426 RID: 1062
			enm_IMono_MBAgentVisuals_set_enforced_visibility_for_all_agents,
			// Token: 0x04000427 RID: 1063
			enm_IMono_MBAgentVisuals_set_entity,
			// Token: 0x04000428 RID: 1064
			enm_IMono_MBAgentVisuals_set_face_generation_params,
			// Token: 0x04000429 RID: 1065
			enm_IMono_MBAgentVisuals_set_frame,
			// Token: 0x0400042A RID: 1066
			enm_IMono_MBAgentVisuals_set_lod_atlas_shading_index,
			// Token: 0x0400042B RID: 1067
			enm_IMono_MBAgentVisuals_set_look_direction,
			// Token: 0x0400042C RID: 1068
			enm_IMono_MBAgentVisuals_set_setup_morph_node,
			// Token: 0x0400042D RID: 1069
			enm_IMono_MBAgentVisuals_set_skeleton,
			// Token: 0x0400042E RID: 1070
			enm_IMono_MBAgentVisuals_set_visible,
			// Token: 0x0400042F RID: 1071
			enm_IMono_MBAgentVisuals_set_voice_definition_index,
			// Token: 0x04000430 RID: 1072
			enm_IMono_MBAgentVisuals_set_wielded_weapon_indices,
			// Token: 0x04000431 RID: 1073
			enm_IMono_MBAgentVisuals_start_rhubarb_record,
			// Token: 0x04000432 RID: 1074
			enm_IMono_MBAgentVisuals_tick,
			// Token: 0x04000433 RID: 1075
			enm_IMono_MBAgentVisuals_update_quiver_mesh_of_weapon_in_slot,
			// Token: 0x04000434 RID: 1076
			enm_IMono_MBAgentVisuals_update_skeleton_scale,
			// Token: 0x04000435 RID: 1077
			enm_IMono_MBAgentVisuals_use_scaled_weapons,
			// Token: 0x04000436 RID: 1078
			enm_IMono_MBAgentVisuals_validate_agent_visuals_reseted,
			// Token: 0x04000437 RID: 1079
			enm_IMono_MBAnimation_get_animation_index_of_action_code,
			// Token: 0x04000438 RID: 1080
			enm_IMono_MBAnimation_check_animation_clip_exists,
			// Token: 0x04000439 RID: 1081
			enm_IMono_MBAnimation_get_action_animation_duration,
			// Token: 0x0400043A RID: 1082
			enm_IMono_MBAnimation_get_action_blend_out_start_progress,
			// Token: 0x0400043B RID: 1083
			enm_IMono_MBAnimation_get_action_code_with_name,
			// Token: 0x0400043C RID: 1084
			enm_IMono_MBAnimation_get_action_name_with_code,
			// Token: 0x0400043D RID: 1085
			enm_IMono_MBAnimation_get_action_type,
			// Token: 0x0400043E RID: 1086
			enm_IMono_MBAnimation_get_animation_blend_in_period,
			// Token: 0x0400043F RID: 1087
			enm_IMono_MBAnimation_get_animation_blends_with_action_index,
			// Token: 0x04000440 RID: 1088
			enm_IMono_MBAnimation_get_animation_continue_to_action,
			// Token: 0x04000441 RID: 1089
			enm_IMono_MBAnimation_get_animation_displacement_at_progress,
			// Token: 0x04000442 RID: 1090
			enm_IMono_MBAnimation_get_animation_duration,
			// Token: 0x04000443 RID: 1091
			enm_IMono_MBAnimation_get_animation_flags,
			// Token: 0x04000444 RID: 1092
			enm_IMono_MBAnimation_get_animation_name,
			// Token: 0x04000445 RID: 1093
			enm_IMono_MBAnimation_get_animation_parameter1,
			// Token: 0x04000446 RID: 1094
			enm_IMono_MBAnimation_get_animation_parameter2,
			// Token: 0x04000447 RID: 1095
			enm_IMono_MBAnimation_get_animation_parameter3,
			// Token: 0x04000448 RID: 1096
			enm_IMono_MBAnimation_get_displacement_vector,
			// Token: 0x04000449 RID: 1097
			enm_IMono_MBAnimation_get_id_with_index,
			// Token: 0x0400044A RID: 1098
			enm_IMono_MBAnimation_get_index_with_id,
			// Token: 0x0400044B RID: 1099
			enm_IMono_MBAnimation_get_num_action_codes,
			// Token: 0x0400044C RID: 1100
			enm_IMono_MBAnimation_get_num_animations,
			// Token: 0x0400044D RID: 1101
			enm_IMono_MBAnimation_is_any_animation_loading_from_disk,
			// Token: 0x0400044E RID: 1102
			enm_IMono_MBAnimation_prefetch_animation_clip,
			// Token: 0x0400044F RID: 1103
			enm_IMono_MBBannerlordChecker_get_engine_struct_member_offset,
			// Token: 0x04000450 RID: 1104
			enm_IMono_MBBannerlordChecker_get_engine_struct_size,
			// Token: 0x04000451 RID: 1105
			enm_IMono_MBBannerlordConfig_validate_options,
			// Token: 0x04000452 RID: 1106
			enm_IMono_MBBannerlordTableauManager_get_number_of_pending_tableau_requests,
			// Token: 0x04000453 RID: 1107
			enm_IMono_MBBannerlordTableauManager_initialize_character_tableau_render_system,
			// Token: 0x04000454 RID: 1108
			enm_IMono_MBBannerlordTableauManager_request_character_tableau_render,
			// Token: 0x04000455 RID: 1109
			enm_IMono_MBDebugExtensions_override_native_parameter,
			// Token: 0x04000456 RID: 1110
			enm_IMono_MBDebugExtensions_reload_native_parameters,
			// Token: 0x04000457 RID: 1111
			enm_IMono_MBDebugExtensions_render_debug_arc_on_terrain,
			// Token: 0x04000458 RID: 1112
			enm_IMono_MBDebugExtensions_render_debug_circle_on_terrain,
			// Token: 0x04000459 RID: 1113
			enm_IMono_MBDebugExtensions_render_debug_line_on_terrain,
			// Token: 0x0400045A RID: 1114
			enm_IMono_MBEditor_activate_scene_editor_presentation,
			// Token: 0x0400045B RID: 1115
			enm_IMono_MBEditor_add_editor_warning,
			// Token: 0x0400045C RID: 1116
			enm_IMono_MBEditor_add_entity_warning,
			// Token: 0x0400045D RID: 1117
			enm_IMono_MBEditor_add_nav_mesh_warning,
			// Token: 0x0400045E RID: 1118
			enm_IMono_MBEditor_apply_delta_to_editor_camera,
			// Token: 0x0400045F RID: 1119
			enm_IMono_MBEditor_border_helpers_enabled,
			// Token: 0x04000460 RID: 1120
			enm_IMono_MBEditor_deactivate_scene_editor_presentation,
			// Token: 0x04000461 RID: 1121
			enm_IMono_MBEditor_enter_edit_mission_mode,
			// Token: 0x04000462 RID: 1122
			enm_IMono_MBEditor_enter_edit_mode,
			// Token: 0x04000463 RID: 1123
			enm_IMono_MBEditor_exit_edit_mode,
			// Token: 0x04000464 RID: 1124
			enm_IMono_MBEditor_get_all_prefabs_and_child_with_tag,
			// Token: 0x04000465 RID: 1125
			enm_IMono_MBEditor_get_editor_scene_view,
			// Token: 0x04000466 RID: 1126
			enm_IMono_MBEditor_helpers_enabled,
			// Token: 0x04000467 RID: 1127
			enm_IMono_MBEditor_is_edit_mode,
			// Token: 0x04000468 RID: 1128
			enm_IMono_MBEditor_is_edit_mode_enabled,
			// Token: 0x04000469 RID: 1129
			enm_IMono_MBEditor_is_entity_selected,
			// Token: 0x0400046A RID: 1130
			enm_IMono_MBEditor_is_replay_manager_recording,
			// Token: 0x0400046B RID: 1131
			enm_IMono_MBEditor_is_replay_manager_rendering,
			// Token: 0x0400046C RID: 1132
			enm_IMono_MBEditor_is_replay_manager_replaying,
			// Token: 0x0400046D RID: 1133
			enm_IMono_MBEditor_leave_edit_mission_mode,
			// Token: 0x0400046E RID: 1134
			enm_IMono_MBEditor_leave_edit_mode,
			// Token: 0x0400046F RID: 1135
			enm_IMono_MBEditor_render_editor_mesh,
			// Token: 0x04000470 RID: 1136
			enm_IMono_MBEditor_set_level_visibility,
			// Token: 0x04000471 RID: 1137
			enm_IMono_MBEditor_set_upgrade_level_visibility,
			// Token: 0x04000472 RID: 1138
			enm_IMono_MBEditor_tick_edit_mode,
			// Token: 0x04000473 RID: 1139
			enm_IMono_MBEditor_tick_scene_editor_presentation,
			// Token: 0x04000474 RID: 1140
			enm_IMono_MBEditor_toggle_enable_editor_physics,
			// Token: 0x04000475 RID: 1141
			enm_IMono_MBEditor_update_scene_tree,
			// Token: 0x04000476 RID: 1142
			enm_IMono_MBEditor_zoom_to_position,
			// Token: 0x04000477 RID: 1143
			enm_IMono_MBFaceGen_enforce_constraints,
			// Token: 0x04000478 RID: 1144
			enm_IMono_MBFaceGen_flush_face_cache,
			// Token: 0x04000479 RID: 1145
			enm_IMono_MBFaceGen_get_deform_key_data,
			// Token: 0x0400047A RID: 1146
			enm_IMono_MBFaceGen_get_face_gen_instances_length,
			// Token: 0x0400047B RID: 1147
			enm_IMono_MBFaceGen_get_facial_indices_by_tag,
			// Token: 0x0400047C RID: 1148
			enm_IMono_MBFaceGen_get_hair_color_count,
			// Token: 0x0400047D RID: 1149
			enm_IMono_MBFaceGen_get_hair_color_gradient_points,
			// Token: 0x0400047E RID: 1150
			enm_IMono_MBFaceGen_get_hair_indices_by_tag,
			// Token: 0x0400047F RID: 1151
			enm_IMono_MBFaceGen_get_maturity_type,
			// Token: 0x04000480 RID: 1152
			enm_IMono_MBFaceGen_get_num_editable_deform_keys,
			// Token: 0x04000481 RID: 1153
			enm_IMono_MBFaceGen_get_params_from_key,
			// Token: 0x04000482 RID: 1154
			enm_IMono_MBFaceGen_get_params_max,
			// Token: 0x04000483 RID: 1155
			enm_IMono_MBFaceGen_get_race_ids,
			// Token: 0x04000484 RID: 1156
			enm_IMono_MBFaceGen_get_random_body_properties,
			// Token: 0x04000485 RID: 1157
			enm_IMono_MBFaceGen_get_scale,
			// Token: 0x04000486 RID: 1158
			enm_IMono_MBFaceGen_get_skin_color_count,
			// Token: 0x04000487 RID: 1159
			enm_IMono_MBFaceGen_get_skin_color_gradient_points,
			// Token: 0x04000488 RID: 1160
			enm_IMono_MBFaceGen_get_tatoo_color_count,
			// Token: 0x04000489 RID: 1161
			enm_IMono_MBFaceGen_get_tatoo_color_gradient_points,
			// Token: 0x0400048A RID: 1162
			enm_IMono_MBFaceGen_get_tattoo_indices_by_tag,
			// Token: 0x0400048B RID: 1163
			enm_IMono_MBFaceGen_get_voice_records_count,
			// Token: 0x0400048C RID: 1164
			enm_IMono_MBFaceGen_get_voice_type_usable_for_player_data,
			// Token: 0x0400048D RID: 1165
			enm_IMono_MBFaceGen_get_zero_probabilities,
			// Token: 0x0400048E RID: 1166
			enm_IMono_MBFaceGen_produce_numeric_key_with_default_values,
			// Token: 0x0400048F RID: 1167
			enm_IMono_MBFaceGen_produce_numeric_key_with_params,
			// Token: 0x04000490 RID: 1168
			enm_IMono_MBFaceGen_transform_face_keys_to_default_face,
			// Token: 0x04000491 RID: 1169
			enm_IMono_MBGame_load_module_data,
			// Token: 0x04000492 RID: 1170
			enm_IMono_MBGame_start_new,
			// Token: 0x04000493 RID: 1171
			enm_IMono_MBGameEntityExtensions_create_from_weapon,
			// Token: 0x04000494 RID: 1172
			enm_IMono_MBGameEntityExtensions_fade_in,
			// Token: 0x04000495 RID: 1173
			enm_IMono_MBGameEntityExtensions_fade_out,
			// Token: 0x04000496 RID: 1174
			enm_IMono_MBGameEntityExtensions_hide_if_not_fading_out,
			// Token: 0x04000497 RID: 1175
			enm_IMono_MBItem_get_holster_frame_by_index,
			// Token: 0x04000498 RID: 1176
			enm_IMono_MBItem_get_item_holster_index,
			// Token: 0x04000499 RID: 1177
			enm_IMono_MBItem_get_item_is_passive_usage,
			// Token: 0x0400049A RID: 1178
			enm_IMono_MBItem_get_item_usage_index,
			// Token: 0x0400049B RID: 1179
			enm_IMono_MBItem_get_item_usage_reload_action_code,
			// Token: 0x0400049C RID: 1180
			enm_IMono_MBItem_get_item_usage_set_flags,
			// Token: 0x0400049D RID: 1181
			enm_IMono_MBItem_get_item_usage_strike_type,
			// Token: 0x0400049E RID: 1182
			enm_IMono_MBItem_get_missile_range,
			// Token: 0x0400049F RID: 1183
			enm_IMono_MBMapScene_get_accessible_point_near_position,
			// Token: 0x040004A0 RID: 1184
			enm_IMono_MBMapScene_get_battle_scene_index_map,
			// Token: 0x040004A1 RID: 1185
			enm_IMono_MBMapScene_get_battle_scene_index_map_resolution,
			// Token: 0x040004A2 RID: 1186
			enm_IMono_MBMapScene_get_color_grade_grid_data,
			// Token: 0x040004A3 RID: 1187
			enm_IMono_MBMapScene_get_mouse_visible,
			// Token: 0x040004A4 RID: 1188
			enm_IMono_MBMapScene_get_nearest_nav_mesh_face_center_position_between_regions_using_path,
			// Token: 0x040004A5 RID: 1189
			enm_IMono_MBMapScene_get_nearest_nav_mesh_face_center_position_for_position,
			// Token: 0x040004A6 RID: 1190
			enm_IMono_MBMapScene_get_season_time_factor,
			// Token: 0x040004A7 RID: 1191
			enm_IMono_MBMapScene_load_atmosphere_data,
			// Token: 0x040004A8 RID: 1192
			enm_IMono_MBMapScene_remove_zero_corner_bodies,
			// Token: 0x040004A9 RID: 1193
			enm_IMono_MBMapScene_send_mouse_key_down_event,
			// Token: 0x040004AA RID: 1194
			enm_IMono_MBMapScene_set_frame_for_atmosphere,
			// Token: 0x040004AB RID: 1195
			enm_IMono_MBMapScene_set_mouse_pos,
			// Token: 0x040004AC RID: 1196
			enm_IMono_MBMapScene_set_mouse_visible,
			// Token: 0x040004AD RID: 1197
			enm_IMono_MBMapScene_set_political_color,
			// Token: 0x040004AE RID: 1198
			enm_IMono_MBMapScene_set_season_time_factor,
			// Token: 0x040004AF RID: 1199
			enm_IMono_MBMapScene_set_terrain_dynamic_params,
			// Token: 0x040004B0 RID: 1200
			enm_IMono_MBMapScene_tick_ambient_sounds,
			// Token: 0x040004B1 RID: 1201
			enm_IMono_MBMapScene_tick_step_sound,
			// Token: 0x040004B2 RID: 1202
			enm_IMono_MBMapScene_tick_visuals,
			// Token: 0x040004B3 RID: 1203
			enm_IMono_MBMapScene_validate_terrain_sound_ids,
			// Token: 0x040004B4 RID: 1204
			enm_IMono_MBMessageManager_display_message,
			// Token: 0x040004B5 RID: 1205
			enm_IMono_MBMessageManager_display_message_with_color,
			// Token: 0x040004B6 RID: 1206
			enm_IMono_MBMessageManager_set_message_manager,
			// Token: 0x040004B7 RID: 1207
			enm_IMono_MBMission_add_ai_debug_text,
			// Token: 0x040004B8 RID: 1208
			enm_IMono_MBMission_add_boundary,
			// Token: 0x040004B9 RID: 1209
			enm_IMono_MBMission_add_missile,
			// Token: 0x040004BA RID: 1210
			enm_IMono_MBMission_add_missile_single_usage,
			// Token: 0x040004BB RID: 1211
			enm_IMono_MBMission_add_particle_system_burst_by_name,
			// Token: 0x040004BC RID: 1212
			enm_IMono_MBMission_add_team,
			// Token: 0x040004BD RID: 1213
			enm_IMono_MBMission_backup_record_to_file,
			// Token: 0x040004BE RID: 1214
			enm_IMono_MBMission_batch_formation_unit_positions,
			// Token: 0x040004BF RID: 1215
			enm_IMono_MBMission_clear_agent_actions,
			// Token: 0x040004C0 RID: 1216
			enm_IMono_MBMission_clear_corpses,
			// Token: 0x040004C1 RID: 1217
			enm_IMono_MBMission_clear_missiles,
			// Token: 0x040004C2 RID: 1218
			enm_IMono_MBMission_clear_record_buffers,
			// Token: 0x040004C3 RID: 1219
			enm_IMono_MBMission_clear_resources,
			// Token: 0x040004C4 RID: 1220
			enm_IMono_MBMission_clear_scene,
			// Token: 0x040004C5 RID: 1221
			enm_IMono_MBMission_compute_exact_missile_range_at_height_difference,
			// Token: 0x040004C6 RID: 1222
			enm_IMono_MBMission_create_agent,
			// Token: 0x040004C7 RID: 1223
			enm_IMono_MBMission_create_mission,
			// Token: 0x040004C8 RID: 1224
			enm_IMono_MBMission_defrag_render_buffers,
			// Token: 0x040004C9 RID: 1225
			enm_IMono_MBMission_end_of_record,
			// Token: 0x040004CA RID: 1226
			enm_IMono_MBMission_finalize_mission,
			// Token: 0x040004CB RID: 1227
			enm_IMono_MBMission_find_agent_with_index,
			// Token: 0x040004CC RID: 1228
			enm_IMono_MBMission_find_convex_hull,
			// Token: 0x040004CD RID: 1229
			enm_IMono_MBMission_force_disable_occlusion,
			// Token: 0x040004CE RID: 1230
			enm_IMono_MBMission_get_agent_count_around_position,
			// Token: 0x040004CF RID: 1231
			enm_IMono_MBMission_get_alternate_position_for_navmeshless_or_out_of_bounds_position,
			// Token: 0x040004D0 RID: 1232
			enm_IMono_MBMission_get_atmosphere_name_for_replay,
			// Token: 0x040004D1 RID: 1233
			enm_IMono_MBMission_get_atmosphere_season_for_replay,
			// Token: 0x040004D2 RID: 1234
			enm_IMono_MBMission_get_average_fps,
			// Token: 0x040004D3 RID: 1235
			enm_IMono_MBMission_get_average_morale_of_agents,
			// Token: 0x040004D4 RID: 1236
			enm_IMono_MBMission_get_best_slope_angle_height_pos_for_defending,
			// Token: 0x040004D5 RID: 1237
			enm_IMono_MBMission_get_best_slope_towards_direction,
			// Token: 0x040004D6 RID: 1238
			enm_IMono_MBMission_get_biggest_agent_collision_padding,
			// Token: 0x040004D7 RID: 1239
			enm_IMono_MBMission_get_boundary_count,
			// Token: 0x040004D8 RID: 1240
			enm_IMono_MBMission_get_boundary_name,
			// Token: 0x040004D9 RID: 1241
			enm_IMono_MBMission_get_boundary_points,
			// Token: 0x040004DA RID: 1242
			enm_IMono_MBMission_get_boundary_radius,
			// Token: 0x040004DB RID: 1243
			enm_IMono_MBMission_get_camera_frame,
			// Token: 0x040004DC RID: 1244
			enm_IMono_MBMission_get_clear_scene_timer_elapsed_time,
			// Token: 0x040004DD RID: 1245
			enm_IMono_MBMission_get_closest_ally,
			// Token: 0x040004DE RID: 1246
			enm_IMono_MBMission_get_closest_boundary_position,
			// Token: 0x040004DF RID: 1247
			enm_IMono_MBMission_get_closest_enemy,
			// Token: 0x040004E0 RID: 1248
			enm_IMono_MBMission_get_combat_type,
			// Token: 0x040004E1 RID: 1249
			enm_IMono_MBMission_get_current_volume_generator_version,
			// Token: 0x040004E2 RID: 1250
			enm_IMono_MBMission_get_debug_agent,
			// Token: 0x040004E3 RID: 1251
			enm_IMono_MBMission_get_fall_avoid_system_active,
			// Token: 0x040004E4 RID: 1252
			enm_IMono_MBMission_get_game_type_for_replay,
			// Token: 0x040004E5 RID: 1253
			enm_IMono_MBMission_get_is_loading_finished,
			// Token: 0x040004E6 RID: 1254
			enm_IMono_MBMission_get_missile_collision_point,
			// Token: 0x040004E7 RID: 1255
			enm_IMono_MBMission_get_missile_has_rigid_body,
			// Token: 0x040004E8 RID: 1256
			enm_IMono_MBMission_get_missile_range,
			// Token: 0x040004E9 RID: 1257
			enm_IMono_MBMission_get_missile_vertical_aim_correction,
			// Token: 0x040004EA RID: 1258
			enm_IMono_MBMission_get_navigation_points,
			// Token: 0x040004EB RID: 1259
			enm_IMono_MBMission_get_nearby_agents_aux,
			// Token: 0x040004EC RID: 1260
			enm_IMono_MBMission_get_number_of_teams,
			// Token: 0x040004ED RID: 1261
			enm_IMono_MBMission_get_old_position_of_missile,
			// Token: 0x040004EE RID: 1262
			enm_IMono_MBMission_get_pause_ai_tick,
			// Token: 0x040004EF RID: 1263
			enm_IMono_MBMission_get_position_of_missile,
			// Token: 0x040004F0 RID: 1264
			enm_IMono_MBMission_get_scene_levels_for_replay,
			// Token: 0x040004F1 RID: 1265
			enm_IMono_MBMission_get_scene_name_for_replay,
			// Token: 0x040004F2 RID: 1266
			enm_IMono_MBMission_get_straight_path_to_target,
			// Token: 0x040004F3 RID: 1267
			enm_IMono_MBMission_get_tick_debug_paused,
			// Token: 0x040004F4 RID: 1268
			enm_IMono_MBMission_get_time,
			// Token: 0x040004F5 RID: 1269
			enm_IMono_MBMission_get_velocity_of_missile,
			// Token: 0x040004F6 RID: 1270
			enm_IMono_MBMission_get_water_level_at_position,
			// Token: 0x040004F7 RID: 1271
			enm_IMono_MBMission_get_weighted_point_of_enemies,
			// Token: 0x040004F8 RID: 1272
			enm_IMono_MBMission_has_any_agents_of_team_around,
			// Token: 0x040004F9 RID: 1273
			enm_IMono_MBMission_idle_tick,
			// Token: 0x040004FA RID: 1274
			enm_IMono_MBMission_initialize_mission,
			// Token: 0x040004FB RID: 1275
			enm_IMono_MBMission_is_agent_in_proximity_map,
			// Token: 0x040004FC RID: 1276
			enm_IMono_MBMission_is_formation_unit_position_available,
			// Token: 0x040004FD RID: 1277
			enm_IMono_MBMission_is_position_inside_any_blocker_nav_mesh_face_2d,
			// Token: 0x040004FE RID: 1278
			enm_IMono_MBMission_is_position_inside_boundaries,
			// Token: 0x040004FF RID: 1279
			enm_IMono_MBMission_is_position_inside_hard_boundaries,
			// Token: 0x04000500 RID: 1280
			enm_IMono_MBMission_is_position_on_any_blocker_nav_mesh_face,
			// Token: 0x04000501 RID: 1281
			enm_IMono_MBMission_make_sound,
			// Token: 0x04000502 RID: 1282
			enm_IMono_MBMission_make_sound_only_on_related_peer,
			// Token: 0x04000503 RID: 1283
			enm_IMono_MBMission_make_sound_with_parameter,
			// Token: 0x04000504 RID: 1284
			enm_IMono_MBMission_on_fast_forward_state_changed,
			// Token: 0x04000505 RID: 1285
			enm_IMono_MBMission_pause_mission_scene_sounds,
			// Token: 0x04000506 RID: 1286
			enm_IMono_MBMission_prepare_missile_weapon_for_drop,
			// Token: 0x04000507 RID: 1287
			enm_IMono_MBMission_process_record_until_time,
			// Token: 0x04000508 RID: 1288
			enm_IMono_MBMission_agent_proximity_map_begin_search,
			// Token: 0x04000509 RID: 1289
			enm_IMono_MBMission_agent_proximity_map_find_next,
			// Token: 0x0400050A RID: 1290
			enm_IMono_MBMission_agent_proximity_map_get_max_search_radius,
			// Token: 0x0400050B RID: 1291
			enm_IMono_MBMission_ray_cast_for_closest_agent,
			// Token: 0x0400050C RID: 1292
			enm_IMono_MBMission_ray_cast_for_closest_agents_limbs,
			// Token: 0x0400050D RID: 1293
			enm_IMono_MBMission_ray_cast_for_given_agents_limbs,
			// Token: 0x0400050E RID: 1294
			enm_IMono_MBMission_record_current_state,
			// Token: 0x0400050F RID: 1295
			enm_IMono_MBMission_remove_boundary,
			// Token: 0x04000510 RID: 1296
			enm_IMono_MBMission_remove_missile,
			// Token: 0x04000511 RID: 1297
			enm_IMono_MBMission_reset_first_third_person_view,
			// Token: 0x04000512 RID: 1298
			enm_IMono_MBMission_reset_teams,
			// Token: 0x04000513 RID: 1299
			enm_IMono_MBMission_restart_record,
			// Token: 0x04000514 RID: 1300
			enm_IMono_MBMission_restore_record_from_file,
			// Token: 0x04000515 RID: 1301
			enm_IMono_MBMission_resume_mission_scene_sounds,
			// Token: 0x04000516 RID: 1302
			enm_IMono_MBMission_set_bow_missile_speed_modifier,
			// Token: 0x04000517 RID: 1303
			enm_IMono_MBMission_set_camera_frame,
			// Token: 0x04000518 RID: 1304
			enm_IMono_MBMission_set_camera_is_first_person,
			// Token: 0x04000519 RID: 1305
			enm_IMono_MBMission_set_close_proximity_wave_sounds_enabled,
			// Token: 0x0400051A RID: 1306
			enm_IMono_MBMission_set_combat_type,
			// Token: 0x0400051B RID: 1307
			enm_IMono_MBMission_set_crossbow_missile_speed_modifier,
			// Token: 0x0400051C RID: 1308
			enm_IMono_MBMission_set_debug_agent,
			// Token: 0x0400051D RID: 1309
			enm_IMono_MBMission_set_fall_avoid_system_active,
			// Token: 0x0400051E RID: 1310
			enm_IMono_MBMission_set_last_movement_key_pressed,
			// Token: 0x0400051F RID: 1311
			enm_IMono_MBMission_set_missile_range_modifier,
			// Token: 0x04000520 RID: 1312
			enm_IMono_MBMission_set_mission_corpse_fade_out_time_in_seconds,
			// Token: 0x04000521 RID: 1313
			enm_IMono_MBMission_set_navigation_face_cost_with_id_around_position,
			// Token: 0x04000522 RID: 1314
			enm_IMono_MBMission_set_override_corpse_count,
			// Token: 0x04000523 RID: 1315
			enm_IMono_MBMission_set_pause_ai_tick,
			// Token: 0x04000524 RID: 1316
			enm_IMono_MBMission_set_random_decide_time_of_agents,
			// Token: 0x04000525 RID: 1317
			enm_IMono_MBMission_set_render_parallel_logic_in_progress,
			// Token: 0x04000526 RID: 1318
			enm_IMono_MBMission_set_report_stuck_agents_mode,
			// Token: 0x04000527 RID: 1319
			enm_IMono_MBMission_set_throwing_missile_speed_modifier,
			// Token: 0x04000528 RID: 1320
			enm_IMono_MBMission_set_velocity_of_missile,
			// Token: 0x04000529 RID: 1321
			enm_IMono_MBMission_skip_forward_mission_replay,
			// Token: 0x0400052A RID: 1322
			enm_IMono_MBMission_start_recording,
			// Token: 0x0400052B RID: 1323
			enm_IMono_MBMission_tick,
			// Token: 0x0400052C RID: 1324
			enm_IMono_MBMission_tick_agents_and_teams_async,
			// Token: 0x0400052D RID: 1325
			enm_IMono_MBNetwork_add_new_bot_on_server,
			// Token: 0x0400052E RID: 1326
			enm_IMono_MBNetwork_add_new_player_on_server,
			// Token: 0x0400052F RID: 1327
			enm_IMono_MBNetwork_add_peer_to_disconnect,
			// Token: 0x04000530 RID: 1328
			enm_IMono_MBNetwork_begin_broadcast_module_event,
			// Token: 0x04000531 RID: 1329
			enm_IMono_MBNetwork_begin_module_event_as_client,
			// Token: 0x04000532 RID: 1330
			enm_IMono_MBNetwork_can_add_new_players_on_server,
			// Token: 0x04000533 RID: 1331
			enm_IMono_MBNetwork_clear_replication_table_statistics,
			// Token: 0x04000534 RID: 1332
			enm_IMono_MBNetwork_elapsed_time_since_last_udp_packet_arrived,
			// Token: 0x04000535 RID: 1333
			enm_IMono_MBNetwork_end_broadcast_module_event,
			// Token: 0x04000536 RID: 1334
			enm_IMono_MBNetwork_end_module_event_as_client,
			// Token: 0x04000537 RID: 1335
			enm_IMono_MBNetwork_get_active_udp_sessions_ip_address,
			// Token: 0x04000538 RID: 1336
			enm_IMono_MBNetwork_get_average_packet_loss_ratio,
			// Token: 0x04000539 RID: 1337
			enm_IMono_MBNetwork_get_debug_uploads_in_bits,
			// Token: 0x0400053A RID: 1338
			enm_IMono_MBNetwork_get_multiplayer_disabled,
			// Token: 0x0400053B RID: 1339
			enm_IMono_MBNetwork_initialize_client_side,
			// Token: 0x0400053C RID: 1340
			enm_IMono_MBNetwork_initialize_server_side,
			// Token: 0x0400053D RID: 1341
			enm_IMono_MBNetwork_is_dedicated_server,
			// Token: 0x0400053E RID: 1342
			enm_IMono_MBNetwork_prepare_new_udp_session,
			// Token: 0x0400053F RID: 1343
			enm_IMono_MBNetwork_print_debug_stats,
			// Token: 0x04000540 RID: 1344
			enm_IMono_MBNetwork_print_replication_table_statistics,
			// Token: 0x04000541 RID: 1345
			enm_IMono_MBNetwork_read_byte_array_from_packet,
			// Token: 0x04000542 RID: 1346
			enm_IMono_MBNetwork_read_float_from_packet,
			// Token: 0x04000543 RID: 1347
			enm_IMono_MBNetwork_read_int_from_packet,
			// Token: 0x04000544 RID: 1348
			enm_IMono_MBNetwork_read_long_from_packet,
			// Token: 0x04000545 RID: 1349
			enm_IMono_MBNetwork_read_string_from_packet,
			// Token: 0x04000546 RID: 1350
			enm_IMono_MBNetwork_read_uint_from_packet,
			// Token: 0x04000547 RID: 1351
			enm_IMono_MBNetwork_read_ulong_from_packet,
			// Token: 0x04000548 RID: 1352
			enm_IMono_MBNetwork_remove_bot_on_server,
			// Token: 0x04000549 RID: 1353
			enm_IMono_MBNetwork_reset_debug_uploads,
			// Token: 0x0400054A RID: 1354
			enm_IMono_MBNetwork_reset_debug_variables,
			// Token: 0x0400054B RID: 1355
			enm_IMono_MBNetwork_reset_mission_data,
			// Token: 0x0400054C RID: 1356
			enm_IMono_MBNetwork_server_ping,
			// Token: 0x0400054D RID: 1357
			enm_IMono_MBNetwork_set_server_bandwidth_limit_in_mbps,
			// Token: 0x0400054E RID: 1358
			enm_IMono_MBNetwork_set_server_frame_rate,
			// Token: 0x0400054F RID: 1359
			enm_IMono_MBNetwork_set_server_tick_rate,
			// Token: 0x04000550 RID: 1360
			enm_IMono_MBNetwork_terminate_client_side,
			// Token: 0x04000551 RID: 1361
			enm_IMono_MBNetwork_terminate_server_side,
			// Token: 0x04000552 RID: 1362
			enm_IMono_MBNetwork_write_byte_array_to_packet,
			// Token: 0x04000553 RID: 1363
			enm_IMono_MBNetwork_write_float_to_packet,
			// Token: 0x04000554 RID: 1364
			enm_IMono_MBNetwork_write_int_to_packet,
			// Token: 0x04000555 RID: 1365
			enm_IMono_MBNetwork_write_long_to_packet,
			// Token: 0x04000556 RID: 1366
			enm_IMono_MBNetwork_write_string_to_packet,
			// Token: 0x04000557 RID: 1367
			enm_IMono_MBNetwork_write_uint_to_packet,
			// Token: 0x04000558 RID: 1368
			enm_IMono_MBNetwork_write_ulong_to_packet,
			// Token: 0x04000559 RID: 1369
			enm_IMono_MBPeer_begin_module_event,
			// Token: 0x0400055A RID: 1370
			enm_IMono_MBPeer_debug_refresh_disconnect_timeout,
			// Token: 0x0400055B RID: 1371
			enm_IMono_MBPeer_end_module_event,
			// Token: 0x0400055C RID: 1372
			enm_IMono_MBPeer_get_average_loss_percent,
			// Token: 0x0400055D RID: 1373
			enm_IMono_MBPeer_get_average_ping_in_milliseconds,
			// Token: 0x0400055E RID: 1374
			enm_IMono_MBPeer_get_host,
			// Token: 0x0400055F RID: 1375
			enm_IMono_MBPeer_get_is_synchronized,
			// Token: 0x04000560 RID: 1376
			enm_IMono_MBPeer_get_port,
			// Token: 0x04000561 RID: 1377
			enm_IMono_MBPeer_get_reversed_host,
			// Token: 0x04000562 RID: 1378
			enm_IMono_MBPeer_is_active,
			// Token: 0x04000563 RID: 1379
			enm_IMono_MBPeer_send_existing_objects,
			// Token: 0x04000564 RID: 1380
			enm_IMono_MBPeer_set_controlled_agent,
			// Token: 0x04000565 RID: 1381
			enm_IMono_MBPeer_set_is_synchronized,
			// Token: 0x04000566 RID: 1382
			enm_IMono_MBPeer_set_relevant_game_options,
			// Token: 0x04000567 RID: 1383
			enm_IMono_MBPeer_set_team,
			// Token: 0x04000568 RID: 1384
			enm_IMono_MBPeer_set_user_data,
			// Token: 0x04000569 RID: 1385
			enm_IMono_MBScreen_on_edit_mode_enter_press,
			// Token: 0x0400056A RID: 1386
			enm_IMono_MBScreen_on_edit_mode_enter_release,
			// Token: 0x0400056B RID: 1387
			enm_IMono_MBScreen_on_exit_button_click,
			// Token: 0x0400056C RID: 1388
			enm_IMono_MBSkeletonExtensions_create_agent_skeleton,
			// Token: 0x0400056D RID: 1389
			enm_IMono_MBSkeletonExtensions_create_simple_skeleton,
			// Token: 0x0400056E RID: 1390
			enm_IMono_MBSkeletonExtensions_create_with_action_set,
			// Token: 0x0400056F RID: 1391
			enm_IMono_MBSkeletonExtensions_does_action_continue_with_current_action_at_channel,
			// Token: 0x04000570 RID: 1392
			enm_IMono_MBSkeletonExtensions_get_action_at_channel,
			// Token: 0x04000571 RID: 1393
			enm_IMono_MBSkeletonExtensions_get_bone_entitial_frame,
			// Token: 0x04000572 RID: 1394
			enm_IMono_MBSkeletonExtensions_get_bone_entitial_frame_at_animation_progress,
			// Token: 0x04000573 RID: 1395
			enm_IMono_MBSkeletonExtensions_get_skeleton_face_animation_name,
			// Token: 0x04000574 RID: 1396
			enm_IMono_MBSkeletonExtensions_get_skeleton_face_animation_time,
			// Token: 0x04000575 RID: 1397
			enm_IMono_MBSkeletonExtensions_set_agent_action_channel,
			// Token: 0x04000576 RID: 1398
			enm_IMono_MBSkeletonExtensions_set_animation_at_channel,
			// Token: 0x04000577 RID: 1399
			enm_IMono_MBSkeletonExtensions_set_facial_animation_of_channel,
			// Token: 0x04000578 RID: 1400
			enm_IMono_MBSkeletonExtensions_set_skeleton_face_animation_time,
			// Token: 0x04000579 RID: 1401
			enm_IMono_MBSkeletonExtensions_tick_action_channels,
			// Token: 0x0400057A RID: 1402
			enm_IMono_MBSoundEvent_create_event_from_external_file,
			// Token: 0x0400057B RID: 1403
			enm_IMono_MBSoundEvent_create_event_from_sound_buffer,
			// Token: 0x0400057C RID: 1404
			enm_IMono_MBSoundEvent_play_sound,
			// Token: 0x0400057D RID: 1405
			enm_IMono_MBSoundEvent_play_sound_with_int_param,
			// Token: 0x0400057E RID: 1406
			enm_IMono_MBSoundEvent_play_sound_with_param,
			// Token: 0x0400057F RID: 1407
			enm_IMono_MBSoundEvent_play_sound_with_str_param,
			// Token: 0x04000580 RID: 1408
			enm_IMono_MBTeam_is_enemy,
			// Token: 0x04000581 RID: 1409
			enm_IMono_MBTeam_set_is_enemy,
			// Token: 0x04000582 RID: 1410
			enm_IMono_MBTestRun_auto_continue,
			// Token: 0x04000583 RID: 1411
			enm_IMono_MBTestRun_close_scene,
			// Token: 0x04000584 RID: 1412
			enm_IMono_MBTestRun_enter_edit_mode,
			// Token: 0x04000585 RID: 1413
			enm_IMono_MBTestRun_get_fps,
			// Token: 0x04000586 RID: 1414
			enm_IMono_MBTestRun_leave_edit_mode,
			// Token: 0x04000587 RID: 1415
			enm_IMono_MBTestRun_new_scene,
			// Token: 0x04000588 RID: 1416
			enm_IMono_MBTestRun_open_default_scene,
			// Token: 0x04000589 RID: 1417
			enm_IMono_MBTestRun_open_scene,
			// Token: 0x0400058A RID: 1418
			enm_IMono_MBTestRun_save_scene,
			// Token: 0x0400058B RID: 1419
			enm_IMono_MBTestRun_start_mission,
			// Token: 0x0400058C RID: 1420
			enm_IMono_MBVoiceManager_get_voice_definition_count_with_monster_sound_and_collision_info_class_name,
			// Token: 0x0400058D RID: 1421
			enm_IMono_MBVoiceManager_get_voice_definitions_with_monster_sound_and_collision_info_class_name,
			// Token: 0x0400058E RID: 1422
			enm_IMono_MBVoiceManager_get_voice_type_index,
			// Token: 0x0400058F RID: 1423
			enm_IMono_MBWindowManager_dont_change_cursor_pos,
			// Token: 0x04000590 RID: 1424
			enm_IMono_MBWindowManager_erase_message_lines,
			// Token: 0x04000591 RID: 1425
			enm_IMono_MBWindowManager_get_screen_resolution,
			// Token: 0x04000592 RID: 1426
			enm_IMono_MBWindowManager_pre_display,
			// Token: 0x04000593 RID: 1427
			enm_IMono_MBWindowManager_screen_to_world,
			// Token: 0x04000594 RID: 1428
			enm_IMono_MBWindowManager_world_to_screen,
			// Token: 0x04000595 RID: 1429
			enm_IMono_MBWindowManager_world_to_screen_with_fixed_z,
			// Token: 0x04000596 RID: 1430
			enm_IMono_MBWorld_check_resource_modifications,
			// Token: 0x04000597 RID: 1431
			enm_IMono_MBWorld_fix_skeletons,
			// Token: 0x04000598 RID: 1432
			enm_IMono_MBWorld_get_game_type,
			// Token: 0x04000599 RID: 1433
			enm_IMono_MBWorld_get_global_time,
			// Token: 0x0400059A RID: 1434
			enm_IMono_MBWorld_get_last_messages,
			// Token: 0x0400059B RID: 1435
			enm_IMono_MBWorld_pause_game,
			// Token: 0x0400059C RID: 1436
			enm_IMono_MBWorld_set_body_used,
			// Token: 0x0400059D RID: 1437
			enm_IMono_MBWorld_set_game_type,
			// Token: 0x0400059E RID: 1438
			enm_IMono_MBWorld_set_material_used,
			// Token: 0x0400059F RID: 1439
			enm_IMono_MBWorld_set_mesh_used,
			// Token: 0x040005A0 RID: 1440
			enm_IMono_MBWorld_unpause_game
		}
	}
}
