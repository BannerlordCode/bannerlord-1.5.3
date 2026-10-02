using System;
using Helpers;
using SandBox.BoardGames.MissionLogics;

namespace SandBox.BoardGames.AI
{
	// Token: 0x0200010C RID: 268
	public class BoardGameAITablut : BoardGameAIBase
	{
		// Token: 0x06000D65 RID: 3429 RVA: 0x000614AE File Offset: 0x0005F6AE
		public BoardGameAITablut(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000614B8 File Offset: 0x0005F6B8
		public override void Initialize()
		{
			base.Initialize();
			BoardGameAITablut.Board = base.BoardGameHandler.Board as BoardGameTablut;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000614D5 File Offset: 0x0005F6D5
		public override void OnSetGameOver()
		{
			base.OnSetGameOver();
			BoardGameAITablut.Board = null;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x000614E4 File Offset: 0x0005F6E4
		public override Move CalculateMovementStageMove()
		{
			Move openingMove;
			openingMove.GoalTile = null;
			openingMove.Unit = null;
			if (BoardGameAITablut.Board.IsReady)
			{
				BoardGameTablut.BoardInformation boardInformation = BoardGameAITablut.Board.TakeBoardSnapshot();
				TreeNodeTablut treeNodeTablut = TreeNodeTablut.CreateTreeAndReturnRootNode(boardInformation, this.MaxDepth);
				int num = 0;
				while (num < this._sampleCount && !base.AbortRequested)
				{
					treeNodeTablut.SelectAction();
					num++;
				}
				if (!base.AbortRequested)
				{
					BoardGameAITablut.Board.UndoMove(ref boardInformation);
					TreeNodeTablut childWithBestScore = treeNodeTablut.GetChildWithBestScore();
					if (childWithBestScore != null)
					{
						openingMove = childWithBestScore.OpeningMove;
					}
				}
			}
			if (!base.AbortRequested)
			{
				bool isValid = openingMove.IsValid;
			}
			return openingMove;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00061580 File Offset: 0x0005F780
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				this._sampleCount = 30000;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 4;
				this._sampleCount = 47000;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 5;
				this._sampleCount = 64000;
				return;
			default:
				return;
			}
		}

		// Token: 0x040005BC RID: 1468
		public static BoardGameTablut Board;

		// Token: 0x040005BD RID: 1469
		private int _sampleCount;
	}
}
