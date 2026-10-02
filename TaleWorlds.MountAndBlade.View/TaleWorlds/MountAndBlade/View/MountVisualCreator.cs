using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200001C RID: 28
	public static class MountVisualCreator
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00005D9C File Offset: 0x00003F9C
		public static void SetMaterialProperties(ItemObject mountItem, MetaMesh mountMesh, MountCreationKey key, ref uint maneMeshMultiplier)
		{
			HorseComponent horseComponent = mountItem.HorseComponent;
			int num = MathF.Min((int)key.MaterialIndex, horseComponent.HorseMaterialNames.Count - 1);
			HorseComponent.MaterialProperty materialProperty = horseComponent.HorseMaterialNames[num];
			Material fromResource = Material.GetFromResource(materialProperty.Name);
			if (mountItem.ItemType == ItemObject.ItemTypeEnum.Horse)
			{
				int num2 = MathF.Min((int)key.MeshMultiplierIndex, materialProperty.MeshMultiplier.Count - 1);
				if (num2 != -1)
				{
					maneMeshMultiplier = materialProperty.MeshMultiplier[num2].Item1;
				}
				mountMesh.SetMaterialToSubMeshesWithTag(fromResource, "horse_body");
				mountMesh.SetFactorColorToSubMeshesWithTag(maneMeshMultiplier, "horse_tail");
				return;
			}
			mountMesh.SetMaterial(fromResource);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005E40 File Offset: 0x00004040
		public static MountVisualCreationOutput AddMountMesh(MBAgentVisuals agentVisual, ItemObject mountItem, ItemObject harnessItem, string mountCreationKeyStr, Agent agent = null)
		{
			MetaMesh metaMesh = null;
			MetaMesh metaMesh2 = null;
			MetaMesh metaMesh3 = null;
			MetaMesh metaMesh4 = null;
			HorseComponent horseComponent = mountItem.HorseComponent;
			uint maxValue = uint.MaxValue;
			metaMesh2 = mountItem.GetMultiMesh(false, false, true);
			if (string.IsNullOrEmpty(mountCreationKeyStr))
			{
				mountCreationKeyStr = MountCreationKey.GetRandomMountKeyString(mountItem, MBRandom.RandomInt());
			}
			MountCreationKey mountCreationKey = MountCreationKey.FromString(mountCreationKeyStr);
			if (mountItem.ItemType == ItemObject.ItemTypeEnum.Horse)
			{
				MountVisualCreator.SetHorseColors(metaMesh2, mountCreationKey);
			}
			if (horseComponent.HorseMaterialNames != null && horseComponent.HorseMaterialNames.Count > 0)
			{
				MountVisualCreator.SetMaterialProperties(mountItem, metaMesh2, mountCreationKey, ref maxValue);
			}
			int nondeterministicRandomInt = MBRandom.NondeterministicRandomInt;
			MountVisualCreator.SetVoiceDefinition(agent, nondeterministicRandomInt);
			if (harnessItem != null)
			{
				metaMesh4 = harnessItem.GetMultiMesh(false, false, true);
			}
			foreach (KeyValuePair<string, bool> keyValuePair in horseComponent.AdditionalMeshesNameList)
			{
				if (keyValuePair.Key.Length > 0)
				{
					string text = keyValuePair.Key;
					if (harnessItem == null || !keyValuePair.Value)
					{
						metaMesh = MetaMesh.GetCopy(text, true, false);
						if (maxValue != 4294967295U)
						{
							metaMesh.SetFactor1Linear(maxValue);
						}
					}
					else
					{
						ArmorComponent armorComponent = harnessItem.ArmorComponent;
						if (armorComponent == null || armorComponent.ManeCoverType != ArmorComponent.HorseHarnessCoverTypes.All)
						{
							ArmorComponent armorComponent2 = harnessItem.ArmorComponent;
							if (armorComponent2 != null && armorComponent2.ManeCoverType > ArmorComponent.HorseHarnessCoverTypes.None)
							{
								object obj = text;
								object obj2 = "_";
								ArmorComponent.HorseHarnessCoverTypes? horseHarnessCoverTypes;
								if (harnessItem == null)
								{
									horseHarnessCoverTypes = null;
								}
								else
								{
									ArmorComponent armorComponent3 = harnessItem.ArmorComponent;
									horseHarnessCoverTypes = ((armorComponent3 != null) ? new ArmorComponent.HorseHarnessCoverTypes?(armorComponent3.ManeCoverType) : null);
								}
								text = obj + obj2 + horseHarnessCoverTypes;
							}
							metaMesh = MetaMesh.GetCopy(text, true, false);
							if (maxValue != 4294967295U)
							{
								metaMesh.SetFactor1Linear(maxValue);
							}
						}
					}
				}
			}
			if (metaMesh2 != null && harnessItem != null)
			{
				ArmorComponent armorComponent4 = harnessItem.ArmorComponent;
				ArmorComponent.HorseTailCoverTypes? horseTailCoverTypes = ((armorComponent4 != null) ? new ArmorComponent.HorseTailCoverTypes?(armorComponent4.TailCoverType) : null);
				ArmorComponent.HorseTailCoverTypes horseTailCoverTypes2 = ArmorComponent.HorseTailCoverTypes.All;
				if ((horseTailCoverTypes.GetValueOrDefault() == horseTailCoverTypes2) & (horseTailCoverTypes != null))
				{
					metaMesh2.RemoveMeshesWithTag("horse_tail");
				}
			}
			if (metaMesh4 != null)
			{
				if (agentVisual != null)
				{
					MetaMesh metaMesh5 = null;
					if (NativeConfig.CharacterDetail > 2 && harnessItem.ArmorComponent != null)
					{
						metaMesh5 = MetaMesh.GetCopy(harnessItem.ArmorComponent.ReinsRopeMesh, false, true);
					}
					ArmorComponent armorComponent5 = harnessItem.ArmorComponent;
					metaMesh3 = MetaMesh.GetCopy((armorComponent5 != null) ? armorComponent5.ReinsMesh : null, false, true);
					if (metaMesh5 != null && metaMesh3 != null)
					{
						agentVisual.AddHorseReinsClothMesh(metaMesh3, metaMesh5);
						metaMesh5.ManualInvalidate();
					}
				}
				else if (harnessItem.ArmorComponent != null)
				{
					metaMesh3 = MetaMesh.GetCopy(harnessItem.ArmorComponent.ReinsMesh, true, true);
				}
			}
			return new MountVisualCreationOutput(metaMesh, metaMesh2, metaMesh3, metaMesh4);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000060E4 File Offset: 0x000042E4
		public static void SetHorseColors(MetaMesh horseMesh, MountCreationKey mountCreationKey)
		{
			horseMesh.SetVectorArgument((float)mountCreationKey._leftFrontLegColorIndex, (float)mountCreationKey._rightFrontLegColorIndex, (float)mountCreationKey._leftBackLegColorIndex, (float)mountCreationKey._rightBackLegColorIndex);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006108 File Offset: 0x00004308
		public static void ClearMountMesh(GameEntity gameEntity)
		{
			gameEntity.RemoveAllChildren();
			gameEntity.Remove(106);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006118 File Offset: 0x00004318
		private static void SetVoiceDefinition(Agent agent, int seedForRandomVoiceTypeAndPitch)
		{
			MBAgentVisuals mbagentVisuals = ((agent != null) ? agent.AgentVisuals : null);
			if (mbagentVisuals != null)
			{
				string soundAndCollisionInfoClassName = agent.GetSoundAndCollisionInfoClassName();
				int num;
				if (string.IsNullOrEmpty(soundAndCollisionInfoClassName))
				{
					num = 0;
				}
				else
				{
					num = SkinVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(soundAndCollisionInfoClassName);
				}
				if (num == 0)
				{
					mbagentVisuals.SetVoiceDefinitionIndex(-1, 0f);
					return;
				}
				int num2 = MathF.Abs(seedForRandomVoiceTypeAndPitch);
				float num3 = (float)num2 * 4.656613E-10f;
				int[] array = new int[num];
				SkinVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(soundAndCollisionInfoClassName, array);
				int num4 = array[num2 % num];
				mbagentVisuals.SetVoiceDefinitionIndex(num4, num3);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006198 File Offset: 0x00004398
		public static void AddMountMeshToEntity(GameEntity gameEntity, ItemObject mountItem, ItemObject harnessItem, string mountCreationKeyStr, out MountVisualCreationOutput mountVisualCreationOutput, Agent agent = null)
		{
			mountVisualCreationOutput = MountVisualCreator.AddMountMesh(null, mountItem, harnessItem, mountCreationKeyStr, agent);
			MountVisualCreator.AddMultiMeshToSkeleton(mountVisualCreationOutput.HorseManeMesh, gameEntity);
			MountVisualCreator.AddMultiMeshToSkeleton(mountVisualCreationOutput.MountMesh, gameEntity);
			MountVisualCreator.AddMultiMeshToSkeleton(mountVisualCreationOutput.ReinMesh, gameEntity);
			MountVisualCreator.AddMultiMeshToSkeleton(mountVisualCreationOutput.MountHarnessMesh, gameEntity);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000061EC File Offset: 0x000043EC
		public static void AddMountMeshToEntity(GameEntity gameEntity, ItemObject mountItem, ItemObject harnessItem, string mountCreationKeyStr, Agent agent = null)
		{
			MountVisualCreationOutput mountVisualCreationOutput;
			MountVisualCreator.AddMountMeshToEntity(gameEntity, mountItem, harnessItem, mountCreationKeyStr, out mountVisualCreationOutput, agent);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006208 File Offset: 0x00004408
		public static void AddMountMeshToAgentVisual(MBAgentVisuals agentVisual, ItemObject mountItem, ItemObject harnessItem, string mountCreationKeyStr, Agent agent = null)
		{
			MountVisualCreationOutput mountVisualCreationOutput = MountVisualCreator.AddMountMesh(agentVisual, mountItem, harnessItem, mountCreationKeyStr, agent);
			MountVisualCreator.AddMultiMeshToAgentVisual(mountVisualCreationOutput.HorseManeMesh, agentVisual);
			MountVisualCreator.AddMultiMeshToAgentVisual(mountVisualCreationOutput.MountMesh, agentVisual);
			MountVisualCreator.AddMultiMeshToAgentVisual(mountVisualCreationOutput.ReinMesh, agentVisual);
			MountVisualCreator.AddMultiMeshToAgentVisual(mountVisualCreationOutput.MountHarnessMesh, agentVisual);
			if (agent != null && harnessItem != null && harnessItem.IsUsingTeamColor && mountVisualCreationOutput.MountHarnessMesh != null)
			{
				AgentVisuals.AddTeamColorToMesh(mountVisualCreationOutput.MountHarnessMesh, agent.ClothingColor1, agent.ClothingColor2);
			}
			HorseComponent horseComponent = mountItem.HorseComponent;
			if (((horseComponent != null) ? horseComponent.SkeletonScale : null) != null)
			{
				agentVisual.ApplySkeletonScale(mountItem.HorseComponent.SkeletonScale.MountSitBoneScale, mountItem.HorseComponent.SkeletonScale.MountRadiusAdder, mountItem.HorseComponent.SkeletonScale.BoneIndices, mountItem.HorseComponent.SkeletonScale.Scales);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000062E7 File Offset: 0x000044E7
		private static void AddMultiMeshToAgentVisual(MetaMesh metaMesh, MBAgentVisuals agentVisual)
		{
			if (metaMesh != null)
			{
				agentVisual.AddMultiMesh(metaMesh, BodyMeshTypes.Invalid);
				metaMesh.ManualInvalidate();
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006300 File Offset: 0x00004500
		private static void AddMultiMeshToSkeleton(MetaMesh metaMesh, GameEntity gameEntity)
		{
			if (metaMesh != null)
			{
				gameEntity.AddMultiMeshToSkeleton(metaMesh);
				metaMesh.ManualInvalidate();
			}
		}
	}
}
