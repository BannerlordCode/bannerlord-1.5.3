using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x02000060 RID: 96
	public class MPCustomGameItemVM : ViewModel
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001C492 File Offset: 0x0001A692
		public GameServerEntry GameServerInfo { get; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001C49A File Offset: 0x0001A69A
		public PremadeGameEntry PremadeGameInfo { get; }

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
		public MPCustomGameItemVM(GameServerEntry gameServerInfo, Action<MPCustomGameItemVM> onSelect, Action<MPCustomGameItemVM> onJoin, Action<MPCustomGameItemVM> onRequestActions, Action<MPCustomGameItemVM> onToggleFavorite)
		{
			this._onSelect = onSelect;
			this._onJoin = onJoin;
			this._onRequestActions = onRequestActions;
			this._onToggleFavorite = onToggleFavorite;
			this.GameServerInfo = gameServerInfo;
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			this._randomString = "-- " + text + " --";
			this.LoadedModulesHint = new BasicTooltipViewModel(() => this.GetLoadedModulesTooltipProperties());
			this.UpdateGameServerInfo();
			this.UpdateIsFavorite();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001C528 File Offset: 0x0001A728
		public MPCustomGameItemVM(PremadeGameEntry premadeGameInfo, Action<MPCustomGameItemVM> onJoin)
		{
			this._onJoin = onJoin;
			this.PremadeGameInfo = premadeGameInfo;
			this.IsClanMatchItem = true;
			this.IsPingInfoAvailable = false;
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			this._randomString = "-- " + text + " --";
			this.UpdatePremadeGameInfo();
			this.UpdateIsFavorite();
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001C58C File Offset: 0x0001A78C
		private async void UpdateGameServerInfo()
		{
			this.IsPasswordProtected = this.GameServerInfo.PasswordProtected;
			this.PlayerCount = this.GameServerInfo.PlayerCount;
			this.MaxPlayerCount = this.GameServerInfo.MaxPlayerCount;
			this.NameText = this.GameServerInfo.ServerName;
			TextObject textObject = GameTexts.FindText("str_multiplayer_official_game_type_name", this.GameServerInfo.GameType);
			this.GameTypeText = (textObject.ToString().StartsWith("ERROR: ") ? new TextObject("{=MT4b8H9h}Unknown", null).ToString() : textObject.ToString());
			GameTexts.SetVariable("LEFT", this.PlayerCount);
			GameTexts.SetVariable("RIGHT", this.MaxPlayerCount);
			this.PlayerCountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			this.SpectatorCount = this.GameServerInfo.SpectatorCount;
			this.MaxSpectatorCount = this.GameServerInfo.MaxSpectatorCount;
			if (this.MaxSpectatorCount > 0)
			{
				GameTexts.SetVariable("LEFT", this.SpectatorCount);
				GameTexts.SetVariable("RIGHT", this.MaxSpectatorCount);
				this.SpectatorCountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			}
			else
			{
				this.SpectatorCountText = this.SpectatorCount.ToString();
			}
			this.ShowSpectatorCount = this.SpectatorCount > 0;
			TextObject textObject2 = new TextObject("{=vGnGbK9I}Spectators: {COUNT}", null);
			textObject2.SetTextVariable("COUNT", this.SpectatorCountText);
			this.SpectatorCountHintText = textObject2.ToString();
			this.EnableSpectators = this.GameServerInfo.EnableSpectators;
			this.IsOfficialServer = this.GameServerInfo.IsOfficial;
			this.IsByOfficialServerProvider = this.GameServerInfo.ByOfficialProvider;
			this.IsCommunityServer = !this.IsOfficialServer && !this.IsByOfficialServerProvider;
			this.HostText = this.GameServerInfo.HostName;
			this.IsPingInfoAvailable = MPCustomGameVM.IsPingInfoAvailable;
			await this.UpdatePingText();
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001C5C8 File Offset: 0x0001A7C8
		private async Task UpdatePingText()
		{
			if (this.IsPingInfoAvailable)
			{
				long num = await NetworkMain.GameClient.GetPingToServer(this.GameServerInfo.Address);
				this.PingText = ((num < 0L) ? "-" : num.ToString());
			}
			else
			{
				this.PingText = "-";
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001C610 File Offset: 0x0001A810
		private void UpdatePremadeGameInfo()
		{
			this.IsPasswordProtected = this.PremadeGameInfo.IsPasswordProtected;
			this.NameText = this.PremadeGameInfo.Name;
			this.GameTypeText = (this.GameTypeText = GameTexts.FindText("str_multiplayer_official_game_type_name", this.PremadeGameInfo.GameType).ToString());
			this.RegionName = this.PremadeGameInfo.Region;
			this.FirstFactionName = ((this.PremadeGameInfo.FactionA == Parameters.RandomSelectionString) ? this._randomString : this.PremadeGameInfo.FactionA);
			this.SecondFactionName = ((this.PremadeGameInfo.FactionB == Parameters.RandomSelectionString) ? this._randomString : this.PremadeGameInfo.FactionB);
			this.HostText = MPCustomGameItemVM.OfficialServerHostName;
			this.IsOfficialServer = true;
			if (this.PremadeGameInfo.PremadeGameType == PremadeGameType.Clan)
			{
				this.PremadeMatchTypeText = new TextObject("{=YNkPy4ta}Clan Match", null).ToString();
				return;
			}
			if (this.PremadeGameInfo.PremadeGameType == PremadeGameType.Practice)
			{
				this.PremadeMatchTypeText = new TextObject("{=H5tiRTya}Practice", null).ToString();
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001C734 File Offset: 0x0001A934
		private List<TooltipProperty> GetLoadedModulesTooltipProperties()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			if (this.GameServerInfo != null)
			{
				if (this.GameServerInfo.LoadedModules.Count > 0)
				{
					list.Add(new TooltipProperty(string.Empty, new TextObject("{=JXyxj1J5}Modules", null).ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.Title));
					string text = " " + new TextObject("{=oYS9sabI}(optional)", null).ToString();
					foreach (ModuleInfoModel moduleInfoModel in this.GameServerInfo.LoadedModules)
					{
						string text2 = moduleInfoModel.Version;
						if (moduleInfoModel.IsOptional)
						{
							text2 += text;
						}
						list.Add(new TooltipProperty(moduleInfoModel.Name, text2, 0, false, TooltipProperty.TooltipPropertyFlags.None));
					}
				}
				TextObject textObject = (this.GameServerInfo.AllowsOptionalModules ? new TextObject("{=BBmEESTT}This server allows optional modules.", null) : new TextObject("{=sEbeLmZP}This server does not allow optional modules.", null));
				list.Add(new TooltipProperty("", textObject.ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.None));
				if (this.IsCommunityServer)
				{
					list.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
					TextObject textObject2 = new TextObject("{=W51HSyXy}Press {VIEW_OPTIONS_KEY} to view options", null);
					string text3 = HotKeyManager.GetCategory("MultiplayerHotkeyCategory").GetHotKey("PreviewCosmeticItem").ToString();
					textObject2.SetTextVariable("VIEW_OPTIONS_KEY", Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(text3.ToLower()));
					list.Add(new TooltipProperty(string.Empty, textObject2.ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.None)
					{
						OnlyShowWhenNotExtended = true
					});
				}
			}
			return list;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001C8F8 File Offset: 0x0001AAF8
		public void UpdateIsFavorite()
		{
			bool flag = false;
			if (this.GameServerInfo != null)
			{
				FavoriteServerData favoriteServerData;
				flag = MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(this.GameServerInfo, out favoriteServerData);
			}
			this.IsFavorite = flag;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001C92E File Offset: 0x0001AB2E
		public void ExecuteSelect()
		{
			Action<MPCustomGameItemVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001C941 File Offset: 0x0001AB41
		public void ExecuteFavorite()
		{
			Action<MPCustomGameItemVM> onToggleFavorite = this._onToggleFavorite;
			if (onToggleFavorite == null)
			{
				return;
			}
			onToggleFavorite(this);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001C954 File Offset: 0x0001AB54
		public void ExecuteJoin()
		{
			Action<MPCustomGameItemVM> onJoin = this._onJoin;
			if (onJoin == null)
			{
				return;
			}
			onJoin(this);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001C967 File Offset: 0x0001AB67
		public void ExecuteViewHostOptions()
		{
			if (this._onRequestActions != null)
			{
				this._onRequestActions(this);
				MBInformationManager.HideInformations();
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0001C982 File Offset: 0x0001AB82
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0001C98A File Offset: 0x0001AB8A
		[DataSourceProperty]
		public bool IsPasswordProtected
		{
			get
			{
				return this._isPasswordProtected;
			}
			set
			{
				if (value != this._isPasswordProtected)
				{
					this._isPasswordProtected = value;
					base.OnPropertyChanged("IsPasswordProtected");
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0001C9A7 File Offset: 0x0001ABA7
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0001C9AF File Offset: 0x0001ABAF
		[DataSourceProperty]
		public bool IsFavorite
		{
			get
			{
				return this._isFavorite;
			}
			set
			{
				if (value != this._isFavorite)
				{
					this._isFavorite = value;
					base.OnPropertyChanged("IsFavorite");
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001C9CC File Offset: 0x0001ABCC
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x0001C9D4 File Offset: 0x0001ABD4
		[DataSourceProperty]
		public bool IsClanMatchItem
		{
			get
			{
				return this._isClanMatchItem;
			}
			set
			{
				if (value != this._isClanMatchItem)
				{
					this._isClanMatchItem = value;
					base.OnPropertyChanged("IsClanMatchItem");
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0001C9F1 File Offset: 0x0001ABF1
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x0001C9F9 File Offset: 0x0001ABF9
		[DataSourceProperty]
		public bool IsOfficialServer
		{
			get
			{
				return this._isOfficialServer;
			}
			set
			{
				if (value != this._isOfficialServer)
				{
					this._isOfficialServer = value;
					base.OnPropertyChanged("IsOfficialServer");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001CA16 File Offset: 0x0001AC16
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x0001CA1E File Offset: 0x0001AC1E
		[DataSourceProperty]
		public bool IsByOfficialServerProvider
		{
			get
			{
				return this._isByOfficialServerProvider;
			}
			set
			{
				if (value != this._isByOfficialServerProvider)
				{
					this._isByOfficialServerProvider = value;
					base.OnPropertyChanged("IsByOfficialServerProvider");
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001CA3B File Offset: 0x0001AC3B
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x0001CA43 File Offset: 0x0001AC43
		[DataSourceProperty]
		public bool IsCommunityServer
		{
			get
			{
				return this._isCommunityServer;
			}
			set
			{
				if (value != this._isCommunityServer)
				{
					this._isCommunityServer = value;
					base.OnPropertyChanged("IsCommunityServer");
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0001CA60 File Offset: 0x0001AC60
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0001CA68 File Offset: 0x0001AC68
		[DataSourceProperty]
		public bool IsPingInfoAvailable
		{
			get
			{
				return this._isPingInfoAvailable;
			}
			set
			{
				if (value != this._isPingInfoAvailable)
				{
					this._isPingInfoAvailable = value;
					base.OnPropertyChanged("IsPingInfoAvailable");
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0001CA85 File Offset: 0x0001AC85
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0001CA8D File Offset: 0x0001AC8D
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001CAAA File Offset: 0x0001ACAA
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x0001CAB2 File Offset: 0x0001ACB2
		[DataSourceProperty]
		public int PlayerCount
		{
			get
			{
				return this._playerCount;
			}
			set
			{
				if (value != this._playerCount)
				{
					this._playerCount = value;
					base.OnPropertyChanged("PlayerCount");
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0001CACF File Offset: 0x0001ACCF
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x0001CAD7 File Offset: 0x0001ACD7
		[DataSourceProperty]
		public int MaxPlayerCount
		{
			get
			{
				return this._maxPlayerCount;
			}
			set
			{
				if (value != this._maxPlayerCount)
				{
					this._maxPlayerCount = value;
					base.OnPropertyChanged("MaxPlayerCount");
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0001CAFC File Offset: 0x0001ACFC
		[DataSourceProperty]
		public int SpectatorCount
		{
			get
			{
				return this._spectatorCount;
			}
			set
			{
				if (value != this._spectatorCount)
				{
					this._spectatorCount = value;
					base.OnPropertyChanged("SpectatorCount");
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0001CB19 File Offset: 0x0001AD19
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x0001CB21 File Offset: 0x0001AD21
		[DataSourceProperty]
		public int MaxSpectatorCount
		{
			get
			{
				return this._maxSpectatorCount;
			}
			set
			{
				if (value != this._maxSpectatorCount)
				{
					this._maxSpectatorCount = value;
					base.OnPropertyChanged("MaxSpectatorCount");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0001CB3E File Offset: 0x0001AD3E
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x0001CB46 File Offset: 0x0001AD46
		[DataSourceProperty]
		public string SpectatorCountText
		{
			get
			{
				return this._spectatorCountText;
			}
			set
			{
				if (value != this._spectatorCountText)
				{
					this._spectatorCountText = value;
					base.OnPropertyChanged("SpectatorCountText");
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0001CB68 File Offset: 0x0001AD68
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x0001CB70 File Offset: 0x0001AD70
		[DataSourceProperty]
		public string SpectatorCountHintText
		{
			get
			{
				return this._spectatorCountHintText;
			}
			set
			{
				if (value != this._spectatorCountHintText)
				{
					this._spectatorCountHintText = value;
					base.OnPropertyChanged("SpectatorCountHintText");
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0001CB92 File Offset: 0x0001AD92
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x0001CB9A File Offset: 0x0001AD9A
		[DataSourceProperty]
		public bool ShowSpectatorCount
		{
			get
			{
				return this._showSpectatorCount;
			}
			set
			{
				if (value != this._showSpectatorCount)
				{
					this._showSpectatorCount = value;
					base.OnPropertyChanged("ShowSpectatorCount");
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0001CBB7 File Offset: 0x0001ADB7
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x0001CBBF File Offset: 0x0001ADBF
		[DataSourceProperty]
		public bool EnableSpectators
		{
			get
			{
				return this._enableSpectators;
			}
			set
			{
				if (value != this._enableSpectators)
				{
					this._enableSpectators = value;
					base.OnPropertyChanged("EnableSpectators");
				}
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0001CBDC File Offset: 0x0001ADDC
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x0001CBE4 File Offset: 0x0001ADE4
		[DataSourceProperty]
		public string HostText
		{
			get
			{
				return this._hostText;
			}
			set
			{
				if (value != this._hostText)
				{
					this._hostText = value;
					base.OnPropertyChanged("HostText");
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0001CC06 File Offset: 0x0001AE06
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x0001CC0E File Offset: 0x0001AE0E
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0001CC30 File Offset: 0x0001AE30
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x0001CC38 File Offset: 0x0001AE38
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChanged("GameTypeText");
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0001CC5A File Offset: 0x0001AE5A
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x0001CC62 File Offset: 0x0001AE62
		[DataSourceProperty]
		public string PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (value != this._playerCountText)
				{
					this._playerCountText = value;
					base.OnPropertyChanged("PlayerCountText");
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0001CC84 File Offset: 0x0001AE84
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x0001CC8C File Offset: 0x0001AE8C
		[DataSourceProperty]
		public string PingText
		{
			get
			{
				return this._pingText;
			}
			set
			{
				if (value != this._pingText)
				{
					this._pingText = value;
					base.OnPropertyChanged("PingText");
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0001CCAE File Offset: 0x0001AEAE
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x0001CCB6 File Offset: 0x0001AEB6
		[DataSourceProperty]
		public string FirstFactionName
		{
			get
			{
				return this._firstFactionName;
			}
			set
			{
				if (value != this._firstFactionName)
				{
					this._firstFactionName = value;
					base.OnPropertyChanged("FirstFactionName");
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001CCD8 File Offset: 0x0001AED8
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
		[DataSourceProperty]
		public string SecondFactionName
		{
			get
			{
				return this._secondFactionName;
			}
			set
			{
				if (value != this._secondFactionName)
				{
					this._secondFactionName = value;
					base.OnPropertyChanged("SecondFactionName");
				}
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0001CD02 File Offset: 0x0001AF02
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0001CD0A File Offset: 0x0001AF0A
		[DataSourceProperty]
		public string RegionName
		{
			get
			{
				return this._regionName;
			}
			set
			{
				if (value != this._regionName)
				{
					this._regionName = value;
					base.OnPropertyChanged("RegionName");
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0001CD2C File Offset: 0x0001AF2C
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x0001CD34 File Offset: 0x0001AF34
		[DataSourceProperty]
		public string PremadeMatchTypeText
		{
			get
			{
				return this._premadeMatchTypeText;
			}
			set
			{
				if (value != this._premadeMatchTypeText)
				{
					this._premadeMatchTypeText = value;
					base.OnPropertyChanged("PremadeMatchTypeText");
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0001CD56 File Offset: 0x0001AF56
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x0001CD5E File Offset: 0x0001AF5E
		[DataSourceProperty]
		public BasicTooltipViewModel LoadedModulesHint
		{
			get
			{
				return this._loadedModulesHint;
			}
			set
			{
				if (value != this._loadedModulesHint)
				{
					this._loadedModulesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LoadedModulesHint");
				}
			}
		}

		// Token: 0x04000417 RID: 1047
		public const string PingTimeoutText = "-";

		// Token: 0x04000418 RID: 1048
		private readonly Action<MPCustomGameItemVM> _onSelect;

		// Token: 0x04000419 RID: 1049
		private readonly Action<MPCustomGameItemVM> _onJoin;

		// Token: 0x0400041A RID: 1050
		private readonly Action<MPCustomGameItemVM> _onRequestActions;

		// Token: 0x0400041B RID: 1051
		private readonly Action<MPCustomGameItemVM> _onToggleFavorite;

		// Token: 0x0400041E RID: 1054
		private string _randomString;

		// Token: 0x0400041F RID: 1055
		public static readonly string OfficialServerHostName = "TaleWorlds";

		// Token: 0x04000420 RID: 1056
		private bool _isPasswordProtected;

		// Token: 0x04000421 RID: 1057
		private bool _isFavorite;

		// Token: 0x04000422 RID: 1058
		private bool _isClanMatchItem;

		// Token: 0x04000423 RID: 1059
		private bool _isOfficialServer;

		// Token: 0x04000424 RID: 1060
		private bool _isByOfficialServerProvider;

		// Token: 0x04000425 RID: 1061
		private bool _isCommunityServer;

		// Token: 0x04000426 RID: 1062
		private bool _isPingInfoAvailable;

		// Token: 0x04000427 RID: 1063
		private bool _isSelected;

		// Token: 0x04000428 RID: 1064
		private int _playerCount;

		// Token: 0x04000429 RID: 1065
		private int _maxPlayerCount;

		// Token: 0x0400042A RID: 1066
		private int _spectatorCount;

		// Token: 0x0400042B RID: 1067
		private int _maxSpectatorCount;

		// Token: 0x0400042C RID: 1068
		private string _spectatorCountText;

		// Token: 0x0400042D RID: 1069
		private string _spectatorCountHintText;

		// Token: 0x0400042E RID: 1070
		private bool _showSpectatorCount;

		// Token: 0x0400042F RID: 1071
		private bool _enableSpectators;

		// Token: 0x04000430 RID: 1072
		private string _hostText;

		// Token: 0x04000431 RID: 1073
		private string _nameText;

		// Token: 0x04000432 RID: 1074
		private string _gameTypeText;

		// Token: 0x04000433 RID: 1075
		private string _playerCountText;

		// Token: 0x04000434 RID: 1076
		private string _pingText;

		// Token: 0x04000435 RID: 1077
		private string _firstFactionName;

		// Token: 0x04000436 RID: 1078
		private string _secondFactionName;

		// Token: 0x04000437 RID: 1079
		private string _regionName;

		// Token: 0x04000438 RID: 1080
		private string _premadeMatchTypeText;

		// Token: 0x04000439 RID: 1081
		private BasicTooltipViewModel _loadedModulesHint;
	}
}
