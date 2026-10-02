using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FC RID: 252
	public abstract class PawnBase
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0005E0EA File Offset: 0x0005C2EA
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0005E0F1 File Offset: 0x0005C2F1
		public static int PawnMoveSoundCodeID { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005E0F9 File Offset: 0x0005C2F9
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x0005E100 File Offset: 0x0005C300
		public static int PawnSelectSoundCodeID { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0005E108 File Offset: 0x0005C308
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0005E10F File Offset: 0x0005C30F
		public static int PawnTapSoundCodeID { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0005E117 File Offset: 0x0005C317
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x0005E11E File Offset: 0x0005C31E
		public static int PawnRemoveSoundCodeID { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000CA6 RID: 3238
		public abstract bool IsPlaced { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0005E126 File Offset: 0x0005C326
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x0005E12E File Offset: 0x0005C32E
		public virtual Vec3 PosBeforeMoving
		{
			get
			{
				return this.PosBeforeMovingBase;
			}
			protected set
			{
				this.PosBeforeMovingBase = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0005E137 File Offset: 0x0005C337
		public GameEntity Entity { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0005E13F File Offset: 0x0005C33F
		protected List<Vec3> GoalPositions { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0005E147 File Offset: 0x0005C347
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x0005E14F File Offset: 0x0005C34F
		private protected Vec3 CurrentPos { protected get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0005E158 File Offset: 0x0005C358
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0005E160 File Offset: 0x0005C360
		public bool Captured { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0005E169 File Offset: 0x0005C369
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0005E171 File Offset: 0x0005C371
		public bool MovingToDifferentTile { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0005E17A File Offset: 0x0005C37A
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x0005E182 File Offset: 0x0005C382
		public bool Moving { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0005E18B File Offset: 0x0005C38B
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0005E193 File Offset: 0x0005C393
		public bool PlayerOne { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0005E19C File Offset: 0x0005C39C
		public bool HasAnyGoalPosition
		{
			get
			{
				bool flag = false;
				if (this.GoalPositions != null)
				{
					flag = !this.GoalPositions.IsEmpty<Vec3>();
				}
				return flag;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		protected PawnBase(GameEntity entity, bool playerOne)
		{
			this.Entity = entity;
			this.PlayerOne = playerOne;
			this.CurrentPos = this.Entity.GetGlobalFrame().origin;
			this.PosBeforeMoving = this.CurrentPos;
			this.Moving = false;
			this._dragged = false;
			this.Captured = false;
			this._movePauseDuration = 0.3f;
			entity.CreateVariableRatePhysics(true);
			this.GoalPositions = new List<Vec3>();
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0005E23C File Offset: 0x0005C43C
		public virtual void Reset()
		{
			this.ClearGoalPositions();
			this.Moving = false;
			this.MovingToDifferentTile = false;
			this._movePauseDuration = 0.3f;
			this._movePauseTimer = 0f;
			this._moveTiming = false;
			this._dragged = false;
			this.Captured = false;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0005E288 File Offset: 0x0005C488
		public virtual void AddGoalPosition(Vec3 goal)
		{
			this.GoalPositions.Add(goal);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0005E298 File Offset: 0x0005C498
		public virtual void SetPawnAtPosition(Vec3 position)
		{
			MatrixFrame globalFrame = this.Entity.GetGlobalFrame();
			globalFrame.origin = position;
			this.Entity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0005E2C8 File Offset: 0x0005C4C8
		public virtual void MovePawnToGoalPositions(bool instantMove, float speed, bool dragged = false)
		{
			this.PosBeforeMoving = this.Entity.GlobalPosition;
			this._moveSpeed = speed;
			this._currentGoalPos = 0;
			this._movePauseTimer = 0f;
			this._dtCounter = 0f;
			this._moveTiming = false;
			this._dragged = dragged;
			if (this.GoalPositions.Count == 1 && this.PosBeforeMoving.Equals(this.GoalPositions[0]))
			{
				instantMove = true;
			}
			if (instantMove)
			{
				MatrixFrame globalFrame = this.Entity.GetGlobalFrame();
				globalFrame.origin = this.GoalPositions[this.GoalPositions.Count - 1];
				this.Entity.SetGlobalFrame(in globalFrame, true);
				this.ClearGoalPositions();
				return;
			}
			this.Moving = true;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0005E39A File Offset: 0x0005C59A
		public virtual void EnableCollisionBody()
		{
			this.Entity.BodyFlag &= ~BodyFlags.Disabled;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0005E3B0 File Offset: 0x0005C5B0
		public virtual void DisableCollisionBody()
		{
			this.Entity.BodyFlag |= BodyFlags.Disabled;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0005E3C8 File Offset: 0x0005C5C8
		public void Tick(float dt)
		{
			if (this._moveTiming)
			{
				this._movePauseTimer += dt;
				if (this._movePauseTimer >= this._movePauseDuration)
				{
					this._moveTiming = false;
					this._movePauseTimer = 0f;
				}
				return;
			}
			if (this.Moving && dt > 0f)
			{
				Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
				Vec3 vec2 = this.GoalPositions[this._currentGoalPos] - this.PosBeforeMoving;
				float num = vec2.Normalize();
				float num2 = num / this._moveSpeed;
				float num3 = this._dtCounter / num2;
				if (this._dtCounter.Equals(0f))
				{
					float x = (this.Entity.GlobalBoxMax - this.Entity.GlobalBoxMin).x;
					float z = (this.Entity.GlobalBoxMax - this.Entity.GlobalBoxMin).z;
					Vec3 vec3 = new Vec3(0f, 0f, z / 2f, -1f);
					Vec3 vec4 = this.Entity.GetGlobalFrame().origin + vec3 + vec2 * (x / 1.8f);
					Vec3 vec5 = this.GoalPositions[this._currentGoalPos] + vec3;
					float num4;
					if (Mission.Current.Scene.RayCastForClosestEntityOrTerrain(vec4, vec5, out num4, 0.001f, BodyFlags.None))
					{
						this._freePathToDestination = false;
						num = num4;
					}
					else
					{
						this._freePathToDestination = true;
						if (!this._dragged)
						{
							this.PlayPawnMoveSound();
						}
						else
						{
							this.PlayPawnTapSound();
						}
					}
				}
				if (!this._freePathToDestination)
				{
					float num5 = MathF.Sin(num3 * 3.1415927f);
					float num6 = num / 6f;
					num5 *= num6;
					vec += new Vec3(0f, 0f, num5, -1f);
				}
				float dtCounter = this._dtCounter;
				this._dtCounter += dt;
				Vec3 vec8;
				if (num3 >= 1f)
				{
					this._dtCounter = 0f;
					this.CurrentPos = this.GoalPositions[this._currentGoalPos];
					vec = Vec3.Zero;
					if (!this._freePathToDestination && this.IsPlaced)
					{
						this.PlayPawnTapSound();
					}
					else if (!this.IsPlaced)
					{
						this.PlayPawnRemovedTapSound();
					}
					Vec3 vec6 = this.GoalPositions[this._currentGoalPos];
					bool flag = true;
					while (this._currentGoalPos < this.GoalPositions.Count - 1)
					{
						this._currentGoalPos++;
						Vec3 vec7 = this.GoalPositions[this._currentGoalPos];
						vec8 = vec6 - vec7;
						if (vec8.LengthSquared > 0f)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Action<PawnBase, Vec3, Vec3> onArrivedFinalGoalPosition = this.OnArrivedFinalGoalPosition;
						if (onArrivedFinalGoalPosition != null)
						{
							onArrivedFinalGoalPosition(this, this.PosBeforeMoving, this.CurrentPos);
						}
						this.Moving = false;
						this.ClearGoalPositions();
					}
					else
					{
						Action<PawnBase, Vec3, Vec3> onArrivedIntermediateGoalPosition = this.OnArrivedIntermediateGoalPosition;
						if (onArrivedIntermediateGoalPosition != null)
						{
							onArrivedIntermediateGoalPosition(this, this.PosBeforeMoving, this.CurrentPos);
						}
						this._movePauseDuration = 0.3f;
						this._moveTiming = true;
					}
					this.PosBeforeMoving = this.CurrentPos;
				}
				else
				{
					this.Moving = true;
					this.CurrentPos = MBMath.Lerp(this.PosBeforeMoving, this.GoalPositions[this._currentGoalPos], num3, 0.005f);
				}
				ref MatrixFrame ptr = ref this.Entity.GetGlobalFrame();
				vec8 = this.CurrentPos + vec;
				MatrixFrame matrixFrame = new MatrixFrame(in ptr.rotation, in vec8);
				this.Entity.SetGlobalFrame(in matrixFrame, true);
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0005E770 File Offset: 0x0005C970
		public void MovePawnToGoalPositionsDelayed(bool instantMove, float speed, bool dragged, float delay)
		{
			if (this.GoalPositions.Count > 0)
			{
				if (this.GoalPositions.Count == 1 && this.PosBeforeMoving.Equals(this.GoalPositions[0]))
				{
					this.ClearGoalPositions();
					return;
				}
				this.MovePawnToGoalPositions(instantMove, speed, dragged);
				this._movePauseDuration = delay;
				this._moveTiming = delay > 0f;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0005E7E7 File Offset: 0x0005C9E7
		public void SetPlayerOne(bool playerOne)
		{
			this.PlayerOne = playerOne;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0005E7F0 File Offset: 0x0005C9F0
		public void ClearGoalPositions()
		{
			this.MovingToDifferentTile = false;
			this.GoalPositions.Clear();
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0005E804 File Offset: 0x0005CA04
		public void UpdatePawnPosition()
		{
			this.PosBeforeMoving = this.Entity.GlobalPosition;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0005E817 File Offset: 0x0005CA17
		public void PlayPawnSelectSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnSelectSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0005E832 File Offset: 0x0005CA32
		private void PlayPawnTapSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnTapSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0005E84D File Offset: 0x0005CA4D
		private void PlayPawnRemovedTapSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnRemoveSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0005E868 File Offset: 0x0005CA68
		private void PlayPawnMoveSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnMoveSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x04000571 RID: 1393
		public Action<PawnBase, Vec3, Vec3> OnArrivedIntermediateGoalPosition;

		// Token: 0x04000572 RID: 1394
		public Action<PawnBase, Vec3, Vec3> OnArrivedFinalGoalPosition;

		// Token: 0x04000573 RID: 1395
		protected Vec3 PosBeforeMovingBase;

		// Token: 0x04000574 RID: 1396
		private int _currentGoalPos;

		// Token: 0x04000575 RID: 1397
		private float _dtCounter;

		// Token: 0x04000576 RID: 1398
		private float _movePauseDuration;

		// Token: 0x04000577 RID: 1399
		private float _movePauseTimer;

		// Token: 0x04000578 RID: 1400
		private float _moveSpeed;

		// Token: 0x04000579 RID: 1401
		private bool _moveTiming;

		// Token: 0x0400057A RID: 1402
		private bool _dragged;

		// Token: 0x0400057B RID: 1403
		private bool _freePathToDestination;
	}
}
