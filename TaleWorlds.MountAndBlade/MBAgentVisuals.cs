using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A3 RID: 419
	[EngineClass("Agent_visuals")]
	public sealed class MBAgentVisuals : NativeObject
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x000523B2 File Offset: 0x000505B2
		internal MBAgentVisuals(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x000523C1 File Offset: 0x000505C1
		private UIntPtr GetPtr()
		{
			return base.Pointer;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000523C9 File Offset: 0x000505C9
		public static MBAgentVisuals CreateAgentVisuals(Scene scene, string ownerName, Vec3 eyeOffset)
		{
			return MBAPI.IMBAgentVisuals.CreateAgentVisuals(scene.Pointer, ownerName, eyeOffset);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x000523DD File Offset: 0x000505DD
		public void Tick(MBAgentVisuals parentAgentVisuals, float dt, bool entityMoving, float speed)
		{
			MBAPI.IMBAgentVisuals.Tick(this.GetPtr(), (parentAgentVisuals != null) ? parentAgentVisuals.GetPtr() : UIntPtr.Zero, dt, entityMoving, speed);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00052404 File Offset: 0x00050604
		public MatrixFrame GetGlobalFrame()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetGlobalFrame(this.GetPtr(), ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0005242C File Offset: 0x0005062C
		public MatrixFrame GetFrame()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetFrame(this.GetPtr(), ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00052454 File Offset: 0x00050654
		public GameEntity GetEntity()
		{
			return MBAPI.IMBAgentVisuals.GetEntity(this.GetPtr());
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00052466 File Offset: 0x00050666
		public WeakGameEntity GetWeakEntity()
		{
			return new WeakGameEntity(MBAPI.IMBAgentVisuals.GetEntityPointer(this.GetPtr()));
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0005247D File Offset: 0x0005067D
		public bool IsValid()
		{
			return MBAPI.IMBAgentVisuals.IsValid(this.GetPtr());
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0005248F File Offset: 0x0005068F
		public Vec3 GetGlobalStableEyePoint(bool isHumanoid)
		{
			return MBAPI.IMBAgentVisuals.GetGlobalStableEyePoint(this.GetPtr(), isHumanoid);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000524A2 File Offset: 0x000506A2
		public Vec3 GetGlobalStableNeckPoint(bool isHumanoid)
		{
			return MBAPI.IMBAgentVisuals.GetGlobalStableNeckPoint(this.GetPtr(), isHumanoid);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000524B8 File Offset: 0x000506B8
		public MatrixFrame GetBoneEntitialFrame(sbyte bone, bool useBoneMapping)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetBoneEntitialFrame(base.Pointer, bone, useBoneMapping, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x000524E2 File Offset: 0x000506E2
		public void SetAttachedPositionForMeshAfterAnimationPostIntegrate(WeakGameEntity ropeEntity, sbyte bone)
		{
			MBAPI.IMBAgentVisuals.SetAttachedPositionForRopeEntityAfterAnimationPostIntegrate(base.Pointer, ropeEntity.Pointer, bone);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000524FC File Offset: 0x000506FC
		public Vec3 GetCurrentHeadLookDirection()
		{
			return MBAPI.IMBAgentVisuals.GetCurrentHeadLookDirection(base.Pointer);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0005250E File Offset: 0x0005070E
		public HumanWalkingMovementMode GetMovementMode()
		{
			return (HumanWalkingMovementMode)MBAPI.IMBAgentVisuals.GetMovementMode(base.Pointer);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00052520 File Offset: 0x00050720
		public float GetVisualStrengthOfAgentVisual(MBAgentVisuals targetAgentVisual, Mission mission, float ambientLightStrength, float sunMoonLightStrength, int agentIndexToIgnore)
		{
			return MBAPI.IMBAgentVisuals.GetVisualStrengthOfAgentVisual(base.Pointer, targetAgentVisual.Pointer, mission.Pointer, ambientLightStrength, sunMoonLightStrength, agentIndexToIgnore);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00052543 File Offset: 0x00050743
		public RagdollState GetCurrentRagdollState()
		{
			return MBAPI.IMBAgentVisuals.GetCurrentRagdollState(base.Pointer);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00052555 File Offset: 0x00050755
		public sbyte GetRealBoneIndex(HumanBone boneType)
		{
			return MBAPI.IMBAgentVisuals.GetRealBoneIndex(this.GetPtr(), boneType);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00052568 File Offset: 0x00050768
		public CompositeComponent AddPrefabToAgentVisualBoneByBoneType(string prefabName, HumanBone boneType)
		{
			return MBAPI.IMBAgentVisuals.AddPrefabToAgentVisualBoneByBoneType(this.GetPtr(), prefabName, boneType);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0005257C File Offset: 0x0005077C
		public CompositeComponent AddPrefabToAgentVisualBoneByRealBoneIndex(string prefabName, sbyte realBoneIndex)
		{
			return MBAPI.IMBAgentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndex(this.GetPtr(), prefabName, realBoneIndex);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00052590 File Offset: 0x00050790
		public GameEntity GetAttachedWeaponEntity(int attachedWeaponIndex)
		{
			return MBAPI.IMBAgentVisuals.GetAttachedWeaponEntity(this.GetPtr(), attachedWeaponIndex);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000525A3 File Offset: 0x000507A3
		public void SetFrame(ref MatrixFrame frame)
		{
			MBAPI.IMBAgentVisuals.SetFrame(this.GetPtr(), ref frame);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000525B6 File Offset: 0x000507B6
		public void SetEntity(GameEntity value)
		{
			MBAPI.IMBAgentVisuals.SetEntity(this.GetPtr(), value.Pointer);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000525CE File Offset: 0x000507CE
		public static void FillEntityWithBodyMeshesWithoutAgentVisuals(GameEntity entity, SkinGenerationParams skinParams, BodyProperties bodyProperties, MetaMesh glovesMesh)
		{
			MBAPI.IMBAgentVisuals.FillEntityWithBodyMeshesWithoutAgentVisuals(entity.Pointer, ref skinParams, ref bodyProperties, glovesMesh);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000525E8 File Offset: 0x000507E8
		public BoneBodyTypeData GetBoneTypeData(sbyte boneIndex)
		{
			BoneBodyTypeData boneBodyTypeData = default(BoneBodyTypeData);
			MBAPI.IMBAgentVisuals.GetBoneTypeData(base.Pointer, boneIndex, ref boneBodyTypeData);
			return boneBodyTypeData;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00052611 File Offset: 0x00050811
		public Skeleton GetSkeleton()
		{
			return MBAPI.IMBAgentVisuals.GetSkeleton(this.GetPtr());
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00052623 File Offset: 0x00050823
		public void SetSkeleton(Skeleton newSkeleton)
		{
			MBAPI.IMBAgentVisuals.SetSkeleton(this.GetPtr(), newSkeleton.Pointer);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0005263C File Offset: 0x0005083C
		public void CreateParticleSystemAttachedToBone(string particleName, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame)
		{
			int runtimeIdByName = ParticleSystemManager.GetRuntimeIdByName(particleName);
			this.CreateParticleSystemAttachedToBone(runtimeIdByName, boneIndex, ref boneLocalParticleFrame);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00052659 File Offset: 0x00050859
		public void CreateParticleSystemAttachedToBone(int runtimeParticleindex, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame)
		{
			MBAPI.IMBAgentVisuals.CreateParticleSystemAttachedToBone(this.GetPtr(), runtimeParticleindex, boneIndex, ref boneLocalParticleFrame);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0005266E File Offset: 0x0005086E
		public void SetVisible(bool value)
		{
			MBAPI.IMBAgentVisuals.SetVisible(this.GetPtr(), value);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00052681 File Offset: 0x00050881
		public bool GetVisible()
		{
			return MBAPI.IMBAgentVisuals.GetVisible(this.GetPtr());
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00052693 File Offset: 0x00050893
		public void AddChildEntity(GameEntity entity)
		{
			MBAPI.IMBAgentVisuals.AddChildEntity(this.GetPtr(), entity.Pointer);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x000526AC File Offset: 0x000508AC
		public void SetClothWindToWeaponAtIndex(Vec3 windVector, bool isLocal, EquipmentIndex weaponIndex)
		{
			MBAPI.IMBAgentVisuals.SetClothWindToWeaponAtIndex(this.GetPtr(), windVector, isLocal, (int)weaponIndex);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000526C1 File Offset: 0x000508C1
		public void RemoveChildEntity(GameEntity entity, int removeReason)
		{
			MBAPI.IMBAgentVisuals.RemoveChildEntity(this.GetPtr(), entity.Pointer, removeReason);
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000526DA File Offset: 0x000508DA
		public bool CheckResources(bool addToQueue)
		{
			return MBAPI.IMBAgentVisuals.CheckResources(this.GetPtr(), addToQueue);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x000526ED File Offset: 0x000508ED
		public void AddSkinMeshes(SkinGenerationParams skinParams, BodyProperties bodyProperties, bool useGPUMorph, bool useFaceCache)
		{
			MBAPI.IMBAgentVisuals.AddSkinMeshesToAgentEntity(this.GetPtr(), ref skinParams, ref bodyProperties, useGPUMorph, useFaceCache);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00052706 File Offset: 0x00050906
		public void SetFaceGenerationParams(FaceGenerationParams faceGenerationParams)
		{
			MBAPI.IMBAgentVisuals.SetFaceGenerationParams(this.GetPtr(), faceGenerationParams);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00052719 File Offset: 0x00050919
		public void SetLodAtlasShadingIndex(int index, bool useTeamColor, uint teamColor1, uint teamColor2)
		{
			MBAPI.IMBAgentVisuals.SetLodAtlasShadingIndex(this.GetPtr(), index, useTeamColor, teamColor1, teamColor2);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00052730 File Offset: 0x00050930
		public void ClearVisualComponents(bool removeSkeleton, bool removeLabel = true)
		{
			MBAPI.IMBAgentVisuals.ClearVisualComponents(this.GetPtr(), removeSkeleton, removeLabel);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00052744 File Offset: 0x00050944
		public void LazyUpdateAgentRendererData()
		{
			MBAPI.IMBAgentVisuals.LazyUpdateAgentRendererData(this.GetPtr());
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00052756 File Offset: 0x00050956
		public void AddMultiMesh(MetaMesh metaMesh, BodyMeshTypes bodyMeshIndex)
		{
			MBAPI.IMBAgentVisuals.AddMultiMesh(this.GetPtr(), metaMesh.Pointer, (int)bodyMeshIndex);
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0005276F File Offset: 0x0005096F
		public void ApplySkeletonScale(Vec3 mountSitBoneScale, float mountRadiusAdder, sbyte[] boneIndices, Vec3[] boneScales)
		{
			MBAPI.IMBAgentVisuals.ApplySkeletonScale(base.Pointer, mountSitBoneScale, mountRadiusAdder, (byte)boneIndices.Length, boneIndices, boneScales);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x0005278A File Offset: 0x0005098A
		public void UpdateSkeletonScale(int bodyDeformType)
		{
			MBAPI.IMBAgentVisuals.UpdateSkeletonScale(base.Pointer, bodyDeformType);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x0005279D File Offset: 0x0005099D
		public void AddHorseReinsClothMesh(MetaMesh reinMesh, MetaMesh ropeMesh)
		{
			MBAPI.IMBAgentVisuals.AddHorseReinsClothMesh(base.Pointer, reinMesh.Pointer, ropeMesh.Pointer);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000527BB File Offset: 0x000509BB
		public void BatchLastLodMeshes()
		{
			MBAPI.IMBAgentVisuals.BatchLastLodMeshes(this.GetPtr());
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000527D0 File Offset: 0x000509D0
		public void AddWeaponToAgentEntity(int slotIndex, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, GameEntity cachedEntity)
		{
			MBAPI.IMBAgentVisuals.AddWeaponToAgentEntity(this.GetPtr(), slotIndex, in weaponData, weaponStatsData, weaponStatsData.Length, in ammoWeaponData, ammoWeaponStatsData, ammoWeaponStatsData.Length, cachedEntity);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000527FD File Offset: 0x000509FD
		public void UpdateQuiverMeshesWithoutAgent(int weaponIndex, int ammoCount)
		{
			MBAPI.IMBAgentVisuals.UpdateQuiverMeshesWithoutAgent(this.GetPtr(), weaponIndex, ammoCount);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00052811 File Offset: 0x00050A11
		public void SetWieldedWeaponIndices(int slotIndexRightHand, int slotIndexLeftHand)
		{
			MBAPI.IMBAgentVisuals.SetWieldedWeaponIndices(this.GetPtr(), slotIndexRightHand, slotIndexLeftHand);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00052825 File Offset: 0x00050A25
		public void ClearAllWeaponMeshes()
		{
			MBAPI.IMBAgentVisuals.ClearAllWeaponMeshes(this.GetPtr());
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00052837 File Offset: 0x00050A37
		public void ClearWeaponMeshes(EquipmentIndex index)
		{
			MBAPI.IMBAgentVisuals.ClearWeaponMeshes(this.GetPtr(), (int)index);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0005284A File Offset: 0x00050A4A
		public void MakeVoice(int voiceId, Vec3 position)
		{
			MBAPI.IMBAgentVisuals.MakeVoice(this.GetPtr(), voiceId, ref position);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0005285F File Offset: 0x00050A5F
		public void SetSetupMorphNode(bool value)
		{
			MBAPI.IMBAgentVisuals.SetSetupMorphNode(this.GetPtr(), value);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00052872 File Offset: 0x00050A72
		public void UseScaledWeapons(bool value)
		{
			MBAPI.IMBAgentVisuals.UseScaledWeapons(this.GetPtr(), value);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00052885 File Offset: 0x00050A85
		public void SetClothComponentKeepStateOfAllMeshes(bool keepState)
		{
			MBAPI.IMBAgentVisuals.SetClothComponentKeepStateOfAllMeshes(this.GetPtr(), keepState);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00052898 File Offset: 0x00050A98
		public MatrixFrame GetFacegenScalingMatrix()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			Vec3 currentHelmetScalingFactor = MBAPI.IMBAgentVisuals.GetCurrentHelmetScalingFactor(this.GetPtr());
			identity.rotation.ApplyScaleLocal(in currentHelmetScalingFactor);
			return identity;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x000528CC File Offset: 0x00050ACC
		public void ReplaceMeshWithMesh(MetaMesh oldMetaMesh, MetaMesh newMetaMesh, BodyMeshTypes bodyMeshIndex)
		{
			if (oldMetaMesh != null)
			{
				MBAPI.IMBAgentVisuals.RemoveMultiMesh(this.GetPtr(), oldMetaMesh.Pointer, (int)bodyMeshIndex);
			}
			if (newMetaMesh != null)
			{
				MBAPI.IMBAgentVisuals.AddMultiMesh(this.GetPtr(), newMetaMesh.Pointer, (int)bodyMeshIndex);
			}
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00052919 File Offset: 0x00050B19
		public void SetAgentActionChannel(int actionChannelNo, int actionIndex, float channelParameter = 0f, float blendPeriodOverride = -0.2f, bool forceFaceMorphRestart = true, float blendWithNextActionFactor = 0f)
		{
			MBAPI.IMBSkeletonExtensions.SetAgentActionChannel(this.GetSkeleton().Pointer, actionChannelNo, actionIndex, channelParameter, blendPeriodOverride, forceFaceMorphRestart, blendWithNextActionFactor);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00052939 File Offset: 0x00050B39
		public void SetVoiceDefinitionIndex(int voiceDefinitionIndex, float voicePitch)
		{
			MBAPI.IMBAgentVisuals.SetVoiceDefinitionIndex(this.GetPtr(), voiceDefinitionIndex, voicePitch);
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0005294D File Offset: 0x00050B4D
		public void StartRhubarbRecord(string path, int soundId)
		{
			MBAPI.IMBAgentVisuals.StartRhubarbRecord(this.GetPtr(), path, soundId);
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00052964 File Offset: 0x00050B64
		public void SetContourColor(uint? color, bool alwaysVisible = true)
		{
			if (color != null)
			{
				MBAPI.IMBAgentVisuals.SetAsContourEntity(this.GetPtr(), color.Value);
				MBAPI.IMBAgentVisuals.SetContourState(this.GetPtr(), alwaysVisible);
				return;
			}
			MBAPI.IMBAgentVisuals.DisableContour(this.GetPtr());
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x000529B3 File Offset: 0x00050BB3
		public void SetEnableOcclusionCulling(bool enable)
		{
			MBAPI.IMBAgentVisuals.SetEnableOcclusionCulling(this.GetPtr(), enable);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x000529C6 File Offset: 0x00050BC6
		public void SetAgentLodZeroOrMax(bool makeZero)
		{
			MBAPI.IMBAgentVisuals.SetAgentLodMakeZeroOrMax(this.GetPtr(), makeZero);
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x000529D9 File Offset: 0x00050BD9
		public void SetAgentLocalSpeed(Vec2 speed)
		{
			MBAPI.IMBAgentVisuals.SetAgentLocalSpeed(this.GetPtr(), speed);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x000529EC File Offset: 0x00050BEC
		public void SetLookDirection(Vec3 direction)
		{
			MBAPI.IMBAgentVisuals.SetLookDirection(this.GetPtr(), direction);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00052A00 File Offset: 0x00050C00
		public static BodyMeshTypes GetBodyMeshIndex(EquipmentIndex equipmentIndex)
		{
			switch (equipmentIndex)
			{
			case EquipmentIndex.NumAllWeaponSlots:
				return BodyMeshTypes.Cap;
			case EquipmentIndex.Body:
				return BodyMeshTypes.Chestpiece;
			case EquipmentIndex.Leg:
				return BodyMeshTypes.Footwear;
			case EquipmentIndex.Gloves:
				return BodyMeshTypes.Gloves;
			case EquipmentIndex.Cape:
				return BodyMeshTypes.Shoulderpiece;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Base\\MBAgentVisuals.cs", "GetBodyMeshIndex", 434);
				return BodyMeshTypes.Invalid;
			}
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00052A52 File Offset: 0x00050C52
		public MatrixFrame GetBoneEntitialFrameAtAnimationProgress(sbyte boneIndex, int animationIndex, float progress)
		{
			return MBAPI.IMBAgentVisuals.GetBoneEntitialFrameAtAnimationProgress(this.GetPtr(), boneIndex, animationIndex, progress);
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00052A67 File Offset: 0x00050C67
		public void Reset()
		{
			MBAPI.IMBAgentVisuals.Reset(this.GetPtr());
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00052A79 File Offset: 0x00050C79
		public void ResetNextFrame()
		{
			MBAPI.IMBAgentVisuals.ResetNextFrame(this.GetPtr());
		}
	}
}
