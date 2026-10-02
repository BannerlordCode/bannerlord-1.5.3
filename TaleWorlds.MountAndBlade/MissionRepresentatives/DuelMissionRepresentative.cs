using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade.MissionRepresentatives
{
	// Token: 0x020003CC RID: 972
	public class DuelMissionRepresentative : MissionRepresentativeBase
	{
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x000E285B File Offset: 0x000E0A5B
		// (set) Token: 0x060036CF RID: 14031 RVA: 0x000E2863 File Offset: 0x000E0A63
		public int Bounty { get; private set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060036D0 RID: 14032 RVA: 0x000E286C File Offset: 0x000E0A6C
		// (set) Token: 0x060036D1 RID: 14033 RVA: 0x000E2874 File Offset: 0x000E0A74
		public int Score { get; private set; }

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x000E287D File Offset: 0x000E0A7D
		// (set) Token: 0x060036D3 RID: 14035 RVA: 0x000E2885 File Offset: 0x000E0A85
		public int NumberOfWins { get; private set; }

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060036D4 RID: 14036 RVA: 0x000E288E File Offset: 0x000E0A8E
		private bool _isInDuel
		{
			get
			{
				return base.MissionPeer != null && base.MissionPeer.Team != null && base.MissionPeer.Team.IsDefender;
			}
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000E28B7 File Offset: 0x000E0AB7
		public override void Initialize()
		{
			this._requesters = new List<Tuple<MissionPeer, MissionTime>>();
			if (GameNetwork.IsServerOrRecorder)
			{
				this._missionMultiplayerDuel = Mission.Current.GetMissionBehavior<MissionMultiplayerDuel>();
			}
			Mission.Current.SetMissionMode(MissionMode.Duel, true);
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000E28E8 File Offset: 0x000E0AE8
		public void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
				networkMessageHandlerRegisterer.Register<NetworkMessages.FromServer.DuelRequest>(new GameNetworkMessage.ServerMessageHandlerDelegate<NetworkMessages.FromServer.DuelRequest>(this.HandleServerEventDuelRequest));
				networkMessageHandlerRegisterer.Register<DuelSessionStarted>(new GameNetworkMessage.ServerMessageHandlerDelegate<DuelSessionStarted>(this.HandleServerEventDuelSessionStarted));
				networkMessageHandlerRegisterer.Register<DuelPreparationStartedForTheFirstTime>(new GameNetworkMessage.ServerMessageHandlerDelegate<DuelPreparationStartedForTheFirstTime>(this.HandleServerEventDuelStarted));
				networkMessageHandlerRegisterer.Register<DuelEnded>(new GameNetworkMessage.ServerMessageHandlerDelegate<DuelEnded>(this.HandleServerEventDuelEnded));
				networkMessageHandlerRegisterer.Register<DuelRoundEnded>(new GameNetworkMessage.ServerMessageHandlerDelegate<DuelRoundEnded>(this.HandleServerEventDuelRoundEnded));
				networkMessageHandlerRegisterer.Register<DuelPointsUpdateMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<DuelPointsUpdateMessage>(this.HandleServerPointUpdate));
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000E2970 File Offset: 0x000E0B70
		public void OnInteraction()
		{
			if (this._focusedObject != null)
			{
				DuelZoneLandmark duelZoneLandmark;
				Agent focusedAgent;
				if ((focusedAgent = this._focusedObject as Agent) != null)
				{
					if (focusedAgent.IsActive())
					{
						if (this._requesters.Any<Tuple<MissionPeer, MissionTime>>((Tuple<MissionPeer, MissionTime> req) => req.Item1 == focusedAgent.MissionPeer))
						{
							for (int i = 0; i < this._requesters.Count; i++)
							{
								if (this._requesters[i].Item1 == base.MissionPeer)
								{
									this._requesters.Remove(this._requesters[i]);
									break;
								}
							}
							MissionRepresentativeBase.PlayerTypes playerTypes = base.PlayerType;
							if (playerTypes == MissionRepresentativeBase.PlayerTypes.Client)
							{
								GameNetwork.BeginModuleEventAsClient();
								GameNetwork.WriteMessage(new DuelResponse(focusedAgent.MissionRepresentative.Peer.Communicator as NetworkCommunicator, true));
								GameNetwork.EndModuleEventAsClient();
								return;
							}
							if (playerTypes != MissionRepresentativeBase.PlayerTypes.Server)
							{
								return;
							}
							this._missionMultiplayerDuel.DuelRequestAccepted(focusedAgent, base.ControlledAgent);
							return;
						}
						else
						{
							MissionRepresentativeBase.PlayerTypes playerTypes = base.PlayerType;
							if (playerTypes == MissionRepresentativeBase.PlayerTypes.Client)
							{
								Action<MissionPeer> onDuelRequestSentEvent = this.OnDuelRequestSentEvent;
								if (onDuelRequestSentEvent != null)
								{
									onDuelRequestSentEvent(focusedAgent.MissionPeer);
								}
								GameNetwork.BeginModuleEventAsClient();
								GameNetwork.WriteMessage(new NetworkMessages.FromClient.DuelRequest(focusedAgent.Index));
								GameNetwork.EndModuleEventAsClient();
								return;
							}
							if (playerTypes != MissionRepresentativeBase.PlayerTypes.Server)
							{
								return;
							}
							this._missionMultiplayerDuel.DuelRequestReceived(base.MissionPeer, focusedAgent.MissionPeer);
							return;
						}
					}
				}
				else if ((duelZoneLandmark = this._focusedObject as DuelZoneLandmark) != null)
				{
					if (this._isInDuel)
					{
						InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=v5EqMSlD}Can't change arena preference while in duel.", null).ToString()));
						return;
					}
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new RequestChangePreferredTroopType(duelZoneLandmark.ZoneTroopType));
					GameNetwork.EndModuleEventAsClient();
					Action<TroopType> onMyPreferredZoneChanged = this.OnMyPreferredZoneChanged;
					if (onMyPreferredZoneChanged == null)
					{
						return;
					}
					onMyPreferredZoneChanged(duelZoneLandmark.ZoneTroopType);
				}
			}
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000E2B44 File Offset: 0x000E0D44
		private void HandleServerEventDuelRequest(NetworkMessages.FromServer.DuelRequest message)
		{
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(message.RequesterAgentIndex, false);
			Mission.MissionNetworkHelper.GetAgentFromIndex(message.RequestedAgentIndex, false);
			this.DuelRequested(agentFromIndex, message.SelectedAreaTroopType);
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000E2B78 File Offset: 0x000E0D78
		private void HandleServerEventDuelSessionStarted(DuelSessionStarted message)
		{
			this.OnDuelPreparation(message.RequesterPeer.GetComponent<MissionPeer>(), message.RequestedPeer.GetComponent<MissionPeer>());
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000E2B98 File Offset: 0x000E0D98
		private void HandleServerEventDuelStarted(DuelPreparationStartedForTheFirstTime message)
		{
			MissionPeer component = message.RequesterPeer.GetComponent<MissionPeer>();
			MissionPeer component2 = message.RequesteePeer.GetComponent<MissionPeer>();
			Action<MissionPeer, MissionPeer, int> onDuelPreparationStartedForTheFirstTimeEvent = this.OnDuelPreparationStartedForTheFirstTimeEvent;
			if (onDuelPreparationStartedForTheFirstTimeEvent == null)
			{
				return;
			}
			onDuelPreparationStartedForTheFirstTimeEvent(component, component2, message.AreaIndex);
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000E2BD5 File Offset: 0x000E0DD5
		private void HandleServerEventDuelEnded(DuelEnded message)
		{
			Action<MissionPeer> onDuelEndedEvent = this.OnDuelEndedEvent;
			if (onDuelEndedEvent == null)
			{
				return;
			}
			onDuelEndedEvent(message.WinnerPeer.GetComponent<MissionPeer>());
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000E2BF2 File Offset: 0x000E0DF2
		private void HandleServerEventDuelRoundEnded(DuelRoundEnded message)
		{
			Action<MissionPeer> onDuelRoundEndedEvent = this.OnDuelRoundEndedEvent;
			if (onDuelRoundEndedEvent == null)
			{
				return;
			}
			onDuelRoundEndedEvent(message.WinnerPeer.GetComponent<MissionPeer>());
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000E2C0F File Offset: 0x000E0E0F
		private void HandleServerPointUpdate(DuelPointsUpdateMessage message)
		{
			DuelMissionRepresentative component = message.NetworkCommunicator.GetComponent<DuelMissionRepresentative>();
			component.Bounty = message.Bounty;
			component.Score = message.Score;
			component.NumberOfWins = message.NumberOfWins;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000E2C40 File Offset: 0x000E0E40
		public void DuelRequested(Agent requesterAgent, TroopType selectedAreaTroopType)
		{
			this._requesters.Add(new Tuple<MissionPeer, MissionTime>(requesterAgent.MissionPeer, MissionTime.Now + MissionTime.Seconds(10f)));
			switch (base.PlayerType)
			{
			case MissionRepresentativeBase.PlayerTypes.Bot:
				this._missionMultiplayerDuel.DuelRequestAccepted(requesterAgent, base.ControlledAgent);
				return;
			case MissionRepresentativeBase.PlayerTypes.Client:
			{
				if (!base.IsMine)
				{
					GameNetwork.BeginModuleEventAsServer(base.Peer);
					GameNetwork.WriteMessage(new NetworkMessages.FromServer.DuelRequest(requesterAgent.Index, base.ControlledAgent.Index, selectedAreaTroopType));
					GameNetwork.EndModuleEventAsServer();
					return;
				}
				Action<MissionPeer, TroopType> onDuelRequestedEvent = this.OnDuelRequestedEvent;
				if (onDuelRequestedEvent == null)
				{
					return;
				}
				onDuelRequestedEvent(requesterAgent.MissionPeer, selectedAreaTroopType);
				return;
			}
			case MissionRepresentativeBase.PlayerTypes.Server:
			{
				Action<MissionPeer, TroopType> onDuelRequestedEvent2 = this.OnDuelRequestedEvent;
				if (onDuelRequestedEvent2 == null)
				{
					return;
				}
				onDuelRequestedEvent2(requesterAgent.MissionPeer, selectedAreaTroopType);
				return;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000E2D10 File Offset: 0x000E0F10
		public bool CheckHasRequestFromAndRemoveRequestIfNeeded(MissionPeer requestOwner)
		{
			if (requestOwner != null && requestOwner.Representative == this)
			{
				this._requesters.Clear();
				return false;
			}
			Tuple<MissionPeer, MissionTime> tuple = this._requesters.FirstOrDefault<Tuple<MissionPeer, MissionTime>>((Tuple<MissionPeer, MissionTime> req) => req.Item1 == requestOwner);
			if (tuple == null)
			{
				return false;
			}
			if (requestOwner.ControlledAgent == null || !requestOwner.ControlledAgent.IsActive())
			{
				this._requesters.Remove(tuple);
				return false;
			}
			if (!tuple.Item2.IsPast)
			{
				return true;
			}
			this._requesters.Remove(tuple);
			return false;
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000E2DB8 File Offset: 0x000E0FB8
		public void OnDuelPreparation(MissionPeer requesterPeer, MissionPeer requesteePeer)
		{
			MissionRepresentativeBase.PlayerTypes playerType = base.PlayerType;
			if (playerType != MissionRepresentativeBase.PlayerTypes.Client)
			{
				if (playerType == MissionRepresentativeBase.PlayerTypes.Server)
				{
					Action<MissionPeer, int> onDuelPrepStartedEvent = this.OnDuelPrepStartedEvent;
					if (onDuelPrepStartedEvent != null)
					{
						onDuelPrepStartedEvent((base.MissionPeer == requesterPeer) ? requesteePeer : requesterPeer, 3);
					}
				}
			}
			else if (base.IsMine)
			{
				Action<MissionPeer, int> onDuelPrepStartedEvent2 = this.OnDuelPrepStartedEvent;
				if (onDuelPrepStartedEvent2 != null)
				{
					onDuelPrepStartedEvent2((base.MissionPeer == requesterPeer) ? requesteePeer : requesterPeer, 3);
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new DuelSessionStarted(requesterPeer.GetNetworkPeer(), requesteePeer.GetNetworkPeer()));
				GameNetwork.EndModuleEventAsServer();
			}
			Tuple<MissionPeer, MissionTime> tuple = this._requesters.FirstOrDefault<Tuple<MissionPeer, MissionTime>>((Tuple<MissionPeer, MissionTime> req) => req.Item1 == requesterPeer);
			if (tuple != null)
			{
				this._requesters.Remove(tuple);
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000E2E97 File Offset: 0x000E1097
		public void OnObjectFocused(IFocusable focusedObject)
		{
			this._focusedObject = focusedObject;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000E2EA0 File Offset: 0x000E10A0
		public void OnObjectFocusLost()
		{
			this._focusedObject = null;
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000E2EA9 File Offset: 0x000E10A9
		public override void OnAgentSpawned()
		{
			if (base.ControlledAgent.Team != null && base.ControlledAgent.Team.Side == BattleSideEnum.Attacker)
			{
				Action onAgentSpawnedWithoutDuelEvent = this.OnAgentSpawnedWithoutDuelEvent;
				if (onAgentSpawnedWithoutDuelEvent == null)
				{
					return;
				}
				onAgentSpawnedWithoutDuelEvent();
			}
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000E2EDB File Offset: 0x000E10DB
		public void ResetBountyAndNumberOfWins()
		{
			this.Bounty = 0;
			this.NumberOfWins = 0;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000E2EEC File Offset: 0x000E10EC
		public void OnDuelWon(float gainedScore)
		{
			this.Bounty += (int)(gainedScore / 5f);
			this.Score += (int)gainedScore;
			int numberOfWins = this.NumberOfWins;
			this.NumberOfWins = numberOfWins + 1;
		}

		// Token: 0x04001784 RID: 6020
		public const int DuelPrepTime = 3;

		// Token: 0x04001785 RID: 6021
		public Action<MissionPeer, TroopType> OnDuelRequestedEvent;

		// Token: 0x04001786 RID: 6022
		public Action<MissionPeer> OnDuelRequestSentEvent;

		// Token: 0x04001787 RID: 6023
		public Action<MissionPeer, int> OnDuelPrepStartedEvent;

		// Token: 0x04001788 RID: 6024
		public Action OnAgentSpawnedWithoutDuelEvent;

		// Token: 0x04001789 RID: 6025
		public Action<MissionPeer, MissionPeer, int> OnDuelPreparationStartedForTheFirstTimeEvent;

		// Token: 0x0400178A RID: 6026
		public Action<MissionPeer> OnDuelEndedEvent;

		// Token: 0x0400178B RID: 6027
		public Action<MissionPeer> OnDuelRoundEndedEvent;

		// Token: 0x0400178C RID: 6028
		public Action<TroopType> OnMyPreferredZoneChanged;

		// Token: 0x0400178D RID: 6029
		private List<Tuple<MissionPeer, MissionTime>> _requesters;

		// Token: 0x0400178E RID: 6030
		private MissionMultiplayerDuel _missionMultiplayerDuel;

		// Token: 0x0400178F RID: 6031
		private IFocusable _focusedObject;
	}
}
