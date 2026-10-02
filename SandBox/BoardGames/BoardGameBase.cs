using System;
using System.Collections.Generic;
using SandBox.BoardGames.AI;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.BoardGames
{
	// Token: 0x020000EF RID: 239
	public abstract class BoardGameBase
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000B88 RID: 2952
		public abstract int TileCount { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000B89 RID: 2953
		protected abstract bool RotateBoard { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000B8A RID: 2954
		protected abstract bool PreMovementStagePresent { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000B8B RID: 2955
		protected abstract bool DiceRollRequired { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00055A33 File Offset: 0x00053C33
		protected virtual int UnitsToPlacePerTurnInPreMovementStage
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00055A36 File Offset: 0x00053C36
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x00055A3E File Offset: 0x00053C3E
		protected virtual PawnBase SelectedUnit
		{
			get
			{
				return this._selectedUnit;
			}
			set
			{
				this.OnBeforeSelectedUnitChanged(this._selectedUnit, value);
				this._selectedUnit = value;
				this.OnAfterSelectedUnitChanged();
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00055A5A File Offset: 0x00053C5A
		public TextObject Name { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00055A62 File Offset: 0x00053C62
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00055A6A File Offset: 0x00053C6A
		public bool InPreMovementStage { get; protected set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00055A73 File Offset: 0x00053C73
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00055A7B File Offset: 0x00053C7B
		public TileBase[] Tiles { get; protected set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00055A84 File Offset: 0x00053C84
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00055A8C File Offset: 0x00053C8C
		public List<PawnBase> PlayerOneUnits { get; protected set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00055A95 File Offset: 0x00053C95
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00055A9D File Offset: 0x00053C9D
		public List<PawnBase> PlayerTwoUnits { get; protected set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00055AA6 File Offset: 0x00053CA6
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00055AAE File Offset: 0x00053CAE
		public int LastDice { get; protected set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00055AB7 File Offset: 0x00053CB7
		public bool IsReady
		{
			get
			{
				return this.ReadyToPlay && !this.SettingUpBoard;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00055ACC File Offset: 0x00053CCC
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x00055AD4 File Offset: 0x00053CD4
		public PlayerTurn PlayerWhoStarted { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00055ADD File Offset: 0x00053CDD
		// (set) Token: 0x06000B9E RID: 2974 RVA: 0x00055AE5 File Offset: 0x00053CE5
		public GameOverEnum GameOverInfo { get; private set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00055AEE File Offset: 0x00053CEE
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x00055AF6 File Offset: 0x00053CF6
		public PlayerTurn PlayerTurn { get; protected set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00055AFF File Offset: 0x00053CFF
		protected IInputContext InputManager
		{
			get
			{
				return this.MissionHandler.Mission.InputManager;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00055B11 File Offset: 0x00053D11
		protected List<PawnBase> PawnSelectFilter { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00055B19 File Offset: 0x00053D19
		protected BoardGameAIBase AIOpponent
		{
			get
			{
				return this.MissionHandler.AIOpponent;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00055B26 File Offset: 0x00053D26
		private bool DiceRolled
		{
			get
			{
				return this.LastDice != -1;
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00055B34 File Offset: 0x00053D34
		protected BoardGameBase(MissionBoardGameLogic mission, TextObject name, PlayerTurn startingPlayer)
		{
			this.Name = name;
			this.MissionHandler = mission;
			this.SetStartingPlayer(startingPlayer);
			this.PlayerOnePool = new CapturedPawnsPool();
			this.PlayerTwoPool = new CapturedPawnsPool();
			this.PlayerOneUnits = new List<PawnBase>();
			this.PlayerTwoUnits = new List<PawnBase>();
			this.PawnSelectFilter = new List<PawnBase>();
		}

		// Token: 0x06000BA6 RID: 2982
		public abstract void InitializeUnits();

		// Token: 0x06000BA7 RID: 2983
		public abstract void InitializeTiles();

		// Token: 0x06000BA8 RID: 2984
		public abstract void InitializeSound();

		// Token: 0x06000BA9 RID: 2985
		public abstract List<Move> CalculateValidMoves(PawnBase pawn);

		// Token: 0x06000BAA RID: 2986
		protected abstract PawnBase SelectPawn(PawnBase pawn);

		// Token: 0x06000BAB RID: 2987
		protected abstract bool CheckGameEnded();

		// Token: 0x06000BAC RID: 2988
		protected abstract void OnAfterBoardSetUp();

		// Token: 0x06000BAD RID: 2989 RVA: 0x00055BB3 File Offset: 0x00053DB3
		protected virtual void OnAfterBoardRotated()
		{
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00055BB5 File Offset: 0x00053DB5
		protected virtual void OnBeforeEndTurn()
		{
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00055BB7 File Offset: 0x00053DB7
		public virtual void RollDice()
		{
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00055BB9 File Offset: 0x00053DB9
		protected virtual void UpdateAllTilesPositions()
		{
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00055BBB File Offset: 0x00053DBB
		public virtual void InitializeDiceBoard()
		{
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00055BC0 File Offset: 0x00053DC0
		public virtual void Reset()
		{
			this.PlayerOnePool.PawnCount = 0;
			this.PlayerTwoPool.PawnCount = 0;
			this.ClearValidMoves();
			this.SelectedUnit = null;
			this.PawnSelectFilter.Clear();
			this.GameOverInfo = GameOverEnum.GameStillInProgress;
			this._draggingSelectedUnit = false;
			this.JustStoppedDraggingUnit = false;
			this._draggingTimer = 0f;
			BoardGameAIBase aiopponent = this.MissionHandler.AIOpponent;
			if (aiopponent != null)
			{
				aiopponent.ResetThinking();
			}
			this.ReadyToPlay = false;
			this._firstTickAfterReady = true;
			this._rotationCompleted = !this.RotateBoard;
			this.SettingUpBoard = true;
			this.UnfocusAllPawns();
			for (int i = 0; i < this.TileCount; i++)
			{
				this.Tiles[i].Reset();
			}
			this.MovesLeftToEndTurn = (this.PreMovementStagePresent ? this.UnitsToPlacePerTurnInPreMovementStage : 1);
			this.LastDice = -1;
			this._waitingAIForfeitResponse = false;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00055CA0 File Offset: 0x00053EA0
		protected virtual void OnPawnArrivesGoalPosition(PawnBase pawn, Vec3 prevPos, Vec3 currentPos)
		{
			if (this.IsReady && pawn.IsPlaced && !pawn.Captured && pawn.MovingToDifferentTile)
			{
				this.MovesLeftToEndTurn--;
			}
			pawn.MovingToDifferentTile = false;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00055CD7 File Offset: 0x00053ED7
		protected virtual void HandlePreMovementStage(float dt)
		{
			Debug.FailedAssert("HandlePreMovementStage is not implemented for " + this.MissionHandler.CurrentBoardGame, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameBase.cs", "HandlePreMovementStage", 293);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00055D08 File Offset: 0x00053F08
		public virtual void InitializeCapturedUnitsZones()
		{
			this.PlayerOnePool.Entity = Mission.Current.Scene.FindEntityWithTag((this.PlayerWhoStarted == PlayerTurn.PlayerOne) ? "captured_pawns_pool_1" : "captured_pawns_pool_2");
			this.PlayerOnePool.PawnCount = 0;
			this.PlayerTwoPool.Entity = Mission.Current.Scene.FindEntityWithTag((this.PlayerWhoStarted == PlayerTurn.PlayerOne) ? "captured_pawns_pool_2" : "captured_pawns_pool_1");
			this.PlayerTwoPool.PawnCount = 0;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00055D89 File Offset: 0x00053F89
		protected virtual void HandlePreMovementStageAI(Move move)
		{
			Debug.FailedAssert("HandlePreMovementStageAI is not implemented for " + this.MissionHandler.CurrentBoardGame, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameBase.cs", "HandlePreMovementStageAI", 311);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00055DB9 File Offset: 0x00053FB9
		public virtual void SetPawnCaptured(PawnBase pawn, bool fake = false)
		{
			pawn.Captured = true;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00055DC4 File Offset: 0x00053FC4
		public virtual List<List<Move>> CalculateAllValidMoves(BoardGameSide side)
		{
			List<List<Move>> list = new List<List<Move>>(100);
			foreach (PawnBase pawnBase in ((side == BoardGameSide.AI) ? this.PlayerTwoUnits : this.PlayerOneUnits))
			{
				list.Add(this.CalculateValidMoves(pawnBase));
			}
			return list;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00055E34 File Offset: 0x00054034
		protected virtual void SwitchPlayerTurn()
		{
			this.MissionHandler.Handler.SwitchTurns();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00055E46 File Offset: 0x00054046
		protected virtual void MovePawnToTile(PawnBase pawn, TileBase tile, bool instantMove = false, bool displayMessage = true)
		{
			this.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, 0f);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00055E58 File Offset: 0x00054058
		protected virtual void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			this.ClearValidMoves();
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00055E60 File Offset: 0x00054060
		protected virtual void OnAfterDiceRollAnimation()
		{
			if (this.LastDice != -1)
			{
				this.MissionHandler.Handler.DiceRoll(this.LastDice);
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00055E81 File Offset: 0x00054081
		public void SetUserRay(Vec3 rayBegin, Vec3 rayEnd)
		{
			this._userRayBegin = rayBegin;
			this._userRayEnd = rayEnd;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00055E94 File Offset: 0x00054094
		public void SetStartingPlayer(PlayerTurn player)
		{
			this.HasToMovePawnsAcross = this.PlayerWhoStarted != player;
			if (player == PlayerTurn.PlayerOne)
			{
				this._rotationTarget = 0f;
			}
			else if (player == PlayerTurn.PlayerTwo)
			{
				this._rotationTarget = 3.1415927f;
			}
			else
			{
				Debug.FailedAssert("Unexpected starting player caught: " + player, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameBase.cs", "SetStartingPlayer", 382);
			}
			this.PlayerWhoStarted = player;
			this.PlayerTurn = player;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00055F08 File Offset: 0x00054108
		public void SetGameOverInfo(GameOverEnum info)
		{
			this.GameOverInfo = info;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00055F14 File Offset: 0x00054114
		public bool HasMovesAvailable(ref List<List<Move>> moves)
		{
			foreach (List<Move> list in moves)
			{
				if (list != null && list.Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00055F70 File Offset: 0x00054170
		public int GetTotalMovesAvailable(ref List<List<Move>> moves)
		{
			int num = 0;
			foreach (List<Move> list in moves)
			{
				if (list != null)
				{
					num += list.Count;
				}
			}
			return num;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00055FC8 File Offset: 0x000541C8
		public void PlayDiceRollSound()
		{
			Vec3 globalPosition = this.DiceBoard.GlobalPosition;
			this.MissionHandler.Mission.MakeSound(this.DiceRollSoundCodeID, globalPosition, true, false, -1, -1);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00055FFC File Offset: 0x000541FC
		public int GetPlayerOneUnitsAlive()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = this.PlayerOneUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00056058 File Offset: 0x00054258
		public int GetPlayerTwoUnitsAlive()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = this.PlayerTwoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000560B4 File Offset: 0x000542B4
		public int GetPlayerOneUnitsDead()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = this.PlayerOneUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00056110 File Offset: 0x00054310
		public int GetPlayerTwoUnitsDead()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = this.PlayerTwoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0005616C File Offset: 0x0005436C
		public void Initialize()
		{
			this.BoardEntity = Mission.Current.Scene.FindEntityWithTag("boardgame");
			this.InitializeUnits();
			this.InitializeTiles();
			this.InitializeCapturedUnitsZones();
			this.InitializeDiceBoard();
			this.InitializeSound();
			this.Reset();
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000561AC File Offset: 0x000543AC
		protected void RemovePawnFromBoard(PawnBase pawn, float speed, bool instantMove = false)
		{
			CapturedPawnsPool capturedPawnsPool = (pawn.PlayerOne ? this.PlayerOnePool : this.PlayerTwoPool);
			IEnumerable<GameEntity> children = capturedPawnsPool.Entity.GetChildren();
			GameEntity gameEntity = null;
			foreach (GameEntity gameEntity2 in children)
			{
				if (gameEntity2.HasTag("pawn_" + capturedPawnsPool.PawnCount))
				{
					gameEntity = gameEntity2;
					break;
				}
			}
			capturedPawnsPool.PawnCount++;
			Vec3 origin = gameEntity.GetGlobalFrame().origin;
			float num = pawn.Entity.GlobalPosition.z - origin.z;
			float num2 = 0.001f;
			if (num > num2)
			{
				Vec3 vec = origin;
				vec.z = pawn.Entity.GlobalPosition.z;
				pawn.AddGoalPosition(vec);
			}
			else if (num < -num2)
			{
				Vec3 globalPosition = pawn.Entity.GlobalPosition;
				globalPosition.z = origin.z;
				pawn.AddGoalPosition(globalPosition);
			}
			pawn.AddGoalPosition(origin);
			pawn.MovePawnToGoalPositions(instantMove, speed, false);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000562D4 File Offset: 0x000544D4
		public bool Tick(float dt)
		{
			foreach (PawnBase pawnBase in this.PlayerOneUnits)
			{
				pawnBase.Tick(dt);
			}
			foreach (PawnBase pawnBase2 in this.PlayerTwoUnits)
			{
				pawnBase2.Tick(dt);
			}
			for (int i = 0; i < this.TileCount; i++)
			{
				this.Tiles[i].Tick(dt);
			}
			if (this.MovingPawnPresent() || !this.DoneSettingUpBoard() || !this.ReadyToPlay)
			{
				return false;
			}
			if (this._firstTickAfterReady)
			{
				this._firstTickAfterReady = false;
				this.MissionHandler.Handler.Activate();
			}
			if (this.IsReady)
			{
				if (this._draggingSelectedUnit)
				{
					Vec3 userRayBegin = this._userRayBegin;
					Vec3 userRayEnd = this._userRayEnd;
					Vec3 globalPosition = this.SelectedUnit.Entity.GlobalPosition;
					float length = (userRayEnd - userRayBegin).Length;
					float num = (globalPosition - userRayBegin).Length / length;
					Vec3 vec = new Vec3(userRayBegin.x + (userRayEnd.x - userRayBegin.x) * num, userRayBegin.y + (userRayEnd.y - userRayBegin.y) * num, this.SelectedUnit.PosBeforeMoving.z + 0.05f, -1f);
					Vec3 vec2 = MBMath.Lerp(globalPosition, vec, 1f, 0.005f);
					this.SelectedUnit.SetPawnAtPosition(vec2);
				}
				if (this.DiceRollAnimationRunning)
				{
					if (this.DiceRollAnimationTimer < 1f)
					{
						this.DiceRollAnimationTimer += dt;
					}
					else
					{
						this.DiceRollAnimationRunning = false;
						this.OnAfterDiceRollAnimation();
					}
				}
				if (this.MovesLeftToEndTurn == 0)
				{
					this.EndTurn();
				}
				else
				{
					this.UpdateTurn(dt);
				}
				this.CheckSwitchPlayerTurn();
				return true;
			}
			return false;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000564E8 File Offset: 0x000546E8
		public void ForceDice(int value)
		{
			this.LastDice = value;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000564F1 File Offset: 0x000546F1
		protected PawnBase InitializeUnit(PawnBase pawnToInit)
		{
			pawnToInit.OnArrivedIntermediateGoalPosition = new Action<PawnBase, Vec3, Vec3>(this.OnPawnArrivesGoalPosition);
			pawnToInit.OnArrivedFinalGoalPosition = new Action<PawnBase, Vec3, Vec3>(this.OnPawnArrivesGoalPosition);
			return pawnToInit;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0005651C File Offset: 0x0005471C
		protected Move HandlePlayerInput(float dt)
		{
			Move move = new Move(null, null);
			if (this.InputManager.IsHotKeyPressed("BoardGamePawnSelect") && !this._draggingSelectedUnit)
			{
				this.JustStoppedDraggingUnit = false;
				PawnBase hoveredPawnIfAny = this.GetHoveredPawnIfAny();
				TileBase hoveredTileIfAny = this.GetHoveredTileIfAny();
				if (hoveredPawnIfAny != null)
				{
					if (this.PawnSelectFilter.Count == 0 || this.PawnSelectFilter.Contains(hoveredPawnIfAny))
					{
						PawnBase selectedUnit = this.SelectedUnit;
						PawnBase pawnBase = this.SelectPawn(hoveredPawnIfAny);
						if (pawnBase.PlayerOne == (this.PlayerTurn == PlayerTurn.PlayerOne) || !pawnBase.PlayerOne == (this.PlayerTurn == PlayerTurn.PlayerTwo))
						{
							if (this.SelectedUnit != null && this.SelectedUnit == selectedUnit)
							{
								this._deselectUnit = true;
							}
						}
						else if (hoveredTileIfAny == null)
						{
							this.SelectedUnit = null;
						}
					}
				}
				else if (hoveredTileIfAny == null)
				{
					this.SelectedUnit = null;
				}
			}
			else if (this.SelectedUnit != null && this.InputManager.IsHotKeyReleased("BoardGamePawnDeselect"))
			{
				if (this._draggingSelectedUnit)
				{
					this._draggingSelectedUnit = false;
					this.JustStoppedDraggingUnit = true;
				}
				else if (this._deselectUnit)
				{
					PawnBase hoveredPawnIfAny2 = this.GetHoveredPawnIfAny();
					if (hoveredPawnIfAny2 != null && hoveredPawnIfAny2 == this.SelectedUnit)
					{
						this.SelectedUnit = null;
						this._deselectUnit = false;
					}
				}
				if (this._validMoves != null)
				{
					this.SelectedUnit.DisableCollisionBody();
					TileBase hoveredTileIfAny2 = this.GetHoveredTileIfAny();
					if (hoveredTileIfAny2 != null && (hoveredTileIfAny2.PawnOnTile == null || hoveredTileIfAny2.PawnOnTile != this.SelectedUnit))
					{
						foreach (Move move2 in this._validMoves)
						{
							if (hoveredTileIfAny2.Entity == move2.GoalTile.Entity)
							{
								move = move2;
							}
						}
					}
					this.SelectedUnit.EnableCollisionBody();
				}
				if (!move.IsValid && this.SelectedUnit != null && this.JustStoppedDraggingUnit)
				{
					this.SelectedUnit.ClearGoalPositions();
					this.SelectedUnit.AddGoalPosition(this.SelectedUnit.PosBeforeMoving);
					this.SelectedUnit.MovePawnToGoalPositions(false, 0.8f, false);
				}
				this._draggingTimer = 0f;
			}
			if (this.SelectedUnit != null && this.InputManager.IsHotKeyDown("BoardGameDragPreview"))
			{
				this._draggingTimer += dt;
				if (this._draggingTimer >= 0.2f)
				{
					this._draggingSelectedUnit = true;
					this._deselectUnit = false;
				}
			}
			return move;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000567AC File Offset: 0x000549AC
		protected PawnBase GetHoveredPawnIfAny()
		{
			PawnBase pawnBase = null;
			float num;
			WeakGameEntity weakGameEntity;
			Mission.Current.Scene.RayCastForClosestEntityOrTerrain(this._userRayBegin, this._userRayEnd, out num, out weakGameEntity, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags);
			if (weakGameEntity.IsValid)
			{
				foreach (PawnBase pawnBase2 in this.PlayerOneUnits)
				{
					if (pawnBase2.Entity.Name.Equals(weakGameEntity.Name))
					{
						pawnBase = pawnBase2;
						break;
					}
				}
				if (pawnBase == null)
				{
					foreach (PawnBase pawnBase3 in this.PlayerTwoUnits)
					{
						if (pawnBase3.Entity.Name.Equals(weakGameEntity.Name))
						{
							pawnBase = pawnBase3;
							break;
						}
					}
				}
			}
			return pawnBase;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000568B0 File Offset: 0x00054AB0
		protected TileBase GetHoveredTileIfAny()
		{
			TileBase tileBase = null;
			float num;
			WeakGameEntity weakGameEntity;
			Mission.Current.Scene.RayCastForClosestEntityOrTerrain(this._userRayBegin, this._userRayEnd, out num, out weakGameEntity, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags);
			if (weakGameEntity.IsValid)
			{
				for (int i = 0; i < this.TileCount; i++)
				{
					TileBase tileBase2 = this.Tiles[i];
					if (tileBase2.Entity.Name.Equals(weakGameEntity.Name))
					{
						tileBase = tileBase2;
						break;
					}
				}
			}
			return tileBase;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00056930 File Offset: 0x00054B30
		protected void CheckSwitchPlayerTurn()
		{
			if (this.PlayerTurn == PlayerTurn.PlayerOneWaiting || this.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
			{
				bool flag = false;
				using (List<PawnBase>.Enumerator enumerator = this.PlayerOneUnits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Moving)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					using (List<PawnBase>.Enumerator enumerator = this.PlayerTwoUnits.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Moving)
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					this.SwitchPlayerTurn();
				}
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000569F0 File Offset: 0x00054BF0
		protected void OnVictory(string message = "str_boardgame_victory_message")
		{
			this.MissionHandler.PlayerOneWon(message);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000569FE File Offset: 0x00054BFE
		protected void OnAfterEndTurn()
		{
			this.ClearValidMoves();
			this.CheckGameEnded();
			this.MovesLeftToEndTurn = (this.InPreMovementStage ? this.UnitsToPlacePerTurnInPreMovementStage : 1);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00056A24 File Offset: 0x00054C24
		protected void OnDefeat(string message = "str_boardgame_defeat_message")
		{
			this.MissionHandler.PlayerTwoWon(message);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00056A32 File Offset: 0x00054C32
		protected void OnDraw(string message = "str_boardgame_draw_message")
		{
			this.MissionHandler.GameWasDraw(message);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00056A40 File Offset: 0x00054C40
		private void OnBeforeSelectedUnitChanged(PawnBase oldSelectedUnit, PawnBase newSelectedUnit)
		{
			if (oldSelectedUnit != null)
			{
				oldSelectedUnit.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnUnselectedFactor);
			}
			if (newSelectedUnit != null)
			{
				newSelectedUnit.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnSelectedFactor);
			}
			this.ClearValidMoves();
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00056A7C File Offset: 0x00054C7C
		protected void EndTurn()
		{
			this.OnBeforeEndTurn();
			this.SwitchToWaiting();
			this.OnAfterEndTurn();
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00056A90 File Offset: 0x00054C90
		protected void ClearValidMoves()
		{
			this.HideAllValidTiles();
			if (this._validMoves != null)
			{
				this._validMoves.Clear();
				this._validMoves = null;
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00056AB4 File Offset: 0x00054CB4
		private void OnAfterSelectedUnitChanged()
		{
			if (this.SelectedUnit != null)
			{
				List<Move> list = this.CalculateValidMoves(this.SelectedUnit);
				if (list != null && list.Count > 0)
				{
					this._validMoves = list;
				}
				if (this.SelectedUnit.PlayerOne || this.MissionHandler.AIOpponent == null)
				{
					this.SelectedUnit.PlayPawnSelectSound();
					this.ShowAllValidTiles();
				}
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00056B14 File Offset: 0x00054D14
		private void UpdateTurn(float dt)
		{
			if (this.PlayerTurn == PlayerTurn.PlayerOne || (this.PlayerTurn == PlayerTurn.PlayerTwo && this.AIOpponent == null))
			{
				if (this.InPreMovementStage)
				{
					this.HandlePreMovementStage(dt);
					return;
				}
				if (!this.DiceRollRequired || this.DiceRolled)
				{
					Move move = this.HandlePlayerInput(dt);
					if (move.IsValid)
					{
						this.MovePawnToTile(move.Unit, move.GoalTile, false, true);
						return;
					}
				}
			}
			else if (this.PlayerTurn == PlayerTurn.PlayerTwo && this.AIOpponent != null && !this._waitingAIForfeitResponse)
			{
				if (this.AIOpponent.WantsToForfeit())
				{
					this.OnAIWantsForfeit();
				}
				if (this.DiceRollRequired && !this.DiceRolled)
				{
					this.RollDice();
				}
				this.AIOpponent.UpdateThinkingAboutMove(dt);
				if (this.AIOpponent.CanMakeMove())
				{
					this.SelectedUnit = this.AIOpponent.RecentMoveCalculated.Unit;
					if (this.SelectedUnit != null)
					{
						if (this.InPreMovementStage)
						{
							this.HandlePreMovementStageAI(this.AIOpponent.RecentMoveCalculated);
						}
						else
						{
							TileBase goalTile = this.AIOpponent.RecentMoveCalculated.GoalTile;
							this.MovePawnToTile(this.SelectedUnit, goalTile, false, true);
						}
					}
					else
					{
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_boardgame_no_available_moves_opponent", null), 0, null, null, "");
						this.EndTurn();
					}
					this.AIOpponent.ResetThinking();
				}
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00056C74 File Offset: 0x00054E74
		private bool DoneSettingUpBoard()
		{
			bool flag = !this.SettingUpBoard;
			if (this.SettingUpBoard)
			{
				if (this._rotationApplied != this._rotationTarget && this.RotateBoard)
				{
					float num = this._rotationTarget - this._rotationApplied;
					float num2 = 0.05f;
					float num3 = MathF.Clamp(num, -num2, num2);
					MatrixFrame globalFrame = this.BoardEntity.GetGlobalFrame();
					globalFrame.rotation.RotateAboutUp(num3);
					this.BoardEntity.SetGlobalFrame(in globalFrame, true);
					this._rotationApplied += num3;
					if (MathF.Abs(this._rotationTarget - this._rotationApplied) <= 1E-05f)
					{
						this._rotationApplied = this._rotationTarget;
						this.UpdateAllPawnsPositions();
						this.UpdateAllTilesPositions();
						return flag;
					}
				}
				else
				{
					if (!this._rotationCompleted)
					{
						this._rotationCompleted = true;
						this.OnAfterBoardRotated();
						return flag;
					}
					this.SettingUpBoard = false;
					this.OnAfterBoardSetUp();
				}
			}
			return flag;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00056D58 File Offset: 0x00054F58
		protected void HideAllValidTiles()
		{
			if (this._validMoves != null && this._validMoves.Count > 0)
			{
				foreach (Move move in this._validMoves)
				{
					move.GoalTile.SetVisibility(false);
				}
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00056DC4 File Offset: 0x00054FC4
		protected void ShowAllValidTiles()
		{
			if (this._validMoves != null && this._validMoves.Count > 0)
			{
				foreach (Move move in this._validMoves)
				{
					move.GoalTile.SetVisibility(true);
				}
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00056E30 File Offset: 0x00055030
		private void UnfocusAllPawns()
		{
			if (this.PlayerOneUnits != null)
			{
				foreach (PawnBase pawnBase in this.PlayerOneUnits)
				{
					pawnBase.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnUnselectedFactor);
				}
			}
			if (this.PlayerTwoUnits != null)
			{
				foreach (PawnBase pawnBase2 in this.PlayerTwoUnits)
				{
					pawnBase2.Entity.GetMetaMesh(0).SetFactor1Linear(this.PawnUnselectedFactor);
				}
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00056EF4 File Offset: 0x000550F4
		private bool MovingPawnPresent()
		{
			bool flag = false;
			foreach (PawnBase pawnBase in this.PlayerOneUnits)
			{
				if (pawnBase.Moving || pawnBase.HasAnyGoalPosition)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (PawnBase pawnBase2 in this.PlayerTwoUnits)
				{
					if (pawnBase2.Moving || pawnBase2.HasAnyGoalPosition)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00056FAC File Offset: 0x000551AC
		private void SwitchToWaiting()
		{
			if (this.PlayerTurn == PlayerTurn.PlayerOne)
			{
				this.PlayerTurn = PlayerTurn.PlayerOneWaiting;
			}
			else if (this.PlayerTurn == PlayerTurn.PlayerTwo)
			{
				this.PlayerTurn = PlayerTurn.PlayerTwoWaiting;
			}
			this.JustStoppedDraggingUnit = false;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00056FD8 File Offset: 0x000551D8
		protected void OnAIWantsForfeit()
		{
			if (!this._waitingAIForfeitResponse)
			{
				this._waitingAIForfeitResponse = true;
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_boardgame", null).ToString(), GameTexts.FindText("str_boardgame_forfeit_question", null).ToString(), true, true, GameTexts.FindText("str_accept", null).ToString(), GameTexts.FindText("str_reject", null).ToString(), new Action(this.OnAIForfeitAccepted), new Action(this.OnAIForfeitRejected), "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00057068 File Offset: 0x00055268
		private void UpdateAllPawnsPositions()
		{
			foreach (PawnBase pawnBase in this.PlayerOneUnits)
			{
				pawnBase.UpdatePawnPosition();
			}
			foreach (PawnBase pawnBase2 in this.PlayerTwoUnits)
			{
				pawnBase2.UpdatePawnPosition();
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000570F8 File Offset: 0x000552F8
		private void OnAIForfeitAccepted()
		{
			this.MissionHandler.AIForfeitGame();
			this._waitingAIForfeitResponse = false;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0005710C File Offset: 0x0005530C
		private void OnAIForfeitRejected()
		{
			this._waitingAIForfeitResponse = false;
		}

		// Token: 0x040004FD RID: 1277
		public const string StringBoardGame = "str_boardgame";

		// Token: 0x040004FE RID: 1278
		public const string StringForfeitQuestion = "str_boardgame_forfeit_question";

		// Token: 0x040004FF RID: 1279
		public const string StringMovePiecePlayer = "str_boardgame_move_piece_player";

		// Token: 0x04000500 RID: 1280
		public const string StringMovePieceOpponent = "str_boardgame_move_piece_opponent";

		// Token: 0x04000501 RID: 1281
		public const string StringCapturePiecePlayer = "str_boardgame_capture_piece_player";

		// Token: 0x04000502 RID: 1282
		public const string StringCapturePieceOpponent = "str_boardgame_capture_piece_opponent";

		// Token: 0x04000503 RID: 1283
		public const string StringVictoryMessage = "str_boardgame_victory_message";

		// Token: 0x04000504 RID: 1284
		public const string StringDefeatMessage = "str_boardgame_defeat_message";

		// Token: 0x04000505 RID: 1285
		public const string StringDrawMessage = "str_boardgame_draw_message";

		// Token: 0x04000506 RID: 1286
		public const string StringNoAvailableMovesPlayer = "str_boardgame_no_available_moves_player";

		// Token: 0x04000507 RID: 1287
		public const string StringNoAvailableMovesOpponent = "str_boardgame_no_available_moves_opponent";

		// Token: 0x04000508 RID: 1288
		public const string StringSeegaBarrierByP1DrawMessage = "str_boardgame_seega_barrier_by_player_one_draw_message";

		// Token: 0x04000509 RID: 1289
		public const string StringSeegaBarrierByP2DrawMessage = "str_boardgame_seega_barrier_by_player_two_draw_message";

		// Token: 0x0400050A RID: 1290
		public const string StringSeegaBarrierByP1VictoryMessage = "str_boardgame_seega_barrier_by_player_one_victory_message";

		// Token: 0x0400050B RID: 1291
		public const string StringSeegaBarrierByP2VictoryMessage = "str_boardgame_seega_barrier_by_player_two_victory_message";

		// Token: 0x0400050C RID: 1292
		public const string StringSeegaBarrierByP1DefeatMessage = "str_boardgame_seega_barrier_by_player_one_defeat_message";

		// Token: 0x0400050D RID: 1293
		public const string StringSeegaBarrierByP2DefeatMessage = "str_boardgame_seega_barrier_by_player_two_defeat_message";

		// Token: 0x0400050E RID: 1294
		public const string StringRollDicePlayer = "str_boardgame_roll_dice_player";

		// Token: 0x0400050F RID: 1295
		public const string StringRollDiceOpponent = "str_boardgame_roll_dice_opponent";

		// Token: 0x04000510 RID: 1296
		protected const int InvalidDice = -1;

		// Token: 0x04000511 RID: 1297
		protected const float DelayBeforeMovingAnyPawn = 0.25f;

		// Token: 0x04000512 RID: 1298
		protected const float DelayBetweenPawnMovementsBegin = 0.15f;

		// Token: 0x04000513 RID: 1299
		private const float DiceRollAnimationDuration = 1f;

		// Token: 0x04000514 RID: 1300
		private const float DraggingDuration = 0.2f;

		// Token: 0x04000515 RID: 1301
		private const int UnitsToPlacePerTurnInMovementStage = 1;

		// Token: 0x04000516 RID: 1302
		protected uint PawnSelectedFactor = uint.MaxValue;

		// Token: 0x04000517 RID: 1303
		protected uint PawnUnselectedFactor = 4282203453U;

		// Token: 0x04000518 RID: 1304
		protected MissionBoardGameLogic MissionHandler;

		// Token: 0x04000519 RID: 1305
		protected GameEntity BoardEntity;

		// Token: 0x0400051A RID: 1306
		protected GameEntity DiceBoard;

		// Token: 0x0400051B RID: 1307
		protected bool JustStoppedDraggingUnit;

		// Token: 0x0400051C RID: 1308
		protected CapturedPawnsPool PlayerOnePool;

		// Token: 0x0400051D RID: 1309
		protected bool ReadyToPlay;

		// Token: 0x0400051E RID: 1310
		protected CapturedPawnsPool PlayerTwoPool;

		// Token: 0x0400051F RID: 1311
		protected bool SettingUpBoard = true;

		// Token: 0x04000520 RID: 1312
		protected bool HasToMovePawnsAcross;

		// Token: 0x04000521 RID: 1313
		protected float DiceRollAnimationTimer;

		// Token: 0x04000522 RID: 1314
		protected int MovesLeftToEndTurn;

		// Token: 0x04000523 RID: 1315
		protected bool DiceRollAnimationRunning;

		// Token: 0x04000524 RID: 1316
		protected int DiceRollSoundCodeID;

		// Token: 0x04000525 RID: 1317
		private List<Move> _validMoves;

		// Token: 0x04000526 RID: 1318
		private PawnBase _selectedUnit;

		// Token: 0x04000527 RID: 1319
		private Vec3 _userRayBegin;

		// Token: 0x04000528 RID: 1320
		private Vec3 _userRayEnd;

		// Token: 0x04000529 RID: 1321
		private float _draggingTimer;

		// Token: 0x0400052A RID: 1322
		private bool _draggingSelectedUnit;

		// Token: 0x0400052B RID: 1323
		private float _rotationApplied;

		// Token: 0x0400052C RID: 1324
		private float _rotationTarget;

		// Token: 0x0400052D RID: 1325
		private bool _rotationCompleted;

		// Token: 0x0400052E RID: 1326
		private bool _deselectUnit;

		// Token: 0x0400052F RID: 1327
		private bool _firstTickAfterReady = true;

		// Token: 0x04000530 RID: 1328
		private bool _waitingAIForfeitResponse;
	}
}
