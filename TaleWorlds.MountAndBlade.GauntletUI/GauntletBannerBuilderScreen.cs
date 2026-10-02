using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder;
using TaleWorlds.ObjectSystem;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000006 RID: 6
	[GameStateScreen(typeof(BannerBuilderState))]
	public class GauntletBannerBuilderScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002333 File Offset: 0x00000533
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000232A File Offset: 0x0000052A
		public SceneLayer SceneLayer { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x0000233B File Offset: 0x0000053B
		public GauntletBannerBuilderScreen(BannerBuilderState state)
		{
			this._state = state;
			this._character = MBObjectManager.Instance.GetObject<BasicCharacterObject>("main_hero");
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002368 File Offset: 0x00000568
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._bannerIconsCategory = UIResourceManager.LoadSpriteCategory("ui_bannericons");
			this._bannerBuilderCategory = UIResourceManager.LoadSpriteCategory("ui_bannerbuilder");
			this._agentVisuals = new AgentVisuals[2];
			string text = (string.IsNullOrWhiteSpace(this._state.DefaultBannerKey) ? "11.163.166.1528.1528.764.764.1.0.0.133.171.171.483.483.764.764.0.0.0" : this._state.DefaultBannerKey);
			this._dataSource = new BannerBuilderVM(this._character, text, new Action<bool>(this.Exit), new Action(this.Refresh), new Action(this.CopyBannerCode));
			this._gauntletLayer = new GauntletLayer("BannerBuilder", 100, false);
			this._gauntletLayer.IsFocusLayer = true;
			base.AddLayer(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._movie = this._gauntletLayer.LoadMovie("BannerBuilderScreen", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this.CreateScene();
			base.AddLayer(this.SceneLayer);
			this._checkWhetherAgentVisualIsReady = true;
			this._firstCharacterRender = true;
			this.RefreshShieldAndCharacter();
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002503 File Offset: 0x00000703
		private void Refresh()
		{
			this.RefreshShieldAndCharacter();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000250C File Offset: 0x0000070C
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._isFinalized)
			{
				return;
			}
			this.HandleUserInput(dt);
			if (this._isFinalized)
			{
				return;
			}
			this.UpdateCamera(dt);
			SceneLayer sceneLayer = this.SceneLayer;
			if (sceneLayer != null && sceneLayer.ReadyToRender())
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			Scene scene = this._scene;
			if (scene != null)
			{
				scene.Tick(dt);
			}
			if (this._refreshBannersNextFrame)
			{
				this.UpdateBanners();
				this._refreshBannersNextFrame = false;
			}
			if (this._refreshCharacterAndShieldNextFrame)
			{
				this.RefreshShieldAndCharacterAux();
				this._refreshCharacterAndShieldNextFrame = false;
			}
			if (this._checkWhetherAgentVisualIsReady)
			{
				int num = (this._agentVisualToShowIndex + 1) % 2;
				if (this._agentVisuals[this._agentVisualToShowIndex].GetEntity().CheckResources(this._firstCharacterRender, true))
				{
					this._agentVisuals[num].SetVisible(false);
					this._agentVisuals[this._agentVisualToShowIndex].SetVisible(true);
					this._checkWhetherAgentVisualIsReady = false;
					this._firstCharacterRender = false;
					return;
				}
				if (!this._firstCharacterRender)
				{
					this._agentVisuals[num].SetVisible(true);
				}
				this._agentVisuals[this._agentVisualToShowIndex].SetVisible(false);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002624 File Offset: 0x00000824
		private void CreateScene()
		{
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.Battle, "mono_renderscene");
			this._scene.SetName("BannerBuilderScreen");
			SceneInitializationData sceneInitializationData = default(SceneInitializationData);
			sceneInitializationData.InitPhysicsWorld = false;
			this._scene.Read("banner_editor_scene", ref sceneInitializationData, "");
			this._scene.SetShadow(true);
			this._scene.DisableStaticShadows(true);
			this._scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._scene);
			float aspectRatio = Screen.AspectRatio;
			GameEntity gameEntity = this._scene.FindEntityWithTag("spawnpoint_player");
			this._characterFrame = gameEntity.GetFrame();
			this._characterFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			this._cameraTargetDistanceAdder = 3.5f;
			this._cameraCurrentDistanceAdder = this._cameraTargetDistanceAdder;
			this._cameraTargetElevationAdder = 1.15f;
			this._cameraCurrentElevationAdder = this._cameraTargetElevationAdder;
			this._camera = Camera.CreateCamera();
			this._camera.SetFovVertical(0.6981317f, aspectRatio, 0.2f, 200f);
			this.SceneLayer = new SceneLayer(true, true);
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.SceneLayer.SetScene(this._scene);
			this.UpdateCamera(0f);
			this.SceneLayer.SetSceneUsesShadows(true);
			this.SceneLayer.SceneView.SetResolutionScaling(true);
			int num = -1;
			num &= -5;
			this.SceneLayer.SetPostfxConfigParams(num);
			this.AddCharacterEntities(in ActionIndexCache.act_walk_idle_1h_with_shield_left_stance);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027BC File Offset: 0x000009BC
		private void AddCharacterEntities(in ActionIndexCache action)
		{
			this._weaponEquipment = new Equipment();
			for (int i = 0; i < 12; i++)
			{
				EquipmentElement equipmentFromSlot = this._character.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) == null || (!equipmentFromSlot.Item.PrimaryWeapon.IsShield && !equipmentFromSlot.Item.ItemFlags.HasAllFlags(ItemFlags.DropOnWeaponChange)))
				{
					this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				}
			}
			this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)this._dataSource.ShieldSlotIndex, this._dataSource.ShieldRosterElement.EquipmentElement);
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(this._character.Race);
			this._agentVisuals[0] = AgentVisuals.Create(new AgentVisualsData().Equipment(this._weaponEquipment).BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1)).Frame(this._characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._character.IsFemale, "_facegen"))
				.ActionCode(in action)
				.Scene(this._scene)
				.Monster(baseMonsterFromRace)
				.SkeletonType(this._character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Race(this._character.Race)
				.PrepareImmediately(true)
				.RightWieldedItemIndex(-1)
				.LeftWieldedItemIndex(this._dataSource.ShieldSlotIndex)
				.ClothColor1(this._dataSource.CurrentBanner.GetPrimaryColor())
				.ClothColor2(this._dataSource.CurrentBanner.GetFirstIconColor())
				.Banner(this._dataSource.CurrentBanner)
				.UseMorphAnims(true), "BannerEditorChar", false, false, true);
			this._agentVisuals[0].SetAgentLodZeroOrMaxExternal(true);
			this._agentVisuals[0].Refresh(false, this._agentVisuals[0].GetCopyAgentVisualsData(), true);
			MissionWeapon shieldWeapon = new MissionWeapon(this._dataSource.ShieldRosterElement.EquipmentElement.Item, this._dataSource.ShieldRosterElement.EquipmentElement.ItemModifier, this._dataSource.CurrentBanner);
			Action<TaleWorlds.Engine.Texture> action2 = delegate(TaleWorlds.Engine.Texture tex)
			{
				shieldWeapon.GetWeaponData(false).TableauMaterial.SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, tex);
			};
			Banner currentBanner = this._dataSource.CurrentBanner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			currentBanner.GetTableauTextureLarge(in bannerDebugInfo, action2);
			this._agentVisuals[0].SetVisible(false);
			this._agentVisuals[0].GetEntity().CheckResources(true, true);
			this._agentVisuals[1] = AgentVisuals.Create(new AgentVisualsData().Equipment(this._weaponEquipment).BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1)).Frame(this._characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._character.IsFemale, "_facegen"))
				.ActionCode(in action)
				.Scene(this._scene)
				.Race(this._character.Race)
				.Monster(baseMonsterFromRace)
				.SkeletonType(this._character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.PrepareImmediately(true)
				.RightWieldedItemIndex(-1)
				.LeftWieldedItemIndex(this._dataSource.ShieldSlotIndex)
				.Banner(this._dataSource.CurrentBanner)
				.ClothColor1(this._dataSource.CurrentBanner.GetPrimaryColor())
				.ClothColor2(this._dataSource.CurrentBanner.GetFirstIconColor())
				.UseMorphAnims(true), "BannerEditorChar", false, false, true);
			this._agentVisuals[1].SetAgentLodZeroOrMaxExternal(true);
			this._agentVisuals[1].Refresh(false, this._agentVisuals[1].GetCopyAgentVisualsData(), true);
			this._agentVisuals[1].SetVisible(false);
			this._agentVisuals[1].GetEntity().CheckResources(true, true);
			this.UpdateBanners();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B90 File Offset: 0x00000D90
		private void UpdateBanners()
		{
			Banner currentBanner = this._dataSource.CurrentBanner;
			Banner currentBanner2 = this._dataSource.CurrentBanner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			BannerTextureCreationData bannerTextureCreationData;
			currentBanner2.GetTableauTextureLarge(in bannerDebugInfo, delegate(TaleWorlds.Engine.Texture resultTexture)
			{
				this.OnNewBannerReadyForBanners(currentBanner, resultTexture);
			}, out bannerTextureCreationData);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BF0 File Offset: 0x00000DF0
		private void OnNewBannerReadyForBanners(Banner bannerOfTexture, TaleWorlds.Engine.Texture newTexture)
		{
			if (!this._isFinalized && this._scene != null)
			{
				Banner currentBanner = this._currentBanner;
				if (((currentBanner != null) ? currentBanner.BannerCode : null) == bannerOfTexture.BannerCode)
				{
					GameEntity gameEntity = this._scene.FindEntityWithTag("banner");
					if (gameEntity != null)
					{
						Mesh firstMesh = gameEntity.GetFirstMesh();
						if (firstMesh != null && this._dataSource.CurrentBanner != null)
						{
							firstMesh.GetMaterial().SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, newTexture);
						}
					}
					else
					{
						gameEntity = this._scene.FindEntityWithTag("banner_2");
						Mesh firstMesh2 = gameEntity.GetFirstMesh();
						if (firstMesh2 != null && this._dataSource.CurrentBanner != null)
						{
							firstMesh2.GetMaterial().SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, newTexture);
						}
					}
					this._refreshCharacterAndShieldNextFrame = true;
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002CC3 File Offset: 0x00000EC3
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._bannerIconsCategory.Unload();
			this._bannerBuilderCategory.Unload();
			this._dataSource.OnFinalize();
			this._isFinalized = true;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002CF3 File Offset: 0x00000EF3
		private void RefreshShieldAndCharacter()
		{
			this._currentBanner = this._dataSource.CurrentBanner;
			this._dataSource.BannerCodeAsString = this._currentBanner.BannerCode;
			this._refreshBannersNextFrame = true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002D24 File Offset: 0x00000F24
		private void RefreshShieldAndCharacterAux()
		{
			int agentVisualToShowIndex = this._agentVisualToShowIndex;
			this._agentVisualToShowIndex = (this._agentVisualToShowIndex + 1) % 2;
			AgentVisualsData copyAgentVisualsData = this._agentVisuals[this._agentVisualToShowIndex].GetCopyAgentVisualsData();
			copyAgentVisualsData.Equipment(this._weaponEquipment).RightWieldedItemIndex(-1).LeftWieldedItemIndex(this._dataSource.ShieldSlotIndex)
				.Banner(this._dataSource.CurrentBanner)
				.Frame(this._characterFrame)
				.BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1))
				.ClothColor1(this._dataSource.CurrentBanner.GetPrimaryColor())
				.ClothColor2(this._dataSource.CurrentBanner.GetFirstIconColor());
			this._agentVisuals[this._agentVisualToShowIndex].Refresh(false, copyAgentVisualsData, true);
			this._agentVisuals[this._agentVisualToShowIndex].GetEntity().CheckResources(true, true);
			this._agentVisuals[this._agentVisualToShowIndex].GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.001f, this._characterFrame, true);
			this._agentVisuals[this._agentVisualToShowIndex].SetVisible(false);
			this._agentVisuals[this._agentVisualToShowIndex].SetVisible(true);
			this._checkWhetherAgentVisualIsReady = true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002E60 File Offset: 0x00001060
		private void HandleUserInput(float dt)
		{
			if (this._gauntletLayer.IsFocusedOnInput())
			{
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm") || this.SceneLayer.Input.IsHotKeyReleased("Confirm"))
			{
				this._dataSource.ExecuteDone();
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit") || this.SceneLayer.Input.IsHotKeyReleased("Exit"))
			{
				this._dataSource.ExecuteCancel();
				return;
			}
			if (this.SceneLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this._gauntletLayer)
			{
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				this.SceneLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this.SceneLayer);
			}
			else if (!this.SceneLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this.SceneLayer)
			{
				this.SceneLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this.SceneLayer);
				this._gauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
			Vec2 vec = new Vec2(this.SceneLayer.Input.GetNormalizedMouseMoveX() * 1920f, this.SceneLayer.Input.GetNormalizedMouseMoveY() * 1080f);
			bool flag = this.SceneLayer.Input.IsHotKeyDown("Zoom");
			bool flag2 = this.SceneLayer.Input.IsHotKeyDown("Rotate");
			bool flag3 = this.SceneLayer.Input.IsHotKeyDown("Ascend");
			if (flag || flag2 || flag3)
			{
				MBWindowManager.DontChangeCursorPos();
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(false);
			}
			else
			{
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(true);
			}
			float gameKeyState = this.SceneLayer.Input.GetGameKeyState(56);
			float num = this.SceneLayer.Input.GetGameKeyState(57) - gameKeyState;
			float num2;
			if (Input.IsGamepadActive)
			{
				this.NormalizeControllerInputForDeadZone(ref num, 0.1f);
				num2 = num * 5f * dt;
			}
			else
			{
				float num3 = this.SceneLayer.Input.GetDeltaMouseScroll() * -1f;
				float num4 = (flag ? (vec.y * -1f) : 0f);
				num2 = num3 * 0.002f + num4 * 0.004f;
			}
			this._cameraTargetDistanceAdder = MBMath.ClampFloat(this._cameraTargetDistanceAdder + num2, 1.5f, 5f);
			float num6;
			if (Input.IsGamepadActive)
			{
				float num5 = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX") * -1f;
				this.NormalizeControllerInputForDeadZone(ref num5, 0.1f);
				num6 = num5 * 600f * this.SceneLayer.Input.GetMouseSensitivity() * dt;
			}
			else
			{
				num6 = (flag2 ? (vec.x * -1f) : 0f) * 0.3f * this.SceneLayer.Input.GetMouseSensitivity();
			}
			this._cameraTargetRotation = MBMath.WrapAngle(this._cameraTargetRotation + num6 * 0.017453292f);
			float num7;
			if (Input.IsGamepadActive)
			{
				float gameKeyAxis = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
				this.NormalizeControllerInputForDeadZone(ref gameKeyAxis, 0.1f);
				num7 = gameKeyAxis * 2f * dt;
			}
			else
			{
				num7 = (flag3 ? vec.y : 0f) * 0.002f;
			}
			this._cameraTargetElevationAdder = MBMath.ClampFloat(this._cameraTargetElevationAdder + num7, 0.5f, 1.9f * this._agentVisuals[this._agentVisualToShowIndex].GetScale());
			if (Input.DebugInput.IsHotKeyPressed("Copy"))
			{
				this.CopyBannerCode();
			}
			if (Input.DebugInput.IsHotKeyPressed("Duplicate"))
			{
				this._dataSource.ExecuteDuplicateCurrentLayer();
			}
			if (Input.DebugInput.IsHotKeyPressed("Paste"))
			{
				this._dataSource.SetBannerCode(Input.GetClipboardText());
				this.RefreshShieldAndCharacter();
			}
			if (Input.DebugInput.IsKeyPressed(InputKey.Delete))
			{
				this._dataSource.DeleteCurrentLayer();
			}
			Vec2 vec2 = new Vec2(0f, 0f);
			if (Input.DebugInput.IsKeyReleased(InputKey.Left))
			{
				vec2.x = -1f;
			}
			else if (Input.DebugInput.IsKeyReleased(InputKey.Right))
			{
				vec2.x = 1f;
			}
			if (Input.DebugInput.IsKeyReleased(InputKey.Down))
			{
				vec2.y = 1f;
			}
			else if (Input.DebugInput.IsKeyReleased(InputKey.Up))
			{
				vec2.y = -1f;
			}
			if (vec2.x != 0f || vec2.y != 0f)
			{
				this._dataSource.TranslateCurrentLayerWith(vec2);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003313 File Offset: 0x00001513
		private void NormalizeControllerInputForDeadZone(ref float inputValue, float controllerDeadZone)
		{
			if (MathF.Abs(inputValue) < controllerDeadZone)
			{
				inputValue = 0f;
				return;
			}
			inputValue = (inputValue - (float)MathF.Sign(inputValue) * controllerDeadZone) / (1f - controllerDeadZone);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003340 File Offset: 0x00001540
		private void UpdateCamera(float dt)
		{
			float num = MathF.Min(1f, 10f * dt);
			this._cameraCurrentRotation = MathF.AngleLerp(this._cameraCurrentRotation, this._cameraTargetRotation, num, 1E-05f);
			this._cameraCurrentElevationAdder = MathF.Lerp(this._cameraCurrentElevationAdder, this._cameraTargetElevationAdder, num, 1E-05f);
			this._cameraCurrentDistanceAdder = MathF.Lerp(this._cameraCurrentDistanceAdder, this._cameraTargetDistanceAdder, num, 1E-05f);
			MatrixFrame characterFrame = this._characterFrame;
			characterFrame.rotation.RotateAboutUp(this._cameraCurrentRotation);
			characterFrame.origin += this._cameraCurrentElevationAdder * characterFrame.rotation.u + this._cameraCurrentDistanceAdder * characterFrame.rotation.f;
			characterFrame.rotation.RotateAboutSide(-1.5707964f);
			characterFrame.rotation.RotateAboutUp(3.1415927f);
			characterFrame.rotation.RotateAboutForward(0.18849556f);
			this._camera.Frame = characterFrame;
			this.SceneLayer.SetCamera(this._camera);
			SoundManager.SetListenerFrame(characterFrame);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000346D File Offset: 0x0000166D
		private void CopyBannerCode()
		{
			Input.SetClipboardText(this._dataSource.GetBannerCode());
			InformationManager.DisplayMessage(new InformationMessage("Banner code copied to the clipboard."));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000348E File Offset: 0x0000168E
		public void Exit(bool isCancel)
		{
			MouseManager.ActivateMouseCursor(CursorType.Default);
			GameStateManager.Current.PopState(0);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000034A1 File Offset: 0x000016A1
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000034A4 File Offset: 0x000016A4
		void IGameStateListener.OnDeactivate()
		{
			this._agentVisuals[0].Reset();
			this._agentVisuals[1].Reset();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			Scene scene = this._scene;
			if (scene != null)
			{
				scene.ManualInvalidate();
			}
			this._scene = null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000034FC File Offset: 0x000016FC
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000034FE File Offset: 0x000016FE
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x0400000A RID: 10
		private BannerBuilderVM _dataSource;

		// Token: 0x0400000B RID: 11
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400000C RID: 12
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400000D RID: 13
		private SpriteCategory _bannerIconsCategory;

		// Token: 0x0400000E RID: 14
		private SpriteCategory _bannerBuilderCategory;

		// Token: 0x0400000F RID: 15
		private BannerBuilderState _state;

		// Token: 0x04000010 RID: 16
		private bool _isFinalized;

		// Token: 0x04000011 RID: 17
		private Camera _camera;

		// Token: 0x04000012 RID: 18
		private AgentVisuals[] _agentVisuals;

		// Token: 0x04000013 RID: 19
		private Scene _scene;

		// Token: 0x04000014 RID: 20
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x04000015 RID: 21
		private MatrixFrame _characterFrame;

		// Token: 0x04000016 RID: 22
		private Equipment _weaponEquipment;

		// Token: 0x04000017 RID: 23
		private Banner _currentBanner;

		// Token: 0x04000018 RID: 24
		private float _cameraCurrentRotation;

		// Token: 0x04000019 RID: 25
		private float _cameraTargetRotation;

		// Token: 0x0400001A RID: 26
		private float _cameraCurrentDistanceAdder;

		// Token: 0x0400001B RID: 27
		private float _cameraTargetDistanceAdder;

		// Token: 0x0400001C RID: 28
		private float _cameraCurrentElevationAdder;

		// Token: 0x0400001D RID: 29
		private float _cameraTargetElevationAdder;

		// Token: 0x0400001E RID: 30
		private int _agentVisualToShowIndex;

		// Token: 0x0400001F RID: 31
		private bool _refreshCharacterAndShieldNextFrame;

		// Token: 0x04000020 RID: 32
		private bool _refreshBannersNextFrame;

		// Token: 0x04000021 RID: 33
		private bool _checkWhetherAgentVisualIsReady;

		// Token: 0x04000022 RID: 34
		private bool _firstCharacterRender = true;

		// Token: 0x04000023 RID: 35
		private BasicCharacterObject _character;

		// Token: 0x04000024 RID: 36
		private const string DefaultBannerKey = "11.163.166.1528.1528.764.764.1.0.0.133.171.171.483.483.764.764.0.0.0";
	}
}
