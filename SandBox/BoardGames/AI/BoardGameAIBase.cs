using System;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000107 RID: 263
	public abstract class BoardGameAIBase
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0005FF18 File Offset: 0x0005E118
		public BoardGameAIBase.AIState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0005FF22 File Offset: 0x0005E122
		// (set) Token: 0x06000D2F RID: 3375 RVA: 0x0005FF2A File Offset: 0x0005E12A
		public Move RecentMoveCalculated { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0005FF33 File Offset: 0x0005E133
		public bool AbortRequested
		{
			get
			{
				return this.State == BoardGameAIBase.AIState.AbortRequested;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0005FF3E File Offset: 0x0005E13E
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x0005FF46 File Offset: 0x0005E146
		private protected BoardGameHelper.AIDifficulty Difficulty { protected get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0005FF4F File Offset: 0x0005E14F
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x0005FF57 File Offset: 0x0005E157
		private protected MissionBoardGameLogic BoardGameHandler { protected get; private set; }

		// Token: 0x06000D35 RID: 3381 RVA: 0x0005FF60 File Offset: 0x0005E160
		protected BoardGameAIBase(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
		{
			this._stateLock = new object();
			this.Difficulty = difficulty;
			this.BoardGameHandler = boardGameHandler;
			this.Initialize();
			this._aiTask = AsyncTask.CreateWithDelegate(new ManagedDelegate
			{
				Instance = new ManagedDelegate.DelegateDefinition(this.UpdateThinkingAboutMoveOnSeparateThread)
			}, true);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0005FFB7 File Offset: 0x0005E1B7
		public virtual Move CalculatePreMovementStageMove()
		{
			Debug.FailedAssert("CalculatePreMovementStageMove is not implemented for " + this.BoardGameHandler.CurrentBoardGame, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\AI\\BoardGameAIBase.cs", "CalculatePreMovementStageMove", 64);
			return Move.Invalid;
		}

		// Token: 0x06000D37 RID: 3383
		public abstract Move CalculateMovementStageMove();

		// Token: 0x06000D38 RID: 3384
		protected abstract void InitializeDifficulty();

		// Token: 0x06000D39 RID: 3385 RVA: 0x0005FFE9 File Offset: 0x0005E1E9
		public virtual bool WantsToForfeit()
		{
			return false;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0005FFEC File Offset: 0x0005E1EC
		public virtual void OnSetGameOver()
		{
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				BoardGameAIBase.AIState state = this.State;
				if (state != BoardGameAIBase.AIState.ReadyToRun)
				{
					if (state == BoardGameAIBase.AIState.Running)
					{
						this._state = BoardGameAIBase.AIState.AbortRequested;
					}
				}
				else
				{
					this._state = BoardGameAIBase.AIState.AbortRequested;
				}
			}
			this._aiTask.Wait();
			this.Reset();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0006005C File Offset: 0x0005E25C
		public virtual void Initialize()
		{
			this.Reset();
			this.InitializeDifficulty();
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0006006A File Offset: 0x0005E26A
		public void SetDifficulty(BoardGameHelper.AIDifficulty difficulty)
		{
			this.Difficulty = difficulty;
			this.InitializeDifficulty();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00060079 File Offset: 0x0005E279
		public float HowLongDidAIThinkAboutMove()
		{
			return this._aiDecisionTimer;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00060084 File Offset: 0x0005E284
		public void UpdateThinkingAboutMove(float dt)
		{
			this._aiDecisionTimer += dt;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.State == BoardGameAIBase.AIState.NeedsToRun)
				{
					this._state = BoardGameAIBase.AIState.ReadyToRun;
					this._aiTask.Invoke();
				}
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000600E8 File Offset: 0x0005E2E8
		private void UpdateThinkingAboutMoveOnSeparateThread()
		{
			if (this.BoardGameHandler.Board.InPreMovementStage)
			{
				this.CalculatePreMovementStageOnSeparateThread();
				return;
			}
			this.CalculateMovementStageMoveOnSeparateThread();
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00060109 File Offset: 0x0005E309
		public void ResetThinking()
		{
			this._aiDecisionTimer = 0f;
			this._state = BoardGameAIBase.AIState.NeedsToRun;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0006011F File Offset: 0x0005E31F
		public bool CanMakeMove()
		{
			return this.State == BoardGameAIBase.AIState.Done && this._aiDecisionTimer >= 1.5f;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0006013C File Offset: 0x0005E33C
		private void Reset()
		{
			this.RecentMoveCalculated = Move.Invalid;
			this.MayForfeit = true;
			this.ResetThinking();
			this.MaxDepth = 0;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00060160 File Offset: 0x0005E360
		private void CalculatePreMovementStageOnSeparateThread()
		{
			if (this.OnBeginSeparateThread())
			{
				Move move = this.CalculatePreMovementStageMove();
				this.OnExitSeparateThread(move);
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00060184 File Offset: 0x0005E384
		private void CalculateMovementStageMoveOnSeparateThread()
		{
			if (this.OnBeginSeparateThread())
			{
				Move move = this.CalculateMovementStageMove();
				this.OnExitSeparateThread(move);
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000601A8 File Offset: 0x0005E3A8
		private bool OnBeginSeparateThread()
		{
			bool flag = false;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.AbortRequested)
				{
					this._state = BoardGameAIBase.AIState.Aborted;
					flag = true;
				}
				else
				{
					this._state = BoardGameAIBase.AIState.Running;
				}
			}
			return !flag;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00060208 File Offset: 0x0005E408
		private void OnExitSeparateThread(Move calculatedMove)
		{
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.AbortRequested)
				{
					this._state = BoardGameAIBase.AIState.Aborted;
					this.RecentMoveCalculated = Move.Invalid;
				}
				else
				{
					this._state = BoardGameAIBase.AIState.Done;
					this.RecentMoveCalculated = calculatedMove;
				}
			}
		}

		// Token: 0x040005AC RID: 1452
		private const float AIDecisionDuration = 1.5f;

		// Token: 0x040005AD RID: 1453
		protected bool MayForfeit;

		// Token: 0x040005AE RID: 1454
		protected int MaxDepth;

		// Token: 0x040005AF RID: 1455
		private float _aiDecisionTimer;

		// Token: 0x040005B0 RID: 1456
		private readonly ITask _aiTask;

		// Token: 0x040005B1 RID: 1457
		private readonly object _stateLock;

		// Token: 0x040005B2 RID: 1458
		private volatile BoardGameAIBase.AIState _state;

		// Token: 0x02000238 RID: 568
		public enum AIState
		{
			// Token: 0x040009E2 RID: 2530
			NeedsToRun,
			// Token: 0x040009E3 RID: 2531
			ReadyToRun,
			// Token: 0x040009E4 RID: 2532
			Running,
			// Token: 0x040009E5 RID: 2533
			AbortRequested,
			// Token: 0x040009E6 RID: 2534
			Aborted,
			// Token: 0x040009E7 RID: 2535
			Done
		}
	}
}
