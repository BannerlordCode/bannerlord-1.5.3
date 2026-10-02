using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BA RID: 698
	public class MissionMultiplayerTeamDeathmatchClient : MissionMultiplayerGameModeBaseClient
	{
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x060027AE RID: 10158 RVA: 0x00093604 File Offset: 0x00091804
		// (remove) Token: 0x060027AF RID: 10159 RVA: 0x0009363C File Offset: 0x0009183C
		public event Action<GoldGain> OnGoldGainEvent;

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x00093671 File Offset: 0x00091871
		public override bool IsGameModeUsingGold
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x00093674 File Offset: 0x00091874
		public override bool IsGameModeTactical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x00093677 File Offset: 0x00091877
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x0009367A File Offset: 0x0009187A
		public override MultiplayerGameType GameType
		{
			get
			{
				return MultiplayerGameType.TeamDeathmatch;
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0009367D File Offset: 0x0009187D
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			base.ScoreboardComponent.OnRoundPropertiesChanged += this.OnTeamScoresChanged;
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000936B3 File Offset: 0x000918B3
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
			if (representative != null && base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				representative.UpdateGold(goldAmount);
				base.ScoreboardComponent.PlayerPropertiesChanged(representative.MissionPeer);
			}
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000936DE File Offset: 0x000918DE
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000936ED File Offset: 0x000918ED
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<SyncGoldsForSkirmish>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateGold));
				registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventTDMGoldGain));
			}
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x0009371A File Offset: 0x0009191A
		private void OnMyClientSynchronized()
		{
			this._myRepresentative = GameNetwork.MyPeer.GetComponent<TeamDeathmatchMissionRepresentative>();
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x0009372C File Offset: 0x0009192C
		private void HandleServerEventUpdateGold(GameNetworkMessage baseMessage)
		{
			SyncGoldsForSkirmish syncGoldsForSkirmish = (SyncGoldsForSkirmish)baseMessage;
			MissionRepresentativeBase component = syncGoldsForSkirmish.VirtualPlayer.GetComponent<MissionRepresentativeBase>();
			this.OnGoldAmountChangedForRepresentative(component, syncGoldsForSkirmish.GoldAmount);
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x0009375C File Offset: 0x0009195C
		private void HandleServerEventTDMGoldGain(GameNetworkMessage baseMessage)
		{
			GoldGain goldGain = (GoldGain)baseMessage;
			Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
			if (onGoldGainEvent == null)
			{
				return;
			}
			onGoldGainEvent(goldGain);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x00093781 File Offset: 0x00091981
		public override int GetGoldAmount()
		{
			return this._myRepresentative.Gold;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x0009378E File Offset: 0x0009198E
		public override void OnRemoveBehavior()
		{
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			base.ScoreboardComponent.OnRoundPropertiesChanged -= this.OnTeamScoresChanged;
			base.OnRemoveBehavior();
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000937C4 File Offset: 0x000919C4
		private void OnTeamScoresChanged()
		{
			if (!GameNetwork.IsDedicatedServer && !this._battleEndingNotificationGiven && this._myRepresentative.MissionPeer.Team != null && this._myRepresentative.MissionPeer.Team.Side != BattleSideEnum.None)
			{
				int intValue = MultiplayerOptions.OptionType.MinScoreToWinMatch.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				float num = (float)(intValue - base.ScoreboardComponent.GetRoundScore(this._myRepresentative.MissionPeer.Team.Side)) / (float)intValue;
				float num2 = (float)(intValue - base.ScoreboardComponent.GetRoundScore(this._myRepresentative.MissionPeer.Team.Side.GetOppositeSide())) / (float)intValue;
				MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
				Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
				if (num <= 0.1f && num2 > 0.1f)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/battle_winning"), vec);
					this._battleEndingNotificationGiven = true;
				}
				if (num2 <= 0.1f && num > 0.1f)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/battle_losing"), vec);
					this._battleEndingNotificationGiven = true;
				}
			}
		}

		// Token: 0x04000F0C RID: 3852
		private const string BattleWinningSoundEventString = "event:/alerts/report/battle_winning";

		// Token: 0x04000F0D RID: 3853
		private const string BattleLosingSoundEventString = "event:/alerts/report/battle_losing";

		// Token: 0x04000F0E RID: 3854
		private const float BattleWinLoseAlertThreshold = 0.1f;

		// Token: 0x04000F10 RID: 3856
		private TeamDeathmatchMissionRepresentative _myRepresentative;

		// Token: 0x04000F11 RID: 3857
		private bool _battleEndingNotificationGiven;
	}
}
