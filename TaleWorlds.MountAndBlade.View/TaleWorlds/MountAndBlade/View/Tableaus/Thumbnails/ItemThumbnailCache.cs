using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000047 RID: 71
	public class ItemThumbnailCache : ThumbnailCache<ItemThumbnailCreationData>
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0001007E File Offset: 0x0000E27E
		public ItemThumbnailCache(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00010087 File Offset: 0x0000E287
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._itemTableauGPUAllocationIndex = Utilities.RegisterGPUAllocationGroup("ItemTableauCache");
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0001009F File Offset: 0x0000E29F
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000100A8 File Offset: 0x0000E2A8
		protected override TextureCreationInfo OnCreateTexture(ItemThumbnailCreationData thumbnailCreationData)
		{
			ItemObject itemObject = thumbnailCreationData.ItemObject;
			string additionalArgs = thumbnailCreationData.AdditionalArgs;
			Action<Texture> setAction = thumbnailCreationData.SetAction;
			Action cancelAction = thumbnailCreationData.CancelAction;
			string renderIdToUse = this.GetRenderIdToUse(thumbnailCreationData);
			Texture texture;
			if (((IThumbnailCache)this).GetValue(renderIdToUse, out texture))
			{
				if (this._renderCallbacks.ContainsKey(renderIdToUse))
				{
					this._renderCallbacks[renderIdToUse].SetActions.Add(setAction);
					this._renderCallbacks[renderIdToUse].CancelActions.Add(cancelAction);
				}
				else if (setAction != null)
				{
					setAction(texture);
				}
				((IThumbnailCache)this).AddReference(renderIdToUse);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			Camera camera = null;
			int num = 2;
			int num2 = 256;
			int num3 = 120;
			GameEntity gameEntity = this.CreateItemBaseEntity(itemObject, BannerlordTableauManager.TableauCharacterScenes[num], ref camera);
			string text = ThumbnailCache<ItemThumbnailCreationData>.CreateDebugIdFrom(renderIdToUse, "itm", "");
			ThumbnailRenderRequest thumbnailRenderRequest = ThumbnailRenderRequest.CreateWithoutTexture(BannerlordTableauManager.TableauCharacterScenes[num], camera, gameEntity, renderIdToUse, num2, num3, text, this._itemTableauGPUAllocationIndex);
			this._thumbnailCreatorView.RegisterRenderRequest(ref thumbnailRenderRequest);
			gameEntity.ManualInvalidate();
			((IThumbnailCache)this).Add(renderIdToUse, null);
			((IThumbnailCache)this).AddReference(renderIdToUse);
			if (!this._renderCallbacks.ContainsKey(renderIdToUse))
			{
				this._renderCallbacks.Add(renderIdToUse, RenderCallbackCollection.CreateEmpty());
			}
			this._renderCallbacks[renderIdToUse].SetActions.Add(setAction);
			this._renderCallbacks[renderIdToUse].CancelActions.Add(cancelAction);
			return TextureCreationInfo.WithNewTexture(null);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0001021C File Offset: 0x0000E41C
		protected override bool OnReleaseTexture(ItemThumbnailCreationData thumbnailCreationData)
		{
			string renderIdToUse = this.GetRenderIdToUse(thumbnailCreationData);
			return ((IThumbnailCache)this).RemoveReference(renderIdToUse);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00010238 File Offset: 0x0000E438
		private string GetRenderIdToUse(ItemThumbnailCreationData thumbnailCreationData)
		{
			ItemObject itemObject = thumbnailCreationData.ItemObject;
			string additionalArgs = thumbnailCreationData.AdditionalArgs;
			Action<Texture> setAction = thumbnailCreationData.SetAction;
			string text = itemObject.StringId;
			if (itemObject.Type == ItemObject.ItemTypeEnum.Shield)
			{
				text = text + "_" + additionalArgs;
			}
			return text;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00010278 File Offset: 0x0000E478
		private GameEntity CreateItemBaseEntity(ItemObject item, Scene scene, ref Camera camera)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			MatrixFrame identity2 = MatrixFrame.Identity;
			MatrixFrame identity3 = MatrixFrame.Identity;
			this.GetItemPoseAndCamera(item, scene, ref camera, ref identity, ref identity2, ref identity3);
			return this.AddItem(scene, item, identity, identity2, identity3);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000102B4 File Offset: 0x0000E4B4
		private void GetItemPoseAndCamera(ItemObject item, Scene scene, ref Camera camera, ref MatrixFrame itemFrame, ref MatrixFrame itemFrame1, ref MatrixFrame itemFrame2)
		{
			if (item.IsCraftedWeapon)
			{
				this.GetItemPoseAndCameraForCraftedItem(item, scene, ref camera, ref itemFrame, ref itemFrame1, ref itemFrame2);
				return;
			}
			string text = "";
			ItemThumbnailCache.CustomPoseParameters customPoseParameters = new ItemThumbnailCache.CustomPoseParameters
			{
				CameraTag = "goods_cam",
				DistanceModifier = 6f,
				FrameTag = "goods_frame"
			};
			if (item.WeaponComponent != null)
			{
				WeaponClass weaponClass = item.WeaponComponent.PrimaryWeapon.WeaponClass;
				if (weaponClass - WeaponClass.OneHandedSword <= 1)
				{
					text = "sword";
				}
			}
			else
			{
				ItemObject.ItemTypeEnum type = item.Type;
				if (type != ItemObject.ItemTypeEnum.HeadArmor)
				{
					if (type == ItemObject.ItemTypeEnum.BodyArmor)
					{
						text = "armor";
					}
				}
				else
				{
					text = "helmet";
				}
			}
			if (item.Type == ItemObject.ItemTypeEnum.Shield)
			{
				text = "shield";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Sling)
			{
				text = "sling";
			}
			if (item.Type == ItemObject.ItemTypeEnum.SlingStones)
			{
				text = "slingstones";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Crossbow)
			{
				text = "crossbow";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Bow)
			{
				text = "bow";
			}
			if (item.Type == ItemObject.ItemTypeEnum.LegArmor)
			{
				text = "boot";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Horse)
			{
				text = ((HorseComponent)item.ItemComponent).Monster.MonsterUsage;
			}
			if (item.Type == ItemObject.ItemTypeEnum.HorseHarness)
			{
				text = "horse";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Cape)
			{
				text = "cape";
			}
			if (item.Type == ItemObject.ItemTypeEnum.HandArmor)
			{
				text = "glove";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Arrows)
			{
				text = "arrow";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Bolts)
			{
				text = "bolt";
			}
			if (item.Type == ItemObject.ItemTypeEnum.Banner)
			{
				customPoseParameters = new ItemThumbnailCache.CustomPoseParameters
				{
					CameraTag = "banner_cam",
					DistanceModifier = 1.5f,
					FrameTag = "banner_frame",
					FocusAlignment = ItemThumbnailCache.CustomPoseParameters.Alignment.Top
				};
			}
			if (item.Type == ItemObject.ItemTypeEnum.Animal)
			{
				customPoseParameters = new ItemThumbnailCache.CustomPoseParameters
				{
					CameraTag = customPoseParameters.CameraTag,
					DistanceModifier = 3f,
					FrameTag = customPoseParameters.FrameTag
				};
			}
			if (item.StringId == "iron" || item.StringId == "hardwood" || item.StringId == "charcoal" || item.StringId == "ironIngot1" || item.StringId == "ironIngot2" || item.StringId == "ironIngot3" || item.StringId == "ironIngot4" || item.StringId == "ironIngot5" || item.StringId == "ironIngot6" || item.ItemCategory == DefaultItemCategories.Silver)
			{
				text = "craftmat";
			}
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = text + "_cam";
				string text3 = text + "_frame";
				GameEntity gameEntity = scene.FindEntityWithTag(text2);
				if (gameEntity != null)
				{
					camera = Camera.CreateCamera();
					Vec3 vec = default(Vec3);
					gameEntity.GetCameraParamsFromCameraScript(camera, ref vec);
				}
				GameEntity gameEntity2 = scene.FindEntityWithTag(text3);
				if (gameEntity2 != null)
				{
					itemFrame = gameEntity2.GetGlobalFrame();
					gameEntity2.SetVisibilityExcludeParents(false);
				}
			}
			else
			{
				GameEntity gameEntity3 = scene.FindEntityWithTag(customPoseParameters.CameraTag);
				if (gameEntity3 != null)
				{
					camera = Camera.CreateCamera();
					Vec3 vec2 = default(Vec3);
					gameEntity3.GetCameraParamsFromCameraScript(camera, ref vec2);
				}
				GameEntity gameEntity4 = scene.FindEntityWithTag(customPoseParameters.FrameTag);
				if (gameEntity4 != null)
				{
					itemFrame = gameEntity4.GetGlobalFrame();
					gameEntity4.SetVisibilityExcludeParents(false);
					gameEntity4.UpdateGlobalBounds();
					MatrixFrame globalFrame = gameEntity4.GetGlobalFrame();
					MetaMesh itemMeshForInventory = new ItemRosterElement(item, 0, null).GetItemMeshForInventory(false);
					Vec3 vec3 = new Vec3(1000000f, 1000000f, 1000000f, -1f);
					Vec3 vec4 = new Vec3(-1000000f, -1000000f, -1000000f, -1f);
					Vec3 vec5;
					if (itemMeshForInventory != null)
					{
						MatrixFrame identity = MatrixFrame.Identity;
						for (int num = 0; num != itemMeshForInventory.MeshCount; num++)
						{
							Vec3 boundingBoxMin = itemMeshForInventory.GetMeshAtIndex(num).GetBoundingBoxMin();
							Vec3 boundingBoxMax = itemMeshForInventory.GetMeshAtIndex(num).GetBoundingBoxMax();
							Vec3[] array = new Vec3[8];
							Vec3[] array2 = array;
							int num2 = 0;
							vec5 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
							array2[num2] = globalFrame.TransformToParent(in vec5);
							Vec3[] array3 = array;
							int num3 = 1;
							vec5 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
							array3[num3] = globalFrame.TransformToParent(in vec5);
							Vec3[] array4 = array;
							int num4 = 2;
							vec5 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
							array4[num4] = globalFrame.TransformToParent(in vec5);
							Vec3[] array5 = array;
							int num5 = 3;
							vec5 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
							array5[num5] = globalFrame.TransformToParent(in vec5);
							Vec3[] array6 = array;
							int num6 = 4;
							vec5 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
							array6[num6] = globalFrame.TransformToParent(in vec5);
							Vec3[] array7 = array;
							int num7 = 5;
							vec5 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
							array7[num7] = globalFrame.TransformToParent(in vec5);
							Vec3[] array8 = array;
							int num8 = 6;
							vec5 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
							array8[num8] = globalFrame.TransformToParent(in vec5);
							Vec3[] array9 = array;
							int num9 = 7;
							vec5 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
							array9[num9] = globalFrame.TransformToParent(in vec5);
							for (int i = 0; i < 8; i++)
							{
								vec3 = Vec3.Vec3Min(vec3, array[i]);
								vec4 = Vec3.Vec3Max(vec4, array[i]);
							}
						}
					}
					Vec3 vec6 = (vec3 + vec4) * 0.5f;
					MatrixFrame matrixFrame = gameEntity4.GetGlobalFrame();
					Vec3 vec7 = matrixFrame.TransformToLocal(in vec6);
					MatrixFrame globalFrame2 = gameEntity4.GetGlobalFrame();
					globalFrame2.origin -= vec7;
					itemFrame = globalFrame2;
					MatrixFrame frame = camera.Frame;
					vec5 = vec4 - vec3;
					float num10 = vec5.Length * customPoseParameters.DistanceModifier;
					frame.origin += frame.rotation.u * num10;
					if (customPoseParameters.FocusAlignment == ItemThumbnailCache.CustomPoseParameters.Alignment.Top)
					{
						Vec3 origin = frame.origin;
						float num11 = 0f;
						float num12 = 0f;
						vec5 = vec4 - vec3;
						frame.origin = origin + new Vec3(num11, num12, vec5.Z * 0.3f, -1f);
					}
					else if (customPoseParameters.FocusAlignment == ItemThumbnailCache.CustomPoseParameters.Alignment.Bottom)
					{
						Vec3 origin2 = frame.origin;
						float num13 = 0f;
						float num14 = 0f;
						vec5 = vec4 - vec3;
						frame.origin = origin2 - new Vec3(num13, num14, vec5.Z * 0.3f, -1f);
					}
					camera.Frame = frame;
				}
			}
			if (camera == null)
			{
				camera = Camera.CreateCamera();
				camera.SetViewVolume(false, -1f, 1f, -0.5f, 0.5f, 0.01f, 100f);
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.origin -= identity2.rotation.u * 7f;
				identity2.rotation.u = identity2.rotation.u * -1f;
				camera.Frame = identity2;
			}
			if (item.Type == ItemObject.ItemTypeEnum.Shield)
			{
				GameEntity gameEntity5 = scene.FindEntityWithTag("shield_cam");
				MatrixFrame holsterFrameByIndex = MBItem.GetHolsterFrameByIndex(MBItem.GetItemHolsterIndex(item.ItemHolsters[0]));
				itemFrame.rotation = holsterFrameByIndex.rotation;
				MatrixFrame matrixFrame = gameEntity5.GetFrame();
				MatrixFrame matrixFrame2 = itemFrame.TransformToParent(in matrixFrame);
				camera.Frame = matrixFrame2;
			}
			if (item.Type == ItemObject.ItemTypeEnum.Banner && item.StringId == "dragon_banner_center")
			{
				itemFrame.rotation.RotateAboutUp(3.1415927f);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00010B10 File Offset: 0x0000ED10
		private GameEntity AddItem(Scene scene, ItemObject item, MatrixFrame itemFrame, MatrixFrame itemFrame1, MatrixFrame itemFrame2)
		{
			ItemRosterElement itemRosterElement = new ItemRosterElement(item, 0, null);
			MetaMesh itemMeshForInventory = itemRosterElement.GetItemMeshForInventory(false);
			if (item.IsCraftedWeapon)
			{
				MatrixFrame frame = itemMeshForInventory.Frame;
				frame.Elevate(-item.WeaponDesign.CraftedWeaponLength / 2f);
				itemMeshForInventory.Frame = frame;
			}
			GameEntity gameEntity = null;
			if (itemMeshForInventory != null && itemRosterElement.EquipmentElement.Item.ItemType == ItemObject.ItemTypeEnum.HandArmor)
			{
				gameEntity = GameEntity.CreateEmpty(scene, true, true, true);
				AnimationSystemData animationSystemData = Game.Current.DefaultMonster.FillAnimationSystemData(MBActionSet.GetActionSet(Game.Current.DefaultMonster.ActionSetCode), 1f, false);
				gameEntity.CreateSkeletonWithActionSet(ref animationSystemData);
				gameEntity.SetFrame(ref itemFrame, true);
				gameEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_tableau_hand_armor_pose, 0f, -0.2f, true, 0f);
				gameEntity.AddMultiMeshToSkeleton(itemMeshForInventory);
				gameEntity.Skeleton.TickAnimationsAndForceUpdate(0.01f, itemFrame, true);
			}
			else if (itemMeshForInventory != null)
			{
				if (item.WeaponComponent != null)
				{
					WeaponClass weaponClass = item.WeaponComponent.PrimaryWeapon.WeaponClass;
					if (weaponClass == WeaponClass.ThrowingAxe || weaponClass == WeaponClass.ThrowingKnife || weaponClass == WeaponClass.Javelin || weaponClass == WeaponClass.Bolt)
					{
						gameEntity = GameEntity.CreateEmpty(scene, true, true, true);
						MetaMesh metaMesh = itemMeshForInventory.CreateCopy();
						metaMesh.Frame = itemFrame;
						gameEntity.AddMultiMesh(metaMesh, true);
						MetaMesh metaMesh2 = itemMeshForInventory.CreateCopy();
						metaMesh2.Frame = itemFrame1;
						gameEntity.AddMultiMesh(metaMesh2, true);
						MetaMesh metaMesh3 = itemMeshForInventory.CreateCopy();
						metaMesh3.Frame = itemFrame2;
						gameEntity.AddMultiMesh(metaMesh3, true);
					}
					else
					{
						gameEntity = scene.AddItemEntity(ref itemFrame, itemMeshForInventory);
					}
				}
				else
				{
					gameEntity = scene.AddItemEntity(ref itemFrame, itemMeshForInventory);
					if (item.Type == ItemObject.ItemTypeEnum.HorseHarness && item.ArmorComponent != null)
					{
						MetaMesh copy = MetaMesh.GetCopy(item.ArmorComponent.ReinsMesh, true, true);
						if (copy != null)
						{
							gameEntity.AddMultiMesh(copy, true);
						}
					}
				}
			}
			else
			{
				MBDebug.ShowWarning("[DEBUG]Item with " + itemRosterElement.EquipmentElement.Item.StringId + "[DEBUG] string id cannot be found");
			}
			gameEntity.SetVisibilityExcludeParents(false);
			return gameEntity;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00010D2C File Offset: 0x0000EF2C
		private void GetItemPoseAndCameraForCraftedItem(ItemObject item, Scene scene, ref Camera camera, ref MatrixFrame itemFrame, ref MatrixFrame itemFrame1, ref MatrixFrame itemFrame2)
		{
			if (camera == null)
			{
				camera = Camera.CreateCamera();
			}
			itemFrame = MatrixFrame.Identity;
			WeaponClass weaponClass = item.WeaponDesign.Template.WeaponDescriptions[0].WeaponClass;
			Vec3 u = itemFrame.rotation.u;
			Vec3 vec = itemFrame.origin - u * (item.WeaponDesign.CraftedWeaponLength * 0.5f);
			Vec3 vec2 = vec + u * item.WeaponDesign.CraftedWeaponLength;
			Vec3 vec3 = vec - u * item.WeaponDesign.BottomPivotOffset;
			int num = 0;
			Vec3 vec4 = default(Vec3);
			foreach (float num2 in item.WeaponDesign.TopPivotOffsets)
			{
				if (num2 > MathF.Abs(1E-05f))
				{
					Vec3 vec5 = vec + u * num2;
					if (num == 1)
					{
						vec4 = vec5;
					}
					num++;
				}
			}
			if (weaponClass == WeaponClass.OneHandedSword || weaponClass == WeaponClass.TwoHandedSword)
			{
				GameEntity gameEntity = scene.FindEntityWithTag("sword_camera");
				Vec3 vec6 = default(Vec3);
				gameEntity.GetCameraParamsFromCameraScript(camera, ref vec6);
				gameEntity.SetVisibilityExcludeParents(false);
				Vec3 vec7 = itemFrame.TransformToLocal(in vec3);
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin = -vec7;
				GameEntity gameEntity2 = scene.FindEntityWithTag("sword");
				gameEntity2.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity2.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity);
			}
			if (weaponClass == WeaponClass.OneHandedAxe || weaponClass == WeaponClass.TwoHandedAxe)
			{
				GameEntity gameEntity3 = scene.FindEntityWithTag("axe_camera");
				Vec3 vec8 = default(Vec3);
				gameEntity3.GetCameraParamsFromCameraScript(camera, ref vec8);
				gameEntity3.SetVisibilityExcludeParents(false);
				Vec3 vec9 = itemFrame.TransformToLocal(in vec4);
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.origin = -vec9;
				GameEntity gameEntity4 = scene.FindEntityWithTag("axe");
				gameEntity4.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity4.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity2);
			}
			if (weaponClass == WeaponClass.Dagger)
			{
				GameEntity gameEntity5 = scene.FindEntityWithTag("sword_camera");
				Vec3 vec10 = default(Vec3);
				gameEntity5.GetCameraParamsFromCameraScript(camera, ref vec10);
				gameEntity5.SetVisibilityExcludeParents(false);
				Vec3 vec11 = itemFrame.TransformToLocal(in vec3);
				MatrixFrame identity3 = MatrixFrame.Identity;
				identity3.origin = -vec11;
				GameEntity gameEntity6 = scene.FindEntityWithTag("sword");
				gameEntity6.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity6.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity3);
			}
			if (weaponClass == WeaponClass.ThrowingAxe)
			{
				GameEntity gameEntity7 = scene.FindEntityWithTag("throwing_axe_camera");
				Vec3 vec12 = default(Vec3);
				gameEntity7.GetCameraParamsFromCameraScript(camera, ref vec12);
				gameEntity7.SetVisibilityExcludeParents(false);
				Vec3 vec13 = vec + u * item.PrimaryWeapon.CenterOfMass;
				Vec3 vec14 = itemFrame.TransformToLocal(in vec13);
				MatrixFrame identity4 = MatrixFrame.Identity;
				identity4.origin = -vec14 * 2.5f;
				GameEntity gameEntity8 = scene.FindEntityWithTag("throwing_axe");
				gameEntity8.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity8.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity4);
				gameEntity8 = scene.FindEntityWithTag("throwing_axe_1");
				gameEntity8.SetVisibilityExcludeParents(false);
				itemFrame1 = gameEntity8.GetGlobalFrame();
				itemFrame1 = itemFrame1.TransformToParent(in identity4);
				gameEntity8 = scene.FindEntityWithTag("throwing_axe_2");
				gameEntity8.SetVisibilityExcludeParents(false);
				itemFrame2 = gameEntity8.GetGlobalFrame();
				itemFrame2 = itemFrame2.TransformToParent(in identity4);
			}
			if (weaponClass == WeaponClass.Javelin)
			{
				GameEntity gameEntity9 = scene.FindEntityWithTag("javelin_camera");
				Vec3 vec15 = default(Vec3);
				gameEntity9.GetCameraParamsFromCameraScript(camera, ref vec15);
				gameEntity9.SetVisibilityExcludeParents(false);
				Vec3 vec16 = itemFrame.TransformToLocal(in vec4);
				MatrixFrame identity5 = MatrixFrame.Identity;
				identity5.origin = -vec16 * 2.2f;
				GameEntity gameEntity10 = scene.FindEntityWithTag("javelin");
				gameEntity10.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity10.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity5);
				gameEntity10 = scene.FindEntityWithTag("javelin_1");
				gameEntity10.SetVisibilityExcludeParents(false);
				itemFrame1 = gameEntity10.GetGlobalFrame();
				itemFrame1 = itemFrame1.TransformToParent(in identity5);
				gameEntity10 = scene.FindEntityWithTag("javelin_2");
				gameEntity10.SetVisibilityExcludeParents(false);
				itemFrame2 = gameEntity10.GetGlobalFrame();
				itemFrame2 = itemFrame2.TransformToParent(in identity5);
			}
			if (weaponClass == WeaponClass.ThrowingKnife)
			{
				GameEntity gameEntity11 = scene.FindEntityWithTag("javelin_camera");
				Vec3 vec17 = default(Vec3);
				gameEntity11.GetCameraParamsFromCameraScript(camera, ref vec17);
				gameEntity11.SetVisibilityExcludeParents(false);
				Vec3 vec18 = itemFrame.TransformToLocal(in vec2);
				MatrixFrame identity6 = MatrixFrame.Identity;
				identity6.origin = -vec18 * 1.4f;
				GameEntity gameEntity12 = scene.FindEntityWithTag("javelin");
				gameEntity12.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity12.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity6);
				gameEntity12 = scene.FindEntityWithTag("javelin_1");
				gameEntity12.SetVisibilityExcludeParents(false);
				itemFrame1 = gameEntity12.GetGlobalFrame();
				itemFrame1 = itemFrame1.TransformToParent(in identity6);
				gameEntity12 = scene.FindEntityWithTag("javelin_2");
				gameEntity12.SetVisibilityExcludeParents(false);
				itemFrame2 = gameEntity12.GetGlobalFrame();
				itemFrame2 = itemFrame2.TransformToParent(in identity6);
			}
			if (weaponClass == WeaponClass.TwoHandedPolearm || weaponClass == WeaponClass.OneHandedPolearm || weaponClass == WeaponClass.LowGripPolearm || weaponClass == WeaponClass.Mace || weaponClass == WeaponClass.TwoHandedMace)
			{
				GameEntity gameEntity13 = scene.FindEntityWithTag("spear_camera");
				Vec3 vec19 = default(Vec3);
				gameEntity13.GetCameraParamsFromCameraScript(camera, ref vec19);
				gameEntity13.SetVisibilityExcludeParents(false);
				Vec3 vec20 = itemFrame.TransformToLocal(in vec4);
				MatrixFrame identity7 = MatrixFrame.Identity;
				identity7.origin = -vec20;
				GameEntity gameEntity14 = scene.FindEntityWithTag("spear");
				gameEntity14.SetVisibilityExcludeParents(false);
				itemFrame = gameEntity14.GetGlobalFrame();
				itemFrame = itemFrame.TransformToParent(in identity7);
			}
		}

		// Token: 0x04000147 RID: 327
		private int _itemTableauGPUAllocationIndex;

		// Token: 0x020000C3 RID: 195
		private struct CustomPoseParameters
		{
			// Token: 0x0400038C RID: 908
			public string CameraTag;

			// Token: 0x0400038D RID: 909
			public string FrameTag;

			// Token: 0x0400038E RID: 910
			public float DistanceModifier;

			// Token: 0x0400038F RID: 911
			public ItemThumbnailCache.CustomPoseParameters.Alignment FocusAlignment;

			// Token: 0x020000F4 RID: 244
			public enum Alignment
			{
				// Token: 0x0400043A RID: 1082
				Center,
				// Token: 0x0400043B RID: 1083
				Top,
				// Token: 0x0400043C RID: 1084
				Bottom
			}
		}
	}
}
