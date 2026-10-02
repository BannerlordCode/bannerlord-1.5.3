using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012E RID: 302
	[Serializable]
	public class GameServerProperties
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0000BE79 File Offset: 0x0000A079
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0000BE81 File Offset: 0x0000A081
		public string Name { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0000BE8A File Offset: 0x0000A08A
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0000BE92 File Offset: 0x0000A092
		public string Address { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0000BE9B File Offset: 0x0000A09B
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0000BEA3 File Offset: 0x0000A0A3
		public int Port { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0000BEB4 File Offset: 0x0000A0B4
		public string Region { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0000BEBD File Offset: 0x0000A0BD
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0000BEC5 File Offset: 0x0000A0C5
		public string GameModule { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0000BECE File Offset: 0x0000A0CE
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0000BED6 File Offset: 0x0000A0D6
		public string GameType { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0000BEDF File Offset: 0x0000A0DF
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0000BEE7 File Offset: 0x0000A0E7
		public string Map { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
		public string UniqueMapId { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0000BF01 File Offset: 0x0000A101
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0000BF09 File Offset: 0x0000A109
		public string GamePassword { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0000BF12 File Offset: 0x0000A112
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0000BF1A File Offset: 0x0000A11A
		public string AdminPassword { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0000BF23 File Offset: 0x0000A123
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0000BF2B File Offset: 0x0000A12B
		public string SpectatorPassword { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0000BF34 File Offset: 0x0000A134
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0000BF3C File Offset: 0x0000A13C
		public int MaxPlayerCount { get; set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0000BF45 File Offset: 0x0000A145
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0000BF4D File Offset: 0x0000A14D
		public int MaxSpectatorCount { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0000BF56 File Offset: 0x0000A156
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0000BF5E File Offset: 0x0000A15E
		public bool EnableSpectators { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0000BF67 File Offset: 0x0000A167
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0000BF6F File Offset: 0x0000A16F
		public bool PasswordProtected { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0000BF78 File Offset: 0x0000A178
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0000BF80 File Offset: 0x0000A180
		public bool IsOfficial { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0000BF89 File Offset: 0x0000A189
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x0000BF91 File Offset: 0x0000A191
		public bool ByOfficialProvider { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0000BF9A File Offset: 0x0000A19A
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0000BFA2 File Offset: 0x0000A1A2
		public bool CrossplayEnabled { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0000BFAB File Offset: 0x0000A1AB
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x0000BFB3 File Offset: 0x0000A1B3
		public int Permission { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		public PlayerId HostId { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0000BFCD File Offset: 0x0000A1CD
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		public string HostName { get; set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0000BFDE File Offset: 0x0000A1DE
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0000BFE6 File Offset: 0x0000A1E6
		public List<ModuleInfoModel> LoadedModules { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0000BFEF File Offset: 0x0000A1EF
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0000BFF7 File Offset: 0x0000A1F7
		public bool AllowsOptionalModules { get; set; }

		// Token: 0x0600080F RID: 2063 RVA: 0x0000C000 File Offset: 0x0000A200
		public GameServerProperties()
		{
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0000C008 File Offset: 0x0000A208
		public GameServerProperties(string name, string address, int port, string region, string gameModule, string gameType, string map, string uniqueMapId, string gamePassword, string adminPassword, string spectatorPassword, int maxPlayerCount, bool isOfficial, bool byOfficialProvider, bool crossplayEnabled, PlayerId hostId, string hostName, List<ModuleInfoModel> loadedModules, bool allowsOptionalModules, int permission, int maxSpectatorCount, bool enableSpectators = false)
		{
			this.Name = name;
			this.Address = address;
			this.Port = port;
			this.Region = region;
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Map = map;
			this.GamePassword = gamePassword;
			this.UniqueMapId = uniqueMapId;
			this.AdminPassword = adminPassword;
			this.SpectatorPassword = spectatorPassword;
			this.MaxPlayerCount = maxPlayerCount;
			this.IsOfficial = isOfficial;
			this.ByOfficialProvider = byOfficialProvider;
			this.CrossplayEnabled = crossplayEnabled;
			this.HostId = hostId;
			this.HostName = hostName;
			this.LoadedModules = loadedModules;
			this.AllowsOptionalModules = allowsOptionalModules;
			this.PasswordProtected = gamePassword != null;
			this.Permission = permission;
			this.MaxSpectatorCount = maxSpectatorCount;
			this.EnableSpectators = enableSpectators;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public void CheckAndReplaceProxyAddress(IReadOnlyDictionary<string, string> proxyAddressMap)
		{
			string text;
			if (proxyAddressMap != null && proxyAddressMap.TryGetValue(this.Address, out text))
			{
				this.Address = text;
			}
		}
	}
}
