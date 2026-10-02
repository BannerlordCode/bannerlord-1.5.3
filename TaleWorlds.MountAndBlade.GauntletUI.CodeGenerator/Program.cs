using System;
using System.Collections.Generic;
using NavalDLC.GauntletUI.Widgets.Widgets;
using NavalDLC.ViewModelCollection.Port;
using SandBox.ViewModelCollection;
using SandBox.ViewModelCollection.BoardGame;
using SandBox.ViewModelCollection.GameOver;
using SandBox.ViewModelCollection.Map;
using SandBox.ViewModelCollection.Map.Tracker;
using SandBox.ViewModelCollection.MapSiege;
using SandBox.ViewModelCollection.Missions;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Nameplate;
using SandBox.ViewModelCollection.SaveLoad;
using SandBox.ViewModelCollection.Tournament;
using StoryMode.ViewModelCollection.Missions;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.Barter;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.OptionsStage;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapConversation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Information.RundownTooltip;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.CodeGenerator;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.Credits;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed;
using TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu;
using TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries;
using TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;

namespace TaleWorlds.MountAndBlade.GauntletUI.CodeGenerator
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		[STAThread]
		private static int Main(string[] args)
		{
			Console.WriteLine("Assembly touched: " + typeof(SceneWidget).Assembly.FullName);
			Console.WriteLine("Assembly touched: " + typeof(TooltipWidget).Assembly.FullName);
			Console.WriteLine("Assembly touched: " + typeof(PortScreenWidget).Assembly.FullName);
			Common.SetInvariantCulture();
			Program.GenerateBannerlord();
			Program.GenerateMultiplayer();
			Program.GenerateSandBox();
			Program.GenerateStoryMode();
			Program.GenerateNavalDLC();
			return 0;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E0 File Offset: 0x000002E0
		private static void AddVariant(UICodeGenerationContext[] generationContexts, Type datasourceType, string prefabName)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("DataSourceType", datasourceType);
			generationContexts[Program.index++ % generationContexts.Length].AddPrefabVariant(prefabName, datasourceType.FullName, new UICodeGenerationDatabindingVariantExtension(), dictionary);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002124 File Offset: 0x00000324
		private static void GenerateBannerlord()
		{
			string[] array = new string[] { "../WOTS/GUI/GauntletUI/", "../WOTS/Modules/Native/GUI/" };
			PrefabExtension[] array2 = new PrefabDatabindingExtension[]
			{
				new PrefabDatabindingExtension()
			};
			PrefabExtension[] array3 = array2;
			UICodeGenerationContext[] array4 = new UICodeGenerationContext[]
			{
				new UICodeGenerationContext("TaleWorlds.MountAndBlade.GauntletUI.AutoGenerated0", "Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.AutoGenerated.0\\Generated"),
				new UICodeGenerationContext("TaleWorlds.MountAndBlade.GauntletUI.AutoGenerated1", "Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.AutoGenerated.1\\Generated")
			};
			WidgetInfo.Refresh();
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].Prepare(array, array3);
			}
			Program.AddVariant(array4, typeof(MPChatVM), "SPChatLog");
			Program.AddVariant(array4, typeof(FaceGenVM), "FaceGen");
			Program.AddVariant(array4, typeof(MissionOrderVM), "OrderRadial");
			Program.AddVariant(array4, typeof(MissionOrderVM), "OrderBar");
			Program.AddVariant(array4, typeof(MissionOrderVM), "OrderBar");
			Program.AddVariant(array4, typeof(InitialMenuVM), "InitialScreen");
			Program.AddVariant(array4, typeof(CreditsVM), "CreditsScreen");
			Program.AddVariant(array4, typeof(EscapeMenuVM), "EscapeMenu");
			Program.AddVariant(array4, typeof(SPKillFeedVM), "SingleplayerKillfeed");
			Program.AddVariant(array4, typeof(MissionAgentStatusVM), "MainAgentHUD");
			Program.AddVariant(array4, typeof(MissionMainAgentCheerBarkControllerVM), "MainAgentCheerBarkController");
			Program.AddVariant(array4, typeof(GameNotificationVM), "GameNotificationUI");
			Program.AddVariant(array4, typeof(PropertyBasedTooltipVM), "PropertyBasedTooltip");
			Program.AddVariant(array4, typeof(HintVM), "HintTooltip");
			Program.AddVariant(array4, typeof(TextQueryPopUpVM), "TextQueryPopup");
			Program.AddVariant(array4, typeof(SingleQueryPopUpVM), "SingleQueryPopup");
			Program.AddVariant(array4, typeof(MultiSelectionQueryPopUpVM), "MultiSelectionQueryPopup");
			Program.AddVariant(array4, typeof(CrosshairVM), "Crosshair");
			Program.AddVariant(array4, typeof(MissionMainAgentControllerEquipDropVM), "MainAgentControllerEquipDrop");
			Program.AddVariant(array4, typeof(CustomBattleScoreboardVM), "SPScoreboard");
			for (int j = 0; j < array4.Length; j++)
			{
				array4[j].Generate();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002368 File Offset: 0x00000568
		private static void GenerateMultiplayer()
		{
			string[] array = new string[] { "../WOTS/GUI/GauntletUI/", "../WOTS/Modules/Native/GUI/", "../WOTS/Modules/Multiplayer/GUI/" };
			PrefabExtension[] array2 = new PrefabDatabindingExtension[]
			{
				new PrefabDatabindingExtension()
			};
			PrefabExtension[] array3 = array2;
			UICodeGenerationContext[] array4 = new UICodeGenerationContext[]
			{
				new UICodeGenerationContext("TaleWorlds.MountAndBlade.GauntletUI.AutoGenerated0", "Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.AutoGenerated\\Generated")
			};
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].Prepare(array, array3);
			}
			Program.AddVariant(array4, typeof(MultiplayerDuelVM), "MultiplayerDuel");
			Program.AddVariant(array4, typeof(MissionScoreboardVM), "MultiplayerScoreboard");
			Program.AddVariant(array4, typeof(MultiplayerMissionMarkerVM), "MPMissionMarkers");
			Program.AddVariant(array4, typeof(GameNotificationVM), "MultiplayerGameNotificationUI");
			Program.AddVariant(array4, typeof(MPChatVM), "MPChatLog");
			Program.AddVariant(array4, typeof(MPKillFeedVM), "MultiplayerKillFeed");
			Program.AddVariant(array4, typeof(MPDeathCardVM), "MultiplayerDeathCard");
			Program.AddVariant(array4, typeof(MultiplayerPollProgressVM), "MultiplayerPollingProgress");
			Program.AddVariant(array4, typeof(MultiplayerEndOfBattleVM), "MultiplayerEndOfBattle");
			Program.AddVariant(array4, typeof(MultiplayerEndOfRoundVM), "MultiplayerEndOfRound");
			Program.AddVariant(array4, typeof(MultiplayerMissionServerStatusVM), "MultiplayerServerStatus");
			Program.AddVariant(array4, typeof(MPEscapeMenuVM), "MultiplayerEscapeMenu");
			Program.AddVariant(array4, typeof(MultiplayerReportPlayerVM), "MultiplayerReportPlayer");
			Program.AddVariant(array4, typeof(MultiplayerAdminInformationVM), "MultiplayerAdminInformation");
			Program.AddVariant(array4, typeof(MultiplayerTeamSelectVM), "MultiplayerTeamSelection");
			Program.AddVariant(array4, typeof(MultiplayerCultureSelectVM), "MultiplayerCultureSelection");
			Program.AddVariant(array4, typeof(MPIntermissionVM), "MultiplayerIntermission");
			Program.AddVariant(array4, typeof(MissionMultiplayerHUDExtensionVM), "HUDExtension");
			for (int j = 0; j < array4.Length; j++)
			{
				array4[j].Generate();
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002570 File Offset: 0x00000770
		private static void GenerateSandBox()
		{
			string[] array = new string[] { "../WOTS/GUI/GauntletUI/", "../WOTS/Modules/Native/GUI/", "../WOTS/Modules/SandBoxCore/GUI/", "../WOTS/Modules/SandBox/GUI/" };
			PrefabExtension[] array2 = new PrefabDatabindingExtension[]
			{
				new PrefabDatabindingExtension()
			};
			PrefabExtension[] array3 = array2;
			UICodeGenerationContext[] array4 = new UICodeGenerationContext[]
			{
				new UICodeGenerationContext("SandBox.GauntletUI.AutoGenerated0", "Bannerlord\\SandBox.GauntletUI.AutoGenerated.0\\Generated"),
				new UICodeGenerationContext("SandBox.GauntletUI.AutoGenerated1", "Bannerlord\\SandBox.GauntletUI.AutoGenerated.1\\Generated")
			};
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].Prepare(array, array3);
			}
			Program.AddVariant(array4, typeof(SPInventoryVM), "Inventory");
			Program.AddVariant(array4, typeof(MapBarVM), "MapBar");
			Program.AddVariant(array4, typeof(ClanManagementVM), "ClanScreen");
			Program.AddVariant(array4, typeof(PartyNameplatesVM), "PartyNameplate");
			Program.AddVariant(array4, typeof(MapConversationVM), "MapConversation");
			Program.AddVariant(array4, typeof(EncyclopediaHomeVM), "EncyclopediaHome");
			Program.AddVariant(array4, typeof(EncyclopediaNavigatorVM), "EncyclopediaBar");
			Program.AddVariant(array4, typeof(SceneNotificationVM), "SceneNotification");
			Program.AddVariant(array4, typeof(TournamentVM), "Tournament");
			Program.AddVariant(array4, typeof(MissionNameMarkerVM), "NameMarker");
			Program.AddVariant(array4, typeof(MissionConversationVM), "SPConversation");
			Program.AddVariant(array4, typeof(MissionArenaPracticeFightVM), "ArenaPracticeFight");
			Program.AddVariant(array4, typeof(BoardGameVM), "BoardGame");
			Program.AddVariant(array4, typeof(TournamentLeaderboardVM), "GameMenuTournamentLeaderboard");
			Program.AddVariant(array4, typeof(RecruitmentVM), "RecruitmentPopup");
			Program.AddVariant(array4, typeof(SettlementMenuOverlayVM), "SettlementOverlay");
			Program.AddVariant(array4, typeof(EncounterMenuOverlayVM), "EncounterOverlay");
			Program.AddVariant(array4, typeof(GameMenuTroopSelectionVM), "GameMenuTroopSelection");
			Program.AddVariant(array4, typeof(MapSiegeVM), "MapSiegeOverlay");
			Program.AddVariant(array4, typeof(MapNotificationVM), "MapNotificationUI");
			Program.AddVariant(array4, typeof(MapTrackerCollectionVM), "MapTrackers");
			Program.AddVariant(array4, typeof(MapEventVisualsVM), "MapEventVisuals");
			Program.AddVariant(array4, typeof(SaveLoadVM), "SaveLoadScreen");
			Program.AddVariant(array4, typeof(SettlementNameplatesVM), "SettlementNameplate");
			Program.AddVariant(array4, typeof(PartyVM), "PartyScreen");
			Program.AddVariant(array4, typeof(TownManagementVM), "TownManagement");
			Program.AddVariant(array4, typeof(BarterVM), "BarterScreen");
			Program.AddVariant(array4, typeof(ArmyManagementVM), "ArmyManagement");
			Program.AddVariant(array4, typeof(BannerEditorVM), "BannerEditor");
			Program.AddVariant(array4, typeof(GameMenuVM), "GameMenu");
			Program.AddVariant(array4, typeof(CraftingVM), "Crafting");
			Program.AddVariant(array4, typeof(ArmyMenuOverlayVM), "ArmyOverlay");
			Program.AddVariant(array4, typeof(CharacterDeveloperVM), "CharacterDeveloper");
			Program.AddVariant(array4, typeof(CharacterCreationClanNamingStageVM), "CharacterCreationClanNamingStage");
			Program.AddVariant(array4, typeof(CharacterCreationCultureStageVM), "CharacterCreationCultureStage");
			Program.AddVariant(array4, typeof(CharacterCreationNarrativeStageVM), "CharacterCreationNarrativeStage");
			Program.AddVariant(array4, typeof(CharacterCreationOptionsStageVM), "CharacterCreationOptionsStage");
			Program.AddVariant(array4, typeof(CharacterCreationReviewStageVM), "CharacterCreationReviewStage");
			Program.AddVariant(array4, typeof(QuestsVM), "QuestsScreen");
			Program.AddVariant(array4, typeof(CampaignOptionsVM), "CampaignOptions");
			Program.AddVariant(array4, typeof(SPScoreboardVM), "SPScoreboard");
			Program.AddVariant(array4, typeof(EncyclopediaPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaListVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaClanPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaConceptPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaFactionPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaHeroPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaShipPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaSettlementPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaUnitPageVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaHomeVM), "EncyclopediaItemList");
			Program.AddVariant(array4, typeof(EncyclopediaHeroPageVM), "EncyclopediaHeroPage");
			Program.AddVariant(array4, typeof(EncyclopediaShipPageVM), "EncyclopediaShipPage");
			Program.AddVariant(array4, typeof(EncyclopediaClanPageVM), "EncyclopediaClanPage");
			Program.AddVariant(array4, typeof(EncyclopediaConceptPageVM), "EncyclopediaConceptPage");
			Program.AddVariant(array4, typeof(EncyclopediaFactionPageVM), "EncyclopediaFactionPage");
			Program.AddVariant(array4, typeof(EncyclopediaUnitPageVM), "EncyclopediaUnitPage");
			Program.AddVariant(array4, typeof(GameOverVM), "GameOverScreen");
			Program.AddVariant(array4, typeof(RundownTooltipVM), "RundownTooltip");
			for (int j = 0; j < array4.Length; j++)
			{
				array4[j].Generate();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private static void GenerateStoryMode()
		{
			UICodeGenerationContext uicodeGenerationContext = new UICodeGenerationContext("StoryMode.GauntletUI.AutoGenerated", "Bannerlord\\StoryMode.GauntletUI.AutoGenerated\\Generated");
			uicodeGenerationContext.Prepare(new string[] { "../WOTS/GUI/GauntletUI/", "../WOTS/Modules/Native/GUI/", "../WOTS/Modules/SandBoxCore/GUI/", "../WOTS/Modules/SandBox/GUI/", "../WOTS/Modules/StoryMode/GUI/" }, new PrefabDatabindingExtension[]
			{
				new PrefabDatabindingExtension()
			});
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type typeFromHandle = typeof(TrainingFieldObjectivesVM);
			dictionary.Add("DataSourceType", typeFromHandle);
			uicodeGenerationContext.AddPrefabVariant("TrainingFieldObjectives", typeFromHandle.FullName, new UICodeGenerationDatabindingVariantExtension(), dictionary);
			uicodeGenerationContext.Generate();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002B8C File Offset: 0x00000D8C
		private static void GenerateNavalDLC()
		{
			UICodeGenerationContext uicodeGenerationContext = new UICodeGenerationContext("NavalDLC.GauntletUI.AutoGenerated", "Bannerlord\\NavalDLC.GauntletUI.AutoGenerated\\Generated");
			uicodeGenerationContext.Prepare(new string[] { "../WOTS/GUI/GauntletUI/", "../WOTS/Modules/Native/GUI/", "../WOTS/Modules/SandBoxCore/GUI/", "../WOTS/Modules/SandBox/GUI/", "../WOTS/Modules/StoryMode/GUI/", "../WOTS/Modules/NavalDLC/GUI/" }, new PrefabDatabindingExtension[]
			{
				new PrefabDatabindingExtension()
			});
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type typeFromHandle = typeof(PortVM);
			dictionary.Add("DataSourceType", typeFromHandle);
			uicodeGenerationContext.AddPrefabVariant("PortScreen", typeFromHandle.FullName, new UICodeGenerationDatabindingVariantExtension(), dictionary);
			uicodeGenerationContext.Generate();
		}

		// Token: 0x04000001 RID: 1
		private static int index;
	}
}
