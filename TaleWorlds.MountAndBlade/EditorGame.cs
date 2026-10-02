using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000220 RID: 544
	public class EditorGame : GameType
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x0006DF96 File Offset: 0x0006C196
		public static EditorGame Current
		{
			get
			{
				return Game.Current.GameType as EditorGame;
			}
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0006DFB0 File Offset: 0x0006C1B0
		protected override void OnInitialize()
		{
			Game currentGame = base.CurrentGame;
			IGameStarter gameStarter = new BasicGameStarter();
			this.InitializeGameModels(gameStarter);
			base.GameManager.InitializeGameStarter(currentGame, gameStarter);
			base.GameManager.OnGameStart(base.CurrentGame, gameStarter);
			MBObjectManager objectManager = currentGame.ObjectManager;
			currentGame.SetBasicModels(gameStarter.Models);
			currentGame.CreateGameManager();
			base.GameManager.BeginGameStart(base.CurrentGame);
			currentGame.InitializeDefaultGameObjects();
			currentGame.LoadBasicFiles();
			this.LoadCustomGameXmls();
			objectManager.UnregisterNonReadyObjects();
			currentGame.SetDefaultEquipments(new Dictionary<string, Equipment>());
			objectManager.UnregisterNonReadyObjects();
			base.GameManager.OnNewCampaignStart(base.CurrentGame, null);
			base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0006E078 File Offset: 0x0006C278
		private void InitializeGameModels(IGameStarter basicGameStarter)
		{
			basicGameStarter.AddModel<AgentStatCalculateModel>(new CustomBattleAgentStatCalculateModel());
			basicGameStarter.AddModel<AgentApplyDamageModel>(new CustomAgentApplyDamageModel());
			basicGameStarter.AddModel<ApplyWeatherEffectsModel>(new CustomBattleApplyWeatherEffectsModel());
			basicGameStarter.AddModel<BattleMoraleModel>(new CustomBattleMoraleModel());
			basicGameStarter.AddModel<BattleInitializationModel>(new CustomBattleInitializationModel());
			basicGameStarter.AddModel<BattleSpawnModel>(new CustomBattleSpawnModel());
			basicGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new DefaultAgentDecideKilledOrUnconsciousModel());
			basicGameStarter.AddModel<RidingModel>(new DefaultRidingModel());
			basicGameStarter.AddModel<StrikeMagnitudeCalculationModel>(new DefaultStrikeMagnitudeModel());
			basicGameStarter.AddModel<BattleBannerBearersModel>(new CustomBattleBannerBearersModel());
			basicGameStarter.AddModel<FormationArrangementModel>(new DefaultFormationArrangementModel());
			basicGameStarter.AddModel<DamageParticleModel>(new DefaultDamageParticleModel());
			basicGameStarter.AddModel<ItemPickupModel>(new DefaultItemPickupModel());
			basicGameStarter.AddModel<MissionSiegeEngineCalculationModel>(new DefaultSiegeEngineCalculationModel());
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0006E120 File Offset: 0x0006C320
		private void LoadCustomGameXmls()
		{
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("EquipmentRosters", false);
			base.ObjectManager.LoadXML("NPCCharacters", false);
			base.ObjectManager.LoadXML("SPCultures", false);
			if (ModuleHelper.IsModuleActive("NavalDLC"))
			{
				base.ObjectManager.LoadXML("ShipPhysicsReferences", false);
				base.ObjectManager.LoadXML("MissionShips", false);
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0006E19F File Offset: 0x0006C39F
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0006E1A4 File Offset: 0x0006C3A4
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "NPCCharacters", 43U, true, false);
			objectManager.RegisterType<BasicCultureObject>("Culture", "SPCultures", 17U, true, false);
			if (ModuleHelper.IsModuleActive("NavalDLC"))
			{
				objectManager.RegisterType<MissionShipObject>("MissionShip", "MissionShips", 57U, true, false);
				objectManager.RegisterType<ShipPhysicsReference>("ShipPhysicsReference", "ShipPhysicsReferences", 64U, true, false);
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0006E20D File Offset: 0x0006C40D
		protected override void DoLoadingForGameType(GameTypeLoadingStates gameTypeLoadingState, out GameTypeLoadingStates nextState)
		{
			nextState = GameTypeLoadingStates.None;
			switch (gameTypeLoadingState)
			{
			case GameTypeLoadingStates.InitializeFirstStep:
				base.CurrentGame.Initialize();
				nextState = GameTypeLoadingStates.WaitSecondStep;
				return;
			case GameTypeLoadingStates.WaitSecondStep:
				nextState = GameTypeLoadingStates.LoadVisualsThirdState;
				return;
			case GameTypeLoadingStates.LoadVisualsThirdState:
				nextState = GameTypeLoadingStates.PostInitializeFourthState;
				break;
			case GameTypeLoadingStates.PostInitializeFourthState:
				break;
			default:
				return;
			}
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0006E23F File Offset: 0x0006C43F
		public override void OnDestroy()
		{
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0006E241 File Offset: 0x0006C441
		public override void OnStateChanged(GameState oldState)
		{
		}
	}
}
