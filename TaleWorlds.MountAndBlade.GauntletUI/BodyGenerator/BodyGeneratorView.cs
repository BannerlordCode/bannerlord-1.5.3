using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator;
using TaleWorlds.ObjectSystem;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator
{
	// Token: 0x02000041 RID: 65
	public class BodyGeneratorView : IFaceGeneratorHandler
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00011C7A File Offset: 0x0000FE7A
		private IInputContext DebugInput
		{
			get
			{
				return Input.DebugInput;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00011C81 File Offset: 0x0000FE81
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00011C89 File Offset: 0x0000FE89
		public FaceGenVM DataSource { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00011C92 File Offset: 0x0000FE92
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00011C9A File Offset: 0x0000FE9A
		public GauntletLayer GauntletLayer { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00011CA3 File Offset: 0x0000FEA3
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00011CAB File Offset: 0x0000FEAB
		public SceneLayer SceneLayer { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00011CBC File Offset: 0x0000FEBC
		public BodyGenerator BodyGen { get; private set; }

		// Token: 0x06000301 RID: 769 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		public BodyGeneratorView(ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, BasicCharacterObject character, bool openedFromMultiplayer, IFaceGeneratorCustomFilter filter, Equipment dressedEquipment = null, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction = null, ControlCharacterCreationStageReturnInt getTotalStageCountAction = null, ControlCharacterCreationStageReturnInt getFurthestIndexAction = null, ControlCharacterCreationStageWithInt goToIndexAction = null, FaceGenHistory faceGenHistory = null)
		{
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._getCurrentStageIndexAction = getCurrentStageIndexAction;
			this._getTotalStageCountAction = getTotalStageCountAction;
			this._getFurthestIndexAction = getFurthestIndexAction;
			this._goToIndexAction = goToIndexAction;
			this._openedFromMultiplayer = openedFromMultiplayer;
			this.BodyGen = new BodyGenerator(character);
			this._dressedEquipment = dressedEquipment ?? this.BodyGen.Character.Equipment.Clone(false);
			if (!this._dressedEquipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty && this._dressedEquipment[EquipmentIndex.ExtraWeaponSlot].Item.IsBannerItem)
			{
				this._dressedEquipment[EquipmentIndex.ExtraWeaponSlot] = EquipmentElement.Invalid;
			}
			FaceGenerationParams faceGenerationParams = this.BodyGen.InitBodyGenerator(false);
			faceGenerationParams.UseCache = true;
			faceGenerationParams.UseGpuMorph = true;
			this.SkeletonType = (this.BodyGen.IsFemale ? SkeletonType.Female : SkeletonType.Male);
			this._facegenCategory = UIResourceManager.LoadSpriteCategory("ui_facegen");
			this.OpenScene();
			this.AddCharacterEntity();
			bool openedFromMultiplayer2 = this._openedFromMultiplayer;
			if (this._getCurrentStageIndexAction == null || this._getTotalStageCountAction == null || this._getFurthestIndexAction == null)
			{
				this.DataSource = new FaceGenVM(this.BodyGen, this, new Action<float>(this.OnHeightChanged), new Action(this.OnAgeChanged), affirmativeActionText, negativeActionText, 0, 0, 0, new Action<int>(this.GoToIndex), openedFromMultiplayer2, openedFromMultiplayer, filter);
			}
			else
			{
				this.DataSource = new FaceGenVM(this.BodyGen, this, new Action<float>(this.OnHeightChanged), new Action(this.OnAgeChanged), affirmativeActionText, negativeActionText, this._getCurrentStageIndexAction(), this._getTotalStageCountAction(), this._getFurthestIndexAction(), new Action<int>(this.GoToIndex), true, openedFromMultiplayer, filter);
			}
			this.DataSource.InitializeHistory(faceGenHistory);
			this.DataSource.SetPreviousTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"));
			this.DataSource.SetNextTabInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"));
			this.DataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this.DataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(56));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(57));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisX"));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisY"));
			this.DataSource.SetFaceGenerationParams(faceGenerationParams);
			this.DataSource.Refresh(true);
			this.GauntletLayer = new GauntletLayer("Facegen", 1, false);
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.GauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this.GauntletLayer);
			this._viewMovie = this.GauntletLayer.LoadMovie("FaceGen", this.DataSource);
			if (!this._openedFromMultiplayer)
			{
				this._templateBodyProperties = new List<BodyProperties>();
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_0").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_1").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_2").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_3").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_4").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_5").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_6").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_7").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_8").GetBodyProperties(null, -1));
				this._templateBodyProperties.Add(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_9").GetBodyProperties(null, -1));
			}
			((IFaceGeneratorHandler)this).RefreshCharacterEntity();
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.DataSource.SelectedGender = (this.BodyGen.IsFemale ? 1 : 0);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00012270 File Offset: 0x00010470
		private void OpenScene()
		{
			this._facegenScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			this._facegenScene.DisableStaticShadows(true);
			SceneInitializationData sceneInitializationData = default(SceneInitializationData);
			sceneInitializationData.InitPhysicsWorld = false;
			this._facegenScene.Read("character_menu_new", ref sceneInitializationData, "");
			this._facegenScene.SetClothSimulationState(true);
			this._facegenScene.SetShadow(true);
			this._facegenScene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
			GameEntity gameEntity = this._facegenScene.FindEntityWithName("cradle");
			if (gameEntity != null)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
			this._facegenScene.DisableStaticShadows(true);
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._facegenScene);
			this._camera = Camera.CreateCamera();
			this._defaultCameraGlobalFrame = BodyGeneratorView.InitCamera(this._camera, new Vec3(6.45f, 5.15f, 1.75f, -1f));
			this._targetCameraGlobalFrame = this._defaultCameraGlobalFrame;
			this.SceneLayer = new SceneLayer(true, true);
			this.SceneLayer.IsFocusLayer = true;
			this.SceneLayer.SetScene(this._facegenScene);
			this.SceneLayer.SetCamera(this._camera);
			this.SceneLayer.SetSceneUsesShadows(true);
			this.SceneLayer.SetRenderWithPostfx(true);
			this.SceneLayer.SetPostfxFromConfig();
			this.SceneLayer.SceneView.SetResolutionScaling(true);
			int num = -1;
			num &= -5;
			this.SceneLayer.SetPostfxConfigParams(num);
			this.SceneLayer.SetPostfxFromConfig();
			this.SceneLayer.SceneView.SetAcceptGlobalDebugRenderObjects(true);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00012404 File Offset: 0x00010604
		private void AddCharacterEntity()
		{
			GameEntity gameEntity = this._facegenScene.FindEntityWithTag("spawnpoint_player_1");
			this._initialCharacterFrame = gameEntity.GetFrame();
			this._initialCharacterFrame.origin.z = 0f;
			this._visualToShow = null;
			this._visualsBeingPrepared = new List<KeyValuePair<AgentVisuals, int>>();
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(this.BodyGen.Race);
			AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Equipment(this.BodyGen.Character.Equipment).BodyProperties(this.BodyGen.Character.GetBodyProperties(this.BodyGen.Character.Equipment, -1))
				.Race(this.BodyGen.Race)
				.Frame(this._initialCharacterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this.BodyGen.IsFemale, "_facegen"))
				.Scene(this._facegenScene)
				.Monster(baseMonsterFromRace)
				.UseTranslucency(true)
				.UseTesselation(false)
				.PrepareImmediately(true);
			this._nextVisualToShow = AgentVisuals.Create(agentVisualsData, "facegenvisual", false, false, false);
			GameEntity entity = this._nextVisualToShow.GetEntity();
			entity.Skeleton.SetAgentActionChannel(1, in ActionIndexCache.act_inventory_idle_start, 0f, -0.2f, true, 0f);
			this._nextVisualToShow.SetAgentLodZeroOrMaxExternal(true);
			entity.CheckResources(true, true);
			this._nextVisualToShow.SetVisible(false);
			this._visualsBeingPrepared.Add(new KeyValuePair<AgentVisuals, int>(this._nextVisualToShow, 1));
			this.SceneLayer.SetFocusedShadowmap(true, ref this._initialCharacterFrame.origin, 0.59999996f);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00012599 File Offset: 0x00010799
		private void SetNewBodyPropertiesAndBodyGen(BodyProperties bodyProperties)
		{
			this.BodyGen.CurrentBodyProperties = bodyProperties;
			((IFaceGeneratorHandler)this).RefreshCharacterEntity();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000125B0 File Offset: 0x000107B0
		public void ResetFaceToDefault()
		{
			MBBodyProperties.ProduceNumericKeyWithDefaultValues(ref this.BodyGen.CurrentBodyProperties, this.BodyGen.Character.Equipment.EarsAreHidden, this.BodyGen.Character.Equipment.MouthIsHidden, this.BodyGen.Race, this.BodyGen.IsFemale ? 1 : 0, (int)this.BodyGen.Character.Age);
			((IFaceGeneratorHandler)this).RefreshCharacterEntity();
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0001262A File Offset: 0x0001082A
		private void OnHeightChanged(float sliderValue)
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0001262C File Offset: 0x0001082C
		private void OnAgeChanged()
		{
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001262E File Offset: 0x0001082E
		[CommandLineFunctionality.CommandLineArgumentFunction("show_debug", "facegen")]
		public static string FaceGenShowDebug(List<string> strings)
		{
			FaceGen.ShowDebugValues = !FaceGen.ShowDebugValues;
			return "FaceGen: Show Debug Values are " + (FaceGen.ShowDebugValues ? "enabled" : "disabled");
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0001265A File Offset: 0x0001085A
		[CommandLineFunctionality.CommandLineArgumentFunction("toggle_update_deform_keys", "facegen")]
		public static string FaceGenUpdateDeformKeys(List<string> strings)
		{
			FaceGen.UpdateDeformKeys = !FaceGen.UpdateDeformKeys;
			return "FaceGen: update deform keys is now " + (FaceGen.UpdateDeformKeys ? "enabled" : "disabled");
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00012686 File Offset: 0x00010886
		public bool ReadyToRender()
		{
			return this.SceneLayer != null && this.SceneLayer.SceneView != null && this.SceneLayer.SceneView.ReadyToRender();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000126B8 File Offset: 0x000108B8
		public void OnTick(float dt)
		{
			this.TickInput(dt);
			if (this.SceneLayer != null && this.SceneLayer.ReadyToRender())
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			if (this._makeVoiceInFrames >= 0)
			{
				if (this._makeVoiceInFrames == 0)
				{
					((IFaceGeneratorHandler)this).MakeVoice();
				}
				this._makeVoiceInFrames--;
			}
			if (this._refreshCharacterEntityNextFrame)
			{
				this.RefreshCharacterEntityAux();
				this._refreshCharacterEntityNextFrame = false;
			}
			if (this._visualToShow != null)
			{
				Skeleton skeleton = this._visualToShow.GetVisuals().GetSkeleton();
				bool flag = skeleton.GetAnimationParameterAtChannel(1) > 0.6f;
				if (skeleton.GetActionAtChannel(1) == ActionIndexCache.act_command_leftstance && flag)
				{
					this._visualToShow.GetEntity().Skeleton.SetAgentActionChannel(1, in ActionIndexCache.act_inventory_idle, 0f, -0.2f, true, 0f);
				}
			}
			if (!this._openedFromMultiplayer)
			{
				if (this.DebugInput.IsHotKeyReleased("MbFaceGeneratorScreenHotkeySetFaceKeyMin"))
				{
					this.BodyGen.BodyPropertiesMin = this.BodyGen.CurrentBodyProperties;
				}
				else if (this.DebugInput.IsHotKeyReleased("MbFaceGeneratorScreenHotkeySetFaceKeyMax"))
				{
					this.BodyGen.BodyPropertiesMax = this.BodyGen.CurrentBodyProperties;
				}
				else if (this.DebugInput.IsHotKeyPressed("Reset"))
				{
					this.DataSource.ExecuteReset();
				}
			}
			if (this.DebugInput.IsHotKeyReleased("MbFaceGeneratorScreenHotkeySetCurFaceKeyToMin"))
			{
				this.BodyGen.CurrentBodyProperties = this.BodyGen.BodyPropertiesMin;
				this.SetNewBodyPropertiesAndBodyGen(this.BodyGen.BodyPropertiesMin);
				this.DataSource.SetBodyProperties(this.BodyGen.CurrentBodyProperties, false, 0, -1, false);
				this.DataSource.UpdateFacegen();
			}
			else if (this.DebugInput.IsHotKeyReleased("MbFaceGeneratorScreenHotkeySetCurFaceKeyToMax"))
			{
				this.BodyGen.CurrentBodyProperties = this.BodyGen.BodyPropertiesMax;
				this.SetNewBodyPropertiesAndBodyGen(this.BodyGen.BodyPropertiesMax);
				this.DataSource.SetBodyProperties(this.BodyGen.CurrentBodyProperties, false, 0, -1, false);
				this.DataSource.UpdateFacegen();
			}
			if (this.DebugInput.IsHotKeyDown("FaceGeneratorExtendedDebugKey") && this.DebugInput.IsHotKeyDown("MbFaceGeneratorScreenHotkeyResetFaceToDefault"))
			{
				this.ResetFaceToDefault();
				this.DataSource.SetBodyProperties(this.BodyGen.CurrentBodyProperties, false, 0, -1, false);
				this.DataSource.UpdateFacegen();
			}
			Utilities.CheckResourceModifications();
			if (this.DebugInput.IsHotKeyReleased("Refresh"))
			{
				((IFaceGeneratorHandler)this).RefreshCharacterEntity();
			}
			Scene facegenScene = this._facegenScene;
			if (facegenScene != null)
			{
				facegenScene.Tick(dt);
			}
			if (this._visualToShow != null)
			{
				this._visualToShow.TickVisuals();
			}
			foreach (KeyValuePair<AgentVisuals, int> keyValuePair in this._visualsBeingPrepared)
			{
				keyValuePair.Key.TickVisuals();
			}
			for (int i = 0; i < this._visualsBeingPrepared.Count; i++)
			{
				AgentVisuals key = this._visualsBeingPrepared[i].Key;
				int value = this._visualsBeingPrepared[i].Value;
				key.SetVisible(false);
				if (key.GetEntity().CheckResources(false, true))
				{
					if (value > 0)
					{
						this._visualsBeingPrepared[i] = new KeyValuePair<AgentVisuals, int>(key, value - 1);
					}
					else
					{
						if (key == this._nextVisualToShow)
						{
							if (this._visualToShow != null)
							{
								this._visualToShow.Reset();
							}
							this._visualToShow = key;
							this._visualToShow.SetVisible(true);
							this._nextVisualToShow = null;
							if (this._setMorphAnimNextFrame)
							{
								this._visualToShow.GetEntity().Skeleton.SetFacialAnimation(Agent.FacialAnimChannel.High, this._nextMorphAnimToSet, true, this._nextMorphAnimLoopValue);
								this._setMorphAnimNextFrame = false;
							}
						}
						else
						{
							this._visualsBeingPrepared[i].Key.Reset();
						}
						this._visualsBeingPrepared[i] = this._visualsBeingPrepared[this._visualsBeingPrepared.Count - 1];
						this._visualsBeingPrepared.RemoveAt(this._visualsBeingPrepared.Count - 1);
						i--;
					}
				}
			}
			SoundManager.SetListenerFrame(this._camera.Frame);
			this.UpdateCamera(dt);
			this.TickLayerInputs();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00012B04 File Offset: 0x00010D04
		public void OnFinalize()
		{
			this._facegenCategory.Unload();
			this.ClearAgentVisuals();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._facegenScene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			this._facegenScene.ClearAll();
			this._facegenScene.ManualInvalidate();
			this._facegenScene = null;
			this.SceneLayer.SceneView.SetEnable(false);
			this.SceneLayer.SceneView.ClearAll(true, true);
			FaceGenVM dataSource = this.DataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this.DataSource = null;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00012B93 File Offset: 0x00010D93
		private void TickLayerInputs()
		{
			if (this.IsHotKeyReleasedOnAnyLayer("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				((IFaceGeneratorHandler)this).Cancel();
				return;
			}
			if (this.IsHotKeyReleasedOnAnyLayer("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				((IFaceGeneratorHandler)this).Done();
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00012BD0 File Offset: 0x00010DD0
		private void TickInput(float dt)
		{
			this.DataSource.CharacterGamepadControlsEnabled = Input.IsGamepadActive && this.SceneLayer.IsHitThisFrame;
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
			float length = (this._targetCameraGlobalFrame.origin.AsVec2 - this._initialCharacterFrame.origin.AsVec2).Length;
			this._cameraCurrentDistanceAdder = MBMath.ClampFloat(this._cameraCurrentDistanceAdder + num2, 0.3f - length, 3f - length);
			float num5;
			if (Input.IsGamepadActive)
			{
				float gameKeyAxis = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX");
				this.NormalizeControllerInputForDeadZone(ref gameKeyAxis, 0.1f);
				num5 = gameKeyAxis * 400f * dt;
			}
			else
			{
				num5 = (flag2 ? vec.x : 0f) * 0.2f;
			}
			this._characterTargetRotation = MBMath.WrapAngle(this._characterTargetRotation + num5 * 0.017453292f);
			float num6 = ((this._visualToShow != null) ? this._visualToShow.GetScale() : 1f);
			float num7 = 0.15f - this._targetCameraGlobalFrame.origin.z;
			float num8 = 1.9f * num6 - this._targetCameraGlobalFrame.origin.z;
			float num9;
			if (Input.IsGamepadActive)
			{
				float gameKeyAxis2 = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
				this.NormalizeControllerInputForDeadZone(ref gameKeyAxis2, 0.1f);
				num9 = gameKeyAxis2 * 2f * dt;
			}
			else
			{
				num9 = (flag3 ? vec.y : 0f) * 0.002f;
			}
			this._cameraCurrentElevationAdder = MBMath.ClampFloat(this._cameraCurrentElevationAdder + num9, num7, num8);
			if (this.IsHotKeyPressedOnAnyLayer("SwitchToPreviousTab"))
			{
				UISoundsHelper.PlayUISound("event:/ui/tab");
				this.DataSource.SelectPreviousTab();
			}
			else if (this.IsHotKeyPressedOnAnyLayer("SwitchToNextTab"))
			{
				UISoundsHelper.PlayUISound("event:/ui/tab");
				this.DataSource.SelectNextTab();
			}
			if (this.SceneLayer.Input.IsControlDown() || this.GauntletLayer.Input.IsControlDown())
			{
				if (this.IsHotKeyPressedOnAnyLayer("Copy"))
				{
					Input.SetClipboardText(this.BodyGen.CurrentBodyProperties.ToString());
					return;
				}
				if (this.IsHotKeyPressedOnAnyLayer("Paste"))
				{
					BodyProperties bodyProperties;
					if (BodyProperties.FromString(Input.GetClipboardText(), out bodyProperties))
					{
						this.DataSource.SetBodyProperties(bodyProperties, !FaceGen.ShowDebugValues, 0, -1, true);
						return;
					}
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_error", null).ToString(), GameTexts.FindText("str_facegen_error_on_paste", null).ToString(), false, true, "", GameTexts.FindText("str_ok", null).ToString(), null, null, "", 0f, null, null, null), false, false);
				}
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00013053 File Offset: 0x00011253
		private void NormalizeControllerInputForDeadZone(ref float inputValue, float controllerDeadZone)
		{
			if (MathF.Abs(inputValue) < controllerDeadZone)
			{
				inputValue = 0f;
				return;
			}
			inputValue = (inputValue - (float)MathF.Sign(inputValue) * controllerDeadZone) / (1f - controllerDeadZone);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001307E File Offset: 0x0001127E
		private bool IsHotKeyReleasedOnAnyLayer(string hotkeyName)
		{
			return this.GauntletLayer.Input.IsHotKeyReleased(hotkeyName) || this.SceneLayer.Input.IsHotKeyReleased(hotkeyName);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000130A6 File Offset: 0x000112A6
		private bool IsHotKeyPressedOnAnyLayer(string hotkeyName)
		{
			return this.GauntletLayer.Input.IsHotKeyPressed(hotkeyName) || this.SceneLayer.Input.IsHotKeyPressed(hotkeyName);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000130D0 File Offset: 0x000112D0
		private void RefreshCharacterEntityAux()
		{
			SkeletonType skeletonType = this.SkeletonType;
			if (skeletonType < SkeletonType.KidsStart)
			{
				skeletonType = (this.BodyGen.IsFemale ? SkeletonType.Female : SkeletonType.Male);
			}
			this._currentAgentVisualIndex = (this._currentAgentVisualIndex + 1) % 2;
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(this.BodyGen.Race);
			AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Scene(this._facegenScene).Monster(baseMonsterFromRace)
				.UseTranslucency(true)
				.UseTesselation(false)
				.SkeletonType(skeletonType)
				.Equipment(this.IsDressed ? this._dressedEquipment : null)
				.BodyProperties(this.BodyGen.CurrentBodyProperties)
				.Race(this.BodyGen.Race)
				.PrepareImmediately(true);
			AgentVisuals agentVisuals = this._visualToShow ?? this._nextVisualToShow;
			ActionIndexCache actionAtChannel = agentVisuals.GetEntity().Skeleton.GetActionAtChannel(1);
			float animationParameterAtChannel = agentVisuals.GetVisuals().GetSkeleton().GetAnimationParameterAtChannel(1);
			this._nextVisualToShow = AgentVisuals.Create(agentVisualsData, "facegenvisual", false, false, false);
			this._nextVisualToShow.SetAgentLodZeroOrMax(true);
			this._nextVisualToShow.GetEntity().Skeleton.SetAgentActionChannel(1, in actionAtChannel, animationParameterAtChannel, -0.2f, true, 0f);
			this._nextVisualToShow.GetEntity().SetEnforcedMaximumLodLevel(0);
			this._nextVisualToShow.GetEntity().CheckResources(true, true);
			this._nextVisualToShow.SetVisible(false);
			MatrixFrame initialCharacterFrame = this._initialCharacterFrame;
			initialCharacterFrame.rotation.RotateAboutUp(this._characterCurrentRotation);
			initialCharacterFrame.rotation.ApplyScaleLocal(this._nextVisualToShow.GetScale());
			this._nextVisualToShow.GetEntity().SetFrame(ref initialCharacterFrame, true);
			this._nextVisualToShow.GetVisuals().GetSkeleton().SetAnimationParameterAtChannel(1, animationParameterAtChannel);
			this._nextVisualToShow.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.001f, initialCharacterFrame, true);
			this._nextVisualToShow.SetVisible(false);
			this._visualsBeingPrepared.Add(new KeyValuePair<AgentVisuals, int>(this._nextVisualToShow, 1));
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000132CB File Offset: 0x000114CB
		void IFaceGeneratorHandler.MakeVoice()
		{
			AgentVisuals visualToShow = this._visualToShow;
			if (visualToShow == null)
			{
				return;
			}
			visualToShow.MakeRandomVoiceForFacegen();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000132DD File Offset: 0x000114DD
		void IFaceGeneratorHandler.MakeVoiceDelayed()
		{
			this._makeVoiceInFrames = 2;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000132E6 File Offset: 0x000114E6
		void IFaceGeneratorHandler.RefreshCharacterEntity()
		{
			this._refreshCharacterEntityNextFrame = true;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000132EF File Offset: 0x000114EF
		void IFaceGeneratorHandler.SetFacialAnimation(string faceAnimation, bool loop)
		{
			this._setMorphAnimNextFrame = true;
			this._nextMorphAnimToSet = faceAnimation;
			this._nextMorphAnimLoopValue = loop;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00013308 File Offset: 0x00011508
		private void ClearAgentVisuals()
		{
			if (this._visualToShow != null)
			{
				this._visualToShow.Reset();
				this._visualToShow = null;
			}
			foreach (KeyValuePair<AgentVisuals, int> keyValuePair in this._visualsBeingPrepared)
			{
				keyValuePair.Key.Reset();
			}
			this._visualsBeingPrepared.Clear();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00013388 File Offset: 0x00011588
		void IFaceGeneratorHandler.Done()
		{
			this.BodyGen.SaveCurrentCharacter();
			this.ClearAgentVisuals();
			if (Mission.Current != null)
			{
				Mission.Current.MainAgent.UpdateBodyProperties(this.BodyGen.CurrentBodyProperties);
				Mission.Current.MainAgent.EquipItemsFromSpawnEquipment(false, false, false, 0);
			}
			this._affirmativeAction();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000133E5 File Offset: 0x000115E5
		void IFaceGeneratorHandler.Cancel()
		{
			this._negativeAction();
			this.ClearAgentVisuals();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000133F8 File Offset: 0x000115F8
		void IFaceGeneratorHandler.ChangeToFaceCamera()
		{
			this._cameraLookMode = 1;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00013417 File Offset: 0x00011617
		void IFaceGeneratorHandler.ChangeToEyeCamera()
		{
			this._cameraLookMode = 2;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00013436 File Offset: 0x00011636
		void IFaceGeneratorHandler.ChangeToNoseCamera()
		{
			this._cameraLookMode = 3;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00013455 File Offset: 0x00011655
		void IFaceGeneratorHandler.ChangeToMouthCamera()
		{
			this._cameraLookMode = 4;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00013474 File Offset: 0x00011674
		void IFaceGeneratorHandler.ChangeToBodyCamera()
		{
			this._cameraLookMode = 0;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00013493 File Offset: 0x00011693
		void IFaceGeneratorHandler.ChangeToHairCamera()
		{
			this._cameraLookMode = 1;
			this._cameraCurrentElevationAdder = 0f;
			this._cameraCurrentDistanceAdder = 0f;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000134B2 File Offset: 0x000116B2
		void IFaceGeneratorHandler.UndressCharacterEntity()
		{
			this.IsDressed = false;
			((IFaceGeneratorHandler)this).RefreshCharacterEntity();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000134C1 File Offset: 0x000116C1
		void IFaceGeneratorHandler.DressCharacterEntity()
		{
			this.IsDressed = true;
			((IFaceGeneratorHandler)this).RefreshCharacterEntity();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000134D0 File Offset: 0x000116D0
		void IFaceGeneratorHandler.DefaultFace()
		{
			FaceGenerationParams faceGenerationParams = this.BodyGen.InitBodyGenerator(false);
			faceGenerationParams.UseCache = true;
			faceGenerationParams.UseGpuMorph = true;
			MBBodyProperties.TransformFaceKeysToDefaultFace(ref faceGenerationParams);
			this.DataSource.SetFaceGenerationParams(faceGenerationParams);
			this.DataSource.Refresh(true);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00013519 File Offset: 0x00011719
		private void GoToIndex(int index)
		{
			this.BodyGen.SaveCurrentCharacter();
			this.ClearAgentVisuals();
			this._goToIndexAction(index);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00013538 File Offset: 0x00011738
		public static MatrixFrame InitCamera(Camera camera, Vec3 cameraPosition)
		{
			camera.SetFovVertical(0.7853982f, Screen.AspectRatio, 0.02f, 200f);
			MatrixFrame matrixFrame = Camera.ConstructCameraFromPositionElevationBearing(cameraPosition, -0.195f, 163.17f);
			camera.Frame = matrixFrame;
			return matrixFrame;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00013578 File Offset: 0x00011778
		private void UpdateCamera(float dt)
		{
			this._characterCurrentRotation = MathF.AngleLerp(this._characterCurrentRotation, this._characterTargetRotation, MathF.Min(1f, 20f * dt), 1E-05f);
			this._targetCameraGlobalFrame.origin = this._defaultCameraGlobalFrame.origin;
			if (this._visualToShow != null)
			{
				MatrixFrame initialCharacterFrame = this._initialCharacterFrame;
				initialCharacterFrame.rotation.RotateAboutUp(this._characterCurrentRotation);
				initialCharacterFrame.rotation.ApplyScaleLocal(this._visualToShow.GetScale());
				this._visualToShow.GetEntity().SetFrame(ref initialCharacterFrame, true);
				float z = this._visualToShow.GetGlobalStableEyePoint(true).z;
				float z2 = this._visualToShow.GetGlobalStableNeckPoint(true).z;
				float scale = this._visualToShow.GetScale();
				switch (this._cameraLookMode)
				{
				case 1:
				{
					Vec2 vec = new Vec2(6.45f, 6.75f);
					vec += (vec - this._initialCharacterFrame.origin.AsVec2) * (scale - 1f);
					this._targetCameraGlobalFrame.origin = new Vec3(vec, z + (z - z2) * 0.75f, -1f);
					break;
				}
				case 2:
				{
					Vec2 vec = new Vec2(6.45f, 7f);
					vec += (vec - this._initialCharacterFrame.origin.AsVec2) * (scale - 1f);
					this._targetCameraGlobalFrame.origin = new Vec3(vec, z + (z - z2) * 0.5f, -1f);
					break;
				}
				case 3:
				{
					Vec2 vec = new Vec2(6.45f, 7f);
					vec += (vec - this._initialCharacterFrame.origin.AsVec2) * (scale - 1f);
					this._targetCameraGlobalFrame.origin = new Vec3(vec, z + (z - z2) * 0.25f, -1f);
					break;
				}
				case 4:
				{
					Vec2 vec = new Vec2(6.45f, 7f);
					vec += (vec - this._initialCharacterFrame.origin.AsVec2) * (scale - 1f);
					this._targetCameraGlobalFrame.origin = new Vec3(vec, z - (z - z2) * 0.25f, -1f);
					break;
				}
				}
			}
			Vec2 vec2 = (this._targetCameraGlobalFrame.origin.AsVec2 - this._initialCharacterFrame.origin.AsVec2).Normalized();
			Vec3 origin = this._targetCameraGlobalFrame.origin;
			origin.AsVec2 = this._targetCameraGlobalFrame.origin.AsVec2 + vec2 * this._cameraCurrentDistanceAdder;
			origin.z += this._cameraCurrentElevationAdder;
			Camera camera = this._camera;
			ref MatrixFrame ptr = ref this._camera.Frame;
			Vec3 vec3 = this._camera.Frame.origin * (1f - 10f * dt) + origin * 10f * dt;
			camera.Frame = new MatrixFrame(in ptr.rotation, in vec3);
			this.SceneLayer.SetCamera(this._camera);
		}

		// Token: 0x04000183 RID: 387
		private const int ViewOrderPriority = 1;

		// Token: 0x04000184 RID: 388
		private const bool MakeSound = true;

		// Token: 0x04000185 RID: 389
		private Scene _facegenScene;

		// Token: 0x04000186 RID: 390
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x04000187 RID: 391
		private GauntletMovieIdentifier _viewMovie;

		// Token: 0x0400018B RID: 395
		private AgentVisuals _visualToShow;

		// Token: 0x0400018C RID: 396
		private List<KeyValuePair<AgentVisuals, int>> _visualsBeingPrepared;

		// Token: 0x0400018D RID: 397
		private readonly bool _openedFromMultiplayer;

		// Token: 0x0400018E RID: 398
		private AgentVisuals _nextVisualToShow;

		// Token: 0x0400018F RID: 399
		private int _currentAgentVisualIndex;

		// Token: 0x04000190 RID: 400
		private bool _refreshCharacterEntityNextFrame;

		// Token: 0x04000191 RID: 401
		private int _makeVoiceInFrames = -1;

		// Token: 0x04000192 RID: 402
		private MatrixFrame _initialCharacterFrame;

		// Token: 0x04000193 RID: 403
		private bool _setMorphAnimNextFrame;

		// Token: 0x04000194 RID: 404
		private string _nextMorphAnimToSet = "";

		// Token: 0x04000195 RID: 405
		private bool _nextMorphAnimLoopValue;

		// Token: 0x04000196 RID: 406
		private List<BodyProperties> _templateBodyProperties;

		// Token: 0x04000198 RID: 408
		private readonly ControlCharacterCreationStage _affirmativeAction;

		// Token: 0x04000199 RID: 409
		private readonly ControlCharacterCreationStage _negativeAction;

		// Token: 0x0400019A RID: 410
		private readonly ControlCharacterCreationStageReturnInt _getTotalStageCountAction;

		// Token: 0x0400019B RID: 411
		private readonly ControlCharacterCreationStageReturnInt _getCurrentStageIndexAction;

		// Token: 0x0400019C RID: 412
		private readonly ControlCharacterCreationStageReturnInt _getFurthestIndexAction;

		// Token: 0x0400019D RID: 413
		private readonly ControlCharacterCreationStageWithInt _goToIndexAction;

		// Token: 0x0400019E RID: 414
		public bool IsDressed;

		// Token: 0x0400019F RID: 415
		public SkeletonType SkeletonType;

		// Token: 0x040001A0 RID: 416
		private readonly Equipment _dressedEquipment;

		// Token: 0x040001A1 RID: 417
		private Camera _camera;

		// Token: 0x040001A2 RID: 418
		private int _cameraLookMode;

		// Token: 0x040001A3 RID: 419
		private MatrixFrame _targetCameraGlobalFrame;

		// Token: 0x040001A4 RID: 420
		private MatrixFrame _defaultCameraGlobalFrame;

		// Token: 0x040001A5 RID: 421
		private float _characterCurrentRotation;

		// Token: 0x040001A6 RID: 422
		private float _characterTargetRotation;

		// Token: 0x040001A7 RID: 423
		private float _cameraCurrentDistanceAdder;

		// Token: 0x040001A8 RID: 424
		private float _cameraCurrentElevationAdder;

		// Token: 0x040001A9 RID: 425
		private SpriteCategory _facegenCategory;
	}
}
