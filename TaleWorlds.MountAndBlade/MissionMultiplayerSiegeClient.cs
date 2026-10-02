using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B9 RID: 697
	public class MissionMultiplayerSiegeClient : MissionMultiplayerGameModeBaseClient, ICommanderInfo, IMissionBehavior
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x00092942 File Offset: 0x00090B42
		public override bool IsGameModeUsingGold
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x00092945 File Offset: 0x00090B45
		public override bool IsGameModeTactical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x00092948 File Offset: 0x00090B48
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0009294B File Offset: 0x00090B4B
		public override MultiplayerGameType GameType
		{
			get
			{
				return MultiplayerGameType.Siege;
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x0600278E RID: 10126 RVA: 0x00092950 File Offset: 0x00090B50
		// (remove) Token: 0x0600278F RID: 10127 RVA: 0x00092988 File Offset: 0x00090B88
		public event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06002790 RID: 10128 RVA: 0x000929C0 File Offset: 0x00090BC0
		// (remove) Token: 0x06002791 RID: 10129 RVA: 0x000929F8 File Offset: 0x00090BF8
		public event Action OnFlagNumberChangedEvent;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06002792 RID: 10130 RVA: 0x00092A30 File Offset: 0x00090C30
		// (remove) Token: 0x06002793 RID: 10131 RVA: 0x00092A68 File Offset: 0x00090C68
		public event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06002794 RID: 10132 RVA: 0x00092AA0 File Offset: 0x00090CA0
		// (remove) Token: 0x06002795 RID: 10133 RVA: 0x00092AD8 File Offset: 0x00090CD8
		public event Action<GoldGain> OnGoldGainEvent;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06002796 RID: 10134 RVA: 0x00092B10 File Offset: 0x00090D10
		// (remove) Token: 0x06002797 RID: 10135 RVA: 0x00092B48 File Offset: 0x00090D48
		public event Action<int[]> OnCapturePointRemainingMoraleGainsChangedEvent;

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x00092B7D File Offset: 0x00090D7D
		public bool AreMoralesIndependent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x00092B80 File Offset: 0x00090D80
		// (set) Token: 0x0600279A RID: 10138 RVA: 0x00092B88 File Offset: 0x00090D88
		public IEnumerable<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x0600279B RID: 10139 RVA: 0x00092B94 File Offset: 0x00090D94
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<SiegeMoraleChangeMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleMoraleChangedMessage));
				registerer.RegisterBaseHandler<SyncGoldsForSkirmish>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateGold));
				registerer.RegisterBaseHandler<FlagDominationFlagsRemovedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleFlagsRemovedMessage));
				registerer.RegisterBaseHandler<FlagDominationCapturePointMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPointCapturedMessage));
				registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventTDMGoldGain));
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x00092C02 File Offset: 0x00090E02
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			this._capturePointOwners = new Team[7];
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00092C44 File Offset: 0x00090E44
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (flagCapturePoint.GameEntity.HasTag("keep_capture_point"))
				{
					this._masterFlag = flagCapturePoint;
				}
				else if (flagCapturePoint.FlagIndex == 0)
				{
					MatrixFrame globalFrame = flagCapturePoint.GameEntity.GetGlobalFrame();
					this._retreatHornPosition = globalFrame.origin + globalFrame.rotation.u * 3f;
				}
			}
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x00092CF4 File Offset: 0x00090EF4
		private void OnMyClientSynchronized()
		{
			this._myRepresentative = GameNetwork.MyPeer.GetComponent<SiegeMissionRepresentative>();
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00092D06 File Offset: 0x00090F06
		public override int GetGoldAmount()
		{
			return this._myRepresentative.Gold;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x00092D13 File Offset: 0x00090F13
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
			if (representative != null && base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				representative.UpdateGold(goldAmount);
				base.ScoreboardComponent.PlayerPropertiesChanged(representative.MissionPeer);
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x00092D40 File Offset: 0x00090F40
		public void OnNumberOfFlagsChanged()
		{
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent != null)
			{
				onFlagNumberChangedEvent();
			}
			SiegeMissionRepresentative myRepresentative = this._myRepresentative;
			bool flag;
			if (myRepresentative == null)
			{
				flag = false;
			}
			else
			{
				Team team = myRepresentative.MissionPeer.Team;
				BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
				BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
				flag = (battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null);
			}
			if (flag)
			{
				Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
				if (onGoldGainEvent == null)
				{
					return;
				}
				onGoldGainEvent(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(512, 35)
				}));
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x00092DD4 File Offset: 0x00090FD4
		public void OnCapturePointOwnerChanged(FlagCapturePoint flagCapturePoint, Team ownerTeam)
		{
			this._capturePointOwners[flagCapturePoint.FlagIndex] = ownerTeam;
			Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
			if (onCapturePointOwnerChangedEvent != null)
			{
				onCapturePointOwnerChangedEvent(flagCapturePoint, ownerTeam);
			}
			if (ownerTeam != null && ownerTeam.Side == BattleSideEnum.Defender && this._remainingTimeForBellSoundToStop > 8f && flagCapturePoint == this._masterFlag)
			{
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._lastBellSoundPercentage += 0.2f;
			}
			if (this._myRepresentative != null && this._myRepresentative.MissionPeer.Team != null)
			{
				MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
				Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
				if (this._myRepresentative.MissionPeer.Team == ownerTeam)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_captured"), vec);
					return;
				}
				MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_lost"), vec);
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x00092EC4 File Offset: 0x000910C4
		public void OnMoraleChanged(int attackerMorale, int defenderMorale, int[] capturePointRemainingMoraleGains)
		{
			float num = (float)attackerMorale / 360f;
			float num2 = (float)defenderMorale / 360f;
			SiegeMissionRepresentative myRepresentative = this._myRepresentative;
			if (((myRepresentative != null) ? myRepresentative.MissionPeer.Team : null) != null && this._myRepresentative.MissionPeer.Team.Side != BattleSideEnum.None)
			{
				if ((this._capturePointOwners[this._masterFlag.FlagIndex] == null || this._capturePointOwners[this._masterFlag.FlagIndex].Side != BattleSideEnum.Defender) && this._remainingTimeForBellSoundToStop < 0f)
				{
					if (num2 > this._lastBellSoundPercentage)
					{
						this._lastBellSoundPercentage += 0.2f;
					}
					if (num2 <= 0.4f)
					{
						if (this._lastBellSoundPercentage > 0.4f)
						{
							this._remainingTimeForBellSoundToStop = float.MaxValue;
							this._lastBellSoundPercentage = 0.4f;
						}
					}
					else if (num2 <= 0.6f)
					{
						if (this._lastBellSoundPercentage > 0.6f)
						{
							this._remainingTimeForBellSoundToStop = 8f;
							this._lastBellSoundPercentage = 0.6f;
						}
					}
					else if (num2 <= 0.8f && this._lastBellSoundPercentage > 0.8f)
					{
						this._remainingTimeForBellSoundToStop = 4f;
						this._lastBellSoundPercentage = 0.8f;
					}
					if (this._remainingTimeForBellSoundToStop > 0f)
					{
						BattleSideEnum side = this._myRepresentative.MissionPeer.Team.Side;
						if (side != BattleSideEnum.Defender)
						{
							if (side == BattleSideEnum.Attacker)
							{
								this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_attacker", base.Mission.Scene);
							}
						}
						else
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_defender", base.Mission.Scene);
						}
						MatrixFrame globalFrame = this._masterFlag.GameEntity.GetGlobalFrame();
						this._bellSoundEvent.PlayInPosition(globalFrame.origin + globalFrame.rotation.u * 3f);
					}
				}
				if (!this._battleEndingNotificationGiven || !this._battleEndingLateNotificationGiven)
				{
					float num3 = ((!this._battleEndingNotificationGiven) ? 0.25f : 0.15f);
					MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
					Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
					if (num <= num3 && num2 > num3)
					{
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString((this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Attacker) ? "event:/alerts/report/battle_losing" : "event:/alerts/report/battle_winning"), vec);
						if (this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Attacker)
						{
							MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/multiplayer/retreat_horn_attacker"), this._retreatHornPosition);
						}
						else if (this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Defender)
						{
							MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/multiplayer/retreat_horn_defender"), this._retreatHornPosition);
						}
						if (this._battleEndingNotificationGiven)
						{
							this._battleEndingLateNotificationGiven = true;
						}
						this._battleEndingNotificationGiven = true;
					}
					if (num2 <= num3 && num > num3)
					{
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString((this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Defender) ? "event:/alerts/report/battle_losing" : "event:/alerts/report/battle_winning"), vec);
						if (this._battleEndingNotificationGiven)
						{
							this._battleEndingLateNotificationGiven = true;
						}
						this._battleEndingNotificationGiven = true;
					}
				}
			}
			Action<BattleSideEnum, float> onMoraleChangedEvent = this.OnMoraleChangedEvent;
			if (onMoraleChangedEvent != null)
			{
				onMoraleChangedEvent(BattleSideEnum.Attacker, num);
			}
			Action<BattleSideEnum, float> onMoraleChangedEvent2 = this.OnMoraleChangedEvent;
			if (onMoraleChangedEvent2 != null)
			{
				onMoraleChangedEvent2(BattleSideEnum.Defender, num2);
			}
			Action<int[]> onCapturePointRemainingMoraleGainsChangedEvent = this.OnCapturePointRemainingMoraleGainsChangedEvent;
			if (onCapturePointRemainingMoraleGainsChangedEvent == null)
			{
				return;
			}
			onCapturePointRemainingMoraleGainsChangedEvent(capturePointRemainingMoraleGains);
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x0009322C File Offset: 0x0009142C
		public Team GetFlagOwner(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x0009323B File Offset: 0x0009143B
		public override void OnRemoveBehavior()
		{
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			base.OnRemoveBehavior();
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x0009325C File Offset: 0x0009145C
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._remainingTimeForBellSoundToStop > 0f)
			{
				this._remainingTimeForBellSoundToStop -= dt;
				if (this._remainingTimeForBellSoundToStop <= 0f || base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Playing)
				{
					this._remainingTimeForBellSoundToStop = float.MinValue;
					this._bellSoundEvent.Stop();
					this._bellSoundEvent = null;
				}
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000932C4 File Offset: 0x000914C4
		public List<ItemObject> GetSiegeMissiles()
		{
			List<ItemObject> list = new List<ItemObject>();
			foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<RangedSiegeWeapon>())
			{
				RangedSiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<RangedSiegeWeapon>();
				if (!string.IsNullOrEmpty(firstScriptOfType.MissileItemID))
				{
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MissileItemID);
					if (!list.Contains(@object))
					{
						list.Add(@object);
					}
				}
				foreach (ItemObject itemObject in new List<ItemObject>
				{
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileFlyingId)
				})
				{
					if (!list.Contains(itemObject))
					{
						list.Add(itemObject);
					}
				}
			}
			foreach (WeakGameEntity weakGameEntity2 in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<StonePile>())
			{
				StonePile firstScriptOfType2 = weakGameEntity2.GetFirstScriptOfType<StonePile>();
				if (!string.IsNullOrEmpty(firstScriptOfType2.GivenItemID))
				{
					ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType2.GivenItemID);
					if (!list.Contains(object2))
					{
						list.Add(object2);
					}
				}
			}
			return list;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000934EC File Offset: 0x000916EC
		private void HandleMoraleChangedMessage(GameNetworkMessage baseMessage)
		{
			SiegeMoraleChangeMessage siegeMoraleChangeMessage = (SiegeMoraleChangeMessage)baseMessage;
			this.OnMoraleChanged(siegeMoraleChangeMessage.AttackerMorale, siegeMoraleChangeMessage.DefenderMorale, siegeMoraleChangeMessage.CapturePointRemainingMoraleGains);
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x00093518 File Offset: 0x00091718
		private void HandleServerEventUpdateGold(GameNetworkMessage baseMessage)
		{
			SyncGoldsForSkirmish syncGoldsForSkirmish = (SyncGoldsForSkirmish)baseMessage;
			SiegeMissionRepresentative component = syncGoldsForSkirmish.VirtualPlayer.GetComponent<SiegeMissionRepresentative>();
			this.OnGoldAmountChangedForRepresentative(component, syncGoldsForSkirmish.GoldAmount);
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x00093545 File Offset: 0x00091745
		private void HandleFlagsRemovedMessage(GameNetworkMessage baseMessage)
		{
			this.OnNumberOfFlagsChanged();
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00093550 File Offset: 0x00091750
		private void HandleServerEventPointCapturedMessage(GameNetworkMessage baseMessage)
		{
			FlagDominationCapturePointMessage flagDominationCapturePointMessage = (FlagDominationCapturePointMessage)baseMessage;
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (flagCapturePoint.FlagIndex == flagDominationCapturePointMessage.FlagIndex)
				{
					this.OnCapturePointOwnerChanged(flagCapturePoint, Mission.MissionNetworkHelper.GetTeamFromTeamIndex(flagDominationCapturePointMessage.OwnerTeamIndex));
					break;
				}
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000935C0 File Offset: 0x000917C0
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

		// Token: 0x04000EF7 RID: 3831
		private const float DefenderMoraleDropThresholdIncrement = 0.2f;

		// Token: 0x04000EF8 RID: 3832
		private const float DefenderMoraleDropThresholdLow = 0.4f;

		// Token: 0x04000EF9 RID: 3833
		private const float DefenderMoraleDropThresholdMedium = 0.6f;

		// Token: 0x04000EFA RID: 3834
		private const float DefenderMoraleDropThresholdHigh = 0.8f;

		// Token: 0x04000EFB RID: 3835
		private const float DefenderMoraleDropMediumDuration = 8f;

		// Token: 0x04000EFC RID: 3836
		private const float DefenderMoraleDropHighDuration = 4f;

		// Token: 0x04000EFD RID: 3837
		private const float BattleWinLoseAlertThreshold = 0.25f;

		// Token: 0x04000EFE RID: 3838
		private const float BattleWinLoseLateAlertThreshold = 0.15f;

		// Token: 0x04000EFF RID: 3839
		private const string BattleWinningSoundEventString = "event:/alerts/report/battle_winning";

		// Token: 0x04000F00 RID: 3840
		private const string BattleLosingSoundEventString = "event:/alerts/report/battle_losing";

		// Token: 0x04000F01 RID: 3841
		private const float IndefiniteDurationThreshold = 8f;

		// Token: 0x04000F02 RID: 3842
		private Team[] _capturePointOwners;

		// Token: 0x04000F04 RID: 3844
		private FlagCapturePoint _masterFlag;

		// Token: 0x04000F05 RID: 3845
		private SiegeMissionRepresentative _myRepresentative;

		// Token: 0x04000F06 RID: 3846
		private SoundEvent _bellSoundEvent;

		// Token: 0x04000F07 RID: 3847
		private float _remainingTimeForBellSoundToStop = float.MinValue;

		// Token: 0x04000F08 RID: 3848
		private float _lastBellSoundPercentage = 1f;

		// Token: 0x04000F09 RID: 3849
		private bool _battleEndingNotificationGiven;

		// Token: 0x04000F0A RID: 3850
		private bool _battleEndingLateNotificationGiven;

		// Token: 0x04000F0B RID: 3851
		private Vec3 _retreatHornPosition;
	}
}
