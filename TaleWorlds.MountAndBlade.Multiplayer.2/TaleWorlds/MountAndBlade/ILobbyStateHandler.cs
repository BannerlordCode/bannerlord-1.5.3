using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000014 RID: 20
	public interface ILobbyStateHandler
	{
		// Token: 0x06000114 RID: 276
		void SetConnectionState(bool isAuthenticated);

		// Token: 0x06000115 RID: 277
		string ShowFeedback(string title, string feedbackText);

		// Token: 0x06000116 RID: 278
		string ShowFeedback(InquiryData inquiryData);

		// Token: 0x06000117 RID: 279
		void DismissFeedback(string id);

		// Token: 0x06000118 RID: 280
		void OnPause();

		// Token: 0x06000119 RID: 281
		void OnResume();

		// Token: 0x0600011A RID: 282
		void OnDisconnected();

		// Token: 0x0600011B RID: 283
		void OnRequestedToSearchBattle();

		// Token: 0x0600011C RID: 284
		void OnUpdateFindingGame(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo);

		// Token: 0x0600011D RID: 285
		void OnRequestedToCancelSearchBattle();

		// Token: 0x0600011E RID: 286
		void OnSearchBattleCanceled();

		// Token: 0x0600011F RID: 287
		void OnPlayerDataReceived(PlayerData playerData);

		// Token: 0x06000120 RID: 288
		void OnPendingRejoin();

		// Token: 0x06000121 RID: 289
		void OnEnterBattleWithParty(string[] selectedGameTypes);

		// Token: 0x06000122 RID: 290
		void OnPartyInvitationReceived(PlayerId playerId);

		// Token: 0x06000123 RID: 291
		void OnPartyJoinRequestReceived(PlayerId joingPlayerId, PlayerId viaPlayerId, string viaPlayerName, bool newParty);

		// Token: 0x06000124 RID: 292
		void OnPartyInvitationInvalidated();

		// Token: 0x06000125 RID: 293
		void OnPlayerInvitedToParty(PlayerId playerId);

		// Token: 0x06000126 RID: 294
		void OnPlayerAddedToParty(PlayerId playerId, string playerName, bool isPartyLeader);

		// Token: 0x06000127 RID: 295
		void OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason);

		// Token: 0x06000128 RID: 296
		void OnPlayerNameUpdated(string newName);

		// Token: 0x06000129 RID: 297
		void OnGameClientStateChange(LobbyClient.State state);

		// Token: 0x0600012A RID: 298
		void OnAdminMessageReceived(string message);

		// Token: 0x0600012B RID: 299
		void OnActivateHome();

		// Token: 0x0600012C RID: 300
		void OnActivateCustomServer();

		// Token: 0x0600012D RID: 301
		void OnActivateMatchmaking();

		// Token: 0x0600012E RID: 302
		void OnActivateArmory();

		// Token: 0x0600012F RID: 303
		void OnActivateOptions();

		// Token: 0x06000130 RID: 304
		void OnDeactivateOptions();

		// Token: 0x06000131 RID: 305
		void OnCustomGameServerListReceived(AvailableCustomGames customGameServerList);

		// Token: 0x06000132 RID: 306
		void OnMatchmakerGameOver(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason);

		// Token: 0x06000133 RID: 307
		void OnBattleServerLost();

		// Token: 0x06000134 RID: 308
		void OnRemovedFromMatchmakerGame(DisconnectType disconnectType);

		// Token: 0x06000135 RID: 309
		void OnRemovedFromCustomGame(DisconnectType disconnectType);

		// Token: 0x06000136 RID: 310
		void OnPlayerAssignedPartyLeader(PlayerId partyLeaderId);

		// Token: 0x06000137 RID: 311
		void OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName);

		// Token: 0x06000138 RID: 312
		void OnJoinCustomGameFailureResponse(CustomGameJoinResponse response);

		// Token: 0x06000139 RID: 313
		void OnRejoinBattleRequestAnswered(bool isSuccessful);

		// Token: 0x0600013A RID: 314
		void OnServerStatusReceived(ServerStatus serverStatus);

		// Token: 0x0600013B RID: 315
		void OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation);

		// Token: 0x0600013C RID: 316
		void OnActivateProfile();

		// Token: 0x0600013D RID: 317
		void OnClanInvitationReceived(string clanName, string clanTag, bool isCreation);

		// Token: 0x0600013E RID: 318
		void OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer);

		// Token: 0x0600013F RID: 319
		void OnClanCreationSuccessful();

		// Token: 0x06000140 RID: 320
		void OnClanCreationFailed();

		// Token: 0x06000141 RID: 321
		void OnClanCreationStarted();

		// Token: 0x06000142 RID: 322
		void OnClanInfoChanged();

		// Token: 0x06000143 RID: 323
		void OnPremadeGameEligibilityStatusReceived(bool isEligible);

		// Token: 0x06000144 RID: 324
		void OnPremadeGameCreated();

		// Token: 0x06000145 RID: 325
		void OnPremadeGameListReceived();

		// Token: 0x06000146 RID: 326
		void OnPremadeGameCreationCancelled();

		// Token: 0x06000147 RID: 327
		void OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType);

		// Token: 0x06000148 RID: 328
		void OnJoinPremadeGameRequestSuccessful();

		// Token: 0x06000149 RID: 329
		void OnSigilChanged();

		// Token: 0x0600014A RID: 330
		void OnNotificationsReceived(LobbyNotification[] notifications);

		// Token: 0x0600014B RID: 331
		void OnFriendListUpdated();
	}
}
