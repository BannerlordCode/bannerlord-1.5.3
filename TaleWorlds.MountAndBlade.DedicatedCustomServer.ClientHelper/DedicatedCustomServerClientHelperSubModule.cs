using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Multiplayer.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.DedicatedCustomServer.ClientHelper
{
	// Token: 0x02000004 RID: 4
	public class DedicatedCustomServerClientHelperSubModule : MBSubModuleBase
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00002B51 File Offset: 0x00000D51
		public DedicatedCustomServerClientHelperSubModule()
		{
			this._httpClient = new HttpClient();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B64 File Offset: 0x00000D64
		protected override void OnSubModuleLoad()
		{
			DedicatedCustomServerClientHelperSubModule.Instance = this;
			base.OnSubModuleLoad();
			TauntUsageManager.Initialize();
			ModLogger.Log("Loaded", 0, Debug.DebugColor.Green);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B84 File Offset: 0x00000D84
		public override void OnMultiplayerGameStart(Game game, object _)
		{
			game.GameStateManager.RegisterListener(new DedicatedCustomServerClientHelperSubModule.StateManagerListener());
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B98 File Offset: 0x00000D98
		public async Task DownloadMapFromHost(string hostAddress, string mapName, bool replaceExisting = false, IProgress<ProgressUpdate> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			string text = "http://" + hostAddress + "/maps/" + ((mapName == null) ? "current" : ("list/" + mapName));
			string text2;
			try
			{
				ModLogger.Log("Downloading from '" + hostAddress + "' in thread...", 0, Debug.DebugColor.Green);
				text2 = await ModHelpers.DownloadToTempFile(this._httpClient, text, progress, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new Exception(new TextObject("{=lTiacca1}Failed to download map file '{MAP_NAME}'", null).SetTextVariable("MAP_NAME", mapName).ToString() + ": " + ex.Message, ex);
			}
			bool flag = true;
			string text3;
			bool flag2 = Utilities.TryGetFullFilePathOfScene(mapName, out text3);
			bool flag3 = flag2 && !ModHelpers.DoesSceneFolderAlreadyExist(mapName);
			string text5;
			try
			{
				string text4 = ModHelpers.ExtractZipToTempDirectory(text2);
				File.Delete(text2);
				text5 = ModHelpers.ReadSceneNameOfDirectory(text4);
				string text6 = Path.Combine(ModHelpers.GetSceneObjRootPath(), text5);
				if (Directory.Exists(text6))
				{
					if (!replaceExisting)
					{
						Directory.Delete(text4, true);
						throw new Exception(new TextObject("{=5bbkOm7r}Map already exists at '{MAP_PATH}', delete this directory first if you want to re-download", null).SetTextVariable("MAP_PATH", text6).ToString());
					}
					flag = !flag2;
					ModLogger.Warn("Been told to replace existing map, deleting '" + text6 + "'");
					Directory.Delete(text6, true);
				}
				Utilities.ExecuteCommandLineCommand("resource.shader.invalidate_temp_shader_cache_of_scene Multiplayer/" + text5);
				Directory.Move(text4, text6);
				ModLogger.Log("Scene is available at '" + text6 + "'", 0, Debug.DebugColor.Green);
			}
			catch (Exception ex2)
			{
				throw new Exception(new TextObject("{=oaNqdada}Failed to save map scene '{MAP_NAME}'", null).SetTextVariable("MAP_NAME", mapName).ToString() + ": " + ex2.Message, ex2);
			}
			if (flag3)
			{
				throw new Exception(new TextObject("{=Urgon7l2}'{MAP_NAME}' was downloaded, but another module already has a scene with this name. To play the new scene, restart the game without that module/scene.", null).SetTextVariable("MAP_NAME", mapName).ToString());
			}
			if (flag)
			{
				try
				{
					Utilities.PairSceneNameToModuleName(text5, "Multiplayer");
					ModLogger.Log("RGL has been informed of the module pairing for scene '" + text5 + "'", 0, Debug.DebugColor.Green);
				}
				catch (Exception ex3)
				{
					throw new Exception(new TextObject("{=iRossEAk}Failed to inform game engine about the new scene '{SCENE_NAME}'", null).SetTextVariable("SCENE_NAME", text5).ToString() + ": " + ex3.Message, ex3);
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C08 File Offset: 0x00000E08
		public async Task<MapListResponse> GetMapListFromHost(string hostAddress)
		{
			MapListResponse mapListResponse;
			try
			{
				string text = await HttpHelper.DownloadStringTaskAsync("http://" + hostAddress + "/maps/list");
				ModLogger.Log("'" + hostAddress + "' has a map list of: " + text, 0, Debug.DebugColor.Green);
				mapListResponse = JsonConvert.DeserializeObject<MapListResponse>(text);
			}
			catch (Exception ex)
			{
				throw new Exception(new TextObject("{=5ZkdGgnQ}Failed to retrieve map list of '{HOST_ADDRESS}'", null).SetTextVariable("HOST_ADDRESS", hostAddress).ToString() + ": " + ex.Message, ex);
			}
			return mapListResponse;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C50 File Offset: 0x00000E50
		[CommandLineFunctionality.CommandLineArgumentFunction("download_map", "dcshelper")]
		public static string DownloadMapCommand(List<string> strings)
		{
			DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass13_0 CS$<>8__locals1 = new DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass13_0();
			string text = "Usage: dcshelper.download_map [host_address[:port]] [map_name]\nOmit map_name to download the currently played map";
			if (strings.Count == 0)
			{
				return text;
			}
			CS$<>8__locals1.hostAddress = strings[0];
			CS$<>8__locals1.mapArg = ((strings.Count > 1) ? strings[1] : null);
			Task.Run(delegate
			{
				DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass13_0.<<DownloadMapCommand>b__0>d <<DownloadMapCommand>b__0>d;
				<<DownloadMapCommand>b__0>d.<>4__this = CS$<>8__locals1;
				<<DownloadMapCommand>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<DownloadMapCommand>b__0>d.<>1__state = -1;
				AsyncTaskMethodBuilder <>t__builder = <<DownloadMapCommand>b__0>d.<>t__builder;
				<>t__builder.Start<DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass13_0.<<DownloadMapCommand>b__0>d>(ref <<DownloadMapCommand>b__0>d);
				return <<DownloadMapCommand>b__0>d.<>t__builder.Task;
			});
			return "Attempting to download in the background...";
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CB0 File Offset: 0x00000EB0
		[CommandLineFunctionality.CommandLineArgumentFunction("get_map_list", "dcshelper")]
		public static string GetMapListCommand(List<string> strings)
		{
			DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass14_0 CS$<>8__locals1 = new DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass14_0();
			string text = "Usage: dcshelper.get_map_list [host_address[:port]]";
			if (strings.Count != 1)
			{
				return text;
			}
			CS$<>8__locals1.hostAddress = strings[0];
			Task.Run(delegate
			{
				DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass14_0.<<GetMapListCommand>b__0>d <<GetMapListCommand>b__0>d;
				<<GetMapListCommand>b__0>d.<>4__this = CS$<>8__locals1;
				<<GetMapListCommand>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<GetMapListCommand>b__0>d.<>1__state = -1;
				AsyncTaskMethodBuilder <>t__builder = <<GetMapListCommand>b__0>d.<>t__builder;
				<>t__builder.Start<DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass14_0.<<GetMapListCommand>b__0>d>(ref <<GetMapListCommand>b__0>d);
				return <<GetMapListCommand>b__0>d.<>t__builder.Task;
			});
			return "The map list was printed to the debug console";
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CF8 File Offset: 0x00000EF8
		[CommandLineFunctionality.CommandLineArgumentFunction("open_download_panel", "dcshelper")]
		public static string OpenDownloadPanel(List<string> strings)
		{
			DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass15_0 CS$<>8__locals1 = new DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass15_0();
			string text = "Usage: dcshelper.open_download_panel [host_address[:port]]";
			if (strings.Count != 1)
			{
				return text;
			}
			CS$<>8__locals1.hostAddress = strings[0];
			if (!(ScreenManager.TopScreen is MultiplayerLobbyGauntletScreen))
			{
				return "The download panel can only be opened while on the multiplayer lobby.";
			}
			Task.Run(delegate
			{
				DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass15_0.<<OpenDownloadPanel>b__0>d <<OpenDownloadPanel>b__0>d;
				<<OpenDownloadPanel>b__0>d.<>4__this = CS$<>8__locals1;
				<<OpenDownloadPanel>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OpenDownloadPanel>b__0>d.<>1__state = -1;
				AsyncTaskMethodBuilder <>t__builder = <<OpenDownloadPanel>b__0>d.<>t__builder;
				<>t__builder.Start<DedicatedCustomServerClientHelperSubModule.<>c__DisplayClass15_0.<<OpenDownloadPanel>b__0>d>(ref <<OpenDownloadPanel>b__0>d);
				return <<OpenDownloadPanel>b__0>d.<>t__builder.Task;
			});
			return "Opening download panel for host '" + CS$<>8__locals1.hostAddress + "'...";
		}

		// Token: 0x0400001C RID: 28
		public const string ModuleName = "Multiplayer";

		// Token: 0x0400001D RID: 29
		public static readonly bool DebugMode;

		// Token: 0x0400001E RID: 30
		public static DedicatedCustomServerClientHelperSubModule Instance;

		// Token: 0x0400001F RID: 31
		private readonly HttpClient _httpClient;

		// Token: 0x04000020 RID: 32
		private const string CommandGroup = "dcshelper";

		// Token: 0x04000021 RID: 33
		private const string DownloadMapCommandName = "download_map";

		// Token: 0x04000022 RID: 34
		private const string GetMapListCommandName = "get_map_list";

		// Token: 0x04000023 RID: 35
		private const string OpenDownloadPanelCommandName = "open_download_panel";

		// Token: 0x02000010 RID: 16
		private class LobbyStateListener : IGameStateListener
		{
			// Token: 0x06000093 RID: 147 RVA: 0x00003AF2 File Offset: 0x00001CF2
			public LobbyStateListener(LobbyState lobbyState)
			{
				this._lobbyState = lobbyState;
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003B01 File Offset: 0x00001D01
			private bool ServerSupportsDownloadPanel(GameServerEntry serverEntry)
			{
				return true;
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00003B04 File Offset: 0x00001D04
			private void OpenDownloadPanelForServer(GameServerEntry serverEntry)
			{
				DedicatedCustomServerClientHelperSubModule.LobbyStateListener.<>c__DisplayClass3_0 CS$<>8__locals1 = new DedicatedCustomServerClientHelperSubModule.LobbyStateListener.<>c__DisplayClass3_0();
				CS$<>8__locals1.serverEntry = serverEntry;
				Task.Run(delegate
				{
					DedicatedCustomServerClientHelperSubModule.LobbyStateListener.<>c__DisplayClass3_0.<<OpenDownloadPanelForServer>b__0>d <<OpenDownloadPanelForServer>b__0>d;
					<<OpenDownloadPanelForServer>b__0>d.<>4__this = CS$<>8__locals1;
					<<OpenDownloadPanelForServer>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<OpenDownloadPanelForServer>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<OpenDownloadPanelForServer>b__0>d.<>t__builder;
					<>t__builder.Start<DedicatedCustomServerClientHelperSubModule.LobbyStateListener.<>c__DisplayClass3_0.<<OpenDownloadPanelForServer>b__0>d>(ref <<OpenDownloadPanelForServer>b__0>d);
					return <<OpenDownloadPanelForServer>b__0>d.<>t__builder.Task;
				});
			}

			// Token: 0x06000096 RID: 150 RVA: 0x00003B24 File Offset: 0x00001D24
			private List<CustomServerAction> ActionSupplier(GameServerEntry serverEntry)
			{
				List<CustomServerAction> list = new List<CustomServerAction>();
				if (this.ServerSupportsDownloadPanel(serverEntry))
				{
					CustomServerAction customServerAction = new CustomServerAction(delegate
					{
						this.OpenDownloadPanelForServer(serverEntry);
					}, serverEntry, new TextObject("{=ebuelCXT}Open Download Panel", null).ToString());
					list.Add(customServerAction);
				}
				return list;
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00003B89 File Offset: 0x00001D89
			private void HandleFailedServerJoinAttempt(GameServerEntry serverEntry)
			{
				if (this.ServerSupportsDownloadPanel(serverEntry))
				{
					this.OpenDownloadPanelForServer(serverEntry);
				}
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00003B9B File Offset: 0x00001D9B
			public void OnActivate()
			{
				this._lobbyState.RegisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.ActionSupplier));
				this._lobbyState.ClientRefusedToJoinCustomServer += this.HandleFailedServerJoinAttempt;
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00003BCB File Offset: 0x00001DCB
			public void OnDeactivate()
			{
				this._lobbyState.UnregisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.ActionSupplier));
				this._lobbyState.ClientRefusedToJoinCustomServer -= this.HandleFailedServerJoinAttempt;
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00003BFB File Offset: 0x00001DFB
			public void OnFinalize()
			{
				this._lobbyState = null;
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00003C04 File Offset: 0x00001E04
			public void OnInitialize()
			{
			}

			// Token: 0x0400004D RID: 77
			private LobbyState _lobbyState;
		}

		// Token: 0x02000011 RID: 17
		private class StateManagerListener : IGameStateManagerListener
		{
			// Token: 0x0600009C RID: 156 RVA: 0x00003C08 File Offset: 0x00001E08
			public void OnCreateState(GameState gameState)
			{
				LobbyState lobbyState;
				if ((lobbyState = gameState as LobbyState) != null)
				{
					lobbyState.RegisterListener(new DedicatedCustomServerClientHelperSubModule.LobbyStateListener(lobbyState));
				}
			}

			// Token: 0x0600009D RID: 157 RVA: 0x00003C2C File Offset: 0x00001E2C
			public void OnPopState(GameState gameState)
			{
			}

			// Token: 0x0600009E RID: 158 RVA: 0x00003C2E File Offset: 0x00001E2E
			public void OnPushState(GameState gameState, bool isTopGameState)
			{
			}

			// Token: 0x0600009F RID: 159 RVA: 0x00003C30 File Offset: 0x00001E30
			public void OnCleanStates()
			{
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x00003C32 File Offset: 0x00001E32
			public void OnSavedGameLoadFinished()
			{
			}
		}
	}
}
