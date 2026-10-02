using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C4 RID: 708
	public class MultiplayerRoundComponent : MissionNetwork, IRoundComponent, IMissionBehavior
	{
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x060028B9 RID: 10425 RVA: 0x0009AD24 File Offset: 0x00098F24
		// (remove) Token: 0x060028BA RID: 10426 RVA: 0x0009AD5C File Offset: 0x00098F5C
		public event Action OnRoundStarted;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x060028BB RID: 10427 RVA: 0x0009AD94 File Offset: 0x00098F94
		// (remove) Token: 0x060028BC RID: 10428 RVA: 0x0009ADCC File Offset: 0x00098FCC
		public event Action OnPreparationEnded;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060028BD RID: 10429 RVA: 0x0009AE04 File Offset: 0x00099004
		// (remove) Token: 0x060028BE RID: 10430 RVA: 0x0009AE3C File Offset: 0x0009903C
		public event Action OnPreRoundEnding;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x060028BF RID: 10431 RVA: 0x0009AE74 File Offset: 0x00099074
		// (remove) Token: 0x060028C0 RID: 10432 RVA: 0x0009AEAC File Offset: 0x000990AC
		public event Action OnRoundEnding;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060028C1 RID: 10433 RVA: 0x0009AEE4 File Offset: 0x000990E4
		// (remove) Token: 0x060028C2 RID: 10434 RVA: 0x0009AF1C File Offset: 0x0009911C
		public event Action OnPostRoundEnded;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x060028C3 RID: 10435 RVA: 0x0009AF54 File Offset: 0x00099154
		// (remove) Token: 0x060028C4 RID: 10436 RVA: 0x0009AF8C File Offset: 0x0009918C
		public event Action OnCurrentRoundStateChanged;

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x0009AFC1 File Offset: 0x000991C1
		public float RemainingRoundTime
		{
			get
			{
				return this._gameModeClient.TimerComponent.GetRemainingTime(true);
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x0009AFD4 File Offset: 0x000991D4
		// (set) Token: 0x060028C7 RID: 10439 RVA: 0x0009AFDC File Offset: 0x000991DC
		public float LastRoundEndRemainingTime { get; private set; }

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x0009AFE5 File Offset: 0x000991E5
		// (set) Token: 0x060028C9 RID: 10441 RVA: 0x0009AFED File Offset: 0x000991ED
		public MultiplayerRoundState CurrentRoundState { get; private set; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x0009AFF6 File Offset: 0x000991F6
		// (set) Token: 0x060028CB RID: 10443 RVA: 0x0009AFFE File Offset: 0x000991FE
		public int RoundCount { get; private set; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x0009B007 File Offset: 0x00099207
		// (set) Token: 0x060028CD RID: 10445 RVA: 0x0009B00F File Offset: 0x0009920F
		public BattleSideEnum RoundWinner { get; private set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x0009B018 File Offset: 0x00099218
		// (set) Token: 0x060028CF RID: 10447 RVA: 0x0009B020 File Offset: 0x00099220
		public RoundEndReason RoundEndReason { get; private set; }

		// Token: 0x060028D0 RID: 10448 RVA: 0x0009B029 File Offset: 0x00099229
		public override void AfterStart()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			this._gameModeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x0009B042 File Offset: 0x00099242
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x0009B04C File Offset: 0x0009924C
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<RoundStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundStateChange>(this.HandleServerEventChangeRoundState));
				networkMessageHandlerRegisterer.Register<RoundCountChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundCountChange>(this.HandleServerEventRoundCountChange));
				networkMessageHandlerRegisterer.Register<RoundWinnerChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundWinnerChange>(this.HandleServerEventRoundWinnerChange));
				networkMessageHandlerRegisterer.Register<RoundEndReasonChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundEndReasonChange>(this.HandleServerEventRoundEndReasonChange));
			}
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x0009B0B0 File Offset: 0x000992B0
		private void HandleServerEventChangeRoundState(RoundStateChange message)
		{
			if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
			{
				this.LastRoundEndRemainingTime = (float)message.RemainingTimeOnPreviousState;
			}
			this.CurrentRoundState = message.RoundState;
			switch (this.CurrentRoundState)
			{
			case MultiplayerRoundState.Preparation:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, (float)MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				if (this.OnRoundStarted != null)
				{
					this.OnRoundStarted();
				}
				break;
			case MultiplayerRoundState.InProgress:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, (float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				if (this.OnPreparationEnded != null)
				{
					this.OnPreparationEnded();
				}
				break;
			case MultiplayerRoundState.Ending:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 3f);
				if (this.OnPreRoundEnding != null)
				{
					this.OnPreRoundEnding();
				}
				if (this.OnRoundEnding != null)
				{
					this.OnRoundEnding();
				}
				break;
			case MultiplayerRoundState.Ended:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 5f);
				if (this.OnPostRoundEnded != null)
				{
					this.OnPostRoundEnded();
				}
				break;
			case MultiplayerRoundState.MatchEnded:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 5f);
				break;
			}
			Action onCurrentRoundStateChanged = this.OnCurrentRoundStateChanged;
			if (onCurrentRoundStateChanged == null)
			{
				return;
			}
			onCurrentRoundStateChanged();
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0009B219 File Offset: 0x00099419
		private void HandleServerEventRoundCountChange(RoundCountChange message)
		{
			this.RoundCount = message.RoundCount;
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x0009B227 File Offset: 0x00099427
		private void HandleServerEventRoundWinnerChange(RoundWinnerChange message)
		{
			this.RoundWinner = message.RoundWinner;
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x0009B235 File Offset: 0x00099435
		private void HandleServerEventRoundEndReasonChange(RoundEndReasonChange message)
		{
			this.RoundEndReason = message.RoundEndReason;
		}

		// Token: 0x04000F91 RID: 3985
		public const int RoundEndDelayTime = 3;

		// Token: 0x04000F92 RID: 3986
		public const int RoundEndWaitTime = 8;

		// Token: 0x04000F93 RID: 3987
		public const int MatchEndWaitTime = 5;

		// Token: 0x04000F94 RID: 3988
		public const int WarmupEndWaitTime = 30;

		// Token: 0x04000F9B RID: 3995
		private MissionMultiplayerGameModeBaseClient _gameModeClient;
	}
}
