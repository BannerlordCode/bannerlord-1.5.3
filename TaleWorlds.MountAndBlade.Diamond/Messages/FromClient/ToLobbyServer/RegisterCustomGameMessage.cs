using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BA RID: 186
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RegisterCustomGameMessage : Message
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000443B File Offset: 0x0000263B
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00004443 File Offset: 0x00002643
		[JsonProperty]
		public string GameModule { get; private set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000444C File Offset: 0x0000264C
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00004454 File Offset: 0x00002654
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000445D File Offset: 0x0000265D
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00004465 File Offset: 0x00002665
		[JsonProperty]
		public string ServerName { get; private set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000446E File Offset: 0x0000266E
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00004476 File Offset: 0x00002676
		[JsonProperty]
		public int MaxPlayerCount { get; private set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000447F File Offset: 0x0000267F
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00004487 File Offset: 0x00002687
		[JsonProperty]
		public string Map { get; private set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00004490 File Offset: 0x00002690
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00004498 File Offset: 0x00002698
		[JsonProperty]
		public string UniqueMapId { get; private set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600035A RID: 858 RVA: 0x000044A1 File Offset: 0x000026A1
		// (set) Token: 0x0600035B RID: 859 RVA: 0x000044A9 File Offset: 0x000026A9
		[JsonProperty]
		public int Port { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600035C RID: 860 RVA: 0x000044B2 File Offset: 0x000026B2
		// (set) Token: 0x0600035D RID: 861 RVA: 0x000044BA File Offset: 0x000026BA
		[JsonProperty]
		public string GamePassword { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600035E RID: 862 RVA: 0x000044C3 File Offset: 0x000026C3
		// (set) Token: 0x0600035F RID: 863 RVA: 0x000044CB File Offset: 0x000026CB
		[JsonProperty]
		public string AdminPassword { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000044D4 File Offset: 0x000026D4
		// (set) Token: 0x06000361 RID: 865 RVA: 0x000044DC File Offset: 0x000026DC
		[JsonProperty]
		public string SpectatorPassword { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000362 RID: 866 RVA: 0x000044E5 File Offset: 0x000026E5
		// (set) Token: 0x06000363 RID: 867 RVA: 0x000044ED File Offset: 0x000026ED
		[JsonProperty]
		public int MaxSpectatorCount { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000364 RID: 868 RVA: 0x000044F6 File Offset: 0x000026F6
		// (set) Token: 0x06000365 RID: 869 RVA: 0x000044FE File Offset: 0x000026FE
		[JsonProperty]
		public bool EnableSpectators { get; private set; }

		// Token: 0x06000366 RID: 870 RVA: 0x00004507 File Offset: 0x00002707
		public RegisterCustomGameMessage()
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00004510 File Offset: 0x00002710
		public RegisterCustomGameMessage(string gameModule, string gameType, string serverName, int maxPlayerCount, string map, string uniqueMapId, string gamePassword, string adminPassword, string spectatorPassword, int port, int maxSpectatorCount, bool enableSpectators)
		{
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.ServerName = serverName;
			this.MaxPlayerCount = maxPlayerCount;
			this.Map = map;
			this.UniqueMapId = uniqueMapId;
			this.GamePassword = gamePassword;
			this.AdminPassword = adminPassword;
			this.SpectatorPassword = spectatorPassword;
			this.Port = port;
			this.MaxSpectatorCount = maxSpectatorCount;
			this.EnableSpectators = enableSpectators;
		}
	}
}
