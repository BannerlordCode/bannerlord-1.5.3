using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.AchievementSystem;
using TaleWorlds.ActivitySystem;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.SaveSystem;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DE RID: 734
	public sealed class Module : DotNetObject, IGameStateManagerOwner
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x000A2DD6 File Offset: 0x000A0FD6
		// (set) Token: 0x06002AB1 RID: 10929 RVA: 0x000A2DDD File Offset: 0x000A0FDD
		public static Module CurrentModule { get; private set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000A2DE5 File Offset: 0x000A0FE5
		// (set) Token: 0x06002AB3 RID: 10931 RVA: 0x000A2DED File Offset: 0x000A0FED
		public GameStateManager GlobalGameStateManager { get; private set; }

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000A2DF6 File Offset: 0x000A0FF6
		public bool MultiplayerRequested
		{
			get
			{
				return this.StartupInfo.StartupType == GameStartupType.Multiplayer || PlatformServices.SessionInvitationType == SessionInvitationType.Multiplayer || PlatformServices.IsPlatformRequestedMultiplayer;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000A2E15 File Offset: 0x000A1015
		// (set) Token: 0x06002AB6 RID: 10934 RVA: 0x000A2E1D File Offset: 0x000A101D
		public bool ReturnToEditorState { get; private set; }

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000A2E26 File Offset: 0x000A1026
		// (set) Token: 0x06002AB8 RID: 10936 RVA: 0x000A2E2E File Offset: 0x000A102E
		public bool LoadingFinished { get; private set; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x000A2E37 File Offset: 0x000A1037
		// (set) Token: 0x06002ABA RID: 10938 RVA: 0x000A2E3F File Offset: 0x000A103F
		public GameTextManager GlobalTextManager { get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000A2E48 File Offset: 0x000A1048
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x000A2E50 File Offset: 0x000A1050
		public bool IsOnlyCoreContentEnabled { get; private set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000A2E59 File Offset: 0x000A1059
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x000A2E61 File Offset: 0x000A1061
		public JobManager JobManager { get; private set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000A2E6A File Offset: 0x000A106A
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x000A2E72 File Offset: 0x000A1072
		public GameStartupInfo StartupInfo { get; private set; }

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000A2E7C File Offset: 0x000A107C
		private Module()
		{
			MBDebug.Print("Creating module...", 0, Debug.DebugColor.White, 17592186044416UL);
			this.StartupInfo = new GameStartupInfo();
			this._testContext = new TestContext();
			this._subModuleBases = new Dictionary<SubModuleInfo, MBSubModuleBase>();
			this.GlobalGameStateManager = new GameStateManager(this, GameStateManager.GameStateManagerType.Global);
			GameStateManager.Current = this.GlobalGameStateManager;
			this.GlobalTextManager = new GameTextManager();
			this.JobManager = new JobManager();
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000A2F00 File Offset: 0x000A1100
		public MBReadOnlyList<MBSubModuleBase> CollectSubModules()
		{
			MBList<MBSubModuleBase> mblist = new MBList<MBSubModuleBase>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetAllModules())
			{
				if (moduleInfo.IsActive)
				{
					foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
					{
						MBSubModuleBase subModuleBase = this.GetSubModuleBase(subModuleInfo);
						if (subModuleBase != null)
						{
							mblist.Add(subModuleBase);
						}
					}
				}
			}
			return mblist;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000A2FAC File Offset: 0x000A11AC
		internal static void CreateModule()
		{
			Module.CurrentModule = new Module();
			Utilities.SetLoadingScreenPercentage(0.4f);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000A2FC4 File Offset: 0x000A11C4
		private AssemblyLoader.AssemblyLoadResult AddSubModule(SubModuleInfo subModuleInfo, Assembly subModuleAssembly)
		{
			ConstructorInfo constructor = subModuleAssembly.GetType(subModuleInfo.SubModuleClassTypeName).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new Type[0], null);
			Dictionary<string, Type> dictionary;
			AssemblyLoader.AssemblyLoadResult assemblyLoadResult = this.CollectModuleAssemblyTypes(subModuleInfo, subModuleAssembly, out dictionary);
			if (assemblyLoadResult == AssemblyLoader.AssemblyLoadResult.Success)
			{
				Managed.AddTypes(dictionary);
				MBSubModuleBase mbsubModuleBase = (MBSubModuleBase)constructor.Invoke(new object[0]);
				this._subModuleBases.Add(subModuleInfo, mbsubModuleBase);
			}
			return assemblyLoadResult;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000A3024 File Offset: 0x000A1224
		private AssemblyLoader.AssemblyLoadResult CollectModuleAssemblyTypes(SubModuleInfo subModule, Assembly moduleAssembly, out Dictionary<string, Type> types)
		{
			AssemblyLoader.AssemblyLoadResult assemblyLoadResult;
			try
			{
				types = new Dictionary<string, Type>();
				foreach (Type type in moduleAssembly.GetTypes())
				{
					if (typeof(ManagedObject).IsAssignableFrom(type) || typeof(DotNetObject).IsAssignableFrom(type))
					{
						types.Add(type.Name, type);
					}
				}
				assemblyLoadResult = AssemblyLoader.AssemblyLoadResult.Success;
			}
			catch (Exception ex)
			{
				MBDebug.Print("Error while getting types and loading" + ex.Message + "\nException: " + ex.GetType().Name, 0, Debug.DebugColor.White, 17592186044416UL);
				ReflectionTypeLoadException ex2;
				if ((ex2 = ex as ReflectionTypeLoadException) != null)
				{
					string text = "";
					foreach (Exception ex3 in ex2.LoaderExceptions)
					{
						MBDebug.Print("Loader Exceptions: " + ex3.Message, 0, Debug.DebugColor.White, 17592186044416UL);
						text = text + ex3.Message + Environment.NewLine;
					}
					Debug.SetCrashReportCustomString(text);
					foreach (Type type2 in ex2.Types)
					{
						if (type2 != null)
						{
							MBDebug.Print("Loaded Types: " + type2.FullName, 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
				}
				if (ex.InnerException != null)
				{
					MBDebug.Print("Inner excetion: " + ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				types = null;
				assemblyLoadResult = AssemblyLoader.AssemblyLoadResult.CriticalError;
			}
			return assemblyLoadResult;
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000A31C8 File Offset: 0x000A13C8
		private void InitializeSubModuleBases()
		{
			Managed.AddConstructorDelegateOfClass<SpawnedItemEntity>();
			foreach (KeyValuePair<SubModuleInfo, MBSubModuleBase> keyValuePair in this._subModuleBases)
			{
				try
				{
					keyValuePair.Value.OnSubModuleLoad();
				}
				catch (Exception ex)
				{
					string text = ((keyValuePair.Key != null) ? (keyValuePair.Key.Name + " (" + keyValuePair.Key.DLLName + ")") : "<unknown submodule>");
					string text2 = string.Concat(new string[]
					{
						"OnSubModuleLoad failed for ",
						text,
						": ",
						ex.GetType().Name,
						": ",
						ex.Message
					});
					if (ex.InnerException != null)
					{
						text2 = string.Concat(new string[]
						{
							text2,
							"\nInner: ",
							ex.InnerException.GetType().Name,
							": ",
							ex.InnerException.Message
						});
					}
					MBDebug.Print(text2, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.SetCrashReportCustomString(text2);
					throw new Exception();
				}
			}
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000A3338 File Offset: 0x000A1538
		private void OnNewModuleLoaded()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnNewModuleLoad();
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000A3390 File Offset: 0x000A1590
		private MBSubModuleBase GetSubModuleBase(SubModuleInfo subModuleInfo)
		{
			MBSubModuleBase mbsubModuleBase;
			if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
			{
				return mbsubModuleBase;
			}
			return null;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000A33B0 File Offset: 0x000A15B0
		private void FinalizeSubModulesBases()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnSubModuleUnloaded();
			}
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000A3408 File Offset: 0x000A1608
		[MBCallback(null, false)]
		internal void LoadSingleModule(string modulePath)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			list.Add(ModuleHelper.InitializeSingleModule(modulePath));
			LocalizedTextManager.AddLocalizationXml(modulePath);
			this.LoadSubModules(list, true);
			BannerManager.ResetAndLoad();
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000A343C File Offset: 0x000A163C
		[MBCallback(null, false)]
		internal void Initialize()
		{
			MBDebug.Print("Module Initialize begin...", 0, Debug.DebugColor.White, 17592186044416UL);
			TWParallel.InitializeAndSetImplementation(new NativeParallelDriver());
			MBSaveLoad.SetSaveDriver(new AsyncFileSaveDriver());
			this.ProcessApplicationArguments();
			this.SetWindowTitle();
			this._initialStateOptions = new List<InitialStateOption>();
			this.FillMultiplayerGameTypes();
			if (!GameNetwork.IsDedicatedServer && !MBDebug.TestModeEnabled)
			{
				MBDebug.Print("Loading platform services...", 0, Debug.DebugColor.White, 17592186044416UL);
				this.LoadPlatformServices();
			}
			string[] array = null;
			ModuleHelper.InitializeModules(Utilities.GetModulesNames(), array);
			this.LoadLocalizationXmls();
			this.GlobalTextManager.LoadDefaultTexts();
			this.IsOnlyCoreContentEnabled = Utilities.IsOnlyCoreContentEnabled();
			NativeConfig.OnConfigChanged();
			List<ModuleInfo> modules = ModuleHelper.GetModules(null);
			this.LoadSubModules(modules, false);
			MBDebug.Print("Adding trace listener...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print("MBModuleBase Initialize begin...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print("MBModuleBase Initialize end...", 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.FindGameNetworkMessages();
			GameNetwork.FindSynchedMissionObjectTypes();
			HasTableauCache.CollectTableauCacheTypes();
			MBDebug.Print("Module Initialize end...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.TestModeEnabled = Utilities.CommandLineArgumentExists("/runTest");
			this.FindMissions();
			NativeOptions.ReadRGLConfigFiles();
			BannerlordConfig.Initialize();
			EngineController.ConfigChange += this.OnConfigChanged;
			EngineController.OnConstrainedStateChanged += this.OnConstrainedStateChange;
			ScreenManager.FocusGained += this.OnFocusGained;
			ScreenManager.PlatformTextRequested += this.OnPlatformTextRequested;
			PlatformServices.Instance.OnTextEnteredFromPlatform += this.OnTextEnteredFromPlatform;
			PlatformServices.Instance.OnTextCanceledFromPlatform += this.OnTextCanceledFromPlatform;
			SaveManager.InitializeGlobalDefinitionContext();
			this.EnsureAsyncJobsAreFinished();
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000A35F8 File Offset: 0x000A17F8
		private bool OnPlatformTextRequested(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			IPlatformServices instance = PlatformServices.Instance;
			return instance != null && instance.ShowGamepadTextInput(descriptionText, initialText, (uint)maxLength, keyboardTypeEnum == 2);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000A3614 File Offset: 0x000A1814
		private void LoadLocalizationXmls()
		{
			List<string> list = new List<string>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
			{
				list.Add(moduleInfo.FolderPath);
			}
			LocalizedTextManager.LoadLocalizationXmls(list.ToArray());
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000A3680 File Offset: 0x000A1880
		private void SetWindowTitle()
		{
			string applicationName = Utilities.GetApplicationName();
			string text;
			if (this.StartupInfo.StartupType == GameStartupType.Singleplayer)
			{
				text = applicationName + " - Singleplayer";
			}
			else if (this.StartupInfo.StartupType == GameStartupType.Multiplayer)
			{
				text = applicationName + " - Multiplayer";
			}
			else if (this.StartupInfo.StartupType == GameStartupType.GameServer)
			{
				text = string.Concat(new object[]
				{
					"[",
					Utilities.GetCurrentProcessID(),
					"] ",
					applicationName,
					" Dedicated Server Port:",
					this.StartupInfo.ServerPort
				});
			}
			else
			{
				text = applicationName;
			}
			text = Utilities.ProcessWindowTitle(text);
			Utilities.SetWindowTitle(text);
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000A3732 File Offset: 0x000A1932
		private void EnsureAsyncJobsAreFinished()
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				while (!MBMusicManager.IsCreationCompleted())
				{
					Thread.Sleep(1);
				}
			}
			if (!GameNetwork.IsDedicatedServer && !MBDebug.TestModeEnabled)
			{
				while (!AchievementManager.AchievementService.IsInitializationCompleted())
				{
					Thread.Sleep(1);
				}
			}
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000A376C File Offset: 0x000A196C
		private void ProcessApplicationArguments()
		{
			this.StartupInfo.StartupType = GameStartupType.None;
			string[] array = Utilities.GetFullCommandLineString().Split(new char[] { ' ' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToLowerInvariant();
				if (text == "/dedicatedmatchmakingserver".ToLower())
				{
					int num = Convert.ToInt32(array[i + 1]);
					string text2 = array[i + 2];
					sbyte b = Convert.ToSByte(array[i + 3]);
					string text3 = array[i + 4];
					i += 4;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Matchmaker;
					this.StartupInfo.ServerPort = num;
					this.StartupInfo.ServerRegion = text2;
					this.StartupInfo.ServerPriority = b;
					this.StartupInfo.ServerGameMode = text3.Trim();
				}
				else if (text == "/dedicatedcustomserver".ToLower())
				{
					int num2 = Convert.ToInt32(array[i + 1]);
					string text4 = array[i + 2];
					int num3 = Convert.ToInt32(array[i + 3]);
					i += 3;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Custom;
					this.StartupInfo.ServerPort = num2;
					this.StartupInfo.ServerRegion = text4;
					this.StartupInfo.Permission = num3;
				}
				else if (text == "/dedicatedcommunityserver".ToLower())
				{
					int num4 = Convert.ToInt32(array[i + 1]);
					i++;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Community;
					this.StartupInfo.ServerPort = num4;
				}
				else if (text == "/dedicatedcustomserverconfigfile".ToLower())
				{
					string text5 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerConfigFile = text5;
				}
				else if (text == "/dedicatedcustomservernameoverride".ToLower())
				{
					string text6 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerNameOverride = text6;
				}
				else if (text == "/dedicatedcustomserverpasswordoverride".ToLower())
				{
					string text7 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerPasswordOverride = text7;
				}
				else if (text == "/dedicatedcustomserverauthtoken".ToLower())
				{
					string text8 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerAuthToken = text8;
				}
				else if (text == "/dedicatedcustomserverDontAllowOptionalModules".ToLower())
				{
					this.StartupInfo.CustomGameServerAllowsOptionalModules = false;
				}
				else if (text == "/playerHostedDedicatedServer".ToLower())
				{
					this.StartupInfo.PlayerHostedDedicatedServer = true;
				}
				else if (text == "/singleplatform")
				{
					this.StartupInfo.IsSinglePlatformServer = true;
				}
				else if (text == "/customserverhost")
				{
					string text9 = array[i + 1];
					i++;
					this.StartupInfo.CustomServerHostIP = text9;
				}
				else if (text == "/singleplayer".ToLower())
				{
					this.StartupInfo.StartupType = GameStartupType.Singleplayer;
				}
				else if (text == "/multiplayer".ToLower())
				{
					this.StartupInfo.StartupType = GameStartupType.Multiplayer;
				}
				else if (text == "/clientConfigurationCategory".ToLower())
				{
					ClientApplicationConfiguration.SetDefaultConfigurationCategory(array[i + 1]);
					i++;
				}
				else if (text == "/overridenusername".ToLower())
				{
					string text10 = array[i + 1];
					this.StartupInfo.OverridenUserName = text10;
					i++;
				}
				else if (text.StartsWith("-PlatformInterface".ToLowerInvariant()))
				{
					this.StartupInfo.PlatformInterface = text.Split(new char[] { '=' })[1];
				}
				else if (text.StartsWith("-epicuserid".ToLowerInvariant()))
				{
					this.StartupInfo.EpicUserId = text.Split(new char[] { '=' })[1];
				}
				else if (text.StartsWith("-epicusername".ToLowerInvariant()))
				{
					this.StartupInfo.EpicUserName = text.Split(new char[] { '=' })[1];
				}
				else if (text == "/continuegame".ToLower())
				{
					this.StartupInfo.IsContinueGame = true;
				}
				else if (text == "/serverbandwidthlimitmbps".ToLower())
				{
					double num5 = Convert.ToDouble(array[i + 1]);
					this.StartupInfo.ServerBandwidthLimitInMbps = num5;
					i++;
				}
				else if (text == "/tickrate".ToLower())
				{
					int num6 = Convert.ToInt32(array[i + 1]);
					this.StartupInfo.ServerTickRate = num6;
					i++;
				}
			}
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000A3C10 File Offset: 0x000A1E10
		internal void OnApplicationTick(float dt)
		{
			bool isOnlyCoreContentEnabled = this.IsOnlyCoreContentEnabled;
			this.IsOnlyCoreContentEnabled = Utilities.IsOnlyCoreContentEnabled();
			if (isOnlyCoreContentEnabled != this.IsOnlyCoreContentEnabled && isOnlyCoreContentEnabled)
			{
				InitialState initialState;
				if ((initialState = GameStateManager.Current.ActiveState as InitialState) != null)
				{
					Utilities.DisableCoreGame();
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=CaSafuAH}Content Download Complete", null).ToString(), new TextObject("{=1nKa4pQX}Rest of the game content has been downloaded.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
					{
						initialState.RefreshContentState();
					}, null, "", 0f, null, null, null), false, false);
				}
				else
				{
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=CaSafuAH}Content Download Complete", null).ToString(), new TextObject("{=BFhMw4bl}Rest of the game content has been downloaded. Do you want to return to the main menu?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.OnConfirmReturnToMainMenu), null, "", 0f, null, null, null), false, false);
					this._enableCoreContentOnReturnToRoot = true;
				}
			}
			if (this._synchronizationContext == null)
			{
				this._synchronizationContext = new SingleThreadedSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(this._synchronizationContext);
			}
			this._testContext.OnApplicationTick(dt);
			if (!GameNetwork.MultiplayerDisabled)
			{
				this.OnNetworkTick(dt);
			}
			if (GameStateManager.Current == null)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			if (GameStateManager.Current == this.GlobalGameStateManager)
			{
				if (this.LoadingFinished && this.GlobalGameStateManager.ActiveState == null)
				{
					if (this.ReturnToEditorState)
					{
						this.ReturnToEditorState = false;
						this.SetEditorScreenAsRootScreen();
					}
					else
					{
						this.SetInitialModuleScreenAsRootScreen();
					}
				}
				this.GlobalGameStateManager.OnTick(dt);
			}
			Utilities.RunJobs();
			IPlatformServices instance = PlatformServices.Instance;
			if (instance != null)
			{
				instance.Tick(dt);
			}
			this._synchronizationContext.Tick();
			if (GameManagerBase.Current != null)
			{
				GameManagerBase.Current.OnTick(dt);
			}
			foreach (MBSubModuleBase mbsubModuleBase in this.CollectSubModules())
			{
				mbsubModuleBase.OnApplicationTick(dt);
			}
			this.JobManager.OnTick(dt);
			AvatarServices.UpdateAvatarServices(dt);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000A3E50 File Offset: 0x000A2050
		private void OnConfirmReturnToMainMenu()
		{
			MBGameManager.EndGame();
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000A3E58 File Offset: 0x000A2058
		private void OnNetworkTick(float dt)
		{
			foreach (MBSubModuleBase mbsubModuleBase in this.CollectSubModules())
			{
				mbsubModuleBase.OnNetworkTick(dt);
			}
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000A3EAC File Offset: 0x000A20AC
		[MBCallback(null, false)]
		internal void RunTest(string commandLine)
		{
			MBDebug.Print(" TEST MODE ENABLED. Command line string: " + commandLine, 0, Debug.DebugColor.White, 17592186044416UL);
			this._testContext.RunTestAux(commandLine);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000A3ED6 File Offset: 0x000A20D6
		[MBCallback(null, true)]
		internal void TickTest(float dt)
		{
			this._testContext.TickTest(dt);
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000A3EE4 File Offset: 0x000A20E4
		[MBCallback(null, false)]
		internal void OnDumpCreated()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.ToggleTimeoutTimer();
				TestCommonBase.BaseInstance.StartTimeoutTimer();
			}
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000A3F01 File Offset: 0x000A2101
		[MBCallback(null, false)]
		internal void OnDumpCreationStarted()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.ToggleTimeoutTimer();
			}
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000A3F14 File Offset: 0x000A2114
		public static void GetMetaMeshPackageMapping(Dictionary<string, string> metaMeshPackageMappings)
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.HasArmorComponent)
				{
					string text = ((itemObject.Culture != null) ? itemObject.Culture.StringId : "shared") + "_armor";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_converted"] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_converted_slim"] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_slim"] = text;
				}
				if (itemObject.WeaponComponent != null)
				{
					string text2 = ((itemObject.Culture != null) ? itemObject.Culture.StringId : "shared") + "_weapon";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text2;
					if (itemObject.HolsterMeshName != null)
					{
						metaMeshPackageMappings[itemObject.HolsterMeshName] = text2;
					}
					if (itemObject.HolsterWithWeaponMeshName != null)
					{
						metaMeshPackageMappings[itemObject.HolsterWithWeaponMeshName] = text2;
					}
				}
				if (itemObject.HasHorseComponent)
				{
					string text3 = "horses";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text3;
				}
				if (itemObject.IsFood)
				{
					string text4 = "food";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text4;
				}
			}
			foreach (CraftingPiece craftingPiece in Game.Current.ObjectManager.GetObjectTypeList<CraftingPiece>())
			{
				string text5 = ((craftingPiece.Culture != null) ? craftingPiece.Culture.StringId : "shared") + "_crafting";
				metaMeshPackageMappings[craftingPiece.MeshName] = text5;
			}
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000A4124 File Offset: 0x000A2324
		public static void GetItemMeshNames(HashSet<string> itemMeshNames)
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (!itemObject.IsCraftedWeapon)
				{
					itemMeshNames.Add(itemObject.MultiMeshName);
				}
				if (itemObject.PrimaryWeapon != null)
				{
					if (itemObject.FlyingMeshName != null && !itemObject.FlyingMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.FlyingMeshName);
					}
					if (itemObject.HolsterMeshName != null && !itemObject.HolsterMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.HolsterMeshName);
					}
					if (itemObject.HolsterWithWeaponMeshName != null && !itemObject.HolsterWithWeaponMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.HolsterWithWeaponMeshName);
					}
				}
				if (itemObject.HasHorseComponent)
				{
					foreach (KeyValuePair<string, bool> keyValuePair in itemObject.HorseComponent.AdditionalMeshesNameList)
					{
						if (keyValuePair.Key != null && !keyValuePair.Key.IsEmpty<char>())
						{
							itemMeshNames.Add(keyValuePair.Key);
						}
					}
				}
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000A4270 File Offset: 0x000A2470
		[MBCallback(null, false)]
		internal string GetMetaMeshPackageMapping()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Module.GetMetaMeshPackageMapping(dictionary);
			string text = "";
			foreach (string text2 in dictionary.Keys)
			{
				text = string.Concat(new string[]
				{
					text,
					text2,
					"|",
					dictionary[text2],
					","
				});
			}
			return text;
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000A4300 File Offset: 0x000A2500
		[MBCallback(null, false)]
		internal string GetItemMeshNames()
		{
			HashSet<string> hashSet = new HashSet<string>();
			Module.GetItemMeshNames(hashSet);
			foreach (CraftingPiece craftingPiece in MBObjectManager.Instance.GetObjectTypeList<CraftingPiece>())
			{
				hashSet.Add(craftingPiece.MeshName);
				if (craftingPiece.BladeData != null)
				{
					hashSet.Add(craftingPiece.BladeData.HolsterMeshName);
				}
			}
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AllIcons)
				{
					if (keyValuePair.Value.MaterialName != "")
					{
						hashSet.Add(keyValuePair.Value.MaterialName + keyValuePair.Value.TextureIndex);
					}
				}
			}
			string text = "";
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000A44AC File Offset: 0x000A26AC
		[CommandLineFunctionality.CommandLineArgumentFunction("get_item_mesh_names", "module")]
		public static string GetCraftedItemMeshNames(List<string> arguments)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (CraftingPiece craftingPiece in MBObjectManager.Instance.GetObjectTypeList<CraftingPiece>())
			{
				hashSet.Add(craftingPiece.MeshName);
				if (craftingPiece.BladeData != null)
				{
					hashSet.Add(craftingPiece.BladeData.HolsterMeshName);
				}
			}
			string text = "";
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000A4584 File Offset: 0x000A2784
		[MBCallback(null, false)]
		internal string GetHorseMaterialNames()
		{
			HashSet<string> hashSet = new HashSet<string>();
			string text = "";
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.HasHorseComponent && itemObject.HorseComponent.HorseMaterialNames != null && itemObject.HorseComponent.HorseMaterialNames.Count > 0)
				{
					foreach (HorseComponent.MaterialProperty materialProperty in itemObject.HorseComponent.HorseMaterialNames)
					{
						hashSet.Add(materialProperty.Name);
					}
				}
			}
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000A46B4 File Offset: 0x000A28B4
		public void SetInitialModuleScreenAsRootScreen()
		{
			if (GameStateManager.Current != this.GlobalGameStateManager)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnBeforeInitialModuleScreenSetAsRoot();
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				string text = ModuleHelper.GetModuleFullPath("Native") + "Videos/TWLogo_and_Partners.ivf";
				string text2 = ModuleHelper.GetModuleFullPath("Native") + "Videos/TWLogo_and_Partners.ogg";
				if (!this._splashScreenPlayed && File.Exists(text) && (text2 == "" || File.Exists(text2)) && !Debugger.IsAttached)
				{
					VideoPlaybackState videoPlaybackState = this.GlobalGameStateManager.CreateState<VideoPlaybackState>();
					videoPlaybackState.SetStartingParameters(text, text2, string.Empty, 30f, true);
					videoPlaybackState.SetOnVideoFinisedDelegate(delegate
					{
						this.OnInitialModuleScreenActivated(true);
					});
					this.GlobalGameStateManager.CleanAndPushState(videoPlaybackState, 0);
					this._splashScreenPlayed = true;
					return;
				}
				this.OnInitialModuleScreenActivated(false);
			}
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000A47D0 File Offset: 0x000A29D0
		private void OnInitialModuleScreenActivated(bool isFromSplashScreenVideo)
		{
			Utilities.EnableGlobalLoadingWindow();
			LoadingWindow.EnableGlobalLoadingWindow();
			if (!this.StartupInfo.IsContinueGame)
			{
				this.StartupInfo.IsContinueGame = PlatformServices.IsPlatformRequestedContinueGame && !this.IsOnlyCoreContentEnabled;
			}
			if (this._enableCoreContentOnReturnToRoot)
			{
				Utilities.DisableCoreGame();
				this._enableCoreContentOnReturnToRoot = false;
			}
			if (this.IsOnlyCoreContentEnabled && PlatformServices.SessionInvitationType == SessionInvitationType.Multiplayer)
			{
				PlatformServices.OnSessionInvitationHandled();
			}
			if (this.IsOnlyCoreContentEnabled && PlatformServices.IsPlatformRequestedMultiplayer)
			{
				PlatformServices.OnPlatformMultiplayerRequestHandled();
			}
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules((ModuleInfo x) => !x.IsActive))
			{
				this.ActivateModule(moduleInfo.Id);
			}
			if (this.IsOnlyCoreContentEnabled || !this.MultiplayerRequested)
			{
				this.GlobalGameStateManager.CleanAndPushState(this.GlobalGameStateManager.CreateState<InitialState>(), 0);
			}
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnInitialState();
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000A4924 File Offset: 0x000A2B24
		private void OnSignInStateUpdated(bool isLoggedIn, TextObject message)
		{
			if (!isLoggedIn && !(this.GlobalGameStateManager.ActiveState is ProfileSelectionState))
			{
				this.GlobalGameStateManager.CleanAndPushState(this.GlobalGameStateManager.CreateState<ProfileSelectionState>(), 0);
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000A4954 File Offset: 0x000A2B54
		[MBCallback(null, false)]
		internal bool SetEditorScreenAsRootScreen()
		{
			if (GameStateManager.Current != this.GlobalGameStateManager)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			if (!(this.GlobalGameStateManager.ActiveState is EditorState))
			{
				this.GlobalGameStateManager.CleanAndPushState(GameStateManager.Current.CreateState<EditorState>(), 0);
				return true;
			}
			return false;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000A49A4 File Offset: 0x000A2BA4
		private bool CheckAssemblyForMissionMethods(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(MissionMethod));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
			for (int i = 0; i < referencedAssembliesSafe.Length; i++)
			{
				if (referencedAssembliesSafe[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000A49FC File Offset: 0x000A2BFC
		private void FindMissions()
		{
			MBDebug.Print("Searching Mission Methods", 0, Debug.DebugColor.White, 17592186044416UL);
			this._missionInfos = new List<MissionInfo>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (this.CheckAssemblyForMissionMethods(assembly))
				{
					foreach (Type type in assembly.GetTypesSafe(null))
					{
						object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(MissionManager), true);
						if (customAttributesSafe != null && customAttributesSafe.Length != 0)
						{
							list.Add(type);
						}
					}
				}
			}
			MBDebug.Print("Found " + list.Count + " mission managers", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (Type type2 in list)
			{
				foreach (MethodInfo methodInfo in type2.GetMethods(BindingFlags.Static | BindingFlags.Public))
				{
					object[] customAttributesSafe2 = methodInfo.GetCustomAttributesSafe(typeof(MissionMethod), true);
					if (customAttributesSafe2 != null && customAttributesSafe2.Length != 0)
					{
						MissionMethod missionMethod = customAttributesSafe2[0] as MissionMethod;
						MissionInfo missionInfo = new MissionInfo();
						missionInfo.Creator = methodInfo;
						missionInfo.Manager = type2;
						missionInfo.UsableByEditor = missionMethod.UsableByEditor;
						missionInfo.Name = methodInfo.Name;
						if (missionInfo.Name.StartsWith("Open"))
						{
							missionInfo.Name = missionInfo.Name.Substring(4);
						}
						if (missionInfo.Name.EndsWith("Mission"))
						{
							missionInfo.Name = missionInfo.Name.Substring(0, missionInfo.Name.Length - 7);
						}
						MissionInfo missionInfo2 = missionInfo;
						missionInfo2.Name = missionInfo2.Name + "[" + type2.Name + "]";
						this._missionInfos.Add(missionInfo);
					}
				}
			}
			MBDebug.Print("Found " + this._missionInfos.Count + " missions", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000A4C84 File Offset: 0x000A2E84
		[MBCallback(null, false)]
		internal string GetMissionControllerClassNames()
		{
			string text = "";
			for (int i = 0; i < this._missionInfos.Count; i++)
			{
				MissionInfo missionInfo = this._missionInfos[i];
				if (missionInfo.UsableByEditor)
				{
					text += missionInfo.Name;
					if (i + 1 != this._missionInfos.Count)
					{
						text += " ";
					}
				}
			}
			return text;
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000A4CEC File Offset: 0x000A2EEC
		private void LoadPlatformServices()
		{
			IPlatformServices platformServices = null;
			Assembly assembly = null;
			string fullModulePath = EngineApplicationInterface.IUtil.GetFullModulePath("Native");
			PlatformInitParams platformInitParams = new PlatformInitParams();
			if (ApplicationPlatform.CurrentPlatform == Platform.WindowsSteam)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.Steam.dll", true);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsEpic)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.Epic.dll", true);
				platformInitParams.Add("PlatformInterface", this.StartupInfo.PlatformInterface);
				platformInitParams.Add("EpicUserId", this.StartupInfo.EpicUserId);
				platformInitParams.Add("EpicUserName", this.StartupInfo.EpicUserName);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsGOG)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.GOG.dll", true);
				platformInitParams.Add("AchievementDataXmlPath", Path.Combine(fullModulePath, "ModuleData", "AchievementData", "gog_achievement_data.xml"));
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop || ApplicationPlatform.CurrentPlatform == Platform.Durango)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.GDK.dll", true);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.PS.dll", true);
				platformInitParams.Add("AchievementDataXmlPath", Path.Combine(fullModulePath, "ModuleData", "AchievementData", "ps_achievement_data.xml"));
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsNoPlatform)
			{
				string text = "TestUser" + DateTime.Now.Ticks % 10000L;
				if (!string.IsNullOrEmpty(this.StartupInfo.OverridenUserName))
				{
					text = this.StartupInfo.OverridenUserName;
				}
				platformServices = new TestPlatformServices(text);
			}
			if (assembly != null)
			{
				List<Type> typesSafe = assembly.GetTypesSafe(null);
				Type type = null;
				foreach (Type type2 in typesSafe)
				{
					if (type2.GetInterfaces().Contains(typeof(IPlatformServices)))
					{
						type = type2;
						break;
					}
				}
				platformServices = (IPlatformServices)type.GetConstructor(new Type[] { typeof(PlatformInitParams) }).Invoke(new object[] { platformInitParams });
			}
			if (platformServices != null)
			{
				PlatformServices.Setup(platformServices);
				PlatformServices.OnSessionInvitationAccepted = (Action<SessionInvitationType>)Delegate.Combine(PlatformServices.OnSessionInvitationAccepted, new Action<SessionInvitationType>(this.OnSessionInvitationAccepted));
				PlatformServices.OnPlatformRequestedMultiplayer = (Action)Delegate.Combine(PlatformServices.OnPlatformRequestedMultiplayer, new Action(this.OnPlatformRequestedMultiplayer));
				BannerlordFriendListService bannerlordFriendListService = new BannerlordFriendListService();
				ClanFriendListService clanFriendListService = new ClanFriendListService();
				RecentPlayersFriendListService recentPlayersFriendListService = new RecentPlayersFriendListService();
				PlatformServices.Initialize(new IFriendListService[] { bannerlordFriendListService, clanFriendListService, recentPlayersFriendListService });
				AchievementManager.AchievementService = platformServices.GetAchievementService();
				ActivityManager.ActivityService = platformServices.GetActivityService();
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000A4FD0 File Offset: 0x000A31D0
		private void OnSessionInvitationAccepted(SessionInvitationType targetGameType)
		{
			if (targetGameType == SessionInvitationType.Multiplayer)
			{
				if (this.IsOnlyCoreContentEnabled)
				{
					PlatformServices.OnSessionInvitationHandled();
					return;
				}
				this.JobManager.AddJob(new OnSessionInvitationAcceptedJob(targetGameType));
			}
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000A4FF5 File Offset: 0x000A31F5
		private void OnPlatformRequestedMultiplayer()
		{
			if (this.IsOnlyCoreContentEnabled)
			{
				PlatformServices.OnPlatformMultiplayerRequestHandled();
				return;
			}
			this.JobManager.AddJob(new OnPlatformRequestedMultiplayerJob());
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000A5018 File Offset: 0x000A3218
		private void LoadSubModules(List<ModuleInfo> modules, bool loadNewModules)
		{
			MBDebug.Print("Loading submodules...", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (ModuleInfo moduleInfo in modules)
			{
				try
				{
					XmlResource.GetMbprojxmls(moduleInfo.Id);
					XmlResource.GetXmlListAndApply(moduleInfo.Id);
				}
				catch (Exception ex)
				{
					string text = "XML Error in module '" + moduleInfo.Name + "'";
					string text2 = string.Concat(new string[] { "Failed to read XML definitions for module '", moduleInfo.Name, "' (ID: ", moduleInfo.Id, ").\n\n", ex.Message });
					if (!GameNetwork.IsDedicatedServer && !MBDebug.TestModeEnabled)
					{
						Debug.ShowMessageBox(text2, text, 256U);
					}
					throw;
				}
			}
			List<SubModuleInfo> list = new List<SubModuleInfo>();
			new List<ModuleInfo>();
			foreach (ModuleInfo moduleInfo2 in modules)
			{
				foreach (SubModuleInfo subModuleInfo in moduleInfo2.SubModules)
				{
					if (this.CheckIfSubmoduleCanBeLoadable(subModuleInfo) && !this._subModuleBases.ContainsKey(subModuleInfo))
					{
						string text3 = Path.Combine(moduleInfo2.FolderPath, "bin", Common.ConfigName);
						string text4 = Path.Combine(text3, subModuleInfo.DLLName);
						string text5 = ManagedDllFolder.Name + subModuleInfo.DLLName;
						MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> mblist = new MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>>();
						foreach (string text6 in subModuleInfo.Assemblies)
						{
							string text7 = Path.Combine(text3, text6);
							string text8 = ManagedDllFolder.Name + text6;
							AssemblyLoader.AssemblyLoadResult assemblyLoadResult;
							AssemblyLoader.LoadFrom(File.Exists(text7) ? text7 : text8, out assemblyLoadResult, true);
							if (assemblyLoadResult != AssemblyLoader.AssemblyLoadResult.Success)
							{
								mblist.Add(new ValueTuple<string, AssemblyLoader.AssemblyLoadResult>(text6, assemblyLoadResult));
							}
						}
						string text9 = (File.Exists(text4) ? text4 : (File.Exists(text5) ? text5 : string.Empty));
						AssemblyLoader.AssemblyLoadResult assemblyLoadResult2 = AssemblyLoader.AssemblyLoadResult.Success;
						if (!string.IsNullOrEmpty(text9))
						{
							Assembly assembly = AssemblyLoader.LoadFrom(text9, out assemblyLoadResult2, true);
							if (assemblyLoadResult2 != AssemblyLoader.AssemblyLoadResult.CriticalError)
							{
								assemblyLoadResult2 = this.AddSubModule(subModuleInfo, assembly);
								if (assemblyLoadResult2 == AssemblyLoader.AssemblyLoadResult.Success && loadNewModules)
								{
									list.Add(subModuleInfo);
								}
							}
							if (assemblyLoadResult2 != AssemblyLoader.AssemblyLoadResult.Success)
							{
								this.HandleSubmoduleLoadError(moduleInfo2, subModuleInfo, assemblyLoadResult2, mblist);
							}
							else if (mblist.Count > 0)
							{
								this.HandleSubmoduleLoadError(moduleInfo2, null, AssemblyLoader.AssemblyLoadResult.LoadedWithErrors, mblist);
							}
						}
						else
						{
							string text10 = "Cannot find: " + text4;
							string text11 = "Error";
							Debug.ShowMessageBox(text10, text11, 4U);
						}
					}
				}
			}
			if (loadNewModules)
			{
				foreach (SubModuleInfo subModuleInfo2 in list)
				{
					MBSubModuleBase mbsubModuleBase = null;
					if (this._subModuleBases.TryGetValue(subModuleInfo2, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleLoad();
					}
				}
				this.OnNewModuleLoaded();
				return;
			}
			this.InitializeSubModuleBases();
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000A53D4 File Offset: 0x000A35D4
		private void HandleSubmoduleLoadError(ModuleInfo module, SubModuleInfo subModule, AssemblyLoader.AssemblyLoadResult result, MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> assemblyLoadResults)
		{
			Debug.Print(module.Id + " could not be loaded correctly.", 0, Debug.DebugColor.White, 17592186044416UL);
			string text = "Error while loading " + module.Name;
			string assemblyLoadResultsMessage = this.GetAssemblyLoadResultsMessage(module, subModule, result, assemblyLoadResults);
			if (result == AssemblyLoader.AssemblyLoadResult.CriticalError)
			{
				Debug.Print(assemblyLoadResultsMessage, 0, Debug.DebugColor.White, 17592186044416UL);
				if (!module.IsOfficial)
				{
					Debug.ShowMessageBox(assemblyLoadResultsMessage, text, 256U);
				}
				throw new Exception();
			}
			if (result == AssemblyLoader.AssemblyLoadResult.LoadedWithErrors)
			{
				Debug.Print(assemblyLoadResultsMessage, 0, Debug.DebugColor.White, 17592186044416UL);
				if (!module.IsOfficial)
				{
					Debug.ShowMessageBox(assemblyLoadResultsMessage, text, 256U);
				}
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000A547C File Offset: 0x000A367C
		private string GetAssemblyLoadResultsMessage(ModuleInfo module, SubModuleInfo subModule, AssemblyLoader.AssemblyLoadResult result, MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> assemblyLoadResults)
		{
			string text = ((subModule != null) ? string.Concat(new string[] { "\"", module.Name, ".", subModule.Name, "\" submodule" }) : ("\"" + module.Name + "\" module"));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(text + " could not be loaded correctly due to a dependency conflict. This may cause the game to experience stability issues.");
			return stringBuilder.ToString();
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000A54F8 File Offset: 0x000A36F8
		public Type GetSubModuleType(string name)
		{
			foreach (KeyValuePair<SubModuleInfo, MBSubModuleBase> keyValuePair in this._subModuleBases)
			{
				if (keyValuePair.Key.SubModuleClassTypeName == name)
				{
					return keyValuePair.Value.GetType();
				}
			}
			return null;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000A556C File Offset: 0x000A376C
		public bool CheckIfSubmoduleCanBeLoadable(SubModuleInfo subModuleInfo)
		{
			if (subModuleInfo.Tags.Count > 0)
			{
				foreach (Tuple<SubModuleInfo.SubModuleTags, string> tuple in subModuleInfo.Tags)
				{
					if (!this.GetSubModuleValiditiy(tuple.Item1, tuple.Item2))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000A55E4 File Offset: 0x000A37E4
		private bool GetSubModuleValiditiy(SubModuleInfo.SubModuleTags tag, string value)
		{
			switch (tag)
			{
			case SubModuleInfo.SubModuleTags.RejectedPlatform:
			{
				Platform platform;
				if (Enum.TryParse<Platform>(value, out platform))
				{
					return ApplicationPlatform.CurrentPlatform != platform;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.ExclusivePlatform:
			{
				Platform platform;
				if (Enum.TryParse<Platform>(value, out platform))
				{
					return ApplicationPlatform.CurrentPlatform == platform;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.DedicatedServerType:
			{
				string text = value.ToLower();
				if (text == "none")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.None;
				}
				if (text == "both" || text == "all")
				{
					return this.StartupInfo.DedicatedServerType != DedicatedServerType.None;
				}
				if (text == "custom")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Custom;
				}
				if (text == "matchmaker")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Matchmaker;
				}
				if (text == "community")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Community;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.IsNoRenderModeElement:
				return value.Equals("false");
			case SubModuleInfo.SubModuleTags.DependantRuntimeLibrary:
			{
				Runtime runtime;
				if (Enum.TryParse<Runtime>(value, out runtime))
				{
					return ApplicationPlatform.CurrentRuntimeLibrary == runtime;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.PlayerHostedDedicatedServer:
			{
				string text2 = value.ToLower();
				if (this.StartupInfo.PlayerHostedDedicatedServer)
				{
					return text2.Equals("true");
				}
				return text2.Equals("false");
			}
			}
			return true;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000A573C File Offset: 0x000A393C
		[MBCallback(null, false)]
		internal static void MBThrowException()
		{
			Debug.FailedAssert("MBThrowException", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Module.cs", "MBThrowException", 1650);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000A5757 File Offset: 0x000A3957
		[MBCallback(null, false)]
		internal void OnEnterEditMode(bool isFirstTime)
		{
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x000A575B File Offset: 0x000A395B
		[MBCallback(null, false)]
		internal static Module GetInstance()
		{
			return Module.CurrentModule;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000A5762 File Offset: 0x000A3962
		[MBCallback(null, false)]
		internal static string GetGameStatus()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				return TestCommonBase.BaseInstance.GetGameStatus();
			}
			return "";
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000A577C File Offset: 0x000A397C
		private void FinalizeModule()
		{
			if (Game.Current != null)
			{
				Game.Current.OnFinalize();
			}
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.OnFinalize();
			}
			this._testContext.FinalizeContext();
			MBInformationManager.Clear();
			InformationManager.Clear();
			ScreenManager.OnFinalize();
			BannerlordConfig.Save();
			this.FinalizeSubModulesBases();
			IPlatformServices instance = PlatformServices.Instance;
			if (instance != null)
			{
				instance.Terminate();
			}
			Common.MemoryCleanupGC(false);
			GC.WaitForPendingFinalizers();
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000A57EC File Offset: 0x000A39EC
		internal static void FinalizeCurrentModule()
		{
			Module.CurrentModule.FinalizeModule();
			Module.CurrentModule = null;
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000A57FE File Offset: 0x000A39FE
		[MBCallback(null, false)]
		internal void SetLoadingFinished()
		{
			this.LoadingFinished = true;
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000A5807 File Offset: 0x000A3A07
		[MBCallback(null, false)]
		internal void OnCloseSceneEditorPresentation()
		{
			GameStateManager.Current.PopState(0);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000A5814 File Offset: 0x000A3A14
		[MBCallback(null, false)]
		internal void OnSceneEditorModeOver()
		{
			GameStateManager.Current.PopState(0);
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000A5824 File Offset: 0x000A3A24
		private void OnConfigChanged()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnConfigChanged();
			}
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000A587C File Offset: 0x000A3A7C
		private void OnConstrainedStateChange(bool isConstrained)
		{
			if (!isConstrained)
			{
				PlatformServices.Instance.OnFocusGained();
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000A588B File Offset: 0x000A3A8B
		private void OnFocusGained()
		{
			PlatformServices.Instance.OnFocusGained();
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000A5897 File Offset: 0x000A3A97
		private void OnTextEnteredFromPlatform(string text)
		{
			ScreenManager.OnOnscreenKeyboardDone(text);
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000A589F File Offset: 0x000A3A9F
		private void OnTextCanceledFromPlatform()
		{
			ScreenManager.OnOnscreenKeyboardCanceled();
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000A58A6 File Offset: 0x000A3AA6
		[MBCallback(null, false)]
		internal void OnSkinsXMLHasChanged()
		{
			if (this.SkinsXMLHasChanged != null)
			{
				this.SkinsXMLHasChanged();
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06002AFD RID: 11005 RVA: 0x000A58BC File Offset: 0x000A3ABC
		// (remove) Token: 0x06002AFE RID: 11006 RVA: 0x000A58F4 File Offset: 0x000A3AF4
		public event Action SkinsXMLHasChanged;

		// Token: 0x06002AFF RID: 11007 RVA: 0x000A5929 File Offset: 0x000A3B29
		[MBCallback(null, false)]
		internal void OnImguiProfilerTick()
		{
			if (this.ImguiProfilerTick != null)
			{
				this.ImguiProfilerTick();
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000A5940 File Offset: 0x000A3B40
		[MBCallback(null, false)]
		internal static string CreateProcessedSkinsXMLForNative(out string baseSkinsXmlPath)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_skins", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			baseSkinsXmlPath = list[0];
			return stringWriter.ToString();
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000A597C File Offset: 0x000A3B7C
		[MBCallback(null, false)]
		internal static string CreateProcessedItemHolstersXMLForNative(out string baseItemHolstersPath)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_item_holsters", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			baseItemHolstersPath = list[0];
			return stringWriter.ToString();
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000A59B8 File Offset: 0x000A3BB8
		[MBCallback(null, true)]
		internal static string CreateProcessedActionSetsXMLForNative()
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_action_sets", out list);
			Dictionary<string, XElement> dictionary = new Dictionary<string, XElement>();
			XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
			IEnumerable<XElement> enumerable = xdocument.Descendants("action_set");
			for (int i = 0; i < enumerable.Count<XElement>(); i++)
			{
				XElement xelement = enumerable.ElementAt<XElement>(i);
				string text = xelement.FirstAttribute.ToString();
				if (dictionary.ContainsKey(text))
				{
					dictionary[text].Add(xelement.Descendants());
					xelement.Remove();
					i--;
				}
				else
				{
					dictionary.Add(text, xelement);
				}
			}
			xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000A5A78 File Offset: 0x000A3C78
		[MBCallback(null, true)]
		internal static string CreateProcessedActionTypesXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_action_types", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000A5AAC File Offset: 0x000A3CAC
		[MBCallback(null, true)]
		internal static string CreateProcessedAnimationsXMLForNative(out string animationsXmlPaths)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_animations", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			animationsXmlPaths = "";
			for (int i = 0; i < list.Count; i++)
			{
				animationsXmlPaths += list[i];
				if (i != list.Count - 1)
				{
					animationsXmlPaths += "\n";
				}
			}
			return stringWriter.ToString();
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000A5B20 File Offset: 0x000A3D20
		[MBCallback(null, true)]
		internal static string CreateProcessedVoiceDefinitionsXMLForNative()
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_voice_definitions", out list);
			XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
			XElement xelement = xdocument.Descendants("voice_type_declarations").First<XElement>();
			for (int i = 1; i < xdocument.Descendants("voice_type_declarations").Count<XElement>(); i++)
			{
				xelement.Add(xdocument.Descendants("voice_type_declarations").ElementAt<XElement>(i).Descendants());
				xdocument.Descendants("voice_type_declarations").ElementAt<XElement>(i).Remove();
				i--;
			}
			for (int j = 0; j < xdocument.Descendants("voice_definition").Count<XElement>(); j++)
			{
				for (int k = j + 1; k < xdocument.Descendants("voice_definition").Count<XElement>(); k++)
				{
					if (xdocument.Descendants("voice_definition").ElementAt<XElement>(j).FirstAttribute.ToString() == xdocument.Descendants("voice_definition").ElementAt<XElement>(k).FirstAttribute.ToString())
					{
						xdocument.Descendants("voice_definition").ElementAt<XElement>(j).Add(xdocument.Descendants("voice_definition").ElementAt<XElement>(k).Descendants());
						xdocument.Descendants("voice_definition").ElementAt<XElement>(k).Remove();
						k--;
					}
				}
			}
			xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000A5CDC File Offset: 0x000A3EDC
		[MBCallback(null, true)]
		internal static string CreateProcessedSoundEventDataXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_sound_event_data", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000A5D10 File Offset: 0x000A3F10
		[MBCallback(null, true)]
		internal static string CreateProcessedSoundParamsXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_sound_parameter_data", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000A5D44 File Offset: 0x000A3F44
		[MBCallback(null, false)]
		internal static string CreateProcessedModuleDataXMLForNative(string xmlType)
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_" + xmlType, out list);
			if (xmlType == "full_movement_sets")
			{
				XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
				for (int i = 0; i < xdocument.Descendants("full_movement_set").Count<XElement>(); i++)
				{
					for (int j = i + 1; j < xdocument.Descendants("full_movement_set").Count<XElement>(); j++)
					{
						if (xdocument.Descendants("full_movement_set").ElementAt<XElement>(i).FirstAttribute.ToString() == xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).FirstAttribute.ToString())
						{
							xdocument.Descendants("full_movement_set").ElementAt<XElement>(i).Add(xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).Descendants());
							xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).Remove();
							j--;
						}
					}
				}
				xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			}
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06002B09 RID: 11017 RVA: 0x000A5E94 File Offset: 0x000A4094
		// (remove) Token: 0x06002B0A RID: 11018 RVA: 0x000A5ECC File Offset: 0x000A40CC
		public event Action ImguiProfilerTick;

		// Token: 0x06002B0B RID: 11019 RVA: 0x000A5F01 File Offset: 0x000A4101
		public void ClearStateOptions()
		{
			this._initialStateOptions.Clear();
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000A5F0E File Offset: 0x000A410E
		public void AddInitialStateOption(InitialStateOption initialStateOption)
		{
			this._initialStateOptions.Add(initialStateOption);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000A5F1C File Offset: 0x000A411C
		public void OverrideInitialStateOption(string id, InitialStateOption newInitialStateOption)
		{
			for (int i = 0; i < this._initialStateOptions.Count; i++)
			{
				if (this._initialStateOptions[i].Id == id)
				{
					this._initialStateOptions[i] = newInitialStateOption;
					return;
				}
			}
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000A5F66 File Offset: 0x000A4166
		public IEnumerable<InitialStateOption> GetInitialStateOptions()
		{
			return this._initialStateOptions.OrderBy<InitialStateOption, int>((InitialStateOption s) => s.OrderIndex);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000A5F94 File Offset: 0x000A4194
		public InitialStateOption GetInitialStateOptionWithId(string id)
		{
			foreach (InitialStateOption initialStateOption in this._initialStateOptions)
			{
				if (initialStateOption.Id == id)
				{
					return initialStateOption;
				}
			}
			return null;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000A5FF8 File Offset: 0x000A41F8
		public void ExecuteInitialStateOptionWithId(string id)
		{
			InitialStateOption initialStateOptionWithId = this.GetInitialStateOptionWithId(id);
			if (initialStateOptionWithId != null)
			{
				initialStateOptionWithId.DoAction();
			}
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000A6016 File Offset: 0x000A4216
		public void SetCanLoadModules(bool canLoadModules)
		{
			EngineApplicationInterface.IUtil.SetCanLoadModules(canLoadModules);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000A6023 File Offset: 0x000A4223
		void IGameStateManagerOwner.OnStateStackEmpty()
		{
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000A6025 File Offset: 0x000A4225
		void IGameStateManagerOwner.OnStateChanged(GameState oldState)
		{
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000A6027 File Offset: 0x000A4227
		public void SetEditorMissionTester(IEditorMissionTester editorMissionTester)
		{
			this._editorMissionTester = editorMissionTester;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000A6030 File Offset: 0x000A4230
		[MBCallback(null, false)]
		internal void StartMissionForEditor(string missionName, string sceneName, string levels)
		{
			if (this._editorMissionTester != null)
			{
				this._editorMissionTester.StartMissionForEditor(missionName, sceneName, levels);
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000A6048 File Offset: 0x000A4248
		[MBCallback(null, false)]
		internal void StartMissionForReplayEditor(string missionName, string sceneName, string levels, string fileName, bool record, float startTime, float endTime)
		{
			if (this._editorMissionTester != null)
			{
				this._editorMissionTester.StartMissionForReplayEditor(missionName, sceneName, levels, fileName, record, startTime, endTime);
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000A6068 File Offset: 0x000A4268
		public void StartMissionForEditorAux(string missionName, string sceneName, string levels, bool forReplay, string replayFileName, bool isRecord)
		{
			GameStateManager.Current = Game.Current.GameStateManager;
			this.ReturnToEditorState = true;
			MissionInfo missionInfo = this._missionInfos.Find((MissionInfo mi) => mi.Name == missionName);
			if (missionInfo == null)
			{
				missionInfo = this._missionInfos.Find((MissionInfo mi) => mi.Name.Contains(missionName));
			}
			if (forReplay)
			{
				missionInfo.Creator.Invoke(null, new object[] { replayFileName, isRecord });
				return;
			}
			missionInfo.Creator.Invoke(null, new object[] { sceneName, levels });
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000A610D File Offset: 0x000A430D
		private void FillMultiplayerGameTypes()
		{
			this._multiplayerGameModesWithNames = new Dictionary<string, MultiplayerGameMode>();
			this._multiplayerGameTypes = new MBList<MultiplayerGameTypeInfo>();
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000A6128 File Offset: 0x000A4328
		public MultiplayerGameMode GetMultiplayerGameMode(string gameType)
		{
			MultiplayerGameMode multiplayerGameMode;
			if (this._multiplayerGameModesWithNames.TryGetValue(gameType, out multiplayerGameMode))
			{
				return multiplayerGameMode;
			}
			return null;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000A6148 File Offset: 0x000A4348
		public void AddMultiplayerGameMode(MultiplayerGameMode multiplayerGameMode)
		{
			this._multiplayerGameModesWithNames.Add(multiplayerGameMode.Name, multiplayerGameMode);
			this._multiplayerGameTypes.Add(new MultiplayerGameTypeInfo("Native", multiplayerGameMode.Name));
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000A6177 File Offset: 0x000A4377
		public MBReadOnlyList<MultiplayerGameTypeInfo> GetMultiplayerGameTypes()
		{
			return this._multiplayerGameTypes;
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000A6180 File Offset: 0x000A4380
		public bool StartMultiplayerGame(string multiplayerGameType, string scene)
		{
			MultiplayerGameMode multiplayerGameMode;
			if (this._multiplayerGameModesWithNames.TryGetValue(multiplayerGameType, out multiplayerGameMode))
			{
				multiplayerGameMode.StartMultiplayerGame(scene);
				return true;
			}
			return false;
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000A61A8 File Offset: 0x000A43A8
		public async void ShutDownWithDelay(string reason, int seconds)
		{
			if (!this._isShuttingDown)
			{
				this._isShuttingDown = true;
				for (int i = 0; i < seconds; i++)
				{
					int num = seconds - i;
					string text = string.Concat(new object[] { "Shutting down in ", num, " seconds with reason '", reason, "'" });
					Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
					Console.WriteLine(text);
					await Task.Delay(1000);
				}
				if (Game.Current != null)
				{
					Debug.Print("Active game exist during ShutDownWithDelay", 0, Debug.DebugColor.White, 17592186044416UL);
					MBGameManager.EndGame();
				}
				Utilities.QuitGame();
			}
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000A61F4 File Offset: 0x000A43F4
		public void DeactiveModule(string moduleId)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleId);
			if (moduleInfo != null && moduleInfo.IsActive && !moduleInfo.IsNative)
			{
				Debug.Print("Deactivating Module: " + moduleId, 0, Debug.DebugColor.Green, 17592186044416UL);
				ModuleHelper.OnModuleDeactivated(moduleId);
				foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
				{
					MBSubModuleBase mbsubModuleBase;
					if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleDeactivated();
					}
				}
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000A6294 File Offset: 0x000A4494
		public void ActivateModule(string moduleId)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleId);
			if (moduleInfo != null && !moduleInfo.IsActive)
			{
				Debug.Print("Activating Module: " + moduleId, 0, Debug.DebugColor.Green, 17592186044416UL);
				ModuleHelper.OnModuleActivated(moduleId);
				foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
				{
					MBSubModuleBase mbsubModuleBase;
					if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleActivated();
					}
				}
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000A632C File Offset: 0x000A452C
		internal void OnBeforeGameStart(MBGameManager mbGameManager)
		{
			List<string> list = new List<string>();
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnBeforeGameStart(mbGameManager, list);
			}
			foreach (string text in list)
			{
				ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(text);
				if (moduleInfo != null && moduleInfo.IsActive)
				{
					this.DeactiveModule(text);
				}
			}
			MBList<ModuleInfo> mblist = new MBList<ModuleInfo>();
			foreach (ModuleInfo moduleInfo2 in ModuleHelper.GetActiveModules())
			{
				foreach (DependedModule dependedModule in moduleInfo2.DependedModules)
				{
					foreach (string text2 in list)
					{
						ModuleInfo moduleInfo3 = ModuleHelper.GetModuleInfo(text2);
						if (moduleInfo3 != null && moduleInfo2 != moduleInfo3 && dependedModule.ModuleId == text2)
						{
							mblist.Add(moduleInfo3);
						}
					}
				}
			}
			if (mblist.Any<ModuleInfo>((ModuleInfo x) => !x.IsOfficial))
			{
				string.Join("\n", from x in mblist
					where !x.IsOfficial
					select x.Name);
			}
			InformationManager.ClearAllMessages();
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000A6548 File Offset: 0x000A4748
		internal void OnGameEnd()
		{
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules((ModuleInfo x) => !x.IsActive))
			{
				this.ActivateModule(moduleInfo.Id);
			}
		}

		// Token: 0x0400105A RID: 4186
		private bool _enableCoreContentOnReturnToRoot;

		// Token: 0x0400105F RID: 4191
		private List<MissionInfo> _missionInfos;

		// Token: 0x04001060 RID: 4192
		private TestContext _testContext;

		// Token: 0x04001063 RID: 4195
		private SingleThreadedSynchronizationContext _synchronizationContext;

		// Token: 0x04001064 RID: 4196
		private readonly Dictionary<SubModuleInfo, MBSubModuleBase> _subModuleBases;

		// Token: 0x04001065 RID: 4197
		private bool _splashScreenPlayed;

		// Token: 0x04001068 RID: 4200
		private List<InitialStateOption> _initialStateOptions;

		// Token: 0x04001069 RID: 4201
		private IEditorMissionTester _editorMissionTester;

		// Token: 0x0400106A RID: 4202
		private Dictionary<string, MultiplayerGameMode> _multiplayerGameModesWithNames;

		// Token: 0x0400106B RID: 4203
		private MBList<MultiplayerGameTypeInfo> _multiplayerGameTypes = new MBList<MultiplayerGameTypeInfo>();

		// Token: 0x0400106C RID: 4204
		private bool _isShuttingDown;

		// Token: 0x020005C8 RID: 1480
		public enum XmlInformationType
		{
			// Token: 0x04001F92 RID: 8082
			Parameters,
			// Token: 0x04001F93 RID: 8083
			MbObjectType
		}

		// Token: 0x020005C9 RID: 1481
		private enum StartupType
		{
			// Token: 0x04001F95 RID: 8085
			None,
			// Token: 0x04001F96 RID: 8086
			TestMode,
			// Token: 0x04001F97 RID: 8087
			GameServer,
			// Token: 0x04001F98 RID: 8088
			Singleplayer,
			// Token: 0x04001F99 RID: 8089
			Multiplayer,
			// Token: 0x04001F9A RID: 8090
			Count
		}
	}
}
