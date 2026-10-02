using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.BoardGames.Pawns;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x0200010D RID: 269
	public class TreeNodeTablut
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x000615DF File Offset: 0x0005F7DF
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x000615E7 File Offset: 0x0005F7E7
		public Move OpeningMove { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x000615F0 File Offset: 0x0005F7F0
		private bool IsLeaf
		{
			get
			{
				return this._children == null;
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000615FB File Offset: 0x0005F7FB
		public TreeNodeTablut(BoardGameSide lastTurnIsPlayedBy, int depth)
		{
			this._lastTurnIsPlayedBy = lastTurnIsPlayedBy;
			this._depth = depth;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00061611 File Offset: 0x0005F811
		public static TreeNodeTablut CreateTreeAndReturnRootNode(BoardGameTablut.BoardInformation initialBoardState, int maxDepth)
		{
			TreeNodeTablut.MaxDepth = maxDepth;
			return new TreeNodeTablut(BoardGameSide.Player, 0)
			{
				_boardState = initialBoardState
			};
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00061628 File Offset: 0x0005F828
		public TreeNodeTablut GetChildWithBestScore()
		{
			TreeNodeTablut treeNodeTablut = null;
			if (!this.IsLeaf)
			{
				float num = float.MinValue;
				foreach (TreeNodeTablut treeNodeTablut2 in this._children)
				{
					if (treeNodeTablut2._visits > 0)
					{
						float num2 = (float)treeNodeTablut2._wins / (float)treeNodeTablut2._visits;
						if (!treeNodeTablut2.IsLeaf)
						{
							float num3 = 0f;
							foreach (TreeNodeTablut treeNodeTablut3 in treeNodeTablut2._children)
							{
								if (treeNodeTablut3._visits > 0)
								{
									float num4 = (float)treeNodeTablut3._wins / (float)treeNodeTablut3._visits;
									if (num4 > num3)
									{
										num3 = num4;
									}
								}
							}
							num2 *= 1f - num3;
						}
						if (num2 > num)
						{
							treeNodeTablut = treeNodeTablut2;
							num = num2;
						}
					}
				}
			}
			return treeNodeTablut;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00061738 File Offset: 0x0005F938
		public void SelectAction()
		{
			TreeNodeTablut treeNodeTablut = this;
			while (!treeNodeTablut.IsLeaf)
			{
				treeNodeTablut = treeNodeTablut.Select();
			}
			TreeNodeTablut.ExpandResult expandResult = treeNodeTablut.Expand();
			BoardGameSide boardGameSide = BoardGameSide.None;
			bool flag = false;
			if (expandResult == TreeNodeTablut.ExpandResult.NeedsToBeSimulated)
			{
				if (!treeNodeTablut.IsLeaf)
				{
					treeNodeTablut = treeNodeTablut.Select();
				}
				TreeNodeTablut.SimulationResult simulationResult = treeNodeTablut.Simulate();
				if (simulationResult.EndState != BoardGameTablut.State.Aborted)
				{
					boardGameSide = ((simulationResult.EndState == BoardGameTablut.State.AIWon) ? BoardGameSide.AI : BoardGameSide.Player);
					treeNodeTablut.BackPropagate(boardGameSide);
					flag = simulationResult.TurnsNeededToReachEndState <= 1;
				}
			}
			else if (expandResult != TreeNodeTablut.ExpandResult.Aborted)
			{
				boardGameSide = ((expandResult == TreeNodeTablut.ExpandResult.AIWon) ? BoardGameSide.AI : BoardGameSide.Player);
				treeNodeTablut.BackPropagate(boardGameSide);
				flag = true;
			}
			if (flag)
			{
				this.PruneSiblings(treeNodeTablut, boardGameSide);
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000617D0 File Offset: 0x0005F9D0
		private void PruneSiblings(TreeNodeTablut node, BoardGameSide winner)
		{
			if (node._parent != null && winner == node._lastTurnIsPlayedBy)
			{
				int count = node._parent._children.Count;
				if (count > 1)
				{
					int num = 0;
					int num2 = 0;
					for (int i = count - 1; i >= 0; i--)
					{
						if (node._parent._children[i] != node)
						{
							num += node._parent._children[i]._wins;
							num2 += node._parent._children[i]._visits;
							node._parent._children.RemoveAt(i);
						}
					}
					int num3 = num2 - num;
					for (TreeNodeTablut treeNodeTablut = node._parent; treeNodeTablut != null; treeNodeTablut = treeNodeTablut._parent)
					{
						if (treeNodeTablut._lastTurnIsPlayedBy == winner)
						{
							treeNodeTablut._wins -= num;
						}
						else
						{
							treeNodeTablut._wins -= num3;
						}
						treeNodeTablut._visits -= num2;
					}
				}
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000618D4 File Offset: 0x0005FAD4
		private TreeNodeTablut Select()
		{
			double num = double.MinValue;
			TreeNodeTablut treeNodeTablut = null;
			foreach (TreeNodeTablut treeNodeTablut2 in this._children)
			{
				if (treeNodeTablut2._visits == 0)
				{
					treeNodeTablut = treeNodeTablut2;
					break;
				}
				double num2 = (double)treeNodeTablut2._wins / (double)treeNodeTablut2._visits + (double)(1.5f * MathF.Sqrt(MathF.Log((float)this._visits) / (float)treeNodeTablut2._visits));
				if (num2 > num)
				{
					treeNodeTablut = treeNodeTablut2;
					num = num2;
				}
			}
			if (treeNodeTablut._boardState.PawnInformation == null)
			{
				BoardGameAITablut.Board.UndoMove(ref treeNodeTablut._parent._boardState);
				BoardGameAITablut.Board.AIMakeMove(treeNodeTablut.OpeningMove);
				treeNodeTablut._boardState = BoardGameAITablut.Board.TakeBoardSnapshot();
			}
			return treeNodeTablut;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000619B8 File Offset: 0x0005FBB8
		private TreeNodeTablut.ExpandResult Expand()
		{
			TreeNodeTablut.ExpandResult expandResult = TreeNodeTablut.ExpandResult.NeedsToBeSimulated;
			if (this._depth < TreeNodeTablut.MaxDepth)
			{
				BoardGameAITablut.Board.UndoMove(ref this._boardState);
				BoardGameTablut.State state = BoardGameAITablut.Board.CheckGameState();
				if (state == BoardGameTablut.State.InProgress)
				{
					BoardGameSide boardGameSide = ((this._lastTurnIsPlayedBy == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
					Move winningMoveIfPresent = BoardGameAITablut.Board.GetWinningMoveIfPresent(boardGameSide);
					if (winningMoveIfPresent.IsValid)
					{
						TreeNodeTablut treeNodeTablut = new TreeNodeTablut(boardGameSide, this._depth + 1);
						treeNodeTablut.OpeningMove = winningMoveIfPresent;
						treeNodeTablut._parent = this;
						this._children = new List<TreeNodeTablut>(1);
						this._children.Add(treeNodeTablut);
					}
					else
					{
						List<List<Move>> list = BoardGameAITablut.Board.CalculateAllValidMoves(boardGameSide);
						int totalMovesAvailable = BoardGameAITablut.Board.GetTotalMovesAvailable(ref list);
						if (totalMovesAvailable > 0)
						{
							this._children = new List<TreeNodeTablut>(totalMovesAvailable);
							using (List<List<Move>>.Enumerator enumerator = list.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									List<Move> list2 = enumerator.Current;
									foreach (Move move in list2)
									{
										TreeNodeTablut treeNodeTablut2 = new TreeNodeTablut(boardGameSide, this._depth + 1);
										treeNodeTablut2.OpeningMove = move;
										treeNodeTablut2._parent = this;
										this._children.Add(treeNodeTablut2);
									}
								}
								return expandResult;
							}
						}
						Debug.FailedAssert("No available moves left but the game is in progress", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\AI\\TreeNodeTablut.cs", "Expand", 396);
					}
				}
				else if (state == BoardGameTablut.State.Aborted)
				{
					expandResult = TreeNodeTablut.ExpandResult.Aborted;
				}
				else if (state == BoardGameTablut.State.AIWon)
				{
					expandResult = TreeNodeTablut.ExpandResult.AIWon;
				}
				else
				{
					expandResult = TreeNodeTablut.ExpandResult.PlayerWon;
				}
			}
			return expandResult;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00061B58 File Offset: 0x0005FD58
		private TreeNodeTablut.SimulationResult Simulate()
		{
			BoardGameAITablut.Board.UndoMove(ref this._boardState);
			BoardGameTablut.State state = BoardGameAITablut.Board.CheckGameState();
			BoardGameSide boardGameSide = ((this._lastTurnIsPlayedBy == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
			int num = 0;
			while (state == BoardGameTablut.State.InProgress)
			{
				Move move = BoardGameAITablut.Board.GetWinningMoveIfPresent(boardGameSide);
				if (!move.IsValid)
				{
					List<PawnBase> list = ((boardGameSide == BoardGameSide.Player) ? BoardGameAITablut.Board.PlayerOneUnits : BoardGameAITablut.Board.PlayerTwoUnits);
					int count = list.Count;
					int num2 = 3;
					PawnBase pawnBase;
					bool flag;
					do
					{
						pawnBase = list[MBRandom.RandomInt(count)];
						flag = BoardGameAITablut.Board.HasAvailableMoves(pawnBase as PawnTablut);
						num2--;
					}
					while (!flag && num2 > 0);
					if (!flag)
					{
						pawnBase = list.OrderBy<PawnBase, int>((PawnBase x) => MBRandom.RandomInt()).FirstOrDefault<PawnBase>((PawnBase x) => BoardGameAITablut.Board.HasAvailableMoves(x as PawnTablut));
						flag = pawnBase != null;
					}
					if (flag)
					{
						move = BoardGameAITablut.Board.GetRandomAvailableMove(pawnBase as PawnTablut);
					}
				}
				if (move.IsValid)
				{
					BoardGameAITablut.Board.AIMakeMove(move);
					state = BoardGameAITablut.Board.CheckGameState();
				}
				else if (boardGameSide == BoardGameSide.Player)
				{
					state = BoardGameTablut.State.AIWon;
				}
				else
				{
					state = BoardGameTablut.State.PlayerWon;
				}
				boardGameSide = ((boardGameSide == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
				num++;
			}
			return new TreeNodeTablut.SimulationResult(state, num);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00061CC0 File Offset: 0x0005FEC0
		private void BackPropagate(BoardGameSide winner)
		{
			for (TreeNodeTablut treeNodeTablut = this; treeNodeTablut != null; treeNodeTablut = treeNodeTablut._parent)
			{
				treeNodeTablut._visits++;
				if (winner == treeNodeTablut._lastTurnIsPlayedBy)
				{
					treeNodeTablut._wins++;
				}
			}
		}

		// Token: 0x040005BE RID: 1470
		private const float UCTConstant = 1.5f;

		// Token: 0x040005BF RID: 1471
		private static int MaxDepth;

		// Token: 0x040005C0 RID: 1472
		private readonly int _depth;

		// Token: 0x040005C1 RID: 1473
		private BoardGameTablut.BoardInformation _boardState;

		// Token: 0x040005C2 RID: 1474
		private TreeNodeTablut _parent;

		// Token: 0x040005C3 RID: 1475
		private List<TreeNodeTablut> _children;

		// Token: 0x040005C4 RID: 1476
		private BoardGameSide _lastTurnIsPlayedBy;

		// Token: 0x040005C5 RID: 1477
		private int _visits;

		// Token: 0x040005C6 RID: 1478
		private int _wins;

		// Token: 0x0200023A RID: 570
		private struct SimulationResult
		{
			// Token: 0x06001479 RID: 5241 RVA: 0x0007A67D File Offset: 0x0007887D
			public SimulationResult(BoardGameTablut.State s, int turns)
			{
				this.EndState = s;
				this.TurnsNeededToReachEndState = turns;
			}

			// Token: 0x040009EA RID: 2538
			public readonly BoardGameTablut.State EndState;

			// Token: 0x040009EB RID: 2539
			public readonly int TurnsNeededToReachEndState;
		}

		// Token: 0x0200023B RID: 571
		private enum ExpandResult
		{
			// Token: 0x040009ED RID: 2541
			NeedsToBeSimulated,
			// Token: 0x040009EE RID: 2542
			AIWon,
			// Token: 0x040009EF RID: 2543
			PlayerWon,
			// Token: 0x040009F0 RID: 2544
			Aborted
		}
	}
}
