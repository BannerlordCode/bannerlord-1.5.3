using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
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
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.BannerEditor
{
	// Token: 0x0200004F RID: 79
	public class BannerEditorView
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001701C File Offset: 0x0001521C
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00017024 File Offset: 0x00015224
		public GauntletLayer GauntletLayer { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001702D File Offset: 0x0001522D
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00017035 File Offset: 0x00015235
		public BannerEditorVM DataSource { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0001703E File Offset: 0x0001523E
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00017046 File Offset: 0x00015246
		public Banner Banner { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001704F File Offset: 0x0001524F
		private ItemRosterElement ShieldRosterElement
		{
			get
			{
				return this.DataSource.ShieldRosterElement;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001705C File Offset: 0x0001525C
		private int ShieldSlotIndex
		{
			get
			{
				return this.DataSource.ShieldSlotIndex;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00017072 File Offset: 0x00015272
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x00017069 File Offset: 0x00015269
		public SceneLayer SceneLayer { get; private set; }

		// Token: 0x060003CA RID: 970 RVA: 0x0001707C File Offset: 0x0001527C
		public BannerEditorView(BasicCharacterObject character, Banner banner, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh = null, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction = null, ControlCharacterCreationStageReturnInt getTotalStageCountAction = null, ControlCharacterCreationStageReturnInt getFurthestIndexAction = null, ControlCharacterCreationStageWithInt goToIndexAction = null)
		{
			BannerEditorTextureCache bannerEditorTextureCache = BannerEditorTextureCache.Current;
			if (bannerEditorTextureCache != null)
			{
				bannerEditorTextureCache.FlushCache();
			}
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_bannericons");
			this._character = character;
			this.Banner = banner;
			this._goToIndexAction = goToIndexAction;
			if (getCurrentStageIndexAction == null || getTotalStageCountAction == null || getFurthestIndexAction == null)
			{
				this.DataSource = new BannerEditorVM(this._character, this.Banner, new Action<bool>(this.Exit), new Action(this.RefreshShieldAndCharacter), 0, 0, 0, new Action<int>(this.GoToIndex));
				this.DataSource.Description = new TextObject("{=3ZO5cMLu}Customize your banner's sigil", null).ToString();
				this._isOpenedFromCharacterCreation = true;
			}
			else
			{
				this.DataSource = new BannerEditorVM(this._character, this.Banner, new Action<bool>(this.Exit), new Action(this.RefreshShieldAndCharacter), getCurrentStageIndexAction(), getTotalStageCountAction(), getFurthestIndexAction(), new Action<int>(this.GoToIndex));
				this.DataSource.Description = new TextObject("{=312lNJTM}Customize your personal banner by choosing your clan's sigil", null).ToString();
				this._isOpenedFromCharacterCreation = false;
			}
			this.DataSource.DoneText = affirmativeActionText.ToString();
			this.DataSource.CancelText = negativeActionText.ToString();
			this.GauntletLayer = new GauntletLayer("BanerEditor", 1, false);
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("FaceGenHotkeyCategory"));
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this.GauntletLayer);
			this.DataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this.DataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(56));
			this.DataSource.AddCameraControlInputKey(HotKeyManager.GetCategory("FaceGenHotkeyCategory").GetGameKey(57));
			GameAxisKey gameAxisKey = HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisX");
			GameAxisKey gameAxisKey2 = HotKeyManager.GetCategory("FaceGenHotkeyCategory").RegisteredGameAxisKeys.FirstOrDefault<GameAxisKey>((GameAxisKey x) => x.Id == "CameraAxisY");
			this.DataSource.AddCameraControlInputKey(gameAxisKey, Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey.Id));
			this.DataSource.AddCameraControlInputKey(gameAxisKey2, Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey2.Id));
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._agentVisuals = new AgentVisuals[2];
			this._currentBanner = this.Banner;
			this.CreateScene();
			Input.ClearKeys();
			this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)this.ShieldSlotIndex, this.ShieldRosterElement.EquipmentElement);
			this._shieldWeapon = new MissionWeapon(this.ShieldRosterElement.EquipmentElement.Item, this.ShieldRosterElement.EquipmentElement.ItemModifier, this.Banner);
			AgentVisualsData copyAgentVisualsData = this._agentVisuals[0].GetCopyAgentVisualsData();
			copyAgentVisualsData.Equipment(this._weaponEquipment).RightWieldedItemIndex(-1).LeftWieldedItemIndex(this.ShieldSlotIndex)
				.Banner(this.Banner)
				.ClothColor1(this.Banner.GetPrimaryColor())
				.ClothColor2(this.Banner.GetFirstIconColor());
			this._agentVisuals[0].Refresh(false, copyAgentVisualsData, true);
			this._agentVisuals[0].SetVisible(false);
			this._agentVisuals[0].GetEntity().CheckResources(true, true);
			AgentVisualsData copyAgentVisualsData2 = this._agentVisuals[1].GetCopyAgentVisualsData();
			copyAgentVisualsData2.Equipment(this._weaponEquipment).RightWieldedItemIndex(-1).LeftWieldedItemIndex(this.ShieldSlotIndex)
				.Banner(this.Banner)
				.ClothColor1(this.Banner.GetPrimaryColor())
				.ClothColor2(this.Banner.GetFirstIconColor());
			this._agentVisuals[1].Refresh(false, copyAgentVisualsData2, true);
			this._agentVisuals[1].SetVisible(false);
			this._agentVisuals[1].GetEntity().CheckResources(true, true);
			this._checkWhetherAgentVisualIsReady = true;
			this._firstCharacterRender = true;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00017554 File Offset: 0x00015754
		public void OnTick(float dt)
		{
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
				if (this._gauntletmovie == null)
				{
					this._gauntletmovie = this.GauntletLayer.LoadMovie("BannerEditor", this.DataSource);
				}
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

		// Token: 0x060003CC RID: 972 RVA: 0x00017689 File Offset: 0x00015889
		public void OnFinalize()
		{
			BannerEditorTextureCache bannerEditorTextureCache = BannerEditorTextureCache.Current;
			if (bannerEditorTextureCache != null)
			{
				bannerEditorTextureCache.FlushCache();
			}
			if (!this._isOpenedFromCharacterCreation)
			{
				this._spriteCategory.Unload();
			}
			BannerEditorVM dataSource = this.DataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._isFinalized = true;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000176C6 File Offset: 0x000158C6
		public void Exit(bool isCancel)
		{
			MouseManager.ActivateMouseCursor(CursorType.Default);
			this._gauntletmovie = null;
			if (isCancel)
			{
				this._negativeAction();
				return;
			}
			this.SetMapIconAsDirtyForAllPlayerClanParties();
			this._affirmativeAction();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000176F8 File Offset: 0x000158F8
		private void SetMapIconAsDirtyForAllPlayerClanParties()
		{
			foreach (Hero hero in Clan.PlayerClan.AliveLords)
			{
				foreach (CaravanPartyComponent caravanPartyComponent in hero.OwnedCaravans)
				{
					PartyBase party = caravanPartyComponent.MobileParty.Party;
					if (party != null)
					{
						party.SetVisualAsDirty();
					}
					caravanPartyComponent.MobileParty.SetNavalVisualAsDirty();
				}
			}
			foreach (Hero hero2 in Clan.PlayerClan.Companions)
			{
				foreach (CaravanPartyComponent caravanPartyComponent2 in hero2.OwnedCaravans)
				{
					PartyBase party2 = caravanPartyComponent2.MobileParty.Party;
					if (party2 != null)
					{
						party2.SetVisualAsDirty();
					}
					caravanPartyComponent2.MobileParty.SetNavalVisualAsDirty();
				}
			}
			foreach (WarPartyComponent warPartyComponent in Clan.PlayerClan.WarPartyComponents)
			{
				PartyBase party3 = warPartyComponent.MobileParty.Party;
				if (party3 != null)
				{
					party3.SetVisualAsDirty();
				}
				warPartyComponent.MobileParty.SetNavalVisualAsDirty();
			}
			foreach (Settlement settlement in Clan.PlayerClan.Settlements)
			{
				if (settlement.IsVillage && settlement.Village.VillagerPartyComponent != null)
				{
					PartyBase party4 = settlement.Village.VillagerPartyComponent.MobileParty.Party;
					if (party4 != null)
					{
						party4.SetVisualAsDirty();
					}
				}
				else if ((settlement.IsCastle || settlement.IsTown) && settlement.Town.GarrisonParty != null)
				{
					PartyBase party5 = settlement.Town.GarrisonParty.Party;
					if (party5 != null)
					{
						party5.SetVisualAsDirty();
					}
				}
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00017950 File Offset: 0x00015B50
		private void CreateScene()
		{
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.Battle, "mono_renderscene");
			this._scene.SetName("MBBannerEditorScreen");
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
			this.SceneLayer.IsFocusLayer = true;
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

		// Token: 0x060003D0 RID: 976 RVA: 0x00017B0C File Offset: 0x00015D0C
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
					goto IL_0076;
				}
				ItemObject item2 = equipmentFromSlot.Item;
				if (((item2 != null) ? item2.PrimaryWeapon : null) != null && !equipmentFromSlot.Item.PrimaryWeapon.IsShield && !equipmentFromSlot.Item.ItemFlags.HasAllFlags(ItemFlags.DropOnWeaponChange))
				{
					goto IL_0076;
				}
				IL_0083:
				i++;
				continue;
				IL_0076:
				this._weaponEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				goto IL_0083;
			}
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(this._character.Race);
			this._agentVisuals[0] = AgentVisuals.Create(new AgentVisualsData().Equipment(this._weaponEquipment).BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1)).Frame(this._characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._character.IsFemale, "_facegen"))
				.ActionCode(in action)
				.Scene(this._scene)
				.Monster(baseMonsterFromRace)
				.SkeletonType(this._character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Race(this._character.Race)
				.PrepareImmediately(true)
				.UseMorphAnims(true), "BannerEditorChar", false, false, true);
			this._agentVisuals[0].SetAgentLodZeroOrMaxExternal(true);
			this._agentVisuals[0].GetEntity().CheckResources(true, true);
			this._agentVisuals[1] = AgentVisuals.Create(new AgentVisualsData().Equipment(this._weaponEquipment).BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1)).Frame(this._characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._character.IsFemale, "_facegen"))
				.ActionCode(in action)
				.Scene(this._scene)
				.Race(this._character.Race)
				.Monster(baseMonsterFromRace)
				.SkeletonType(this._character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.PrepareImmediately(true)
				.UseMorphAnims(true), "BannerEditorChar", false, false, true);
			this._agentVisuals[1].SetAgentLodZeroOrMaxExternal(true);
			this._agentVisuals[1].GetEntity().CheckResources(true, true);
			this.UpdateBanners();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00017D58 File Offset: 0x00015F58
		private void UpdateBanners()
		{
			if (this._latestBannerTextureCreationData != null)
			{
				ThumbnailCacheManager.Current.DestroyTexture(this._latestBannerTextureCreationData);
				this._latestBannerTextureCreationData = null;
			}
			Banner banner = this.Banner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			banner.GetTableauTextureLargeForBannerEditor(in bannerDebugInfo, delegate(TaleWorlds.Engine.Texture tex)
			{
				this.OnNewBannerReadyForBanners(this.Banner, tex);
			}, out this._latestBannerTextureCreationData);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00017DB8 File Offset: 0x00015FB8
		private void OnNewBannerReadyForBanners(Banner bannerOfTexture, TaleWorlds.Engine.Texture newTexture)
		{
			if (!this._isFinalized && this._scene != null && this._currentBanner.BannerCode == bannerOfTexture.BannerCode)
			{
				GameEntity gameEntity = this._scene.FindEntityWithTag("banner");
				if (gameEntity != null)
				{
					Mesh firstMesh = gameEntity.GetFirstMesh();
					if (firstMesh != null && this.Banner != null)
					{
						firstMesh.GetMaterial().SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, newTexture);
					}
				}
				else
				{
					gameEntity = this._scene.FindEntityWithTag("banner_2");
					Mesh firstMesh2 = gameEntity.GetFirstMesh();
					if (firstMesh2 != null && this.Banner != null)
					{
						firstMesh2.GetMaterial().SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, newTexture);
					}
				}
				this._refreshCharacterAndShieldNextFrame = true;
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00017E77 File Offset: 0x00016077
		private void RefreshShieldAndCharacter()
		{
			this._currentBanner = this.Banner;
			this._refreshBannersNextFrame = true;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00017E8C File Offset: 0x0001608C
		private void RefreshShieldAndCharacterAux()
		{
			if (this._latestShieldTextureCreationData != null)
			{
				ThumbnailCacheManager.Current.DestroyTexture(this._latestShieldTextureCreationData);
				this._latestShieldTextureCreationData = null;
			}
			Banner banner = this.Banner;
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
			banner.GetTableauTextureLargeForBannerEditor(in bannerDebugInfo, delegate(TaleWorlds.Engine.Texture tex)
			{
				this.OnNewBannerReadyForShield(tex);
			}, out this._latestShieldTextureCreationData);
			int agentVisualToShowIndex = this._agentVisualToShowIndex;
			this._agentVisualToShowIndex = (this._agentVisualToShowIndex + 1) % 2;
			AgentVisualsData copyAgentVisualsData = this._agentVisuals[this._agentVisualToShowIndex].GetCopyAgentVisualsData();
			copyAgentVisualsData.Equipment(this._weaponEquipment).RightWieldedItemIndex(-1).LeftWieldedItemIndex(this.ShieldSlotIndex)
				.Banner(this.Banner)
				.Frame(this._characterFrame)
				.BodyProperties(this._character.GetBodyProperties(this._weaponEquipment, -1))
				.ClothColor1(this.Banner.GetPrimaryColor())
				.ClothColor2(this.Banner.GetFirstIconColor());
			this._agentVisuals[this._agentVisualToShowIndex].Refresh(false, copyAgentVisualsData, true);
			this._agentVisuals[this._agentVisualToShowIndex].GetEntity().CheckResources(true, true);
			this._agentVisuals[this._agentVisualToShowIndex].GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.001f, this._characterFrame, true);
			this._agentVisuals[this._agentVisualToShowIndex].SetVisible(false);
			this._agentVisuals[this._agentVisualToShowIndex].SetVisible(true);
			this._checkWhetherAgentVisualIsReady = true;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00018003 File Offset: 0x00016203
		private void OnNewBannerReadyForShield(TaleWorlds.Engine.Texture newTexture)
		{
			this._shieldWeapon.GetWeaponData(false).TableauMaterial.SetTexture(TaleWorlds.Engine.Material.MBTextureType.DiffuseMap2, newTexture);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001801D File Offset: 0x0001621D
		private bool IsHotKeyReleasedOnAnyLayer(string hotkeyName)
		{
			return this.GauntletLayer.Input.IsHotKeyReleased(hotkeyName) || this.SceneLayer.Input.IsHotKeyReleased(hotkeyName);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00018048 File Offset: 0x00016248
		private void HandleUserInput(float dt)
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
			if (this.IsHotKeyReleasedOnAnyLayer("Confirm"))
			{
				this.DataSource.ExecuteDone();
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				return;
			}
			if (this.IsHotKeyReleasedOnAnyLayer("Exit"))
			{
				this.DataSource.ExecuteCancel();
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				return;
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
			this._cameraTargetElevationAdder = MBMath.ClampFloat(this._cameraTargetElevationAdder + num7, 0.5f, 1.9f * this._agentVisuals[this._agentVisualToShowIndex].GetScale());
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000183B7 File Offset: 0x000165B7
		private void NormalizeControllerInputForDeadZone(ref float inputValue, float controllerDeadZone)
		{
			if (MathF.Abs(inputValue) < controllerDeadZone)
			{
				inputValue = 0f;
				return;
			}
			inputValue = (inputValue - (float)MathF.Sign(inputValue) * controllerDeadZone) / (1f - controllerDeadZone);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000183E4 File Offset: 0x000165E4
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

		// Token: 0x060003DA RID: 986 RVA: 0x00018514 File Offset: 0x00016714
		public void OnDeactivate()
		{
			this._agentVisuals[0].Reset();
			this._agentVisuals[1].Reset();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			this._scene.ClearAll();
			this._scene.ManualInvalidate();
			this._scene = null;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018571 File Offset: 0x00016771
		public void GoToIndex(int index)
		{
			this._goToIndexAction(index);
		}

		// Token: 0x040001A0 RID: 416
		private GauntletMovieIdentifier _gauntletmovie;

		// Token: 0x040001A1 RID: 417
		private readonly SpriteCategory _spriteCategory;

		// Token: 0x040001A2 RID: 418
		private bool _isFinalized;

		// Token: 0x040001A3 RID: 419
		private float _cameraCurrentRotation;

		// Token: 0x040001A4 RID: 420
		private float _cameraTargetRotation;

		// Token: 0x040001A5 RID: 421
		private float _cameraCurrentDistanceAdder;

		// Token: 0x040001A6 RID: 422
		private float _cameraTargetDistanceAdder;

		// Token: 0x040001A7 RID: 423
		private float _cameraCurrentElevationAdder;

		// Token: 0x040001A8 RID: 424
		private float _cameraTargetElevationAdder;

		// Token: 0x040001A9 RID: 425
		private readonly BasicCharacterObject _character;

		// Token: 0x040001AA RID: 426
		private Scene _scene;

		// Token: 0x040001AB RID: 427
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x040001AC RID: 428
		private AgentVisuals[] _agentVisuals;

		// Token: 0x040001AD RID: 429
		private int _agentVisualToShowIndex;

		// Token: 0x040001AE RID: 430
		private bool _checkWhetherAgentVisualIsReady;

		// Token: 0x040001AF RID: 431
		private bool _firstCharacterRender = true;

		// Token: 0x040001B0 RID: 432
		private bool _refreshBannersNextFrame;

		// Token: 0x040001B1 RID: 433
		private bool _refreshCharacterAndShieldNextFrame;

		// Token: 0x040001B2 RID: 434
		private MatrixFrame _characterFrame;

		// Token: 0x040001B3 RID: 435
		private MissionWeapon _shieldWeapon;

		// Token: 0x040001B4 RID: 436
		private Equipment _weaponEquipment;

		// Token: 0x040001B5 RID: 437
		private Banner _currentBanner;

		// Token: 0x040001B6 RID: 438
		private Camera _camera;

		// Token: 0x040001B8 RID: 440
		private BannerEditorTextureCreationData _latestBannerTextureCreationData;

		// Token: 0x040001B9 RID: 441
		private BannerEditorTextureCreationData _latestShieldTextureCreationData;

		// Token: 0x040001BA RID: 442
		private bool _isOpenedFromCharacterCreation;

		// Token: 0x040001BB RID: 443
		private ControlCharacterCreationStage _affirmativeAction;

		// Token: 0x040001BC RID: 444
		private ControlCharacterCreationStage _negativeAction;

		// Token: 0x040001BD RID: 445
		private ControlCharacterCreationStageWithInt _goToIndexAction;
	}
}
