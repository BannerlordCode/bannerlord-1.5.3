using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattleObjects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000F RID: 15
	public class CustomGame : GameType
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00007CAE File Offset: 0x00005EAE
		public IEnumerable<CustomBattleSceneData> CustomBattleScenes
		{
			get
			{
				return this._customBattleScenes;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00007CB6 File Offset: 0x00005EB6
		public override bool IsCoreOnlyGameMode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00007CB9 File Offset: 0x00005EB9
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00007CC1 File Offset: 0x00005EC1
		public CustomBattleBannerEffects CustomBattleBannerEffects { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007CCA File Offset: 0x00005ECA
		public static CustomGame Current
		{
			get
			{
				return Game.Current.GameType as CustomGame;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007CDB File Offset: 0x00005EDB
		public CustomGame()
		{
			this._customBattleScenes = new List<CustomBattleSceneData>();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007CF0 File Offset: 0x00005EF0
		protected override void OnInitialize()
		{
			this.InitializeScenes();
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

		// Token: 0x060000E9 RID: 233 RVA: 0x00007DBC File Offset: 0x00005FBC
		private void InitializeGameModels(IGameStarter basicGameStarter)
		{
			basicGameStarter.AddModel<AgentStatCalculateModel>(new CustomBattleAgentStatCalculateModel());
			basicGameStarter.AddModel<AgentApplyDamageModel>(new CustomAgentApplyDamageModel());
			basicGameStarter.AddModel<ApplyWeatherEffectsModel>(new CustomBattleApplyWeatherEffectsModel());
			basicGameStarter.AddModel<AutoBlockModel>(new CustomBattleAutoBlockModel());
			basicGameStarter.AddModel<BattleMoraleModel>(new CustomBattleMoraleModel());
			basicGameStarter.AddModel<BattleInitializationModel>(new CustomBattleInitializationModel());
			basicGameStarter.AddModel<BattleSpawnModel>(new CustomBattleSpawnModel());
			basicGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new DefaultAgentDecideKilledOrUnconsciousModel());
			basicGameStarter.AddModel<MissionDifficultyModel>(new DefaultMissionDifficultyModel());
			basicGameStarter.AddModel<RidingModel>(new DefaultRidingModel());
			basicGameStarter.AddModel<StrikeMagnitudeCalculationModel>(new DefaultStrikeMagnitudeModel());
			basicGameStarter.AddModel<BattleBannerBearersModel>(new CustomBattleBannerBearersModel());
			basicGameStarter.AddModel<FormationArrangementModel>(new DefaultFormationArrangementModel());
			basicGameStarter.AddModel<DamageParticleModel>(new DefaultDamageParticleModel());
			basicGameStarter.AddModel<ItemPickupModel>(new DefaultItemPickupModel());
			basicGameStarter.AddModel<ItemValueModel>(new DefaultItemValueModel());
			basicGameStarter.AddModel<MissionSiegeEngineCalculationModel>(new DefaultSiegeEngineCalculationModel());
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007E84 File Offset: 0x00006084
		private void InitializeScenes()
		{
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("CustomBattleScenes", true, true, "");
			this.LoadCustomBattleScenes(mergedXmlForManaged);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007EAC File Offset: 0x000060AC
		private void LoadCustomGameXmls()
		{
			this.CustomBattleBannerEffects = new CustomBattleBannerEffects();
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("EquipmentRosters", false);
			base.ObjectManager.LoadXML("NPCCharacters", false);
			base.ObjectManager.LoadXML("SPCultures", false);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007F08 File Offset: 0x00006108
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007F0A File Offset: 0x0000610A
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "NPCCharacters", 43U, true, false);
			objectManager.RegisterType<BasicCultureObject>("Culture", "SPCultures", 17U, true, false);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007F34 File Offset: 0x00006134
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

		// Token: 0x060000EF RID: 239 RVA: 0x00007F66 File Offset: 0x00006166
		public override void OnDestroy()
		{
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007F68 File Offset: 0x00006168
		private void LoadCustomBattleScenes(XmlDocument doc)
		{
			if (doc.ChildNodes.Count == 0)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document has no nodes.");
			}
			bool flag = doc.ChildNodes[0].Name.ToLower().Equals("xml");
			if (flag && doc.ChildNodes.Count == 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least one child node");
			}
			XmlNode xmlNode = (flag ? doc.ChildNodes[1] : doc.ChildNodes[0]);
			if (xmlNode.Name != "CustomBattleScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be CustomBattleScenes.");
			}
			if (xmlNode.Name == "CustomBattleScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						TextObject textObject = null;
						TerrainType terrainType = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						bool flag2 = false;
						bool flag3 = false;
						bool flag4 = false;
						bool flag5 = false;
						bool flag6 = false;
						string text2 = "";
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "name")
							{
								textObject = new TextObject(xmlNode2.Attributes[i].InnerText, null);
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrainType))
								{
									terrainType = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array[0] = char.ToUpper(array[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "is_siege_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out flag2);
							}
							else if (xmlNode2.Attributes[i].Name == "is_village_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out flag3);
							}
							else if (xmlNode2.Attributes[i].Name == "is_lords_hall_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out flag4);
							}
							else if (xmlNode2.Attributes[i].Name == "is_naval_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out flag5);
							}
							else if (xmlNode2.Attributes[i].Name == "is_naval_raid_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out flag6);
							}
							else if (xmlNode2.Attributes[i].Name == "forced_scene_level")
							{
								text2 = xmlNode2.Attributes[i].InnerText;
							}
						}
						if (!flag5 && !flag6)
						{
							XmlNodeList childNodes = xmlNode2.ChildNodes;
							List<TerrainType> list = new List<TerrainType>();
							foreach (object obj2 in childNodes)
							{
								XmlNode xmlNode3 = (XmlNode)obj2;
								if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "flags")
								{
									foreach (object obj3 in xmlNode3.ChildNodes)
									{
										XmlNode xmlNode4 = (XmlNode)obj3;
										TerrainType terrainType2;
										if (xmlNode4.NodeType != XmlNodeType.Comment && xmlNode4.Attributes["name"].InnerText == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["value"].InnerText, out terrainType2) && !list.Contains(terrainType2))
										{
											list.Add(terrainType2);
										}
									}
								}
							}
							this._customBattleScenes.Add(new CustomBattleSceneData(text, textObject, terrainType, list, forestDensity, flag2, flag3, flag4, text2));
						}
					}
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000849C File Offset: 0x0000669C
		public override void OnStateChanged(GameState oldState)
		{
		}

		// Token: 0x04000080 RID: 128
		private List<CustomBattleSceneData> _customBattleScenes;

		// Token: 0x04000081 RID: 129
		private const TerrainType DefaultTerrain = TerrainType.Plain;

		// Token: 0x04000082 RID: 130
		private const ForestDensity DefaultForestDensity = ForestDensity.None;
	}
}
