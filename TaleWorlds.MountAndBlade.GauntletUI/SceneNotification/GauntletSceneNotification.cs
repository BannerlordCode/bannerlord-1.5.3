using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.SceneNotification;
using TaleWorlds.MountAndBlade.View.Scripts;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.SceneNotification
{
	// Token: 0x0200002A RID: 42
	public class GauntletSceneNotification : GlobalLayer
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000991A File Offset: 0x00007B1A
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00009921 File Offset: 0x00007B21
		public static GauntletSceneNotification Current { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00009929 File Offset: 0x00007B29
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009934 File Offset: 0x00007B34
		private GauntletSceneNotification()
		{
			this._dataSource = new SceneNotificationVM(new Action(this.OnPositiveAction), new Action(this.CloseNotification), new Func<string>(this.GetContinueKeyText));
			this._notificationQueue = new Queue<GauntletSceneNotification.SceneNotificationQueueItem>();
			this._contextProviders = new List<ISceneNotificationContextProvider>();
			this._gauntletLayer = new GauntletLayer("SceneNotification", 19000, false);
			this._gauntletLayer.LoadMovie("SceneNotification", this._dataSource);
			base.Layer = this._gauntletLayer;
			MBInformationManager.OnShowSceneNotification += this.OnShowSceneNotification;
			MBInformationManager.OnHideSceneNotification += this.OnHideSceneNotification;
			MBInformationManager.IsAnySceneNotificationActive += this.IsAnySceneNotifiationActive;
			MBInformationManager.ActiveSceneNotificationData += this.GetActiveSceneNotificationData;
			Game.OnGameCreated += this.OnGameCreatedOrDestroyed;
			this._gauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(2, null);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009A37 File Offset: 0x00007C37
		private bool IsAnySceneNotifiationActive()
		{
			return this._isActive;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009A3F File Offset: 0x00007C3F
		private SceneNotificationData GetActiveSceneNotificationData()
		{
			if (!this._isActive)
			{
				return null;
			}
			SceneNotificationVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return null;
			}
			return dataSource.ActiveData;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009A5C File Offset: 0x00007C5C
		private void OnGameCreatedOrDestroyed()
		{
			this._isRegisteredToTutorialContextEvent = false;
			this._previousTutorialContext = TutorialContexts.None;
			this._shouldReassertTutorialContext = false;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009A73 File Offset: 0x00007C73
		private void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			if (obj.NewContext == TutorialContexts.SceneNotification)
			{
				return;
			}
			this._previousTutorialContext = obj.NewContext;
			this._shouldReassertTutorialContext = this._isActive;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009A98 File Offset: 0x00007C98
		public static void Initialize()
		{
			if (GauntletSceneNotification.Current == null)
			{
				GauntletSceneNotification.Current = new GauntletSceneNotification();
				ScreenManager.AddGlobalLayer(GauntletSceneNotification.Current, false);
				ScreenManager.SetSuspendLayer(GauntletSceneNotification.Current.Layer, true);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009AC6 File Offset: 0x00007CC6
		private void OnHideSceneNotification()
		{
			this.CloseNotification();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009ACE File Offset: 0x00007CCE
		private void OnShowSceneNotification(SceneNotificationData campaignNotification)
		{
			this._notificationQueue.Enqueue(new GauntletSceneNotification.SceneNotificationQueueItem
			{
				Data = campaignNotification,
				FramesUntilDisplay = 2
			});
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00009AF0 File Offset: 0x00007CF0
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._isActive && (MBGameManager.Current == null || Game.Current == null || MBGameManager.Current == null || MBGameManager.Current.IsEnding))
			{
				this._notificationQueue.Clear();
				this.CloseNotification();
				return;
			}
			if (!this._isRegisteredToTutorialContextEvent)
			{
				Game game = Game.Current;
				if (((game != null) ? game.EventManager : null) != null)
				{
					Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
					this._isRegisteredToTutorialContextEvent = true;
				}
			}
			if (this._isActive && this._shouldReassertTutorialContext)
			{
				this._shouldReassertTutorialContext = false;
				Game game2 = Game.Current;
				if (game2 != null)
				{
					game2.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.SceneNotification));
				}
			}
			if (this._dataSource != null)
			{
				SceneNotificationVM dataSource = this._dataSource;
				PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
				dataSource.EndProgress = ((cameraPathScript != null) ? cameraPathScript.GetCameraFade() : 0f);
				PopupSceneCameraPath cameraPathScript2 = this._cameraPathScript;
				if (cameraPathScript2 != null)
				{
					cameraPathScript2.SetIsReady(this._dataSource.IsReady);
				}
				if (this._dataSource.IsReady && this._scene != null)
				{
					this._scene.WaitWaterRendererCPUSimulation();
					this._scene.Tick(dt);
				}
			}
			if (this._isPendingSceneLoad)
			{
				if (this._isActive)
				{
					this.OpenScene();
					base.Layer.IsFocusLayer = true;
					ScreenManager.TrySetFocus(base.Layer);
					base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				}
				else
				{
					Debug.FailedAssert("Scene load was pending but scene notification is not active", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OnTick", 166);
				}
				this._isPendingSceneLoad = false;
			}
			SceneNotificationVM dataSource2 = this._dataSource;
			if (dataSource2 != null && dataSource2.IsReady && this._isActive)
			{
				SceneNotificationData activeData = this._dataSource.ActiveData;
				if (activeData != null && activeData.ShouldAutoConfirm && this._autoConfirmTimer > 0f)
				{
					this._autoConfirmTimer -= dt;
					if (this._autoConfirmTimer <= 0f)
					{
						this._autoConfirmTimer = -1f;
						this._dataSource.ExecuteAffirmativeProcess();
					}
				}
			}
			this.QueueTick();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009D00 File Offset: 0x00007F00
		private void QueueTick()
		{
			if (LoadingWindow.IsLoadingWindowActive)
			{
				return;
			}
			if (!this._isActive && this._notificationQueue.Count > 0)
			{
				GauntletSceneNotification.SceneNotificationQueueItem sceneNotificationQueueItem = this._notificationQueue.Peek();
				if (sceneNotificationQueueItem.FramesUntilDisplay > 0)
				{
					sceneNotificationQueueItem.FramesUntilDisplay--;
					return;
				}
				SceneNotificationData.RelevantContextType relevantContext = sceneNotificationQueueItem.Data.RelevantContext;
				if (this.IsGivenContextApplicableToCurrentContext(relevantContext))
				{
					GauntletSceneNotification.SceneNotificationQueueItem sceneNotificationQueueItem2 = this._notificationQueue.Dequeue();
					this.CreateSceneNotification(sceneNotificationQueueItem2.Data);
				}
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009D7C File Offset: 0x00007F7C
		private void OnPositiveAction()
		{
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.SetPositiveState();
			}
			foreach (PopupSceneSpawnPoint popupSceneSpawnPoint in this._sceneCharacterScripts)
			{
				popupSceneSpawnPoint.SetPositiveState();
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009DE0 File Offset: 0x00007FE0
		private void OpenScene()
		{
			SceneNotificationData activeData = this._dataSource.ActiveData;
			SceneNotificationData.SceneNotificationCharacter[] sceneNotificationCharacters = activeData.GetSceneNotificationCharacters();
			Banner[] banners = activeData.GetBanners();
			SceneNotificationData.SceneNotificationShip[] ships = activeData.GetShips();
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.Battle, "mono_renderscene");
			this._scene.SetUsesDeleteLaterSystem(true);
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true)
			{
				InitPhysicsWorld = activeData.SceneProperties.InitializePhysics
			};
			if (sceneInitializationData.InitPhysicsWorld)
			{
				this._scene.EnableInclusiveAsyncPhysx();
			}
			this._scene.Read(activeData.SceneID, ref sceneInitializationData, "");
			if (sceneInitializationData.InitPhysicsWorld)
			{
				this._scene.EnableFixedTick();
				this._scene.SetFixedTickCallbackActive(true);
			}
			this._scene.DisableStaticShadows(activeData.SceneProperties.DisableStaticShadows);
			SceneNotificationData.NotificationSceneProperties notificationSceneProperties = activeData.SceneProperties;
			if (notificationSceneProperties.OverriddenWaterStrength != null)
			{
				Scene scene = this._scene;
				notificationSceneProperties = activeData.SceneProperties;
				scene.SetWaterStrength(notificationSceneProperties.OverriddenWaterStrength.Value);
			}
			this._scene.SetClothSimulationState(true);
			this._scene.SetShadow(true);
			this._scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._scene);
			this._agentRendererSceneController.SetEnforcedVisibilityForAllAgents(this._scene);
			this._sceneCharacterScripts = new List<PopupSceneSpawnPoint>();
			this._customPrefabBannerEntities = new Dictionary<string, GameEntity>();
			GameEntity firstEntityWithScriptComponent = this._scene.GetFirstEntityWithScriptComponent<PopupSceneCameraPath>();
			this._cameraPathScript = ((firstEntityWithScriptComponent != null) ? firstEntityWithScriptComponent.GetFirstScriptOfType<PopupSceneCameraPath>() : null);
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.Initialize();
			}
			PopupSceneCameraPath cameraPathScript2 = this._cameraPathScript;
			if (cameraPathScript2 != null)
			{
				cameraPathScript2.SetInitialState();
			}
			if (sceneNotificationCharacters != null)
			{
				int num = 1;
				foreach (SceneNotificationData.SceneNotificationCharacter sceneNotificationCharacter in sceneNotificationCharacters)
				{
					BasicCharacterObject character = sceneNotificationCharacter.Character;
					if (character == null)
					{
						num++;
					}
					else
					{
						string text = "spawnpoint_player_" + num.ToString();
						GameEntity gameEntity = this._scene.FindEntitiesWithTag(text).ToList<GameEntity>().FirstOrDefault<GameEntity>();
						if (gameEntity == null)
						{
							num++;
						}
						else
						{
							PopupSceneSpawnPoint firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSpawnPoint>();
							MatrixFrame frame = gameEntity.GetFrame();
							Equipment equipment = character.FirstBattleEquipment;
							if (sceneNotificationCharacter.OverriddenEquipment != null)
							{
								equipment = sceneNotificationCharacter.OverriddenEquipment;
							}
							else if (sceneNotificationCharacter.UseCivilianEquipment)
							{
								equipment = character.FirstCivilianEquipment;
							}
							BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
							if (sceneNotificationCharacter.OverriddenBodyProperties != default(BodyProperties))
							{
								bodyProperties = sceneNotificationCharacter.OverriddenBodyProperties;
							}
							uint num2 = character.Culture.Color;
							uint num3 = character.Culture.Color2;
							if (sceneNotificationCharacter.CustomColor1 != 4294967295U)
							{
								num2 = sceneNotificationCharacter.CustomColor1;
							}
							if (sceneNotificationCharacter.CustomColor2 != 4294967295U)
							{
								num3 = sceneNotificationCharacter.CustomColor2;
							}
							Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(character.Race);
							AgentVisuals agentVisuals = AgentVisuals.Create(new AgentVisualsData().UseMorphAnims(true).Equipment(equipment).Race(character.Race)
								.BodyProperties(bodyProperties)
								.SkeletonType(character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
								.Frame(frame)
								.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, character.IsFemale, "_facegen"))
								.Scene(this._scene)
								.Monster(baseMonsterFromRace)
								.PrepareImmediately(true)
								.UseTranslucency(true)
								.UseTesselation(true)
								.ClothColor1(num2)
								.ClothColor2(num3), "notification_agent_visuals_" + num, false, false, false);
							AgentVisuals agentVisuals2 = null;
							if (sceneNotificationCharacter.UseHorse)
							{
								ItemObject item = equipment[EquipmentIndex.ArmorItemEndSlot].Item;
								string randomMountKeyString = MountCreationKey.GetRandomMountKeyString(item, character.GetMountKeySeed());
								MBActionSet actionSet = MBGlobals.GetActionSet(item.HorseComponent.Monster.ActionSetCode);
								agentVisuals2 = AgentVisuals.Create(new AgentVisualsData().Equipment(equipment).Frame(frame).ActionSet(actionSet)
									.Scene(this._scene)
									.Monster(item.HorseComponent.Monster)
									.Scale(item.ScaleFactor)
									.PrepareImmediately(true)
									.UseTranslucency(true)
									.UseTesselation(true)
									.MountCreationKey(randomMountKeyString), "notification_mount_visuals_" + num, false, false, false);
							}
							firstScriptOfType.InitializeWithAgentVisuals(agentVisuals, agentVisuals2);
							agentVisuals.SetAgentLodZeroOrMaxExternal(true);
							if (agentVisuals2 != null)
							{
								agentVisuals2.SetAgentLodZeroOrMaxExternal(true);
							}
							firstScriptOfType.SetInitialState();
							this._sceneCharacterScripts.Add(firstScriptOfType);
							if (!string.IsNullOrEmpty(firstScriptOfType.BannerTagToUseForAddedPrefab) && firstScriptOfType.AddedPrefabComponent != null)
							{
								this._customPrefabBannerEntities.Add(firstScriptOfType.BannerTagToUseForAddedPrefab, GameEntity.CreateFromWeakEntity(firstScriptOfType.AddedPrefabComponent.GetEntity()));
							}
							num++;
						}
					}
				}
			}
			if (banners != null)
			{
				for (int j = 0; j < banners.Length; j++)
				{
					Banner banner = banners[j];
					string text2 = "banner_" + (j + 1).ToString();
					GameEntity bannerEntity = this._scene.FindEntityWithTag(text2);
					if (bannerEntity != null)
					{
						BannerVisual bannerVisual = (BannerVisual)banner.BannerVisual;
						BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
						bannerVisual.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
						{
							this.OnBannerTableauRenderDone(bannerEntity, t);
						}, true);
					}
					else
					{
						GameEntity entity;
						if (this._customPrefabBannerEntities.TryGetValue(text2, out entity))
						{
							BannerVisual bannerVisual2 = (BannerVisual)banner.BannerVisual;
							BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
							bannerVisual2.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
							{
								this.OnBannerTableauRenderDone(entity, t);
							}, true);
						}
					}
				}
			}
			if (ships != null)
			{
				int num4 = 1;
				foreach (SceneNotificationData.SceneNotificationShip sceneNotificationShip in ships)
				{
					if (string.IsNullOrEmpty(sceneNotificationShip.ShipPrefabId))
					{
						num4++;
						Debug.FailedAssert("Scene notification ship does not have a valid prefab", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OpenScene", 427);
					}
					else
					{
						string text3 = "spawnpoint_ship_" + num4.ToString();
						GameEntity gameEntity2 = this._scene.FindEntityWithTag(text3);
						if (gameEntity2 == null)
						{
							Debug.FailedAssert("Ship spawn point entity with tag: " + text3 + " was not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OpenScene", 435);
							num4++;
						}
						else
						{
							List<GameEntity> list = gameEntity2.GetChildren().ToList<GameEntity>();
							for (int l = 0; l < list.Count; l++)
							{
								GameEntity gameEntity3 = list[l];
								this._scene.RemoveEntity(gameEntity3, 62);
							}
							gameEntity2.GetFirstScriptOfType<PopupSceneShipSpawnPoint>();
							GameEntity gameEntity4 = VisualShipFactory.CreateVisualShip(sceneNotificationShip.ShipPrefabId, this._scene, sceneNotificationShip.ShipUpgrades, sceneNotificationShip.ShipSeed, sceneNotificationShip.ShipHitPointRatio, sceneNotificationShip.SailColor1, sceneNotificationShip.SailColor2, true, 1f, true);
							gameEntity2.AddChild(gameEntity4, false);
							num4++;
						}
					}
				}
			}
			List<GameEntity> list2 = new List<GameEntity>();
			this._scene.GetAllEntitiesWithScriptComponent<Bird>(ref list2);
			foreach (GameEntity gameEntity5 in list2)
			{
				int animationIndexAtChannel = gameEntity5.Skeleton.GetAnimationIndexAtChannel(0);
				gameEntity5.Skeleton.SetAnimationAtChannel(animationIndexAtChannel, 0, 1f, 0f, 0f);
			}
			this._dataSource.Scene = this._scene;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000A57C File Offset: 0x0000877C
		private void OnBannerTableauRenderDone(GameEntity bannerEntity, Texture bannerTexture)
		{
			if (bannerEntity != null)
			{
				foreach (Mesh mesh in bannerEntity.GetAllMeshesWithTag("banner_replacement_mesh"))
				{
					this.ApplyBannerTextureToMesh(mesh, bannerTexture);
				}
				Skeleton skeleton = bannerEntity.Skeleton;
				if (((skeleton != null) ? skeleton.GetAllMeshes() : null) != null)
				{
					Skeleton skeleton2 = bannerEntity.Skeleton;
					foreach (Mesh mesh2 in ((skeleton2 != null) ? skeleton2.GetAllMeshes() : null))
					{
						if (mesh2.HasTag("banner_replacement_mesh"))
						{
							this.ApplyBannerTextureToMesh(mesh2, bannerTexture);
						}
					}
				}
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000A648 File Offset: 0x00008848
		private void ApplyBannerTextureToMesh(Mesh bannerMesh, Texture bannerTexture)
		{
			if (bannerMesh != null)
			{
				Material material = bannerMesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, bannerTexture);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				bannerMesh.SetMaterial(material);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000A6A0 File Offset: 0x000088A0
		private void CreateSceneNotification(SceneNotificationData data)
		{
			if (this._isActive)
			{
				Debug.FailedAssert("Trying to create scene notification while another notification is playing", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "CreateSceneNotification", 513);
				return;
			}
			this._isActive = true;
			this._autoConfirmTimer = 1f;
			this._dataSource.CreateNotification(data);
			ScreenManager.SetSuspendLayer(base.Layer, false);
			this._shouldReassertTutorialContext = false;
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.SceneNotification));
			}
			this._isLastActiveGameStatePaused = data.PauseActiveState;
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager.Current.RegisterActiveStateDisableRequest(this);
				MBCommon.PauseGameEngine();
			}
			this._dataSource.EndProgress = 0f;
			this._isPendingSceneLoad = true;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000A758 File Offset: 0x00008958
		private void CloseNotification()
		{
			if (!this._isActive)
			{
				return;
			}
			this._dataSource.ClearData();
			this._isActive = false;
			this._autoConfirmTimer = 1f;
			base.Layer.InputRestrictions.ResetInputRestrictions();
			ScreenManager.SetSuspendLayer(base.Layer, true);
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			this._shouldReassertTutorialContext = false;
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(this._previousTutorialContext));
			}
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				if (gameStateManager != null)
				{
					gameStateManager.UnregisterActiveStateDisableRequest(this);
				}
				MBCommon.UnPauseGameEngine();
			}
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.Destroy();
			}
			if (this._sceneCharacterScripts != null)
			{
				foreach (PopupSceneSpawnPoint popupSceneSpawnPoint in this._sceneCharacterScripts)
				{
					popupSceneSpawnPoint.Destroy();
				}
				this._sceneCharacterScripts = null;
			}
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._scene.ClearAll();
			this._scene.ManualInvalidate();
			this._scene = null;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000A898 File Offset: 0x00008A98
		private string GetContinueKeyText()
		{
			Module currentModule = Module.CurrentModule;
			GameTextManager gameTextManager = ((currentModule != null) ? currentModule.GlobalTextManager : null);
			if (gameTextManager == null)
			{
				return string.Empty;
			}
			if (Input.IsGamepadActive)
			{
				return gameTextManager.FindText("str_click_to_continue_console", null).SetTextVariable("CONSOLE_KEY_NAME", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("ConversationHotKeyCategory", "ContinueClick"), 1f)).ToString();
			}
			return gameTextManager.FindText("str_click_to_continue", null).ToString();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000A90D File Offset: 0x00008B0D
		public void OnFinalize()
		{
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000A921 File Offset: 0x00008B21
		public void RegisterContextProvider(ISceneNotificationContextProvider provider)
		{
			this._contextProviders.Add(provider);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000A92F File Offset: 0x00008B2F
		public bool RemoveContextProvider(ISceneNotificationContextProvider provider)
		{
			return this._contextProviders.Remove(provider);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A940 File Offset: 0x00008B40
		private bool IsGivenContextApplicableToCurrentContext(SceneNotificationData.RelevantContextType givenContextType)
		{
			if (LoadingWindow.IsLoadingWindowActive)
			{
				return false;
			}
			if (givenContextType == SceneNotificationData.RelevantContextType.Any)
			{
				return true;
			}
			for (int i = 0; i < this._contextProviders.Count; i++)
			{
				ISceneNotificationContextProvider sceneNotificationContextProvider = this._contextProviders[i];
				if (sceneNotificationContextProvider != null && !sceneNotificationContextProvider.IsContextAllowed(givenContextType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000DD RID: 221
		private readonly GauntletLayer _gauntletLayer;

		// Token: 0x040000DE RID: 222
		private readonly Queue<GauntletSceneNotification.SceneNotificationQueueItem> _notificationQueue;

		// Token: 0x040000DF RID: 223
		private readonly List<ISceneNotificationContextProvider> _contextProviders;

		// Token: 0x040000E0 RID: 224
		private SceneNotificationVM _dataSource;

		// Token: 0x040000E1 RID: 225
		private bool _isActive;

		// Token: 0x040000E2 RID: 226
		private bool _isLastActiveGameStatePaused;

		// Token: 0x040000E3 RID: 227
		private bool _isPendingSceneLoad;

		// Token: 0x040000E4 RID: 228
		private float _autoConfirmTimer = -1f;

		// Token: 0x040000E5 RID: 229
		private TutorialContexts _previousTutorialContext;

		// Token: 0x040000E6 RID: 230
		private bool _shouldReassertTutorialContext;

		// Token: 0x040000E7 RID: 231
		private bool _isRegisteredToTutorialContextEvent;

		// Token: 0x040000E8 RID: 232
		private Scene _scene;

		// Token: 0x040000E9 RID: 233
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x040000EA RID: 234
		private List<PopupSceneSpawnPoint> _sceneCharacterScripts;

		// Token: 0x040000EB RID: 235
		private PopupSceneCameraPath _cameraPathScript;

		// Token: 0x040000EC RID: 236
		private Dictionary<string, GameEntity> _customPrefabBannerEntities;

		// Token: 0x02000053 RID: 83
		private class SceneNotificationQueueItem
		{
			// Token: 0x040001CA RID: 458
			public SceneNotificationData Data;

			// Token: 0x040001CB RID: 459
			public int FramesUntilDisplay;
		}
	}
}
