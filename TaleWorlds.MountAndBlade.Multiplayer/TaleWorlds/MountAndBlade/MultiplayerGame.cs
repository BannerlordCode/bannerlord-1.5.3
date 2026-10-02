using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000021 RID: 33
	public class MultiplayerGame : GameType
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007A67 File Offset: 0x00005C67
		public override bool IsCoreOnlyGameMode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007A6A File Offset: 0x00005C6A
		public static MultiplayerGame Current
		{
			get
			{
				return Game.Current.GameType as MultiplayerGame;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007A7B File Offset: 0x00005C7B
		public override bool RequiresTutorial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007A88 File Offset: 0x00005C88
		protected override void OnInitialize()
		{
			Game currentGame = base.CurrentGame;
			IGameStarter gameStarter = new BasicGameStarter();
			this.AddGameModels(gameStarter);
			base.GameManager.InitializeGameStarter(currentGame, gameStarter);
			base.GameManager.OnGameStart(base.CurrentGame, gameStarter);
			currentGame.SetBasicModels(gameStarter.Models);
			currentGame.CreateGameManager();
			base.GameManager.BeginGameStart(base.CurrentGame);
			currentGame.InitializeDefaultGameObjects();
			if (!GameNetwork.IsDedicatedServer)
			{
				currentGame.GameTextManager.LoadGameTexts();
			}
			currentGame.LoadBasicFiles();
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("MPCharacters", false);
			base.ObjectManager.LoadXML("BasicCultures", false);
			base.ObjectManager.LoadXML("MPClassDivisions", false);
			base.ObjectManager.UnregisterNonReadyObjects();
			MultiplayerClassDivisions.Initialize();
			BadgeManager.InitializeWithXML(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/mpbadges.xml");
			base.GameManager.OnNewCampaignStart(base.CurrentGame, null);
			base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
			base.CurrentGame.AddGameHandler<ChatBox>();
			if (GameNetwork.IsDedicatedServer)
			{
				base.CurrentGame.AddGameHandler<MultiplayerGameLogger>();
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007BCC File Offset: 0x00005DCC
		private void AddGameModels(IGameStarter basicGameStarter)
		{
			basicGameStarter.AddModel<RidingModel>(new MultiplayerRidingModel());
			basicGameStarter.AddModel<StrikeMagnitudeCalculationModel>(new MultiplayerStrikeMagnitudeModel());
			basicGameStarter.AddModel<AgentStatCalculateModel>(new MultiplayerAgentStatCalculateModel());
			basicGameStarter.AddModel<AgentApplyDamageModel>(new MultiplayerAgentApplyDamageModel());
			basicGameStarter.AddModel<BattleMoraleModel>(new MultiplayerBattleMoraleModel());
			basicGameStarter.AddModel<BattleInitializationModel>(new MultiplayerBattleInitializationModel());
			basicGameStarter.AddModel<BattleSpawnModel>(new MultiplayerBattleSpawnModel());
			basicGameStarter.AddModel<BattleBannerBearersModel>(new MultiplayerBattleBannerBearersModel());
			basicGameStarter.AddModel<FormationArrangementModel>(new DefaultFormationArrangementModel());
			basicGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new DefaultAgentDecideKilledOrUnconsciousModel());
			basicGameStarter.AddModel<DamageParticleModel>(new DefaultDamageParticleModel());
			basicGameStarter.AddModel<ItemPickupModel>(new DefaultItemPickupModel());
			basicGameStarter.AddModel<MissionSiegeEngineCalculationModel>(new DefaultSiegeEngineCalculationModel());
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007C68 File Offset: 0x00005E68
		public static Dictionary<string, Equipment> ReadDefaultEquipments(string defaultEquipmentsPath)
		{
			Dictionary<string, Equipment> dictionary = new Dictionary<string, Equipment>();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(defaultEquipmentsPath);
			foreach (object obj in xmlDocument.ChildNodes[0].ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					string value = xmlNode.Attributes["name"].Value;
					Equipment equipment = new Equipment(Equipment.EquipmentType.Battle);
					equipment.Deserialize(null, xmlNode);
					dictionary.Add(value, equipment);
				}
			}
			return dictionary;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007D14 File Offset: 0x00005F14
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007D16 File Offset: 0x00005F16
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "MPCharacters", 43U, true, false);
			objectManager.RegisterType<BasicCultureObject>("Culture", "BasicCultures", 17U, true, false);
			objectManager.RegisterType<MultiplayerClassDivisions.MPHeroClass>("MPClassDivision", "MPClassDivisions", 45U, true, false);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007D54 File Offset: 0x00005F54
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

		// Token: 0x060001B3 RID: 435 RVA: 0x00007D86 File Offset: 0x00005F86
		public override void OnDestroy()
		{
			BadgeManager.OnFinalize();
			MultiplayerOptions.Release();
			InformationManager.ClearAllMessages();
			MultiplayerClassDivisions.Release();
			AvatarServices.ClearAvatarCaches();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007DA1 File Offset: 0x00005FA1
		public override void OnStateChanged(GameState oldState)
		{
		}
	}
}
