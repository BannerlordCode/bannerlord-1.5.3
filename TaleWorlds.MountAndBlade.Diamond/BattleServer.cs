using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages.FromBattleServer.ToBattleServerManager;
using Messages.FromBattleServerManager.ToBattleServer;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FD RID: 253
	public class BattleServer : Client
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00005EAF File Offset: 0x000040AF
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00005EB7 File Offset: 0x000040B7
		public string SceneName { get; private set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00005EC0 File Offset: 0x000040C0
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00005EC8 File Offset: 0x000040C8
		public string GameType { get; private set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00005ED1 File Offset: 0x000040D1
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00005ED9 File Offset: 0x000040D9
		public string Faction1 { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00005EE2 File Offset: 0x000040E2
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00005EEA File Offset: 0x000040EA
		public string Faction2 { get; private set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00005EF3 File Offset: 0x000040F3
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00005EFB File Offset: 0x000040FB
		public int MinRequiredPlayerCountToStartBattle { get; private set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00005F04 File Offset: 0x00004104
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00005F0C File Offset: 0x0000410C
		public int BattleSize { get; private set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00005F15 File Offset: 0x00004115
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00005F1D File Offset: 0x0000411D
		public int RoundThreshold { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00005F26 File Offset: 0x00004126
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00005F2E File Offset: 0x0000412E
		public float MoraleThreshold { get; private set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00005F37 File Offset: 0x00004137
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00005F3F File Offset: 0x0000413F
		public Guid BattleId { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00005F48 File Offset: 0x00004148
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00005F50 File Offset: 0x00004150
		public bool UseAnalytics { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00005F59 File Offset: 0x00004159
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00005F61 File Offset: 0x00004161
		public bool CaptureMovementData { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00005F6A File Offset: 0x0000416A
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00005F72 File Offset: 0x00004172
		public string AnalyticsServiceAddress { get; private set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00005F7B File Offset: 0x0000417B
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00005F83 File Offset: 0x00004183
		public bool IsPremadeGame { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00005F8C File Offset: 0x0000418C
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00005F94 File Offset: 0x00004194
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00005F9D File Offset: 0x0000419D
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00005FA5 File Offset: 0x000041A5
		public PlayerId[] AssignedPlayers { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00005FAE File Offset: 0x000041AE
		public bool IsActive
		{
			get
			{
				return this._state == BattleServer.State.BattleAssigned || this._state == BattleServer.State.Running || this._state == BattleServer.State.WaitingBattle;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00005FCD File Offset: 0x000041CD
		public bool IsFinished
		{
			get
			{
				return this._state == BattleServer.State.Finished;
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00005FD8 File Offset: 0x000041D8
		public BattleServer(DiamondClientApplication diamondClientApplication, IClientSessionFactory provider)
			: base(diamondClientApplication, provider, false)
		{
			this._state = BattleServer.State.Idle;
			this._peerId = new PeerId(Guid.NewGuid());
			base.Application.Parameters.TryGetParameter("BattleServer.Host.Address", out this._assignedAddress);
			base.Application.Parameters.TryGetParameterAsUInt16("BattleServer.Host.Port", out this._assignedPort);
			base.Application.Parameters.TryGetParameter("BattleServer.Host.Region", out this._region);
			base.Application.Parameters.TryGetParameterAsSByte("BattleServer.Host.Priority", out this._priority);
			base.Application.Parameters.TryGetParameterAsByte("BattleServer.Host.NumCores", out this._numCores);
			base.Application.Parameters.TryGetParameter("BattleServer.Password", out this._password);
			base.Application.Parameters.TryGetParameter("BattleServer.Host.GameMode", out this._gameMode);
			if (!base.Application.Parameters.TryGetParameterAsInt("BattleServer.TimeoutDuration", out this._timeoutDuration))
			{
				this._timeoutDuration = this._defaultServerTimeoutDuration;
			}
			this._passedTimeSinceLastMaxAllowedPriorityRequest = this._requestMaxAllowedPriorityIntervalInSeconds * 2f;
			this._peers = new List<BattlePeer>();
			this._timer = new Stopwatch();
			this._timer.Start();
			this._timeoutTimer = new Stopwatch();
			this._terminationTime = null;
			this._maxAllowedPriority = sbyte.MaxValue;
			this._newPlayerRequests = new Queue<NewPlayerMessage>();
			this._isWarmupEnded = false;
			this._playerSpawnCounts = new Dictionary<PlayerId, int>();
			this._badgeComponent = null;
			this._playerPartyMap = new Dictionary<PlayerId, Guid>();
			this._playerRoundFriendlyDamageMap = new Dictionary<PlayerId, Dictionary<int, ValueTuple<int, float>>>();
			this._maxFriendlyKillCount = int.MaxValue;
			this._maxFriendlyDamage = float.MaxValue;
			this._maxFriendlyDamagePerSingleRound = float.MaxValue;
			this._roundFriendlyDamageLimit = float.MaxValue;
			this._maxRoundsOverLimitCount = int.MaxValue;
			CosmeticsManager.Initialize(ModuleHelper.GetModuleFullPath("Native") + "ModuleData");
			base.AddMessageHandler<NewPlayerMessage>(new ClientMessageHandler<NewPlayerMessage>(this.OnNewPlayerMessage));
			base.AddMessageHandler<StartBattleMessage>(new ClientMessageHandler<StartBattleMessage>(this.OnStartBattleMessage));
			base.AddMessageHandler<PlayerFledBattleMessage>(new ClientMessageHandler<PlayerFledBattleMessage>(this.OnPlayerFledBattleMessage));
			base.AddMessageHandler<PlayerDisconnectedFromLobbyMessage>(new ClientMessageHandler<PlayerDisconnectedFromLobbyMessage>(this.OnPlayerDisconnectedFromLobbyMessage));
			base.AddMessageHandler<TerminateOperationMatchmakingMessage>(new ClientMessageHandler<TerminateOperationMatchmakingMessage>(this.OnTerminateOperationMatchmakingMessage));
			base.AddMessageHandler<FriendlyDamageKickPlayerResponseMessage>(new ClientMessageHandler<FriendlyDamageKickPlayerResponseMessage>(this.OnFriendlyDamageKickPlayerResponseMessage));
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00006257 File Offset: 0x00004457
		public void Initialize(IBattleServerSessionHandler handler)
		{
			this._handler = handler;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00006260 File Offset: 0x00004460
		public void SetBadgeComponent(IBadgeComponent badgeComponent)
		{
			this._badgeComponent = badgeComponent;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00006269 File Offset: 0x00004469
		public void StartServer()
		{
			this._state = BattleServer.State.Connecting;
			base.BeginConnect();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00006278 File Offset: 0x00004478
		protected override void OnTick()
		{
			if (this._terminationTime != null && this._terminationTime < DateTime.UtcNow)
			{
				throw new Exception("I am sorry Dave, I am afraid I can't do that");
			}
			long elapsedMilliseconds = this._timer.ElapsedMilliseconds;
			float num = (float)(elapsedMilliseconds - this._previousTimeInMS);
			this._previousTimeInMS = elapsedMilliseconds;
			float num2 = num / 1000f;
			this._passedTimeSinceLastMaxAllowedPriorityRequest += num2;
			this._battleResultUpdateTimeElapsed += num2;
			if (this._battleResultUpdateTimeElapsed >= 5f)
			{
				if (this._latestQueuedBattleResult != null && this._latestQueuedTeamScores != null)
				{
					base.SendMessage(new BattleServerStatsUpdateMessage(this._latestQueuedBattleResult, this._latestQueuedTeamScores));
					this._latestQueuedBattleResult = null;
					this._latestQueuedTeamScores = null;
				}
				this._battleResultUpdateTimeElapsed = 0f;
			}
			switch (this._state)
			{
			case BattleServer.State.Idle:
			case BattleServer.State.Connecting:
			case BattleServer.State.Connected:
			case BattleServer.State.LoggingIn:
			case BattleServer.State.BattleAssigned:
			case BattleServer.State.Running:
			case BattleServer.State.Finishing:
			case BattleServer.State.Finished:
				break;
			case BattleServer.State.WaitingBattle:
				if (this._passedTimeSinceLastMaxAllowedPriorityRequest > this._requestMaxAllowedPriorityIntervalInSeconds)
				{
					this.UpdateMaxAllowedPriority();
				}
				if (this._priority > this._maxAllowedPriority || this._timeoutTimer.ElapsedMilliseconds > (long)this._timeoutDuration)
				{
					this.Shutdown();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000063C4 File Offset: 0x000045C4
		private async void DoLogin()
		{
			this._state = BattleServer.State.LoggingIn;
			LoginResult loginResult = await base.Login(new BattleServerReadyMessage(this._peerId, base.ApplicationVersion, this._assignedAddress, this._assignedPort, this._region, this._priority, this._password, this._gameMode));
			if (loginResult != null && loginResult.Successful)
			{
				this._state = BattleServer.State.WaitingBattle;
				this._timeoutTimer.Reset();
				this._timeoutTimer.Start();
			}
			else
			{
				this._state = BattleServer.State.Finished;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000063FD File Offset: 0x000045FD
		public override int MaxConsecutiveFailuresBeforeDisconnect
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00006401 File Offset: 0x00004601
		public override void OnConnected()
		{
			base.OnConnected();
			this._state = BattleServer.State.Connected;
			this._handler.OnConnected();
			this.DoLogin();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00006421 File Offset: 0x00004621
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this._handler.OnCantConnect();
			this._state = BattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnStopServer();
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000644E File Offset: 0x0000464E
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			this._handler.OnDisconnected();
			this._state = BattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnStopServer();
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000647C File Offset: 0x0000467C
		private void OnNewPlayerMessage(NewPlayerMessage message)
		{
			if (this._battleBecomeReady)
			{
				PlayerBattleInfo playerBattleInfo = message.PlayerBattleInfo;
				PlayerData playerData = message.PlayerData;
				this.ProcessNewPlayer(playerBattleInfo, playerData, message.PlayerParty, message.UsedCosmetics);
				return;
			}
			this._newPlayerRequests.Enqueue(message);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000064C0 File Offset: 0x000046C0
		private void ProcessNewPlayer(PlayerBattleInfo playerBattleInfo, PlayerData playerData, Guid playerParty, Dictionary<string, List<string>> usedCosmetics)
		{
			string name = playerBattleInfo.Name;
			PlayerId playerId = playerBattleInfo.PlayerId;
			int teamNo = playerBattleInfo.TeamNo;
			this._playerPartyMap[playerData.PlayerId] = playerParty;
			BattlePeer battlePeer = this.GetPeer(playerId);
			if (battlePeer == null)
			{
				battlePeer = new BattlePeer(name, playerData, usedCosmetics, teamNo, playerBattleInfo.JoinType, playerBattleInfo.IsSpectator);
				this._peers.Add(battlePeer);
			}
			else
			{
				battlePeer.Rejoin(teamNo);
			}
			if (!this._playerSpawnCounts.ContainsKey(playerId))
			{
				this._playerSpawnCounts.Add(playerId, 0);
			}
			this._handler.OnNewPlayer(battlePeer);
			IBadgeComponent badgeComponent = this._badgeComponent;
			if (badgeComponent != null)
			{
				badgeComponent.OnPlayerJoin(playerData);
			}
			PlayerBattleServerInformation playerBattleServerInformation = new PlayerBattleServerInformation(battlePeer.Index, battlePeer.SessionKey);
			base.SendMessage(new NewPlayerResponseMessage(playerId, playerBattleServerInformation));
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00006585 File Offset: 0x00004785
		public void BeginEndMission()
		{
			this._state = BattleServer.State.Finishing;
			base.SendMessage(new BattleEndingMessage());
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000659C File Offset: 0x0000479C
		public void EndMission(BattleResult battleResult, GameLog[] gameLogs, int gameTime, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this._state = BattleServer.State.Finished;
			this.SetBattleJoinTypes(battleResult);
			IBadgeComponent badgeComponent = this._badgeComponent;
			base.SendMessage(new BattleEndedMessage(battleResult, gameLogs, (badgeComponent != null) ? badgeComponent.DataDictionary : null, gameTime, teamScores, playerScores));
			if (this._handler != null)
			{
				this._handler.OnEndMission();
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000065EE File Offset: 0x000047EE
		public void BattleCancelledForPlayerLeaving(PlayerId leaverID)
		{
			base.SendMessage(new BattleCancelledDueToPlayerQuitMessage(leaverID, this.GameType));
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00006604 File Offset: 0x00004804
		public void BattleStarted(BattleResult battleResult)
		{
			if (this._shouldReportActivities)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (KeyValuePair<string, BattlePlayerEntry> keyValuePair in battleResult.PlayerEntries)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value.TeamNo);
				}
				base.SendMessage(new BattleStartedMessage(true, dictionary));
				return;
			}
			base.SendMessage(new BattleStartedMessage(false));
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00006694 File Offset: 0x00004894
		public void UpdateBattleStats(BattleResult battleResult, Dictionary<int, int> teamScores)
		{
			if (this._shouldReportActivities)
			{
				this._latestQueuedBattleResult = battleResult;
				this._latestQueuedTeamScores = teamScores;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000066AC File Offset: 0x000048AC
		private void Shutdown()
		{
			this._state = BattleServer.State.Finished;
			base.BeginDisconnect();
			this._handler.OnDisconnected();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000066C8 File Offset: 0x000048C8
		private void OnStartBattleMessage(StartBattleMessage message)
		{
			this.BattleId = message.BattleId;
			this.SceneName = message.SceneName;
			this.Faction1 = message.Faction1;
			this.Faction2 = message.Faction2;
			this.GameType = message.GameType;
			this.MinRequiredPlayerCountToStartBattle = message.MinRequiredPlayerCountToStartBattle;
			this.BattleSize = message.BattleSize;
			this.RoundThreshold = message.RoundThreshold;
			this.MoraleThreshold = message.MoraleThreshold;
			this.UseAnalytics = message.UseAnalytics;
			this.CaptureMovementData = message.CaptureMovementData;
			this.AnalyticsServiceAddress = message.AnalyticsServiceAddress;
			this.IsPremadeGame = message.IsPremadeGame;
			this.PremadeGameType = message.PremadeGameType;
			this.AssignedPlayers = message.AssignedPlayers;
			this._maxFriendlyKillCount = message.MaxFriendlyKillCount;
			this._maxFriendlyDamage = message.MaxFriendlyDamage;
			this._maxFriendlyDamagePerSingleRound = message.MaxFriendlyDamagePerSingleRound;
			this._roundFriendlyDamageLimit = message.RoundFriendlyDamageLimit;
			this._maxRoundsOverLimitCount = message.MaxRoundsOverLimitCount;
			this._handler.OnStartGame(this.SceneName, this.GameType, this.Faction1, this.Faction2, this.MinRequiredPlayerCountToStartBattle, this.BattleSize, message.ProfanityList, message.AllowList);
			this._state = BattleServer.State.BattleAssigned;
			base.SendMessage(new BattleInitializedMessage(this.GameType, this.AssignedPlayers.ToList<PlayerId>(), this.Faction2, this.Faction1));
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00006830 File Offset: 0x00004A30
		private void OnPlayerFledBattleMessage(PlayerFledBattleMessage message)
		{
			if (this._state == BattleServer.State.Finished)
			{
				return;
			}
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				battlePeer.Flee();
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, true);
				int num;
				bool flag = !this._isWarmupEnded || this._state == BattleServer.State.Finishing || !this._playerSpawnCounts.TryGetValue(playerId, out num) || num <= 0;
				base.SendMessage(new PlayerFledBattleAnswerMessage(playerId, battleResult, flag));
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000068D4 File Offset: 0x00004AD4
		private void OnPlayerDisconnectedFromLobbyMessage(PlayerDisconnectedFromLobbyMessage message)
		{
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, false);
				battlePeer.SetPlayerDisconnectdFromLobby();
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00006928 File Offset: 0x00004B28
		private void OnFriendlyDamageKickPlayerResponseMessage(FriendlyDamageKickPlayerResponseMessage message)
		{
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, false);
				battlePeer.SetPlayerKickedDueToFriendlyDamage();
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000697C File Offset: 0x00004B7C
		private void OnTerminateOperationMatchmakingMessage(TerminateOperationMatchmakingMessage message)
		{
			Random random = new Random();
			this._terminationTime = new DateTime?(DateTime.UtcNow.AddMilliseconds((double)random.Next(3000, 10000)));
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000069B8 File Offset: 0x00004BB8
		public void DoNotAcceptNewPlayers()
		{
			base.SendMessage(new StopAcceptingNewPlayersMessage());
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000069C5 File Offset: 0x00004BC5
		public void OnWarmupEnded()
		{
			this._isWarmupEnded = true;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000069D0 File Offset: 0x00004BD0
		public void OnPlayerSpawned(PlayerId playerId)
		{
			int num;
			if (!this._playerSpawnCounts.TryGetValue(playerId, out num))
			{
				num = 0;
			}
			this._playerSpawnCounts[playerId] = num + 1;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00006A00 File Offset: 0x00004C00
		public BattlePeer GetPeer(string name)
		{
			return this._peers.First<BattlePeer>((BattlePeer peer) => peer.Name == name);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00006A34 File Offset: 0x00004C34
		public BattlePeer GetPeer(PlayerId playerId)
		{
			return this._peers.FirstOrDefault<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00006A68 File Offset: 0x00004C68
		public Guid GetPlayerParty(PlayerId playerId)
		{
			Guid empty;
			if (!this._playerPartyMap.TryGetValue(playerId, out empty))
			{
				empty = Guid.Empty;
			}
			return empty;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00006A8C File Offset: 0x00004C8C
		public void HandlePlayerDisconnect(PlayerId playerId, DisconnectType disconnectType, BattleResult battleResult)
		{
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				battlePeer.SetPlayerDisconnectdFromGameSession();
				int num;
				bool flag = !this._isWarmupEnded || this._state == BattleServer.State.Finishing || !this._playerSpawnCounts.TryGetValue(playerId, out num) || num <= 0;
				base.SendMessage(new PlayerDisconnectedMessage(playerId, disconnectType, flag, battleResult));
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00006B14 File Offset: 0x00004D14
		public async void InformGameServerReady()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<BattleReadyResponseMessage>(new BattleReadyMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			BattleReadyResponseMessage battleReadyResponseMessage = taskAwaiter.GetResult().Result as BattleReadyResponseMessage;
			this._shouldReportActivities = battleReadyResponseMessage != null && battleReadyResponseMessage.ShouldReportActivities;
			this._state = BattleServer.State.Running;
			this._battleBecomeReady = true;
			while (this._newPlayerRequests.Count > 0)
			{
				NewPlayerMessage newPlayerMessage = this._newPlayerRequests.Dequeue();
				this.ProcessNewPlayer(newPlayerMessage.PlayerBattleInfo, newPlayerMessage.PlayerData, newPlayerMessage.PlayerParty, newPlayerMessage.UsedCosmetics);
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00006B50 File Offset: 0x00004D50
		private async void UpdateMaxAllowedPriority()
		{
			this._passedTimeSinceLastMaxAllowedPriorityRequest = 0f;
			sbyte b = await this.GetMaxAllowedPriority();
			this._maxAllowedPriority = b;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00006B8C File Offset: 0x00004D8C
		public void OnFriendlyHit(int round, PlayerId hitter, PlayerId victim, float damage)
		{
			if (!this._isWarmupEnded || damage <= 0f || round < 0)
			{
				return;
			}
			Dictionary<int, ValueTuple<int, float>> dictionary;
			if (!this._playerRoundFriendlyDamageMap.TryGetValue(hitter, out dictionary))
			{
				dictionary = new Dictionary<int, ValueTuple<int, float>>();
				this._playerRoundFriendlyDamageMap.Add(hitter, dictionary);
			}
			ValueTuple<int, float> valueTuple;
			if (dictionary.TryGetValue(round, out valueTuple))
			{
				dictionary[round] = new ValueTuple<int, float>(valueTuple.Item1, valueTuple.Item2 + damage);
			}
			else
			{
				dictionary.Add(round, new ValueTuple<int, float>(0, damage));
			}
			float num = 0f;
			int num2 = 0;
			bool flag = false;
			foreach (KeyValuePair<int, ValueTuple<int, float>> keyValuePair in dictionary)
			{
				num += keyValuePair.Value.Item2;
				if (num > this._maxFriendlyDamage || keyValuePair.Value.Item2 > this._maxFriendlyDamagePerSingleRound)
				{
					flag = true;
					break;
				}
				if (keyValuePair.Value.Item2 > this._roundFriendlyDamageLimit)
				{
					num2++;
					if (num2 > this._maxRoundsOverLimitCount)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				base.SendMessage(new FriendlyDamageKickPlayerMessage(hitter, dictionary));
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00006CB8 File Offset: 0x00004EB8
		public void OnFriendlyKill(int round, PlayerId killer, PlayerId victim)
		{
			if (!this._isWarmupEnded || round < 0)
			{
				return;
			}
			Dictionary<int, ValueTuple<int, float>> dictionary;
			if (!this._playerRoundFriendlyDamageMap.TryGetValue(killer, out dictionary))
			{
				dictionary = new Dictionary<int, ValueTuple<int, float>>();
				this._playerRoundFriendlyDamageMap.Add(killer, dictionary);
			}
			ValueTuple<int, float> valueTuple;
			if (dictionary.TryGetValue(round, out valueTuple))
			{
				dictionary[round] = new ValueTuple<int, float>(valueTuple.Item1 + 1, valueTuple.Item2);
			}
			else
			{
				dictionary.Add(round, new ValueTuple<int, float>(1, 0f));
			}
			int num = 0;
			foreach (KeyValuePair<int, ValueTuple<int, float>> keyValuePair in dictionary)
			{
				num += keyValuePair.Value.Item1;
				if (num > this._maxFriendlyKillCount)
				{
					base.SendMessage(new FriendlyDamageKickPlayerMessage(killer, dictionary));
					break;
				}
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00006D94 File Offset: 0x00004F94
		private async Task<sbyte> GetMaxAllowedPriority()
		{
			sbyte b;
			try
			{
				TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<RequestMaxAllowedPriorityResponse>(new RequestMaxAllowedPriorityMessage()).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CallResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CallResult>);
				}
				RequestMaxAllowedPriorityResponse requestMaxAllowedPriorityResponse = taskAwaiter.GetResult().Result as RequestMaxAllowedPriorityResponse;
				b = ((requestMaxAllowedPriorityResponse != null) ? requestMaxAllowedPriorityResponse.Priority : sbyte.MaxValue);
			}
			catch (Exception)
			{
				b = sbyte.MaxValue;
			}
			return b;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00006DDC File Offset: 0x00004FDC
		private void SetBattleJoinTypes(BattleResult battleResult)
		{
			foreach (BattlePlayerEntry battlePlayerEntry in battleResult.PlayerEntries.Values)
			{
				foreach (BattlePeer battlePeer in this._peers)
				{
					if (battlePeer.PlayerId == battlePlayerEntry.PlayerId)
					{
						battlePlayerEntry.BattleJoinType = battlePeer.BattleJoinType;
						break;
					}
				}
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00006E8C File Offset: 0x0000508C
		public bool AllPlayersConnected()
		{
			PlayerId[] assignedPlayers = this.AssignedPlayers;
			for (int i = 0; i < assignedPlayers.Length; i++)
			{
				PlayerId playerId = assignedPlayers[i];
				if (this._peers.FirstOrDefault<BattlePeer>((BattlePeer p) => p.PlayerId == playerId) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040001BB RID: 443
		private BattleServer.State _state = BattleServer.State.Connecting;

		// Token: 0x040001CB RID: 459
		private IBattleServerSessionHandler _handler;

		// Token: 0x040001CC RID: 460
		private List<BattlePeer> _peers;

		// Token: 0x040001CD RID: 461
		private string _assignedAddress;

		// Token: 0x040001CE RID: 462
		private ushort _assignedPort;

		// Token: 0x040001CF RID: 463
		private string _region;

		// Token: 0x040001D0 RID: 464
		private sbyte _priority;

		// Token: 0x040001D1 RID: 465
		private sbyte _maxAllowedPriority;

		// Token: 0x040001D2 RID: 466
		private byte _numCores;

		// Token: 0x040001D3 RID: 467
		private string _password;

		// Token: 0x040001D4 RID: 468
		private string _gameMode;

		// Token: 0x040001D5 RID: 469
		private PeerId _peerId;

		// Token: 0x040001D6 RID: 470
		private float _requestMaxAllowedPriorityIntervalInSeconds = 10f;

		// Token: 0x040001D7 RID: 471
		private float _passedTimeSinceLastMaxAllowedPriorityRequest;

		// Token: 0x040001D8 RID: 472
		private Stopwatch _timer;

		// Token: 0x040001D9 RID: 473
		private long _previousTimeInMS;

		// Token: 0x040001DA RID: 474
		private Queue<NewPlayerMessage> _newPlayerRequests;

		// Token: 0x040001DB RID: 475
		private bool _battleBecomeReady;

		// Token: 0x040001DC RID: 476
		private int _defaultServerTimeoutDuration = 600000;

		// Token: 0x040001DD RID: 477
		private int _timeoutDuration;

		// Token: 0x040001DE RID: 478
		private Stopwatch _timeoutTimer;

		// Token: 0x040001DF RID: 479
		private DateTime? _terminationTime;

		// Token: 0x040001E0 RID: 480
		private bool _isWarmupEnded;

		// Token: 0x040001E1 RID: 481
		private Dictionary<PlayerId, int> _playerSpawnCounts;

		// Token: 0x040001E2 RID: 482
		private IBadgeComponent _badgeComponent;

		// Token: 0x040001E3 RID: 483
		private Dictionary<PlayerId, Guid> _playerPartyMap;

		// Token: 0x040001E4 RID: 484
		[TupleElementNames(new string[] { "killCount", "damage" })]
		private Dictionary<PlayerId, Dictionary<int, ValueTuple<int, float>>> _playerRoundFriendlyDamageMap;

		// Token: 0x040001E5 RID: 485
		private int _maxFriendlyKillCount;

		// Token: 0x040001E6 RID: 486
		private float _maxFriendlyDamage;

		// Token: 0x040001E7 RID: 487
		private float _maxFriendlyDamagePerSingleRound;

		// Token: 0x040001E8 RID: 488
		private float _roundFriendlyDamageLimit;

		// Token: 0x040001E9 RID: 489
		private int _maxRoundsOverLimitCount;

		// Token: 0x040001EA RID: 490
		private bool _shouldReportActivities;

		// Token: 0x040001EB RID: 491
		private const float BattleResultUpdatePeriod = 5f;

		// Token: 0x040001EC RID: 492
		private float _battleResultUpdateTimeElapsed;

		// Token: 0x040001ED RID: 493
		private BattleResult _latestQueuedBattleResult;

		// Token: 0x040001EE RID: 494
		private Dictionary<int, int> _latestQueuedTeamScores;

		// Token: 0x02000186 RID: 390
		private enum State
		{
			// Token: 0x0400056C RID: 1388
			Idle,
			// Token: 0x0400056D RID: 1389
			Connecting,
			// Token: 0x0400056E RID: 1390
			Connected,
			// Token: 0x0400056F RID: 1391
			LoggingIn,
			// Token: 0x04000570 RID: 1392
			WaitingBattle,
			// Token: 0x04000571 RID: 1393
			BattleAssigned,
			// Token: 0x04000572 RID: 1394
			Running,
			// Token: 0x04000573 RID: 1395
			Finishing,
			// Token: 0x04000574 RID: 1396
			Finished
		}
	}
}
