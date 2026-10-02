using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.CharacterCreation
{
	// Token: 0x0200007F RID: 127
	[GameStateScreen(typeof(CharacterCreationState))]
	public class CharacterCreationScreen : ScreenBase, ICharacterCreationStateHandler, IGameStateListener
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x000292D0 File Offset: 0x000274D0
		public CharacterCreationScreen(CharacterCreationState characterCreationState)
		{
			this._characterCreationStateState = characterCreationState;
			characterCreationState.Handler = this;
			this._stageViews = new Dictionary<Type, Type>();
			this.CollectUnorderedStages();
			this._cultureAmbientSoundEvent = SoundEvent.CreateEventFromString("event:/mission/ambient/special/charactercreation", null);
			this._cultureAmbientSoundEvent.Play();
			this.CreateGenericScene();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00029328 File Offset: 0x00027528
		private void CreateGenericScene()
		{
			this._genericScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			SceneInitializationData sceneInitializationData = default(SceneInitializationData);
			sceneInitializationData.InitPhysicsWorld = false;
			this._genericScene.Read("character_menu_new", ref sceneInitializationData, "");
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._genericScene);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00029380 File Offset: 0x00027580
		private void StopSound()
		{
			SoundManager.SetGlobalParameter("MissionCulture", 0f);
			SoundEvent cultureAmbientSoundEvent = this._cultureAmbientSoundEvent;
			if (cultureAmbientSoundEvent != null)
			{
				cultureAmbientSoundEvent.Stop();
			}
			this._cultureAmbientSoundEvent = null;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000293A9 File Offset: 0x000275A9
		void ICharacterCreationStateHandler.OnCharacterCreationFinalized()
		{
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000293B0 File Offset: 0x000275B0
		void ICharacterCreationStateHandler.OnRefresh()
		{
			if (this._shownLayers != null)
			{
				foreach (ScreenLayer screenLayer in this._shownLayers.ToArray<ScreenLayer>())
				{
					base.RemoveLayer(screenLayer);
				}
			}
			if (this._currentStageView != null)
			{
				this._shownLayers = this._currentStageView.GetLayers();
				if (this._shownLayers != null)
				{
					foreach (ScreenLayer screenLayer2 in this._shownLayers.ToArray<ScreenLayer>())
					{
						base.AddLayer(screenLayer2);
					}
				}
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00029430 File Offset: 0x00027630
		void ICharacterCreationStateHandler.OnStageCreated(CharacterCreationStageBase stage)
		{
			Type type;
			if (this._stageViews.TryGetValue(stage.GetType(), out type))
			{
				this._currentStageView = Activator.CreateInstance(type, new object[]
				{
					this._characterCreationStateState.CharacterCreationManager,
					new ControlCharacterCreationStage(this._characterCreationStateState.CharacterCreationManager.NextStage),
					new TextObject("{=Rvr1bcu8}Next", null),
					new ControlCharacterCreationStage(this._characterCreationStateState.CharacterCreationManager.PreviousStage),
					new TextObject("{=WXAaWZVf}Previous", null),
					new ControlCharacterCreationStage(this._characterCreationStateState.Refresh),
					new ControlCharacterCreationStageReturnInt(this._characterCreationStateState.CharacterCreationManager.GetIndexOfCurrentStage),
					new ControlCharacterCreationStageReturnInt(this._characterCreationStateState.CharacterCreationManager.GetTotalStagesCount),
					new ControlCharacterCreationStageReturnInt(this._characterCreationStateState.CharacterCreationManager.GetFurthestIndex),
					new ControlCharacterCreationStageWithInt(this._characterCreationStateState.CharacterCreationManager.GoToStage)
				}) as CharacterCreationStageViewBase;
				stage.Listener = this._currentStageView;
				this._currentStageView.SetGenericScene(this._genericScene);
				return;
			}
			this._currentStageView = null;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00029567 File Offset: 0x00027767
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			CharacterCreationStageViewBase currentStageView = this._currentStageView;
			if (currentStageView == null)
			{
				return;
			}
			currentStageView.Tick(dt);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0002958D File Offset: 0x0002778D
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00029595 File Offset: 0x00027795
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002959D File Offset: 0x0002779D
		void IGameStateListener.OnInitialize()
		{
			base.OnInitialize();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000295A8 File Offset: 0x000277A8
		void IGameStateListener.OnFinalize()
		{
			base.OnFinalize();
			this.StopSound();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._genericScene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			this._genericScene.ClearAll();
			this._genericScene.ManualInvalidate();
			this._genericScene = null;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000295F8 File Offset: 0x000277F8
		private void CollectUnorderedStages()
		{
			Assembly assembly = typeof(CharacterCreationStageViewAttribute).Assembly;
			Assembly[] activeReferencingGameAssembliesSafe = assembly.GetActiveReferencingGameAssembliesSafe();
			this.CollectStagesFromAssembly(assembly);
			foreach (Assembly assembly2 in activeReferencingGameAssembliesSafe)
			{
				this.CollectStagesFromAssembly(assembly2);
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002963C File Offset: 0x0002783C
		private void CollectStagesFromAssembly(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				CharacterCreationStageViewAttribute characterCreationStageViewAttribute;
				if (typeof(CharacterCreationStageViewBase).IsAssignableFrom(type) && (characterCreationStageViewAttribute = type.GetCustomAttributesSafe(typeof(CharacterCreationStageViewAttribute), true).FirstOrDefault<object>() as CharacterCreationStageViewAttribute) != null)
				{
					if (this._stageViews.ContainsKey(characterCreationStageViewAttribute.StageType))
					{
						this._stageViews[characterCreationStageViewAttribute.StageType] = type;
					}
					else
					{
						this._stageViews.Add(characterCreationStageViewAttribute.StageType, type);
					}
				}
			}
		}

		// Token: 0x04000291 RID: 657
		private const string CultureParameterId = "MissionCulture";

		// Token: 0x04000292 RID: 658
		private readonly CharacterCreationState _characterCreationStateState;

		// Token: 0x04000293 RID: 659
		private IEnumerable<ScreenLayer> _shownLayers;

		// Token: 0x04000294 RID: 660
		private CharacterCreationStageViewBase _currentStageView;

		// Token: 0x04000295 RID: 661
		private readonly Dictionary<Type, Type> _stageViews;

		// Token: 0x04000296 RID: 662
		private SoundEvent _cultureAmbientSoundEvent;

		// Token: 0x04000297 RID: 663
		private Scene _genericScene;

		// Token: 0x04000298 RID: 664
		private MBAgentRendererSceneController _agentRendererSceneController;
	}
}
