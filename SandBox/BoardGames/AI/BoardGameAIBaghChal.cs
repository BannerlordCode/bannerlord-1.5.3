using System;
using System.Collections.Generic;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000106 RID: 262
	public class BoardGameAIBaghChal : BoardGameAIBase
	{
		// Token: 0x06000D24 RID: 3364 RVA: 0x0005F8DC File Offset: 0x0005DADC
		public BoardGameAIBaghChal(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
			this._board = base.BoardGameHandler.Board as BoardGameBaghChal;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0005F8FC File Offset: 0x0005DAFC
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 4;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 5;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0005F93C File Offset: 0x0005DB3C
		public override Move CalculateMovementStageMove()
		{
			Move move;
			move.GoalTile = null;
			move.Unit = null;
			if (this._board.IsReady)
			{
				List<List<Move>> list = this._board.CalculateAllValidMoves(BoardGameSide.AI);
				BoardGameBaghChal.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
				if (this._board.HasMovesAvailable(ref list))
				{
					int num = int.MinValue;
					foreach (List<Move> list2 in list)
					{
						if (base.AbortRequested)
						{
							break;
						}
						foreach (Move move2 in list2)
						{
							if (base.AbortRequested)
							{
								break;
							}
							this._board.AIMakeMove(move2);
							int num2 = -this.NegaMax(this.MaxDepth, -1, -2147483647, int.MaxValue);
							this._board.UndoMove(ref boardInformation);
							if (num2 > num)
							{
								move = move2;
								num = num2;
							}
						}
					}
				}
			}
			if (!base.AbortRequested)
			{
				bool isValid = move.IsValid;
			}
			return move;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0005FA78 File Offset: 0x0005DC78
		public override Move CalculatePreMovementStageMove()
		{
			return this.CalculateMovementStageMove();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0005FA80 File Offset: 0x0005DC80
		private int NegaMax(int depth, int color, int alpha, int beta)
		{
			if (depth == 0)
			{
				return color * this.Evaluation() * ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? 1 : (-1));
			}
			BoardGameBaghChal.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
			if (color == ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? (-1) : 1) && this._board.GetANonePlacedGoat() != null)
			{
				for (int i = 0; i < this._board.TileCount; i++)
				{
					TileBase tileBase = this._board.Tiles[i];
					if (tileBase.PawnOnTile == null)
					{
						Move move = new Move(this._board.GetANonePlacedGoat(), tileBase);
						this._board.AIMakeMove(move);
						int num = -this.NegaMax(depth - 1, -color, -beta, -alpha);
						this._board.UndoMove(ref boardInformation);
						if (num >= beta)
						{
							return num;
						}
						alpha = MathF.Max(num, alpha);
					}
				}
			}
			else
			{
				List<List<Move>> list = this._board.CalculateAllValidMoves((color == 1) ? BoardGameSide.AI : BoardGameSide.Player);
				if (!this._board.HasMovesAvailable(ref list))
				{
					return color * this.Evaluation() * ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? 1 : (-1));
				}
				foreach (List<Move> list2 in list)
				{
					foreach (Move move2 in list2)
					{
						this._board.AIMakeMove(move2);
						int num2 = -this.NegaMax(depth - 1, -color, -beta, -alpha);
						this._board.UndoMove(ref boardInformation);
						if (num2 >= beta)
						{
							return num2;
						}
						alpha = MathF.Max(num2, alpha);
					}
				}
				return alpha;
			}
			return alpha;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0005FC5C File Offset: 0x0005DE5C
		private int Evaluation()
		{
			float num = MBRandom.RandomFloat;
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				num = num * 0.7f + 0.5f;
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				num = num * 0.5f + 0.65f;
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				num = num * 0.35f + 0.75f;
				break;
			}
			List<List<Move>> list = this._board.CalculateAllValidMoves((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? BoardGameSide.AI : BoardGameSide.Player);
			int totalMovesAvailable = this._board.GetTotalMovesAvailable(ref list);
			return (int)((float)(100 * -(float)this.GetTigersStuck() + 50 * this.GetGoatsCaptured() + totalMovesAvailable + this.GetCombinedDistanceBetweenTigers()) * num);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0005FD04 File Offset: 0x0005DF04
		private int GetTigersStuck()
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				if (this._board.CalculateValidMoves(pawnBaghChal).Count == 0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005FD90 File Offset: 0x0005DF90
		private int GetGoatsCaptured()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((PawnBaghChal)enumerator.Current).Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0005FE10 File Offset: 0x0005E010
		private int GetCombinedDistanceBetweenTigers()
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				foreach (PawnBase pawnBase2 in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
				{
					PawnBaghChal pawnBaghChal2 = (PawnBaghChal)pawnBase2;
					if (pawnBaghChal != pawnBaghChal2)
					{
						num += MathF.Abs(pawnBaghChal.X - pawnBaghChal2.X) + MathF.Abs(pawnBaghChal.Y + pawnBaghChal2.Y);
					}
				}
			}
			return num;
		}

		// Token: 0x040005AB RID: 1451
		private readonly BoardGameBaghChal _board;
	}
}
