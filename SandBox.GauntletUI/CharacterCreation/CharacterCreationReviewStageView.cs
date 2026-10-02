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
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.CharacterCreation
{
	// Token: 0x0200004E RID: 78
	[CharacterCreationStageView(typeof(CharacterCreationReviewStage))]
	public class CharacterCreationReviewStageView : CharacterCreationStageViewBase
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0001656C File Offset: 0x0001476C
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00016574 File Offset: 0x00014774
		public SceneLayer CharacterLayer { get; private set; }

		// Token: 0x060003AD RID: 941 RVA: 0x00016580 File Offset: 0x00014780
		public CharacterCreationReviewStageView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
			: base(affirmativeAction, negativeAction, onRefresh, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction)
		{
			this._characterCreationManager = characterCreationManager;
			this._affirmativeActionText = new TextObject("{=Rvr1bcu8}Next", null);
			this._negativeActionText = negativeActionText;
			this.GauntletLayer = new GauntletLayer("CharacterCreationReview", 1, false);
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.IsFocusLayer = true;
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			ScreenManager.TrySetFocus(this.GauntletLayer);
			bool flag = this._characterCreationManager.GetStage<CharacterCreationBannerEditorStage>() != null && this._characterCreationManager.GetStage<CharacterCreationClanNamingStage>() != null;
			this._dataSource = new CharacterCreationReviewStageVM(this._characterCreationManager, new Action(this.NextStage), this._affirmativeActionText, new Action(this.PreviousStage), this._negativeActionText, flag);
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			GameAxisKey gameAxisKey = HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisX");
			this._dataSource.AddCameraControlInputKey(gameAxisKey, Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey.Id));
			this._movie = this.GauntletLayer.LoadMovie("CharacterCreationReviewStage", this._dataSource);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00016739 File Offset: 0x00014939
		public override void SetGenericScene(Scene scene)
		{
			this.OpenScene(scene);
			this.AddCharacterEntity();
			this.RefreshMountEntity();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00016750 File Offset: 0x00014950
		private void OpenScene(Scene cachedScene)
		{
			this._characterScene = cachedScene;
			this._characterScene.SetShadow(true);
			this._characterScene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
			GameEntity gameEntity = this._characterScene.FindEntityWithName("cradle");
			if (gameEntity != null)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
			this._characterScene.SetDoNotWaitForLoadingStatesToRender(true);
			this._characterScene.DisableStaticShadows(true);
			this._camera = Camera.CreateCamera();
			BodyGeneratorView.InitCamera(this._camera, this._cameraPosition);
			this.CharacterLayer = new SceneLayer(false, true);
			this.CharacterLayer.SetScene(this._characterScene);
			this.CharacterLayer.SetCamera(this._camera);
			this.CharacterLayer.SetSceneUsesShadows(true);
			this.CharacterLayer.SetRenderWithPostfx(true);
			this.CharacterLayer.SetPostfxFromConfig();
			this.CharacterLayer.SceneView.SetResolutionScaling(true);
			int num = -1;
			num &= -5;
			this.CharacterLayer.SetPostfxConfigParams(num);
			this.CharacterLayer.SetPostfxFromConfig();
			this.CharacterLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.CharacterLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00016888 File Offset: 0x00014A88
		private void AddCharacterEntity()
		{
			GameEntity gameEntity = this._characterScene.FindEntityWithTag("spawnpoint_player_1");
			this._initialCharacterFrame = gameEntity.GetFrame();
			this._initialCharacterFrame.origin.z = 0f;
			CharacterObject characterObject = Hero.MainHero.CharacterObject;
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(characterObject.Race);
			AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Equipment(characterObject.Equipment).BodyProperties(characterObject.GetBodyProperties(characterObject.Equipment, -1))
				.SkeletonType(characterObject.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Frame(this._initialCharacterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterObject.IsFemale, "_facegen"))
				.ActionCode(in ActionIndexCache.act_childhood_schooled)
				.Scene(this._characterScene)
				.Race(characterObject.Race)
				.Monster(baseMonsterFromRace)
				.UseTranslucency(true)
				.UseTesselation(true);
			CharacterCreationContent characterCreationContent = (GameStateManager.Current.ActiveState as CharacterCreationState).CharacterCreationManager.CharacterCreationContent;
			Banner selectedBanner = characterCreationContent.SelectedBanner;
			CultureObject selectedCulture = characterCreationContent.SelectedCulture;
			if (selectedBanner != null)
			{
				agentVisualsData.ClothColor1(selectedBanner.GetPrimaryColor());
				agentVisualsData.ClothColor2(selectedBanner.GetFirstIconColor());
			}
			else if (characterCreationContent.SelectedCulture != null)
			{
				agentVisualsData.ClothColor1(selectedCulture.Color);
				agentVisualsData.ClothColor2(selectedCulture.Color2);
			}
			this._agentVisuals = AgentVisuals.Create(agentVisualsData, "facegenvisual", false, false, true);
			this.CharacterLayer.SetFocusedShadowmap(true, ref this._initialCharacterFrame.origin, 0.59999996f);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00016A10 File Offset: 0x00014C10
		private void RefreshCharacterEntityFrame()
		{
			if (this._agentVisuals != null)
			{
				MatrixFrame initialCharacterFrame = this._initialCharacterFrame;
				initialCharacterFrame.rotation.RotateAboutUp(this._charRotationAmount);
				initialCharacterFrame.rotation.ApplyScaleLocal(this._agentVisuals.GetScale());
				this._agentVisuals.GetEntity().SetFrame(ref initialCharacterFrame, true);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00016A68 File Offset: 0x00014C68
		private void RefreshMountEntity()
		{
			this.RemoveMount();
			if (CharacterObject.PlayerCharacter.HasMount())
			{
				ItemObject item = CharacterObject.PlayerCharacter.Equipment[EquipmentIndex.ArmorItemEndSlot].Item;
				MountCreationKey randomMountKey = MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed());
				GameEntity gameEntity = this._characterScene.FindEntityWithTag("spawnpoint_mount_1");
				HorseComponent horseComponent = item.HorseComponent;
				Monster monster = horseComponent.Monster;
				this._mountEntity = GameEntity.CreateEmpty(this._characterScene, true, true, true);
				AnimationSystemData animationSystemData = monster.FillAnimationSystemData(MBGlobals.GetActionSet(horseComponent.Monster.ActionSetCode), 1f, false);
				this._mountEntity.CreateSkeletonWithActionSet(ref animationSystemData);
				this._mountEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_inventory_idle_start, 0f, -0.2f, true, 0f);
				ItemObject item2 = CharacterObject.PlayerCharacter.Equipment[EquipmentIndex.HorseHarness].Item;
				MountVisualCreator.AddMountMeshToEntity(this._mountEntity, item, item2, randomMountKey.ToString(), null);
				MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
				this._mountEntity.SetFrame(ref globalFrame, true);
				this._agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.001f, this._initialCharacterFrame, true);
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00016B9D File Offset: 0x00014D9D
		private void RemoveMount()
		{
			if (this._mountEntity != null)
			{
				this._mountEntity.Remove(118);
			}
			this._mountEntity = null;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00016BC4 File Offset: 0x00014DC4
		public override void Tick(float dt)
		{
			base.Tick(dt);
			base.HandleEscapeMenu(this, this.CharacterLayer);
			Scene characterScene = this._characterScene;
			if (characterScene != null)
			{
				characterScene.Tick(dt);
			}
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals != null)
			{
				agentVisuals.TickVisuals();
			}
			this.TickInput(dt);
			this.HandleLayerInput();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00016C18 File Offset: 0x00014E18
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

		// Token: 0x060003B6 RID: 950 RVA: 0x00016C78 File Offset: 0x00014E78
		private void TickInput(float dt)
		{
			this._dataSource.CharacterGamepadControlsEnabled = Input.IsGamepadActive && this.CharacterLayer.IsHitThisFrame;
			if (this.CharacterLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this.GauntletLayer)
			{
				this.GauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this.GauntletLayer);
				this.CharacterLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this.CharacterLayer);
			}
			else if (!this.CharacterLayer.IsHitThisFrame && ScreenManager.FocusedLayer == this.CharacterLayer)
			{
				this.CharacterLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this.CharacterLayer);
				this.GauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this.GauntletLayer);
			}
			Vec2 vec = new Vec2(this.CharacterLayer.Input.GetNormalizedMouseMoveX() * 1920f, this.CharacterLayer.Input.GetNormalizedMouseMoveY() * 1080f);
			bool flag = this.CharacterLayer.Input.IsHotKeyDown("Rotate");
			if (flag)
			{
				MBWindowManager.DontChangeCursorPos();
				this.GauntletLayer.InputRestrictions.SetMouseVisibility(false);
			}
			else
			{
				this.GauntletLayer.InputRestrictions.SetMouseVisibility(true);
			}
			float num;
			if (Input.IsGamepadActive)
			{
				float gameKeyAxis = this.CharacterLayer.Input.GetGameKeyAxis("CameraAxisX");
				this.NormalizeControllerInputForDeadZone(ref gameKeyAxis, 0.1f);
				num = gameKeyAxis * 400f * dt;
			}
			else
			{
				num = (flag ? vec.x : 0f) * 0.2f;
			}
			this._charRotationAmount = MBMath.WrapAngle(this._charRotationAmount + num * 0.017453292f);
			this.RefreshCharacterEntityFrame();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00016E18 File Offset: 0x00015018
		private void NormalizeControllerInputForDeadZone(ref float inputValue, float controllerDeadZone)
		{
			if (MathF.Abs(inputValue) < controllerDeadZone)
			{
				inputValue = 0f;
				return;
			}
			inputValue = (inputValue - (float)MathF.Sign(inputValue) * controllerDeadZone) / (1f - controllerDeadZone);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00016E43 File Offset: 0x00015043
		private bool IsHotKeyReleasedOnAnyLayer(string hotkeyName)
		{
			return this.GauntletLayer.Input.IsHotKeyReleased(hotkeyName) || this.CharacterLayer.Input.IsHotKeyReleased(hotkeyName);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00016E6C File Offset: 0x0001506C
		public override void NextStage()
		{
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(this._dataSource.Name, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(this._dataSource.Name, null));
			textObject2.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			textObject.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			Hero.MainHero.SetName(textObject2, textObject);
			this.RemoveMount();
			this._affirmativeAction();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00016F20 File Offset: 0x00015120
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.CharacterLayer.SceneView.SetEnable(false);
			this.CharacterLayer.SceneView.ClearAll(false, false);
			this._agentVisuals.Reset();
			this._agentVisuals = null;
			this.GauntletLayer = null;
			CharacterCreationReviewStageVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this.CharacterLayer = null;
			this._characterScene = null;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00016F95 File Offset: 0x00015195
		public override int GetVirtualStageCount()
		{
			return 1;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00016F98 File Offset: 0x00015198
		public override void PreviousStage()
		{
			this.RemoveMount();
			this._negativeAction();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00016FAB File Offset: 0x000151AB
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer> { this.CharacterLayer, this.GauntletLayer };
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016FCA File Offset: 0x000151CA
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00016FFB File Offset: 0x000151FB
		public override void ReleaseEscapeMenuMovie()
		{
			this.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x0400018E RID: 398
		protected readonly TextObject _affirmativeActionText;

		// Token: 0x0400018F RID: 399
		protected readonly TextObject _negativeActionText;

		// Token: 0x04000190 RID: 400
		private readonly GauntletMovieIdentifier _movie;

		// Token: 0x04000191 RID: 401
		private GauntletLayer GauntletLayer;

		// Token: 0x04000192 RID: 402
		private CharacterCreationReviewStageVM _dataSource;

		// Token: 0x04000193 RID: 403
		private readonly CharacterCreationManager _characterCreationManager;

		// Token: 0x04000194 RID: 404
		private Scene _characterScene;

		// Token: 0x04000195 RID: 405
		private Camera _camera;

		// Token: 0x04000196 RID: 406
		private MatrixFrame _initialCharacterFrame;

		// Token: 0x04000197 RID: 407
		private AgentVisuals _agentVisuals;

		// Token: 0x04000198 RID: 408
		private GameEntity _mountEntity;

		// Token: 0x04000199 RID: 409
		private float _charRotationAmount;

		// Token: 0x0400019B RID: 411
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x0400019C RID: 412
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
