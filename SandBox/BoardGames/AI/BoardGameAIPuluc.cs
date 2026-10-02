using System;
using System.Collections.Generic;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Pawns;

namespace SandBox.BoardGames.AI
{
	// Token: 0x0200010A RID: 266
	public class BoardGameAIPuluc : BoardGameAIBase
	{
		// Token: 0x06000D53 RID: 3411 RVA: 0x0006096E File Offset: 0x0005EB6E
		public BoardGameAIPuluc(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
			this._board = base.BoardGameHandler.Board as BoardGamePuluc;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000609A8 File Offset: 0x0005EBA8
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 5;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 7;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000609E8 File Offset: 0x0005EBE8
		public override Move CalculateMovementStageMove()
		{
			Move move;
			move.GoalTile = null;
			move.Unit = null;
			if (this._board.IsReady)
			{
				this.ExpectiMax(this.MaxDepth, BoardGameSide.AI, false, ref move);
			}
			if (!base.AbortRequested)
			{
				bool isValid = move.IsValid;
			}
			return move;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00060A34 File Offset: 0x0005EC34
		private float ExpectiMax(int depth, BoardGameSide side, bool chanceNode, ref Move bestMove)
		{
			float num;
			if (depth == 0)
			{
				num = (float)this.Evaluation();
				if (side == BoardGameSide.Player)
				{
					num = -num;
				}
			}
			else if (chanceNode)
			{
				num = 0f;
				for (int i = 0; i < 5; i++)
				{
					int lastDice = this._board.LastDice;
					this._board.ForceDice((i == 0) ? 5 : i);
					num += this._diceProbabilities[i] * this.ExpectiMax(depth - 1, side, false, ref bestMove);
					this._board.ForceDice(lastDice);
				}
			}
			else
			{
				BoardGamePuluc.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
				List<List<Move>> list = this._board.CalculateAllValidMoves(side);
				if (this._board.HasMovesAvailable(ref list))
				{
					num = float.MinValue;
					using (List<List<Move>>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							List<Move> list2 = enumerator.Current;
							if (list2 != null)
							{
								foreach (Move move in list2)
								{
									this._board.AIMakeMove(move);
									BoardGameSide boardGameSide = ((side == BoardGameSide.AI) ? BoardGameSide.Player : BoardGameSide.AI);
									float num2 = -this.ExpectiMax(depth - 1, boardGameSide, true, ref bestMove);
									this._board.UndoMove(ref boardInformation);
									if (num < num2)
									{
										num = num2;
										if (depth == this.MaxDepth)
										{
											bestMove = move;
										}
									}
								}
							}
						}
						return num;
					}
				}
				num = (float)this.Evaluation();
				if (side == BoardGameSide.Player)
				{
					num = -num;
				}
			}
			return num;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00060BCC File Offset: 0x0005EDCC
		private int Evaluation()
		{
			return 20 * (this._board.GetPlayerTwoUnitsAlive() - this._board.GetPlayerOneUnitsAlive()) + 5 * (this.GetUnitsBeingCaptured(true) - this.GetUnitsBeingCaptured(false)) + (this.GetUnitsInPlay(false) - this.GetUnitsInPlay(true));
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00060C0C File Offset: 0x0005EE0C
		private int GetUnitsInSpawn(bool playerOne)
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((PawnPuluc)enumerator.Current).IsInSpawn)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00060C80 File Offset: 0x0005EE80
		private int GetUnitsBeingCaptured(bool playerOne)
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!((PawnPuluc)enumerator.Current).IsTopPawn)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00060CF4 File Offset: 0x0005EEF4
		private int GetUnitsInPlay(bool playerOne)
		{
			int num = 0;
			foreach (PawnBase pawnBase in (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits))
			{
				PawnPuluc pawnPuluc = (PawnPuluc)pawnBase;
				if (pawnPuluc.InPlay && pawnPuluc.IsTopPawn)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040005B8 RID: 1464
		private readonly BoardGamePuluc _board;

		// Token: 0x040005B9 RID: 1465
		private readonly float[] _diceProbabilities = new float[] { 0.0625f, 0.25f, 0.375f, 0.25f, 0.0625f };
	}
}
