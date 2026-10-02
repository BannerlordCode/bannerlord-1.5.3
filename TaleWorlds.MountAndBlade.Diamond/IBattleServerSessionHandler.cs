using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000120 RID: 288
	public interface IBattleServerSessionHandler
	{
		// Token: 0x06000680 RID: 1664
		void OnConnected();

		// Token: 0x06000681 RID: 1665
		void OnCantConnect();

		// Token: 0x06000682 RID: 1666
		void OnDisconnected();

		// Token: 0x06000683 RID: 1667
		void OnNewPlayer(BattlePeer peer);

		// Token: 0x06000684 RID: 1668
		void OnStartGame(string sceneName, string gameType, string faction1, string faction2, int minRequiredPlayerCountToStartBattle, int battleSize, string[] profanityList, string[] allowList);

		// Token: 0x06000685 RID: 1669
		void OnPlayerFledBattle(BattlePeer peer, out BattleResult battleResult, bool isQuitFromBattle);

		// Token: 0x06000686 RID: 1670
		void OnEndMission();

		// Token: 0x06000687 RID: 1671
		void OnStopServer();
	}
}
