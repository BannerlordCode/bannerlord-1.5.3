using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000B RID: 11
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", true)]
	[Serializable]
	public class RegisterCustomGameMessage : Message
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000023DF File Offset: 0x000005DF
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000023E7 File Offset: 0x000005E7
		[JsonProperty]
		public int GameDefinitionId { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000023F0 File Offset: 0x000005F0
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000023F8 File Offset: 0x000005F8
		[JsonProperty]
		public string GameModule { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002401 File Offset: 0x00000601
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002409 File Offset: 0x00000609
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002412 File Offset: 0x00000612
		// (set) Token: 0x06000046 RID: 70 RVA: 0x0000241A File Offset: 0x0000061A
		[JsonProperty]
		public string ServerName { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002423 File Offset: 0x00000623
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000242B File Offset: 0x0000062B
		[JsonProperty]
		public string ServerAddress { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002434 File Offset: 0x00000634
		// (set) Token: 0x0600004A RID: 74 RVA: 0x0000243C File Offset: 0x0000063C
		[JsonProperty]
		public int MaxPlayerCount { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002445 File Offset: 0x00000645
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000244D File Offset: 0x0000064D
		[JsonProperty]
		public string Map { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002456 File Offset: 0x00000656
		// (set) Token: 0x0600004E RID: 78 RVA: 0x0000245E File Offset: 0x0000065E
		[JsonProperty]
		public string UniqueMapId { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002467 File Offset: 0x00000667
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000246F File Offset: 0x0000066F
		[JsonProperty]
		public string GamePassword { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002478 File Offset: 0x00000678
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002480 File Offset: 0x00000680
		[JsonProperty]
		public string AdminPassword { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002489 File Offset: 0x00000689
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002491 File Offset: 0x00000691
		[JsonProperty]
		public string SpectatorPassword { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000249A File Offset: 0x0000069A
		// (set) Token: 0x06000056 RID: 86 RVA: 0x000024A2 File Offset: 0x000006A2
		[JsonProperty]
		public int Port { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000024AB File Offset: 0x000006AB
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000024B3 File Offset: 0x000006B3
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000024BC File Offset: 0x000006BC
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000024C4 File Offset: 0x000006C4
		[JsonProperty]
		public int Permission { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000024CD File Offset: 0x000006CD
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000024D5 File Offset: 0x000006D5
		[JsonProperty]
		public bool IsOverridingIP { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000024DE File Offset: 0x000006DE
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000024E6 File Offset: 0x000006E6
		[JsonProperty]
		public bool CrossplayEnabled { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000024EF File Offset: 0x000006EF
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000024F7 File Offset: 0x000006F7
		[JsonProperty]
		public int MaxSpectatorCount { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002500 File Offset: 0x00000700
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002508 File Offset: 0x00000708
		[JsonProperty]
		public bool EnableSpectators { get; private set; }

		// Token: 0x06000063 RID: 99 RVA: 0x00002511 File Offset: 0x00000711
		public RegisterCustomGameMessage()
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000251C File Offset: 0x0000071C
		public RegisterCustomGameMessage(int gameDefinitionId, string gameModule, string gameType, string serverName, string serverAddress, int maxPlayerCount, string map, string uniqueMapId, string gamePassword, string adminPassword, string spectatorPassword, int port, string region, int permission, bool crossplayEnabled, bool isOverridingIP, int maxSpectatorCount, bool enableSpectators)
		{
			this.GameDefinitionId = gameDefinitionId;
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.ServerName = serverName;
			this.ServerAddress = serverAddress;
			this.MaxPlayerCount = maxPlayerCount;
			this.Map = map;
			this.UniqueMapId = uniqueMapId;
			this.GamePassword = gamePassword;
			this.AdminPassword = adminPassword;
			this.SpectatorPassword = spectatorPassword;
			this.Port = port;
			this.Region = region;
			this.Permission = permission;
			this.CrossplayEnabled = crossplayEnabled;
			this.IsOverridingIP = isOverridingIP;
			this.MaxSpectatorCount = maxSpectatorCount;
			this.EnableSpectators = enableSpectators;
		}
	}
}
