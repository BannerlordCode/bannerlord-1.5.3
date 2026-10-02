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
	// Token: 0x020000F1 RID: 241
	public class BoardGameMuTorere : BoardGameBase
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00058C5A File Offset: 0x00056E5A
		public override int TileCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00058C5E File Offset: 0x00056E5E
		protected override bool RotateBoard
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00058C61 File Offset: 0x00056E61
		protected override bool PreMovementStagePresent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00058C64 File Offset: 0x00056E64
		protected override bool DiceRollRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00058C67 File Offset: 0x00056E67
		public BoardGameMuTorere(MissionBoardGameLogic mission, PlayerTurn startingPlayer)
			: base(mission, new TextObject("{=5siAbi69}Mu Torere", null), startingPlayer)
		{
			this.PawnUnselectedFactor = 4288711820U;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00058C88 File Offset: 0x00056E88
		public override void InitializeUnits()
		{
			base.PlayerOneUnits.Clear();
			base.PlayerTwoUnits.Clear();
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			for (int i = 0; i < 4; i++)
			{
				GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("player_one_unit_" + i);
				list.Add(base.InitializeUnit(new PawnMuTorere(gameEntity, base.PlayerWhoStarted == PlayerTurn.PlayerOne)));
			}
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int j = 0; j < 4; j++)
			{
				GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("player_two_unit_" + j);
				list2.Add(base.InitializeUnit(new PawnMuTorere(gameEntity2, base.PlayerWhoStarted > PlayerTurn.PlayerOne)));
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00058D70 File Offset: 0x00056F70
		public override void InitializeTiles()
		{
			if (base.Tiles == null)
			{
				base.Tiles = new TileBase[this.TileCount];
			}
			int x;
			IEnumerable<GameEntity> enumerable = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("tile_"))
				select x;
			IEnumerable<GameEntity> enumerable2 = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("decal_"))
				select x;
			int num;
			for (x = 0; x < this.TileCount; x = num)
			{
				GameEntity gameEntity = enumerable.Single<GameEntity>((GameEntity e) => e.HasTag("tile_" + x));
				BoardGameDecal firstScriptOfType = enumerable2.Single<GameEntity>((GameEntity e) => e.HasTag("decal_" + x)).GetFirstScriptOfType<BoardGameDecal>();
				num = x;
				int num2;
				int num3;
				if (num != 0)
				{
					if (num != 1)
					{
						if (num != 8)
						{
							num2 = x - 1;
							num3 = x + 1;
						}
						else
						{
							num2 = 7;
							num3 = 1;
						}
					}
					else
					{
						num2 = 8;
						num3 = 2;
					}
				}
				else
				{
					num3 = (num2 = -1);
				}
				base.Tiles[x] = new TileMuTorere(gameEntity, firstScriptOfType, x, num2, num3);
				gameEntity.CreateVariableRatePhysics(true);
				num = x + 1;
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00058EC1 File Offset: 0x000570C1
		public override void InitializeCapturedUnitsZones()
		{
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00058EC3 File Offset: 0x000570C3
		public override void InitializeSound()
		{
			PawnBase.PawnMoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/move_stone");
			PawnBase.PawnSelectSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/pick_stone");
			PawnBase.PawnTapSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/drop_wood");
			PawnBase.PawnRemoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/out_stone");
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00058F01 File Offset: 0x00057101
		public override void Reset()
		{
			base.Reset();
			this.PreplaceUnits();
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00058F10 File Offset: 0x00057110
		public override List<Move> CalculateValidMoves(PawnBase pawn)
		{
			List<Move> list = new List<Move>();
			PawnMuTorere pawnMuTorere = pawn as PawnMuTorere;
			if (pawnMuTorere != null)
			{
				TileMuTorere tileMuTorere = this.FindAvailableTile() as TileMuTorere;
				if (pawnMuTorere.X == 0)
				{
					Move move;
					move.Unit = pawn;
					move.GoalTile = tileMuTorere;
					list.Add(move);
				}
				else if (tileMuTorere.X != 0)
				{
					if (pawnMuTorere.X == tileMuTorere.XLeftTile || pawnMuTorere.X == tileMuTorere.XRightTile)
					{
						Move move2;
						move2.Unit = pawn;
						move2.GoalTile = tileMuTorere;
						list.Add(move2);
					}
				}
				else
				{
					TileMuTorere tileMuTorere2 = this.FindTileByCoordinate(pawnMuTorere.X);
					PawnBase pawnOnTile = base.Tiles[tileMuTorere2.XLeftTile].PawnOnTile;
					PawnBase pawnOnTile2 = base.Tiles[tileMuTorere2.XRightTile].PawnOnTile;
					if (pawnOnTile.PlayerOne != pawnMuTorere.PlayerOne || pawnOnTile2.PlayerOne != pawnMuTorere.PlayerOne)
					{
						Move move3;
						move3.Unit = pawn;
						move3.GoalTile = tileMuTorere;
						list.Add(move3);
					}
				}
			}
			return list;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0005900C File Offset: 0x0005720C
		protected override PawnBase SelectPawn(PawnBase pawn)
		{
			if (base.PlayerTurn == PlayerTurn.PlayerOne)
			{
				if (pawn.PlayerOne)
				{
					this.SelectedUnit = pawn;
				}
			}
			else if (base.AIOpponent == null && !pawn.PlayerOne)
			{
				this.SelectedUnit = pawn;
			}
			return pawn;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00059040 File Offset: 0x00057240
		protected override void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			base.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, delay);
			TileMuTorere tileMuTorere = tile as TileMuTorere;
			PawnMuTorere pawnMuTorere = pawn as PawnMuTorere;
			if (tileMuTorere.PawnOnTile == null && pawnMuTorere != null)
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
				if (pawnMuTorere.X != -1)
				{
					base.Tiles[pawnMuTorere.X].PawnOnTile = null;
				}
				tileMuTorere.PawnOnTile = pawnMuTorere;
				pawnMuTorere.MovingToDifferentTile = pawnMuTorere.X != tileMuTorere.X;
				pawnMuTorere.X = tileMuTorere.X;
				Vec3 globalPosition = tileMuTorere.Entity.GlobalPosition;
				pawnMuTorere.AddGoalPosition(globalPosition);
				pawnMuTorere.MovePawnToGoalPositionsDelayed(instantMove, 0.6f, this.JustStoppedDraggingUnit, delay);
				if (pawnMuTorere == this.SelectedUnit)
				{
					this.SelectedUnit = null;
				}
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00059138 File Offset: 0x00057338
		protected override void SwitchPlayerTurn()
		{
			if (base.PlayerTurn == PlayerTurn.PlayerOneWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerTwo;
			}
			else if (base.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerOne;
			}
			this.CheckGameEnded();
			base.SwitchPlayerTurn();
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0005916C File Offset: 0x0005736C
		protected override bool CheckGameEnded()
		{
			bool flag = false;
			List<List<Move>> list = this.CalculateAllValidMoves((base.PlayerTurn == PlayerTurn.PlayerOne) ? BoardGameSide.Player : BoardGameSide.AI);
			if (base.GetTotalMovesAvailable(ref list) <= 0)
			{
				if (base.PlayerTurn == PlayerTurn.PlayerOne)
				{
					base.OnDefeat("str_boardgame_defeat_message");
					this.ReadyToPlay = false;
					flag = true;
				}
				else if (base.PlayerTurn == PlayerTurn.PlayerTwo)
				{
					base.OnVictory("str_boardgame_victory_message");
					this.ReadyToPlay = false;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000591D5 File Offset: 0x000573D5
		protected override void OnAfterBoardSetUp()
		{
			this.ReadyToPlay = true;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000591E0 File Offset: 0x000573E0
		public TileMuTorere FindTileByCoordinate(int x)
		{
			TileMuTorere tileMuTorere = null;
			for (int i = 0; i < this.TileCount; i++)
			{
				TileMuTorere tileMuTorere2 = base.Tiles[i] as TileMuTorere;
				if (tileMuTorere2.X == x)
				{
					tileMuTorere = tileMuTorere2;
				}
			}
			return tileMuTorere;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0005921C File Offset: 0x0005741C
		public BoardGameMuTorere.BoardInformation TakePawnsSnapshot()
		{
			BoardGameMuTorere.PawnInformation[] array = new BoardGameMuTorere.PawnInformation[base.PlayerOneUnits.Count + base.PlayerTwoUnits.Count];
			TileBaseInformation[] array2 = new TileBaseInformation[this.TileCount];
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				PawnMuTorere pawnMuTorere = (PawnMuTorere)pawnBase;
				BoardGameMuTorere.PawnInformation pawnInformation = new BoardGameMuTorere.PawnInformation(pawnMuTorere.X);
				array[num++] = pawnInformation;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnMuTorere pawnMuTorere2 = (PawnMuTorere)pawnBase2;
				BoardGameMuTorere.PawnInformation pawnInformation2 = new BoardGameMuTorere.PawnInformation(pawnMuTorere2.X);
				array[num++] = pawnInformation2;
			}
			for (int i = 0; i < this.TileCount; i++)
			{
				array2[i] = new TileBaseInformation(ref base.Tiles[i].PawnOnTile);
			}
			return new BoardGameMuTorere.BoardInformation(ref array, ref array2);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0005936C File Offset: 0x0005756C
		public void UndoMove(ref BoardGameMuTorere.BoardInformation board)
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				((PawnMuTorere)pawnBase).X = board.PawnInformation[num++].X;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				((PawnMuTorere)pawnBase2).X = board.PawnInformation[num++].X;
			}
			for (int i = 0; i < this.TileCount; i++)
			{
				base.Tiles[i].PawnOnTile = board.TileInformation[i].PawnOnTile;
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00059480 File Offset: 0x00057680
		public void AIMakeMove(Move move)
		{
			TileMuTorere tileMuTorere = move.GoalTile as TileMuTorere;
			PawnMuTorere pawnMuTorere = move.Unit as PawnMuTorere;
			base.Tiles[pawnMuTorere.X].PawnOnTile = null;
			tileMuTorere.PawnOnTile = pawnMuTorere;
			pawnMuTorere.X = tileMuTorere.X;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000594CC File Offset: 0x000576CC
		public TileBase FindAvailableTile()
		{
			foreach (TileBase tileBase in base.Tiles)
			{
				if (tileBase.PawnOnTile == null)
				{
					return tileBase;
				}
			}
			return null;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00059500 File Offset: 0x00057700
		private void PreplaceUnits()
		{
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int i = 0; i < 4; i++)
			{
				this.MovePawnToTileDelayed(list[i], base.Tiles[i + 1], false, false, 0.15f * (float)(i + 1) + 0.25f);
				this.MovePawnToTileDelayed(list2[i], base.Tiles[8 - i], false, false, 0.15f * (float)(i + 1) + 0.5f);
			}
		}

		// Token: 0x04000541 RID: 1345
		public const int WhitePawnCount = 4;

		// Token: 0x04000542 RID: 1346
		public const int BlackPawnCount = 4;

		// Token: 0x02000222 RID: 546
		public struct BoardInformation
		{
			// Token: 0x06001445 RID: 5189 RVA: 0x0007A15D File Offset: 0x0007835D
			public BoardInformation(ref BoardGameMuTorere.PawnInformation[] pawns, ref TileBaseInformation[] tiles)
			{
				this.PawnInformation = pawns;
				this.TileInformation = tiles;
			}

			// Token: 0x0400099E RID: 2462
			public readonly BoardGameMuTorere.PawnInformation[] PawnInformation;

			// Token: 0x0400099F RID: 2463
			public readonly TileBaseInformation[] TileInformation;
		}

		// Token: 0x02000223 RID: 547
		public struct PawnInformation
		{
			// Token: 0x06001446 RID: 5190 RVA: 0x0007A16F File Offset: 0x0007836F
			public PawnInformation(int x)
			{
				this.X = x;
			}

			// Token: 0x040009A0 RID: 2464
			public readonly int X;
		}
	}
}
