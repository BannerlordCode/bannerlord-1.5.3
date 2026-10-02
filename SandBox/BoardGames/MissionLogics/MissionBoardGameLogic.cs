using System;
using System.Linq;
using Helpers;
using SandBox.BoardGames.AI;
using SandBox.Conversation;
using SandBox.Conversation.MissionLogics;
using SandBox.Objects.Usables;
using SandBox.Source.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace SandBox.BoardGames.MissionLogics
{
	// Token: 0x02000105 RID: 261
	public class MissionBoardGameLogic : MissionLogic
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000CF5 RID: 3317 RVA: 0x0005EF24 File Offset: 0x0005D124
		// (remove) Token: 0x06000CF6 RID: 3318 RVA: 0x0005EF5C File Offset: 0x0005D15C
		public event Action GameStarted;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000CF7 RID: 3319 RVA: 0x0005EF94 File Offset: 0x0005D194
		// (remove) Token: 0x06000CF8 RID: 3320 RVA: 0x0005EFCC File Offset: 0x0005D1CC
		public event Action GameEnded;

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0005F001 File Offset: 0x0005D201
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0005F009 File Offset: 0x0005D209
		public BoardGameBase Board { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0005F012 File Offset: 0x0005D212
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x0005F01A File Offset: 0x0005D21A
		public BoardGameAIBase AIOpponent { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0005F023 File Offset: 0x0005D223
		public bool IsOpposingAgentMovingToPlayingChair
		{
			get
			{
				return BoardGameAgentBehavior.IsAgentMovingToChair(this.OpposingAgent);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0005F030 File Offset: 0x0005D230
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0005F038 File Offset: 0x0005D238
		public bool IsGameInProgress { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0005F041 File Offset: 0x0005D241
		public BoardGameHelper.BoardGameState BoardGameFinalState
		{
			get
			{
				return this._boardGameState;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0005F049 File Offset: 0x0005D249
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x0005F051 File Offset: 0x0005D251
		public CultureObject.BoardGameType CurrentBoardGame { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0005F05A File Offset: 0x0005D25A
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x0005F062 File Offset: 0x0005D262
		public BoardGameHelper.AIDifficulty Difficulty { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0005F06B File Offset: 0x0005D26B
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x0005F073 File Offset: 0x0005D273
		public int BetAmount { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0005F07C File Offset: 0x0005D27C
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x0005F084 File Offset: 0x0005D284
		public Agent OpposingAgent { get; private set; }

		// Token: 0x06000D09 RID: 3337 RVA: 0x0005F090 File Offset: 0x0005D290
		public override void AfterStart()
		{
			base.AfterStart();
			this._opposingChair = base.Mission.Scene.FindEntityWithTag("gambler_npc").CollectScriptComponentsIncludingChildrenRecursive<Chair>().FirstOrDefault<Chair>();
			this._playerChair = base.Mission.Scene.FindEntityWithTag("gambler_player").CollectScriptComponentsIncludingChildrenRecursive<Chair>().FirstOrDefault<Chair>();
			foreach (StandingPoint standingPoint in this._opposingChair.StandingPoints)
			{
				standingPoint.IsDisabledForPlayers = true;
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0005F138 File Offset: 0x0005D338
		public void SetStartingPlayer(bool playerOneStarts)
		{
			this._startingPlayer = (playerOneStarts ? PlayerTurn.PlayerOne : PlayerTurn.PlayerTwo);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0005F147 File Offset: 0x0005D347
		public void StartBoardGame()
		{
			this._startingBoardGame = true;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0005F150 File Offset: 0x0005D350
		private void BoardGameInit(CultureObject.BoardGameType game)
		{
			if (this.Board == null)
			{
				switch (game)
				{
				case CultureObject.BoardGameType.Seega:
					this.Board = new BoardGameSeega(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAISeega(this.Difficulty, this);
					break;
				case CultureObject.BoardGameType.Puluc:
					this.Board = new BoardGamePuluc(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAIPuluc(this.Difficulty, this);
					break;
				case CultureObject.BoardGameType.Konane:
					this.Board = new BoardGameKonane(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAIKonane(this.Difficulty, this);
					break;
				case CultureObject.BoardGameType.MuTorere:
					this.Board = new BoardGameMuTorere(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAIMuTorere(this.Difficulty, this);
					break;
				case CultureObject.BoardGameType.Tablut:
					this.Board = new BoardGameTablut(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAITablut(this.Difficulty, this);
					break;
				case CultureObject.BoardGameType.BaghChal:
					this.Board = new BoardGameBaghChal(this, this._startingPlayer);
					this.AIOpponent = new BoardGameAIBaghChal(this.Difficulty, this);
					break;
				default:
					Debug.FailedAssert("[DEBUG]No board with this name was found.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\MissionLogics\\MissionBoardGameLogic.cs", "BoardGameInit", 119);
					break;
				}
				this.Board.Initialize();
				if (this.AIOpponent != null)
				{
					this.AIOpponent.Initialize();
				}
			}
			else
			{
				this.Board.SetStartingPlayer(this._startingPlayer);
				this.Board.InitializeUnits();
				this.Board.InitializeCapturedUnitsZones();
				this.Board.Reset();
				if (this.AIOpponent != null)
				{
					this.AIOpponent.SetDifficulty(this.Difficulty);
					this.AIOpponent.Initialize();
				}
			}
			if (this.Handler != null)
			{
				this.Handler.Install();
			}
			this._boardGameState = BoardGameHelper.BoardGameState.None;
			this.IsGameInProgress = true;
			this._isTavernGame = CampaignMission.Current.Location == Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern");
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0005F34C File Offset: 0x0005D54C
		public override void OnMissionTick(float dt)
		{
			if (base.Mission.IsInPhotoMode)
			{
				return;
			}
			if (this._startingBoardGame)
			{
				this._startingBoardGame = false;
				this.BoardGameInit(this.CurrentBoardGame);
				Action gameStarted = this.GameStarted;
				if (gameStarted == null)
				{
					return;
				}
				gameStarted();
				return;
			}
			else
			{
				if (this.IsGameInProgress)
				{
					this.Board.Tick(dt);
					return;
				}
				if (this.OpposingAgent != null && this.OpposingAgent.IsHero && Hero.OneToOneConversationHero == null && this.CheckIfBothSidesAreSitting())
				{
					this.StartBoardGame();
				}
				return;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0005F3D4 File Offset: 0x0005D5D4
		public void DetectOpposingAgent()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				if (agent == ConversationMission.OneToOneConversationAgent)
				{
					this.OpposingAgent = agent;
					if (agent.IsHero)
					{
						BoardGameAgentBehavior.AddTargetChair(this.OpposingAgent, this._opposingChair);
					}
					AgentNavigator agentNavigator = this.OpposingAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
					this._specialTagCacheOfOpposingHero = agentNavigator.SpecialTargetTag;
					agentNavigator.SpecialTargetTag = "gambler_npc";
					break;
				}
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0005F478 File Offset: 0x0005D678
		public bool CheckIfBothSidesAreSitting()
		{
			return Agent.Main != null && this.OpposingAgent != null && this._playerChair.IsAgentFullySitting(Agent.Main) && this._opposingChair.IsAgentFullySitting(this.OpposingAgent);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		public void PlayerOneWon(string message = "str_boardgame_victory_message")
		{
			Agent opposingAgent = this.OpposingAgent;
			this.SetGameOver(GameOverEnum.PlayerOneWon);
			this.ShowInquiry(message, opposingAgent);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0005F4D4 File Offset: 0x0005D6D4
		public void PlayerTwoWon(string message = "str_boardgame_defeat_message")
		{
			Agent opposingAgent = this.OpposingAgent;
			this.SetGameOver(GameOverEnum.PlayerTwoWon);
			this.ShowInquiry(message, opposingAgent);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0005F4F8 File Offset: 0x0005D6F8
		public void GameWasDraw(string message = "str_boardgame_draw_message")
		{
			Agent opposingAgent = this.OpposingAgent;
			this.SetGameOver(GameOverEnum.Draw);
			this.ShowInquiry(message, opposingAgent);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0005F51C File Offset: 0x0005D71C
		private void ShowInquiry(string message, Agent conversationAgent)
		{
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_boardgame", null).ToString(), GameTexts.FindText(message, null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
			{
				this.StartConversationWithOpponentAfterGameEnd(conversationAgent);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0005F596 File Offset: 0x0005D796
		private void StartConversationWithOpponentAfterGameEnd(Agent conversationAgent)
		{
			MissionConversationLogic.Current.StartConversation(conversationAgent, false, false);
			this._boardGameState = BoardGameHelper.BoardGameState.None;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0005F5AC File Offset: 0x0005D7AC
		public void SetGameOver(GameOverEnum gameOverInfo)
		{
			base.Mission.MainAgent.ClearTargetFrame();
			if (this.Handler != null && gameOverInfo != GameOverEnum.PlayerCanceledTheGame)
			{
				this.Handler.Uninstall();
			}
			Hero hero = (this.OpposingAgent.IsHero ? ((CharacterObject)this.OpposingAgent.Character).HeroObject : null);
			switch (gameOverInfo)
			{
			case GameOverEnum.PlayerOneWon:
				this._boardGameState = BoardGameHelper.BoardGameState.Win;
				break;
			case GameOverEnum.PlayerTwoWon:
				this._boardGameState = BoardGameHelper.BoardGameState.Loss;
				break;
			case GameOverEnum.Draw:
				this._boardGameState = BoardGameHelper.BoardGameState.Draw;
				break;
			case GameOverEnum.PlayerCanceledTheGame:
				this._boardGameState = BoardGameHelper.BoardGameState.None;
				break;
			}
			if (gameOverInfo != GameOverEnum.PlayerCanceledTheGame)
			{
				CampaignEventDispatcher.Instance.OnPlayerBoardGameOver(hero, this._boardGameState);
			}
			Action gameEnded = this.GameEnded;
			if (gameEnded != null)
			{
				gameEnded();
			}
			BoardGameAgentBehavior.RemoveBoardGameBehaviorOfAgent(this.OpposingAgent);
			this.OpposingAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag = this._specialTagCacheOfOpposingHero;
			this.OpposingAgent = null;
			this.IsGameInProgress = false;
			BoardGameAIBase aiopponent = this.AIOpponent;
			if (aiopponent == null)
			{
				return;
			}
			aiopponent.OnSetGameOver();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0005F6B0 File Offset: 0x0005D8B0
		public void ForfeitGame()
		{
			this.Board.SetGameOverInfo(GameOverEnum.PlayerTwoWon);
			Agent opposingAgent = this.OpposingAgent;
			this.SetGameOver(this.Board.GameOverInfo);
			this.StartConversationWithOpponentAfterGameEnd(opposingAgent);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0005F6E8 File Offset: 0x0005D8E8
		public void AIForfeitGame()
		{
			this.Board.SetGameOverInfo(GameOverEnum.PlayerOneWon);
			this.SetGameOver(this.Board.GameOverInfo);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0005F707 File Offset: 0x0005D907
		public void RollDice()
		{
			this.Board.RollDice();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005F714 File Offset: 0x0005D914
		public bool RequiresDiceRolling()
		{
			switch (this.CurrentBoardGame)
			{
			case CultureObject.BoardGameType.Seega:
				return false;
			case CultureObject.BoardGameType.Puluc:
				return true;
			case CultureObject.BoardGameType.Konane:
				return false;
			case CultureObject.BoardGameType.MuTorere:
				return false;
			case CultureObject.BoardGameType.Tablut:
				return false;
			case CultureObject.BoardGameType.BaghChal:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005F755 File Offset: 0x0005D955
		public void SetBetAmount(int bet)
		{
			this.BetAmount = bet;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0005F75E File Offset: 0x0005D95E
		public void SetCurrentDifficulty(BoardGameHelper.AIDifficulty difficulty)
		{
			this.Difficulty = difficulty;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0005F767 File Offset: 0x0005D967
		public void SetBoardGame(CultureObject.BoardGameType game)
		{
			this.CurrentBoardGame = game;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0005F770 File Offset: 0x0005D970
		protected override void OnEndMission()
		{
			base.OnEndMission();
			if (this.IsGameInProgress)
			{
				this.SetGameOver(GameOverEnum.PlayerCanceledTheGame);
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0005F787 File Offset: 0x0005D987
		public override InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = true;
			return null;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0005F790 File Offset: 0x0005D990
		public static bool IsBoardGameAvailable()
		{
			Mission mission = Mission.Current;
			MissionBoardGameLogic missionBoardGameLogic = ((mission != null) ? mission.GetMissionBehavior<MissionBoardGameLogic>() : null);
			Mission mission2 = Mission.Current;
			return ((mission2 != null) ? mission2.Scene : null) != null && missionBoardGameLogic != null && Mission.Current.Scene.FindEntityWithTag("boardgame") != null && missionBoardGameLogic.OpposingAgent == null;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0005F7F4 File Offset: 0x0005D9F4
		public static bool IsThereActiveBoardGameWithHero(Hero hero)
		{
			Mission mission = Mission.Current;
			MissionBoardGameLogic missionBoardGameLogic = ((mission != null) ? mission.GetMissionBehavior<MissionBoardGameLogic>() : null);
			Mission mission2 = Mission.Current;
			if (((mission2 != null) ? mission2.Scene : null) != null && Mission.Current.Scene.FindEntityWithTag("boardgame") != null && missionBoardGameLogic != null)
			{
				Agent opposingAgent = missionBoardGameLogic.OpposingAgent;
				return ((opposingAgent != null) ? opposingAgent.Character : null) == hero.CharacterObject;
			}
			return false;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005F867 File Offset: 0x0005DA67
		public override void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && !Campaign.Current.ConversationManager.IsConversationInProgress && this.IsThereAgentAction(userAgent, agent))
			{
				Mission.Current.GetMissionBehavior<MissionConversationLogic>().StartConversation(agent, false, false);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0005F8A3 File Offset: 0x0005DAA3
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return userAgent.IsMainAgent && this._playerChair.IsAgentFullySitting(Agent.Main) && this._opposingChair.IsAgentFullySitting(otherAgent);
		}

		// Token: 0x04000598 RID: 1432
		private const string BoardGameEntityTag = "boardgame";

		// Token: 0x04000599 RID: 1433
		private const string SpecialTargetGamblerNpcTag = "gambler_npc";

		// Token: 0x0400059C RID: 1436
		public IBoardGameHandler Handler;

		// Token: 0x0400059D RID: 1437
		private PlayerTurn _startingPlayer = PlayerTurn.PlayerTwo;

		// Token: 0x0400059E RID: 1438
		private Chair _playerChair;

		// Token: 0x0400059F RID: 1439
		private Chair _opposingChair;

		// Token: 0x040005A0 RID: 1440
		private string _specialTagCacheOfOpposingHero;

		// Token: 0x040005A1 RID: 1441
		private bool _isTavernGame;

		// Token: 0x040005A2 RID: 1442
		private bool _startingBoardGame;

		// Token: 0x040005A3 RID: 1443
		private BoardGameHelper.BoardGameState _boardGameState;
	}
}
