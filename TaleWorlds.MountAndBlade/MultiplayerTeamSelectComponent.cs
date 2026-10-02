using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C8 RID: 712
	public class MultiplayerTeamSelectComponent : MissionNetwork
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600290A RID: 10506 RVA: 0x0009BC9C File Offset: 0x00099E9C
		// (remove) Token: 0x0600290B RID: 10507 RVA: 0x0009BCD4 File Offset: 0x00099ED4
		public event MultiplayerTeamSelectComponent.OnSelectingTeamDelegate OnSelectingTeam;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600290C RID: 10508 RVA: 0x0009BD0C File Offset: 0x00099F0C
		// (remove) Token: 0x0600290D RID: 10509 RVA: 0x0009BD44 File Offset: 0x00099F44
		public event Action OnMyTeamChange;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x0600290E RID: 10510 RVA: 0x0009BD7C File Offset: 0x00099F7C
		// (remove) Token: 0x0600290F RID: 10511 RVA: 0x0009BDB4 File Offset: 0x00099FB4
		public event Action OnUpdateTeams;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002910 RID: 10512 RVA: 0x0009BDEC File Offset: 0x00099FEC
		// (remove) Token: 0x06002911 RID: 10513 RVA: 0x0009BE24 File Offset: 0x0009A024
		public event Action OnUpdateFriendsPerTeam;

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002912 RID: 10514 RVA: 0x0009BE59 File Offset: 0x0009A059
		// (set) Token: 0x06002913 RID: 10515 RVA: 0x0009BE61 File Offset: 0x0009A061
		public bool TeamSelectionEnabled { get; private set; }

		// Token: 0x06002915 RID: 10517 RVA: 0x0009BE72 File Offset: 0x0009A072
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this._gameModeServer = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			if (BannerlordNetwork.LobbyMissionType == LobbyMissionType.Matchmaker)
			{
				this.TeamSelectionEnabled = false;
				return;
			}
			this.TeamSelectionEnabled = true;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0009BEB2 File Offset: 0x0009A0B2
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<TeamChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventTeamChange));
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0009BED0 File Offset: 0x0009A0D0
		private void OnMyClientSynchronized()
		{
			base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized -= this.OnMyClientSynchronized;
			if (Mission.Current.GetMissionBehavior<MissionLobbyComponent>().CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && GameNetwork.MyPeer.GetComponent<MissionPeer>().Team == null && !GameNetwork.MyPeer.IsSpectator)
			{
				this.SelectTeam();
			}
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0009BF30 File Offset: 0x0009A130
		private void SendClanInfoOnClientSynchronized()
		{
			MissionNetworkComponent missionBehavior = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			if (missionBehavior != null)
			{
				missionBehavior.OnMyClientSynchronized -= this.SendClanInfoOnClientSynchronized;
			}
			this.SendClanInfoToServer();
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x0009BF64 File Offset: 0x0009A164
		private void SendClanInfoToServer()
		{
			if (!GameNetwork.IsClient)
			{
				return;
			}
			LobbyClient gameClient = NetworkMain.GameClient;
			string text;
			if (gameClient == null)
			{
				text = null;
			}
			else
			{
				ClanInfo clanInfo = gameClient.ClanInfo;
				text = ((clanInfo != null) ? clanInfo.Name : null);
			}
			string text2 = text ?? string.Empty;
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new SendClanInfo(text2));
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x0009BFB4 File Offset: 0x0009A1B4
		public override void AfterStart()
		{
			this._platformFriends = new HashSet<PlayerId>();
			foreach (PlayerId playerId in FriendListService.GetAllFriendsInAllPlatforms())
			{
				this._platformFriends.Add(playerId);
			}
			this._friendsPerTeam = new Dictionary<Team, IEnumerable<VirtualPlayer>>();
			MissionPeer.OnTeamChanged += this.UpdateTeams;
			if (GameNetwork.IsClient)
			{
				MissionNetworkComponent missionBehavior = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
				missionBehavior.OnMyClientSynchronized += this.SendClanInfoOnClientSynchronized;
				if (this.TeamSelectionEnabled)
				{
					missionBehavior.OnMyClientSynchronized += this.OnMyClientSynchronized;
				}
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0009C06C File Offset: 0x0009A26C
		public override void OnRemoveBehavior()
		{
			MissionPeer.OnTeamChanged -= this.UpdateTeams;
			this.OnMyTeamChange = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x0009C08C File Offset: 0x0009A28C
		protected override void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			base.HandleLateNewClientAfterSynchronized(networkPeer);
			if (GameNetwork.IsServer && networkPeer.IsSpectator)
			{
				if (base.Mission.SpectatorTeam == null)
				{
					Debug.FailedAssert("Spectator peer connected but Mission.SpectatorTeam is null; the peer will stay teamless.", "MultiplayerTeamSelectComponent.cs", "HandleLateNewClientAfterSynchronized", 147);
					return;
				}
				MissionPeer component = networkPeer.GetComponent<MissionPeer>();
				if (component == null)
				{
					Debug.FailedAssert("Spectator peer connected without a MissionPeer component; the peer will stay teamless.", "MultiplayerTeamSelectComponent.cs", "HandleLateNewClientAfterSynchronized", 155);
					return;
				}
				if (component.Team != base.Mission.SpectatorTeam)
				{
					this.ChangeTeamServer(networkPeer, base.Mission.SpectatorTeam);
				}
			}
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x0009C120 File Offset: 0x0009A320
		private bool HandleClientEventTeamChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			TeamChange teamChange = (TeamChange)baseMessage;
			if (peer.IsSpectator)
			{
				return true;
			}
			if (this.TeamSelectionEnabled)
			{
				if (teamChange.AutoAssign)
				{
					this.AutoAssignTeam(peer);
				}
				else
				{
					Team teamFromTeamIndex = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(teamChange.TeamIndex);
					this.ChangeTeamServer(peer, teamFromTeamIndex);
				}
			}
			return true;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x0009C16C File Offset: 0x0009A36C
		public void SelectTeam()
		{
			if (this.OnSelectingTeam != null)
			{
				List<Team> disabledTeams = this.GetDisabledTeams();
				this.OnSelectingTeam(disabledTeams);
			}
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x0009C194 File Offset: 0x0009A394
		public void UpdateTeams(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
			if (this.OnUpdateTeams != null)
			{
				this.OnUpdateTeams();
			}
			if (GameNetwork.IsMyPeerReady)
			{
				this.CacheFriendsForTeams();
			}
			if (newTeam.Side != BattleSideEnum.None)
			{
				MissionPeer component = peer.GetComponent<MissionPeer>();
				component.SelectedTroopIndex = 0;
				component.NextSelectedTroopIndex = 0;
				component.OverrideCultureWithTeamCulture();
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x0009C1E4 File Offset: 0x0009A3E4
		public List<Team> GetDisabledTeams()
		{
			List<Team> list = new List<Team>();
			if (MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) == 0)
			{
				return list;
			}
			Team myTeam = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
			Team[] array = base.Mission.Teams.Where<Team>((Team q) => q != this.Mission.SpectatorTeam).OrderBy<Team, int>(delegate(Team q)
			{
				if (myTeam == null)
				{
					return this.GetPlayerCountForTeam(q);
				}
				if (q != myTeam)
				{
					return this.GetPlayerCountForTeam(q);
				}
				return this.GetPlayerCountForTeam(q) - 1;
			}).ToArray<Team>();
			foreach (Team team in array)
			{
				int num = this.GetPlayerCountForTeam(team);
				int num2 = this.GetPlayerCountForTeam(array[0]);
				if (myTeam == team)
				{
					num--;
				}
				if (myTeam == array[0])
				{
					num2--;
				}
				if (num - num2 >= MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					list.Add(team);
				}
			}
			return list;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x0009C2CC File Offset: 0x0009A4CC
		public void ChangeTeamServer(NetworkCommunicator networkPeer, Team team)
		{
			if (GameNetwork.IsServer && networkPeer.IsSpectator && team != base.Mission.SpectatorTeam)
			{
				return;
			}
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			Team team2 = component.Team;
			if (team2 != null && team2 != base.Mission.SpectatorTeam && team2 != team && component.ControlledAgent != null)
			{
				Blow blow = new Blow(component.ControlledAgent.Index);
				blow.DamageType = DamageTypes.Invalid;
				blow.BaseMagnitude = 10000f;
				blow.GlobalPosition = component.ControlledAgent.Position;
				blow.DamagedPercentage = 1f;
				component.ControlledAgent.Die(blow, Agent.KillInfo.TeamSwitch);
			}
			component.Team = team;
			if (team != base.Mission.SpectatorTeam && team.Side != BattleSideEnum.None)
			{
				BasicCultureObject basicCultureObject = (component.Team.IsAttacker ? MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)) : MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)));
				component.Culture = basicCultureObject;
			}
			if (team != team2)
			{
				if (component.HasSpawnedAgentVisuals)
				{
					component.HasSpawnedAgentVisuals = false;
					MBDebug.Print("HasSpawnedAgentVisuals = false for peer: " + component.Name + " because he just changed his team", 0, Debug.DebugColor.White, 17592186044416UL);
					component.SpawnCountThisRound = 0;
					Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().RemoveAgentVisuals(component, true);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(component.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					component.HasSpawnedAgentVisuals = false;
				}
				if (!this._gameModeServer.IsGameModeHidingAllAgentVisuals && !networkPeer.IsServerPeer)
				{
					MissionNetworkComponent missionNetworkComponent = this._missionNetworkComponent;
					if (missionNetworkComponent != null)
					{
						missionNetworkComponent.OnPeerSelectedTeam(component);
					}
				}
				this._gameModeServer.OnPeerChangedTeam(networkPeer, team2, team);
				component.SpawnTimer.Reset(Mission.Current.CurrentTime, 0.1f);
				component.WantsToSpawnAsBot = false;
				component.HasSpawnTimerExpired = false;
			}
			this.UpdateTeams(networkPeer, team2, team);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x0009C4B4 File Offset: 0x0009A6B4
		public void ChangeTeam(Team team)
		{
			if (team != GameNetwork.MyPeer.GetComponent<MissionPeer>().Team)
			{
				if (GameNetwork.IsServer)
				{
					Mission.Current.PlayerTeam = team;
					this.ChangeTeamServer(GameNetwork.MyPeer, team);
				}
				else
				{
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						if (component != null)
						{
							component.ClearAllVisuals(false);
						}
					}
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new TeamChange(false, team.TeamIndex));
					GameNetwork.EndModuleEventAsClient();
				}
				if (this.OnMyTeamChange != null)
				{
					this.OnMyTeamChange();
				}
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x0009C574 File Offset: 0x0009A774
		public int GetPlayerCountForTeam(Team team)
		{
			int num = 0;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team == team)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x0009C5E4 File Offset: 0x0009A7E4
		private void CacheFriendsForTeams()
		{
			this._friendsPerTeam.Clear();
			if (this._platformFriends.Count > 0)
			{
				List<MissionPeer> list = new List<MissionPeer>();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && this._platformFriends.Contains(networkCommunicator.VirtualPlayer.Id))
					{
						list.Add(component);
					}
				}
				using (List<Team>.Enumerator enumerator2 = base.Mission.Teams.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Team team = enumerator2.Current;
						if (team != null)
						{
							this._friendsPerTeam.Add(team, from x in list
								where x.Team == team
								select x.Peer);
						}
					}
				}
				if (this.OnUpdateFriendsPerTeam != null)
				{
					this.OnUpdateFriendsPerTeam();
				}
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x0009C730 File Offset: 0x0009A930
		public IEnumerable<VirtualPlayer> GetFriendsForTeam(Team team)
		{
			if (this._friendsPerTeam.ContainsKey(team))
			{
				return this._friendsPerTeam[team];
			}
			return new List<VirtualPlayer>();
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x0009C754 File Offset: 0x0009A954
		public void BalanceTeams()
		{
			if (MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) != 0)
			{
				int i = this.GetPlayerCountForTeam(Mission.Current.AttackerTeam);
				int j = this.GetPlayerCountForTeam(Mission.Current.DefenderTeam);
				while (i > j + MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					MissionPeer missionPeer = null;
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator.IsSynchronized)
						{
							MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
							if (((component != null) ? component.Team : null) != null && component.Team == base.Mission.AttackerTeam && (missionPeer == null || component.JoinTime >= missionPeer.JoinTime))
							{
								missionPeer = component;
							}
						}
					}
					this.ChangeTeamServer(missionPeer.GetNetworkPeer(), Mission.Current.DefenderTeam);
					i--;
					j++;
				}
				while (j > i + MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					MissionPeer missionPeer2 = null;
					foreach (NetworkCommunicator networkCommunicator2 in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator2.IsSynchronized)
						{
							MissionPeer component2 = networkCommunicator2.GetComponent<MissionPeer>();
							if (((component2 != null) ? component2.Team : null) != null && component2.Team == base.Mission.DefenderTeam && (missionPeer2 == null || component2.JoinTime >= missionPeer2.JoinTime))
							{
								missionPeer2 = component2;
							}
						}
					}
					this.ChangeTeamServer(missionPeer2.GetNetworkPeer(), Mission.Current.AttackerTeam);
					i++;
					j--;
				}
			}
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x0009C91C File Offset: 0x0009AB1C
		public void AutoAssignTeam(NetworkCommunicator peer)
		{
			if (!GameNetwork.IsServer)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new TeamChange(true, -1));
				GameNetwork.EndModuleEventAsClient();
				if (this.OnMyTeamChange != null)
				{
					this.OnMyTeamChange();
				}
				return;
			}
			List<Team> disabledTeams = this.GetDisabledTeams();
			List<Team> list = base.Mission.Teams.Where<Team>((Team x) => !disabledTeams.Contains(x) && x.Side != BattleSideEnum.None).ToList<Team>();
			Team team;
			if (list.Count > 1)
			{
				int[] array = new int[list.Count];
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (((component != null) ? component.Team : null) != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							if (component.Team == list[i])
							{
								array[i]++;
							}
						}
					}
				}
				int num = -1;
				int num2 = -1;
				for (int j = 0; j < array.Length; j++)
				{
					if (num2 < 0 || array[j] < num)
					{
						num2 = j;
						num = array[j];
					}
				}
				team = list[num2];
			}
			else
			{
				team = list[0];
			}
			if (!peer.IsMine)
			{
				this.ChangeTeamServer(peer, team);
				return;
			}
			this.ChangeTeam(team);
		}

		// Token: 0x04000FC3 RID: 4035
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000FC4 RID: 4036
		private MissionMultiplayerGameModeBase _gameModeServer;

		// Token: 0x04000FC5 RID: 4037
		private HashSet<PlayerId> _platformFriends;

		// Token: 0x04000FC6 RID: 4038
		private Dictionary<Team, IEnumerable<VirtualPlayer>> _friendsPerTeam;

		// Token: 0x020005B3 RID: 1459
		// (Invoke) Token: 0x06003EDC RID: 16092
		public delegate void OnSelectingTeamDelegate(List<Team> disableTeams);
	}
}
