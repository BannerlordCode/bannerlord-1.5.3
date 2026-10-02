using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000064 RID: 100
	public static class MultiplayerReportPlayerManager
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002F8 RID: 760 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		// (remove) Token: 0x060002F9 RID: 761 RVA: 0x0000DA28 File Offset: 0x0000BC28
		public static event Action<string, PlayerId, string, bool> ReportHandlers;

		// Token: 0x060002FA RID: 762 RVA: 0x0000DA5B File Offset: 0x0000BC5B
		public static void RequestReportPlayer(string gameId, PlayerId playerId, string playerName, bool isRequestedFromMission)
		{
			Action<string, PlayerId, string, bool> reportHandlers = MultiplayerReportPlayerManager.ReportHandlers;
			if (reportHandlers == null)
			{
				return;
			}
			reportHandlers(gameId, playerId, playerName, isRequestedFromMission);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DA70 File Offset: 0x0000BC70
		public static void OnPlayerReported(PlayerId playerId)
		{
			MultiplayerReportPlayerManager.IncrementReportOfPlayer(playerId);
			NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.Find((NetworkCommunicator x) => x.VirtualPlayer.Id == playerId);
			if (networkCommunicator != null)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SetMuted(true);
				}
			}
			Game.Current.GetGameHandler<ChatBox>().SetPlayerMuted(playerId, true);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DAD8 File Offset: 0x0000BCD8
		public static bool IsPlayerReportedOverLimit(PlayerId player)
		{
			int num;
			return MultiplayerReportPlayerManager._reportsPerPlayer.TryGetValue(player, out num) && num == 3;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		private static void IncrementReportOfPlayer(PlayerId player)
		{
			if (MultiplayerReportPlayerManager._reportsPerPlayer.ContainsKey(player))
			{
				Dictionary<PlayerId, int> reportsPerPlayer = MultiplayerReportPlayerManager._reportsPerPlayer;
				int num = reportsPerPlayer[player];
				reportsPerPlayer[player] = num + 1;
				return;
			}
			MultiplayerReportPlayerManager._reportsPerPlayer.Add(player, 1);
		}

		// Token: 0x040000F0 RID: 240
		private static Dictionary<PlayerId, int> _reportsPerPlayer = new Dictionary<PlayerId, int>();

		// Token: 0x040000F1 RID: 241
		private const int _maxReportsPerPlayer = 3;
	}
}
