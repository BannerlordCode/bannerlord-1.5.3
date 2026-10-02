using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000131 RID: 305
	public interface ILobbyClientSessionHandler
	{
		// Token: 0x0600081A RID: 2074
		void OnConnected();

		// Token: 0x0600081B RID: 2075
		void OnCantConnect();

		// Token: 0x0600081C RID: 2076
		void OnDisconnected(TextObject feedback);

		// Token: 0x0600081D RID: 2077
		void OnPlayerDataReceived(PlayerData playerData);

		// Token: 0x0600081E RID: 2078
		void OnPendingRejoin();

		// Token: 0x0600081F RID: 2079
		void OnBattleResultReceived();

		// Token: 0x06000820 RID: 2080
		void OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation);

		// Token: 0x06000821 RID: 2081
		void OnBattleServerLost();

		// Token: 0x06000822 RID: 2082
		void OnCancelJoiningBattle();

		// Token: 0x06000823 RID: 2083
		void OnRejoinRequestRejected();

		// Token: 0x06000824 RID: 2084
		void OnFindGameAnswer(bool successful, string[] selectedAndDisabledGameTypes, bool isRejoin);

		// Token: 0x06000825 RID: 2085
		void OnEnterBattleWithPartyAnswer(string[] selectedGameTypes);

		// Token: 0x06000826 RID: 2086
		void OnWhisperMessageReceived(string fromPlayer, string toPlayer, string message);

		// Token: 0x06000827 RID: 2087
		void OnClanMessageReceived(string playerName, string message);

		// Token: 0x06000828 RID: 2088
		void OnPartyMessageReceived(string playerName, string message);

		// Token: 0x06000829 RID: 2089
		void OnSystemMessageReceived(string message);

		// Token: 0x0600082A RID: 2090
		void OnAdminMessageReceived(string message);

		// Token: 0x0600082B RID: 2091
		void OnGameClientStateChange(LobbyClient.State oldState);

		// Token: 0x0600082C RID: 2092
		void OnCustomGameServerListReceived(AvailableCustomGames customGameServerList);

		// Token: 0x0600082D RID: 2093
		void OnPartyInvitationReceived(string inviterPlayerName, PlayerId inviterPlayerId);

		// Token: 0x0600082E RID: 2094
		void OnPartyJoinRequestReceived(PlayerId playerId, PlayerId viaPlayerId, string viaFriendName);

		// Token: 0x0600082F RID: 2095
		void OnPartyInvitationInvalidated();

		// Token: 0x06000830 RID: 2096
		void OnPlayerInvitedToParty(PlayerId playerId);

		// Token: 0x06000831 RID: 2097
		void OnPlayersAddedToParty([TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })] List<ValueTuple<PlayerId, string, bool>> addedPlayers, [TupleElementNames(new string[] { "PlayerId", "PlayerName" })] List<ValueTuple<PlayerId, string>> invitedPlayers);

		// Token: 0x06000832 RID: 2098
		void OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason);

		// Token: 0x06000833 RID: 2099
		void OnPlayerAssignedPartyLeader(PlayerId partyLeaderId);

		// Token: 0x06000834 RID: 2100
		void OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName);

		// Token: 0x06000835 RID: 2101
		void OnServerStatusReceived(ServerStatus serverStatus);

		// Token: 0x06000836 RID: 2102
		void OnSigilChanged();

		// Token: 0x06000837 RID: 2103
		void OnFriendListReceived(FriendInfo[] friends);

		// Token: 0x06000838 RID: 2104
		void OnRecentPlayerStatusesReceived(FriendInfo[] friends);

		// Token: 0x06000839 RID: 2105
		void OnNotificationsReceived(LobbyNotification[] notifications);

		// Token: 0x0600083A RID: 2106
		void OnClanInvitationReceived(string clanName, string clanTag, bool isCreation);

		// Token: 0x0600083B RID: 2107
		void OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer);

		// Token: 0x0600083C RID: 2108
		void OnClanCreationSuccessful();

		// Token: 0x0600083D RID: 2109
		void OnClanCreationFailed();

		// Token: 0x0600083E RID: 2110
		void OnClanCreationStarted();

		// Token: 0x0600083F RID: 2111
		void OnClanInfoChanged();

		// Token: 0x06000840 RID: 2112
		void OnPremadeGameEligibilityStatusReceived(bool isEligible);

		// Token: 0x06000841 RID: 2113
		void OnPremadeGameCreated();

		// Token: 0x06000842 RID: 2114
		void OnPremadeGameListReceived();

		// Token: 0x06000843 RID: 2115
		void OnPremadeGameCreationCancelled();

		// Token: 0x06000844 RID: 2116
		void OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType);

		// Token: 0x06000845 RID: 2117
		void OnJoinPremadeGameRequestSuccessful();

		// Token: 0x06000846 RID: 2118
		void OnQuitFromMatchmakerGame();

		// Token: 0x06000847 RID: 2119
		void OnMatchmakerGameOver(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason);

		// Token: 0x06000848 RID: 2120
		void OnRemovedFromMatchmakerGame(DisconnectType disconnectType);

		// Token: 0x06000849 RID: 2121
		void OnRejoinBattleRequestAnswered(bool isSuccessful);

		// Token: 0x0600084A RID: 2122
		void OnRegisterCustomGameServerResponse();

		// Token: 0x0600084B RID: 2123
		void OnCustomGameEnd();

		// Token: 0x0600084C RID: 2124
		PlayerJoinGameResponseDataFromHost[] OnClientWantsToConnectCustomGame(PlayerJoinGameData[] playerJoinData);

		// Token: 0x0600084D RID: 2125
		void OnClientQuitFromCustomGame(PlayerId playerId);

		// Token: 0x0600084E RID: 2126
		void OnJoinCustomGameResponse(bool success, JoinGameData joinGameData, CustomGameJoinResponse failureReason, CustomGameJoinType joinType);

		// Token: 0x0600084F RID: 2127
		void OnJoinCustomGameFailureResponse(CustomGameJoinResponse response);

		// Token: 0x06000850 RID: 2128
		void OnQuitFromCustomGame();

		// Token: 0x06000851 RID: 2129
		void OnRemovedFromCustomGame(DisconnectType disconnectType);

		// Token: 0x06000852 RID: 2130
		void OnAnnouncementReceived(Announcement announcement);

		// Token: 0x06000853 RID: 2131
		Task<bool> OnInviteToPlatformSession(PlayerId playerId);

		// Token: 0x06000854 RID: 2132
		void OnEnterCustomBattleWithPartyAnswer();
	}
}
