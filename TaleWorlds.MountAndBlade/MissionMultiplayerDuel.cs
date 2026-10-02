using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BB RID: 699
	public class MissionMultiplayerDuel : MissionMultiplayerGameModeBase
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000938EF File Offset: 0x00091AEF
		public override bool IsGameModeHidingAllAgentVisuals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000938F2 File Offset: 0x00091AF2
		public override bool IsGameModeUsingOpposingTeams
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x060027C1 RID: 10177 RVA: 0x000938F8 File Offset: 0x00091AF8
		// (remove) Token: 0x060027C2 RID: 10178 RVA: 0x00093930 File Offset: 0x00091B30
		public event MissionMultiplayerDuel.OnDuelEndedDelegate OnDuelEnded;

		// Token: 0x060027C3 RID: 10179 RVA: 0x00093965 File Offset: 0x00091B65
		public override MultiplayerGameType GetMissionType()
		{
			return MultiplayerGameType.Duel;
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x00093968 File Offset: 0x00091B68
		public override void AfterStart()
		{
			base.AfterStart();
			Mission.Current.SetMissionCorpseFadeOutTimeInSeconds(1f);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			Banner banner = new Banner(@object.Banner, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint, banner, false, false, true);
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x00093A04 File Offset: 0x00091C04
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._duelAreaFlags.AddRange(Mission.Current.Scene.FindEntitiesWithTagExpression("area_flag(_\\d+)*"));
			List<GameEntity> list = new List<GameEntity>();
			list.AddRange(Mission.Current.Scene.FindEntitiesWithTagExpression("area_box(_\\d+)*"));
			this._cachedSelectedAreaFlags = new KeyValuePair<int, TroopType>[this._duelAreaFlags.Count];
			for (int i = 0; i < list.Count; i++)
			{
				VolumeBox firstScriptOfType = list[i].GetFirstScriptOfType<VolumeBox>();
				this._areaBoxes.Add(firstScriptOfType);
			}
			this._cachedSelectedVolumeBoxes = new VolumeBox[this._areaBoxes.Count];
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00093AAC File Offset: 0x00091CAC
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			registerer.RegisterBaseHandler<NetworkMessages.FromClient.DuelRequest>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequest));
			registerer.RegisterBaseHandler<DuelResponse>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequestAccepted));
			registerer.RegisterBaseHandler<RequestChangePreferredTroopType>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequestChangePreferredTroopType));
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00093AE4 File Offset: 0x00091CE4
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			networkPeer.AddComponent<DuelMissionRepresentative>();
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00093AF0 File Offset: 0x00091CF0
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (!networkPeer.IsSpectator)
			{
				component.Team = base.Mission.AttackerTeam;
				this._peersAndSelections.Add(new KeyValuePair<MissionPeer, TroopType>(component, TroopType.Invalid));
				return;
			}
			if (base.Mission.SpectatorTeam != null)
			{
				component.Team = base.Mission.SpectatorTeam;
				return;
			}
			Debug.FailedAssert("Spectator joined a duel mission but Mission.SpectatorTeam is null; the peer will stay teamless.", "MissionMultiplayerDuel.cs", "HandleNewClientAfterSynchronized", 390);
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00093B68 File Offset: 0x00091D68
		private bool HandleClientEventDuelRequest(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.DuelRequest duelRequest = (NetworkMessages.FromClient.DuelRequest)baseMessage;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(duelRequest.RequestedAgentIndex, false);
				if (agentFromIndex != null && agentFromIndex.IsActive())
				{
					this.DuelRequestReceived(missionPeer, agentFromIndex.MissionPeer);
				}
			}
			return true;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00093BB4 File Offset: 0x00091DB4
		private bool HandleClientEventDuelRequestAccepted(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			DuelResponse duelResponse = (DuelResponse)baseMessage;
			if (((peer != null) ? peer.GetComponent<MissionPeer>() : null) != null && peer.GetComponent<MissionPeer>().ControlledAgent != null)
			{
				NetworkCommunicator peer2 = duelResponse.Peer;
				if (((peer2 != null) ? peer2.GetComponent<MissionPeer>() : null) != null && duelResponse.Peer.GetComponent<MissionPeer>().ControlledAgent != null)
				{
					this.DuelRequestAccepted(duelResponse.Peer.GetComponent<DuelMissionRepresentative>().ControlledAgent, peer.GetComponent<DuelMissionRepresentative>().ControlledAgent);
				}
			}
			return true;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00093C2C File Offset: 0x00091E2C
		private bool HandleClientEventDuelRequestChangePreferredTroopType(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestChangePreferredTroopType requestChangePreferredTroopType = (RequestChangePreferredTroopType)baseMessage;
			this.OnPeerSelectedPreferredTroopType(peer.GetComponent<MissionPeer>(), requestChangePreferredTroopType.TroopType);
			return true;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00093C54 File Offset: 0x00091E54
		public override bool CheckIfPlayerCanDespawn(MissionPeer missionPeer)
		{
			for (int i = 0; i < this._activeDuels.Count; i++)
			{
				if (this._activeDuels[i].IsPeerInThisDuel(missionPeer))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00093C8E File Offset: 0x00091E8E
		public void OnPlayerDespawn(MissionPeer missionPeer)
		{
			missionPeer.GetComponent<DuelMissionRepresentative>();
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x00093C98 File Offset: 0x00091E98
		public void DuelRequestReceived(MissionPeer requesterPeer, MissionPeer requesteePeer)
		{
			if (!this.IsThereARequestBetweenPeers(requesterPeer, requesteePeer) && !this.IsHavingDuel(requesterPeer) && !this.IsHavingDuel(requesteePeer))
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = new MissionMultiplayerDuel.DuelInfo(requesterPeer, requesteePeer, this.GetNextAvailableDuelAreaIndex(requesterPeer.ControlledAgent));
				this._duelRequests.Add(duelInfo);
				(requesteePeer.Representative as DuelMissionRepresentative).DuelRequested(requesterPeer.ControlledAgent, duelInfo.DuelAreaTroopType);
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00093D00 File Offset: 0x00091F00
		private KeyValuePair<int, TroopType> GetNextAvailableDuelAreaIndex(Agent requesterAgent)
		{
			TroopType troopType = TroopType.Invalid;
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == requesterAgent.MissionPeer)
				{
					troopType = this._peersAndSelections[i].Value;
					break;
				}
			}
			if (troopType == TroopType.Invalid)
			{
				troopType = this.GetAgentTroopType(requesterAgent);
			}
			bool flag = false;
			int num = 0;
			for (int j = 0; j < this._duelAreaFlags.Count; j++)
			{
				GameEntity gameEntity = this._duelAreaFlags[j];
				int num2 = int.Parse(gameEntity.Tags.Single<string>((string ft) => ft.StartsWith("area_flag_")).Replace("area_flag_", ""));
				int flagIndex = num2 - 1;
				if (this._activeDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex) && this._restartingDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex) && this._restartPreparationDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex))
				{
					TroopType troopType2 = (gameEntity.HasTag("flag_infantry") ? TroopType.Infantry : (gameEntity.HasTag("flag_archery") ? TroopType.Ranged : TroopType.Cavalry));
					if (!flag && troopType2 == troopType)
					{
						flag = true;
						num = 0;
					}
					if (!flag || troopType2 == troopType)
					{
						this._cachedSelectedAreaFlags[num] = new KeyValuePair<int, TroopType>(flagIndex, troopType2);
						num++;
					}
				}
			}
			return this._cachedSelectedAreaFlags[MBRandom.RandomInt(num)];
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x00093EA8 File Offset: 0x000920A8
		public void DuelRequestAccepted(Agent requesterAgent, Agent requesteeAgent)
		{
			MissionMultiplayerDuel.DuelInfo duelInfo = this._duelRequests.FirstOrDefault<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo dr) => dr.IsPeerInThisDuel(requesterAgent.MissionPeer) && dr.IsPeerInThisDuel(requesteeAgent.MissionPeer));
			if (duelInfo != null)
			{
				this.PrepareDuel(duelInfo);
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00093EEB File Offset: 0x000920EB
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.CheckRestartPreparationDuels();
			this.CheckForRestartingDuels();
			this.CheckDuelsToStart();
			this.CheckDuelRequestTimeouts();
			this.CheckEndedDuels();
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00093F14 File Offset: 0x00092114
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (!affectedAgent.IsHuman)
			{
				return;
			}
			if (affectedAgent.MissionPeer.Team.IsDefender)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = null;
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					if (this._activeDuels[i].IsPeerInThisDuel(affectedAgent.MissionPeer))
					{
						duelInfo = this._activeDuels[i];
					}
				}
				if (duelInfo != null && !this._endingDuels.Contains(duelInfo))
				{
					duelInfo.OnDuelEnding();
					this._endingDuels.Add(duelInfo);
					return;
				}
			}
			else
			{
				for (int j = this._duelRequests.Count - 1; j >= 0; j--)
				{
					if (this._duelRequests[j].IsPeerInThisDuel(affectedAgent.MissionPeer))
					{
						this._duelRequests.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x00093FDB File Offset: 0x000921DB
		private Team ActivateAndGetDuelTeam()
		{
			if (this._deactiveDuelTeams.Count <= 0)
			{
				return base.Mission.Teams.Add(BattleSideEnum.Defender, uint.MaxValue, uint.MaxValue, null, true, false, false);
			}
			return this._deactiveDuelTeams.Dequeue();
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x0009400E File Offset: 0x0009220E
		private void DeactivateDuelTeam(Team team)
		{
			this._deactiveDuelTeams.Enqueue(team);
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x0009401C File Offset: 0x0009221C
		private bool IsHavingDuel(MissionPeer peer)
		{
			return this._activeDuels.AnyQ<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo d) => d.IsPeerInThisDuel(peer));
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x00094050 File Offset: 0x00092250
		private bool IsThereARequestBetweenPeers(MissionPeer requesterAgent, MissionPeer requesteeAgent)
		{
			for (int i = 0; i < this._duelRequests.Count; i++)
			{
				if (this._duelRequests[i].IsPeerInThisDuel(requesterAgent) && this._duelRequests[i].IsPeerInThisDuel(requesteeAgent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000940A0 File Offset: 0x000922A0
		private void CheckDuelsToStart()
		{
			for (int i = this._activeDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._activeDuels[i];
				if (!duelInfo.Started && duelInfo.Timer.IsPast && duelInfo.IsDuelStillValid(false))
				{
					this.StartDuel(duelInfo);
				}
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000940FC File Offset: 0x000922FC
		private void CheckDuelRequestTimeouts()
		{
			for (int i = this._duelRequests.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._duelRequests[i];
				if (duelInfo.Timer.IsPast)
				{
					this._duelRequests.Remove(duelInfo);
				}
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x0009414C File Offset: 0x0009234C
		private void CheckForRestartingDuels()
		{
			for (int i = this._restartingDuels.Count - 1; i >= 0; i--)
			{
				if (!this._restartingDuels[i].IsDuelStillValid(true))
				{
					Debug.Print("!_restartingDuels[i].IsDuelStillValid(true)", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				this._duelRequests.Add(this._restartingDuels[i]);
				this.PrepareDuel(this._restartingDuels[i]);
				this._restartingDuels.RemoveAt(i);
			}
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000941D0 File Offset: 0x000923D0
		private void CheckEndedDuels()
		{
			for (int i = this._endingDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._endingDuels[i];
				if (duelInfo.Timer.IsPast)
				{
					this.EndDuel(duelInfo);
					this._endingDuels.RemoveAt(i);
					if (!duelInfo.ChallengeEnded)
					{
						this._restartPreparationDuels.Add(duelInfo);
					}
				}
			}
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x0009423C File Offset: 0x0009243C
		private void CheckRestartPreparationDuels()
		{
			for (int i = this._restartPreparationDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._restartPreparationDuels[i];
				Agent controlledAgent = duelInfo.RequesterPeer.ControlledAgent;
				Agent controlledAgent2 = duelInfo.RequesteePeer.ControlledAgent;
				if ((controlledAgent == null || controlledAgent.IsActive()) && (controlledAgent2 == null || controlledAgent2.IsActive()))
				{
					this._restartPreparationDuels.RemoveAt(i);
					this._restartingDuels.Add(duelInfo);
				}
			}
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000942B4 File Offset: 0x000924B4
		private void PrepareDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			this._duelRequests.Remove(duel);
			if (!this.IsHavingDuel(duel.RequesteePeer) && !this.IsHavingDuel(duel.RequesterPeer))
			{
				this._activeDuels.Add(duel);
				Team team = (duel.Started ? duel.DuelingTeam : this.ActivateAndGetDuelTeam());
				duel.OnDuelPreparation(team);
				for (int i = 0; i < this._duelRequests.Count; i++)
				{
					if (this._duelRequests[i].DuelAreaIndex == duel.DuelAreaIndex)
					{
						this._duelRequests[i].UpdateDuelAreaIndex(this.GetNextAvailableDuelAreaIndex(this._duelRequests[i].RequesterPeer.ControlledAgent));
					}
				}
				return;
			}
			Debug.FailedAssert("IsHavingDuel(duel.RequesteePeer) || IsHavingDuel(duel.RequesterPeer)", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ServerGameModeLogics\\MissionMultiplayerDuel.cs", "PrepareDuel", 730);
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x00094390 File Offset: 0x00092590
		private void StartDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			duel.OnDuelStarted();
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x00094398 File Offset: 0x00092598
		private void EndDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			this._activeDuels.Remove(duel);
			duel.OnDuelEnded();
			this.CleanSpawnedEntitiesInDuelArea(duel.DuelAreaIndex);
			if (duel.ChallengeEnded)
			{
				TroopType troopType = TroopType.Invalid;
				MissionPeer challengeWinnerPeer = duel.ChallengeWinnerPeer;
				if (((challengeWinnerPeer != null) ? challengeWinnerPeer.ControlledAgent : null) != null)
				{
					troopType = this.GetAgentTroopType(challengeWinnerPeer.ControlledAgent);
				}
				MissionMultiplayerDuel.OnDuelEndedDelegate onDuelEnded = this.OnDuelEnded;
				if (onDuelEnded != null)
				{
					onDuelEnded(challengeWinnerPeer, troopType);
				}
				this.DeactivateDuelTeam(duel.DuelingTeam);
				this.HandleEndedChallenge(duel);
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x00094418 File Offset: 0x00092618
		private TroopType GetAgentTroopType(Agent requesterAgent)
		{
			TroopType troopType = TroopType.Invalid;
			switch (requesterAgent.Character.DefaultFormationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.HeavyInfantry:
				troopType = TroopType.Infantry;
				break;
			case FormationClass.Ranged:
				troopType = TroopType.Ranged;
				break;
			case FormationClass.Cavalry:
			case FormationClass.HorseArcher:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				troopType = TroopType.Cavalry;
				break;
			}
			return troopType;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00094468 File Offset: 0x00092668
		private void CleanSpawnedEntitiesInDuelArea(int duelAreaIndex)
		{
			int num = duelAreaIndex + 1;
			int num2 = 0;
			for (int i = 0; i < this._areaBoxes.Count; i++)
			{
				if (this._areaBoxes[i].GameEntity.HasTag(string.Format("{0}_{1}", "area_box", num)))
				{
					this._cachedSelectedVolumeBoxes[num2] = this._areaBoxes[i];
					num2++;
				}
			}
			for (int j = 0; j < Mission.Current.ActiveMissionObjects.Count; j++)
			{
				SpawnedItemEntity spawnedItemEntity;
				if ((spawnedItemEntity = Mission.Current.ActiveMissionObjects[j] as SpawnedItemEntity) != null && !spawnedItemEntity.IsDeactivated)
				{
					for (int k = 0; k < num2; k++)
					{
						if (this._cachedSelectedVolumeBoxes[k].IsPointIn(spawnedItemEntity.GameEntity.GlobalPosition))
						{
							spawnedItemEntity.RequestDeletionOnNextTick();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00094554 File Offset: 0x00092754
		private void HandleEndedChallenge(MissionMultiplayerDuel.DuelInfo duel)
		{
			MissionPeer challengeWinnerPeer = duel.ChallengeWinnerPeer;
			MissionPeer challengeLoserPeer = duel.ChallengeLoserPeer;
			if (challengeWinnerPeer != null)
			{
				DuelMissionRepresentative component = challengeWinnerPeer.GetComponent<DuelMissionRepresentative>();
				DuelMissionRepresentative component2 = challengeLoserPeer.GetComponent<DuelMissionRepresentative>();
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(challengeWinnerPeer, true);
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer2 = MultiplayerClassDivisions.GetMPHeroClassForPeer(challengeLoserPeer, true);
				float num = (float)MathF.Max(100, component2.Bounty) * MathF.Max(1f, (float)mpheroClassForPeer.TroopCasualCost / (float)mpheroClassForPeer2.TroopCasualCost) * MathF.Pow(2.7182817f, (float)component.NumberOfWins / 10f);
				component.OnDuelWon(num);
				if (challengeWinnerPeer.Peer.Communicator.IsConnectionActive)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				component2.ResetBountyAndNumberOfWins();
				if (challengeLoserPeer.Peer.Communicator.IsConnectionActive)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
			PeerComponent peerComponent = challengeWinnerPeer ?? duel.RequesterPeer;
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new DuelEnded(peerComponent.GetNetworkPeer()));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x00094660 File Offset: 0x00092860
		public int GetDuelAreaIndexIfDuelTeam(Team team)
		{
			if (team.IsDefender)
			{
				return this._activeDuels.FirstOrDefaultQ<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelingTeam == team).DuelAreaIndex;
			}
			return -1;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000946A8 File Offset: 0x000928A8
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman && agent.Team != null && agent.Team.IsDefender)
			{
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					if (this._activeDuels[i].IsPeerInThisDuel(agent.MissionPeer))
					{
						this._activeDuels[i].OnAgentBuild(agent);
						return;
					}
				}
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x00094714 File Offset: 0x00092914
		protected override void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					DuelMissionRepresentative component = networkCommunicator.GetComponent<DuelMissionRepresentative>();
					if (component != null)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component));
						GameNetwork.EndModuleEventAsServer();
					}
					if (networkPeer != networkCommunicator)
					{
						MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
						if (component2 != null)
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(networkCommunicator, component2.Perks[component2.SelectedTroopIndex]));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new DuelPreparationStartedForTheFirstTime(this._activeDuels[i].RequesterPeer.GetNetworkPeer(), this._activeDuels[i].RequesteePeer.GetNetworkPeer(), this._activeDuels[i].DuelAreaIndex));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00094830 File Offset: 0x00092A30
		protected override void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == component)
				{
					this._peersAndSelections.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00094880 File Offset: 0x00092A80
		protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Team = null;
			}
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000948A0 File Offset: 0x00092AA0
		private void OnPeerSelectedPreferredTroopType(MissionPeer missionPeer, TroopType troopType)
		{
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == missionPeer)
				{
					this._peersAndSelections[i] = new KeyValuePair<MissionPeer, TroopType>(missionPeer, troopType);
					return;
				}
			}
		}

		// Token: 0x04000F12 RID: 3858
		public const float DuelRequestTimeOutInSeconds = 10f;

		// Token: 0x04000F13 RID: 3859
		private const int MinBountyGain = 100;

		// Token: 0x04000F14 RID: 3860
		private const string AreaBoxTagPrefix = "area_box";

		// Token: 0x04000F15 RID: 3861
		private const string AreaFlagTagPrefix = "area_flag";

		// Token: 0x04000F16 RID: 3862
		public const int NumberOfDuelAreas = 16;

		// Token: 0x04000F17 RID: 3863
		public const float DuelEndInSeconds = 2f;

		// Token: 0x04000F18 RID: 3864
		private const float DuelRequestTimeOutServerToleranceInSeconds = 0.5f;

		// Token: 0x04000F19 RID: 3865
		private const float CorpseFadeOutTimeInSeconds = 1f;

		// Token: 0x04000F1B RID: 3867
		private List<GameEntity> _duelAreaFlags = new List<GameEntity>();

		// Token: 0x04000F1C RID: 3868
		private List<VolumeBox> _areaBoxes = new List<VolumeBox>();

		// Token: 0x04000F1D RID: 3869
		private List<MissionMultiplayerDuel.DuelInfo> _duelRequests = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000F1E RID: 3870
		private List<MissionMultiplayerDuel.DuelInfo> _activeDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000F1F RID: 3871
		private List<MissionMultiplayerDuel.DuelInfo> _endingDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000F20 RID: 3872
		private List<MissionMultiplayerDuel.DuelInfo> _restartingDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000F21 RID: 3873
		private List<MissionMultiplayerDuel.DuelInfo> _restartPreparationDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000F22 RID: 3874
		private readonly Queue<Team> _deactiveDuelTeams = new Queue<Team>();

		// Token: 0x04000F23 RID: 3875
		private List<KeyValuePair<MissionPeer, TroopType>> _peersAndSelections = new List<KeyValuePair<MissionPeer, TroopType>>();

		// Token: 0x04000F24 RID: 3876
		private VolumeBox[] _cachedSelectedVolumeBoxes;

		// Token: 0x04000F25 RID: 3877
		private KeyValuePair<int, TroopType>[] _cachedSelectedAreaFlags;

		// Token: 0x02000598 RID: 1432
		private class DuelInfo
		{
			// Token: 0x17000A91 RID: 2705
			// (get) Token: 0x06003E5F RID: 15967 RVA: 0x000F7C9C File Offset: 0x000F5E9C
			public MissionPeer RequesterPeer
			{
				get
				{
					return this._challengers[0].MissionPeer;
				}
			}

			// Token: 0x17000A92 RID: 2706
			// (get) Token: 0x06003E60 RID: 15968 RVA: 0x000F7CAF File Offset: 0x000F5EAF
			public MissionPeer RequesteePeer
			{
				get
				{
					return this._challengers[1].MissionPeer;
				}
			}

			// Token: 0x17000A93 RID: 2707
			// (get) Token: 0x06003E61 RID: 15969 RVA: 0x000F7CC2 File Offset: 0x000F5EC2
			// (set) Token: 0x06003E62 RID: 15970 RVA: 0x000F7CCA File Offset: 0x000F5ECA
			public int DuelAreaIndex { get; private set; }

			// Token: 0x17000A94 RID: 2708
			// (get) Token: 0x06003E63 RID: 15971 RVA: 0x000F7CD3 File Offset: 0x000F5ED3
			// (set) Token: 0x06003E64 RID: 15972 RVA: 0x000F7CDB File Offset: 0x000F5EDB
			public TroopType DuelAreaTroopType { get; private set; }

			// Token: 0x17000A95 RID: 2709
			// (get) Token: 0x06003E65 RID: 15973 RVA: 0x000F7CE4 File Offset: 0x000F5EE4
			// (set) Token: 0x06003E66 RID: 15974 RVA: 0x000F7CEC File Offset: 0x000F5EEC
			public MissionTime Timer { get; private set; }

			// Token: 0x17000A96 RID: 2710
			// (get) Token: 0x06003E67 RID: 15975 RVA: 0x000F7CF5 File Offset: 0x000F5EF5
			// (set) Token: 0x06003E68 RID: 15976 RVA: 0x000F7CFD File Offset: 0x000F5EFD
			public Team DuelingTeam { get; private set; }

			// Token: 0x17000A97 RID: 2711
			// (get) Token: 0x06003E69 RID: 15977 RVA: 0x000F7D06 File Offset: 0x000F5F06
			// (set) Token: 0x06003E6A RID: 15978 RVA: 0x000F7D0E File Offset: 0x000F5F0E
			public bool Started { get; private set; }

			// Token: 0x17000A98 RID: 2712
			// (get) Token: 0x06003E6B RID: 15979 RVA: 0x000F7D17 File Offset: 0x000F5F17
			// (set) Token: 0x06003E6C RID: 15980 RVA: 0x000F7D1F File Offset: 0x000F5F1F
			public bool ChallengeEnded { get; private set; }

			// Token: 0x17000A99 RID: 2713
			// (get) Token: 0x06003E6D RID: 15981 RVA: 0x000F7D28 File Offset: 0x000F5F28
			public MissionPeer ChallengeWinnerPeer
			{
				get
				{
					if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
					{
						return this._challengers[(int)this._winnerChallengerType].MissionPeer;
					}
					return null;
				}
			}

			// Token: 0x17000A9A RID: 2714
			// (get) Token: 0x06003E6E RID: 15982 RVA: 0x000F7D4B File Offset: 0x000F5F4B
			public MissionPeer ChallengeLoserPeer
			{
				get
				{
					if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
					{
						return this._challengers[(this._winnerChallengerType == MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester) ? 1 : 0].MissionPeer;
					}
					return null;
				}
			}

			// Token: 0x06003E6F RID: 15983 RVA: 0x000F7D74 File Offset: 0x000F5F74
			public DuelInfo(MissionPeer requesterPeer, MissionPeer requesteePeer, KeyValuePair<int, TroopType> duelAreaPair)
			{
				this.DuelAreaIndex = duelAreaPair.Key;
				this.DuelAreaTroopType = duelAreaPair.Value;
				this._challengers = new MissionMultiplayerDuel.DuelInfo.Challenger[2];
				this._challengers[0] = new MissionMultiplayerDuel.DuelInfo.Challenger(requesterPeer);
				this._challengers[1] = new MissionMultiplayerDuel.DuelInfo.Challenger(requesteePeer);
				this.Timer = MissionTime.Now + MissionTime.Seconds(10.5f);
			}

			// Token: 0x06003E70 RID: 15984 RVA: 0x000F7DF4 File Offset: 0x000F5FF4
			private void DecideRoundWinner()
			{
				bool isConnectionActive = this._challengers[0].MissionPeer.Peer.Communicator.IsConnectionActive;
				bool isConnectionActive2 = this._challengers[1].MissionPeer.Peer.Communicator.IsConnectionActive;
				if (!this.Started)
				{
					if (isConnectionActive == isConnectionActive2)
					{
						this.ChallengeEnded = true;
					}
					else
					{
						this._winnerChallengerType = (isConnectionActive ? MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester : MissionMultiplayerDuel.DuelInfo.ChallengerType.Requestee);
					}
				}
				else
				{
					Agent duelingAgent = this._challengers[0].DuelingAgent;
					Agent duelingAgent2 = this._challengers[1].DuelingAgent;
					if (duelingAgent.IsActive())
					{
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester;
					}
					else if (duelingAgent2.IsActive())
					{
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.Requestee;
					}
					else
					{
						if (!isConnectionActive && !isConnectionActive2)
						{
							this.ChallengeEnded = true;
						}
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;
					}
				}
				if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
				{
					this._challengers[(int)this._winnerChallengerType].IncreaseWinCount();
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelRoundEnded(this._challengers[(int)this._winnerChallengerType].NetworkPeer));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					if (this._challengers[(int)this._winnerChallengerType].KillCountInDuel == MultiplayerOptions.OptionType.MinScoreToWinDuel.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) || !isConnectionActive || !isConnectionActive2)
					{
						this.ChallengeEnded = true;
					}
				}
			}

			// Token: 0x06003E71 RID: 15985 RVA: 0x000F7F38 File Offset: 0x000F6138
			public void OnDuelPreparation(Team duelTeam)
			{
				if (!this.Started)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPreparationStartedForTheFirstTime(this._challengers[0].MissionPeer.GetNetworkPeer(), this._challengers[1].MissionPeer.GetNetworkPeer(), this.DuelAreaIndex));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.Started = false;
				this.DuelingTeam = duelTeam;
				this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;
				for (int i = 0; i < 2; i++)
				{
					this._challengers[i].OnDuelPreparation(this.DuelingTeam);
					this._challengers[i].MissionPeer.GetComponent<DuelMissionRepresentative>().OnDuelPreparation(this._challengers[0].MissionPeer, this._challengers[1].MissionPeer);
				}
				this.Timer = MissionTime.Now + MissionTime.Seconds(3f);
			}

			// Token: 0x06003E72 RID: 15986 RVA: 0x000F8024 File Offset: 0x000F6224
			public void OnDuelStarted()
			{
				this.Started = true;
				this.DuelingTeam.SetIsEnemyOf(this.DuelingTeam, true);
			}

			// Token: 0x06003E73 RID: 15987 RVA: 0x000F803F File Offset: 0x000F623F
			public void OnDuelEnding()
			{
				this.Timer = MissionTime.Now + MissionTime.Seconds(2f);
			}

			// Token: 0x06003E74 RID: 15988 RVA: 0x000F805C File Offset: 0x000F625C
			public void OnDuelEnded()
			{
				if (this.Started)
				{
					this.DuelingTeam.SetIsEnemyOf(this.DuelingTeam, false);
				}
				this.DecideRoundWinner();
				for (int i = 0; i < 2; i++)
				{
					this._challengers[i].OnDuelEnded();
					Agent agent = this._challengers[i].DuelingAgent ?? this._challengers[i].MissionPeer.ControlledAgent;
					if (this.ChallengeEnded && agent != null && agent.IsActive())
					{
						agent.FadeOut(true, false);
					}
					this._challengers[i].MissionPeer.HasSpawnedAgentVisuals = true;
				}
				for (int j = 0; j < 2; j++)
				{
					if (this._challengers[j].MountAgent != null && this._challengers[j].MountAgent.IsActive() && (this.ChallengeEnded || this._challengers[j].MountAgent.RiderAgent == null))
					{
						this._challengers[j].MountAgent.FadeOut(true, false);
					}
				}
			}

			// Token: 0x06003E75 RID: 15989 RVA: 0x000F8174 File Offset: 0x000F6374
			public void OnAgentBuild(Agent agent)
			{
				for (int i = 0; i < 2; i++)
				{
					if (this._challengers[i].MissionPeer == agent.MissionPeer)
					{
						this._challengers[i].SetAgents(agent);
						return;
					}
				}
			}

			// Token: 0x06003E76 RID: 15990 RVA: 0x000F81BC File Offset: 0x000F63BC
			public bool IsDuelStillValid(bool doNotCheckAgent = false)
			{
				for (int i = 0; i < 2; i++)
				{
					if (!this._challengers[i].MissionPeer.Peer.Communicator.IsConnectionActive || (!doNotCheckAgent && !this._challengers[i].MissionPeer.IsControlledAgentActive))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06003E77 RID: 15991 RVA: 0x000F8218 File Offset: 0x000F6418
			public bool IsPeerInThisDuel(MissionPeer peer)
			{
				for (int i = 0; i < 2; i++)
				{
					if (this._challengers[i].MissionPeer == peer)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06003E78 RID: 15992 RVA: 0x000F8248 File Offset: 0x000F6448
			public void UpdateDuelAreaIndex(KeyValuePair<int, TroopType> duelAreaPair)
			{
				this.DuelAreaIndex = duelAreaPair.Key;
				this.DuelAreaTroopType = duelAreaPair.Value;
			}

			// Token: 0x04001EF8 RID: 7928
			private const float DuelStartCountdown = 3f;

			// Token: 0x04001EF9 RID: 7929
			private readonly MissionMultiplayerDuel.DuelInfo.Challenger[] _challengers;

			// Token: 0x04001EFA RID: 7930
			private MissionMultiplayerDuel.DuelInfo.ChallengerType _winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;

			// Token: 0x020006CC RID: 1740
			private enum ChallengerType
			{
				// Token: 0x040023C8 RID: 9160
				None = -1,
				// Token: 0x040023C9 RID: 9161
				Requester,
				// Token: 0x040023CA RID: 9162
				Requestee,
				// Token: 0x040023CB RID: 9163
				NumChallengerType
			}

			// Token: 0x020006CD RID: 1741
			private struct Challenger
			{
				// Token: 0x17000B2C RID: 2860
				// (get) Token: 0x06004305 RID: 17157 RVA: 0x00101385 File Offset: 0x000FF585
				// (set) Token: 0x06004306 RID: 17158 RVA: 0x0010138D File Offset: 0x000FF58D
				public Agent DuelingAgent { get; private set; }

				// Token: 0x17000B2D RID: 2861
				// (get) Token: 0x06004307 RID: 17159 RVA: 0x00101396 File Offset: 0x000FF596
				// (set) Token: 0x06004308 RID: 17160 RVA: 0x0010139E File Offset: 0x000FF59E
				public Agent MountAgent { get; private set; }

				// Token: 0x17000B2E RID: 2862
				// (get) Token: 0x06004309 RID: 17161 RVA: 0x001013A7 File Offset: 0x000FF5A7
				// (set) Token: 0x0600430A RID: 17162 RVA: 0x001013AF File Offset: 0x000FF5AF
				public int KillCountInDuel { get; private set; }

				// Token: 0x0600430B RID: 17163 RVA: 0x001013B8 File Offset: 0x000FF5B8
				public Challenger(MissionPeer missionPeer)
				{
					this.MissionPeer = missionPeer;
					MissionPeer missionPeer2 = this.MissionPeer;
					this.NetworkPeer = ((missionPeer2 != null) ? missionPeer2.GetNetworkPeer() : null);
					this.DuelingAgent = null;
					this.MountAgent = null;
					this.KillCountInDuel = 0;
				}

				// Token: 0x0600430C RID: 17164 RVA: 0x001013EE File Offset: 0x000FF5EE
				public void OnDuelPreparation(Team duelingTeam)
				{
					Agent controlledAgent = this.MissionPeer.ControlledAgent;
					if (controlledAgent != null)
					{
						controlledAgent.FadeOut(true, true);
					}
					this.MissionPeer.Team = duelingTeam;
					this.MissionPeer.HasSpawnedAgentVisuals = true;
				}

				// Token: 0x0600430D RID: 17165 RVA: 0x00101420 File Offset: 0x000FF620
				public void OnDuelEnded()
				{
					if (this.MissionPeer.Peer.Communicator.IsConnectionActive)
					{
						this.MissionPeer.Team = Mission.Current.AttackerTeam;
					}
				}

				// Token: 0x0600430E RID: 17166 RVA: 0x00101450 File Offset: 0x000FF650
				public void IncreaseWinCount()
				{
					int killCountInDuel = this.KillCountInDuel;
					this.KillCountInDuel = killCountInDuel + 1;
				}

				// Token: 0x0600430F RID: 17167 RVA: 0x0010146D File Offset: 0x000FF66D
				public void SetAgents(Agent agent)
				{
					this.DuelingAgent = agent;
					this.MountAgent = this.DuelingAgent.MountAgent;
				}

				// Token: 0x040023CC RID: 9164
				public readonly MissionPeer MissionPeer;

				// Token: 0x040023CD RID: 9165
				public readonly NetworkCommunicator NetworkPeer;
			}
		}

		// Token: 0x02000599 RID: 1433
		// (Invoke) Token: 0x06003E7A RID: 15994
		public delegate void OnDuelEndedDelegate(MissionPeer winnerPeer, TroopType troopType);
	}
}
