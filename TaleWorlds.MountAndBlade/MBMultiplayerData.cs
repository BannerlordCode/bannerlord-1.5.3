using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D6 RID: 470
	public class MBMultiplayerData
	{
		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x0006163C File Offset: 0x0005F83C
		// (set) Token: 0x06001C31 RID: 7217 RVA: 0x00061643 File Offset: 0x0005F843
		public static Guid ServerId { get; set; }

		// Token: 0x06001C32 RID: 7218 RVA: 0x0006164C File Offset: 0x0005F84C
		[MBCallback(null, false)]
		public static string GetServerId()
		{
			return MBMultiplayerData.ServerId.ToString();
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0006166C File Offset: 0x0005F86C
		[MBCallback(null, false)]
		public static string GetServerName()
		{
			return MBMultiplayerData.ServerName;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00061673 File Offset: 0x0005F873
		[MBCallback(null, false)]
		public static string GetGameModule()
		{
			return MBMultiplayerData.GameModule;
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0006167A File Offset: 0x0005F87A
		[MBCallback(null, false)]
		public static string GetGameType()
		{
			return MBMultiplayerData.GameType;
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00061681 File Offset: 0x0005F881
		[MBCallback(null, false)]
		public static string GetMap()
		{
			return MBMultiplayerData.Map;
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00061688 File Offset: 0x0005F888
		[MBCallback(null, false)]
		public static int GetCurrentPlayerCount()
		{
			return GameNetwork.NetworkPeerCount;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0006168F File Offset: 0x0005F88F
		[MBCallback(null, false)]
		public static int GetPlayerCountLimit()
		{
			return MBMultiplayerData.PlayerCountLimit;
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001C39 RID: 7225 RVA: 0x00061698 File Offset: 0x0005F898
		// (remove) Token: 0x06001C3A RID: 7226 RVA: 0x000616CC File Offset: 0x0005F8CC
		public static event MBMultiplayerData.GameServerInfoReceivedDelegate GameServerInfoReceived;

		// Token: 0x06001C3B RID: 7227 RVA: 0x00061700 File Offset: 0x0005F900
		[MBCallback(null, false)]
		public static void UpdateGameServerInfo(string id, string gameServer, string gameModule, string gameType, string map, int currentPlayerCount, int maxPlayerCount, string address, int port)
		{
			if (MBMultiplayerData.GameServerInfoReceived != null)
			{
				MBMultiplayerData.GameServerInfoReceived(new CustomBattleId(Guid.Parse(id)), gameServer, gameModule, gameType, map, currentPlayerCount, maxPlayerCount, address, port);
			}
		}

		// Token: 0x04000939 RID: 2361
		public static string ServerName;

		// Token: 0x0400093A RID: 2362
		public static string GameModule;

		// Token: 0x0400093B RID: 2363
		public static string GameType;

		// Token: 0x0400093C RID: 2364
		public static string Map;

		// Token: 0x0400093D RID: 2365
		public static int PlayerCountLimit;

		// Token: 0x02000514 RID: 1300
		// (Invoke) Token: 0x06003C97 RID: 15511
		public delegate void GameServerInfoReceivedDelegate(CustomBattleId id, string gameServer, string gameModule, string gameType, string map, int currentPlayerCount, int maxPlayerCount, string address, int port);
	}
}
