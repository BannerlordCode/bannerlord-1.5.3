using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.View.CharacterCreation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.CharacterCreation
{
	// Token: 0x02000049 RID: 73
	[CharacterCreationStageView(typeof(CharacterCreationClanNamingStage))]
	public class CharacterCreationClanNamingStageView : CharacterCreationStageViewBase
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00013D61 File Offset: 0x00011F61
		private ItemRosterElement ShieldRosterElement
		{
			get
			{
				return this._dataSource.ShieldRosterElement;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00013D6E File Offset: 0x00011F6E
		private int ShieldSlotIndex
		{
			get
			{
				return this._dataSource.ShieldSlotIndex;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00013D84 File Offset: 0x00011F84
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00013D7B File Offset: 0x00011F7B
		public SceneLayer SceneLayer { get; private set; }

		// Token: 0x06000357 RID: 855 RVA: 0x00013D8C File Offset: 0x00011F8C
		public CharacterCreationClanNamingStageView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage refreshAction, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
			: base(affirmativeAction, negativeAction, refreshAction, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction)
		{
			this._characterCreationManager = characterCreationManager;
			this._affirmativeActionText = affirmativeActionText;
			this._negativeActionText = negativeActionText;
			this.GauntletLayer = new GauntletLayer("CharacterCreationClanNaming", 1, false)
			{
				IsFocusLayer = true
			};
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			ScreenManager.TrySetFocus(this.GauntletLayer);
			this._character = CharacterObject.PlayerCharacter;
			this._banner = Clan.PlayerClan.Banner;
			this._dataSource = new CharacterCreationClanNamingStageVM(this._character, this._characterCreationManager, new Action(this.NextStage), this._affirmativeActionText, new Action(this.PreviousStage), this._negativeActionText);
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(56));
			this._dataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(57));
			GameAxisKey gameAxisKey = HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisX");
			GameAxisKey gameAxisKey2 = HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisY");
			this._dataSource.AddCameraControlInputKey(gameAxisKey, Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey.Id));
			this._dataSource.AddCameraControlInputKey(gameAxisKey2, Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey2.Id));
			this._clanNamingStageMovie = this.GauntletLayer.LoadMovie("CharacterCreationClanNamingStage", this._dataSource);
			this.CreateScene();
			this.RefreshCharacterEntity();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00013FF0 File Offset: 0x000121F0
		public override void Tick(float dt)
		{
			this.HandleUserInput(dt);
			this.UpdateCamera(dt);
			if (this.SceneLayer != null && this.SceneLayer.ReadyToRender())
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			if (this._scene != null)
			{
				this._scene.Tick(dt);
			}
			base.HandleEscapeMenu(this, this.GauntletLayer);
			this.HandleLayerInput();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00014054 File Offset: 0x00012254
		private void CreateScene()
		{
			this._scene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.SetName("MBBannerEditorScreen");
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true);
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
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.SceneLayer.SetScene(this._scene);
			this.UpdateCamera(0f);
			this.SceneLayer.SetSceneUsesShadows(true);
			this.SceneLayer.SceneView.SetResolutionScaling(true);
			int num = -1;
			num &= -5;
			this.SceneLayer.SetPostfxConfigParams(num);
			this.AddCharacterEntity(in ActionIndexCache.act_walk_idle_1h_with_shield_left_stance);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000141FC File Offset: 0x000123FC
		private void AddCharacterEntity(in ActionIndexCache action)
		{
			this._weaponEquipment = new Equipment();
			int i = 0;
			while (i < 12)
			{
				EquipmentElement equipmentFromSlot = this._character.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) == null)
				{
					goto IL_005E;
				}
				ItemObject item2 = equipmentFromSlot.Item;
				if (((item2 != null) ? item2.PrimaryWeapon : null) != null && !equipmentFromSlot.Item.PrimaryWeapon.IsShield)
				{
					goto IL_005E;
				}
				IL_006B:
				i++;
				continue;
				IL_005E:
				this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				goto IL_006B;
			}
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(this._character.Race);
			this._agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(this._weaponEquipment).BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1)).Frame(this._characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._character.IsFemale, "_facegen"))
				.ActionCode(in action)
				.Scene(this._scene)
				.Race(this._character.Race)
				.Monster(baseMonsterFromRace)
				.SkeletonType(this._character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.PrepareImmediately(true)
				.UseMorphAnims(true), "BannerEditorChar", false, false, true);
			this._agentVisuals.SetAgentLodZeroOrMaxExternal(true);
			this.UpdateBanners();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001434C File Offset: 0x0001254C
		private void UpdateBanners()
		{
			Banner banner = this._banner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			banner.GetTableauTextureLarge(in bannerDebugInfo, new Action<Texture>(this.OnNewBannerReadyForBanners));
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00014384 File Offset: 0x00012584
		private void OnNewBannerReadyForBanners(Texture newTexture)
		{
			if (this._scene == null)
			{
				return;
			}
			GameEntity gameEntity = this._scene.FindEntityWithTag("banner");
			if (gameEntity == null)
			{
				return;
			}
			Mesh mesh = gameEntity.GetFirstMesh();
			if (mesh != null && this._banner != null)
			{
				mesh.GetMaterial().SetTexture(Material.MBTextureType.DiffuseMap2, newTexture);
			}
			gameEntity = this._scene.FindEntityWithTag("banner_2");
			if (gameEntity == null)
			{
				return;
			}
			mesh = gameEntity.GetFirstMesh();
			if (mesh != null && this._banner != null)
			{
				mesh.GetMaterial().SetTexture(Material.MBTextureType.DiffuseMap2, newTexture);
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00014420 File Offset: 0x00012620
		private void RefreshCharacterEntity()
		{
			this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)this.ShieldSlotIndex, this.ShieldRosterElement.EquipmentElement);
			AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
			copyAgentVisualsData.Equipment(this._weaponEquipment).RightWieldedItemIndex(-1).LeftWieldedItemIndex(this.ShieldSlotIndex)
				.Banner(this._banner)
				.ClothColor1(this._banner.GetPrimaryColor())
				.ClothColor2(this._banner.GetFirstIconColor());
			this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
			MissionWeapon shieldWeapon = new MissionWeapon(this.ShieldRosterElement.EquipmentElement.Item, this.ShieldRosterElement.EquipmentElement.ItemModifier, this._banner);
			Action<Texture> action = delegate(Texture tex)
			{
				shieldWeapon.GetWeaponData(false).TableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
			};
			Banner banner = this._banner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			banner.GetTableauTextureLarge(in bannerDebugInfo, action);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00014520 File Offset: 0x00012720
		private void HandleLayerInput()
		{
			if (this.IsHotKeyReleasedOnAnyLayer("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnPreviousStage();
				return;
			}
			if (this.IsHotKeyReleasedOnAnyLayer("Confirm") && this._dataSource.CanAdvance)
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnNextStage();
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00014580 File Offset: 0x00012780
		private void HandleUserInput(float dt)
		{
			this._dataSource.CharacterGamepadControlsEnabled = Input.IsGamepadActive && this.SceneLayer.IsHitThisFrame;
			if (this.SceneLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this.GauntletLayer)
			{
				this.GauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this.GauntletLayer);
				this.SceneLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this.SceneLayer);
			}
			else if (!this.SceneLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this.SceneLayer)
			{
				this.SceneLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this.SceneLayer);
				this.GauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this.GauntletLayer);
			}
			Vec2 vec = new Vec2(this.SceneLayer.Input.GetNormalizedMouseMoveX() * 1920f, this.SceneLayer.Input.GetNormalizedMouseMoveY() * 1080f);
			bool flag = this.SceneLayer.Input.IsHotKeyDown("Zoom");
			bool flag2 = this.SceneLayer.Input.IsHotKeyDown("Rotate");
			bool flag3 = this.SceneLayer.Input.IsHotKeyDown("Ascend");
			if (flag || flag2 || flag3)
			{
				MBWindowManager.DontChangeCursorPos();
				this.GauntletLayer.InputRestrictions.SetMouseVisibility(false);
			}
			else
			{
				this.GauntletLayer.InputRestrictions.SetMouseVisibility(true);
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
			this._cameraTargetElevationAdder = MBMath.ClampFloat(this._cameraTargetElevationAdder + num7, 0.5f, 1.9f * this._agentVisuals.GetScale());
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000148A2 File Offset: 0x00012AA2
		private void NormalizeControllerInputForDeadZone(ref float inputValue, float controllerDeadZone)
		{
			if (MathF.Abs(inputValue) < controllerDeadZone)
			{
				inputValue = 0f;
				return;
			}
			inputValue = (inputValue - (float)MathF.Sign(inputValue) * controllerDeadZone) / (1f - controllerDeadZone);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000148D0 File Offset: 0x00012AD0
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
			characterFrame.rotation.RotateAboutForward(-0.18849556f);
			this._camera.Frame = characterFrame;
			this.SceneLayer.SetCamera(this._camera);
			SoundManager.SetListenerFrame(characterFrame);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000149FD File Offset: 0x00012BFD
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer> { this.SceneLayer, this.GauntletLayer };
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00014A1C File Offset: 0x00012C1C
		public override int GetVirtualStageCount()
		{
			return 1;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00014A20 File Offset: 0x00012C20
		public override void NextStage()
		{
			TextObject textObject = new TextObject(this._dataSource.ClanName, null);
			TextObject textObject2 = GameTexts.FindText("str_generic_clan_name", null);
			textObject2.SetTextVariable("CLAN_NAME", textObject);
			Clan.PlayerClan.ChangeClanName(textObject2, textObject2);
			ControlCharacterCreationStage affirmativeAction = this._affirmativeAction;
			if (affirmativeAction == null)
			{
				return;
			}
			affirmativeAction();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00014A74 File Offset: 0x00012C74
		public override void PreviousStage()
		{
			ControlCharacterCreationStage negativeAction = this._negativeAction;
			if (negativeAction == null)
			{
				return;
			}
			negativeAction();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00014A88 File Offset: 0x00012C88
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.SceneLayer.SceneView.SetEnable(false);
			this.SceneLayer.SceneView.ClearAll(true, true);
			this.GauntletLayer = null;
			this.SceneLayer = null;
			CharacterCreationClanNamingStageVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._clanNamingStageMovie = null;
			this._agentVisuals.Reset();
			this._agentVisuals = null;
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			Scene scene = this._scene;
			if (scene != null)
			{
				scene.ManualInvalidate();
			}
			this._scene = null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00014B2E File Offset: 0x00012D2E
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00014B5F File Offset: 0x00012D5F
		public override void ReleaseEscapeMenuMovie()
		{
			this.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00014B80 File Offset: 0x00012D80
		private bool IsHotKeyReleasedOnAnyLayer(string hotkeyName)
		{
			return this.GauntletLayer.Input.IsHotKeyReleased(hotkeyName) || this.SceneLayer.Input.IsHotKeyReleased(hotkeyName);
		}

		// Token: 0x0400014C RID: 332
		private CharacterCreationManager _characterCreationManager;

		// Token: 0x0400014D RID: 333
		private GauntletLayer GauntletLayer;

		// Token: 0x0400014E RID: 334
		private CharacterCreationClanNamingStageVM _dataSource;

		// Token: 0x0400014F RID: 335
		private GauntletMovieIdentifier _clanNamingStageMovie;

		// Token: 0x04000150 RID: 336
		private TextObject _affirmativeActionText;

		// Token: 0x04000151 RID: 337
		private TextObject _negativeActionText;

		// Token: 0x04000152 RID: 338
		private Banner _banner;

		// Token: 0x04000153 RID: 339
		private float _cameraCurrentRotation;

		// Token: 0x04000154 RID: 340
		private float _cameraTargetRotation;

		// Token: 0x04000155 RID: 341
		private float _cameraCurrentDistanceAdder;

		// Token: 0x04000156 RID: 342
		private float _cameraTargetDistanceAdder;

		// Token: 0x04000157 RID: 343
		private float _cameraCurrentElevationAdder;

		// Token: 0x04000158 RID: 344
		private float _cameraTargetElevationAdder;

		// Token: 0x04000159 RID: 345
		private readonly BasicCharacterObject _character;

		// Token: 0x0400015A RID: 346
		private Scene _scene;

		// Token: 0x0400015B RID: 347
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x0400015C RID: 348
		private AgentVisuals _agentVisuals;

		// Token: 0x0400015D RID: 349
		private MatrixFrame _characterFrame;

		// Token: 0x0400015E RID: 350
		private Equipment _weaponEquipment;

		// Token: 0x0400015F RID: 351
		private Camera _camera;

		// Token: 0x04000161 RID: 353
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x04000162 RID: 354
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
