using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages.FromCustomBattleServer.ToCustomBattleServerManager;
using Messages.FromCustomBattleServerManager.ToCustomBattleServer;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000113 RID: 275
	public class CustomBattleServer : Client
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0000767A File Offset: 0x0000587A
		public bool Finished
		{
			get
			{
				return this._state == CustomBattleServer.State.Finished;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00007685 File Offset: 0x00005885
		public bool IsRegistered
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame || this._state == CustomBattleServer.State.RegisteredServer;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0000769B File Offset: 0x0000589B
		public bool IsPlaying
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x000076A6 File Offset: 0x000058A6
		public bool Connected
		{
			get
			{
				return this.CurrentState != CustomBattleServer.State.Working && this.CurrentState > CustomBattleServer.State.Idle;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x000076BC File Offset: 0x000058BC
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x000076C4 File Offset: 0x000058C4
		public CustomBattleServer.State CurrentState
		{
			get
			{
				return this._state;
			}
			private set
			{
				if (this._state != value)
				{
					CustomBattleServer.State state = this._state;
					this._state = value;
					if (this._handler != null)
					{
						this._handler.OnStateChanged(state);
					}
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000076FC File Offset: 0x000058FC
		public bool IsIdle
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame && this._customBattlePlayers.Count == 0 && this._useTimeoutTimer && this._timeoutTimer.ElapsedMilliseconds > (long)this._timeoutDuration;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00007734 File Offset: 0x00005934
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0000773C File Offset: 0x0000593C
		public string CustomGameType { get; private set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00007745 File Offset: 0x00005945
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0000774D File Offset: 0x0000594D
		public string CustomGameScene { get; private set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00007756 File Offset: 0x00005956
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x0000775E File Offset: 0x0000595E
		public int Port { get; private set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00007767 File Offset: 0x00005967
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0000776F File Offset: 0x0000596F
		public MultipleBattleResult BattleResult { get; private set; }

		// Token: 0x0600060E RID: 1550 RVA: 0x00007778 File Offset: 0x00005978
		public CustomBattleServer(DiamondClientApplication diamondClientApplication, IClientSessionFactory provider)
			: base(diamondClientApplication, provider, false)
		{
			this._peerId = new PeerId(Guid.NewGuid());
			this._customBattlePlayers = new List<PlayerId>();
			this._requestedPlayers = new List<PlayerId>();
			this._timeoutTimer = new Stopwatch();
			this._terminationTime = null;
			this._state = CustomBattleServer.State.Idle;
			this._timer = new Stopwatch();
			this._timer.Start();
			if (!base.Application.Parameters.TryGetParameterAsInt("CustomBattleServer.TimeoutDuration", out this._timeoutDuration))
			{
				this._timeoutDuration = this._defaultServerTimeoutDuration;
			}
			this._badgeComponent = null;
			this._badgeComponentPlayers = new List<PlayerData>();
			this.BattleResult = new MultipleBattleResult();
			try
			{
				CosmeticsManager.Initialize(ModuleHelper.GetModuleFullPath("Native") + "ModuleData");
			}
			catch
			{
			}
			this._pendingDisconnects = new List<PlayerDisconnectData>();
			this._pendingJoinResponses = new List<PlayerJoinGameResponseDataFromHost>();
			base.AddMessageHandler<ClientWantsToConnectCustomGameMessage>(new ClientMessageHandler<ClientWantsToConnectCustomGameMessage>(this.OnClientWantsToConnectCustomGameMessage));
			base.AddMessageHandler<ClientQuitFromCustomGameMessage>(new ClientMessageHandler<ClientQuitFromCustomGameMessage>(this.OnClientQuitFromCustomGameMessage));
			base.AddMessageHandler<TerminateOperationCustomMessage>(new ClientMessageHandler<TerminateOperationCustomMessage>(this.OnTerminateOperationCustomMessage));
			base.AddMessageHandler<SetChatFilterListsMessage>(new ClientMessageHandler<SetChatFilterListsMessage>(this.OnSetChatFilterListsMessage));
			base.AddMessageHandler<PlayerDisconnectedFromLobbyMessage>(new ClientMessageHandler<PlayerDisconnectedFromLobbyMessage>(this.OnPlayerDisconnectedFromLobbyMessage));
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000078D8 File Offset: 0x00005AD8
		public void SetBadgeComponent(IBadgeComponent badgeComponent)
		{
			this._badgeComponent = badgeComponent;
			if (this._badgeComponent != null)
			{
				foreach (PlayerData playerData in this._badgeComponentPlayers)
				{
					this._badgeComponent.OnPlayerJoin(playerData);
				}
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00007940 File Offset: 0x00005B40
		public void Connect(ICustomBattleServerSessionHandler handler, string authToken, bool isSinglePlatformServer, string[] loadedModuleIDs, bool allowsOptionalModules, bool isPlayerHosted)
		{
			this._handler = handler;
			this._authToken = authToken;
			this._allowsOptionalModules = allowsOptionalModules;
			this._useTimeoutTimer = !isPlayerHosted;
			this._isSinglePlatformServer = isSinglePlatformServer;
			this._loadedModules = new List<ModuleInfoModel>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetSortedModules(loadedModuleIDs))
			{
				if (!allowsOptionalModules && moduleInfo.Category == ModuleCategory.MultiplayerOptional)
				{
					throw new InvalidOperationException("Optional modules are explicitly disallowed, yet an optional module (" + moduleInfo.Id + ") was loaded! You must use category 'Server' instead of 'MultiplayerOptional'.");
				}
				ModuleInfoModel moduleInfoModel;
				if (ModuleInfoModel.TryCreateForSession(moduleInfo, out moduleInfoModel))
				{
					this._loadedModules.Add(moduleInfoModel);
				}
			}
			this.CurrentState = CustomBattleServer.State.Working;
			base.BeginConnect();
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00007A0C File Offset: 0x00005C0C
		public override int MaxConsecutiveFailuresBeforeDisconnect
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00007A10 File Offset: 0x00005C10
		public override void OnConnected()
		{
			base.OnConnected();
			this.CurrentState = CustomBattleServer.State.Connected;
			if (this._handler != null)
			{
				this._handler.OnConnected();
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00007A32 File Offset: 0x00005C32
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this.CurrentState = CustomBattleServer.State.Idle;
			if (this._handler != null)
			{
				this._handler.OnCantConnect();
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00007A54 File Offset: 0x00005C54
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			this.CurrentState = CustomBattleServer.State.Idle;
			if (this._handler != null)
			{
				this._handler.OnDisconnected();
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00007A78 File Offset: 0x00005C78
		protected override void OnTick()
		{
			if (this._terminationTime != null && this._terminationTime < DateTime.UtcNow)
			{
				throw new Exception("Now I am become Death, the destroyer of worlds");
			}
			long elapsedMilliseconds = this._timer.ElapsedMilliseconds;
			float num = (float)(elapsedMilliseconds - this._previousTimeInMS);
			this._previousTimeInMS = elapsedMilliseconds;
			float num2 = num / 1000f;
			this._battleResultUpdateTimeElapsed += num2;
			if (this._battleResultUpdateTimeElapsed >= 5f)
			{
				if (this._latestQueuedBattleResult != null && this._latestQueuedTeamScores != null && this._latestQueuedPlayerScores != null)
				{
					base.SendMessage(new CustomBattleServerStatsUpdateMessage(this._latestQueuedBattleResult, this._latestQueuedTeamScores, this._latestQueuedPlayerScores));
					this._latestQueuedBattleResult = null;
					this._latestQueuedTeamScores = null;
					this._latestQueuedPlayerScores = null;
				}
				this._battleResultUpdateTimeElapsed = 0f;
			}
			this._disconnectBatchTimeElapsed += num2;
			if (this._disconnectBatchTimeElapsed >= 3f)
			{
				this.FlushDisconnectBatch();
				this._disconnectBatchTimeElapsed = 0f;
			}
			this._joinResponseBatchTimeElapsed += num2;
			if (this._joinResponseBatchTimeElapsed >= 3f)
			{
				this.FlushJoinResponseBatch();
				this._joinResponseBatchTimeElapsed = 0f;
			}
			CustomBattleServer.State state = this._state;
			if (state == CustomBattleServer.State.Connected)
			{
				this.DoLogin();
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00007BC4 File Offset: 0x00005DC4
		private async void DoLogin()
		{
			this._state = CustomBattleServer.State.SessionRequested;
			LoginResult loginResult = await base.Login(new CustomBattleServerReadyMessage(this._peerId, base.ApplicationVersion, this._authToken, this._loadedModules.ToArray(), this._allowsOptionalModules));
			if (loginResult != null && loginResult.Successful)
			{
				this._state = CustomBattleServer.State.RegisteredServer;
			}
			else
			{
				Console.WriteLine("Login Failed! Server is shutting down.");
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00007BFD File Offset: 0x00005DFD
		private void OnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			this.HandleOnClientWantsToConnectCustomGameMessage(message);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00007C08 File Offset: 0x00005E08
		private async void HandleOnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			if (this.CurrentState == CustomBattleServer.State.Finished)
			{
				PlayerJoinGameData[] playerJoinGameData = message.PlayerJoinGameData;
				List<PlayerJoinGameResponseDataFromHost> list = new List<PlayerJoinGameResponseDataFromHost>();
				foreach (PlayerJoinGameData playerJoinGameData2 in playerJoinGameData)
				{
					list.Add(new PlayerJoinGameResponseDataFromHost
					{
						PlayerId = playerJoinGameData2.PlayerId,
						PeerIndex = -1,
						SessionKey = -1,
						CustomGameJoinResponse = CustomGameJoinResponse.CustomGameServerFinishing
					});
				}
				this.ResponseCustomGameClientConnection(list.ToArray());
			}
			else
			{
				PlayerJoinGameData[] requestedPlayers = message.PlayerJoinGameData;
				for (int k = 0; k < requestedPlayers.Length; k++)
				{
					if (requestedPlayers[k] != null)
					{
						PlayerJoinGameData playerJoinGameData3 = requestedPlayers[k];
						Debug.Print(string.Concat(new object[] { "Player ", playerJoinGameData3.Name, " - ", playerJoinGameData3.PlayerId, " with IP address ", playerJoinGameData3.IpAddress, " wants to join the game" }), 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				int j;
				for (int i = 0; i < requestedPlayers.Length; i = j + 1)
				{
					if (requestedPlayers[i] != null)
					{
						List<PlayerJoinGameData> requestedGroup = new List<PlayerJoinGameData>();
						PlayerJoinGameData playerJoinGameData4 = requestedPlayers[i];
						Guid? guid = playerJoinGameData4.PartyId;
						if (guid == null)
						{
							requestedGroup.Add(playerJoinGameData4);
						}
						else
						{
							for (int l = i; l < requestedPlayers.Length; l++)
							{
								PlayerJoinGameData playerJoinGameData5 = requestedPlayers[l];
								guid = playerJoinGameData4.PartyId;
								if (guid.Equals((playerJoinGameData5 != null) ? playerJoinGameData5.PartyId : null))
								{
									requestedGroup.Add(playerJoinGameData5);
									requestedPlayers[l] = null;
								}
							}
						}
						bool flag = true;
						foreach (PlayerJoinGameData playerJoinGameData6 in requestedGroup)
						{
							if (this._requestedPlayers.Contains(playerJoinGameData6.PlayerId) || this._customBattlePlayers.Contains(playerJoinGameData6.PlayerId))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this._timeoutTimer.Restart();
							foreach (PlayerJoinGameData playerJoinGameData7 in requestedGroup)
							{
								this._requestedPlayers.Add(playerJoinGameData7.PlayerId);
							}
							if (this._handler != null)
							{
								PlayerJoinGameResponseDataFromHost[] array2 = await this._handler.OnClientWantsToConnectCustomGame(requestedGroup.ToArray());
								if (this._badgeComponent != null)
								{
									foreach (PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost in array2)
									{
										if (playerJoinGameResponseDataFromHost.CustomGameJoinResponse == CustomGameJoinResponse.Success)
										{
											foreach (PlayerJoinGameData playerJoinGameData8 in requestedGroup)
											{
												if (playerJoinGameData8.PlayerId.Equals(playerJoinGameResponseDataFromHost.PlayerId))
												{
													this._badgeComponent.OnPlayerJoin(playerJoinGameData8.PlayerData);
													this._badgeComponentPlayers.Add(playerJoinGameData8.PlayerData);
												}
											}
										}
									}
								}
								this._pendingJoinResponses.AddRange(array2);
							}
						}
						else
						{
							foreach (PlayerJoinGameData playerJoinGameData9 in requestedGroup)
							{
								this._pendingJoinResponses.Add(new PlayerJoinGameResponseDataFromHost
								{
									PlayerId = playerJoinGameData9.PlayerId,
									PeerIndex = -1,
									SessionKey = -1,
									CustomGameJoinResponse = CustomGameJoinResponse.NotAllPlayersReady
								});
							}
						}
						requestedGroup = null;
					}
					j = i;
				}
				requestedPlayers = null;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00007C4C File Offset: 0x00005E4C
		private void OnClientQuitFromCustomGameMessage(ClientQuitFromCustomGameMessage message)
		{
			if (this.CurrentState == CustomBattleServer.State.RegisteredGame && this._customBattlePlayers.Contains(message.PlayerId))
			{
				if (this._handler != null)
				{
					this._handler.OnClientQuitFromCustomGame(message.PlayerId);
				}
				this._customBattlePlayers.Remove(message.PlayerId);
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public void OnPlayerDisconnectedFromLobbyMessage(PlayerDisconnectedFromLobbyMessage message)
		{
			this.HandlePlayerDisconnect(message.PlayerId, DisconnectType.DisconnectedFromLobby);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00007CB0 File Offset: 0x00005EB0
		private void OnTerminateOperationCustomMessage(TerminateOperationCustomMessage message)
		{
			Random random = new Random();
			this._terminationTime = new DateTime?(DateTime.UtcNow.AddMilliseconds((double)random.Next(3000, 10000)));
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00007CEC File Offset: 0x00005EEC
		private void OnSetChatFilterListsMessage(SetChatFilterListsMessage message)
		{
			if (this._handler != null)
			{
				this._handler.OnChatFilterListsReceived(message.ProfanityList, message.AllowList);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00007D10 File Offset: 0x00005F10
		public void ResponseCustomGameClientConnection(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			if (this.CurrentState == CustomBattleServer.State.RegisteredGame)
			{
				foreach (PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost in playerJoinData)
				{
					this._requestedPlayers.Remove(playerJoinGameResponseDataFromHost.PlayerId);
					if (playerJoinGameResponseDataFromHost.CustomGameJoinResponse == CustomGameJoinResponse.Success)
					{
						this._customBattlePlayers.Add(playerJoinGameResponseDataFromHost.PlayerId);
					}
				}
				base.SendMessage(new ResponseCustomGameClientConnectionMessage(playerJoinData));
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00007D74 File Offset: 0x00005F74
		public async Task RegisterGame(string gameModule, string gameType, string serverName, int maxPlayerCount, string scene, string uniqueSceneId, int port, string region, string gamePassword, string adminPassword, string spectatorPassword, int permission, int maxSpectatorCount = 0, bool enableSpectators = false)
		{
			await this.RegisterGame(0, gameModule, gameType, serverName, maxPlayerCount, scene, uniqueSceneId, port, region, gamePassword, adminPassword, spectatorPassword, permission, string.Empty, maxSpectatorCount, enableSpectators);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00007E34 File Offset: 0x00006034
		public async Task RegisterGame(int gameDefinitionId, string gameModule, string gameType, string serverName, int maxPlayerCount, string scene, string uniqueSceneId, int port, string region, string gamePassword, string adminPassword, string spectatorPassword, int permission, string overriddenIP, int maxSpectatorCount = 0, bool enableSpectators = false)
		{
			this.Port = port;
			this.CustomGameType = gameType;
			this.CustomGameScene = scene;
			string text = null;
			bool flag = false;
			if (base.Application.Parameters.TryGetParameter("CustomBattleServer.Host.Address", out text))
			{
				flag = true;
			}
			if (overriddenIP != string.Empty)
			{
				flag = true;
				text = overriddenIP;
			}
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<RegisterCustomGameMessageResponseMessage>(new RegisterCustomGameMessage(gameDefinitionId, gameModule, gameType, serverName, text, maxPlayerCount, scene, uniqueSceneId, gamePassword, adminPassword, spectatorPassword, port, region, permission, !this._isSinglePlatformServer, flag, maxSpectatorCount, enableSpectators)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			RegisterCustomGameMessageResponseMessage registerCustomGameMessageResponseMessage = taskAwaiter.GetResult().Result as RegisterCustomGameMessageResponseMessage;
			this._shouldReportActivities = registerCustomGameMessageResponseMessage.ShouldReportActivities;
			this.CurrentState = CustomBattleServer.State.RegisteredGame;
			this._timeoutTimer.Start();
			if (this._handler != null)
			{
				this._handler.OnSuccessfulGameRegister();
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00007F06 File Offset: 0x00006106
		public void UpdateCustomGameData(string newGameType, string newMap, int newCount)
		{
			base.SendMessage(new UpdateCustomGameData(newGameType, newMap, newCount));
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00007F16 File Offset: 0x00006116
		public void KickPlayer(PlayerId id, bool banPlayer)
		{
			ICustomBattleServerSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerKickRequested(id, banPlayer);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00007F2A File Offset: 0x0000612A
		public void HandlePlayerDisconnect(PlayerId playerId, DisconnectType disconnectType)
		{
			this._timeoutTimer.Restart();
			this._customBattlePlayers.Remove(playerId);
			this._pendingDisconnects.Add(new PlayerDisconnectData(playerId, disconnectType));
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00007F56 File Offset: 0x00006156
		private void FlushDisconnectBatch()
		{
			if (this._pendingDisconnects.Count == 0)
			{
				return;
			}
			base.SendMessage(new PlayersDisconnectedMessage(this._pendingDisconnects.ToArray()));
			this._pendingDisconnects.Clear();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00007F87 File Offset: 0x00006187
		private void FlushJoinResponseBatch()
		{
			if (this._pendingJoinResponses.Count == 0)
			{
				return;
			}
			this.ResponseCustomGameClientConnection(this._pendingJoinResponses.ToArray());
			this._pendingJoinResponses.Clear();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00007FB3 File Offset: 0x000061B3
		public void FinishAsIdle(GameLog[] gameLogs)
		{
			this.FinishGame(gameLogs);
			base.BeginDisconnect();
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00007FC4 File Offset: 0x000061C4
		public void FinishGame(GameLog[] gameLogs)
		{
			this.CurrentState = CustomBattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnGameFinished();
			}
			this.FlushJoinResponseBatch();
			this.FlushDisconnectBatch();
			IBadgeComponent badgeComponent = this._badgeComponent;
			base.SendMessage(new CustomBattleServerFinishingMessage(gameLogs, (badgeComponent != null) ? badgeComponent.DataDictionary : null, this.BattleResult));
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0000801B File Offset: 0x0000621B
		public void UpdateGameProperties(string gameType, string scene, string uniqueSceneId)
		{
			this.CustomGameType = gameType;
			this.CustomGameScene = scene;
			base.SendMessage(new UpdateGamePropertiesMessage(gameType, scene, uniqueSceneId));
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00008039 File Offset: 0x00006239
		public void BeforeStartingNextBattle(GameLog[] gameLogs)
		{
			IBadgeComponent badgeComponent = this._badgeComponent;
			if (badgeComponent != null)
			{
				badgeComponent.OnStartingNextBattle();
			}
			if (gameLogs != null && gameLogs.Length != 0)
			{
				base.SendMessage(new AddGameLogsMessage(gameLogs));
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000805F File Offset: 0x0000625F
		public void BattleStarted(Dictionary<PlayerId, int> playerTeams, string cultureTeam1, string cultureTeam2)
		{
			if (this._shouldReportActivities)
			{
				base.SendMessage(new CustomBattleStartedMessage(this.CustomGameType, playerTeams, new List<string> { cultureTeam2, cultureTeam1 }));
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000808E File Offset: 0x0000628E
		public void BattleFinished(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			if (this._shouldReportActivities)
			{
				base.SendMessage(new CustomBattleFinishedMessage(battleResult, teamScores, playerScores));
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000080A6 File Offset: 0x000062A6
		public void UpdateBattleStats(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			if (this._shouldReportActivities)
			{
				this._latestQueuedBattleResult = battleResult;
				this._latestQueuedTeamScores = teamScores;
				this._latestQueuedPlayerScores = playerScores;
			}
		}

		// Token: 0x04000243 RID: 579
		private CustomBattleServer.State _state;

		// Token: 0x04000244 RID: 580
		private string _authToken;

		// Token: 0x04000245 RID: 581
		private List<ModuleInfoModel> _loadedModules;

		// Token: 0x04000246 RID: 582
		private bool _allowsOptionalModules;

		// Token: 0x04000247 RID: 583
		private bool _isSinglePlatformServer;

		// Token: 0x04000248 RID: 584
		private Stopwatch _timer;

		// Token: 0x04000249 RID: 585
		private long _previousTimeInMS;

		// Token: 0x0400024E RID: 590
		private ICustomBattleServerSessionHandler _handler;

		// Token: 0x0400024F RID: 591
		private PeerId _peerId;

		// Token: 0x04000250 RID: 592
		private List<PlayerId> _customBattlePlayers;

		// Token: 0x04000251 RID: 593
		private List<PlayerId> _requestedPlayers;

		// Token: 0x04000252 RID: 594
		private int _defaultServerTimeoutDuration = 600000;

		// Token: 0x04000253 RID: 595
		private int _timeoutDuration;

		// Token: 0x04000254 RID: 596
		private Stopwatch _timeoutTimer;

		// Token: 0x04000255 RID: 597
		private DateTime? _terminationTime;

		// Token: 0x04000256 RID: 598
		private bool _useTimeoutTimer;

		// Token: 0x04000257 RID: 599
		private IBadgeComponent _badgeComponent;

		// Token: 0x04000258 RID: 600
		private readonly List<PlayerData> _badgeComponentPlayers;

		// Token: 0x04000259 RID: 601
		private bool _shouldReportActivities;

		// Token: 0x0400025A RID: 602
		private const float BattleResultUpdatePeriod = 5f;

		// Token: 0x0400025B RID: 603
		private float _battleResultUpdateTimeElapsed;

		// Token: 0x0400025C RID: 604
		private BattleResult _latestQueuedBattleResult;

		// Token: 0x0400025D RID: 605
		private Dictionary<int, int> _latestQueuedTeamScores;

		// Token: 0x0400025E RID: 606
		private Dictionary<PlayerId, int> _latestQueuedPlayerScores;

		// Token: 0x0400025F RID: 607
		private const float DisconnectBatchFlushPeriod = 3f;

		// Token: 0x04000260 RID: 608
		private float _disconnectBatchTimeElapsed;

		// Token: 0x04000261 RID: 609
		private List<PlayerDisconnectData> _pendingDisconnects;

		// Token: 0x04000262 RID: 610
		private const float JoinResponseBatchFlushPeriod = 3f;

		// Token: 0x04000263 RID: 611
		private float _joinResponseBatchTimeElapsed;

		// Token: 0x04000264 RID: 612
		private List<PlayerJoinGameResponseDataFromHost> _pendingJoinResponses;

		// Token: 0x02000192 RID: 402
		public enum State
		{
			// Token: 0x0400058D RID: 1421
			Idle,
			// Token: 0x0400058E RID: 1422
			Working,
			// Token: 0x0400058F RID: 1423
			Connected,
			// Token: 0x04000590 RID: 1424
			SessionRequested,
			// Token: 0x04000591 RID: 1425
			RegisteredServer,
			// Token: 0x04000592 RID: 1426
			RegisteredGame,
			// Token: 0x04000593 RID: 1427
			Finished
		}
	}
}
