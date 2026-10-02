using System;
using System.Collections.Generic;
using System.Globalization;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000032 RID: 50
	public class BasicCharacterTableau
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00009CF7 File Offset: 0x00007EF7
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00009CFF File Offset: 0x00007EFF
		public Texture Texture { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00009D08 File Offset: 0x00007F08
		public bool IsVersionCompatible
		{
			get
			{
				return this._isVersionCompatible;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00009D10 File Offset: 0x00007F10
		private TableauView View
		{
			get
			{
				Texture texture = this.Texture;
				if (texture == null)
				{
					return null;
				}
				return texture.TableauView;
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00009D24 File Offset: 0x00007F24
		public BasicCharacterTableau()
		{
			this._isFirstFrame = true;
			this._isVisualsDirty = false;
			this._bodyProperties = BodyProperties.Default;
			this._currentCharacters = new GameEntity[2];
			this._currentCharacters[0] = null;
			this._currentCharacters[1] = null;
			this._currentMounts = new GameEntity[2];
			this._currentMounts[0] = null;
			this._currentMounts[1] = null;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00009DA4 File Offset: 0x00007FA4
		public void OnTick(float dt)
		{
			if (this._isEnabled && this._isRotatingCharacter)
			{
				this.UpdateCharacterRotation((int)Input.MouseMoveX);
			}
			TableauView view = this.View;
			if (view != null)
			{
				view.SetDoNotRenderThisFrame(false);
			}
			if (this._isFirstFrame)
			{
				this.FirstTimeInit();
				this._isFirstFrame = false;
			}
			if (this._isVisualsDirty)
			{
				this.RefreshCharacterTableau();
				this._isVisualsDirty = false;
			}
			if (this._checkWhetherEntitiesAreReady)
			{
				int num = (this._currentEntityToShowIndex + 1) % 2;
				bool flag = true;
				if (!this._currentCharacters[this._currentEntityToShowIndex].CheckResources(true, true))
				{
					flag = false;
				}
				if (!this._currentMounts[this._currentEntityToShowIndex].CheckResources(true, true))
				{
					flag = false;
				}
				if (!flag)
				{
					this._currentCharacters[this._currentEntityToShowIndex].SetVisibilityExcludeParents(false);
					this._currentMounts[this._currentEntityToShowIndex].SetVisibilityExcludeParents(false);
					this._currentCharacters[num].SetVisibilityExcludeParents(true);
					this._currentMounts[num].SetVisibilityExcludeParents(true);
					return;
				}
				this._currentCharacters[this._currentEntityToShowIndex].SetVisibilityExcludeParents(true);
				this._currentMounts[this._currentEntityToShowIndex].SetVisibilityExcludeParents(true);
				this._currentCharacters[num].SetVisibilityExcludeParents(false);
				this._currentMounts[num].SetVisibilityExcludeParents(false);
				this._checkWhetherEntitiesAreReady = false;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009EE0 File Offset: 0x000080E0
		private void UpdateCharacterRotation(int mouseMoveX)
		{
			if (this._initialized && this._skeletonName != null)
			{
				this._mainCharacterRotation += (float)mouseMoveX * 0.005f;
				MatrixFrame initialSpawnFrame = this._initialSpawnFrame;
				initialSpawnFrame.rotation.RotateAboutUp(this._mainCharacterRotation);
				this._currentCharacters[0].SetFrame(ref initialSpawnFrame, true);
				this._currentCharacters[1].SetFrame(ref initialSpawnFrame, true);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00009F4B File Offset: 0x0000814B
		private void SetEnabled(bool enabled)
		{
			this._isEnabled = enabled;
			TableauView view = this.View;
			if (view == null)
			{
				return;
			}
			view.SetEnable(this._isEnabled);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00009F6C File Offset: 0x0000816C
		public void SetTargetSize(int width, int height)
		{
			this._isRotatingCharacter = false;
			if (width <= 0 || height <= 0)
			{
				this._tableauSizeX = 10;
				this._tableauSizeY = 10;
			}
			else
			{
				this.RenderScale = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ResolutionScale) / 100f;
				this._tableauSizeX = (int)((float)width * this.RenderScale);
				this._tableauSizeY = (int)((float)height * this.RenderScale);
			}
			this._cameraRatio = (float)this._tableauSizeX / (float)this._tableauSizeY;
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(true);
			}
			Texture texture = this.Texture;
			if (texture != null)
			{
				texture.Release();
			}
			this.Texture = TableauView.AddTableau(string.Format("BasicCharacterTableau_{0}", BasicCharacterTableau._tableauIndex++), new RenderTargetComponent.TextureUpdateEventHandler(this.CharacterTableauContinuousRenderFunction), this._tableauScene, this._tableauSizeX, this._tableauSizeY);
			this.View.SetScene(this._tableauScene);
			this.View.SetSceneUsesSkybox(false);
			this.View.SetDeleteAfterRendering(false);
			this.View.SetContinuousRendering(true);
			this.View.SetDoNotRenderThisFrame(true);
			this.View.SetClearColor(0U);
			this.View.SetFocusedShadowmap(true, ref this._initialSpawnFrame.origin, 1.55f);
			this.View.SetRenderWithPostfx(true);
			this.SetCamera();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A0D8 File Offset: 0x000082D8
		public void OnFinalize()
		{
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(false);
			}
			this.Texture = null;
			this._banner = null;
			if (this._tableauScene != null)
			{
				this._tableauScene.ManualInvalidate();
				this._tableauScene = null;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A138 File Offset: 0x00008338
		public void DeserializeCharacterCode(string code)
		{
			if (code != this._characterCode)
			{
				if (this._initialized)
				{
					this.ResetProperties();
				}
				this._characterCode = code;
				string[] array = code.Split(new char[] { '|' });
				int num;
				if (int.TryParse(array[0], out num) && 4 == num)
				{
					this._isVersionCompatible = true;
					int num2 = 0;
					try
					{
						num2++;
						this._skeletonName = array[num2];
						num2++;
						Enum.TryParse<SkinMask>(array[num2], false, out this._skinMeshesMask);
						num2++;
						bool.TryParse(array[num2], out this._isFemale);
						num2++;
						this._race = int.Parse(array[num2]);
						num2++;
						Enum.TryParse<Equipment.UnderwearTypes>(array[num2], false, out this._underwearType);
						num2++;
						Enum.TryParse<ArmorComponent.BodyMeshTypes>(array[num2], false, out this._bodyMeshType);
						num2++;
						Enum.TryParse<ArmorComponent.HairCoverTypes>(array[num2], false, out this._hairCoverType);
						num2++;
						Enum.TryParse<ArmorComponent.BeardCoverTypes>(array[num2], false, out this._beardCoverType);
						num2++;
						Enum.TryParse<ArmorComponent.BodyDeformTypes>(array[num2], false, out this._bodyDeformType);
						num2++;
						float.TryParse(array[num2], NumberStyles.Any, CultureInfo.InvariantCulture, out this._faceDirtAmount);
						num2++;
						BodyProperties.FromString(array[num2], out this._bodyProperties);
						num2++;
						uint.TryParse(array[num2], out this._clothColor1);
						num2++;
						uint.TryParse(array[num2], out this._clothColor2);
						this._equipmentMeshes = new List<string>();
						this._equipmentHasColors = new List<bool>();
						this._equipmentHasGenderVariations = new List<bool>();
						this._equipmentHasTableau = new List<bool>();
						for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
						{
							num2++;
							this._equipmentMeshes.Add(array[num2]);
							num2++;
							bool flag;
							bool.TryParse(array[num2], out flag);
							this._equipmentHasColors.Add(flag);
							num2++;
							bool flag2;
							bool.TryParse(array[num2], out flag2);
							this._equipmentHasGenderVariations.Add(flag2);
							num2++;
							bool flag3;
							bool.TryParse(array[num2], out flag3);
							this._equipmentHasTableau.Add(flag3);
						}
						num2++;
						this._mountSkeletonName = array[num2];
						num2++;
						this._mountMeshName = array[num2];
						num2++;
						this._mountCreationKey = array[num2];
						num2++;
						this._mountMaterialName = array[num2];
						num2++;
						if (array[num2].Length > 0)
						{
							uint.TryParse(array[num2], out this._mountManeMeshMultiplier);
						}
						else
						{
							this._mountManeMeshMultiplier = 0U;
						}
						num2++;
						this._mountIdleAnimationName = array[num2];
						num2++;
						this._mountHarnessMeshName = array[num2];
						num2++;
						bool.TryParse(array[num2], out this._mountHarnessHasColors);
						num2++;
						this._mountReinsMeshName = array[num2];
						num2++;
						int num3 = int.Parse(array[num2]);
						this._maneMeshNames = new string[num3];
						for (int i = 0; i < num3; i++)
						{
							num2++;
							this._maneMeshNames[i] = array[num2];
						}
					}
					catch (Exception ex)
					{
						this.ResetProperties();
						Debug.FailedAssert("Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\BasicCharacterTableau.cs", "DeserializeCharacterCode", 348);
						Debug.FailedAssert("Couldn't parse character code: " + code, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\BasicCharacterTableau.cs", "DeserializeCharacterCode", 349);
					}
				}
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A480 File Offset: 0x00008680
		private void ResetProperties()
		{
			this._skeletonName = string.Empty;
			this._skinMeshesMask = SkinMask.NoneVisible;
			this._isFemale = false;
			this._underwearType = Equipment.UnderwearTypes.NoUnderwear;
			this._bodyMeshType = ArmorComponent.BodyMeshTypes.Normal;
			this._hairCoverType = ArmorComponent.HairCoverTypes.None;
			this._beardCoverType = ArmorComponent.BeardCoverTypes.None;
			this._bodyDeformType = ArmorComponent.BodyDeformTypes.Medium;
			this._faceDirtAmount = 0f;
			this._bodyProperties = BodyProperties.Default;
			this._clothColor1 = 0U;
			this._clothColor2 = 0U;
			List<string> equipmentMeshes = this._equipmentMeshes;
			if (equipmentMeshes != null)
			{
				equipmentMeshes.Clear();
			}
			List<bool> equipmentHasColors = this._equipmentHasColors;
			if (equipmentHasColors != null)
			{
				equipmentHasColors.Clear();
			}
			this._mountSkeletonName = string.Empty;
			this._mountMeshName = string.Empty;
			this._mountCreationKey = string.Empty;
			this._mountMaterialName = string.Empty;
			this._mountManeMeshMultiplier = uint.MaxValue;
			this._mountIdleAnimationName = string.Empty;
			this._mountHarnessMeshName = string.Empty;
			this._mountReinsMeshName = string.Empty;
			this._maneMeshNames = null;
			this._mountHarnessHasColors = false;
			this._race = 0;
			this._isVersionCompatible = false;
			this._characterCode = string.Empty;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A58C File Offset: 0x0000878C
		private void FirstTimeInit()
		{
			if (this._tableauScene == null)
			{
				this._tableauScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
				this._tableauScene.SetName("BasicCharacterTableau");
				this._tableauScene.DisableStaticShadows(true);
				SceneInitializationData sceneInitializationData = new SceneInitializationData(true)
				{
					InitPhysicsWorld = false,
					DoNotUseLoadingScreen = true
				};
				this._tableauScene.Read("inventory_character_scene", ref sceneInitializationData, "");
				this._tableauScene.SetShadow(true);
				this._mountSpawnPoint = this._tableauScene.FindEntityWithTag("horse_inv").GetGlobalFrame();
				this._initialSpawnFrame = this._tableauScene.FindEntityWithTag("agent_inv").GetGlobalFrame();
				this._tableauScene.EnsurePostfxSystem();
				this._tableauScene.SetDofMode(false);
				this._tableauScene.SetMotionBlurMode(false);
				this._tableauScene.SetBloom(true);
				this._tableauScene.SetDynamicShadowmapCascadesRadiusMultiplier(0.31f);
				this._tableauScene.RemoveEntity(this._tableauScene.FindEntityWithTag("agent_inv"), 99);
				this._tableauScene.RemoveEntity(this._tableauScene.FindEntityWithTag("horse_inv"), 100);
				this._currentCharacters[0] = GameEntity.CreateEmpty(this._tableauScene, false, true, true);
				this._currentCharacters[1] = GameEntity.CreateEmpty(this._tableauScene, false, true, true);
				this._currentMounts[0] = GameEntity.CreateEmpty(this._tableauScene, false, true, true);
				this._currentMounts[1] = GameEntity.CreateEmpty(this._tableauScene, false, true, true);
			}
			this.SetEnabled(true);
			this._initialized = true;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A72C File Offset: 0x0000892C
		private static void ApplyBannerTextureToMesh(Mesh armorMesh, Texture bannerTexture)
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

		// Token: 0x06000180 RID: 384 RVA: 0x0000A784 File Offset: 0x00008984
		private void RefreshCharacterTableau()
		{
			if (!this._initialized)
			{
				return;
			}
			this._currentEntityToShowIndex = (this._currentEntityToShowIndex + 1) % 2;
			GameEntity gameEntity = this._currentCharacters[this._currentEntityToShowIndex];
			gameEntity.ClearEntityComponents(true, true, true);
			GameEntity gameEntity2 = this._currentMounts[this._currentEntityToShowIndex];
			gameEntity2.ClearEntityComponents(true, true, true);
			this._mainCharacterRotation = 0f;
			if (!string.IsNullOrEmpty(this._skeletonName))
			{
				AnimationSystemData hardcodedAnimationSystemDataForHumanSkeleton = AnimationSystemData.GetHardcodedAnimationSystemDataForHumanSkeleton();
				bool flag = this._bodyProperties.Age >= 14f && this._isFemale;
				gameEntity.Skeleton = MBSkeletonExtensions.CreateWithActionSet(ref hardcodedAnimationSystemDataForHumanSkeleton);
				MetaMesh metaMesh = null;
				bool flag2 = this._equipmentMeshes[3].Length > 0;
				for (int i = 0; i < 5; i++)
				{
					string text = this._equipmentMeshes[i];
					if (text.Length > 0)
					{
						bool flag3 = flag && this._equipmentHasGenderVariations[i];
						MetaMesh metaMesh2 = MetaMesh.GetCopy(flag3 ? (text + "_female") : (text + "_male"), false, true);
						if (metaMesh2 == null)
						{
							string text2 = text;
							if (flag3)
							{
								text2 += (flag2 ? "_converted_slim" : "_converted");
							}
							else
							{
								text2 += (flag2 ? "_slim" : "");
							}
							metaMesh2 = MetaMesh.GetCopy(text2, false, true) ?? MetaMesh.GetCopy(text, false, true);
						}
						if (metaMesh2 != null)
						{
							if (i == 3)
							{
								metaMesh = metaMesh2;
							}
							gameEntity.AddMultiMeshToSkeleton(metaMesh2);
							if (this._equipmentHasTableau[i])
							{
								for (int j = 0; j < metaMesh2.MeshCount; j++)
								{
									Mesh currentMesh = metaMesh2.GetMeshAtIndex(j);
									Mesh currentMesh3 = currentMesh;
									if (currentMesh3 != null && !currentMesh3.HasTag("dont_use_tableau"))
									{
										Mesh currentMesh2 = currentMesh;
										if (currentMesh2 != null && currentMesh2.HasTag("banner_replacement_mesh") && this._banner != null)
										{
											BannerTextureCreationData bannerTextureCreationData = new BannerTextureCreationData(this._banner, delegate(Texture t)
											{
												BasicCharacterTableau.ApplyBannerTextureToMesh(currentMesh, t);
											}, null, BannerDebugInfo.CreateManual(base.GetType().Name), true, true);
											ThumbnailCacheManager.Current.CreateTexture(bannerTextureCreationData);
											break;
										}
									}
								}
							}
							else if (this._equipmentHasColors[i])
							{
								for (int k = 0; k < metaMesh2.MeshCount; k++)
								{
									Mesh meshAtIndex = metaMesh2.GetMeshAtIndex(k);
									if (!meshAtIndex.HasTag("no_team_color"))
									{
										meshAtIndex.Color = this._clothColor1;
										meshAtIndex.Color2 = this._clothColor2;
										Material material = meshAtIndex.GetMaterial().CreateCopy();
										material.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
										meshAtIndex.SetMaterial(material);
									}
								}
							}
							metaMesh2.ManualInvalidate();
						}
					}
				}
				gameEntity.SetGlobalFrame(in this._initialSpawnFrame, true);
				SkinGenerationParams skinGenerationParams = new SkinGenerationParams((int)this._skinMeshesMask, this._underwearType, (int)this._bodyMeshType, (int)this._hairCoverType, (int)this._beardCoverType, (int)this._bodyDeformType, true, this._faceDirtAmount, flag ? 1 : 0, this._race, false, false, 0);
				MBAgentVisuals.FillEntityWithBodyMeshesWithoutAgentVisuals(gameEntity, skinGenerationParams, this._bodyProperties, metaMesh);
				gameEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_inventory_idle, 0f, -0.2f, true, 0f);
				gameEntity.SetEnforcedMaximumLodLevel(0);
				gameEntity.CheckResources(true, true);
				if (this._mountMeshName.Length > 0)
				{
					gameEntity2.Skeleton = Skeleton.CreateFromModel(this._mountSkeletonName);
					MetaMesh copy = MetaMesh.GetCopy(this._mountMeshName, true, false);
					if (copy != null)
					{
						MountCreationKey mountCreationKey = MountCreationKey.FromString(this._mountCreationKey);
						MountVisualCreator.SetHorseColors(copy, mountCreationKey);
						if (!string.IsNullOrEmpty(this._mountMaterialName))
						{
							Material fromResource = Material.GetFromResource(this._mountMaterialName);
							copy.SetMaterialToSubMeshesWithTag(fromResource, "horse_body");
							copy.SetFactorColorToSubMeshesWithTag(this._mountManeMeshMultiplier, "horse_tail");
						}
						gameEntity2.AddMultiMeshToSkeleton(copy);
						copy.ManualInvalidate();
						if (this._mountHarnessMeshName.Length > 0)
						{
							MetaMesh copy2 = MetaMesh.GetCopy(this._mountHarnessMeshName, false, true);
							if (copy2 != null)
							{
								if (this._mountReinsMeshName.Length > 0)
								{
									MetaMesh copy3 = MetaMesh.GetCopy(this._mountReinsMeshName, false, true);
									if (copy3 != null)
									{
										gameEntity2.AddMultiMeshToSkeleton(copy3);
										copy3.ManualInvalidate();
									}
								}
								gameEntity2.AddMultiMeshToSkeleton(copy2);
								if (this._mountHarnessHasColors)
								{
									for (int l = 0; l < copy2.MeshCount; l++)
									{
										Mesh meshAtIndex2 = copy2.GetMeshAtIndex(l);
										if (!meshAtIndex2.HasTag("no_team_color"))
										{
											meshAtIndex2.Color = this._clothColor1;
											meshAtIndex2.Color2 = this._clothColor2;
											Material material2 = meshAtIndex2.GetMaterial().CreateCopy();
											material2.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
											meshAtIndex2.SetMaterial(material2);
										}
									}
								}
								copy2.ManualInvalidate();
							}
						}
					}
					string[] maneMeshNames = this._maneMeshNames;
					for (int m = 0; m < maneMeshNames.Length; m++)
					{
						MetaMesh copy4 = MetaMesh.GetCopy(maneMeshNames[m], false, true);
						if (this._mountManeMeshMultiplier != 4294967295U)
						{
							copy4.SetFactor1Linear(this._mountManeMeshMultiplier);
						}
						gameEntity2.AddMultiMeshToSkeleton(copy4);
						copy4.ManualInvalidate();
					}
					gameEntity2.SetGlobalFrame(in this._mountSpawnPoint, true);
					gameEntity2.Skeleton.SetAnimationAtChannel(this._mountIdleAnimationName, 0, 1f, 0f, 0f);
					gameEntity2.SetEnforcedMaximumLodLevel(0);
					gameEntity2.CheckResources(true, true);
				}
			}
			this._currentCharacters[this._currentEntityToShowIndex].SetVisibilityExcludeParents(false);
			this._currentMounts[this._currentEntityToShowIndex].SetVisibilityExcludeParents(false);
			int num = (this._currentEntityToShowIndex + 1) % 2;
			this._currentCharacters[num].SetVisibilityExcludeParents(true);
			this._currentMounts[num].SetVisibilityExcludeParents(true);
			this._checkWhetherEntitiesAreReady = true;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000AD64 File Offset: 0x00008F64
		internal void SetCamera()
		{
			Camera camera = Camera.CreateCamera();
			camera.Frame = this._tableauScene.FindEntityWithTag("camera_instance").GetGlobalFrame();
			camera.SetFovVertical(0.7853982f, this._cameraRatio, 0.2f, 200f);
			this.View.SetCamera(camera);
			camera.ManualInvalidate();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000ADBF File Offset: 0x00008FBF
		public void RotateCharacter(bool value)
		{
			this._isRotatingCharacter = value;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		public void SetBannerCode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				this._banner = null;
				return;
			}
			this._banner = new Banner(value);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000ADE8 File Offset: 0x00008FE8
		internal void CharacterTableauContinuousRenderFunction(Texture sender, EventArgs e)
		{
			NativeObject nativeObject = (Scene)sender.UserData;
			TableauView tableauView = sender.TableauView;
			if (nativeObject == null)
			{
				tableauView.SetContinuousRendering(false);
				tableauView.SetDeleteAfterRendering(true);
			}
		}

		// Token: 0x0400007A RID: 122
		private static int _tableauIndex;

		// Token: 0x0400007C RID: 124
		private bool _isVersionCompatible;

		// Token: 0x0400007D RID: 125
		private const int _expectedCharacterCodeVersion = 4;

		// Token: 0x0400007E RID: 126
		private bool _initialized;

		// Token: 0x0400007F RID: 127
		private int _tableauSizeX;

		// Token: 0x04000080 RID: 128
		private int _tableauSizeY;

		// Token: 0x04000081 RID: 129
		private float RenderScale = 1f;

		// Token: 0x04000082 RID: 130
		private float _cameraRatio;

		// Token: 0x04000083 RID: 131
		private List<string> _equipmentMeshes;

		// Token: 0x04000084 RID: 132
		private List<bool> _equipmentHasColors;

		// Token: 0x04000085 RID: 133
		private List<bool> _equipmentHasGenderVariations;

		// Token: 0x04000086 RID: 134
		private List<bool> _equipmentHasTableau;

		// Token: 0x04000087 RID: 135
		private uint _clothColor1;

		// Token: 0x04000088 RID: 136
		private uint _clothColor2;

		// Token: 0x04000089 RID: 137
		private MatrixFrame _mountSpawnPoint;

		// Token: 0x0400008A RID: 138
		private MatrixFrame _initialSpawnFrame;

		// Token: 0x0400008B RID: 139
		private Scene _tableauScene;

		// Token: 0x0400008C RID: 140
		private SkinMask _skinMeshesMask;

		// Token: 0x0400008D RID: 141
		private bool _isFemale;

		// Token: 0x0400008E RID: 142
		private string _skeletonName;

		// Token: 0x0400008F RID: 143
		private string _characterCode;

		// Token: 0x04000090 RID: 144
		private Equipment.UnderwearTypes _underwearType;

		// Token: 0x04000091 RID: 145
		private string _mountMeshName;

		// Token: 0x04000092 RID: 146
		private string _mountCreationKey;

		// Token: 0x04000093 RID: 147
		private string _mountMaterialName;

		// Token: 0x04000094 RID: 148
		private uint _mountManeMeshMultiplier;

		// Token: 0x04000095 RID: 149
		private ArmorComponent.BodyMeshTypes _bodyMeshType;

		// Token: 0x04000096 RID: 150
		private ArmorComponent.HairCoverTypes _hairCoverType;

		// Token: 0x04000097 RID: 151
		private ArmorComponent.BeardCoverTypes _beardCoverType;

		// Token: 0x04000098 RID: 152
		private ArmorComponent.BodyDeformTypes _bodyDeformType;

		// Token: 0x04000099 RID: 153
		private string _mountSkeletonName;

		// Token: 0x0400009A RID: 154
		private string _mountIdleAnimationName;

		// Token: 0x0400009B RID: 155
		private string _mountHarnessMeshName;

		// Token: 0x0400009C RID: 156
		private string _mountReinsMeshName;

		// Token: 0x0400009D RID: 157
		private string[] _maneMeshNames;

		// Token: 0x0400009E RID: 158
		private bool _mountHarnessHasColors;

		// Token: 0x0400009F RID: 159
		private bool _isFirstFrame;

		// Token: 0x040000A0 RID: 160
		private float _faceDirtAmount;

		// Token: 0x040000A1 RID: 161
		private float _mainCharacterRotation;

		// Token: 0x040000A2 RID: 162
		private bool _isVisualsDirty;

		// Token: 0x040000A3 RID: 163
		private bool _isRotatingCharacter;

		// Token: 0x040000A4 RID: 164
		private bool _isEnabled;

		// Token: 0x040000A5 RID: 165
		private int _race;

		// Token: 0x040000A6 RID: 166
		private readonly GameEntity[] _currentCharacters;

		// Token: 0x040000A7 RID: 167
		private readonly GameEntity[] _currentMounts;

		// Token: 0x040000A8 RID: 168
		private int _currentEntityToShowIndex;

		// Token: 0x040000A9 RID: 169
		private bool _checkWhetherEntitiesAreReady;

		// Token: 0x040000AA RID: 170
		private BodyProperties _bodyProperties = BodyProperties.Default;

		// Token: 0x040000AB RID: 171
		private Banner _banner;
	}
}
