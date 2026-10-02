using System;
using System.Collections.Generic;
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
	// Token: 0x0200004C RID: 76
	[CharacterCreationStageView(typeof(CharacterCreationNarrativeStage))]
	public class CharacterCreationNarrativeStageView : CharacterCreationStageViewBase
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600037F RID: 895 RVA: 0x000150B9 File Offset: 0x000132B9
		// (set) Token: 0x06000380 RID: 896 RVA: 0x000150C1 File Offset: 0x000132C1
		public SceneLayer CharacterLayer { get; private set; }

		// Token: 0x06000381 RID: 897 RVA: 0x000150CC File Offset: 0x000132CC
		public CharacterCreationNarrativeStageView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
			: base(affirmativeAction, negativeAction, onRefresh, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction)
		{
			this._characterCreationManager = characterCreationManager;
			this._affirmativeActionText = affirmativeActionText;
			this._negativeActionText = negativeActionText;
			this._currentMenuAgentVisuals = new List<AgentVisuals>();
			this._currentMenuMountEntities = new List<GameEntity>();
			this.GauntletLayer = new GauntletLayer("CharacterCreationNarrative", 1, false);
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.IsFocusLayer = true;
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			ScreenManager.TrySetFocus(this.GauntletLayer);
			this._characterCreationManager.StartNarrativeStage();
			this._dataSource = new CharacterCreationNarrativeStageVM(this._characterCreationManager, new Action(this.NextStage), this._affirmativeActionText, new Action(this.PreviousStage), this._negativeActionText, new Action(this.OnMenuChanged))
			{
				OnOptionSelection = new Action(this.OnSelectionChanged)
			};
			this._dataSource.RefreshMenu();
			this.CreateHotKeyVisuals();
			this._movie = this.GauntletLayer.LoadMovie("CharacterCreationNarrativeStage", this._dataSource);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000151FA File Offset: 0x000133FA
		public override void SetGenericScene(Scene scene)
		{
			this.OpenScene(scene);
			this.RefreshAgentVisuals();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001520C File Offset: 0x0001340C
		private void CreateHotKeyVisuals()
		{
			CharacterCreationNarrativeStageVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			}
			CharacterCreationNarrativeStageVM dataSource2 = this._dataSource;
			if (dataSource2 == null)
			{
				return;
			}
			dataSource2.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00015264 File Offset: 0x00013464
		private void OpenScene(Scene cachedScene)
		{
			this._characterScene = cachedScene;
			this._characterScene.SetShadow(true);
			this._characterScene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
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
			GameEntity gameEntity = this._characterScene.FindEntityWithName("cradle");
			if (gameEntity == null)
			{
				return;
			}
			gameEntity.SetVisibilityExcludeParents(false);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00015368 File Offset: 0x00013568
		private void RefreshAgentVisuals()
		{
			if (this._characterScene == null)
			{
				this._isAgentVisualsDirty = true;
				return;
			}
			this.ClearCharacterVisuals();
			this.ClearMountEntities();
			NarrativeMenu currentMenu = this._characterCreationManager.CurrentMenu;
			for (int i = 0; i < currentMenu.Characters.Count; i++)
			{
				NarrativeMenuCharacter narrativeMenuCharacter = currentMenu.Characters[i];
				if (narrativeMenuCharacter.IsHuman)
				{
					this.SpawnHumanNarrativeMenuCharacter(narrativeMenuCharacter);
				}
				else
				{
					this.SpawnNonHumanNarrativeMenuCharacter(narrativeMenuCharacter);
				}
			}
			this._isAgentVisualsDirty = false;
			this._isAgentVisualVisibilitiesDirty = true;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000153EC File Offset: 0x000135EC
		private void SpawnHumanNarrativeMenuCharacter(NarrativeMenuCharacter character)
		{
			GameEntity gameEntity = this._characterScene.FindEntityWithTag(character.SpawnPointEntityId);
			MatrixFrame matrixFrame = ((gameEntity != null) ? gameEntity.GetGlobalFrame() : MatrixFrame.Identity);
			matrixFrame.origin.z = 0f;
			AgentVisuals agentVisuals = AgentVisuals.Create(this.CreateAgentVisual(character, matrixFrame), "facegenvisual" + character.StringId, false, false, false);
			agentVisuals.SetVisible(false);
			this._currentMenuAgentVisuals.Add(agentVisuals);
			agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(MBRandom.RandomFloat, matrixFrame, true);
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(character.Race);
			if (character.IsHuman)
			{
				if (!string.IsNullOrEmpty(character.Item1Id) && GameEntity.Instantiate(this._characterScene, character.Item1Id, true, true, "") != null)
				{
					agentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndex(character.Item1Id, baseMonsterFromRace.MainHandItemBoneIndex);
				}
				if (!string.IsNullOrEmpty(character.Item2Id) && GameEntity.Instantiate(this._characterScene, character.Item2Id, true, true, "") != null)
				{
					agentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndex(character.Item2Id, baseMonsterFromRace.OffHandItemBoneIndex);
				}
			}
			agentVisuals.SetAgentLodZeroOrMax(true);
			agentVisuals.GetEntity().SetEnforcedMaximumLodLevel(0);
			agentVisuals.GetEntity().CheckResources(true, true);
			this.CharacterLayer.SetFocusedShadowmap(true, ref matrixFrame.origin, 0.59999996f);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00015548 File Offset: 0x00013748
		private void SpawnNonHumanNarrativeMenuCharacter(NarrativeMenuCharacter character)
		{
			this.ClearMountEntities();
			GameEntity gameEntity = this._characterScene.FindEntityWithTag(character.SpawnPointEntityId);
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(character.Item1Id);
			HorseComponent horseComponent = @object.HorseComponent;
			Monster monster = horseComponent.Monster;
			GameEntity gameEntity2 = GameEntity.CreateEmpty(this._characterScene, true, true, true);
			AnimationSystemData animationSystemData = monster.FillAnimationSystemData(MBGlobals.GetActionSet(horseComponent.Monster.ActionSetCode), 1f, false);
			gameEntity2.CreateSkeletonWithActionSet(ref animationSystemData);
			ActionIndexCache actionIndexCache = ActionIndexCache.Create(character.AnimationId);
			gameEntity2.Skeleton.SetAgentActionChannel(0, in actionIndexCache, 0f, -0.2f, true, 0f);
			ItemObject object2 = Game.Current.ObjectManager.GetObject<ItemObject>(character.Item2Id);
			MountVisualCreator.AddMountMeshToEntity(gameEntity2, @object, object2, character.MountCreationKey.ToString(), null);
			MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
			gameEntity2.SetFrame(ref globalFrame, true);
			gameEntity2.SetVisibilityExcludeParents(false);
			gameEntity2.SetEnforcedMaximumLodLevel(0);
			gameEntity2.CheckResources(true, false);
			this._currentMenuMountEntities.Add(gameEntity2);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001564C File Offset: 0x0001384C
		private void ClearCharacterVisuals()
		{
			for (int i = 0; i < this._currentMenuAgentVisuals.Count; i++)
			{
				this._currentMenuAgentVisuals[i].Reset();
			}
			this._currentMenuAgentVisuals.Clear();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001568C File Offset: 0x0001388C
		private void ClearMountEntities()
		{
			for (int i = 0; i < this._currentMenuMountEntities.Count; i++)
			{
				this._currentMenuMountEntities[i].Remove(116);
			}
			this._currentMenuMountEntities.Clear();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000156D0 File Offset: 0x000138D0
		private AgentVisualsData CreateAgentVisual(NarrativeMenuCharacter character, MatrixFrame characterFrame)
		{
			EquipmentIndex equipmentIndex = character.RightHandEquipmentIndex;
			EquipmentIndex equipmentIndex2 = character.LeftHandEquipmentIndex;
			MBEquipmentRoster equipment = character.Equipment;
			Equipment equipment2 = ((equipment != null) ? equipment.DefaultEquipment.Clone(false) : null);
			if (character.IsHuman)
			{
				if (!string.IsNullOrEmpty(character.Item1Id))
				{
					ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(character.Item1Id);
					if (@object != null)
					{
						equipmentIndex = EquipmentIndex.WeaponItemBeginSlot;
						equipment2.AddEquipmentToSlotWithoutAgent(equipmentIndex, new EquipmentElement(@object, null, null, false));
					}
				}
				if (!string.IsNullOrEmpty(character.Item2Id))
				{
					ItemObject object2 = Game.Current.ObjectManager.GetObject<ItemObject>(character.Item2Id);
					if (object2 != null)
					{
						equipmentIndex2 = EquipmentIndex.Weapon1;
						equipment2.AddEquipmentToSlotWithoutAgent(equipmentIndex2, new EquipmentElement(object2, null, null, false));
					}
				}
			}
			ActionIndexCache actionIndexCache = ActionIndexCache.Create(character.AnimationId);
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(character.Race);
			AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Equipment(equipment2).BodyProperties(character.BodyProperties)
				.Frame(characterFrame)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, character.IsFemale, "_facegen"))
				.ActionCode(in actionIndexCache)
				.Scene(this._characterScene)
				.Monster(baseMonsterFromRace)
				.UseTranslucency(true)
				.UseTesselation(true)
				.RightWieldedItemIndex((int)equipmentIndex)
				.LeftWieldedItemIndex((int)equipmentIndex2)
				.Race(CharacterObject.PlayerCharacter.Race)
				.SkeletonType(character.IsFemale ? SkeletonType.Female : SkeletonType.Male);
			CharacterCreationContent characterCreationContent = ((CharacterCreationState)GameStateManager.Current.ActiveState).CharacterCreationManager.CharacterCreationContent;
			if (characterCreationContent.SelectedCulture != null)
			{
				agentVisualsData.ClothColor1(characterCreationContent.SelectedCulture.Color);
				agentVisualsData.ClothColor2(characterCreationContent.SelectedCulture.Color2);
			}
			return agentVisualsData;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00015875 File Offset: 0x00013A75
		private void OnMenuChanged()
		{
			this.RefreshAgentVisuals();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001587D File Offset: 0x00013A7D
		private void OnSelectionChanged()
		{
			this.RefreshAgentVisuals();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00015888 File Offset: 0x00013A88
		public override void Tick(float dt)
		{
			base.Tick(dt);
			base.HandleEscapeMenu(this, this.CharacterLayer);
			if (this._characterScene != null)
			{
				if (this._isAgentVisualsDirty)
				{
					this.RefreshAgentVisuals();
				}
				this._characterScene.Tick(dt);
			}
			bool flag = this._currentMenuAgentVisuals.Count > 0 || this._currentMenuMountEntities.Count > 0;
			for (int i = 0; i < this._currentMenuAgentVisuals.Count; i++)
			{
				AgentVisuals agentVisuals = this._currentMenuAgentVisuals[i];
				agentVisuals.TickVisuals();
				if (!agentVisuals.GetEntity().CheckResources(false, true))
				{
					flag = false;
				}
			}
			for (int j = 0; j < this._currentMenuMountEntities.Count; j++)
			{
				GameEntity gameEntity = this._currentMenuMountEntities[j];
				if (gameEntity != null && !gameEntity.CheckResources(false, true))
				{
					flag = false;
				}
			}
			if (this._isAgentVisualVisibilitiesDirty && flag)
			{
				for (int k = 0; k < this._currentMenuAgentVisuals.Count; k++)
				{
					this._currentMenuAgentVisuals[k].SetVisible(true);
				}
				for (int l = 0; l < this._currentMenuMountEntities.Count; l++)
				{
					this._currentMenuMountEntities[l].SetVisibilityExcludeParents(true);
				}
				this._isAgentVisualVisibilitiesDirty = false;
			}
			this.HandleLayerInput();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000159D4 File Offset: 0x00013BD4
		private void HandleLayerInput()
		{
			if (this.GauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnPreviousStage();
				return;
			}
			if (this.GauntletLayer.Input.IsHotKeyReleased("Confirm") && this._dataSource.CanAdvance)
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnNextStage();
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00015A47 File Offset: 0x00013C47
		public override void NextStage()
		{
			this.ClearMountEntities();
			this._affirmativeAction();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00015A5A File Offset: 0x00013C5A
		public override void PreviousStage()
		{
			this.ClearMountEntities();
			this._negativeAction();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00015A70 File Offset: 0x00013C70
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.ClearCharacterVisuals();
			this.ClearMountEntities();
			this.CharacterLayer.SceneView.SetEnable(false);
			this.CharacterLayer.SceneView.ClearAll(false, false);
			this._currentMenuAgentVisuals = null;
			this.GauntletLayer = null;
			CharacterCreationNarrativeStageVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this.CharacterLayer = null;
			this._characterScene = null;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00015AE6 File Offset: 0x00013CE6
		public override int GetVirtualStageCount()
		{
			return this._characterCreationManager.CharacterCreationMenuCount;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00015AF3 File Offset: 0x00013CF3
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer> { this.CharacterLayer, this.GauntletLayer };
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00015B12 File Offset: 0x00013D12
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00015B43 File Offset: 0x00013D43
		public override void ReleaseEscapeMenuMovie()
		{
			this.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x04000170 RID: 368
		protected readonly TextObject _affirmativeActionText;

		// Token: 0x04000171 RID: 369
		protected readonly TextObject _negativeActionText;

		// Token: 0x04000172 RID: 370
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000173 RID: 371
		private GauntletLayer GauntletLayer;

		// Token: 0x04000174 RID: 372
		private CharacterCreationNarrativeStageVM _dataSource;

		// Token: 0x04000175 RID: 373
		private readonly CharacterCreationManager _characterCreationManager;

		// Token: 0x04000176 RID: 374
		private Scene _characterScene;

		// Token: 0x04000177 RID: 375
		private Camera _camera;

		// Token: 0x04000178 RID: 376
		private List<AgentVisuals> _currentMenuAgentVisuals;

		// Token: 0x04000179 RID: 377
		private List<GameEntity> _currentMenuMountEntities;

		// Token: 0x0400017A RID: 378
		private bool _isAgentVisualsDirty;

		// Token: 0x0400017B RID: 379
		private bool _isAgentVisualVisibilitiesDirty;

		// Token: 0x0400017D RID: 381
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x0400017E RID: 382
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
