using System;
using System.Collections.Generic;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.ServiceDiscovery.Client;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000062 RID: 98
	public static class MultiplayerMain
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000D5F2 File Offset: 0x0000B7F2
		public static LobbyClient GameClient
		{
			get
			{
				return NetworkMain.GameClient;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		static MultiplayerMain()
		{
			ServiceAddressManager.Initalize();
			MultiplayerMain._lobbyClientApplicationConfiguration = new ClientApplicationConfiguration();
			MultiplayerMain._lobbyClientApplicationConfiguration.FillFrom("LobbyClient");
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo("Multiplayer");
			if (!GameNetwork.IsDedicatedServer && moduleInfo != null)
			{
				MultiplayerMain._diamondClientApplication = new DiamondClientApplication(moduleInfo.Version);
				MultiplayerMain._diamondClientApplication.Initialize(MultiplayerMain._lobbyClientApplicationConfiguration);
				NetworkMain.SetPeers(MultiplayerMain._diamondClientApplication.GetClient<LobbyClient>("LobbyClient"), new CommunityClient(), null);
				MachineId.Initialize();
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000D680 File Offset: 0x0000B880
		public static void Initialize(IGameNetworkHandler gameNetworkHandler)
		{
			Debug.Print("Initializing NetworkMain", 0, Debug.DebugColor.White, 17592186044416UL);
			MBCommon.CurrentGameType = MBCommon.GameType.Single;
			GameNetwork.InitializeCompressionInfos();
			if (!MultiplayerMain.IsInitialized)
			{
				MultiplayerMain.IsInitialized = true;
				GameNetwork.Initialize(gameNetworkHandler);
			}
			PermaMuteList.SetPermanentMuteAvailableCallback(() => PlatformServices.Instance.IsPermanentMuteAvailable);
			Debug.Print("NetworkMain Initialized", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D6FB File Offset: 0x0000B8FB
		public static void InitializeAsDedicatedServer(IGameNetworkHandler gameNetworkHandler)
		{
			MBCommon.CurrentGameType = MBCommon.GameType.MultiServer;
			GameNetwork.InitializeCompressionInfos();
			if (!MultiplayerMain.IsInitialized)
			{
				MultiplayerMain.IsInitialized = true;
				GameNetwork.Initialize(gameNetworkHandler);
				GameStartupInfo startupInfo = Module.CurrentModule.StartupInfo;
				GameNetwork.SetServerBandwidthLimitInMbps(startupInfo.ServerBandwidthLimitInMbps);
				GameNetwork.SetServerTickRate((double)startupInfo.ServerTickRate);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000D73B File Offset: 0x0000B93B
		internal static void Tick(float dt)
		{
			if (MultiplayerMain.IsInitialized)
			{
				if (MultiplayerMain.GameClient != null)
				{
					MultiplayerMain.GameClient.Update();
				}
				if (MultiplayerMain._diamondClientApplication != null)
				{
					MultiplayerMain._diamondClientApplication.Update();
				}
				GameNetwork.Tick(dt);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000D76C File Offset: 0x0000B96C
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000D773 File Offset: 0x0000B973
		public static bool IsInitialized { get; private set; } = false;

		// Token: 0x060002E9 RID: 745 RVA: 0x0000D77B File Offset: 0x0000B97B
		public static MultiplayerGameType[] GetAvailableRankedGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.Captain,
				MultiplayerGameType.Skirmish
			};
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000D78B File Offset: 0x0000B98B
		public static MultiplayerGameType[] GetAvailableCustomGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.TeamDeathmatch,
				MultiplayerGameType.Siege
			};
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000D797 File Offset: 0x0000B997
		public static MultiplayerGameType[] GetAvailableQuickPlayGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.Captain,
				MultiplayerGameType.Skirmish
			};
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D7A7 File Offset: 0x0000B9A7
		public static string[] GetAvailableMatchmakerRegions()
		{
			return new string[] { "USE", "USW", "EU", "EA" };
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D7CF File Offset: 0x0000B9CF
		public static string GetUserDefaultRegion()
		{
			return "None";
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D7D6 File Offset: 0x0000B9D6
		public static string GetUserCurrentRegion()
		{
			LobbyClient gameClient = MultiplayerMain.GameClient;
			if (gameClient != null && gameClient.LoggedIn && MultiplayerMain.GameClient.PlayerData != null)
			{
				return MultiplayerMain.GameClient.PlayerData.LastRegion;
			}
			return MultiplayerMain.GetUserDefaultRegion();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D80C File Offset: 0x0000BA0C
		public static string[] GetUserSelectedGameTypes()
		{
			LobbyClient gameClient = MultiplayerMain.GameClient;
			if (gameClient != null && gameClient.LoggedIn)
			{
				return MultiplayerMain.GameClient.PlayerData.LastGameTypes;
			}
			return new string[0];
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D837 File Offset: 0x0000BA37
		[CommandLineFunctionality.CommandLineArgumentFunction("gettoken", "customserver")]
		public static string GetDedicatedCustomServerAuthToken(List<string> strings)
		{
			if (!(Common.PlatformFileHelper is PlatformFileHelperPC))
			{
				return "Platform not supported.";
			}
			if (MultiplayerMain.GameClient == null)
			{
				return "Not logged into lobby.";
			}
			MultiplayerMain.GetDedicatedCustomServerAuthToken();
			return string.Empty;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D864 File Offset: 0x0000BA64
		private static async void GetDedicatedCustomServerAuthToken()
		{
			string text = await MultiplayerMain.GameClient.GetDedicatedCustomServerAuthToken();
			if (text == null)
			{
				MBDebug.EchoCommandWindow("Could not get token.");
			}
			else
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Tokens");
				PlatformFilePath platformFilePath = new PlatformFilePath(platformDirectoryPath, "DedicatedCustomServerAuthToken.txt");
				FileHelper.SaveFileString(platformFilePath, text);
				MBDebug.EchoCommandWindow(text + " (Saved to " + platformFilePath.FileFullPath + ")");
			}
		}

		// Token: 0x040000EC RID: 236
		private static ClientApplicationConfiguration _lobbyClientApplicationConfiguration;

		// Token: 0x040000ED RID: 237
		private static DiamondClientApplication _diamondClientApplication;
	}
}
