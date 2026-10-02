using System;
using System.Threading.Tasks;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000121 RID: 289
	public interface ICustomBattleServerSessionHandler
	{
		// Token: 0x06000688 RID: 1672
		void OnConnected();

		// Token: 0x06000689 RID: 1673
		void OnCantConnect();

		// Token: 0x0600068A RID: 1674
		void OnDisconnected();

		// Token: 0x0600068B RID: 1675
		void OnStateChanged(CustomBattleServer.State state);

		// Token: 0x0600068C RID: 1676
		void OnSuccessfulGameRegister();

		// Token: 0x0600068D RID: 1677
		Task<PlayerJoinGameResponseDataFromHost[]> OnClientWantsToConnectCustomGame(PlayerJoinGameData[] playerJoinData);

		// Token: 0x0600068E RID: 1678
		void OnClientQuitFromCustomGame(PlayerId playerId);

		// Token: 0x0600068F RID: 1679
		void OnGameFinished();

		// Token: 0x06000690 RID: 1680
		void OnChatFilterListsReceived(string[] profanityList, string[] allowList);

		// Token: 0x06000691 RID: 1681
		void OnPlayerKickRequested(PlayerId playerID, bool isBanning);
	}
}
