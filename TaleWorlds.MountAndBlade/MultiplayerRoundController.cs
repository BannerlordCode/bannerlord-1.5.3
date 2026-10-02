using System;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C7 RID: 711
	public class MultiplayerRoundController : MissionNetwork, IRoundComponent, IMissionBehavior
	{
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060028D8 RID: 10456 RVA: 0x0009B24C File Offset: 0x0009944C
		// (remove) Token: 0x060028D9 RID: 10457 RVA: 0x0009B284 File Offset: 0x00099484
		public event Action OnRoundStarted;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x060028DA RID: 10458 RVA: 0x0009B2BC File Offset: 0x000994BC
		// (remove) Token: 0x060028DB RID: 10459 RVA: 0x0009B2F4 File Offset: 0x000994F4
		public event Action OnPreparationEnded;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x060028DC RID: 10460 RVA: 0x0009B32C File Offset: 0x0009952C
		// (remove) Token: 0x060028DD RID: 10461 RVA: 0x0009B364 File Offset: 0x00099564
		public event Action OnPreRoundEnding;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x060028DE RID: 10462 RVA: 0x0009B39C File Offset: 0x0009959C
		// (remove) Token: 0x060028DF RID: 10463 RVA: 0x0009B3D4 File Offset: 0x000995D4
		public event Action OnRoundEnding;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060028E0 RID: 10464 RVA: 0x0009B40C File Offset: 0x0009960C
		// (remove) Token: 0x060028E1 RID: 10465 RVA: 0x0009B444 File Offset: 0x00099644
		public event Action OnPostRoundEnded;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x060028E2 RID: 10466 RVA: 0x0009B47C File Offset: 0x0009967C
		// (remove) Token: 0x060028E3 RID: 10467 RVA: 0x0009B4B4 File Offset: 0x000996B4
		public event Action OnCurrentRoundStateChanged;

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x0009B4E9 File Offset: 0x000996E9
		// (set) Token: 0x060028E5 RID: 10469 RVA: 0x0009B4F1 File Offset: 0x000996F1
		public int RoundCount
		{
			get
			{
				return this._roundCount;
			}
			set
			{
				if (this._roundCount != value)
				{
					this._roundCount = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundCountChange(this._roundCount));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x0009B526 File Offset: 0x00099726
		// (set) Token: 0x060028E7 RID: 10471 RVA: 0x0009B52E File Offset: 0x0009972E
		public BattleSideEnum RoundWinner
		{
			get
			{
				return this._roundWinner;
			}
			set
			{
				if (this._roundWinner != value)
				{
					this._roundWinner = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundWinnerChange(value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x0009B55E File Offset: 0x0009975E
		// (set) Token: 0x060028E9 RID: 10473 RVA: 0x0009B566 File Offset: 0x00099766
		public RoundEndReason RoundEndReason
		{
			get
			{
				return this._roundEndReason;
			}
			set
			{
				if (this._roundEndReason != value)
				{
					this._roundEndReason = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundEndReasonChange(value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x0009B596 File Offset: 0x00099796
		// (set) Token: 0x060028EB RID: 10475 RVA: 0x0009B59E File Offset: 0x0009979E
		public bool IsMatchEnding { get; private set; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x0009B5A7 File Offset: 0x000997A7
		// (set) Token: 0x060028ED RID: 10477 RVA: 0x0009B5AF File Offset: 0x000997AF
		public float LastRoundEndRemainingTime { get; private set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x0009B5B8 File Offset: 0x000997B8
		public float RemainingRoundTime
		{
			get
			{
				return this._gameModeServer.TimerComponent.GetRemainingTime(false);
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x0009B5CB File Offset: 0x000997CB
		// (set) Token: 0x060028F0 RID: 10480 RVA: 0x0009B5D3 File Offset: 0x000997D3
		public MultiplayerRoundState CurrentRoundState { get; private set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060028F1 RID: 10481 RVA: 0x0009B5DC File Offset: 0x000997DC
		public bool IsRoundInProgress
		{
			get
			{
				return this.CurrentRoundState == MultiplayerRoundState.InProgress;
			}
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0009B5E7 File Offset: 0x000997E7
		public void EnableEquipmentUpdate()
		{
			this._equipmentUpdateDisabled = false;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0009B5F0 File Offset: 0x000997F0
		public override void AfterStart()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			if (GameNetwork.IsServerOrRecorder)
			{
				this._gameModeServer = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			}
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._roundCount = 0;
			this._gameModeServer.TimerComponent.StartTimerAsServer(8f);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x0009B648 File Offset: 0x00099848
		private void EndRound()
		{
			if (this.OnPreRoundEnding != null)
			{
				this.OnPreRoundEnding();
			}
			this.ChangeRoundState(MultiplayerRoundState.Ending);
			this._gameModeServer.TimerComponent.StartTimerAsServer(3f);
			this._roundTimeOver = false;
			if (this.OnRoundEnding != null)
			{
				this.OnRoundEnding();
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0009B69E File Offset: 0x0009989E
		private bool CheckPostEndRound()
		{
			return this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x0009B6B0 File Offset: 0x000998B0
		private bool CheckPostMatchEnd()
		{
			return this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x0009B6C4 File Offset: 0x000998C4
		private void PostRoundEnd()
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer(5f);
			this.ChangeRoundState(MultiplayerRoundState.Ended);
			if (this._roundCount == MultiplayerOptions.OptionType.RoundTotal.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) || this.CheckForMatchEndEarly() || !this.HasEnoughCharactersOnBothSides())
			{
				this.IsMatchEnding = true;
			}
			if (this.OnPostRoundEnded != null)
			{
				this.OnPostRoundEnded();
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x0009B727 File Offset: 0x00099927
		private void PostMatchEnd()
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer(5f);
			this.ChangeRoundState(MultiplayerRoundState.MatchEnded);
			this._missionLobbyComponent.SetStateEndingAsServer();
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x0009B750 File Offset: 0x00099950
		public override void OnRemoveBehavior()
		{
			GameNetwork.RemoveNetworkHandler(this);
			base.OnRemoveBehavior();
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x0009B760 File Offset: 0x00099960
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (!GameNetwork.IsClient && GameNetwork.IsServer)
			{
				networkMessageHandlerRegisterer.Register<CultureVoteClient>(new GameNetworkMessage.ClientMessageHandlerDelegate<CultureVoteClient>(this.HandleClientEventCultureSelect));
			}
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x0009B794 File Offset: 0x00099994
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0009B7A0 File Offset: 0x000999A0
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (GameNetwork.IsServer)
			{
				if (this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
				{
					if (!this.IsMatchEnding && this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && (this.CurrentRoundState == MultiplayerRoundState.WaitingForPlayers || this.CurrentRoundState == MultiplayerRoundState.Ended))
					{
						if (this.CheckForNewRound())
						{
							this.BeginNewRound();
							return;
						}
						if (this.IsMatchEnding)
						{
							this.PostMatchEnd();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Preparation)
					{
						if (this.CheckForPreparationEnd())
						{
							this.EndPreparation();
							this.StartSpawning(this._equipmentUpdateDisabled);
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
					{
						if (this.CheckForRoundEnd())
						{
							this.EndRound();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Ending)
					{
						if (this.CheckPostEndRound())
						{
							this.PostRoundEnd();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Ended && this.IsMatchEnding && this.CheckPostMatchEnd())
					{
						this.PostMatchEnd();
						return;
					}
				}
			}
			else
			{
				this._gameModeServer.TimerComponent.CheckIfTimerPassed();
			}
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x0009B894 File Offset: 0x00099A94
		private void ChangeRoundState(MultiplayerRoundState newRoundState)
		{
			if (this.CurrentRoundState != newRoundState)
			{
				if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
				{
					this.LastRoundEndRemainingTime = this.RemainingRoundTime;
				}
				this.CurrentRoundState = newRoundState;
				this._currentRoundStateStartTime = MissionTime.Now;
				Action onCurrentRoundStateChanged = this.OnCurrentRoundStateChanged;
				if (onCurrentRoundStateChanged != null)
				{
					onCurrentRoundStateChanged();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RoundStateChange(newRoundState, this._currentRoundStateStartTime.NumberOfTicks, MathF.Ceiling(this.LastRoundEndRemainingTime)));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x0009B90F File Offset: 0x00099B0F
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x0009B911 File Offset: 0x00099B11
		public bool HandleClientEventCultureSelect(NetworkCommunicator peer, CultureVoteClient message)
		{
			peer.GetComponent<MissionPeer>().HandleVoteChange(message.VotedType, message.VotedCulture);
			return true;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x0009B92C File Offset: 0x00099B2C
		private bool CheckForRoundEnd()
		{
			if (!this._roundTimeOver)
			{
				this._roundTimeOver = this._gameModeServer.TimerComponent.CheckIfTimerPassed();
			}
			return (!this._gameModeServer.CheckIfOvertime() && this._roundTimeOver) || this._gameModeServer.CheckForRoundEnd();
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0009B97C File Offset: 0x00099B7C
		private bool CheckForNewRound()
		{
			if (this.CurrentRoundState != MultiplayerRoundState.WaitingForPlayers && !this._gameModeServer.TimerComponent.CheckIfTimerPassed())
			{
				return false;
			}
			int[] array = new int[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (networkCommunicator.IsSynchronized && ((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side]++;
				}
			}
			if (array.Sum() < MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) && this.RoundCount == 0)
			{
				this.IsMatchEnding = true;
				return false;
			}
			return true;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0009BA54 File Offset: 0x00099C54
		private bool HasEnoughCharactersOnBothSides()
		{
			bool flag;
			if (MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				flag = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator q) => q.GetComponent<MissionPeer>() != null && q.GetComponent<MissionPeer>().Team == Mission.Current.AttackerTeam) > 0;
			}
			else
			{
				flag = true;
			}
			bool flag2;
			if (MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				flag2 = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator q) => q.GetComponent<MissionPeer>() != null && q.GetComponent<MissionPeer>().Team == Mission.Current.DefenderTeam) > 0;
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			return flag && flag3;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0009BAD8 File Offset: 0x00099CD8
		private void BeginNewRound()
		{
			if (this.CurrentRoundState == MultiplayerRoundState.WaitingForPlayers)
			{
				this._gameModeServer.ClearPeerCounts();
			}
			this.ChangeRoundState(MultiplayerRoundState.Preparation);
			int roundCount = this.RoundCount;
			this.RoundCount = roundCount + 1;
			Mission.Current.ResetMission();
			this._gameModeServer.MultiplayerTeamSelectComponent.BalanceTeams();
			this._gameModeServer.TimerComponent.StartTimerAsServer((float)MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			Action onRoundStarted = this.OnRoundStarted;
			if (onRoundStarted != null)
			{
				onRoundStarted();
			}
			this._gameModeServer.SpawnComponent.ToggleUpdatingSpawnEquipment(true);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0009BB64 File Offset: 0x00099D64
		private bool CheckForPreparationEnd()
		{
			return this.CurrentRoundState == MultiplayerRoundState.Preparation && this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0009BB81 File Offset: 0x00099D81
		private void EndPreparation()
		{
			if (this.OnPreparationEnded != null)
			{
				this.OnPreparationEnded();
			}
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0009BB96 File Offset: 0x00099D96
		private void StartSpawning(bool disableEquipmentUpdate = true)
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer((float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (disableEquipmentUpdate)
			{
				this._gameModeServer.SpawnComponent.ToggleUpdatingSpawnEquipment(false);
			}
			this.ChangeRoundState(MultiplayerRoundState.InProgress);
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0009BBCC File Offset: 0x00099DCC
		private bool CheckForMatchEndEarly()
		{
			bool flag = false;
			MissionScoreboardComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			if (missionBehavior != null)
			{
				for (int i = 0; i < 2; i++)
				{
					if (missionBehavior.GetRoundScore((BattleSideEnum)i) > MultiplayerOptions.OptionType.RoundTotal.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) / 2)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0009BC10 File Offset: 0x00099E10
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundStateChange(this.CurrentRoundState, this._currentRoundStateStartTime.NumberOfTicks, MathF.Ceiling(this.LastRoundEndRemainingTime)));
				GameNetwork.EndModuleEventAsServer();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundWinnerChange(this.RoundWinner));
				GameNetwork.EndModuleEventAsServer();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundCountChange(this.RoundCount));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x04000FB4 RID: 4020
		private MissionMultiplayerGameModeBase _gameModeServer;

		// Token: 0x04000FB5 RID: 4021
		private int _roundCount;

		// Token: 0x04000FB6 RID: 4022
		private BattleSideEnum _roundWinner;

		// Token: 0x04000FB7 RID: 4023
		private RoundEndReason _roundEndReason;

		// Token: 0x04000FB8 RID: 4024
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000FBA RID: 4026
		private bool _roundTimeOver;

		// Token: 0x04000FBC RID: 4028
		private MissionTime _currentRoundStateStartTime;

		// Token: 0x04000FBE RID: 4030
		private bool _equipmentUpdateDisabled = true;
	}
}
