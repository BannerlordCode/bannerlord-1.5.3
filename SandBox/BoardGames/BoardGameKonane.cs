using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Objects;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.BoardGames
{
	// Token: 0x020000F0 RID: 240
	public class BoardGameKonane : BoardGameBase
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00057115 File Offset: 0x00055315
		public override int TileCount
		{
			get
			{
				return BoardGameKonane.BoardWidth * BoardGameKonane.BoardHeight;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00057122 File Offset: 0x00055322
		protected override bool RotateBoard
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00057125 File Offset: 0x00055325
		protected override bool PreMovementStagePresent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00057128 File Offset: 0x00055328
		protected override bool DiceRollRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0005712C File Offset: 0x0005532C
		public BoardGameKonane(MissionBoardGameLogic mission, PlayerTurn startingPlayer)
			: base(mission, new TextObject("{=5DSafcSC}Konane", null), startingPlayer)
		{
			if (base.Tiles == null)
			{
				base.Tiles = new TileBase[this.TileCount];
			}
			this.SelectedUnit = null;
			this.PawnUnselectedFactor = 4287395960U;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00057184 File Offset: 0x00055384
		public override void InitializeUnits()
		{
			base.PlayerOneUnits.Clear();
			base.PlayerTwoUnits.Clear();
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			for (int i = 0; i < 18; i++)
			{
				GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("player_one_unit_" + i);
				list.Add(base.InitializeUnit(new PawnKonane(gameEntity, base.PlayerWhoStarted == PlayerTurn.PlayerOne)));
			}
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int j = 0; j < 18; j++)
			{
				GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("player_two_unit_" + j);
				list2.Add(base.InitializeUnit(new PawnKonane(gameEntity2, base.PlayerWhoStarted > PlayerTurn.PlayerOne)));
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0005726C File Offset: 0x0005546C
		public override void InitializeTiles()
		{
			int x;
			IEnumerable<GameEntity> enumerable = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("tile_"))
				select x;
			IEnumerable<GameEntity> enumerable2 = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("decal_"))
				select x;
			int num;
			for (x = 0; x < BoardGameKonane.BoardWidth; x = num)
			{
				int y;
				for (y = 0; y < BoardGameKonane.BoardHeight; y = num)
				{
					GameEntity gameEntity = enumerable.Single<GameEntity>((GameEntity e) => e.HasTag(string.Concat(new object[] { "tile_", x, "_", y })));
					BoardGameDecal firstScriptOfType = enumerable2.Single<GameEntity>((GameEntity e) => e.HasTag(string.Concat(new object[] { "decal_", x, "_", y }))).GetFirstScriptOfType<BoardGameDecal>();
					Tile2D tile2D = new Tile2D(gameEntity, firstScriptOfType, x, y);
					gameEntity.CreateVariableRatePhysics(true);
					this.SetTile(tile2D, x, y);
					num = y + 1;
				}
				num = x + 1;
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000573AA File Offset: 0x000555AA
		public override void InitializeSound()
		{
			PawnBase.PawnMoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/move_stone");
			PawnBase.PawnSelectSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/pick_stone");
			PawnBase.PawnTapSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/drop_stone");
			PawnBase.PawnRemoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/out_stone");
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000573E8 File Offset: 0x000555E8
		public override void Reset()
		{
			base.Reset();
			base.InPreMovementStage = true;
			if (this._startState.PawnInformation == null)
			{
				this.PreplaceUnits();
				return;
			}
			this.RestoreStartingBoard();
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00057414 File Offset: 0x00055614
		public override List<Move> CalculateValidMoves(PawnBase pawn)
		{
			List<Move> list = new List<Move>();
			PawnKonane pawnKonane = pawn as PawnKonane;
			if (pawn != null)
			{
				int x = pawnKonane.X;
				int y = pawnKonane.Y;
				if (!base.InPreMovementStage && pawn.IsPlaced)
				{
					if (x > 1)
					{
						PawnBase pawnOnTile = this.GetTile(x - 1, y).PawnOnTile;
						PawnBase pawnOnTile2 = this.GetTile(x - 2, y).PawnOnTile;
						if (pawnOnTile != null && pawnOnTile2 == null && pawnOnTile.PlayerOne != pawn.PlayerOne)
						{
							Move move;
							move.Unit = pawn;
							move.GoalTile = this.GetTile(x - 2, y);
							list.Add(move);
							if (x > 3)
							{
								PawnBase pawnOnTile3 = this.GetTile(x - 3, y).PawnOnTile;
								PawnBase pawnOnTile4 = this.GetTile(x - 4, y).PawnOnTile;
								if (pawnOnTile3 != null && pawnOnTile4 == null && pawnOnTile3.PlayerOne != pawn.PlayerOne)
								{
									Move move2;
									move2.Unit = pawn;
									move2.GoalTile = this.GetTile(x - 4, y);
									list.Add(move2);
								}
							}
						}
					}
					if (x < BoardGameKonane.BoardWidth - 2)
					{
						PawnBase pawnOnTile5 = this.GetTile(x + 1, y).PawnOnTile;
						PawnBase pawnOnTile6 = this.GetTile(x + 2, y).PawnOnTile;
						if (pawnOnTile5 != null && pawnOnTile6 == null && pawnOnTile5.PlayerOne != pawn.PlayerOne)
						{
							Move move3;
							move3.Unit = pawn;
							move3.GoalTile = this.GetTile(x + 2, y);
							list.Add(move3);
							if (x < 2)
							{
								PawnBase pawnOnTile7 = this.GetTile(x + 3, y).PawnOnTile;
								PawnBase pawnOnTile8 = this.GetTile(x + 4, y).PawnOnTile;
								if (pawnOnTile7 != null && pawnOnTile8 == null && pawnOnTile7.PlayerOne != pawn.PlayerOne)
								{
									Move move4;
									move4.Unit = pawn;
									move4.GoalTile = this.GetTile(x + 4, y);
									list.Add(move4);
								}
							}
						}
					}
					if (y > 1)
					{
						PawnBase pawnOnTile9 = this.GetTile(x, y - 1).PawnOnTile;
						PawnBase pawnOnTile10 = this.GetTile(x, y - 2).PawnOnTile;
						if (pawnOnTile9 != null && pawnOnTile10 == null && pawnOnTile9.PlayerOne != pawn.PlayerOne)
						{
							Move move5;
							move5.Unit = pawn;
							move5.GoalTile = this.GetTile(x, y - 2);
							list.Add(move5);
							if (y > 3)
							{
								PawnBase pawnOnTile11 = this.GetTile(x, y - 3).PawnOnTile;
								PawnBase pawnOnTile12 = this.GetTile(x, y - 4).PawnOnTile;
								if (pawnOnTile11 != null && pawnOnTile12 == null && pawnOnTile11.PlayerOne != pawn.PlayerOne)
								{
									Move move6;
									move6.Unit = pawn;
									move6.GoalTile = this.GetTile(x, y - 4);
									list.Add(move6);
								}
							}
						}
					}
					if (y < BoardGameKonane.BoardHeight - 2)
					{
						PawnBase pawnOnTile13 = this.GetTile(x, y + 1).PawnOnTile;
						PawnBase pawnOnTile14 = this.GetTile(x, y + 2).PawnOnTile;
						if (pawnOnTile13 != null && pawnOnTile14 == null && pawnOnTile13.PlayerOne != pawn.PlayerOne)
						{
							Move move7;
							move7.Unit = pawn;
							move7.GoalTile = this.GetTile(x, y + 2);
							list.Add(move7);
							if (y < 2)
							{
								PawnBase pawnOnTile15 = this.GetTile(x, y + 3).PawnOnTile;
								PawnBase pawnOnTile16 = this.GetTile(x, y + 4).PawnOnTile;
								if (pawnOnTile15 != null && pawnOnTile16 == null && pawnOnTile15.PlayerOne != pawn.PlayerOne)
								{
									Move move8;
									move8.Unit = pawn;
									move8.GoalTile = this.GetTile(x, y + 4);
									list.Add(move8);
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0005777C File Offset: 0x0005597C
		public override void SetPawnCaptured(PawnBase pawn, bool fake = false)
		{
			base.SetPawnCaptured(pawn, fake);
			PawnKonane pawnKonane = pawn as PawnKonane;
			this.GetTile(pawnKonane.X, pawnKonane.Y).PawnOnTile = null;
			pawnKonane.PrevX = pawnKonane.X;
			pawnKonane.PrevY = pawnKonane.Y;
			pawnKonane.X = -1;
			pawnKonane.Y = -1;
			if (!fake)
			{
				base.RemovePawnFromBoard(pawnKonane, 0.6f, false);
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000577E8 File Offset: 0x000559E8
		protected override PawnBase SelectPawn(PawnBase pawn)
		{
			if (base.PlayerTurn == PlayerTurn.PlayerOne)
			{
				if (pawn.PlayerOne)
				{
					if (base.InPreMovementStage)
					{
						if (!pawn.IsPlaced)
						{
							this.SelectedUnit = pawn;
						}
					}
					else
					{
						this.SelectedUnit = pawn;
					}
				}
			}
			else if (base.AIOpponent == null && !pawn.PlayerOne)
			{
				if (base.InPreMovementStage)
				{
					if (!pawn.IsPlaced)
					{
						this.SelectedUnit = pawn;
					}
				}
				else
				{
					this.SelectedUnit = pawn;
				}
			}
			return pawn;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00057858 File Offset: 0x00055A58
		protected override void HandlePreMovementStage(float dt)
		{
			if (base.InputManager.IsHotKeyPressed("BoardGamePawnSelect"))
			{
				PawnBase hoveredPawnIfAny = base.GetHoveredPawnIfAny();
				if (hoveredPawnIfAny != null && this.RemovablePawns.Contains(hoveredPawnIfAny))
				{
					this.SetPawnCaptured(hoveredPawnIfAny, false);
					this.UnFocusRemovablePawns();
					base.EndTurn();
					return;
				}
			}
			else
			{
				this.SelectedUnit = null;
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000578AB File Offset: 0x00055AAB
		protected override void HandlePreMovementStageAI(Move move)
		{
			this.SetPawnCaptured(move.Unit, false);
			base.EndTurn();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000578C0 File Offset: 0x00055AC0
		protected override void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			base.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, delay);
			Tile2D tile2D = tile as Tile2D;
			PawnKonane pawnKonane = pawn as PawnKonane;
			if (tile2D.PawnOnTile == null && pawnKonane != null)
			{
				if (displayMessage)
				{
					if (base.PlayerTurn == PlayerTurn.PlayerOne)
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_boardgame_move_piece_player", null).ToString()));
					}
					else
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_boardgame_move_piece_opponent", null).ToString()));
					}
				}
				Vec3 globalPosition = tile2D.Entity.GlobalPosition;
				float num = 0.5f;
				if (!base.InPreMovementStage)
				{
					num = 0.3f;
				}
				pawnKonane.MovingToDifferentTile = pawnKonane.X != tile2D.X || pawnKonane.Y != tile2D.Y;
				pawnKonane.PrevX = pawnKonane.X;
				pawnKonane.PrevY = pawnKonane.Y;
				pawnKonane.X = tile2D.X;
				pawnKonane.Y = tile2D.Y;
				if (pawnKonane.PrevX != -1 && pawnKonane.PrevY != -1)
				{
					this.GetTile(pawnKonane.PrevX, pawnKonane.PrevY).PawnOnTile = null;
				}
				tile.PawnOnTile = pawnKonane;
				if (instantMove || base.InPreMovementStage || this.JustStoppedDraggingUnit)
				{
					pawnKonane.AddGoalPosition(globalPosition);
					pawnKonane.MovePawnToGoalPositionsDelayed(instantMove, num, true, delay);
				}
				else
				{
					Tile2D tile2D2 = this.GetTile(pawnKonane.PrevX, pawnKonane.PrevY) as Tile2D;
					this.SetAllGoalPositions(pawnKonane, tile2D2, num);
				}
				if (instantMove && !base.InPreMovementStage)
				{
					this.CheckWhichPawnsAreCaptured(pawnKonane, false);
				}
				else if (pawnKonane == this.SelectedUnit && instantMove)
				{
					this.SelectedUnit = null;
				}
				base.ClearValidMoves();
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00057A60 File Offset: 0x00055C60
		protected override void SwitchPlayerTurn()
		{
			if ((base.PlayerTurn == PlayerTurn.PlayerOneWaiting || base.PlayerTurn == PlayerTurn.PlayerTwoWaiting) && !base.InPreMovementStage && this.SelectedUnit != null)
			{
				this.CheckWhichPawnsAreCaptured(this.SelectedUnit as PawnKonane, false);
			}
			this.SelectedUnit = null;
			bool flag = false;
			if (base.InPreMovementStage)
			{
				base.InPreMovementStage = !this.CheckPlacementStageOver();
				flag = !base.InPreMovementStage;
			}
			if (!flag)
			{
				if (base.PlayerTurn == PlayerTurn.PlayerOneWaiting)
				{
					base.PlayerTurn = PlayerTurn.PlayerTwo;
				}
				else if (base.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
				{
					base.PlayerTurn = PlayerTurn.PlayerOne;
				}
			}
			if (base.InPreMovementStage)
			{
				if (base.PlayerTurn == PlayerTurn.PlayerOne)
				{
					this.CheckForRemovablePawns(true);
				}
				else if (base.PlayerTurn == PlayerTurn.PlayerTwo)
				{
					this.CheckForRemovablePawns(false);
				}
			}
			else if (flag)
			{
				base.EndTurn();
			}
			else
			{
				this.CheckGameEnded();
			}
			base.SwitchPlayerTurn();
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00057B38 File Offset: 0x00055D38
		protected override bool CheckGameEnded()
		{
			bool flag = false;
			if (base.PlayerTurn == PlayerTurn.PlayerTwo)
			{
				List<List<Move>> list = this.CalculateAllValidMoves(BoardGameSide.AI);
				if (!base.HasMovesAvailable(ref list))
				{
					base.OnVictory("str_boardgame_victory_message");
					this.ReadyToPlay = false;
					flag = true;
				}
			}
			else if (base.PlayerTurn == PlayerTurn.PlayerOne)
			{
				List<List<Move>> list2 = this.CalculateAllValidMoves(BoardGameSide.None);
				if (!base.HasMovesAvailable(ref list2))
				{
					base.OnDefeat("str_boardgame_defeat_message");
					this.ReadyToPlay = false;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00057BA7 File Offset: 0x00055DA7
		protected override void OnAfterBoardSetUp()
		{
			if (this._startState.PawnInformation == null)
			{
				this._startState = this.TakeBoardSnapshot();
			}
			this.ReadyToPlay = true;
			this.CheckForRemovablePawns(base.PlayerWhoStarted == PlayerTurn.PlayerOne);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00057BDC File Offset: 0x00055DDC
		public void AIMakeMove(Move move)
		{
			Tile2D tile2D = move.GoalTile as Tile2D;
			PawnKonane pawnKonane = move.Unit as PawnKonane;
			if (tile2D.PawnOnTile == null)
			{
				pawnKonane.PrevX = pawnKonane.X;
				pawnKonane.PrevY = pawnKonane.Y;
				pawnKonane.X = tile2D.X;
				pawnKonane.Y = tile2D.Y;
				this.GetTile(pawnKonane.PrevX, pawnKonane.PrevY).PawnOnTile = null;
				tile2D.PawnOnTile = pawnKonane;
				this.CheckWhichPawnsAreCaptured(pawnKonane, true);
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00057C60 File Offset: 0x00055E60
		public int CheckForRemovablePawns(bool playerOne)
		{
			this.UnFocusRemovablePawns();
			int num = (playerOne ? base.GetPlayerTwoUnitsDead() : base.GetPlayerOneUnitsDead());
			if (num == 0)
			{
				using (List<PawnBase>.Enumerator enumerator = (playerOne ? base.PlayerOneUnits : base.PlayerTwoUnits).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PawnBase pawnBase = enumerator.Current;
						PawnKonane pawnKonane = (PawnKonane)pawnBase;
						if (pawnKonane.X == 0 && pawnKonane.Y == 0)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 5 && pawnKonane.Y == 0)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 0 && pawnKonane.Y == 5)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 5 && pawnKonane.Y == 5)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 2 && pawnKonane.Y == 2)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 3 && pawnKonane.Y == 2)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 2 && pawnKonane.Y == 3)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
						else if (pawnKonane.X == 3 && pawnKonane.Y == 3)
						{
							this.RemovablePawns.Add(pawnKonane);
						}
					}
					goto IL_040C;
				}
			}
			if (num == 1)
			{
				using (List<PawnBase>.Enumerator enumerator = (playerOne ? base.PlayerTwoUnits : base.PlayerOneUnits).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PawnBase pawnBase2 = enumerator.Current;
						PawnKonane pawnKonane2 = (PawnKonane)pawnBase2;
						if (pawnKonane2.X == -1 && pawnKonane2.Y == -1)
						{
							if (pawnKonane2.PrevX == 0 && pawnKonane2.PrevY == 0)
							{
								this.RemovablePawns.Add(this.GetTile(1, 0).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(0, 1).PawnOnTile);
							}
							else if (pawnKonane2.PrevX == 5 && pawnKonane2.PrevY == 0)
							{
								this.RemovablePawns.Add(this.GetTile(4, 0).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(5, 1).PawnOnTile);
							}
							else if (pawnKonane2.PrevX == 0 && pawnKonane2.PrevY == 5)
							{
								this.RemovablePawns.Add(this.GetTile(0, 4).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(1, 5).PawnOnTile);
							}
							else if (pawnKonane2.PrevX == 5 && pawnKonane2.PrevY == 5)
							{
								this.RemovablePawns.Add(this.GetTile(5, 4).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(4, 5).PawnOnTile);
							}
							if (pawnKonane2.PrevX == 2 && pawnKonane2.PrevY == 2)
							{
								this.RemovablePawns.Add(this.GetTile(2, 3).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(3, 2).PawnOnTile);
								break;
							}
							if (pawnKonane2.PrevX == 3 && pawnKonane2.PrevY == 2)
							{
								this.RemovablePawns.Add(this.GetTile(2, 2).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(3, 3).PawnOnTile);
								break;
							}
							if (pawnKonane2.PrevX == 2 && pawnKonane2.PrevY == 3)
							{
								this.RemovablePawns.Add(this.GetTile(3, 3).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(2, 2).PawnOnTile);
								break;
							}
							if (pawnKonane2.PrevX == 3 && pawnKonane2.PrevY == 3)
							{
								this.RemovablePawns.Add(this.GetTile(2, 3).PawnOnTile);
								this.RemovablePawns.Add(this.GetTile(3, 2).PawnOnTile);
								break;
							}
							break;
						}
					}
					goto IL_040C;
				}
			}
			Debug.FailedAssert("[DEBUG]This should not be reached!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameKonane.cs", "CheckForRemovablePawns", 655);
			IL_040C:
			this.FocusRemovablePawns();
			return this.RemovablePawns.Count;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000580C0 File Offset: 0x000562C0
		public BoardGameKonane.BoardInformation TakeBoardSnapshot()
		{
			BoardGameKonane.PawnInformation[] array = new BoardGameKonane.PawnInformation[base.PlayerOneUnits.Count + base.PlayerTwoUnits.Count];
			TileBaseInformation[,] array2 = new TileBaseInformation[BoardGameKonane.BoardWidth, BoardGameKonane.BoardHeight];
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				PawnKonane pawnKonane = (PawnKonane)pawnBase;
				array[num++] = new BoardGameKonane.PawnInformation(pawnKonane.X, pawnKonane.Y, pawnKonane.PrevX, pawnKonane.PrevY, pawnKonane.Captured, pawnKonane.Entity.GlobalPosition);
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnKonane pawnKonane2 = (PawnKonane)pawnBase2;
				array[num++] = new BoardGameKonane.PawnInformation(pawnKonane2.X, pawnKonane2.Y, pawnKonane2.PrevX, pawnKonane2.PrevY, pawnKonane2.Captured, pawnKonane2.Entity.GlobalPosition);
			}
			for (int i = 0; i < BoardGameKonane.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameKonane.BoardHeight; j++)
				{
					array2[i, j] = new TileBaseInformation(ref this.GetTile(i, j).PawnOnTile);
				}
			}
			return new BoardGameKonane.BoardInformation(ref array, ref array2);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00058270 File Offset: 0x00056470
		public void UndoMove(ref BoardGameKonane.BoardInformation board)
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				PawnKonane pawnKonane = (PawnKonane)pawnBase;
				pawnKonane.X = board.PawnInformation[num].X;
				pawnKonane.Y = board.PawnInformation[num].Y;
				pawnKonane.PrevX = board.PawnInformation[num].PrevX;
				pawnKonane.PrevY = board.PawnInformation[num].PrevY;
				pawnKonane.Captured = board.PawnInformation[num].IsCaptured;
				num++;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnKonane pawnKonane2 = (PawnKonane)pawnBase2;
				pawnKonane2.X = board.PawnInformation[num].X;
				pawnKonane2.Y = board.PawnInformation[num].Y;
				pawnKonane2.PrevX = board.PawnInformation[num].PrevX;
				pawnKonane2.PrevY = board.PawnInformation[num].PrevY;
				pawnKonane2.Captured = board.PawnInformation[num].IsCaptured;
				num++;
			}
			for (int i = 0; i < BoardGameKonane.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameKonane.BoardHeight; j++)
				{
					this.GetTile(i, j).PawnOnTile = board.TileInformation[i, j].PawnOnTile;
				}
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00058458 File Offset: 0x00056658
		protected void CheckWhichPawnsAreCaptured(PawnKonane pawn, bool fake = false)
		{
			int x = pawn.X;
			int y = pawn.Y;
			int prevX = pawn.PrevX;
			int prevY = pawn.PrevY;
			bool flag = false;
			if (x == -1 || y == -1 || prevX == -1 || prevY == -1)
			{
				Debug.FailedAssert("x == -1 || y == -1 || prevX == -1 || prevY == -1", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameKonane.cs", "CheckWhichPawnsAreCaptured", 738);
			}
			Vec2i vec2i = new Vec2i(x - prevX, y - prevY);
			if (vec2i.X == 4 || vec2i.Y == 4 || vec2i.X == -4 || vec2i.Y == -4)
			{
				flag = true;
			}
			else if (vec2i.X == 2 || vec2i.Y == 2 || vec2i.X == -2 || vec2i.Y == -2)
			{
				flag = false;
			}
			else
			{
				Debug.FailedAssert("CheckWhichPawnsAreCaptured", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameKonane.cs", "CheckWhichPawnsAreCaptured", 753);
			}
			if (!flag)
			{
				Vec2i vec2i2 = new Vec2i(vec2i.X / 2, vec2i.Y / 2);
				Vec2i vec2i3 = new Vec2i(x - vec2i2.X, y - vec2i2.Y);
				this.SetPawnCaptured(this.GetTile(vec2i3.X, vec2i3.Y).PawnOnTile, fake);
				return;
			}
			Vec2i vec2i4 = new Vec2i(vec2i.X / 4, vec2i.Y / 4);
			Vec2i vec2i5 = new Vec2i(x - vec2i4.X, y - vec2i4.Y);
			Vec2i vec2i6 = new Vec2i(x - vec2i4.X - vec2i4.X * 2, y - vec2i4.Y - vec2i4.Y * 2);
			this.SetPawnCaptured(this.GetTile(vec2i5.X, vec2i5.Y).PawnOnTile, fake);
			this.SetPawnCaptured(this.GetTile(vec2i6.X, vec2i6.Y).PawnOnTile, fake);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00058627 File Offset: 0x00056827
		private void SetTile(TileBase tile, int x, int y)
		{
			base.Tiles[y * BoardGameKonane.BoardWidth + x] = tile;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0005863A File Offset: 0x0005683A
		private TileBase GetTile(int x, int y)
		{
			return base.Tiles[y * BoardGameKonane.BoardWidth + x];
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0005864C File Offset: 0x0005684C
		private void FocusRemovablePawns()
		{
			foreach (PawnBase pawnBase in this.RemovablePawns)
			{
				((PawnKonane)pawnBase).Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnSelectedFactor);
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000586B4 File Offset: 0x000568B4
		private void UnFocusRemovablePawns()
		{
			foreach (PawnBase pawnBase in this.RemovablePawns)
			{
				((PawnKonane)pawnBase).Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnUnselectedFactor);
			}
			this.RemovablePawns.Clear();
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00058728 File Offset: 0x00056928
		private void SetAllGoalPositions(PawnKonane pawn, Tile2D prevTile, float speed)
		{
			Vec3 globalPosition = prevTile.Entity.GlobalPosition;
			Vec3 globalPosition2 = this.GetTile(pawn.X, pawn.Y).Entity.GlobalPosition;
			bool flag = false;
			Vec2i vec2i = new Vec2i(pawn.X - prevTile.X, pawn.Y - prevTile.Y);
			if (vec2i.X == 4 || vec2i.Y == 4 || vec2i.X == -4 || vec2i.Y == -4)
			{
				flag = true;
			}
			if (!flag)
			{
				pawn.AddGoalPosition(globalPosition2);
			}
			else
			{
				Vec2i vec2i2 = new Vec2i(vec2i.X / 4, vec2i.Y / 4);
				pawn.AddGoalPosition(this.GetTile(prevTile.X + 2 * vec2i2.X, prevTile.Y + 2 * vec2i2.Y).Entity.GlobalPosition);
				pawn.AddGoalPosition(globalPosition2);
			}
			pawn.MovePawnToGoalPositions(false, speed, false);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00058810 File Offset: 0x00056A10
		private bool CheckPlacementStageOver()
		{
			bool flag = false;
			if (base.GetPlayerOneUnitsDead() + base.GetPlayerTwoUnitsDead() == 2)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00058834 File Offset: 0x00056A34
		private void PreplaceUnits()
		{
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int i = 0; i < 18; i++)
			{
				int num = i % 3 * 2;
				int num2 = i / 3;
				float num3 = 0.15f * (float)(i + 1) + 0.25f;
				if (num2 % 2 == 0)
				{
					this.MovePawnToTileDelayed(list[i], this.GetTile(num, num2), false, false, num3);
					this.MovePawnToTileDelayed(list2[i], this.GetTile(num + 1, num2), false, false, num3);
				}
				else
				{
					this.MovePawnToTileDelayed(list[i], this.GetTile(num + 1, num2), false, false, num3);
					this.MovePawnToTileDelayed(list2[i], this.GetTile(num, num2), false, false, num3);
				}
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00058914 File Offset: 0x00056B14
		private void RestoreStartingBoard()
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				PawnKonane pawnKonane = (PawnKonane)pawnBase;
				if (this._startState.PawnInformation[num].X != -1)
				{
					if (this._startState.PawnInformation[num].X != pawnKonane.X && this._startState.PawnInformation[num].Y != pawnKonane.Y)
					{
						pawnKonane.Reset();
						TileBase tile = this.GetTile(this._startState.PawnInformation[num].X, this._startState.PawnInformation[num].Y);
						this.MovePawnToTile(pawnKonane, tile, false, true);
					}
				}
				else if (!pawnKonane.Entity.GlobalPosition.NearlyEquals(in this._startState.PawnInformation[num].Position, 1E-05f))
				{
					if (pawnKonane.X != -1 && this.GetTile(pawnKonane.X, pawnKonane.Y).PawnOnTile == pawnKonane)
					{
						this.GetTile(pawnKonane.X, pawnKonane.Y).PawnOnTile = null;
					}
					pawnKonane.Reset();
					pawnKonane.AddGoalPosition(this._startState.PawnInformation[num].Position);
					pawnKonane.MovePawnToGoalPositions(false, 0.5f, false);
				}
				num++;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnKonane pawnKonane2 = (PawnKonane)pawnBase2;
				if (this._startState.PawnInformation[num].X != -1)
				{
					if (this._startState.PawnInformation[num].X != pawnKonane2.X && this._startState.PawnInformation[num].Y != pawnKonane2.Y)
					{
						TileBase tile2 = this.GetTile(this._startState.PawnInformation[num].X, this._startState.PawnInformation[num].Y);
						this.MovePawnToTile(pawnKonane2, tile2, false, true);
					}
				}
				else
				{
					if (pawnKonane2.X != -1 && this.GetTile(pawnKonane2.X, pawnKonane2.Y).PawnOnTile == pawnKonane2)
					{
						this.GetTile(pawnKonane2.X, pawnKonane2.Y).PawnOnTile = null;
					}
					pawnKonane2.Reset();
					pawnKonane2.AddGoalPosition(this._startState.PawnInformation[num].Position);
					pawnKonane2.MovePawnToGoalPositions(false, 0.5f, false);
				}
				num++;
			}
		}

		// Token: 0x0400053B RID: 1339
		public const int WhitePawnCount = 18;

		// Token: 0x0400053C RID: 1340
		public const int BlackPawnCount = 18;

		// Token: 0x0400053D RID: 1341
		public static readonly int BoardWidth = 6;

		// Token: 0x0400053E RID: 1342
		public static readonly int BoardHeight = 6;

		// Token: 0x0400053F RID: 1343
		public List<PawnBase> RemovablePawns = new List<PawnBase>();

		// Token: 0x04000540 RID: 1344
		private BoardGameKonane.BoardInformation _startState;

		// Token: 0x0200021D RID: 541
		public struct BoardInformation
		{
			// Token: 0x06001439 RID: 5177 RVA: 0x00079FE5 File Offset: 0x000781E5
			public BoardInformation(ref BoardGameKonane.PawnInformation[] pawns, ref TileBaseInformation[,] tiles)
			{
				this.PawnInformation = pawns;
				this.TileInformation = tiles;
			}

			// Token: 0x0400098E RID: 2446
			public readonly BoardGameKonane.PawnInformation[] PawnInformation;

			// Token: 0x0400098F RID: 2447
			public readonly TileBaseInformation[,] TileInformation;
		}

		// Token: 0x0200021E RID: 542
		public struct PawnInformation
		{
			// Token: 0x0600143A RID: 5178 RVA: 0x00079FF7 File Offset: 0x000781F7
			public PawnInformation(int x, int y, int prevX, int prevY, bool captured, Vec3 position)
			{
				this.X = x;
				this.Y = y;
				this.PrevX = prevX;
				this.PrevY = prevY;
				this.IsCaptured = captured;
				this.Position = position;
			}

			// Token: 0x04000990 RID: 2448
			public readonly int X;

			// Token: 0x04000991 RID: 2449
			public readonly int Y;

			// Token: 0x04000992 RID: 2450
			public readonly int PrevX;

			// Token: 0x04000993 RID: 2451
			public readonly int PrevY;

			// Token: 0x04000994 RID: 2452
			public readonly bool IsCaptured;

			// Token: 0x04000995 RID: 2453
			public readonly Vec3 Position;
		}
	}
}
