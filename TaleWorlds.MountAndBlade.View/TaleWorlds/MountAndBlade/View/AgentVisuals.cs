using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200000E RID: 14
	public class AgentVisuals : IAgentVisual
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000253C File Offset: 0x0000073C
		public bool IsFemale
		{
			get
			{
				return this._data.SkeletonTypeData == SkeletonType.Female || this._data.SkeletonTypeData == SkeletonType.KidFemale1 || this._data.SkeletonTypeData == SkeletonType.KidFemale2 || this._data.SkeletonTypeData == SkeletonType.KidFemale3;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002578 File Offset: 0x00000778
		public MBAgentVisuals GetVisuals()
		{
			return this._data.AgentVisuals;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002585 File Offset: 0x00000785
		public void Reset()
		{
			this._data.AgentVisuals.Reset();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002597 File Offset: 0x00000797
		public void ResetNextFrame()
		{
			this._data.AgentVisuals.ResetNextFrame();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000025A9 File Offset: 0x000007A9
		public MatrixFrame GetFrame()
		{
			return this._data.FrameData;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000025B6 File Offset: 0x000007B6
		public BodyProperties GetBodyProperties()
		{
			return this._data.BodyPropertiesData;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000025C3 File Offset: 0x000007C3
		public void SetBodyProperties(BodyProperties bodyProperties)
		{
			this._data.BodyProperties(bodyProperties);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000025D2 File Offset: 0x000007D2
		public bool GetIsFemale()
		{
			return this.IsFemale;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000025DA File Offset: 0x000007DA
		public string GetCharacterObjectID()
		{
			return this._data.CharacterObjectStringIdData;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000025E7 File Offset: 0x000007E7
		public void SetCharacterObjectID(string id)
		{
			this._data.CharacterObjectStringId(id);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000025F6 File Offset: 0x000007F6
		public Equipment GetEquipment()
		{
			return this._data.EquipmentData;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002604 File Offset: 0x00000804
		private AgentVisuals(AgentVisualsData data, string name, bool isRandomProgress, bool needBatchedVersionForWeaponMeshes, bool forceUseFaceCache)
		{
			this._data = data;
			this._data.AgentVisuals = MBAgentVisuals.CreateAgentVisuals(this._data.SceneData, name, data.MonsterData.EyeOffsetWrtHead);
			if (data.EntityData != null)
			{
				this._data.AgentVisuals.SetEntity(data.EntityData);
			}
			this._scale = ((this._data.ScaleData <= 1E-05f) ? 1f : this._data.ScaleData);
			this.Refresh(needBatchedVersionForWeaponMeshes, false, null, isRandomProgress, forceUseFaceCache);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000026A0 File Offset: 0x000008A0
		public AgentVisualsData GetCopyAgentVisualsData()
		{
			return new AgentVisualsData(this._data);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000026AD File Offset: 0x000008AD
		public GameEntity GetEntity()
		{
			return this._data.AgentVisuals.GetEntity();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000026BF File Offset: 0x000008BF
		public WeakGameEntity GetWeakEntity()
		{
			return this._data.AgentVisuals.GetWeakEntity();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000026D1 File Offset: 0x000008D1
		public void SetVisible(bool value)
		{
			this._data.AgentVisuals.SetVisible(value);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000026E4 File Offset: 0x000008E4
		public Vec3 GetGlobalStableEyePoint(bool isHumanoid)
		{
			return this._data.AgentVisuals.GetGlobalStableEyePoint(isHumanoid);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000026F7 File Offset: 0x000008F7
		public Vec3 GetGlobalStableNeckPoint(bool isHumanoid)
		{
			return this._data.AgentVisuals.GetGlobalStableNeckPoint(isHumanoid);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000270A File Offset: 0x0000090A
		public CompositeComponent AddPrefabToAgentVisualBoneByBoneType(string prefabName, HumanBone boneType)
		{
			return this._data.AgentVisuals.AddPrefabToAgentVisualBoneByBoneType(prefabName, boneType);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000271E File Offset: 0x0000091E
		public CompositeComponent AddPrefabToAgentVisualBoneByRealBoneIndex(string prefabName, sbyte realBoneIndex)
		{
			return this._data.AgentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndex(prefabName, realBoneIndex);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002732 File Offset: 0x00000932
		public void SetAgentLodZeroOrMax(bool value)
		{
			this._data.AgentVisuals.SetAgentLodZeroOrMax(value);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002745 File Offset: 0x00000945
		public float GetScale()
		{
			return this._scale;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002750 File Offset: 0x00000950
		public void SetAction(in ActionIndexCache actionIndex, float startProgress = 0f, bool forceFaceMorphRestart = true)
		{
			if (this._data.AgentVisuals != null)
			{
				Skeleton skeleton = this._data.AgentVisuals.GetSkeleton();
				if (skeleton != null)
				{
					skeleton.SetAgentActionChannel(0, in actionIndex, startProgress, -0.2f, forceFaceMorphRestart, 0f);
					skeleton.ManualInvalidate();
				}
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000027A4 File Offset: 0x000009A4
		public bool DoesActionContinueWithCurrentAction(in ActionIndexCache actionIndex)
		{
			bool flag = false;
			if (this._data.AgentVisuals != null)
			{
				Skeleton skeleton = this._data.AgentVisuals.GetSkeleton();
				if (skeleton != null)
				{
					flag = skeleton.DoesActionContinueWithCurrentActionAtChannel(0, in actionIndex);
				}
			}
			return flag;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000027EC File Offset: 0x000009EC
		public float GetAnimationParameterAtChannel(int channelIndex)
		{
			float num = 0f;
			if (this._data.AgentVisuals != null && this._data.AgentVisuals.GetSkeleton() != null)
			{
				num = this._data.AgentVisuals.GetSkeleton().GetAnimationParameterAtChannel(channelIndex);
			}
			return num;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002844 File Offset: 0x00000A44
		public void Refresh(bool needBatchedVersionForWeaponMeshes, AgentVisualsData data, bool forceUseFaceCache = false)
		{
			AgentVisualsData data2 = this._data;
			this._data = data;
			bool flag = data2.SkeletonTypeData != this._data.SkeletonTypeData;
			Equipment equipmentData = this._data.EquipmentData;
			this.Refresh(needBatchedVersionForWeaponMeshes, flag, equipmentData, false, forceUseFaceCache);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000288B File Offset: 0x00000A8B
		public void SetClothWindToWeaponAtIndex(Vec3 localWindVector, bool isLocal, EquipmentIndex weaponIndex)
		{
			this._data.AgentVisuals.SetClothWindToWeaponAtIndex(localWindVector, isLocal, weaponIndex);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000028A0 File Offset: 0x00000AA0
		private void Refresh(bool needBatchedVersionForWeaponMeshes, bool removeSkeleton = false, Equipment oldEquipment = null, bool isRandomProgress = false, bool forceUseFaceCache = false)
		{
			float num = 0f;
			float num2 = 0f;
			string text = "";
			bool flag = this._data.MonsterData.Flags.HasAnyFlag(AgentFlag.IsHumanoid);
			Skeleton skeleton = this._data.AgentVisuals.GetSkeleton();
			float num3 = -0.2f;
			ActionIndexCache actionIndexCache;
			if (skeleton != null && this._data.ActionSetData.IsValid)
			{
				num = skeleton.GetAnimationParameterAtChannel(0);
				actionIndexCache = skeleton.GetActionAtChannel(0);
				num3 = 0f;
				if (flag)
				{
					num2 = MBSkeletonExtensions.GetSkeletonFaceAnimationTime(skeleton);
					text = MBSkeletonExtensions.GetSkeletonFaceAnimationName(skeleton);
				}
			}
			else
			{
				actionIndexCache = this._data.ActionCodeData;
			}
			if (skeleton != null)
			{
				skeleton.ManualInvalidate();
			}
			this._data.AgentVisuals.SetSetupMorphNode(this._data.UseMorphAnimsData);
			this._data.AgentVisuals.UseScaledWeapons(this._data.UseScaledWeaponsData);
			MatrixFrame frameData = this._data.FrameData;
			this._scale = ((this._data.ScaleData == 0f) ? MBBodyProperties.GetScaleFromKey(this._data.RaceData, this.IsFemale ? 1 : 0, this._data.BodyPropertiesData) : this._data.ScaleData);
			frameData.rotation.ApplyScaleLocal(this._scale);
			this._data.AgentVisuals.SetFrame(ref frameData);
			object obj = !removeSkeleton && skeleton != null && oldEquipment != null;
			bool flag2 = false;
			object obj2 = obj;
			if (obj2 != null)
			{
				flag2 = this.ClearAndAddChangedVisualComponentsOfWeapons(oldEquipment, needBatchedVersionForWeaponMeshes);
			}
			if (obj2 == null || !flag2)
			{
				this._data.AgentVisuals.ClearVisualComponents(false, false);
				if (this._data.ActionSetData.IsValid && text != "facegen_teeth")
				{
					AnimationSystemData animationSystemData = this._data.MonsterData.FillAnimationSystemData(this._data.ActionSetData, 1f, this._data.HasClippingPlaneData);
					Skeleton skeleton2 = MBSkeletonExtensions.CreateWithActionSet(ref animationSystemData);
					this._data.AgentVisuals.SetSkeleton(skeleton2);
					skeleton2.ManualInvalidate();
				}
				if (this._data.EquipmentData == null)
				{
					int num4 = 481;
					this.AddSkinMeshesToEntity(num4, !needBatchedVersionForWeaponMeshes, forceUseFaceCache);
				}
				else if (!string.IsNullOrEmpty(this._data.MountCreationKeyData) || !flag)
				{
					MountVisualCreationOutput mountVisualCreationOutput;
					MountVisualCreator.AddMountMeshToEntity(this.GetEntity(), this._data.EquipmentData[EquipmentIndex.ArmorItemEndSlot].Item, this._data.EquipmentData[EquipmentIndex.HorseHarness].Item, this._data.MountCreationKeyData, out mountVisualCreationOutput, null);
					ItemObject item = this._data.EquipmentData[EquipmentIndex.HorseHarness].Item;
					if (item != null && item.IsUsingTeamColor && mountVisualCreationOutput.MountHarnessMesh != null)
					{
						AgentVisuals.AddTeamColorToMesh(mountVisualCreationOutput.MountHarnessMesh, this._data.ClothColor1Data, this._data.ClothColor2Data);
					}
				}
				else
				{
					this.AddSkinArmorWeaponMultiMeshesToEntity(this._data.ClothColor1Data, this._data.ClothColor2Data, needBatchedVersionForWeaponMeshes, forceUseFaceCache);
				}
			}
			if (this._data.ActionSetData.IsValid && actionIndexCache != ActionIndexCache.act_none)
			{
				if (isRandomProgress)
				{
					num = MBRandom.RandomFloat;
				}
				skeleton = this._data.AgentVisuals.GetSkeleton();
				if (skeleton != null)
				{
					skeleton.SetAgentActionChannel(0, in actionIndexCache, num, num3, true, 0f);
					if (num2 > 0f)
					{
						MBSkeletonExtensions.SetSkeletonFaceAnimationTime(skeleton, num2);
					}
					skeleton.ManualInvalidate();
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002C44 File Offset: 0x00000E44
		public void TickVisuals()
		{
			if (this._data.ActionSetData.IsValid)
			{
				this._data.AgentVisuals.GetSkeleton().TickActionChannels();
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002C7B File Offset: 0x00000E7B
		public void Tick(AgentVisuals parentAgentVisuals, float dt, bool isEntityMoving = false, float speed = 0f)
		{
			this._data.AgentVisuals.Tick((parentAgentVisuals != null) ? parentAgentVisuals._data.AgentVisuals : null, dt, isEntityMoving, speed);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public static AgentVisuals Create(AgentVisualsData data, string name, bool isRandomProgress, bool needBatchedVersionForWeaponMeshes, bool forceUseFaceCache)
		{
			return new AgentVisuals(data, name, isRandomProgress, needBatchedVersionForWeaponMeshes, forceUseFaceCache);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002CAF File Offset: 0x00000EAF
		public static float GetRandomGlossFactor(Random randomGenerator)
		{
			return 1f + (randomGenerator.NextFloat() * 2f - 1f) * 0.05f;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public static void GetRandomClothingColors(int seed, Color inputColor1, Color inputColor2, out Color color1, out Color color2)
		{
			MBFastRandom mbfastRandom = new MBFastRandom((uint)seed);
			color1 = inputColor1.AddFactorInHSB((2f * mbfastRandom.NextFloat() - 1f) * 4f, (2f * mbfastRandom.NextFloat() - 1f) * 0.2f, (2f * mbfastRandom.NextFloat() - 1f) * 0.2f);
			color2 = inputColor2.AddFactorInHSB((2f * mbfastRandom.NextFloat() - 1f) * 8f, (2f * mbfastRandom.NextFloat() - 1f) * 0.5f, (2f * mbfastRandom.NextFloat() - 1f) * 0.3f);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D90 File Offset: 0x00000F90
		private void AddSkinArmorWeaponMultiMeshesToEntity(uint teamColor1, uint teamColor2, bool needBatchedVersion, bool forceUseFaceCache = false)
		{
			this.AddSkinMeshesToEntity((int)this._data.EquipmentData.GetSkinMeshesMask(), !needBatchedVersion, forceUseFaceCache);
			this.AddArmorMultiMeshesToAgentEntity(teamColor1, teamColor2);
			int hashCode = this._data.BodyPropertiesData.GetHashCode();
			for (int i = 0; i < 5; i++)
			{
				if (!this._data.EquipmentData[i].IsEmpty)
				{
					MissionWeapon missionWeapon = new MissionWeapon(this._data.EquipmentData[i].Item, this._data.EquipmentData[i].ItemModifier, this._data.BannerData);
					if (this._data.AddColorRandomnessData)
					{
						missionWeapon.SetRandomGlossMultiplier(hashCode);
					}
					WeaponData weaponData = missionWeapon.GetWeaponData(needBatchedVersion);
					WeaponData ammoWeaponData = missionWeapon.GetAmmoWeaponData(needBatchedVersion);
					this._data.AgentVisuals.AddWeaponToAgentEntity(i, in weaponData, missionWeapon.GetWeaponStatsData(), in ammoWeaponData, missionWeapon.GetAmmoWeaponStatsData(), this._data.GetCachedWeaponEntity((EquipmentIndex)i));
					weaponData.DeinitializeManagedPointers();
					ammoWeaponData.DeinitializeManagedPointers();
				}
			}
			this._data.AgentVisuals.SetWieldedWeaponIndices(this._data.RightWieldedItemIndexData, this._data.LeftWieldedItemIndexData);
			for (int j = 0; j < 5; j++)
			{
				if (!this._data.EquipmentData[j].IsEmpty && this._data.EquipmentData[j].Item.PrimaryWeapon.IsConsumable)
				{
					short num = this._data.EquipmentData[j].Item.PrimaryWeapon.MaxDataValue;
					if (j == this._data.RightWieldedItemIndexData)
					{
						num -= 1;
					}
					this._data.AgentVisuals.UpdateQuiverMeshesWithoutAgent(j, (int)num);
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002F84 File Offset: 0x00001184
		private void AddSkinMeshesToEntity(int mask, bool useGPUMorph, bool forceUseFaceCache = false)
		{
			SkinGenerationParams skinGenerationParams;
			if (this._data.EquipmentData != null)
			{
				bool flag = this._data.BodyPropertiesData.Age >= 14f && this._data.SkeletonTypeData == SkeletonType.Female;
				skinGenerationParams = new SkinGenerationParams(mask, this._data.EquipmentData.GetUnderwearType(flag), (int)this._data.EquipmentData.BodyMeshType, (int)this._data.EquipmentData.HairCoverType, (int)this._data.EquipmentData.BeardCoverType, (int)this._data.EquipmentData.BodyDeformType, this._data.PrepareImmediatelyData, 0f, (int)this._data.SkeletonTypeData, this._data.RaceData, this._data.UseTranslucencyData, this._data.UseTesselationData, 0);
			}
			else
			{
				skinGenerationParams = new SkinGenerationParams(mask, Equipment.UnderwearTypes.FullUnderwear, 0, 4, 0, 0, this._data.PrepareImmediatelyData, 0f, (int)this._data.SkeletonTypeData, this._data.RaceData, this._data.UseTranslucencyData, this._data.UseTesselationData, 0);
			}
			if (this._data.CharacterObjectStringIdData != null)
			{
				MBObjectManager.Instance.GetObject<BasicCharacterObject>(this._data.CharacterObjectStringIdData);
			}
			this._data.AgentVisuals.AddSkinMeshes(skinGenerationParams, this._data.BodyPropertiesData, useGPUMorph, false);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000030F0 File Offset: 0x000012F0
		public void SetFaceGenerationParams(FaceGenerationParams faceGenerationParams)
		{
			this._data.AgentVisuals.SetFaceGenerationParams(faceGenerationParams);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003103 File Offset: 0x00001303
		public void SetVoiceDefinitionIndex(int voiceDefinitionIndex, float voicePitch)
		{
			this._data.AgentVisuals.SetVoiceDefinitionIndex(voiceDefinitionIndex, voicePitch);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003117 File Offset: 0x00001317
		public void StartRhubarbRecord(string path, int soundId)
		{
			this._data.AgentVisuals.StartRhubarbRecord(path, soundId);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000312B File Offset: 0x0000132B
		public void SetAgentLodZeroOrMaxExternal(bool makeZero)
		{
			this._data.AgentVisuals.SetAgentLodZeroOrMax(makeZero);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000313E File Offset: 0x0000133E
		public void SetAgentLocalSpeed(Vec2 speed)
		{
			this._data.AgentVisuals.SetAgentLocalSpeed(speed);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003151 File Offset: 0x00001351
		public void SetLookDirection(Vec3 direction)
		{
			this._data.AgentVisuals.SetLookDirection(direction);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003164 File Offset: 0x00001364
		public void AddArmorMultiMeshesToAgentEntity(uint teamColor1, uint teamColor2)
		{
			Random random = null;
			uint num;
			uint num2;
			if (this._data.AddColorRandomnessData)
			{
				int hashCode = this._data.BodyPropertiesData.GetHashCode();
				random = new Random(hashCode);
				Color color;
				Color color2;
				AgentVisuals.GetRandomClothingColors(hashCode, Color.FromUint(teamColor1), Color.FromUint(teamColor2), out color, out color2);
				num = color.ToUnsignedInteger();
				num2 = color2.ToUnsignedInteger();
			}
			else
			{
				num = teamColor1;
				num2 = teamColor2;
			}
			for (EquipmentIndex equipmentIndex = EquipmentIndex.HorseHarness; equipmentIndex >= EquipmentIndex.WeaponItemBeginSlot; equipmentIndex--)
			{
				if (equipmentIndex == EquipmentIndex.NumAllWeaponSlots || equipmentIndex == EquipmentIndex.Body || equipmentIndex == EquipmentIndex.Leg || equipmentIndex == EquipmentIndex.Gloves || equipmentIndex == EquipmentIndex.Cape)
				{
					ItemObject item = this._data.EquipmentData[(int)equipmentIndex].Item;
					ItemObject itemObject = this._data.EquipmentData[(int)equipmentIndex].CosmeticItem ?? item;
					if (itemObject != null)
					{
						bool flag = this._data.BodyPropertiesData.Age >= 14f && this._data.SkeletonTypeData == SkeletonType.Female;
						bool flag2 = equipmentIndex == EquipmentIndex.Body && this._data.EquipmentData[EquipmentIndex.Gloves].Item != null && !this._data.EquipmentData[EquipmentIndex.Gloves].Item.ArmorComponent.IsNoSlim;
						MetaMesh multiMesh = this._data.EquipmentData[(int)equipmentIndex].GetMultiMesh(flag, flag2, true);
						if (multiMesh != null)
						{
							if (this._data.AddColorRandomnessData)
							{
								multiMesh.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(random));
							}
							if (itemObject.IsUsingTableau && this._data.BannerData != null)
							{
								for (int i = 0; i < multiMesh.MeshCount; i++)
								{
									Mesh currentMesh = multiMesh.GetMeshAtIndex(i);
									Mesh currentMesh3 = currentMesh;
									if (currentMesh3 != null && !currentMesh3.HasTag("dont_use_tableau"))
									{
										Mesh currentMesh2 = currentMesh;
										if (currentMesh2 != null && currentMesh2.HasTag("banner_replacement_mesh"))
										{
											BannerVisual bannerVisual = (BannerVisual)this._data.BannerData.BannerVisual;
											BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
											bannerVisual.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
											{
												this.ApplyBannerTextureToMesh(currentMesh, t);
											}, true);
											currentMesh.ManualInvalidate();
											break;
										}
									}
									currentMesh.ManualInvalidate();
								}
							}
							else if (itemObject.IsUsingTeamColor)
							{
								AgentVisuals.AddTeamColorToMesh(multiMesh, num, num2);
							}
							if (itemObject.UsingFacegenScaling)
							{
								Skeleton skeleton = this._data.AgentVisuals.GetSkeleton();
								multiMesh.UseHeadBoneFaceGenScaling(skeleton, this._data.MonsterData.HeadLookDirectionBoneIndex, this._data.AgentVisuals.GetFacegenScalingMatrix());
								skeleton.ManualInvalidate();
							}
							this._data.AgentVisuals.AddMultiMesh(multiMesh, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
							multiMesh.ManualInvalidate();
						}
					}
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000346C File Offset: 0x0000166C
		private void ApplyBannerTextureToMesh(Mesh armorMesh, Texture bannerTexture)
		{
			if (armorMesh != null)
			{
				Material material = armorMesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, bannerTexture);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				armorMesh.SetMaterial(material);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000034C4 File Offset: 0x000016C4
		public void MakeRandomVoiceForFacegen()
		{
			GameEntity entity = this._data.AgentVisuals.GetEntity();
			Vec3 origin = entity.Skeleton.GetBoneEntitialFrame(this._data.MonsterData.HeadLookDirectionBoneIndex).origin;
			Vec3 vec = entity.GetFrame().TransformToParent(in origin);
			entity.Skeleton.SetAgentActionChannel(1, in ActionIndexCache.act_command_leftstance, 0f, -0.2f, true, 0f);
			SkinVoiceManager.SkinVoiceType[] array = new SkinVoiceManager.SkinVoiceType[]
			{
				SkinVoiceManager.VoiceType.Yell,
				SkinVoiceManager.VoiceType.Victory,
				SkinVoiceManager.VoiceType.Charge,
				SkinVoiceManager.VoiceType.Advance,
				SkinVoiceManager.VoiceType.Stop,
				SkinVoiceManager.VoiceType.FallBack,
				SkinVoiceManager.VoiceType.UseLadders,
				SkinVoiceManager.VoiceType.Infantry,
				SkinVoiceManager.VoiceType.FireAtWill,
				SkinVoiceManager.VoiceType.FormLine,
				SkinVoiceManager.VoiceType.FormShieldWall,
				SkinVoiceManager.VoiceType.FormCircle
			};
			int index = array[MBRandom.RandomInt(array.Length)].Index;
			this._data.AgentVisuals.MakeVoice(index, vec);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000035F4 File Offset: 0x000017F4
		private bool ClearAndAddChangedVisualComponentsOfWeapons(Equipment oldEquipment, bool needBatchedVersionForMeshes)
		{
			int num = 0;
			for (int i = 0; i <= 3; i++)
			{
				if (!oldEquipment[i].IsEqualTo(this._data.EquipmentData[i]))
				{
					num++;
				}
			}
			if (num > 1)
			{
				return false;
			}
			bool flag = false;
			for (int j = 0; j <= 3; j++)
			{
				if (!oldEquipment[j].IsEqualTo(this._data.EquipmentData[j]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this._data.AgentVisuals.ClearAllWeaponMeshes();
				int k = 0;
				int num2 = 0;
				while (k < 5)
				{
					if (!this._data.EquipmentData[k].IsEmpty)
					{
						MissionWeapon missionWeapon = new MissionWeapon(this._data.EquipmentData[k].Item, this._data.EquipmentData[k].ItemModifier, this._data.BannerData);
						if (this._data.AddColorRandomnessData)
						{
							missionWeapon.SetRandomGlossMultiplier(this._data.BodyPropertiesData.GetHashCode());
						}
						ItemObject.ItemTypeEnum ammoTypeForItemType = ItemObject.GetAmmoTypeForItemType(WeaponComponentData.GetItemTypeFromWeaponClass(this._data.EquipmentData[k].Item.PrimaryWeapon.WeaponClass));
						bool flag2 = false;
						MissionWeapon missionWeapon2 = default(MissionWeapon);
						for (int l = 0; l < 5; l++)
						{
							if (!this._data.EquipmentData[l].IsEmpty && WeaponComponentData.GetItemTypeFromWeaponClass(this._data.EquipmentData[l].Item.PrimaryWeapon.WeaponClass) == ammoTypeForItemType)
							{
								flag2 = true;
								missionWeapon2 = new MissionWeapon(this._data.EquipmentData[l].Item, this._data.EquipmentData[l].ItemModifier, this._data.BannerData);
								if (this._data.AddColorRandomnessData)
								{
									missionWeapon2.SetRandomGlossMultiplier(this._data.BodyPropertiesData.GetHashCode());
								}
							}
						}
						WeaponData weaponData = missionWeapon.GetWeaponData(needBatchedVersionForMeshes);
						WeaponData weaponData2 = (flag2 ? missionWeapon2.GetWeaponData(needBatchedVersionForMeshes) : WeaponData.InvalidWeaponData);
						WeaponStatsData[] array = (flag2 ? missionWeapon2.GetWeaponStatsData() : null);
						this._data.AgentVisuals.AddWeaponToAgentEntity(k, in weaponData, missionWeapon.GetWeaponStatsData(), in weaponData2, array, null);
					}
					k++;
					num2++;
				}
				this._data.AgentVisuals.SetWieldedWeaponIndices(this._data.RightWieldedItemIndexData, this._data.LeftWieldedItemIndexData);
			}
			return flag;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000038C8 File Offset: 0x00001AC8
		public static void AddTeamColorToMesh(MetaMesh metaMesh, uint color1, uint color2)
		{
			for (int i = 0; i < metaMesh.MeshCount; i++)
			{
				Mesh meshAtIndex = metaMesh.GetMeshAtIndex(i);
				if (!meshAtIndex.HasTag("no_team_color"))
				{
					meshAtIndex.Color = color1;
					meshAtIndex.Color2 = color2;
					Material material = meshAtIndex.GetMaterial().CreateCopy();
					material.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
					meshAtIndex.SetMaterial(material);
				}
				meshAtIndex.ManualInvalidate();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000392E File Offset: 0x00001B2E
		public void SetClothingColors(uint color1, uint color2)
		{
			this._data.ClothColor1(color1);
			this._data.ClothColor2(color2);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000394A File Offset: 0x00001B4A
		public void GetClothingColors(out uint color1, out uint color2)
		{
			color1 = this._data.ClothColor1Data;
			color2 = this._data.ClothColor2Data;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003966 File Offset: 0x00001B66
		public void SetEntity(GameEntity entity)
		{
			this._data.AgentVisuals.SetEntity(entity);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003979 File Offset: 0x00001B79
		void IAgentVisual.SetAction(in ActionIndexCache actionName, float startProgress, bool forceFaceMorphRestart)
		{
			this.SetAction(in actionName, startProgress, forceFaceMorphRestart);
		}

		// Token: 0x04000006 RID: 6
		public const float RandomGlossinessRange = 0.05f;

		// Token: 0x04000007 RID: 7
		public const float RandomClothingColor1HueRange = 4f;

		// Token: 0x04000008 RID: 8
		public const float RandomClothingColor1SaturationRange = 0.2f;

		// Token: 0x04000009 RID: 9
		public const float RandomClothingColor1BrightnessRange = 0.2f;

		// Token: 0x0400000A RID: 10
		public const float RandomClothingColor2HueRange = 8f;

		// Token: 0x0400000B RID: 11
		public const float RandomClothingColor2SaturationRange = 0.5f;

		// Token: 0x0400000C RID: 12
		public const float RandomClothingColor2BrightnessRange = 0.3f;

		// Token: 0x0400000D RID: 13
		private AgentVisualsData _data;

		// Token: 0x0400000E RID: 14
		private float _scale;
	}
}
