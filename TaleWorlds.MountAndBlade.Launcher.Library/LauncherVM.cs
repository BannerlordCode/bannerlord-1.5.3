using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Launcher.Library.UserDatas;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000017 RID: 23
	public class LauncherVM : ViewModel
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004FBA File Offset: 0x000031BA
		public string GameTypeArgument
		{
			get
			{
				if (!this.IsMultiplayer)
				{
					return "/singleplayer";
				}
				return "/multiplayer";
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004FCF File Offset: 0x000031CF
		public string ContinueGameArgument
		{
			get
			{
				if (!this._isContinueSelected)
				{
					return "";
				}
				return " /continuegame";
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004FE4 File Offset: 0x000031E4
		public LauncherVM(UserDataManager userDataManager, Action onClose, Action onMinimize)
		{
			this._userDataManager = userDataManager;
			this._newsManager = new NewsManager();
			this._newsManager.SetNewsSourceURL(this.GetApplicableNewsSourceURL());
			this._onClose = onClose;
			this._onMinimize = onMinimize;
			this.PlayText = "P L A Y";
			this.ContinueText = "C O N T I N U E";
			this.LaunchText = "L A U N C H";
			this.SingleplayerText = "Singleplayer";
			this.MultiplayerText = "Multiplayer";
			this.DigitalCompanionText = "Digital Companion";
			this.NewsText = "News";
			this.DlcText = "DLC";
			this.ModsText = "Mods";
			this.VersionText = ApplicationVersion.FromParametersFile(null).ToString();
			this.IsSingleplayerAvailable = this.GameModExists("Sandbox");
			this.IsDigitalCompanionAvailable = Program.IsDigitalCompanionAvailable();
			bool flag = !this.IsSingleplayerAvailable || this._userDataManager.UserData.GameType == GameType.Multiplayer;
			this.ConfirmStart = new LauncherConfirmStartVM(new Action(this.ExecuteConfirmUnverifiedDLLStart));
			this.News = new LauncherNewsVM(this._newsManager, flag);
			this.ModsData = new LauncherModsVM(userDataManager);
			this.Hint = new LauncherInformationVM();
			this.IsSingleplayer = !flag;
			this.IsMultiplayer = flag;
			this.IsDigitalCompanion = false;
			this.Refresh();
			this._isInitialized = true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005148 File Offset: 0x00003348
		private void UpdateAndSaveUserModsData(bool isMultiplayer)
		{
			UserData userData = this._userDataManager.UserData;
			UserGameTypeData userGameTypeData = (isMultiplayer ? userData.MultiplayerData : userData.SingleplayerData);
			userGameTypeData.ModDatas.Clear();
			foreach (LauncherModuleVM launcherModuleVM in this.ModsData.Modules)
			{
				userGameTypeData.ModDatas.Add(new UserModData(launcherModuleVM.Info.Id, launcherModuleVM.Info.Version.ToString(), launcherModuleVM.IsSelected));
			}
			this._userDataManager.SaveUserData();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005204 File Offset: 0x00003404
		private bool GameModExists(string modId)
		{
			List<ModuleInfo> modulesForLauncher = ModuleHelper.GetModulesForLauncher();
			for (int i = 0; i < modulesForLauncher.Count; i++)
			{
				if (modulesForLauncher[i].Id == modId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000523F File Offset: 0x0000343F
		private void OnBeforeGameTypeChange(bool preSelectionIsMultiplayer, bool newSelectionIsMultiplayer)
		{
			if (!this._isInitialized)
			{
				return;
			}
			this._userDataManager.UserData.GameType = (newSelectionIsMultiplayer ? GameType.Multiplayer : GameType.Singleplayer);
			this.UpdateAndSaveUserModsData(preSelectionIsMultiplayer);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005268 File Offset: 0x00003468
		private void OnAfterGameTypeChange(bool isMultiplayer, bool isSingleplayer, bool isDigitalCompanion)
		{
			this.IsMultiplayer = isMultiplayer;
			this.IsSingleplayer = isSingleplayer;
			this.IsDigitalCompanion = isDigitalCompanion;
			this.Refresh();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005288 File Offset: 0x00003488
		public void ExecuteStartGame(int mode)
		{
			this._isContinueSelected = mode == 1;
			this.UpdateAndSaveUserModsData(this.IsMultiplayer);
			List<SubModuleInfo> list = new List<SubModuleInfo>();
			List<DependentVersionMissmatchItem> list2 = new List<DependentVersionMissmatchItem>();
			if (this.IsSingleplayer)
			{
				foreach (LauncherModuleVM launcherModuleVM in this.ModsData.Modules)
				{
					if (launcherModuleVM.IsSelected)
					{
						foreach (LauncherSubModule launcherSubModule in launcherModuleVM.SubModules)
						{
							if (!string.IsNullOrEmpty(launcherSubModule.Info.DLLName) && launcherSubModule.Info.DLLExists && !launcherSubModule.Info.IsTWCertifiedDLL)
							{
								list.Add(launcherSubModule.Info);
							}
						}
						if (!launcherModuleVM.Info.IsOfficial)
						{
							List<Tuple<DependedModule, ApplicationVersion>> list3 = new List<Tuple<DependedModule, ApplicationVersion>>();
							foreach (DependedModule dependedModule in launcherModuleVM.Info.DependedModules)
							{
								ApplicationVersion applicationVersionOfModule = this.GetApplicationVersionOfModule(dependedModule.ModuleId);
								if (!dependedModule.Version.IsSame(applicationVersionOfModule, false))
								{
									list3.Add(new Tuple<DependedModule, ApplicationVersion>(dependedModule, applicationVersionOfModule));
								}
							}
							if (list3.Count > 0)
							{
								list2.Add(new DependentVersionMissmatchItem(launcherModuleVM.Name, list3));
							}
						}
					}
				}
			}
			if (this.IsDigitalCompanion)
			{
				Program.StartDigitalCompanion();
				return;
			}
			if (list.Count > 0 || list2.Count > 0)
			{
				this.ConfirmStart.EnableWith(list, list2);
				return;
			}
			Program.StartGame();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000548C File Offset: 0x0000368C
		private ApplicationVersion GetApplicationVersionOfModule(string id)
		{
			foreach (LauncherModuleVM launcherModuleVM in this.ModsData.Modules)
			{
				if (((launcherModuleVM != null) ? launcherModuleVM.Info : null) == null)
				{
					Debug.FailedAssert("Info for module is null: " + (((launcherModuleVM != null) ? launcherModuleVM.Name : null) ?? "---"), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Launcher.Library\\ViewModels\\LauncherVM.cs", "GetApplicationVersionOfModule", 198);
				}
				if (launcherModuleVM.Info.Id == id)
				{
					return launcherModuleVM.Info.Version;
				}
			}
			return ApplicationVersion.Empty;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005540 File Offset: 0x00003740
		private void ExecuteConfirmUnverifiedDLLStart()
		{
			Program.StartGame();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005547 File Offset: 0x00003747
		public void ExecuteClose()
		{
			this.UpdateAndSaveUserModsData(this.IsMultiplayer);
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005565 File Offset: 0x00003765
		public void ExecuteMinimize()
		{
			Action onMinimize = this._onMinimize;
			if (onMinimize == null)
			{
				return;
			}
			onMinimize();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005578 File Offset: 0x00003778
		private void Refresh()
		{
			this.News.Refresh(this.IsMultiplayer);
			this.ModsData.Refresh(this.IsDigitalCompanion, this.IsMultiplayer);
			this.VersionText = ApplicationVersion.FromParametersFile(null).ToString();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000055C8 File Offset: 0x000037C8
		private string GetApplicableNewsSourceURL()
		{
			int geoID = Kernel32.GetUserGeoID(Kernel32.GeoTypeId.Nation);
			RegionInfo regionInfo = (from x in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				select new RegionInfo(x.ToString())).FirstOrDefault<RegionInfo>((RegionInfo r) => r.GeoId == geoID);
			bool flag = string.Equals((regionInfo != null) ? regionInfo.TwoLetterISORegionName : null, "cn", StringComparison.OrdinalIgnoreCase);
			bool isInPreviewMode = this._newsManager.IsInPreviewMode;
			string text = (flag ? "zh" : "en");
			this._newsManager.UpdateLocalizationID(text);
			if (!isInPreviewMode)
			{
				return "https://taleworldswebsiteassets.blob.core.windows.net/upload/bannerlordnews/NewsFeed_" + text + ".json";
			}
			return "https://taleworldswebsiteassets.blob.core.windows.net/upload/bannerlordnews/NewsFeed_" + text + "_preview.json";
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005685 File Offset: 0x00003885
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000568D File Offset: 0x0000388D
		[DataSourceProperty]
		public bool IsSingleplayer
		{
			get
			{
				return this._isSingleplayer;
			}
			set
			{
				if (this._isSingleplayer != value)
				{
					this.OnBeforeGameTypeChange(this._isMultiplayer, !value);
					this._isSingleplayer = value;
					base.OnPropertyChangedWithValue(value, "IsSingleplayer");
					if (value)
					{
						this.OnAfterGameTypeChange(false, true, false);
					}
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000056C7 File Offset: 0x000038C7
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000056CF File Offset: 0x000038CF
		[DataSourceProperty]
		public bool IsMultiplayer
		{
			get
			{
				return this._isMultiplayer;
			}
			set
			{
				if (this._isMultiplayer != value)
				{
					this.OnBeforeGameTypeChange(this._isMultiplayer, value);
					this._isMultiplayer = value;
					base.OnPropertyChangedWithValue(value, "IsMultiplayer");
					if (value)
					{
						this.OnAfterGameTypeChange(true, false, false);
					}
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005706 File Offset: 0x00003906
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000570E File Offset: 0x0000390E
		[DataSourceProperty]
		public bool IsDigitalCompanion
		{
			get
			{
				return this._isDigitalCompanion;
			}
			set
			{
				if (this._isDigitalCompanion != value)
				{
					this._isDigitalCompanion = value;
					base.OnPropertyChangedWithValue(value, "IsDigitalCompanion");
					if (value)
					{
						this.OnAfterGameTypeChange(false, false, true);
					}
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005738 File Offset: 0x00003938
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00005740 File Offset: 0x00003940
		[DataSourceProperty]
		public bool IsSingleplayerAvailable
		{
			get
			{
				return this._isSingleplayerAvailable;
			}
			set
			{
				if (value != this._isSingleplayerAvailable)
				{
					this._isSingleplayerAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSingleplayerAvailable");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000575E File Offset: 0x0000395E
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005766 File Offset: 0x00003966
		[DataSourceProperty]
		public bool IsDigitalCompanionAvailable
		{
			get
			{
				return this._isDigitalCompanionAvailable;
			}
			set
			{
				if (value != this._isDigitalCompanionAvailable)
				{
					this._isDigitalCompanionAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsDigitalCompanionAvailable");
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00005784 File Offset: 0x00003984
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000578C File Offset: 0x0000398C
		[DataSourceProperty]
		public string VersionText
		{
			get
			{
				return this._versionText;
			}
			set
			{
				if (value != this._versionText)
				{
					this._versionText = value;
					base.OnPropertyChangedWithValue<string>(value, "VersionText");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000057AF File Offset: 0x000039AF
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000057B7 File Offset: 0x000039B7
		[DataSourceProperty]
		public LauncherNewsVM News
		{
			get
			{
				return this._news;
			}
			set
			{
				if (value != this._news)
				{
					this._news = value;
					base.OnPropertyChangedWithValue<LauncherNewsVM>(value, "News");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000057D5 File Offset: 0x000039D5
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000057DD File Offset: 0x000039DD
		[DataSourceProperty]
		public LauncherConfirmStartVM ConfirmStart
		{
			get
			{
				return this._confirmStart;
			}
			set
			{
				if (value != this._confirmStart)
				{
					this._confirmStart = value;
					base.OnPropertyChangedWithValue<LauncherConfirmStartVM>(value, "ConfirmStart");
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000057FB File Offset: 0x000039FB
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005803 File Offset: 0x00003A03
		[DataSourceProperty]
		public LauncherModsVM ModsData
		{
			get
			{
				return this._modsData;
			}
			set
			{
				if (value != this._modsData)
				{
					this._modsData = value;
					base.OnPropertyChangedWithValue<LauncherModsVM>(value, "ModsData");
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005821 File Offset: 0x00003A21
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005829 File Offset: 0x00003A29
		[DataSourceProperty]
		public LauncherInformationVM Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (this._hint != value)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<LauncherInformationVM>(value, "Hint");
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005847 File Offset: 0x00003A47
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x0000584F File Offset: 0x00003A4F
		[DataSourceProperty]
		public string PlayText
		{
			get
			{
				return this._playText;
			}
			set
			{
				if (this._playText != value)
				{
					this._playText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayText");
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005872 File Offset: 0x00003A72
		// (set) Token: 0x060000FB RID: 251 RVA: 0x0000587A File Offset: 0x00003A7A
		[DataSourceProperty]
		public string ContinueText
		{
			get
			{
				return this._continueText;
			}
			set
			{
				if (this._continueText != value)
				{
					this._continueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ContinueText");
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000589D File Offset: 0x00003A9D
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000058A5 File Offset: 0x00003AA5
		[DataSourceProperty]
		public string LaunchText
		{
			get
			{
				return this._launchText;
			}
			set
			{
				if (this._launchText != value)
				{
					this._launchText = value;
					base.OnPropertyChangedWithValue<string>(value, "LaunchText");
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000058C8 File Offset: 0x00003AC8
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000058D0 File Offset: 0x00003AD0
		[DataSourceProperty]
		public string SingleplayerText
		{
			get
			{
				return this._singleplayerText;
			}
			set
			{
				if (this._singleplayerText != value)
				{
					this._singleplayerText = value;
					base.OnPropertyChangedWithValue<string>(value, "SingleplayerText");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000058F3 File Offset: 0x00003AF3
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000058FB File Offset: 0x00003AFB
		[DataSourceProperty]
		public string DigitalCompanionText
		{
			get
			{
				return this._digitalCompanionText;
			}
			set
			{
				if (this._digitalCompanionText != value)
				{
					this._digitalCompanionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DigitalCompanionText");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000591E File Offset: 0x00003B1E
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005926 File Offset: 0x00003B26
		[DataSourceProperty]
		public string MultiplayerText
		{
			get
			{
				return this._multiplayerText;
			}
			set
			{
				if (this._multiplayerText != value)
				{
					this._multiplayerText = value;
					base.OnPropertyChangedWithValue<string>(value, "MultiplayerText");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005949 File Offset: 0x00003B49
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005951 File Offset: 0x00003B51
		[DataSourceProperty]
		public string NewsText
		{
			get
			{
				return this._newsText;
			}
			set
			{
				if (this._newsText != value)
				{
					this._newsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NewsText");
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005974 File Offset: 0x00003B74
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000597C File Offset: 0x00003B7C
		[DataSourceProperty]
		public string DlcText
		{
			get
			{
				return this._dlcText;
			}
			set
			{
				if (this._dlcText != value)
				{
					this._dlcText = value;
					base.OnPropertyChangedWithValue<string>(value, "DlcText");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000599F File Offset: 0x00003B9F
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000059A7 File Offset: 0x00003BA7
		[DataSourceProperty]
		public string ModsText
		{
			get
			{
				return this._modsText;
			}
			set
			{
				if (this._modsText != value)
				{
					this._modsText = value;
					base.OnPropertyChangedWithValue<string>(value, "ModsText");
				}
			}
		}

		// Token: 0x0400006C RID: 108
		private UserDataManager _userDataManager;

		// Token: 0x0400006D RID: 109
		private NewsManager _newsManager;

		// Token: 0x0400006E RID: 110
		private readonly Action _onClose;

		// Token: 0x0400006F RID: 111
		private readonly Action _onMinimize;

		// Token: 0x04000070 RID: 112
		private bool _isInitialized;

		// Token: 0x04000071 RID: 113
		private bool _isContinueSelected;

		// Token: 0x04000072 RID: 114
		private const string _newsSourceURLBase = "https://taleworldswebsiteassets.blob.core.windows.net/upload/bannerlordnews/NewsFeed_";

		// Token: 0x04000073 RID: 115
		private bool _isMultiplayer;

		// Token: 0x04000074 RID: 116
		private bool _isSingleplayer;

		// Token: 0x04000075 RID: 117
		private bool _isDigitalCompanion;

		// Token: 0x04000076 RID: 118
		private bool _isSingleplayerAvailable;

		// Token: 0x04000077 RID: 119
		private bool _isDigitalCompanionAvailable;

		// Token: 0x04000078 RID: 120
		private LauncherNewsVM _news;

		// Token: 0x04000079 RID: 121
		private LauncherModsVM _modsData;

		// Token: 0x0400007A RID: 122
		private LauncherConfirmStartVM _confirmStart;

		// Token: 0x0400007B RID: 123
		private LauncherInformationVM _hint;

		// Token: 0x0400007C RID: 124
		private string _playText;

		// Token: 0x0400007D RID: 125
		private string _continueText;

		// Token: 0x0400007E RID: 126
		private string _launchText;

		// Token: 0x0400007F RID: 127
		private string _singleplayerText;

		// Token: 0x04000080 RID: 128
		private string _multiplayerText;

		// Token: 0x04000081 RID: 129
		private string _digitalCompanionText;

		// Token: 0x04000082 RID: 130
		private string _newsText;

		// Token: 0x04000083 RID: 131
		private string _dlcText;

		// Token: 0x04000084 RID: 132
		private string _modsText;

		// Token: 0x04000085 RID: 133
		private string _versionText;
	}
}
