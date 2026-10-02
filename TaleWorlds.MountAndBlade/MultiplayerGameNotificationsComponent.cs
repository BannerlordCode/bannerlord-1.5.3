using System;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C0 RID: 704
	public class MultiplayerGameNotificationsComponent : MissionNetwork
	{
		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x0009964F File Offset: 0x0009784F
		public static int NotificationCount
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x00099653 File Offset: 0x00097853
		public void WarmupEnding()
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleWarmupEnding, 30, -1, null, null);
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00099664 File Offset: 0x00097864
		public void GameOver(Team winnerTeam)
		{
			if (winnerTeam == null)
			{
				this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDraw, -1, -1, null, null);
				return;
			}
			Team team = ((winnerTeam.Side == BattleSideEnum.Attacker) ? base.Mission.Teams.Defender : base.Mission.Teams.Attacker);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverVictory, -1, -1, winnerTeam, null);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDefeat, -1, -1, team, null);
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000996C2 File Offset: 0x000978C2
		public void PreparationStarted()
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattlePreparationStart, -1, -1, null, null);
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000996D0 File Offset: 0x000978D0
		public void FlagsXRemoved(FlagCapturePoint removedFlag)
		{
			int flagChar = removedFlag.FlagChar;
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemoved, flagChar, -1, null, null);
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000996F0 File Offset: 0x000978F0
		public void FlagXRemaining(FlagCapturePoint remainingFlag)
		{
			int flagChar = remainingFlag.FlagChar;
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemaining, flagChar, -1, null, null);
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x0009970F File Offset: 0x0009790F
		public void FlagsWillBeRemovedInXSeconds(int timeLeft)
		{
			this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagsWillBeRemoved, new int[] { timeLeft });
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x00099724 File Offset: 0x00097924
		public void FlagXCapturedByTeamX(SynchedMissionObject flag, Team capturingTeam)
		{
			FlagCapturePoint flagCapturePoint = flag as FlagCapturePoint;
			int num = ((flagCapturePoint != null) ? flagCapturePoint.FlagChar : 65);
			Team team = ((capturingTeam.Side == BattleSideEnum.Attacker) ? base.Mission.Teams.Defender : base.Mission.Teams.Attacker);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam, num, -1, capturingTeam, null);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam, num, -1, team, null);
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x0009978C File Offset: 0x0009798C
		public void GoldCarriedFromPreviousRound(int carriedGoldAmount, NetworkCommunicator syncToPeer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GoldCarriedFromPreviousRound, carriedGoldAmount, -1, null, syncToPeer);
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000997A7 File Offset: 0x000979A7
		public void PlayerIsInactive(NetworkCommunicator peer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsInactive, -1, -1, null, peer);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000997B5 File Offset: 0x000979B5
		public void FormationAutoFollowEnforced(NetworkCommunicator peer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FormationAutoFollowEnforced, -1, -1, null, peer);
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000997C4 File Offset: 0x000979C4
		public void PollRejected(MultiplayerPollRejectReason reason)
		{
			if (reason == MultiplayerPollRejectReason.TooManyPollRequests)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.TooManyPollRequests, Array.Empty<int>());
				return;
			}
			if (reason == MultiplayerPollRejectReason.HasOngoingPoll)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.HasOngoingPoll, Array.Empty<int>());
				return;
			}
			if (reason == MultiplayerPollRejectReason.NotEnoughPlayersToOpenPoll)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.NotEnoughPlayersToOpenPoll, new int[] { 3 });
				return;
			}
			if (reason == MultiplayerPollRejectReason.KickPollTargetNotSynced)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.KickPollTargetNotSynced, Array.Empty<int>());
				return;
			}
			Debug.FailedAssert("Notification of a PollRejectReason is missing (" + reason + ")", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameNotificationsComponent.cs", "PollRejected", 153);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x00099846 File Offset: 0x00097A46
		public void PlayerKicked(NetworkCommunicator kickedPeer)
		{
			this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsKicked, new int[] { kickedPeer.Index });
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0009985F File Offset: 0x00097A5F
		private void HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum notification, int param1 = -1, int param2 = -1, Team syncToTeam = null, NetworkCommunicator syncToPeer = null)
		{
			if (syncToPeer != null)
			{
				this.SendNotificationToPeer(syncToPeer, notification, param1, param2);
				return;
			}
			if (syncToTeam != null)
			{
				this.SendNotificationToTeam(syncToTeam, notification, param1, param2);
				return;
			}
			this.SendNotificationToEveryone(notification, param1, param2);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0009988C File Offset: 0x00097A8C
		private void ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum notification, params int[] parameters)
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				NotificationProperty notificationProperty = (NotificationProperty)notification.GetType().GetField(notification.ToString()).GetCustomAttributesSafe(typeof(NotificationProperty), false)
					.Single<object>();
				if (notificationProperty != null)
				{
					int[] array = parameters.Where<int>((int x) => x != -1).ToArray<int>();
					TextObject textObject = this.ToNotificationString(notification, notificationProperty, array);
					string text = this.ToSoundString(notification, notificationProperty, array);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, text);
				}
			}
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x00099925 File Offset: 0x00097B25
		private void SendNotificationToEveryone(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			this.ShowNotification(message, new int[] { param1, param2 });
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x00099955 File Offset: 0x00097B55
		private void SendNotificationToPeer(NetworkCommunicator peer, MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			if (peer.IsServerPeer)
			{
				this.ShowNotification(message, new int[] { param1, param2 });
				return;
			}
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00099990 File Offset: 0x00097B90
		private void SendNotificationToTeam(Team team, MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (!GameNetwork.IsDedicatedServer && ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsEnemyOf(team))
			{
				this.ShowNotification(message, new int[] { param1, param2 });
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && !component.IsMine && !component.Team.IsEnemyOf(team))
				{
					GameNetwork.BeginModuleEventAsServer(component.Peer);
					GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x00099A70 File Offset: 0x00097C70
		private string ToSoundString(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum value, NotificationProperty attribute, params int[] parameters)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(attribute.SoundIdTwo))
			{
				text = attribute.SoundIdOne;
			}
			else if (value != MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleYouHaveXTheRound)
			{
				if (value != MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam)
				{
					if (value == MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam)
					{
						text = attribute.SoundIdTwo;
					}
				}
				else
				{
					text = attribute.SoundIdOne;
				}
			}
			else
			{
				Team team = ((parameters[0] == 0) ? Mission.Current.AttackerTeam : Mission.Current.DefenderTeam);
				Team team2 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
				text = attribute.SoundIdOne;
				if (team2 != null && team2 != team)
				{
					text = attribute.SoundIdTwo;
				}
			}
			return text;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x00099B07 File Offset: 0x00097D07
		private TextObject ToNotificationString(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum value, NotificationProperty attribute, params int[] parameters)
		{
			if (parameters.Length != 0)
			{
				this.SetGameTextVariables(value, parameters);
			}
			return GameTexts.FindText(attribute.StringId, null);
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x00099B24 File Offset: 0x00097D24
		private void SetGameTextVariables(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, params int[] parameters)
		{
			if (parameters.Length == 0)
			{
				return;
			}
			switch (message)
			{
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleWarmupEnding:
				GameTexts.SetVariable("SECONDS_LEFT", parameters[0]);
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattlePreparationStart:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDraw:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverVictory:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDefeat:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsInactive:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.HasOngoingPoll:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.TooManyPollRequests:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.KickPollTargetNotSynced:
				break;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleYouHaveXTheRound:
			{
				Team team = ((parameters[0] == 0) ? Mission.Current.AttackerTeam : Mission.Current.DefenderTeam);
				Team team2 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
				if (team2 != null)
				{
					GameTexts.SetVariable("IS_WINNER", (team2 == team) ? 1 : 0);
					return;
				}
				break;
			}
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemoved:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemaining:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagsWillBeRemoved:
				GameTexts.SetVariable("PARAM1", parameters[0]);
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GoldCarriedFromPreviousRound:
				GameTexts.SetVariable("PARAM1", parameters[0].ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.NotEnoughPlayersToOpenPoll:
				GameTexts.SetVariable("MIN_PARTICIPANT_COUNT", parameters[0]);
				break;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsKicked:
				GameTexts.SetVariable("PLAYER_NAME", GameNetwork.FindNetworkPeer(parameters[0]).UserName);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x00099C71 File Offset: 0x00097E71
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<NotificationMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventServerMessage));
			}
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x00099C8C File Offset: 0x00097E8C
		private void HandleServerEventServerMessage(GameNetworkMessage baseMessage)
		{
			NotificationMessage notificationMessage = (NotificationMessage)baseMessage;
			this.ShowNotification((MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum)notificationMessage.Message, new int[] { notificationMessage.ParameterOne, notificationMessage.ParameterTwo });
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x00099CC4 File Offset: 0x00097EC4
		protected override void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			bool isServerPeer = clientConnectionInfo.NetworkPeer.IsServerPeer;
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x00099CD2 File Offset: 0x00097ED2
		protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			bool isServer = GameNetwork.IsServer;
		}

		// Token: 0x020005AB RID: 1451
		private enum MultiplayerNotificationEnum
		{
			// Token: 0x04001F23 RID: 7971
			[NotificationProperty("str_battle_warmup_ending_in_x_seconds", "event:/ui/mission/multiplayer/lastmanstanding", "")]
			BattleWarmupEnding,
			// Token: 0x04001F24 RID: 7972
			[NotificationProperty("str_battle_preparation_start", "event:/ui/mission/multiplayer/roundstart", "")]
			BattlePreparationStart,
			// Token: 0x04001F25 RID: 7973
			[NotificationProperty("str_round_result_win_lose", "event:/ui/mission/multiplayer/victory", "event:/ui/mission/multiplayer/defeat")]
			BattleYouHaveXTheRound,
			// Token: 0x04001F26 RID: 7974
			[NotificationProperty("str_mp_mission_game_over_draw", "", "")]
			GameOverDraw,
			// Token: 0x04001F27 RID: 7975
			[NotificationProperty("str_mp_mission_game_over_victory", "", "")]
			GameOverVictory,
			// Token: 0x04001F28 RID: 7976
			[NotificationProperty("str_mp_mission_game_over_defeat", "", "")]
			GameOverDefeat,
			// Token: 0x04001F29 RID: 7977
			[NotificationProperty("str_mp_flag_removed", "event:/ui/mission/multiplayer/pointsremoved", "")]
			FlagXRemoved,
			// Token: 0x04001F2A RID: 7978
			[NotificationProperty("str_sergeant_a_one_flag_remaining", "event:/ui/mission/multiplayer/pointsremoved", "")]
			FlagXRemaining,
			// Token: 0x04001F2B RID: 7979
			[NotificationProperty("str_sergeant_a_flags_will_be_removed", "event:/ui/mission/multiplayer/pointwarning", "")]
			FlagsWillBeRemoved,
			// Token: 0x04001F2C RID: 7980
			[NotificationProperty("str_sergeant_a_flag_captured_by_your_team", "event:/ui/mission/multiplayer/pointcapture", "event:/ui/mission/multiplayer/pointlost")]
			FlagXCapturedByYourTeam,
			// Token: 0x04001F2D RID: 7981
			[NotificationProperty("str_sergeant_a_flag_captured_by_other_team", "event:/ui/mission/multiplayer/pointcapture", "event:/ui/mission/multiplayer/pointlost")]
			FlagXCapturedByOtherTeam,
			// Token: 0x04001F2E RID: 7982
			[NotificationProperty("str_gold_carried_from_previous_round", "", "")]
			GoldCarriedFromPreviousRound,
			// Token: 0x04001F2F RID: 7983
			[NotificationProperty("str_player_is_inactive", "", "")]
			PlayerIsInactive,
			// Token: 0x04001F30 RID: 7984
			[NotificationProperty("str_has_ongoing_poll", "", "")]
			HasOngoingPoll,
			// Token: 0x04001F31 RID: 7985
			[NotificationProperty("str_too_many_poll_requests", "", "")]
			TooManyPollRequests,
			// Token: 0x04001F32 RID: 7986
			[NotificationProperty("str_kick_poll_target_not_synced", "", "")]
			KickPollTargetNotSynced,
			// Token: 0x04001F33 RID: 7987
			[NotificationProperty("str_not_enough_players_to_open_poll", "", "")]
			NotEnoughPlayersToOpenPoll,
			// Token: 0x04001F34 RID: 7988
			[NotificationProperty("str_player_is_kicked", "", "")]
			PlayerIsKicked,
			// Token: 0x04001F35 RID: 7989
			[NotificationProperty("str_formation_autofollow_enforced", "", "")]
			FormationAutoFollowEnforced,
			// Token: 0x04001F36 RID: 7990
			Count
		}
	}
}
