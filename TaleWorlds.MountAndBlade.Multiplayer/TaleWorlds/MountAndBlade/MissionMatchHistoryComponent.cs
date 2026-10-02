using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.NetworkComponents;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000016 RID: 22
	public class MissionMatchHistoryComponent : MissionNetwork
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0000563C File Offset: 0x0000383C
		public static MissionMatchHistoryComponent CreateIfConditionsAreMet()
		{
			BaseNetworkComponent networkComponent = GameNetwork.GetNetworkComponent<BaseNetworkComponent>();
			if ((networkComponent == null || networkComponent.ClientIntermissionState == MultiplayerIntermissionState.Idle || NetworkMain.GameClient.IsInGame) && NetworkMain.GameClient.LastBattleIsOfficial)
			{
				return new MissionMatchHistoryComponent();
			}
			Debug.Print(string.Format("Failed to create {0}. NetworkMain.GameClient.IsInGame: {1}, NetworkMain.GameClient.LastBattleIsOfficial: {2}", typeof(MissionMatchHistoryComponent).Name, NetworkMain.GameClient.IsInGame, NetworkMain.GameClient.LastBattleIsOfficial), 0, Debug.DebugColor.White, 17592186044416UL);
			return null;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000056C8 File Offset: 0x000038C8
		private MissionMatchHistoryComponent()
		{
			this._recordedHistory = false;
			MatchHistoryData matchHistoryData;
			if (MultiplayerLocalDataManager.Instance.MatchHistory.TryGetHistoryData(NetworkMain.GameClient.CurrentMatchId, out matchHistoryData))
			{
				this._matchHistoryData = matchHistoryData;
			}
			else
			{
				this._matchHistoryData = new MatchHistoryData();
				this._matchHistoryData.MatchId = NetworkMain.GameClient.CurrentMatchId;
			}
			this._matchHistoryData.MatchDate = DateTime.Now;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005738 File Offset: 0x00003938
		private static void PrintDebugLog(string text)
		{
			Debug.Print("[MATCH_HISTORY_COMPONTENT]: " + text, 0, Debug.DebugColor.Yellow, 17592186044416UL);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005758 File Offset: 0x00003958
		public override void OnBehaviorInitialize()
		{
			MissionMultiplayerGameModeBaseClient missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			MultiplayerGameType multiplayerGameType = ((missionBehavior != null) ? missionBehavior.GameType : MultiplayerGameType.TeamDeathmatch);
			this._matchHistoryData.GameType = multiplayerGameType.ToString();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005794 File Offset: 0x00003994
		public override void AfterStart()
		{
			base.AfterStart();
			string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue2 = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue3 = MultiplayerOptions.OptionType.Map.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this._matchHistoryData.Faction1 = strValue;
			this._matchHistoryData.Faction2 = strValue2;
			this._matchHistoryData.Map = strValue3;
			MissionPeer.OnTeamChanged += this.TeamChange;
			base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._matchHistoryData.MatchType = BannerlordNetwork.LobbyMissionType.ToString();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005821 File Offset: 0x00003A21
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			registerer.RegisterBaseHandler<MissionStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMissionStateChange));
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005838 File Offset: 0x00003A38
		private void TeamChange(NetworkCommunicator player, Team oldTeam, Team nextTeam)
		{
			this._matchHistoryData.AddOrUpdatePlayer(player.VirtualPlayer.Id.ToString(), player.VirtualPlayer.UserName, player.ForcedAvatarIndex, (int)nextTeam.Side);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005880 File Offset: 0x00003A80
		private void HandleServerEventMissionStateChange(GameNetworkMessage baseMessage)
		{
			if (((MissionStateChange)baseMessage).CurrentState == MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				MissionMatchHistoryComponent.PrintDebugLog("Received mission ending message from server");
				if (!this._recordedHistory)
				{
					MissionMatchHistoryComponent.PrintDebugLog("Match history is eligible for recording after end message");
					MissionScoreboardComponent missionBehavior = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
					if (missionBehavior != null && !missionBehavior.IsOneSided)
					{
						int roundScore = missionBehavior.GetRoundScore(BattleSideEnum.Attacker);
						int roundScore2 = missionBehavior.GetRoundScore(BattleSideEnum.Defender);
						BattleSideEnum matchWinnerSide = missionBehavior.GetMatchWinnerSide();
						this._matchHistoryData.WinnerTeam = (int)matchWinnerSide;
						this._matchHistoryData.AttackerScore = roundScore;
						this._matchHistoryData.DefenderScore = roundScore2;
						MissionScoreboardComponent.MissionScoreboardSide[] sides = missionBehavior.Sides;
						for (int i = 0; i < sides.Length; i++)
						{
							foreach (MissionPeer missionPeer in sides[i].Players)
							{
								this._matchHistoryData.TryUpdatePlayerStats(missionPeer.Peer.Id.ToString(), missionPeer.KillCount, missionPeer.DeathCount, missionPeer.AssistCount);
							}
						}
					}
					MultiplayerLocalDataManager.Instance.MatchHistory.AddEntry(this._matchHistoryData);
					this._recordedHistory = true;
					MissionMatchHistoryComponent.PrintDebugLog("Recorded match history after end message");
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000059D8 File Offset: 0x00003BD8
		public override void OnRemoveBehavior()
		{
			MissionMatchHistoryComponent.PrintDebugLog("Removing match history behavior");
			if (!this._recordedHistory)
			{
				MissionMatchHistoryComponent.PrintDebugLog("Match history was eligible for recording when removing behavior");
				this._matchHistoryData.WinnerTeam = -1;
				MissionScoreboardComponent missionBehavior = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
				if (missionBehavior != null && !missionBehavior.IsOneSided)
				{
					int roundScore = missionBehavior.GetRoundScore(BattleSideEnum.Attacker);
					int roundScore2 = missionBehavior.GetRoundScore(BattleSideEnum.Defender);
					this._matchHistoryData.AttackerScore = roundScore;
					this._matchHistoryData.DefenderScore = roundScore2;
					MissionScoreboardComponent.MissionScoreboardSide[] sides = missionBehavior.Sides;
					for (int i = 0; i < sides.Length; i++)
					{
						foreach (MissionPeer missionPeer in sides[i].Players)
						{
							this._matchHistoryData.TryUpdatePlayerStats(missionPeer.Peer.Id.ToString(), missionPeer.KillCount, missionPeer.DeathCount, missionPeer.AssistCount);
						}
					}
				}
				MultiplayerLocalDataManager.Instance.MatchHistory.AddEntry(this._matchHistoryData);
				MissionMatchHistoryComponent.PrintDebugLog("Recorded match history after removing behavior");
				this._recordedHistory = true;
			}
			MissionPeer.OnTeamChanged -= this.TeamChange;
			base.OnRemoveBehavior();
		}

		// Token: 0x0400003C RID: 60
		private bool _recordedHistory;

		// Token: 0x0400003D RID: 61
		private MatchHistoryData _matchHistoryData;
	}
}
