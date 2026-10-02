using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B8 RID: 696
	public class MissionMultiplayerGameModeFlagDominationClient : MissionMultiplayerGameModeBaseClient, ICommanderInfo, IMissionBehavior
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x000917B2 File Offset: 0x0008F9B2
		public override bool IsGameModeUsingGold
		{
			get
			{
				return this.GameType != MultiplayerGameType.Captain;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x000917C0 File Offset: 0x0008F9C0
		public override bool IsGameModeTactical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x000917C3 File Offset: 0x0008F9C3
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x000917C6 File Offset: 0x0008F9C6
		public override MultiplayerGameType GameType
		{
			get
			{
				return this._currentGameType;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000917CE File Offset: 0x0008F9CE
		public override bool IsGameModeUsingCasualGold
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x0600275B RID: 10075 RVA: 0x000917D4 File Offset: 0x0008F9D4
		// (remove) Token: 0x0600275C RID: 10076 RVA: 0x0009180C File Offset: 0x0008FA0C
		public event Action<NetworkCommunicator> OnBotsControlledChangedEvent;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x0600275D RID: 10077 RVA: 0x00091844 File Offset: 0x0008FA44
		// (remove) Token: 0x0600275E RID: 10078 RVA: 0x0009187C File Offset: 0x0008FA7C
		public event Action<BattleSideEnum, float> OnTeamPowerChangedEvent;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x0600275F RID: 10079 RVA: 0x000918B4 File Offset: 0x0008FAB4
		// (remove) Token: 0x06002760 RID: 10080 RVA: 0x000918EC File Offset: 0x0008FAEC
		public event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06002761 RID: 10081 RVA: 0x00091924 File Offset: 0x0008FB24
		// (remove) Token: 0x06002762 RID: 10082 RVA: 0x0009195C File Offset: 0x0008FB5C
		public event Action OnFlagNumberChangedEvent;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06002763 RID: 10083 RVA: 0x00091994 File Offset: 0x0008FB94
		// (remove) Token: 0x06002764 RID: 10084 RVA: 0x000919CC File Offset: 0x0008FBCC
		public event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06002765 RID: 10085 RVA: 0x00091A04 File Offset: 0x0008FC04
		// (remove) Token: 0x06002766 RID: 10086 RVA: 0x00091A3C File Offset: 0x0008FC3C
		public event Action<GoldGain> OnGoldGainEvent;

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x00091A71 File Offset: 0x0008FC71
		// (set) Token: 0x06002768 RID: 10088 RVA: 0x00091A79 File Offset: 0x0008FC79
		public IEnumerable<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x00091A82 File Offset: 0x0008FC82
		public bool AreMoralesIndependent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00091A88 File Offset: 0x0008FC88
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._scoreboardComponent = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			if (MultiplayerOptions.OptionType.SingleSpawn.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
			{
				this._currentGameType = ((MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0) ? MultiplayerGameType.Captain : MultiplayerGameType.Battle);
			}
			else
			{
				this._currentGameType = MultiplayerGameType.Skirmish;
			}
			this.ResetTeamPowers(1f);
			this._capturePointOwners = new Team[3];
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			base.RoundComponent.OnPreparationEnded += this.OnPreparationEnded;
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x00091B2D File Offset: 0x0008FD2D
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.RoundComponent.OnPreparationEnded -= this.OnPreparationEnded;
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x00091B63 File Offset: 0x0008FD63
		private void OnMyClientSynchronized()
		{
			this._myRepresentative = GameNetwork.MyPeer.GetComponent<FlagDominationMissionRepresentative>();
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x00091B75 File Offset: 0x0008FD75
		public override void AfterStart()
		{
			Mission.Current.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00091B84 File Offset: 0x0008FD84
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<BotsControlledChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBotsControlledChangeEvent));
				registerer.RegisterBaseHandler<FlagDominationMoraleChangeMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleMoraleChangedMessage));
				registerer.RegisterBaseHandler<SyncGoldsForSkirmish>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateGold));
				registerer.RegisterBaseHandler<FlagDominationFlagsRemovedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleFlagsRemovedMessage));
				registerer.RegisterBaseHandler<FlagDominationCapturePointMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPointCapturedMessage));
				registerer.RegisterBaseHandler<FormationWipedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventFormationWipedMessage));
				registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPersonalGoldGain));
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00091C18 File Offset: 0x0008FE18
		public void OnPreparationEnded()
		{
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent != null)
			{
				onFlagNumberChangedEvent();
			}
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
				if (onCapturePointOwnerChangedEvent != null)
				{
					onCapturePointOwnerChangedEvent(flagCapturePoint, null);
				}
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x00091C98 File Offset: 0x0008FE98
		public override SpectatorCameraTypes GetMissionCameraLockMode(bool lockedToMainPlayer)
		{
			SpectatorCameraTypes spectatorCameraTypes = SpectatorCameraTypes.Invalid;
			MissionPeer missionPeer = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
			if (!lockedToMainPlayer && missionPeer != null)
			{
				if (missionPeer.Team != base.Mission.SpectatorTeam)
				{
					if (this.GameType == MultiplayerGameType.Captain && base.IsRoundInProgress)
					{
						Formation controlledFormation = missionPeer.ControlledFormation;
						if (controlledFormation != null)
						{
							if (controlledFormation.HasUnitsWithCondition((Agent agent) => !agent.IsPlayerControlled && agent.IsActive()))
							{
								spectatorCameraTypes = SpectatorCameraTypes.LockToPlayerFormation;
							}
						}
					}
				}
				else
				{
					spectatorCameraTypes = SpectatorCameraTypes.Free;
				}
			}
			return spectatorCameraTypes;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x00091D20 File Offset: 0x0008FF20
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (base.IsRoundInProgress && !affectedAgent.IsMount)
			{
				Team team = affectedAgent.Team;
				if (this.IsGameModeUsingGold)
				{
					this.UpdateTeamPowerBasedOnGold(team);
					return;
				}
				this.UpdateTeamPowerBasedOnTroopCount(team);
			}
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x00091D5C File Offset: 0x0008FF5C
		public override void OnClearScene()
		{
			this._informedAboutFlagRemoval = false;
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				this._capturePointOwners[flagCapturePoint.FlagIndex] = null;
			}
			this.ResetTeamPowers(1f);
			if (this._bellSoundEvent != null)
			{
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x00091DFC File Offset: 0x0008FFFC
		protected override int GetWarningTimer()
		{
			int num = 0;
			if (base.IsRoundInProgress)
			{
				float num2 = -1f;
				switch (this.GameType)
				{
				case MultiplayerGameType.Battle:
					num2 = 210f;
					break;
				case MultiplayerGameType.Captain:
					num2 = 180f;
					break;
				case MultiplayerGameType.Skirmish:
					num2 = 120f;
					break;
				default:
					Debug.FailedAssert("A flag domination mode cannot be " + this.GameType + ".", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ClientGameModeLogics\\MissionMultiplayerGameModeFlagDominationClient.cs", "GetWarningTimer", 207);
					break;
				}
				float num3 = (float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) - num2;
				float num4 = num3 + 30f;
				if (base.RoundComponent.RemainingRoundTime <= num4 && base.RoundComponent.RemainingRoundTime > num3)
				{
					num = MathF.Ceiling(30f - (num4 - base.RoundComponent.RemainingRoundTime));
					if (!this._informedAboutFlagRemoval)
					{
						this._informedAboutFlagRemoval = true;
						base.NotificationsComponent.FlagsWillBeRemovedInXSeconds(30);
					}
				}
			}
			return num;
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x00091EE7 File Offset: 0x000900E7
		public Team GetFlagOwner(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x00091EF8 File Offset: 0x000900F8
		private void HandleServerEventBotsControlledChangeEvent(GameNetworkMessage baseMessage)
		{
			BotsControlledChange botsControlledChange = (BotsControlledChange)baseMessage;
			MissionPeer component = botsControlledChange.Peer.GetComponent<MissionPeer>();
			this.OnBotsControlledChanged(component, botsControlledChange.AliveCount, botsControlledChange.TotalCount);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x00091F2C File Offset: 0x0009012C
		private void HandleMoraleChangedMessage(GameNetworkMessage baseMessage)
		{
			FlagDominationMoraleChangeMessage flagDominationMoraleChangeMessage = (FlagDominationMoraleChangeMessage)baseMessage;
			this.OnMoraleChanged(flagDominationMoraleChangeMessage.Morale);
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x00091F4C File Offset: 0x0009014C
		private void HandleServerEventUpdateGold(GameNetworkMessage baseMessage)
		{
			SyncGoldsForSkirmish syncGoldsForSkirmish = (SyncGoldsForSkirmish)baseMessage;
			FlagDominationMissionRepresentative component = syncGoldsForSkirmish.VirtualPlayer.GetComponent<FlagDominationMissionRepresentative>();
			this.OnGoldAmountChangedForRepresentative(component, syncGoldsForSkirmish.GoldAmount);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x00091F79 File Offset: 0x00090179
		private void HandleFlagsRemovedMessage(GameNetworkMessage baseMessage)
		{
			this.OnNumberOfFlagsChanged();
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x00091F84 File Offset: 0x00090184
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

		// Token: 0x0600277A RID: 10106 RVA: 0x00091FF4 File Offset: 0x000901F4
		private void HandleServerEventFormationWipedMessage(GameNetworkMessage baseMessage)
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/squad_wiped"), vec);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00092034 File Offset: 0x00090234
		private void HandleServerEventPersonalGoldGain(GameNetworkMessage baseMessage)
		{
			GoldGain goldGain = (GoldGain)baseMessage;
			Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
			if (onGoldGainEvent == null)
			{
				return;
			}
			onGoldGainEvent(goldGain);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x00092059 File Offset: 0x00090259
		public void OnTeamPowerChanged(BattleSideEnum teamSide, float power)
		{
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(teamSide, power);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x00092070 File Offset: 0x00090270
		public void OnMoraleChanged(float morale)
		{
			for (int i = 0; i < 2; i++)
			{
				float num = (morale + 1f) / 2f;
				if (i == 0)
				{
					Action<BattleSideEnum, float> onMoraleChangedEvent = this.OnMoraleChangedEvent;
					if (onMoraleChangedEvent != null)
					{
						onMoraleChangedEvent(BattleSideEnum.Defender, 1f - num);
					}
				}
				else if (i == 1)
				{
					Action<BattleSideEnum, float> onMoraleChangedEvent2 = this.OnMoraleChangedEvent;
					if (onMoraleChangedEvent2 != null)
					{
						onMoraleChangedEvent2(BattleSideEnum.Attacker, num);
					}
				}
			}
			FlagDominationMissionRepresentative myRepresentative = this._myRepresentative;
			if (((myRepresentative != null) ? myRepresentative.MissionPeer.Team : null) != null && this._myRepresentative.MissionPeer.Team.Side != BattleSideEnum.None)
			{
				float num2 = MathF.Abs(morale);
				if (this._remainingTimeForBellSoundToStop < 0f)
				{
					if (num2 >= 0.6f && num2 < 1f)
					{
						this._remainingTimeForBellSoundToStop = float.MaxValue;
					}
					else
					{
						this._remainingTimeForBellSoundToStop = float.MinValue;
					}
					if (this._remainingTimeForBellSoundToStop > 0f)
					{
						BattleSideEnum side = this._myRepresentative.MissionPeer.Team.Side;
						if ((side == BattleSideEnum.Defender && morale >= 0.6f) || (side == BattleSideEnum.Attacker && morale <= -0.6f))
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_defender", base.Mission.Scene);
						}
						else
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_attacker", base.Mission.Scene);
						}
						MatrixFrame globalFrame = this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated).GetRandomElementInefficiently<FlagCapturePoint>().GameEntity.GetGlobalFrame();
						this._bellSoundEvent.PlayInPosition(globalFrame.origin + globalFrame.rotation.u * 3f);
						return;
					}
				}
				else if (num2 >= 1f || num2 < 0.6f)
				{
					this._remainingTimeForBellSoundToStop = float.MinValue;
				}
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00092240 File Offset: 0x00090440
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
			if (representative != null)
			{
				MissionPeer component = representative.GetComponent<MissionPeer>();
				if (component != null)
				{
					representative.UpdateGold(goldAmount);
					this._scoreboardComponent.PlayerPropertiesChanged(component);
					if (this.IsGameModeUsingGold && base.IsRoundInProgress && component.Team != null && component.Team.Side != BattleSideEnum.None)
					{
						this.UpdateTeamPowerBasedOnGold(component.Team);
					}
				}
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x0009229F File Offset: 0x0009049F
		public void OnNumberOfFlagsChanged()
		{
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent == null)
			{
				return;
			}
			onFlagNumberChangedEvent();
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000922B1 File Offset: 0x000904B1
		public void OnBotsControlledChanged(MissionPeer missionPeer, int botAliveCount, int botTotalCount)
		{
			missionPeer.BotsUnderControlAlive = botAliveCount;
			missionPeer.BotsUnderControlTotal = botTotalCount;
			Action<NetworkCommunicator> onBotsControlledChangedEvent = this.OnBotsControlledChangedEvent;
			if (onBotsControlledChangedEvent == null)
			{
				return;
			}
			onBotsControlledChangedEvent(missionPeer.GetNetworkPeer());
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000922D8 File Offset: 0x000904D8
		public void OnCapturePointOwnerChanged(FlagCapturePoint flagCapturePoint, Team ownerTeam)
		{
			this._capturePointOwners[flagCapturePoint.FlagIndex] = ownerTeam;
			Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
			if (onCapturePointOwnerChangedEvent != null)
			{
				onCapturePointOwnerChangedEvent(flagCapturePoint, ownerTeam);
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

		// Token: 0x06002782 RID: 10114 RVA: 0x00092378 File Offset: 0x00090578
		public void OnRequestForfeitSpawn()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestForfeitSpawn());
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Mission.Current.GetMissionBehavior<MissionMultiplayerFlagDomination>().ForfeitSpawning(GameNetwork.MyPeer);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000923AA File Offset: 0x000905AA
		private void ResetTeamPowers(float value = 1f)
		{
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent != null)
			{
				onTeamPowerChangedEvent(BattleSideEnum.Attacker, value);
			}
			Action<BattleSideEnum, float> onTeamPowerChangedEvent2 = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent2 == null)
			{
				return;
			}
			onTeamPowerChangedEvent2(BattleSideEnum.Defender, value);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000923D4 File Offset: 0x000905D4
		private void UpdateTeamPowerBasedOnGold(Team team)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team.Side == team.Side)
				{
					int gold = component.GetComponent<FlagDominationMissionRepresentative>().Gold;
					if (gold >= 100)
					{
						num2 += gold;
					}
					if (component.ControlledAgent != null && component.ControlledAgent.IsActive())
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(component.ControlledAgent.Character);
						num2 += ((this._currentGameType == MultiplayerGameType.Battle) ? mpheroClassForCharacter.TroopBattleCost : mpheroClassForCharacter.TroopCost);
					}
					num++;
				}
			}
			if (this._currentGameType == MultiplayerGameType.Battle)
			{
				num3 = 120;
			}
			else
			{
				num3 = 300;
			}
			num += ((team.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.MissionPeer == null)
					{
						num2 += num3;
					}
				}
			}
			int num4 = num * num3;
			float num5 = ((num4 == 0) ? 0f : ((float)num2 / (float)num4));
			num5 = MathF.Min(1f, num5);
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(team.Side, num5);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x0009256C File Offset: 0x0009076C
		private void UpdateTeamPowerBasedOnTroopCount(Team team)
		{
			int count = team.ActiveAgents.Count;
			int num = count + team.QuerySystem.DeathCount;
			float num2 = (float)count / (float)num;
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(team.Side, num2);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000925B0 File Offset: 0x000907B0
		public override List<CompassItemUpdateParams> GetCompassTargets()
		{
			List<CompassItemUpdateParams> list = new List<CompassItemUpdateParams>();
			if (!GameNetwork.IsMyPeerReady || !base.IsRoundInProgress)
			{
				return list;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component == null || component.Team == null || component.Team.Side == BattleSideEnum.None)
			{
				return list;
			}
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated))
			{
				int num = 17 + flagCapturePoint.FlagIndex;
				list.Add(new CompassItemUpdateParams(flagCapturePoint, (TargetIconType)num, flagCapturePoint.Position, flagCapturePoint.GetFlagColor(), flagCapturePoint.GetFlagColor2()));
			}
			bool flag = true;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
				if (((component2 != null) ? component2.Team : null) != null && component2.Team.Side != BattleSideEnum.None)
				{
					bool flag2 = component2.ControlledFormation != null;
					if (!flag2)
					{
						flag = false;
					}
					if (flag || component2.Team == component.Team)
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component2, false);
						if (flag2)
						{
							Formation controlledFormation = component2.ControlledFormation;
							if (controlledFormation.CountOfUnits != 0)
							{
								WorldPosition cachedMedianPosition = controlledFormation.CachedMedianPosition;
								Vec2 vec = controlledFormation.SmoothedAverageUnitPosition;
								if (!vec.IsValid)
								{
									vec = controlledFormation.CachedAveragePosition;
								}
								cachedMedianPosition.SetVec2(vec);
								Banner banner = null;
								bool flag3 = false;
								bool flag4 = false;
								if (controlledFormation.Team != null)
								{
									if (controlledFormation.Banner == null)
									{
										controlledFormation.Banner = new Banner(controlledFormation.BannerCode, controlledFormation.Team.Color, controlledFormation.Team.Color2);
									}
									flag3 = controlledFormation.Team.IsAttacker;
									flag4 = controlledFormation.Team.IsPlayerAlly;
									banner = controlledFormation.Banner;
								}
								TargetIconType targetIconType = ((mpheroClassForPeer != null) ? mpheroClassForPeer.IconType : TargetIconType.None);
								list.Add(new CompassItemUpdateParams(controlledFormation, targetIconType, cachedMedianPosition.GetNavMeshVec3(), banner, flag3, flag4));
							}
						}
						else
						{
							Agent controlledAgent = component2.ControlledAgent;
							if (controlledAgent != null && controlledAgent.IsActive() && controlledAgent.Controller != AgentControllerType.Player)
							{
								Banner banner2 = new Banner(component2.Peer.BannerCode, component2.Team.Color, component2.Team.Color2);
								list.Add(new CompassItemUpdateParams(controlledAgent, mpheroClassForPeer.IconType, controlledAgent.Position, banner2, component2.Team.IsAttacker, component2.Team.IsPlayerAlly));
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000928A8 File Offset: 0x00090AA8
		public override int GetGoldAmount()
		{
			if (this._myRepresentative != null)
			{
				return this._myRepresentative.Gold;
			}
			return 0;
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000928C0 File Offset: 0x00090AC0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._remainingTimeForBellSoundToStop > 0f)
			{
				this._remainingTimeForBellSoundToStop -= dt;
			}
			if (this._bellSoundEvent != null && (this._remainingTimeForBellSoundToStop <= 0f || base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Playing))
			{
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
			}
		}

		// Token: 0x04000EE9 RID: 3817
		private const float MySideMoraleDropThreshold = 0.4f;

		// Token: 0x04000EEA RID: 3818
		private float _remainingTimeForBellSoundToStop = float.MinValue;

		// Token: 0x04000EEB RID: 3819
		private SoundEvent _bellSoundEvent;

		// Token: 0x04000EEC RID: 3820
		private FlagDominationMissionRepresentative _myRepresentative;

		// Token: 0x04000EED RID: 3821
		private MissionScoreboardComponent _scoreboardComponent;

		// Token: 0x04000EEE RID: 3822
		private MultiplayerGameType _currentGameType;

		// Token: 0x04000EEF RID: 3823
		private Team[] _capturePointOwners;

		// Token: 0x04000EF1 RID: 3825
		private bool _informedAboutFlagRemoval;
	}
}
