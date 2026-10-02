using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CA RID: 714
	public class MultiplayerWarmupComponent : MissionNetwork
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x0009CB33 File Offset: 0x0009AD33
		public static float TotalWarmupDuration
		{
			get
			{
				return (float)MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06002931 RID: 10545 RVA: 0x0009CB40 File Offset: 0x0009AD40
		// (remove) Token: 0x06002932 RID: 10546 RVA: 0x0009CB78 File Offset: 0x0009AD78
		public event Action OnWarmupEnding;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06002933 RID: 10547 RVA: 0x0009CBB0 File Offset: 0x0009ADB0
		// (remove) Token: 0x06002934 RID: 10548 RVA: 0x0009CBE8 File Offset: 0x0009ADE8
		public event Action OnWarmupEnded;

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x0009CC1D File Offset: 0x0009AE1D
		public bool IsInWarmup
		{
			get
			{
				return this.WarmupState != MultiplayerWarmupComponent.WarmupStates.Ended;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x0009CC2B File Offset: 0x0009AE2B
		// (set) Token: 0x06002937 RID: 10551 RVA: 0x0009CC34 File Offset: 0x0009AE34
		private MultiplayerWarmupComponent.WarmupStates WarmupState
		{
			get
			{
				return this._warmupState;
			}
			set
			{
				this._warmupState = value;
				if (GameNetwork.IsServer)
				{
					this._currentStateStartTime = MissionTime.Now;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new WarmupStateChange(this._warmupState, this._currentStateStartTime.NumberOfTicks));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0009CC81 File Offset: 0x0009AE81
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._gameMode = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this._timerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0009CCBC File Offset: 0x0009AEBC
		public override void AfterStart()
		{
			base.AfterStart();
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0009CCCB File Offset: 0x0009AECB
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x0009CCD4 File Offset: 0x0009AED4
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<WarmupStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<WarmupStateChange>(this.HandleServerEventWarmupStateChange));
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0009CD01 File Offset: 0x0009AF01
		public bool CheckForWarmupProgressEnd()
		{
			return this._gameMode.CheckForWarmupEnd() || this._timerComponent.GetRemainingTime(false) <= 30f;
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0009CD28 File Offset: 0x0009AF28
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (GameNetwork.IsServer && this._lobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				switch (this.WarmupState)
				{
				case MultiplayerWarmupComponent.WarmupStates.WaitingForPlayers:
					this.BeginWarmup();
					return;
				case MultiplayerWarmupComponent.WarmupStates.InProgress:
					if (this.CheckForWarmupProgressEnd())
					{
						this.EndWarmupProgress();
						return;
					}
					break;
				case MultiplayerWarmupComponent.WarmupStates.Ending:
					if (this._timerComponent.CheckIfTimerPassed())
					{
						this.EndWarmup();
						return;
					}
					break;
				case MultiplayerWarmupComponent.WarmupStates.Ended:
					if (this._timerComponent.CheckIfTimerPassed())
					{
						base.Mission.RemoveMissionBehavior(this);
						return;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x0009CDB4 File Offset: 0x0009AFB4
		private void BeginWarmup()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.InProgress;
			Mission.Current.ResetMission();
			this._gameMode.MultiplayerTeamSelectComponent.BalanceTeams();
			this._timerComponent.StartTimerAsServer(MultiplayerWarmupComponent.TotalWarmupDuration);
			this._gameMode.SpawnComponent.SpawningBehavior.Clear();
			SpawnComponent.SetWarmupSpawningBehavior();
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x0009CE0C File Offset: 0x0009B00C
		public void EndWarmupProgress()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.Ending;
			this._timerComponent.StartTimerAsServer(30f);
			Action onWarmupEnding = this.OnWarmupEnding;
			if (onWarmupEnding == null)
			{
				return;
			}
			onWarmupEnding();
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x0009CE38 File Offset: 0x0009B038
		private void EndWarmup()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.Ended;
			this._timerComponent.StartTimerAsServer(3f);
			Action onWarmupEnded = this.OnWarmupEnded;
			if (onWarmupEnded != null)
			{
				onWarmupEnded();
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				this.PlayBattleStartingSound();
			}
			Mission.Current.ResetMission();
			this._gameMode.MultiplayerTeamSelectComponent.BalanceTeams();
			this._gameMode.SpawnComponent.SpawningBehavior.Clear();
			SpawnComponent.SetSpawningBehaviorForCurrentGameType(this._gameMode.GetMissionType());
			if (!this.CanMatchStartAfterWarmup())
			{
				this._lobbyComponent.SetStateEndingAsServer();
			}
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x0009CECC File Offset: 0x0009B0CC
		public bool CanMatchStartAfterWarmup()
		{
			bool[] array = new bool[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side] = true;
				}
				if (array[1] && array[0])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x0009CF60 File Offset: 0x0009B160
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			this.OnWarmupEnding = null;
			this.OnWarmupEnded = null;
			if (GameNetwork.IsServer && !this._gameMode.UseRoundController() && this._lobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				this._gameMode.SpawnComponent.SpawningBehavior.RequestStartSpawnSession();
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x0009CFB8 File Offset: 0x0009B1B8
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (this.IsInWarmup && !networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new WarmupStateChange(this._warmupState, this._currentStateStartTime.NumberOfTicks));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x0009CFF0 File Offset: 0x0009B1F0
		private void HandleServerEventWarmupStateChange(WarmupStateChange message)
		{
			this.WarmupState = message.WarmupState;
			switch (this.WarmupState)
			{
			case MultiplayerWarmupComponent.WarmupStates.InProgress:
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, MultiplayerWarmupComponent.TotalWarmupDuration);
				return;
			case MultiplayerWarmupComponent.WarmupStates.Ending:
			{
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 30f);
				Action onWarmupEnding = this.OnWarmupEnding;
				if (onWarmupEnding == null)
				{
					return;
				}
				onWarmupEnding();
				return;
			}
			case MultiplayerWarmupComponent.WarmupStates.Ended:
			{
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 3f);
				Action onWarmupEnded = this.OnWarmupEnded;
				if (onWarmupEnded != null)
				{
					onWarmupEnded();
				}
				this.PlayBattleStartingSound();
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x0009D090 File Offset: 0x0009B290
		private void PlayBattleStartingSound()
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (((missionPeer != null) ? missionPeer.Team : null) != null)
			{
				string text = ((missionPeer.Team.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/rally/" + text.ToLower()), vec);
				return;
			}
			MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/rally/generic"), vec);
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x0009D130 File Offset: 0x0009B330
		[CommandLineFunctionality.CommandLineArgumentFunction("end_warmup", "mp_host")]
		public static string CommandEndWarmup(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "end_warmup can only be called within a mission.";
			}
			if (!GameNetwork.IsServer)
			{
				return "end_warmup can only be called by the server.";
			}
			MultiplayerWarmupComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
			if (missionBehavior == null)
			{
				return "end_warmup can only be called when the game is in warmup.";
			}
			missionBehavior.EndWarmupProgress();
			return "Success";
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0009D178 File Offset: 0x0009B378
		[CommandLineFunctionality.CommandLineArgumentFunction("reset_warmup_timer", "mp_host")]
		public static string CommandExtendWarmup(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "extend_warmup can only be called within a mission.";
			}
			if (!GameNetwork.IsServer)
			{
				return "extend_warmup can only be called by the server.";
			}
			MultiplayerWarmupComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
			if (missionBehavior == null)
			{
				return "extend_warmup can only be called when the game is in warmup.";
			}
			missionBehavior.WarmupState = MultiplayerWarmupComponent.WarmupStates.InProgress;
			missionBehavior._timerComponent.StartTimerAsServer(MultiplayerWarmupComponent.TotalWarmupDuration);
			return "Success";
		}

		// Token: 0x04000FCA RID: 4042
		public const int RespawnPeriodInWarmup = 3;

		// Token: 0x04000FCB RID: 4043
		public const int WarmupEndWaitTime = 30;

		// Token: 0x04000FCE RID: 4046
		private MissionMultiplayerGameModeBase _gameMode;

		// Token: 0x04000FCF RID: 4047
		private MultiplayerTimerComponent _timerComponent;

		// Token: 0x04000FD0 RID: 4048
		private MissionLobbyComponent _lobbyComponent;

		// Token: 0x04000FD1 RID: 4049
		private MissionTime _currentStateStartTime;

		// Token: 0x04000FD2 RID: 4050
		private MultiplayerWarmupComponent.WarmupStates _warmupState;

		// Token: 0x020005B8 RID: 1464
		public enum WarmupStates
		{
			// Token: 0x04001F5D RID: 8029
			WaitingForPlayers,
			// Token: 0x04001F5E RID: 8030
			InProgress,
			// Token: 0x04001F5F RID: 8031
			Ending,
			// Token: 0x04001F60 RID: 8032
			Ended
		}
	}
}
