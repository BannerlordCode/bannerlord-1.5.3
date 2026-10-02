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
	// Token: 0x020000F3 RID: 243
	public class BoardGameSeega : BoardGameBase
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0005AB00 File Offset: 0x00058D00
		public override int TileCount
		{
			get
			{
				return BoardGameSeega.BoardWidth * BoardGameSeega.BoardHeight;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0005AB0D File Offset: 0x00058D0D
		protected override int UnitsToPlacePerTurnInPreMovementStage
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0005AB10 File Offset: 0x00058D10
		protected override bool RotateBoard
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0005AB13 File Offset: 0x00058D13
		protected override bool PreMovementStagePresent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0005AB16 File Offset: 0x00058D16
		protected override bool DiceRollRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0005AB19 File Offset: 0x00058D19
		public BoardGameSeega(MissionBoardGameLogic mission, PlayerTurn startingPlayer)
			: base(mission, new TextObject("{=C4n1rgBC}Seega", null), startingPlayer)
		{
			this.SelectedUnit = null;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0005AB40 File Offset: 0x00058D40
		public override void InitializeUnits()
		{
			base.PlayerOneUnits.Clear();
			base.PlayerTwoUnits.Clear();
			int num = 0;
			GameEntity gameEntity;
			do
			{
				gameEntity = Mission.Current.Scene.FindEntityWithTag("player_one_unit_" + num.ToString());
				if (gameEntity != null)
				{
					base.PlayerOneUnits.Add(base.InitializeUnit(new PawnSeega(gameEntity, true)));
					num++;
				}
			}
			while (gameEntity != null);
			num = 0;
			do
			{
				gameEntity = Mission.Current.Scene.FindEntityWithTag("player_two_unit_" + num.ToString());
				if (gameEntity != null)
				{
					base.PlayerTwoUnits.Add(base.InitializeUnit(new PawnSeega(gameEntity, false)));
					num++;
				}
			}
			while (gameEntity != null);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0005AC08 File Offset: 0x00058E08
		public override void InitializeTiles()
		{
			int x;
			IEnumerable<GameEntity> enumerable = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("tile_"))
				select x;
			IEnumerable<GameEntity> enumerable2 = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("decal_"))
				select x;
			if (base.Tiles == null)
			{
				base.Tiles = new TileBase[this.TileCount];
			}
			int num;
			for (x = 0; x < BoardGameSeega.BoardWidth; x = num)
			{
				int y;
				for (y = 0; y < BoardGameSeega.BoardHeight; y = num)
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

		// Token: 0x06000C3F RID: 3135 RVA: 0x0005AD5F File Offset: 0x00058F5F
		public override void InitializeSound()
		{
			PawnBase.PawnMoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/move_stone");
			PawnBase.PawnSelectSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/pick_stone");
			PawnBase.PawnTapSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/drop_stone");
			PawnBase.PawnRemoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/out_stone");
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0005AD9D File Offset: 0x00058F9D
		public override void Reset()
		{
			base.Reset();
			base.InPreMovementStage = true;
			this._placementStageOver = false;
			if (this._startState.PawnInformation == null)
			{
				this.PreplaceUnits();
				return;
			}
			this.RestoreStartingBoard();
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0005ADD0 File Offset: 0x00058FD0
		public override List<Move> CalculateValidMoves(PawnBase pawn)
		{
			List<Move> list = new List<Move>();
			PawnSeega pawnSeega = pawn as PawnSeega;
			if (pawn != null)
			{
				int x = pawnSeega.X;
				int y = pawnSeega.Y;
				if (!base.InPreMovementStage && pawnSeega.IsPlaced)
				{
					if (x > 0)
					{
						TileBase tile = this.GetTile(x - 1, y);
						if (tile.PawnOnTile == null && (pawnSeega.PrevX != x - 1 || pawnSeega.PrevY != y))
						{
							Move move;
							move.Unit = pawn;
							move.GoalTile = tile;
							list.Add(move);
						}
					}
					if (x < BoardGameSeega.BoardWidth - 1)
					{
						TileBase tile2 = this.GetTile(x + 1, y);
						if (tile2.PawnOnTile == null && (pawnSeega.PrevX != x + 1 || pawnSeega.PrevY != y))
						{
							Move move2;
							move2.Unit = pawn;
							move2.GoalTile = tile2;
							list.Add(move2);
						}
					}
					if (y > 0)
					{
						TileBase tile3 = this.GetTile(x, y - 1);
						if (tile3.PawnOnTile == null && (pawnSeega.PrevX != x || pawnSeega.PrevY != y - 1))
						{
							Move move3;
							move3.Unit = pawn;
							move3.GoalTile = tile3;
							list.Add(move3);
						}
					}
					if (y < BoardGameSeega.BoardHeight - 1)
					{
						TileBase tile4 = this.GetTile(x, y + 1);
						if (tile4.PawnOnTile == null && (pawnSeega.PrevX != x || pawnSeega.PrevY != y + 1))
						{
							Move move4;
							move4.Unit = pawn;
							move4.GoalTile = tile4;
							list.Add(move4);
						}
					}
				}
				else if (base.InPreMovementStage && !pawnSeega.IsPlaced)
				{
					for (int i = 0; i < this.TileCount; i++)
					{
						TileBase tileBase = base.Tiles[i];
						if (tileBase.PawnOnTile == null && !tileBase.Entity.HasTag("obstructed_at_start"))
						{
							Move move5;
							move5.Unit = pawn;
							move5.GoalTile = tileBase;
							list.Add(move5);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0005AFAC File Offset: 0x000591AC
		public override void SetPawnCaptured(PawnBase pawn, bool aiSimulation = false)
		{
			base.SetPawnCaptured(pawn, aiSimulation);
			PawnSeega pawnSeega = pawn as PawnSeega;
			this.GetTile(pawnSeega.X, pawnSeega.Y).PawnOnTile = null;
			pawnSeega.X = -1;
			pawnSeega.Y = -1;
			if (!aiSimulation)
			{
				base.RemovePawnFromBoard(pawnSeega, 0.8f, false);
				MBDebug.Print(string.Concat(new object[] { "Setting pawn captured: X: ", pawnSeega.X, ", Y: ", pawnSeega.Y }), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0005B048 File Offset: 0x00059248
		protected override void OnPawnArrivesGoalPosition(PawnBase pawn, Vec3 prevPos, Vec3 currentPos)
		{
			if (pawn.MovingToDifferentTile)
			{
				base.PawnSelectFilter.Clear();
				PawnSeega pawnSeega = this.SelectedUnit as PawnSeega;
				if (!base.InPreMovementStage && pawnSeega != null)
				{
					if (this.CheckIfPawnCaptures(pawnSeega, false, true) > 0)
					{
						PawnSeega pawnSeega2 = pawn as PawnSeega;
						bool flag = false;
						List<Move> list = this.CalculateValidMoves(pawn);
						int count = list.Count;
						for (int i = 0; i < count; i++)
						{
							Tile2D tile2D = this.GetTile(pawnSeega2.X, pawnSeega2.Y) as Tile2D;
							Tile2D tile2D2 = list[i].GoalTile as Tile2D;
							tile2D.PawnOnTile = null;
							pawnSeega2.X = tile2D2.X;
							pawnSeega2.Y = tile2D2.Y;
							tile2D2.PawnOnTile = pawnSeega2;
							int num = this.CheckIfPawnCaptures(pawnSeega2, false, false);
							tile2D2.PawnOnTile = null;
							pawnSeega2.X = tile2D.X;
							pawnSeega2.Y = tile2D.Y;
							tile2D.PawnOnTile = pawnSeega2;
							if (num > 0)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							if (!base.PawnSelectFilter.Contains(pawn))
							{
								base.PawnSelectFilter.Add(pawn);
							}
							this.MovesLeftToEndTurn++;
						}
					}
					if (this.CheckIfUnitsIsolatedByBarrier(new Vec2i(pawnSeega.X, pawnSeega.Y)))
					{
						MBDebug.Print("Barrier was formed!", 0, Debug.DebugColor.White, 17592186044416UL);
						int playerOneUnitsAlive = base.GetPlayerOneUnitsAlive();
						int playerTwoUnitsAlive = base.GetPlayerTwoUnitsAlive();
						if (playerOneUnitsAlive > playerTwoUnitsAlive)
						{
							string text = (pawnSeega.PlayerOne ? "str_boardgame_seega_barrier_by_player_one_victory_message" : "str_boardgame_seega_barrier_by_player_two_victory_message");
							base.OnVictory(text);
						}
						else if (playerOneUnitsAlive < playerTwoUnitsAlive)
						{
							string text2 = (pawnSeega.PlayerOne ? "str_boardgame_seega_barrier_by_player_one_defeat_message" : "str_boardgame_seega_barrier_by_player_two_defeat_message");
							base.OnDefeat(text2);
						}
						else
						{
							string text3 = (pawnSeega.PlayerOne ? "str_boardgame_seega_barrier_by_player_one_draw_message" : "str_boardgame_seega_barrier_by_player_two_draw_message");
							base.OnDraw(text3);
						}
						this.ReadyToPlay = false;
					}
				}
				this.CheckGameEnded();
			}
			base.OnPawnArrivesGoalPosition(pawn, prevPos, currentPos);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0005B248 File Offset: 0x00059448
		protected override void SwitchPlayerTurn()
		{
			this.SelectedUnit = null;
			if (base.PlayerTurn == PlayerTurn.PlayerOneWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerTwo;
				if (base.InPreMovementStage)
				{
					goto IL_00C5;
				}
				if (this.MissionHandler.AIOpponent == null)
				{
					this.CheckIfPlayerIsStuck(false);
				}
				using (List<PawnBase>.Enumerator enumerator = base.PlayerTwoUnits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PawnBase pawnBase = enumerator.Current;
						((PawnSeega)pawnBase).UpdateMoveBackAvailable();
					}
					goto IL_00C5;
				}
			}
			if (base.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerOne;
				if (!base.InPreMovementStage)
				{
					this.CheckIfPlayerIsStuck(true);
					foreach (PawnBase pawnBase2 in base.PlayerOneUnits)
					{
						((PawnSeega)pawnBase2).UpdateMoveBackAvailable();
					}
				}
			}
			IL_00C5:
			bool inPreMovementStage = base.InPreMovementStage;
			base.InPreMovementStage = !this.CheckPlacementStageOver() || (this._blockingPawns != null && this._blockingPawns.Count > 0);
			if (inPreMovementStage != base.InPreMovementStage && !base.InPreMovementStage)
			{
				base.EndTurn();
			}
			base.SwitchPlayerTurn();
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0005B384 File Offset: 0x00059584
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

		// Token: 0x06000C46 RID: 3142 RVA: 0x0005B3F4 File Offset: 0x000595F4
		protected override void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			base.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, delay);
			Tile2D tile2D = tile as Tile2D;
			PawnSeega pawnSeega = pawn as PawnSeega;
			if (tile2D.PawnOnTile == null && pawnSeega != null)
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
				float num = 0.7f;
				if (!base.InPreMovementStage)
				{
					num = 0.3f;
				}
				pawnSeega.MovingToDifferentTile = pawnSeega.X != tile2D.X || pawnSeega.Y != tile2D.Y;
				pawnSeega.AddGoalPosition(globalPosition);
				pawnSeega.MovePawnToGoalPositionsDelayed(instantMove, num, this.JustStoppedDraggingUnit, delay);
				pawnSeega.PrevX = pawnSeega.X;
				pawnSeega.PrevY = pawnSeega.Y;
				pawnSeega.X = tile2D.X;
				pawnSeega.Y = tile2D.Y;
				if (pawnSeega.PrevX != -1 && pawnSeega.PrevY != -1)
				{
					this.GetTile(pawnSeega.PrevX, pawnSeega.PrevY).PawnOnTile = null;
				}
				tile2D.PawnOnTile = pawnSeega;
				if (instantMove && !base.InPreMovementStage)
				{
					this.CheckIfPawnCaptures(pawnSeega, false, true);
					return;
				}
				if (pawnSeega == this.SelectedUnit && instantMove)
				{
					this.SelectedUnit = null;
				}
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0005B55C File Offset: 0x0005975C
		protected override void HandlePreMovementStage(float dt)
		{
			if (this._blockingPawns != null && this._blockingPawns.Count > 0)
			{
				if (!base.InputManager.IsHotKeyPressed("BoardGamePawnSelect"))
				{
					this.SelectedUnit = null;
					return;
				}
				PawnBase hoveredPawnIfAny = base.GetHoveredPawnIfAny();
				if (hoveredPawnIfAny != null && this._blockingPawns.ContainsKey(hoveredPawnIfAny))
				{
					this.SetPawnCaptured(hoveredPawnIfAny, false);
					this.UnfocusBlockingPawns();
					base.InPreMovementStage = false;
					return;
				}
			}
			else
			{
				Move move = base.HandlePlayerInput(dt);
				if (move.IsValid)
				{
					this.MovePawnToTile(move.Unit, move.GoalTile, false, true);
				}
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0005B5EC File Offset: 0x000597EC
		protected override void HandlePreMovementStageAI(Move move)
		{
			this.MovePawnToTile(move.Unit, move.GoalTile, false, true);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0005B604 File Offset: 0x00059804
		protected override bool CheckGameEnded()
		{
			if (this.ReadyToPlay)
			{
				if (base.GetPlayerOneUnitsAlive() <= 1)
				{
					base.OnDefeat("str_boardgame_defeat_message");
					this.ReadyToPlay = false;
				}
				else if (base.GetPlayerTwoUnitsAlive() <= 1)
				{
					base.OnVictory("str_boardgame_victory_message");
					this.ReadyToPlay = false;
				}
			}
			return !this.ReadyToPlay;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0005B65A File Offset: 0x0005985A
		protected override void OnAfterBoardSetUp()
		{
			if (this._startState.PawnInformation == null)
			{
				this._startState = this.TakeBoardSnapshot();
			}
			this.ReadyToPlay = true;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0005B67C File Offset: 0x0005987C
		public void AIMakeMove(Move move)
		{
			Tile2D tile2D = move.GoalTile as Tile2D;
			PawnSeega pawnSeega = move.Unit as PawnSeega;
			if (tile2D.PawnOnTile == null)
			{
				pawnSeega.PrevX = pawnSeega.X;
				pawnSeega.PrevY = pawnSeega.Y;
				pawnSeega.X = tile2D.X;
				pawnSeega.Y = tile2D.Y;
				this.GetTile(pawnSeega.PrevX, pawnSeega.PrevY).PawnOnTile = null;
				tile2D.PawnOnTile = pawnSeega;
				this.CheckIfPawnCaptures(pawnSeega, true, true);
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0005B704 File Offset: 0x00059904
		public Dictionary<PawnBase, int> GetBlockingPawns(bool playerOneBlocked)
		{
			Dictionary<PawnBase, int> dictionary = new Dictionary<PawnBase, int>();
			foreach (PawnBase pawnBase in (playerOneBlocked ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnSeega pawnSeega = (PawnSeega)pawnBase;
				if (pawnSeega.IsPlaced && !this.IsOnCentralTile(pawnSeega))
				{
					BoardGameSeega.BoardInformation boardInformation = this.TakeBoardSnapshot();
					this.SetPawnCaptured(pawnSeega, true);
					int num = 0;
					foreach (PawnBase pawnBase2 in (playerOneBlocked ? base.PlayerOneUnits : base.PlayerTwoUnits))
					{
						PawnSeega pawnSeega2 = (PawnSeega)pawnBase2;
						if (pawnSeega2.IsPlaced)
						{
							num += this.CalculateValidMoves(pawnSeega2).Count;
						}
					}
					if (num > 0)
					{
						dictionary.Add(pawnSeega, num);
					}
					this.UndoMove(ref boardInformation);
				}
			}
			return dictionary;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0005B814 File Offset: 0x00059A14
		public BoardGameSeega.BoardInformation TakeBoardSnapshot()
		{
			BoardGameSeega.PawnInformation[] array = new BoardGameSeega.PawnInformation[base.PlayerOneUnits.Count + base.PlayerTwoUnits.Count];
			TileBaseInformation[,] array2 = new TileBaseInformation[BoardGameSeega.BoardWidth, BoardGameSeega.BoardHeight];
			int num = 0;
			foreach (PawnBase pawnBase in base.PlayerOneUnits)
			{
				PawnSeega pawnSeega = (PawnSeega)pawnBase;
				array[num++] = new BoardGameSeega.PawnInformation(pawnSeega.X, pawnSeega.Y, pawnSeega.PrevX, pawnSeega.PrevY, pawnSeega.MovedThisTurn, pawnSeega.Captured, pawnSeega.Entity.GlobalPosition);
			}
			foreach (PawnBase pawnBase2 in base.PlayerTwoUnits)
			{
				PawnSeega pawnSeega2 = (PawnSeega)pawnBase2;
				array[num++] = new BoardGameSeega.PawnInformation(pawnSeega2.X, pawnSeega2.Y, pawnSeega2.PrevX, pawnSeega2.PrevY, pawnSeega2.MovedThisTurn, pawnSeega2.Captured, pawnSeega2.Entity.GlobalPosition);
			}
			for (int i = 0; i < BoardGameSeega.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameSeega.BoardHeight; j++)
				{
					array2[i, j] = new TileBaseInformation(ref this.GetTile(i, j).PawnOnTile);
				}
			}
			return new BoardGameSeega.BoardInformation(ref array, ref array2);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0005B9B4 File Offset: 0x00059BB4
		public void UndoMove(ref BoardGameSeega.BoardInformation board)
		{
			int num = 0;
			foreach (PawnBase pawnBase in base.PlayerOneUnits)
			{
				PawnSeega pawnSeega = (PawnSeega)pawnBase;
				pawnSeega.X = board.PawnInformation[num].X;
				pawnSeega.Y = board.PawnInformation[num].Y;
				pawnSeega.PrevX = board.PawnInformation[num].PrevX;
				pawnSeega.PrevY = board.PawnInformation[num].PrevY;
				pawnSeega.Captured = board.PawnInformation[num].IsCaptured;
				pawnSeega.AISetMovedThisTurn(board.PawnInformation[num].MovedThisTurn);
				num++;
			}
			foreach (PawnBase pawnBase2 in base.PlayerTwoUnits)
			{
				PawnSeega pawnSeega2 = (PawnSeega)pawnBase2;
				pawnSeega2.X = board.PawnInformation[num].X;
				pawnSeega2.Y = board.PawnInformation[num].Y;
				pawnSeega2.PrevX = board.PawnInformation[num].PrevX;
				pawnSeega2.PrevY = board.PawnInformation[num].PrevY;
				pawnSeega2.Captured = board.PawnInformation[num].IsCaptured;
				pawnSeega2.AISetMovedThisTurn(board.PawnInformation[num].MovedThisTurn);
				num++;
			}
			for (int i = 0; i < BoardGameSeega.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameSeega.BoardHeight; j++)
				{
					this.GetTile(i, j).PawnOnTile = board.TileInformation[i, j].PawnOnTile;
				}
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0005BBAC File Offset: 0x00059DAC
		public TileBase GetTile(int x, int y)
		{
			return base.Tiles[y * BoardGameSeega.BoardWidth + x];
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0005BBBE File Offset: 0x00059DBE
		private void SetTile(TileBase tile, int x, int y)
		{
			base.Tiles[y * BoardGameSeega.BoardWidth + x] = tile;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0005BBD1 File Offset: 0x00059DD1
		private bool IsCentralTile(Tile2D tile)
		{
			return tile.X == 2 && tile.Y == 2;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0005BBE7 File Offset: 0x00059DE7
		private bool IsOnCentralTile(PawnSeega pawn)
		{
			return pawn.X == 2 && pawn.Y == 2;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0005BC00 File Offset: 0x00059E00
		private void PreplaceUnits()
		{
			this.MovePawnToTileDelayed(base.PlayerOneUnits[0], this.GetTile(0, 2), false, false, 0.55f);
			this.MovePawnToTileDelayed(base.PlayerTwoUnits[0], this.GetTile(2, 0), false, false, 0.70000005f);
			this.MovePawnToTileDelayed(base.PlayerOneUnits[1], this.GetTile(4, 2), false, false, 0.85f);
			this.MovePawnToTileDelayed(base.PlayerTwoUnits[1], this.GetTile(2, 4), false, false, 1f);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0005BC94 File Offset: 0x00059E94
		private void RestoreStartingBoard()
		{
			int num = 0;
			foreach (PawnBase pawnBase in base.PlayerOneUnits)
			{
				PawnSeega pawnSeega = (PawnSeega)pawnBase;
				if (this._startState.PawnInformation[num].X != -1)
				{
					int x = this._startState.PawnInformation[num].X;
					int y = this._startState.PawnInformation[num].Y;
					if (this._startState.PawnInformation[num].X != pawnSeega.X && this._startState.PawnInformation[num].Y != pawnSeega.Y)
					{
						pawnSeega.Reset();
						TileBase tile = this.GetTile(x, y);
						this.MovePawnToTile(pawnSeega, tile, false, true);
					}
				}
				else if (!pawnSeega.Entity.GlobalPosition.NearlyEquals(in this._startState.PawnInformation[num].Position, 1E-05f))
				{
					if (pawnSeega.X != -1 && this.GetTile(pawnSeega.X, pawnSeega.Y).PawnOnTile == pawnSeega)
					{
						this.GetTile(pawnSeega.X, pawnSeega.Y).PawnOnTile = null;
					}
					pawnSeega.Reset();
					pawnSeega.AddGoalPosition(this._startState.PawnInformation[num].Position);
					pawnSeega.MovePawnToGoalPositions(false, 0.5f, false);
				}
				num++;
			}
			foreach (PawnBase pawnBase2 in base.PlayerTwoUnits)
			{
				PawnSeega pawnSeega2 = (PawnSeega)pawnBase2;
				if (this._startState.PawnInformation[num].X != -1)
				{
					int x2 = this._startState.PawnInformation[num].X;
					int y2 = this._startState.PawnInformation[num].Y;
					if (this._startState.PawnInformation[num].X != pawnSeega2.X && this._startState.PawnInformation[num].Y != pawnSeega2.Y)
					{
						pawnSeega2.Reset();
						TileBase tile2 = this.GetTile(x2, y2);
						this.MovePawnToTile(pawnSeega2, tile2, false, true);
					}
				}
				else
				{
					if (pawnSeega2.X != -1 && this.GetTile(pawnSeega2.X, pawnSeega2.Y).PawnOnTile == pawnSeega2)
					{
						this.GetTile(pawnSeega2.X, pawnSeega2.Y).PawnOnTile = null;
					}
					pawnSeega2.Reset();
					pawnSeega2.AddGoalPosition(this._startState.PawnInformation[num].Position);
					pawnSeega2.MovePawnToGoalPositions(false, 0.5f, false);
				}
				num++;
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0005BFC4 File Offset: 0x0005A1C4
		private bool CheckPlacementStageOver()
		{
			if (!this._placementStageOver)
			{
				bool flag = false;
				using (List<PawnBase>.Enumerator enumerator = base.PlayerOneUnits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!((PawnSeega)enumerator.Current).IsPlaced)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					using (List<PawnBase>.Enumerator enumerator = base.PlayerTwoUnits.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!((PawnSeega)enumerator.Current).IsPlaced)
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					this._placementStageOver = true;
				}
			}
			return this._placementStageOver;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0005C08C File Offset: 0x0005A28C
		private bool CheckIfPawnCapturedEnemyPawn(PawnSeega pawn, bool aiSimulation, Tile2D victimTile, TileBase helperTile, bool setCaptured)
		{
			bool flag = false;
			PawnBase pawnOnTile = victimTile.PawnOnTile;
			if (pawnOnTile != null && !this.IsCentralTile(victimTile) && pawnOnTile.PlayerOne != pawn.PlayerOne)
			{
				PawnBase pawnOnTile2 = helperTile.PawnOnTile;
				if (pawnOnTile2 != null && pawnOnTile2.PlayerOne == pawn.PlayerOne)
				{
					flag = true;
					if (setCaptured)
					{
						this.SetPawnCaptured(pawnOnTile, aiSimulation);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0005C0EC File Offset: 0x0005A2EC
		private int CheckIfPawnCaptures(PawnSeega pawn, bool aiSimulation = false, bool setCaptured = true)
		{
			int num = 0;
			int x = pawn.X;
			int y = pawn.Y;
			if (x > 1 && this.CheckIfPawnCapturedEnemyPawn(pawn, aiSimulation, this.GetTile(x - 1, y) as Tile2D, this.GetTile(x - 2, y), setCaptured))
			{
				num++;
			}
			if (x < BoardGameSeega.BoardWidth - 2 && this.CheckIfPawnCapturedEnemyPawn(pawn, aiSimulation, this.GetTile(x + 1, y) as Tile2D, this.GetTile(x + 2, y), setCaptured))
			{
				num++;
			}
			if (y > 1 && this.CheckIfPawnCapturedEnemyPawn(pawn, aiSimulation, this.GetTile(x, y - 1) as Tile2D, this.GetTile(x, y - 2), setCaptured))
			{
				num++;
			}
			if (y < BoardGameSeega.BoardHeight - 2 && this.CheckIfPawnCapturedEnemyPawn(pawn, aiSimulation, this.GetTile(x, y + 1) as Tile2D, this.GetTile(x, y + 2), setCaptured))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0005C1C8 File Offset: 0x0005A3C8
		private void CheckIfPlayerIsStuck(bool playerOne)
		{
			List<List<Move>> list = this.CalculateAllValidMoves(playerOne ? BoardGameSide.Player : BoardGameSide.AI);
			if (!base.HasMovesAvailable(ref list))
			{
				MBDebug.Print("Player has no available moves! " + (playerOne ? "PLAYER ONE" : "PLAYER TWO"), 0, Debug.DebugColor.White, 17592186044416UL);
				this._blockingPawns = this.GetBlockingPawns(playerOne);
				this.FocusBlockingPawns();
				if (playerOne)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=GwHPEgsv}You can't move. Chose one of the opponent's pawns to remove and make a move", null).ToString()));
				}
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0005C248 File Offset: 0x0005A448
		private void FocusBlockingPawns()
		{
			foreach (KeyValuePair<PawnBase, int> keyValuePair in this._blockingPawns)
			{
				keyValuePair.Key.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnSelectedFactor);
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0005C2B4 File Offset: 0x0005A4B4
		private void UnfocusBlockingPawns()
		{
			foreach (KeyValuePair<PawnBase, int> keyValuePair in this._blockingPawns)
			{
				keyValuePair.Key.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnUnselectedFactor);
			}
			this._blockingPawns.Clear();
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0005C328 File Offset: 0x0005A528
		private BoardGameSeega.BarrierInfo CheckIfBarrier(Vec2i pawnNewPos)
		{
			if (pawnNewPos.X > 0 && pawnNewPos.X < BoardGameSeega.BoardWidth - 1)
			{
				PawnBase pawnOnTile = this.GetTile(pawnNewPos.X, 0).PawnOnTile;
				if (pawnOnTile != null)
				{
					for (int i = 1; i < BoardGameSeega.BoardHeight; i++)
					{
						PawnBase pawnOnTile2 = this.GetTile(pawnNewPos.X, i).PawnOnTile;
						if (pawnOnTile2 == null || pawnOnTile2.PlayerOne != pawnOnTile.PlayerOne)
						{
							break;
						}
						if (i == BoardGameSeega.BoardHeight - 1)
						{
							return new BoardGameSeega.BarrierInfo(false, pawnNewPos.X, pawnOnTile.PlayerOne);
						}
					}
				}
			}
			if (pawnNewPos.Y > 0 && pawnNewPos.Y < BoardGameSeega.BoardHeight - 1)
			{
				PawnBase pawnOnTile3 = this.GetTile(0, pawnNewPos.Y).PawnOnTile;
				if (pawnOnTile3 != null)
				{
					for (int j = 1; j < BoardGameSeega.BoardWidth; j++)
					{
						PawnBase pawnOnTile4 = this.GetTile(j, pawnNewPos.Y).PawnOnTile;
						if (pawnOnTile4 == null || pawnOnTile4.PlayerOne != pawnOnTile3.PlayerOne)
						{
							break;
						}
						if (j == BoardGameSeega.BoardWidth - 1)
						{
							return new BoardGameSeega.BarrierInfo(true, pawnNewPos.Y, pawnOnTile3.PlayerOne);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0005C440 File Offset: 0x0005A640
		private bool CheckIfUnitsIsolatedByBarrier(Vec2i pawnNewPos)
		{
			BoardGameSeega.BarrierInfo barrierInfo = this.CheckIfBarrier(pawnNewPos);
			if (barrierInfo != null)
			{
				bool flag = false;
				bool flag2 = false;
				foreach (PawnBase pawnBase in (barrierInfo.PlayerOne ? base.PlayerTwoUnits : base.PlayerOneUnits))
				{
					PawnSeega pawnSeega = (PawnSeega)pawnBase;
					if (!pawnSeega.Captured)
					{
						if ((barrierInfo.IsHorizontal ? pawnSeega.Y : pawnSeega.X) > barrierInfo.Position)
						{
							flag = true;
						}
						if ((barrierInfo.IsHorizontal ? pawnSeega.Y : pawnSeega.X) < barrierInfo.Position)
						{
							flag2 = true;
						}
					}
				}
				return !flag || !flag2;
			}
			return false;
		}

		// Token: 0x04000549 RID: 1353
		private const int CentralTileX = 2;

		// Token: 0x0400054A RID: 1354
		private const int CentralTileY = 2;

		// Token: 0x0400054B RID: 1355
		public static readonly int BoardWidth = 5;

		// Token: 0x0400054C RID: 1356
		public static readonly int BoardHeight = 5;

		// Token: 0x0400054D RID: 1357
		private Dictionary<PawnBase, int> _blockingPawns = new Dictionary<PawnBase, int>();

		// Token: 0x0400054E RID: 1358
		private BoardGameSeega.BoardInformation _startState;

		// Token: 0x0400054F RID: 1359
		private bool _placementStageOver;

		// Token: 0x0200022A RID: 554
		public class BarrierInfo
		{
			// Token: 0x0600145B RID: 5211 RVA: 0x0007A351 File Offset: 0x00078551
			public BarrierInfo(bool isHor, int pos, bool playerOne)
			{
				this.IsHorizontal = isHor;
				this.Position = pos;
				this.PlayerOne = playerOne;
			}

			// Token: 0x040009B6 RID: 2486
			public bool IsHorizontal;

			// Token: 0x040009B7 RID: 2487
			public int Position;

			// Token: 0x040009B8 RID: 2488
			public bool PlayerOne;
		}

		// Token: 0x0200022B RID: 555
		public struct BoardInformation
		{
			// Token: 0x0600145C RID: 5212 RVA: 0x0007A36E File Offset: 0x0007856E
			public BoardInformation(ref BoardGameSeega.PawnInformation[] pawns, ref TileBaseInformation[,] tiles)
			{
				this.PawnInformation = pawns;
				this.TileInformation = tiles;
			}

			// Token: 0x040009B9 RID: 2489
			public readonly BoardGameSeega.PawnInformation[] PawnInformation;

			// Token: 0x040009BA RID: 2490
			public readonly TileBaseInformation[,] TileInformation;
		}

		// Token: 0x0200022C RID: 556
		public struct PawnInformation
		{
			// Token: 0x0600145D RID: 5213 RVA: 0x0007A380 File Offset: 0x00078580
			public PawnInformation(int x, int y, int prevX, int prevY, bool movedThisTurn, bool captured, Vec3 position)
			{
				this.X = x;
				this.Y = y;
				this.PrevX = prevX;
				this.PrevY = prevY;
				this.MovedThisTurn = movedThisTurn;
				this.IsCaptured = captured;
				this.Position = position;
			}

			// Token: 0x040009BB RID: 2491
			public readonly int X;

			// Token: 0x040009BC RID: 2492
			public readonly int Y;

			// Token: 0x040009BD RID: 2493
			public readonly int PrevX;

			// Token: 0x040009BE RID: 2494
			public readonly int PrevY;

			// Token: 0x040009BF RID: 2495
			public readonly bool MovedThisTurn;

			// Token: 0x040009C0 RID: 2496
			public readonly bool IsCaptured;

			// Token: 0x040009C1 RID: 2497
			public readonly Vec3 Position;
		}
	}
}
