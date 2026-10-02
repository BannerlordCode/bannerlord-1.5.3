using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B4 RID: 692
	public class MissionScoreboardComponent : MissionNetwork
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060026E8 RID: 9960 RVA: 0x000900D0 File Offset: 0x0008E2D0
		// (remove) Token: 0x060026E9 RID: 9961 RVA: 0x00090108 File Offset: 0x0008E308
		public event Action OnRoundPropertiesChanged;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060026EA RID: 9962 RVA: 0x00090140 File Offset: 0x0008E340
		// (remove) Token: 0x060026EB RID: 9963 RVA: 0x00090178 File Offset: 0x0008E378
		public event Action<BattleSideEnum> OnBotPropertiesChanged;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060026EC RID: 9964 RVA: 0x000901B0 File Offset: 0x0008E3B0
		// (remove) Token: 0x060026ED RID: 9965 RVA: 0x000901E8 File Offset: 0x0008E3E8
		public event Action<Team, Team, MissionPeer> OnPlayerSideChanged;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060026EE RID: 9966 RVA: 0x00090220 File Offset: 0x0008E420
		// (remove) Token: 0x060026EF RID: 9967 RVA: 0x00090258 File Offset: 0x0008E458
		public event Action<BattleSideEnum, MissionPeer> OnPlayerPropertiesChanged;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060026F0 RID: 9968 RVA: 0x00090290 File Offset: 0x0008E490
		// (remove) Token: 0x060026F1 RID: 9969 RVA: 0x000902C8 File Offset: 0x0008E4C8
		public event Action<MissionPeer, int> OnMVPSelected;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060026F2 RID: 9970 RVA: 0x00090300 File Offset: 0x0008E500
		// (remove) Token: 0x060026F3 RID: 9971 RVA: 0x00090338 File Offset: 0x0008E538
		public event Action OnScoreboardInitialized;

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0009036D File Offset: 0x0008E56D
		public bool IsOneSided
		{
			get
			{
				return this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.OneSide;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x00090378 File Offset: 0x0008E578
		public BattleSideEnum RoundWinner
		{
			get
			{
				IRoundComponent roundComponent = this._mpGameModeBase.RoundComponent;
				if (roundComponent == null)
				{
					return BattleSideEnum.None;
				}
				return roundComponent.RoundWinner;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x00090390 File Offset: 0x0008E590
		public MissionScoreboardComponent.ScoreboardHeader[] Headers
		{
			get
			{
				return this._scoreboardData.GetScoreboardHeaders();
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x0009039D File Offset: 0x0008E59D
		public MissionScoreboardComponent(IScoreboardData scoreboardData)
		{
			this._scoreboardData = scoreboardData;
			this._spectators = new List<MissionPeer>();
			this._sides = new MissionScoreboardComponent.MissionScoreboardSide[2];
			this._roundWinnerList = new List<BattleSideEnum>();
			this._mvpCountPerPeer = new List<ValueTuple<MissionPeer, int>>();
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000903D9 File Offset: 0x0008E5D9
		public IEnumerable<BattleSideEnum> RoundWinnerList
		{
			get
			{
				return this._roundWinnerList.AsReadOnly();
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000903E6 File Offset: 0x0008E5E6
		public MissionScoreboardComponent.MissionScoreboardSide[] Sides
		{
			get
			{
				return this._sides;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x000903EE File Offset: 0x0008E5EE
		public List<MissionPeer> Spectators
		{
			get
			{
				return this._spectators;
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000903F8 File Offset: 0x0008E5F8
		public override void AfterStart()
		{
			this._spectators.Clear();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this._mpGameModeBase = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			if (this._missionLobbyComponent.MissionType == MultiplayerGameType.Duel)
			{
				this._scoreboardSides = MissionScoreboardComponent.ScoreboardSides.OneSide;
			}
			else
			{
				this._scoreboardSides = MissionScoreboardComponent.ScoreboardSides.TwoSides;
			}
			MissionPeer.OnTeamChanged += this.TeamChange;
			this._missionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			if (GameNetwork.IsServerOrRecorder && this._mpGameModeBase.RoundComponent != null)
			{
				this._mpGameModeBase.RoundComponent.OnRoundEnding += this.OnRoundEnding;
				this._mpGameModeBase.RoundComponent.OnPreRoundEnding += this.OnPreRoundEnding;
			}
			this.LateInitScoreboard();
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000904DB File Offset: 0x0008E6DB
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<UpdateRoundScores>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerUpdateRoundScoresMessage));
				registerer.RegisterBaseHandler<SetRoundMVP>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerSetRoundMVP));
				registerer.RegisterBaseHandler<BotData>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBotDataMessage));
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x0009051C File Offset: 0x0008E71C
		public override void OnRemoveBehavior()
		{
			this._spectators.Clear();
			for (int i = 0; i < 2; i++)
			{
				if (this._sides[i] != null)
				{
					this._sides[i].Clear();
				}
			}
			MissionPeer.OnTeamChanged -= this.TeamChange;
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			}
			if (GameNetwork.IsServerOrRecorder && this._mpGameModeBase.RoundComponent != null)
			{
				this._mpGameModeBase.RoundComponent.OnRoundEnding -= this.OnRoundEnding;
			}
			base.OnRemoveBehavior();
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000905C0 File Offset: 0x0008E7C0
		public void ResetBotScores()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (((missionScoreboardSide != null) ? missionScoreboardSide.BotScores : null) != null)
				{
					missionScoreboardSide.BotScores.ResetKillDeathAssist();
				}
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00090600 File Offset: 0x0008E800
		public void ChangeTeamScore(Team team, int scoreChange)
		{
			MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(team.Side);
			sideSafe.SideScore += scoreChange;
			sideSafe.SideScore = MBMath.ClampInt(sideSafe.SideScore, -1023000, 1023000);
			if (GameNetwork.IsServer)
			{
				int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x00090698 File Offset: 0x0008E898
		private void UpdateRoundScores()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null && missionScoreboardSide.Side == this.RoundWinner)
				{
					this._roundWinnerList.Add(this.RoundWinner);
					if (this.RoundWinner != BattleSideEnum.None)
					{
						this._sides[(int)this.RoundWinner].SideScore++;
					}
				}
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
			if (GameNetwork.IsServer)
			{
				int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x0009075A File Offset: 0x0008E95A
		public MissionScoreboardComponent.MissionScoreboardSide GetSideSafe(BattleSideEnum battleSide)
		{
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.OneSide)
			{
				return this._sides[1];
			}
			return this._sides[(int)battleSide];
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x00090775 File Offset: 0x0008E975
		public int GetRoundScore(BattleSideEnum side)
		{
			if (side > (BattleSideEnum)this._sides.Length || side < BattleSideEnum.Defender)
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetRoundScore", 462);
				return 0;
			}
			return this.GetSideSafe(side).SideScore;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000907B0 File Offset: 0x0008E9B0
		public void HandleServerUpdateRoundScoresMessage(GameNetworkMessage baseMessage)
		{
			UpdateRoundScores updateRoundScores = (UpdateRoundScores)baseMessage;
			this._sides[1].SideScore = updateRoundScores.AttackerTeamScore;
			if (this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide)
			{
				this._sides[0].SideScore = updateRoundScores.DefenderTeamScore;
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x00090808 File Offset: 0x0008EA08
		public void HandleServerSetRoundMVP(GameNetworkMessage baseMessage)
		{
			SetRoundMVP setRoundMVP = (SetRoundMVP)baseMessage;
			Action<MissionPeer, int> onMVPSelected = this.OnMVPSelected;
			if (onMVPSelected != null)
			{
				onMVPSelected(setRoundMVP.MVPPeer.GetComponent<MissionPeer>(), setRoundMVP.MVPCount);
			}
			this.PlayerPropertiesChanged(setRoundMVP.MVPPeer);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0009084C File Offset: 0x0008EA4C
		public void CalculateTotalNumbers()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null)
				{
					int num = missionScoreboardSide.BotScores.DeathCount;
					int num2 = missionScoreboardSide.BotScores.AssistCount;
					int num3 = missionScoreboardSide.BotScores.KillCount;
					foreach (MissionPeer missionPeer in missionScoreboardSide.Players)
					{
						num2 += missionPeer.AssistCount;
						num += missionPeer.DeathCount;
						num3 += missionPeer.KillCount;
					}
				}
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x00090904 File Offset: 0x0008EB04
		private void TeamChange(NetworkCommunicator player, Team oldTeam, Team nextTeam)
		{
			if (oldTeam == null && GameNetwork.VirtualPlayers[player.VirtualPlayer.Index] != player.VirtualPlayer)
			{
				Debug.Print("Ignoring team change call for {}, dced peer.", 0, Debug.DebugColor.White, 17179869184UL);
				return;
			}
			MissionPeer component = player.GetComponent<MissionPeer>();
			if (oldTeam != null)
			{
				if (oldTeam == base.Mission.SpectatorTeam)
				{
					this._spectators.Remove(component);
				}
				else
				{
					this.GetSideSafe(oldTeam.Side).RemovePlayer(component);
				}
			}
			if (nextTeam != null)
			{
				if (nextTeam == base.Mission.SpectatorTeam)
				{
					this._spectators.Add(component);
				}
				else
				{
					Debug.Print(string.Format(">SBC => {0} is switching from {1} to {2}. Adding to scoreboard side {3}.", new object[]
					{
						player.UserName,
						(oldTeam == null) ? "NULL" : oldTeam.Side.ToString(),
						nextTeam.Side.ToString(),
						nextTeam.Side
					}), 0, Debug.DebugColor.Blue, 17179869184UL);
					this.GetSideSafe(nextTeam.Side).AddPlayer(component);
				}
			}
			if (this.OnPlayerSideChanged != null)
			{
				this.OnPlayerSideChanged(oldTeam, nextTeam, component);
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x00090A3C File Offset: 0x0008EC3C
		public override void OnClearScene()
		{
			if (this._mpGameModeBase.RoundComponent == null && GameNetwork.IsServer)
			{
				this.ClearSideScores();
			}
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this.Sides)
			{
				if (missionScoreboardSide != null)
				{
					missionScoreboardSide.BotScores.AliveCount = 0;
				}
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x00090A8C File Offset: 0x0008EC8C
		public override void OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null && component.Team != null)
			{
				this.TeamChange(networkPeer, null, component.Team);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x00090ABC File Offset: 0x0008ECBC
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			MissionPeer missionPeer = networkPeer.GetComponent<MissionPeer>();
			if (missionPeer != null)
			{
				bool flag = this._spectators.Contains(missionPeer);
				bool flag2 = this._sides.Any<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide x) => x != null && x.Players.Contains(missionPeer));
				if (flag)
				{
					this._spectators.Remove(missionPeer);
					return;
				}
				if (flag2)
				{
					this.GetSideSafe(missionPeer.Team.Side).RemovePlayer(missionPeer);
					Formation controlledFormation = missionPeer.ControlledFormation;
					if (controlledFormation != null)
					{
						Team team = missionPeer.Team;
						BotData botScores = this.Sides[(int)team.Side].BotScores;
						botScores.AliveCount += controlledFormation.GetCountOfUnitsWithCondition((Agent agent) => agent.IsActive());
						this.BotPropertiesChanged(team.Side);
					}
					Action<Team, Team, MissionPeer> onPlayerSideChanged = this.OnPlayerSideChanged;
					if (onPlayerSideChanged == null)
					{
						return;
					}
					onPlayerSideChanged(missionPeer.Team, null, missionPeer);
				}
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x00090BD7 File Offset: 0x0008EDD7
		private void BotsControlledChanged(NetworkCommunicator peer)
		{
			this.PlayerPropertiesChanged(peer);
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00090BE0 File Offset: 0x0008EDE0
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsActive() && !agent.IsMount)
			{
				if (agent.MissionPeer == null)
				{
					this.BotPropertiesChanged(agent.Team.Side);
					return;
				}
				if (agent.MissionPeer != null)
				{
					this.PlayerPropertiesChanged(agent.MissionPeer.GetNetworkPeer());
				}
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00090C30 File Offset: 0x0008EE30
		public override void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
			if (agent.MissionPeer != null)
			{
				this.PlayerPropertiesChanged(agent.MissionPeer.GetNetworkPeer());
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x00090C4B File Offset: 0x0008EE4B
		public void BotPropertiesChanged(BattleSideEnum side)
		{
			if (this.OnBotPropertiesChanged != null)
			{
				this.OnBotPropertiesChanged(side);
			}
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x00090C64 File Offset: 0x0008EE64
		public void PlayerPropertiesChanged(NetworkCommunicator player)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return;
			}
			MissionPeer component = player.GetComponent<MissionPeer>();
			if (component != null)
			{
				this.PlayerPropertiesChanged(component);
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x00090C8C File Offset: 0x0008EE8C
		public void PlayerPropertiesChanged(MissionPeer player)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return;
			}
			this.CalculateTotalNumbers();
			if (this.OnPlayerPropertiesChanged != null && player.Team != null && player.Team != Mission.Current.SpectatorTeam)
			{
				BattleSideEnum side = player.Team.Side;
				this.OnPlayerPropertiesChanged(side, player);
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x00090CE4 File Offset: 0x0008EEE4
		protected override void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			networkPeer.GetComponent<MissionPeer>();
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null && !networkPeer.IsServerPeer)
				{
					if (missionScoreboardSide.BotScores.IsAnyValid)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new BotData(missionScoreboardSide.Side, missionScoreboardSide.BotScores.KillCount, missionScoreboardSide.BotScores.AssistCount, missionScoreboardSide.BotScores.DeathCount, missionScoreboardSide.BotScores.AliveCount));
						GameNetwork.EndModuleEventAsServer();
					}
					if (this._mpGameModeBase != null)
					{
						int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
			if (!networkPeer.IsServerPeer && this._mvpCountPerPeer != null)
			{
				foreach (ValueTuple<MissionPeer, int> valueTuple in this._mvpCountPerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new SetRoundMVP(valueTuple.Item1.GetNetworkPeer(), valueTuple.Item2));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x00090E3C File Offset: 0x0008F03C
		public void HandleServerEventBotDataMessage(GameNetworkMessage baseMessage)
		{
			BotData botData = (BotData)baseMessage;
			MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(botData.Side);
			sideSafe.BotScores.KillCount = botData.KillCount;
			sideSafe.BotScores.AssistCount = botData.AssistCount;
			sideSafe.BotScores.DeathCount = botData.DeathCount;
			sideSafe.BotScores.AliveCount = botData.AliveBotCount;
			this.BotPropertiesChanged(botData.Side);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00090EAC File Offset: 0x0008F0AC
		private void ClearSideScores()
		{
			this._sides[1].SideScore = 0;
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.TwoSides)
			{
				this._sides[0].SideScore = 0;
			}
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(0, 0));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00090F10 File Offset: 0x0008F110
		public void OnRoundEnding()
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				this.UpdateRoundScores();
			}
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x00090F1F File Offset: 0x0008F11F
		private void OnMyClientSynchronized()
		{
			this.LateInitializeHeaders();
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x00090F28 File Offset: 0x0008F128
		private void LateInitScoreboard()
		{
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = new MissionScoreboardComponent.MissionScoreboardSide(BattleSideEnum.Attacker);
			this._sides[1] = missionScoreboardSide;
			this._sides[1].BotScores = new BotData();
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.TwoSides)
			{
				MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = new MissionScoreboardComponent.MissionScoreboardSide(BattleSideEnum.Defender);
				this._sides[0] = missionScoreboardSide2;
				this._sides[0].BotScores = new BotData();
			}
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00090F84 File Offset: 0x0008F184
		private void LateInitializeHeaders()
		{
			if (this._isInitialized)
			{
				return;
			}
			this._isInitialized = true;
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null)
				{
					missionScoreboardSide.UpdateHeader(this.Headers);
				}
			}
			if (this.OnScoreboardInitialized != null)
			{
				this.OnScoreboardInitialized();
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00090FDC File Offset: 0x0008F1DC
		public void OnMultiplayerGameClientBehaviorInitialized(ref Action<NetworkCommunicator> onBotsControlledChanged)
		{
			onBotsControlledChanged = (Action<NetworkCommunicator>)Delegate.Combine(onBotsControlledChanged, new Action<NetworkCommunicator>(this.BotsControlledChanged));
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00090FF8 File Offset: 0x0008F1F8
		public BattleSideEnum GetMatchWinnerSide()
		{
			List<int> scores = new List<int>();
			KeyValuePair<BattleSideEnum, int> keyValuePair = new KeyValuePair<BattleSideEnum, int>(BattleSideEnum.None, -1);
			for (int i = 0; i < 2; i++)
			{
				BattleSideEnum battleSideEnum = (BattleSideEnum)i;
				MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(battleSideEnum);
				if (sideSafe.SideScore > keyValuePair.Value && sideSafe.CurrentPlayerCount > 0)
				{
					keyValuePair = new KeyValuePair<BattleSideEnum, int>(battleSideEnum, sideSafe.SideScore);
				}
				scores.Add(sideSafe.SideScore);
			}
			if (!scores.IsEmpty<int>() && scores.All<int>((int s) => s == scores[0]))
			{
				return BattleSideEnum.None;
			}
			return keyValuePair.Key;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000910A0 File Offset: 0x0008F2A0
		private void OnPreRoundEnding()
		{
			if (GameNetwork.IsServer)
			{
				KeyValuePair<MissionPeer, int> keyValuePair2;
				KeyValuePair<MissionPeer, int> keyValuePair4;
				foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this.Sides)
				{
					if (missionScoreboardSide.Side == BattleSideEnum.Attacker)
					{
						KeyValuePair<MissionPeer, int> keyValuePair = missionScoreboardSide.CalculateAndGetMVPScoreWithPeer();
						if (keyValuePair2.Key == null || keyValuePair2.Value < keyValuePair.Value)
						{
							keyValuePair2 = keyValuePair;
						}
					}
					else if (missionScoreboardSide.Side == BattleSideEnum.Defender)
					{
						KeyValuePair<MissionPeer, int> keyValuePair3 = missionScoreboardSide.CalculateAndGetMVPScoreWithPeer();
						if (keyValuePair4.Key == null || keyValuePair4.Value < keyValuePair3.Value)
						{
							keyValuePair4 = keyValuePair3;
						}
					}
				}
				if (keyValuePair2.Key != null)
				{
					this.SetPeerAsMVP(keyValuePair2.Key);
				}
				if (keyValuePair4.Key != null)
				{
					this.SetPeerAsMVP(keyValuePair4.Key);
				}
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0009115C File Offset: 0x0008F35C
		private void SetPeerAsMVP(MissionPeer peer)
		{
			int num = -1;
			for (int i = 0; i < this._mvpCountPerPeer.Count; i++)
			{
				if (peer == this._mvpCountPerPeer[i].Item1)
				{
					num = i;
					break;
				}
			}
			int num2 = 1;
			if (num != -1)
			{
				num2 = this._mvpCountPerPeer[num].Item2 + 1;
				this._mvpCountPerPeer.RemoveAt(num);
			}
			this._mvpCountPerPeer.Add(new ValueTuple<MissionPeer, int>(peer, num2));
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SetRoundMVP(peer.GetNetworkPeer(), num2));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			Action<MissionPeer, int> onMVPSelected = this.OnMVPSelected;
			if (onMVPSelected == null)
			{
				return;
			}
			onMVPSelected(peer, num2);
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00091200 File Offset: 0x0008F400
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && GameNetwork.IsServer && !isBlocked && damagedHp > 0f)
			{
				if (affectorAgent.IsMount)
				{
					affectorAgent = affectorAgent.RiderAgent;
				}
				if (affectorAgent != null)
				{
					MissionPeer missionPeer = affectorAgent.MissionPeer ?? ((affectorAgent.IsAIControlled && affectorAgent.OwningAgentMissionPeer != null) ? affectorAgent.OwningAgentMissionPeer : null);
					if (missionPeer != null)
					{
						int num = (int)damagedHp;
						if (affectedAgent.IsMount)
						{
							num = (int)(damagedHp * 0.35f);
							affectedAgent = affectedAgent.RiderAgent;
						}
						if (affectedAgent != null && affectorAgent != affectedAgent)
						{
							if (!affectorAgent.IsFriendOf(affectedAgent))
							{
								missionPeer.Score += num;
								if (attackerWeapon != null && missionPeer.RegisterWeaponUsage(attackerWeapon.WeaponClass, num))
								{
									GameNetwork.BeginBroadcastModuleEvent();
									GameNetwork.WriteMessage(new PeerMostUsedWeaponChange(missionPeer.GetNetworkPeer(), missionPeer.MostUsedWeaponClass));
									GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
								}
							}
							else
							{
								missionPeer.Score -= (int)((float)num * 1.5f);
							}
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new KillDeathCountChange(missionPeer.GetNetworkPeer(), null, missionPeer.KillCount, missionPeer.AssistCount, missionPeer.DeathCount, missionPeer.Score));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
					}
				}
			}
		}

		// Token: 0x04000EC5 RID: 3781
		private const int TotalSideCount = 2;

		// Token: 0x04000EC6 RID: 3782
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000EC7 RID: 3783
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000EC8 RID: 3784
		private MissionMultiplayerGameModeBaseClient _mpGameModeBase;

		// Token: 0x04000EC9 RID: 3785
		private IScoreboardData _scoreboardData;

		// Token: 0x04000ED0 RID: 3792
		private List<MissionPeer> _spectators;

		// Token: 0x04000ED1 RID: 3793
		private MissionScoreboardComponent.MissionScoreboardSide[] _sides;

		// Token: 0x04000ED2 RID: 3794
		private bool _isInitialized;

		// Token: 0x04000ED3 RID: 3795
		private List<BattleSideEnum> _roundWinnerList;

		// Token: 0x04000ED4 RID: 3796
		private MissionScoreboardComponent.ScoreboardSides _scoreboardSides;

		// Token: 0x04000ED5 RID: 3797
		private List<ValueTuple<MissionPeer, int>> _mvpCountPerPeer;

		// Token: 0x02000591 RID: 1425
		private enum ScoreboardSides
		{
			// Token: 0x04001EE4 RID: 7908
			OneSide,
			// Token: 0x04001EE5 RID: 7909
			TwoSides
		}

		// Token: 0x02000592 RID: 1426
		public struct ScoreboardHeader
		{
			// Token: 0x06003E44 RID: 15940 RVA: 0x000F76D3 File Offset: 0x000F58D3
			public ScoreboardHeader(string id, Func<MissionPeer, string> playerGetterFunc, Func<BotData, string> botGetterFunc)
			{
				this.Id = id;
				this.Name = GameTexts.FindText("str_scoreboard_header", id);
				this._playerGetterFunc = playerGetterFunc;
				this._botGetterFunc = botGetterFunc;
			}

			// Token: 0x06003E45 RID: 15941 RVA: 0x000F76FC File Offset: 0x000F58FC
			public string GetValueOf(MissionPeer missionPeer)
			{
				if (missionPeer == null || this._playerGetterFunc == null)
				{
					string text = "Scoreboard header values are invalid: Peer: ";
					string text2 = ((missionPeer != null) ? missionPeer.ToString() : null) ?? "NULL";
					string text3 = " Getter: ";
					Func<MissionPeer, string> playerGetterFunc = this._playerGetterFunc;
					Debug.FailedAssert(text + text2 + text3 + (((playerGetterFunc != null) ? playerGetterFunc.ToString() : null) ?? "NULL"), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 43);
					return string.Empty;
				}
				string text4;
				try
				{
					text4 = this._playerGetterFunc(missionPeer);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(string.Format("An error occured while trying to get scoreboard value ({0}) for peer: {1}. Exception: {2}", this.Id, missionPeer.Name, ex.InnerException), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 53);
					text4 = string.Empty;
				}
				return text4;
			}

			// Token: 0x06003E46 RID: 15942 RVA: 0x000F77C4 File Offset: 0x000F59C4
			public string GetValueOf(BotData botData)
			{
				if (botData == null || this._botGetterFunc == null)
				{
					string text = "Scoreboard header values are invalid: Bot Data: ";
					string text2 = ((botData != null) ? botData.ToString() : null) ?? "NULL";
					string text3 = " Getter: ";
					Func<BotData, string> botGetterFunc = this._botGetterFunc;
					Debug.FailedAssert(text + text2 + text3 + (((botGetterFunc != null) ? botGetterFunc.ToString() : null) ?? "NULL"), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 62);
					return string.Empty;
				}
				string text4;
				try
				{
					text4 = this._botGetterFunc(botData);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(string.Format("An error occured while trying to get scoreboard value ({0}) for a bot. Exception: {1}", this.Id, ex.InnerException), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 72);
					text4 = string.Empty;
				}
				return text4;
			}

			// Token: 0x04001EE6 RID: 7910
			private readonly Func<MissionPeer, string> _playerGetterFunc;

			// Token: 0x04001EE7 RID: 7911
			private readonly Func<BotData, string> _botGetterFunc;

			// Token: 0x04001EE8 RID: 7912
			public readonly string Id;

			// Token: 0x04001EE9 RID: 7913
			public readonly TextObject Name;
		}

		// Token: 0x02000593 RID: 1427
		public class MissionScoreboardSide
		{
			// Token: 0x17000A8F RID: 2703
			// (get) Token: 0x06003E47 RID: 15943 RVA: 0x000F7884 File Offset: 0x000F5A84
			public int CurrentPlayerCount
			{
				get
				{
					return this._players.Count;
				}
			}

			// Token: 0x17000A90 RID: 2704
			// (get) Token: 0x06003E48 RID: 15944 RVA: 0x000F7891 File Offset: 0x000F5A91
			public IEnumerable<MissionPeer> Players
			{
				get
				{
					return this._players;
				}
			}

			// Token: 0x06003E49 RID: 15945 RVA: 0x000F7899 File Offset: 0x000F5A99
			public MissionScoreboardSide(BattleSideEnum side)
			{
				this.Side = side;
				this._players = new List<MissionPeer>();
				this._playerLastRoundScoreMap = new List<int>();
			}

			// Token: 0x06003E4A RID: 15946 RVA: 0x000F78BE File Offset: 0x000F5ABE
			public void AddPlayer(MissionPeer peer)
			{
				if (!this._players.Contains(peer))
				{
					this._players.Add(peer);
					this._playerLastRoundScoreMap.Add(0);
				}
			}

			// Token: 0x06003E4B RID: 15947 RVA: 0x000F78E8 File Offset: 0x000F5AE8
			public void RemovePlayer(MissionPeer peer)
			{
				for (int i = 0; i < this._players.Count; i++)
				{
					if (this._players[i] == peer)
					{
						this._players.RemoveAt(i);
						this._playerLastRoundScoreMap.RemoveAt(i);
						return;
					}
				}
			}

			// Token: 0x06003E4C RID: 15948 RVA: 0x000F7934 File Offset: 0x000F5B34
			public string[] GetValuesOf(MissionPeer peer)
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				if (peer == null)
				{
					for (int i = 0; i < this._properties.Length; i++)
					{
						array[i] = this._properties[i].GetValueOf(this.BotScores);
					}
					return array;
				}
				for (int j = 0; j < this._properties.Length; j++)
				{
					array[j] = this._properties[j].GetValueOf(peer);
				}
				return array;
			}

			// Token: 0x06003E4D RID: 15949 RVA: 0x000F79BC File Offset: 0x000F5BBC
			public string[] GetHeaderNames()
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				for (int i = 0; i < this._properties.Length; i++)
				{
					array[i] = this._properties[i].Name.ToString();
				}
				return array;
			}

			// Token: 0x06003E4E RID: 15950 RVA: 0x000F7A14 File Offset: 0x000F5C14
			public string[] GetHeaderIds()
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				for (int i = 0; i < this._properties.Length; i++)
				{
					array[i] = this._properties[i].Id;
				}
				return array;
			}

			// Token: 0x06003E4F RID: 15951 RVA: 0x000F7A68 File Offset: 0x000F5C68
			public int GetScore(MissionPeer peer)
			{
				if (this._properties == null)
				{
					return 0;
				}
				string text;
				if (peer == null)
				{
					if (this._properties.Any<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader p) => p.Id == "score"))
					{
						text = this._properties.FirstOrDefault<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader x) => x.Id == "score").GetValueOf(this.BotScores);
					}
					else
					{
						text = string.Empty;
					}
				}
				else if (this._properties.Any<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader p) => p.Id == "score"))
				{
					text = this._properties.Single<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader x) => x.Id == "score").GetValueOf(peer);
				}
				else
				{
					text = string.Empty;
				}
				int num = 0;
				int.TryParse(text, out num);
				return num;
			}

			// Token: 0x06003E50 RID: 15952 RVA: 0x000F7B65 File Offset: 0x000F5D65
			public void UpdateHeader(MissionScoreboardComponent.ScoreboardHeader[] headers)
			{
				this._properties = headers;
			}

			// Token: 0x06003E51 RID: 15953 RVA: 0x000F7B6E File Offset: 0x000F5D6E
			public void Clear()
			{
				this._players.Clear();
			}

			// Token: 0x06003E52 RID: 15954 RVA: 0x000F7B7C File Offset: 0x000F5D7C
			public KeyValuePair<MissionPeer, int> CalculateAndGetMVPScoreWithPeer()
			{
				KeyValuePair<MissionPeer, int> keyValuePair = default(KeyValuePair<MissionPeer, int>);
				for (int i = 0; i < this._players.Count; i++)
				{
					int num = this._players[i].Score - this._playerLastRoundScoreMap[i];
					this._playerLastRoundScoreMap[i] = this._players[i].Score;
					if (keyValuePair.Key == null || keyValuePair.Value < num)
					{
						keyValuePair = new KeyValuePair<MissionPeer, int>(this._players[i], num);
					}
				}
				return keyValuePair;
			}

			// Token: 0x04001EEA RID: 7914
			public readonly BattleSideEnum Side;

			// Token: 0x04001EEB RID: 7915
			private MissionScoreboardComponent.ScoreboardHeader[] _properties;

			// Token: 0x04001EEC RID: 7916
			public BotData BotScores;

			// Token: 0x04001EED RID: 7917
			public int SideScore;

			// Token: 0x04001EEE RID: 7918
			private List<MissionPeer> _players;

			// Token: 0x04001EEF RID: 7919
			private List<int> _playerLastRoundScoreMap;
		}
	}
}
