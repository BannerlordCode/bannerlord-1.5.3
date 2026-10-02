using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000035 RID: 53
	public class ItemTableau
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000CE2A File Offset: 0x0000B02A
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000CE32 File Offset: 0x0000B032
		public Texture Texture { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000CE3B File Offset: 0x0000B03B
		private TableauView View
		{
			get
			{
				if (this.Texture != null)
				{
					return this.Texture.TableauView;
				}
				return null;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000CE58 File Offset: 0x0000B058
		private bool _isSizeValid
		{
			get
			{
				return this._tableauSizeX > 0 && this._tableauSizeY > 0;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000CE70 File Offset: 0x0000B070
		public ItemTableau()
		{
			this.SetEnabled(true);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public void SetTargetSize(int width, int height)
		{
			bool isSizeValid = this._isSizeValid;
			this._isRotating = false;
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
			if (!isSizeValid && this._isSizeValid)
			{
				this.Recalculate();
			}
			this.Texture = TableauView.AddTableau(string.Format("ItemTableau_{0}", ItemTableau._tableauIndex++), new RenderTargetComponent.TextureUpdateEventHandler(this.TableauMaterialTabInventoryItemTooltipOnRender), this._tableauScene, this._tableauSizeX, this._tableauSizeY);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000CFD4 File Offset: 0x0000B1D4
		public void OnFinalize()
		{
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			Camera camera = this._camera;
			if (camera != null)
			{
				camera.ReleaseCameraEntity();
			}
			this._camera = null;
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(false);
			}
			Scene tableauScene = this._tableauScene;
			if (tableauScene != null)
			{
				tableauScene.ManualInvalidate();
			}
			this._tableauScene = null;
			this.Texture = null;
			this._initialized = false;
			if (this._lockMouse)
			{
				this.UpdateMouseLock(true);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D054 File Offset: 0x0000B254
		protected void SetEnabled(bool enabled)
		{
			this._isRotatingByDefault = true;
			this._isRotating = false;
			this.ResetCamera();
			this._isEnabled = enabled;
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(this._isEnabled);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D098 File Offset: 0x0000B298
		public void SetStringId(string stringId)
		{
			this._stringId = stringId;
			this.Recalculate();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D0A7 File Offset: 0x0000B2A7
		public void SetAmmo(int ammo)
		{
			this._ammo = ammo;
			this.Recalculate();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
		public void SetAverageUnitCost(int averageUnitCost)
		{
			this._averageUnitCost = averageUnitCost;
			this.Recalculate();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D0C5 File Offset: 0x0000B2C5
		public void SetItemModifierId(string itemModifierId)
		{
			this._itemModifierId = itemModifierId;
			this.Recalculate();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		public void SetBannerCode(string bannerCode)
		{
			this._bannerCode = bannerCode;
			this.Recalculate();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public void Recalculate()
		{
			if (UiStringHelper.IsStringNoneOrEmptyForUi(this._stringId) || !this._isSizeValid)
			{
				return;
			}
			ItemModifier itemModifier = null;
			ItemObject itemObject = MBObjectManager.Instance.GetObject<ItemObject>(this._stringId);
			if (itemObject == null)
			{
				itemObject = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>().FirstOrDefault<ItemObject>((ItemObject item) => item.IsCraftedWeapon && item.WeaponDesign.HashedCode == this._stringId);
			}
			if (!string.IsNullOrEmpty(this._itemModifierId))
			{
				itemModifier = MBObjectManager.Instance.GetObject<ItemModifier>(this._itemModifierId);
			}
			if (itemObject == null)
			{
				return;
			}
			this._itemRosterElement = new ItemRosterElement(itemObject, this._ammo, itemModifier);
			this.RefreshItemTableau();
			if (this._itemTableauEntity != null)
			{
				float num = Screen.RealScreenResolutionWidth / (float)this._tableauSizeX;
				float num2 = Screen.RealScreenResolutionHeight / (float)this._tableauSizeY;
				float num3 = ((num > num2) ? num : num2);
				if (num3 < 1f)
				{
					Vec3 globalBoxMax = this._itemTableauEntity.GlobalBoxMax;
					Vec3 globalBoxMin = this._itemTableauEntity.GlobalBoxMin;
					this._itemTableauFrame = this._itemTableauEntity.GetFrame();
					float length = this._itemTableauFrame.rotation.f.Length;
					this._itemTableauFrame.rotation.Orthonormalize();
					this._itemTableauFrame.rotation.ApplyScaleLocal(length * num3);
					this._itemTableauEntity.SetFrame(ref this._itemTableauFrame, true);
					Vec3 globalBoxMax2 = this._itemTableauEntity.GlobalBoxMax;
					if (globalBoxMax.NearlyEquals(in globalBoxMax2, 1E-05f))
					{
						Vec3 globalBoxMin2 = this._itemTableauEntity.GlobalBoxMin;
						if (globalBoxMin.NearlyEquals(in globalBoxMin2, 1E-05f))
						{
							this._itemTableauEntity.SetBoundingboxDirty();
							this._itemTableauEntity.RecomputeBoundingBox();
						}
					}
					this._itemTableauFrame.origin = this._itemTableauFrame.origin + (globalBoxMax + globalBoxMin - this._itemTableauEntity.GlobalBoxMax - this._itemTableauEntity.GlobalBoxMin) * 0.5f;
					this._itemTableauEntity.SetFrame(ref this._itemTableauFrame, true);
					this._midPoint = (this._itemTableauEntity.GlobalBoxMax + this._itemTableauEntity.GlobalBoxMin) * 0.5f + (globalBoxMax + globalBoxMin - this._itemTableauEntity.GlobalBoxMax - this._itemTableauEntity.GlobalBoxMin) * 0.5f;
				}
				else
				{
					this._midPoint = (this._itemTableauEntity.GlobalBoxMax + this._itemTableauEntity.GlobalBoxMin) * 0.5f;
				}
				if (this._itemRosterElement.EquipmentElement.Item.ItemType == ItemObject.ItemTypeEnum.HandArmor || this._itemRosterElement.EquipmentElement.Item.ItemType == ItemObject.ItemTypeEnum.Shield)
				{
					this._midPoint *= 1.2f;
				}
				this.ResetCamera();
			}
			this._isRotatingByDefault = true;
			this._isRotating = false;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public void Initialize()
		{
			this._isRotatingByDefault = true;
			this._isRotating = false;
			this._isTranslating = false;
			this._tableauScene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			this._tableauScene.SetName("ItemTableau");
			this._tableauScene.DisableStaticShadows(true);
			this._tableauScene.SetAtmosphereWithName("character_menu_a");
			Vec3 vec = new Vec3(2.15f, 2.2f, 2.25f, -1f) * 2.25f;
			Vec3 vec2 = new Vec3(1f, -1f, -1f, -1f);
			this._tableauScene.SetSunLight(ref vec, ref vec2);
			this._tableauScene.SetClothSimulationState(false);
			this.ResetCamera();
			this._initialized = true;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D4A0 File Offset: 0x0000B6A0
		private void TranslateCamera(bool value)
		{
			this.TranslateCameraAux(value);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D4A9 File Offset: 0x0000B6A9
		private void TranslateCameraAux(bool value)
		{
			this._isRotatingByDefault = !value && this._isRotatingByDefault;
			this._isTranslating = value;
			this.UpdateMouseLock(false);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		private void ResetCamera()
		{
			this._curCamDisplacement = Vec3.Zero;
			this._curZoomSpeed = 0f;
			if (this._camera != null)
			{
				this._camera.Frame = MatrixFrame.Identity;
				this.SetCamFovHorizontal(1f);
				this.MakeCameraLookMidPoint();
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D51E File Offset: 0x0000B71E
		public void RotateItem(bool value)
		{
			this._isRotatingByDefault = !value && this._isRotatingByDefault;
			this._isRotating = value;
			this.UpdateMouseLock(false);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D540 File Offset: 0x0000B740
		public void RotateItemVerticalWithAmount(float value)
		{
			this.UpdateRotation(0f, value / -2f);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000D554 File Offset: 0x0000B754
		public void RotateItemHorizontalWithAmount(float value)
		{
			this.UpdateRotation(value / 2f, 0f);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000D568 File Offset: 0x0000B768
		public void OnTick(float dt)
		{
			float num = Input.MouseMoveX + Input.GetKeyState(InputKey.ControllerLStick).X * 1000f * dt;
			float num2 = Input.MouseMoveY + Input.GetKeyState(InputKey.ControllerLStick).Y * -1000f * dt;
			if (this._isEnabled && (this._isRotating || this._isTranslating) && (!num.ApproximatelyEqualsTo(0f, 1E-05f) || !num2.ApproximatelyEqualsTo(0f, 1E-05f)))
			{
				if (this._isRotating)
				{
					this.UpdateRotation(num, num2);
				}
				if (this._isTranslating)
				{
					this.UpdatePosition(num, num2);
				}
			}
			this.TickCameraZoom(dt);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000D61C File Offset: 0x0000B81C
		private void UpdatePosition(float mouseMoveX, float mouseMoveY)
		{
			if (this._initialized)
			{
				Vec3 vec = new Vec3(mouseMoveX / (float)(-(float)this._tableauSizeX), mouseMoveY / (float)this._tableauSizeY, 0f, -1f);
				vec *= 2.2f * this._camera.HorizontalFov;
				this._curCamDisplacement += vec;
				this.MakeCameraLookMidPoint();
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D688 File Offset: 0x0000B888
		private void UpdateRotation(float mouseMoveX, float mouseMoveY)
		{
			if (this._initialized)
			{
				this._panRotation += mouseMoveX * 0.004363323f;
				this._tiltRotation += mouseMoveY * 0.004363323f;
				this._tiltRotation = MathF.Clamp(this._tiltRotation, -2.984513f, -0.15707964f);
				MatrixFrame matrixFrame = this._itemTableauEntity.GetFrame();
				Vec3 vec = (this._itemTableauEntity.GetBoundingBoxMax() + this._itemTableauEntity.GetBoundingBoxMin()) * 0.5f;
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin = vec;
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.origin = -vec;
				matrixFrame = (in matrixFrame) * (in identity);
				matrixFrame.rotation = Mat3.Identity;
				Vec3 scaleVector = this._initialFrame.rotation.GetScaleVector();
				matrixFrame.rotation.ApplyScaleLocal(in scaleVector);
				matrixFrame.rotation.RotateAboutSide(this._tiltRotation);
				matrixFrame.rotation.RotateAboutUp(this._panRotation);
				matrixFrame = (in matrixFrame) * (in identity2);
				this._itemTableauEntity.SetFrame(ref matrixFrame, true);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000D7A8 File Offset: 0x0000B9A8
		public void SetInitialTiltRotation(float amount)
		{
			this._hasInitialTiltRotation = true;
			this._initialTiltRotation = amount;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		public void SetInitialPanRotation(float amount)
		{
			this._hasInitialPanRotation = true;
			this._initialPanRotation = amount;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		public void Zoom(double value)
		{
			this._curZoomSpeed -= (float)(value / 1000.0);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D7E3 File Offset: 0x0000B9E3
		public void SetItem(ItemRosterElement itemRosterElement)
		{
			this._itemRosterElement = itemRosterElement;
			this.RefreshItemTableau();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		private void RefreshItemTableau()
		{
			if (!this._initialized)
			{
				this.Initialize();
			}
			if (this._itemTableauEntity != null)
			{
				this._itemTableauEntity.Remove(102);
				this._itemTableauEntity = null;
			}
			if (this._itemRosterElement.EquipmentElement.Item != null)
			{
				ItemObject.ItemTypeEnum itemType = this._itemRosterElement.EquipmentElement.Item.ItemType;
				if (this._itemTableauEntity == null)
				{
					MatrixFrame itemFrameForItemTooltip = this._itemRosterElement.GetItemFrameForItemTooltip();
					itemFrameForItemTooltip.origin.z = itemFrameForItemTooltip.origin.z + 2.5f;
					MetaMesh itemMeshForInventory = this._itemRosterElement.GetItemMeshForInventory(false);
					uint num = 0U;
					uint num2 = 0U;
					if (!string.IsNullOrEmpty(this._bannerCode))
					{
						Banner banner = new Banner(this._bannerCode);
						num = banner.GetPrimaryColor();
						BannerColor bannerColor;
						if (banner.BannerDataList.Count > 0 && BannerManager.Instance.ReadOnlyColorPalette.TryGetValue(banner.BannerDataList[1].ColorId, out bannerColor))
						{
							num2 = bannerColor.Color;
						}
					}
					if (itemMeshForInventory != null)
					{
						if (itemType == ItemObject.ItemTypeEnum.HandArmor)
						{
							this._itemTableauEntity = GameEntity.CreateEmpty(this._tableauScene, true, true, true);
							AnimationSystemData animationSystemData = Game.Current.DefaultMonster.FillAnimationSystemData(MBActionSet.GetActionSet(Game.Current.DefaultMonster.ActionSetCode), 1f, false);
							this._itemTableauEntity.CreateSkeletonWithActionSet(ref animationSystemData);
							this._itemTableauEntity.SetFrame(ref itemFrameForItemTooltip, true);
							this._itemTableauEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_tableau_hand_armor_pose, 0f, -0.2f, true, 0f);
							this._itemTableauEntity.AddMultiMeshToSkeleton(itemMeshForInventory);
							this._itemTableauEntity.Skeleton.TickActionChannels();
							this._itemTableauEntity.Skeleton.TickAnimationsAndForceUpdate(0.01f, itemFrameForItemTooltip, true);
						}
						else if (itemType == ItemObject.ItemTypeEnum.Horse || itemType == ItemObject.ItemTypeEnum.Animal)
						{
							HorseComponent horseComponent = this._itemRosterElement.EquipmentElement.Item.HorseComponent;
							Monster monster = horseComponent.Monster;
							this._itemTableauEntity = GameEntity.CreateEmpty(this._tableauScene, true, true, true);
							AnimationSystemData animationSystemData2 = monster.FillAnimationSystemData(MBGlobals.GetActionSet(horseComponent.Monster.ActionSetCode), 1f, false);
							this._itemTableauEntity.CreateSkeletonWithActionSet(ref animationSystemData2);
							this._itemTableauEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_inventory_idle_start, 0f, -0.2f, true, 0f);
							this._itemTableauEntity.SetFrame(ref itemFrameForItemTooltip, true);
							this._itemTableauEntity.AddMultiMeshToSkeleton(itemMeshForInventory);
						}
						else if (itemType == ItemObject.ItemTypeEnum.HorseHarness && this._itemRosterElement.EquipmentElement.Item.ArmorComponent != null)
						{
							this._itemTableauEntity = this._tableauScene.AddItemEntity(ref itemFrameForItemTooltip, itemMeshForInventory);
							MetaMesh copy = MetaMesh.GetCopy(this._itemRosterElement.EquipmentElement.Item.ArmorComponent.ReinsMesh, true, true);
							if (copy != null)
							{
								this._itemTableauEntity.AddMultiMesh(copy, true);
							}
						}
						else if (itemType == ItemObject.ItemTypeEnum.Shield)
						{
							if (!string.IsNullOrEmpty(this._bannerCode))
							{
								Banner banner2 = new Banner(this._bannerCode);
								if (this._itemRosterElement.EquipmentElement.Item.IsUsingTableau && !banner2.IsBannerDataListEmpty())
								{
									itemMeshForInventory.SetMaterial(this._itemRosterElement.EquipmentElement.Item.GetTableauMaterial(banner2));
								}
							}
							this._itemTableauEntity = this._tableauScene.AddItemEntity(ref itemFrameForItemTooltip, itemMeshForInventory);
						}
						else if (itemType == ItemObject.ItemTypeEnum.Banner)
						{
							if (!string.IsNullOrEmpty(this._bannerCode))
							{
								Banner banner3 = new Banner(this._bannerCode);
								if (this._itemRosterElement.EquipmentElement.Item.IsUsingTableau && !banner3.IsBannerDataListEmpty())
								{
									itemMeshForInventory.SetMaterial(this._itemRosterElement.EquipmentElement.Item.GetTableauMaterial(banner3));
								}
								for (int i = 0; i < itemMeshForInventory.MeshCount; i++)
								{
									itemMeshForInventory.GetMeshAtIndex(i).Color = num;
									itemMeshForInventory.GetMeshAtIndex(i).Color2 = num2;
								}
							}
							this._itemTableauEntity = this._tableauScene.AddItemEntity(ref itemFrameForItemTooltip, itemMeshForInventory);
						}
						else
						{
							this._itemTableauEntity = this._tableauScene.AddItemEntity(ref itemFrameForItemTooltip, itemMeshForInventory);
						}
					}
					else
					{
						MBDebug.ShowWarning("[DEBUG]Item with " + this._itemRosterElement.EquipmentElement.Item.StringId + "[DEBUG] string id cannot be found");
					}
				}
				SkinMask skinMask = SkinMask.AllVisible;
				if (this._itemRosterElement.EquipmentElement.Item.HasArmorComponent)
				{
					skinMask = this._itemRosterElement.EquipmentElement.Item.ArmorComponent.MeshesMask;
				}
				string text = "";
				bool flag = this._itemRosterElement.EquipmentElement.Item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale);
				bool flag2 = false;
				if (ItemObject.ItemTypeEnum.HeadArmor == itemType || ItemObject.ItemTypeEnum.Cape == itemType)
				{
					text = "base_head";
					flag2 = true;
				}
				else if (ItemObject.ItemTypeEnum.BodyArmor == itemType)
				{
					if (skinMask.HasAnyFlag(SkinMask.BodyVisible))
					{
						text = "base_body";
						flag2 = true;
					}
				}
				else if (ItemObject.ItemTypeEnum.LegArmor == itemType)
				{
					if (skinMask.HasAnyFlag(SkinMask.LegsVisible))
					{
						text = "base_foot";
						flag2 = true;
					}
				}
				else if (ItemObject.ItemTypeEnum.HandArmor == itemType)
				{
					if (skinMask.HasAnyFlag(SkinMask.HandsVisible))
					{
						MetaMesh copy2 = MetaMesh.GetCopy(flag ? "base_hand_female" : "base_hand", false, false);
						this._itemTableauEntity.AddMultiMeshToSkeleton(copy2);
					}
				}
				else if (ItemObject.ItemTypeEnum.HorseHarness == itemType)
				{
					text = "horse_base_mesh";
					flag2 = false;
				}
				if (text.Length > 0)
				{
					if (flag2 && flag)
					{
						text += "_female";
					}
					MetaMesh copy3 = MetaMesh.GetCopy(text, false, false);
					this._itemTableauEntity.AddMultiMesh(copy3, true);
				}
				TableauView view = this.View;
				if (view != null)
				{
					float num3 = (this._itemTableauEntity.GetBoundingBoxMax() - this._itemTableauEntity.GetBoundingBoxMin()).Length * 2f;
					Vec3 origin = this._itemTableauEntity.GetGlobalFrame().origin;
					view.SetFocusedShadowmap(true, ref origin, num3);
				}
				if (this._itemTableauEntity != null)
				{
					this._initialFrame = this._itemTableauEntity.GetFrame();
					Vec3 eulerAngles = this._initialFrame.rotation.GetEulerAngles();
					this._panRotation = eulerAngles.x;
					this._tiltRotation = eulerAngles.z;
					if (this._hasInitialPanRotation)
					{
						this._panRotation = this._initialPanRotation;
					}
					else if (itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._panRotation = -3.1415927f;
					}
					if (this._hasInitialTiltRotation)
					{
						this._tiltRotation = this._initialTiltRotation;
						return;
					}
					if (itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._tiltRotation = 0f;
						return;
					}
					this._tiltRotation = -1.5707964f;
				}
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
		private void TableauMaterialTabInventoryItemTooltipOnRender(Texture sender, EventArgs e)
		{
			if (this._initialized)
			{
				TableauView tableauView = this.View;
				if (tableauView == null)
				{
					tableauView = sender.TableauView;
					tableauView.SetEnable(this._isEnabled);
				}
				if (this._itemRosterElement.EquipmentElement.Item == null)
				{
					tableauView.SetContinuousRendering(false);
					tableauView.SetDeleteAfterRendering(true);
					return;
				}
				tableauView.SetRenderWithPostfx(true);
				tableauView.SetClearColor(0U);
				tableauView.SetScene(this._tableauScene);
				if (this._camera == null)
				{
					this._camera = Camera.CreateCamera();
					this._camera.SetViewVolume(true, -0.5f, 0.5f, -0.5f, 0.5f, 0.01f, 100f);
					this.ResetCamera();
					tableauView.SetSceneUsesSkybox(false);
				}
				tableauView.SetCamera(this._camera);
				if (this._isRotatingByDefault)
				{
					this.UpdateRotation(1f, 0f);
				}
				tableauView.SetDeleteAfterRendering(false);
				tableauView.SetContinuousRendering(true);
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000DFB4 File Offset: 0x0000C1B4
		private void MakeCameraLookMidPoint()
		{
			Vec3 vec = this._camera.Frame.rotation.TransformToParent(in this._curCamDisplacement);
			Vec3 vec2 = this._midPoint + vec;
			float num = this._midPoint.Length * 0.5263158f;
			Vec3 vec3 = vec2 - this._camera.Direction * num;
			this._camera.Position = vec3;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000E021 File Offset: 0x0000C221
		private void SetCamFovHorizontal(float camFov)
		{
			this._camera.SetFovHorizontal(camFov, 1f, 0.1f, 50f);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000E03E File Offset: 0x0000C23E
		private void UpdateMouseLock(bool forceUnlock = false)
		{
			this._lockMouse = (this._isRotating || this._isTranslating) && !forceUnlock;
			MouseManager.LockCursorAtCurrentPosition(this._lockMouse);
			MouseManager.ShowCursor(!this._lockMouse);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000E078 File Offset: 0x0000C278
		private void TickCameraZoom(float dt)
		{
			if (this._camera != null)
			{
				float num = this._camera.HorizontalFov;
				num += this._curZoomSpeed;
				float num2 = 0.61086524f;
				float num3 = 1.134464f;
				num = MBMath.ClampFloat(num, num2, num3);
				this.SetCamFovHorizontal(num);
				if (dt > 0f)
				{
					this._curZoomSpeed = MBMath.Lerp(this._curZoomSpeed, 0f, MBMath.ClampFloat(dt * 25.9f, 0f, 1f), 1E-05f);
				}
			}
		}

		// Token: 0x040000F7 RID: 247
		private static int _tableauIndex;

		// Token: 0x040000F9 RID: 249
		private Scene _tableauScene;

		// Token: 0x040000FA RID: 250
		private GameEntity _itemTableauEntity;

		// Token: 0x040000FB RID: 251
		private MatrixFrame _itemTableauFrame = MatrixFrame.Identity;

		// Token: 0x040000FC RID: 252
		private bool _isRotating;

		// Token: 0x040000FD RID: 253
		private bool _isTranslating;

		// Token: 0x040000FE RID: 254
		private bool _isRotatingByDefault;

		// Token: 0x040000FF RID: 255
		private bool _initialized;

		// Token: 0x04000100 RID: 256
		private int _tableauSizeX;

		// Token: 0x04000101 RID: 257
		private int _tableauSizeY;

		// Token: 0x04000102 RID: 258
		private float _cameraRatio;

		// Token: 0x04000103 RID: 259
		private Camera _camera;

		// Token: 0x04000104 RID: 260
		private Vec3 _midPoint;

		// Token: 0x04000105 RID: 261
		private const float InitialCamFov = 1f;

		// Token: 0x04000106 RID: 262
		private float _curZoomSpeed;

		// Token: 0x04000107 RID: 263
		private Vec3 _curCamDisplacement = Vec3.Zero;

		// Token: 0x04000108 RID: 264
		private bool _isEnabled;

		// Token: 0x04000109 RID: 265
		private float _panRotation;

		// Token: 0x0400010A RID: 266
		private float _tiltRotation;

		// Token: 0x0400010B RID: 267
		private bool _hasInitialTiltRotation;

		// Token: 0x0400010C RID: 268
		private float _initialTiltRotation;

		// Token: 0x0400010D RID: 269
		private bool _hasInitialPanRotation;

		// Token: 0x0400010E RID: 270
		private float _initialPanRotation;

		// Token: 0x0400010F RID: 271
		private float RenderScale = 1f;

		// Token: 0x04000110 RID: 272
		private string _stringId = "";

		// Token: 0x04000111 RID: 273
		private int _ammo;

		// Token: 0x04000112 RID: 274
		private int _averageUnitCost;

		// Token: 0x04000113 RID: 275
		private string _itemModifierId = "";

		// Token: 0x04000114 RID: 276
		private string _bannerCode = "";

		// Token: 0x04000115 RID: 277
		private ItemRosterElement _itemRosterElement;

		// Token: 0x04000116 RID: 278
		private MatrixFrame _initialFrame;

		// Token: 0x04000117 RID: 279
		private bool _lockMouse;
	}
}
