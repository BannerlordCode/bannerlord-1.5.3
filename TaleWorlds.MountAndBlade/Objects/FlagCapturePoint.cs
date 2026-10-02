using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003AA RID: 938
	public class FlagCapturePoint : SynchedMissionObject
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060035AC RID: 13740 RVA: 0x000DDA64 File Offset: 0x000DBC64
		[EditableScriptComponentVariable(false, "")]
		public Vec3 Position
		{
			get
			{
				return base.GameEntity.GlobalPosition;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060035AD RID: 13741 RVA: 0x000DDA7F File Offset: 0x000DBC7F
		public int FlagChar
		{
			get
			{
				return 65 + this.FlagIndex;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060035AE RID: 13742 RVA: 0x000DDA8A File Offset: 0x000DBC8A
		public bool IsContested
		{
			get
			{
				return this._currentDirection == CaptureTheFlagFlagDirection.Down;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060035AF RID: 13743 RVA: 0x000DDA95 File Offset: 0x000DBC95
		public bool IsFullyRaised
		{
			get
			{
				return this._currentDirection == CaptureTheFlagFlagDirection.None;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060035B0 RID: 13744 RVA: 0x000DDAA0 File Offset: 0x000DBCA0
		public bool IsDeactivated
		{
			get
			{
				return !base.GameEntity.IsVisibleIncludeParents();
			}
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000DDABE File Offset: 0x000DBCBE
		protected internal override void OnMissionReset()
		{
			this._currentDirection = CaptureTheFlagFlagDirection.None;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000DDAC8 File Offset: 0x000DBCC8
		public void ResetPointAsServer(uint defaultColor, uint defaultColor2)
		{
			MatrixFrame globalFrame = this._flagTopBoundary.GetGlobalFrame();
			this._flagHolder.SetGlobalFrameSynched(ref globalFrame, false);
			this.SetTeamColorsWithAllSynched(defaultColor, defaultColor2);
			this.SetVisibleWithAllSynched(true, false);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000DDAFF File Offset: 0x000DBCFF
		public void RemovePointAsServer()
		{
			this.SetVisibleWithAllSynched(false, false);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000DDB0C File Offset: 0x000DBD0C
		protected internal override void OnInit()
		{
			this._flagHolder = base.GameEntity.GetFirstChildEntityWithTag("score_stand").GetFirstScriptOfType<SynchedMissionObject>();
			this._theFlag = this._flagHolder.GameEntity.GetFirstChildEntityWithTag("flag_white").GetFirstScriptOfType<SynchedMissionObject>();
			this._flagBottomBoundary = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("flag_raising_bottom"));
			this._flagTopBoundary = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("flag_raising_top"));
			MatrixFrame globalFrame = this._flagTopBoundary.GetGlobalFrame();
			this._flagHolder.GameEntity.SetGlobalFrame(in globalFrame, true);
			this._flagDependentObjects = new List<SynchedMissionObject>();
			foreach (WeakGameEntity weakGameEntity in Mission.Current.Scene.FindWeakEntitiesWithTag("depends_flag_" + this.FlagIndex).ToList<WeakGameEntity>())
			{
				SynchedMissionObject firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>();
				this._flagDependentObjects.Add(firstScriptOfType);
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000DDC44 File Offset: 0x000DBE44
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), 4f, 2852192000U, true, false);
				DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), 6f, 2868838400U, true, false);
			}
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000DDCB0 File Offset: 0x000DBEB0
		public void OnAfterTick(bool canOwnershipChange, out bool ownerTeamChanged)
		{
			ownerTeamChanged = false;
			if (this._flagHolder.SynchronizeCompleted)
			{
				bool flag = this._flagHolder.GameEntity.GlobalPosition.DistanceSquared(this._flagTopBoundary.GlobalPosition).ApproximatelyEqualsTo(0f, 1E-05f);
				if (canOwnershipChange)
				{
					if (!flag)
					{
						ownerTeamChanged = true;
						return;
					}
					this._currentDirection = CaptureTheFlagFlagDirection.None;
					return;
				}
				else if (flag)
				{
					this._currentDirection = CaptureTheFlagFlagDirection.None;
				}
			}
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000DDD20 File Offset: 0x000DBF20
		public void SetMoveFlag(CaptureTheFlagFlagDirection directionTo, float speedMultiplier = 1f)
		{
			float flagProgress = this.GetFlagProgress();
			float num = 1f / speedMultiplier;
			float num2 = ((directionTo == CaptureTheFlagFlagDirection.Up) ? (1f - flagProgress) : flagProgress);
			float num3 = 10f * num;
			float num4 = num2 * num3;
			this._currentDirection = directionTo;
			MatrixFrame matrixFrame;
			if (directionTo != CaptureTheFlagFlagDirection.Up)
			{
				if (directionTo != CaptureTheFlagFlagDirection.Down)
				{
					throw new ArgumentOutOfRangeException("directionTo", directionTo, null);
				}
				matrixFrame = this._flagBottomBoundary.GetFrame();
			}
			else
			{
				matrixFrame = this._flagTopBoundary.GetFrame();
			}
			this._flagHolder.SetFrameSynchedOverTime(ref matrixFrame, num4, false);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000DDDA3 File Offset: 0x000DBFA3
		public void ChangeMovementSpeed(float speedMultiplier)
		{
			if (this._currentDirection != CaptureTheFlagFlagDirection.None)
			{
				this.SetMoveFlag(this._currentDirection, speedMultiplier);
			}
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000DDDBC File Offset: 0x000DBFBC
		public void SetMoveNone()
		{
			this._currentDirection = CaptureTheFlagFlagDirection.None;
			MatrixFrame frame = this._flagHolder.GameEntity.GetFrame();
			this._flagHolder.SetFrameSynched(ref frame, false);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000DDDF4 File Offset: 0x000DBFF4
		public void SetVisibleWithAllSynched(bool value, bool forceChildrenVisible = false)
		{
			this.SetVisibleSynched(value, forceChildrenVisible);
			foreach (SynchedMissionObject synchedMissionObject in this._flagDependentObjects)
			{
				synchedMissionObject.SetVisibleSynched(value, false);
			}
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000DDE50 File Offset: 0x000DC050
		public void SetTeamColorsWithAllSynched(uint color, uint color2)
		{
			this._theFlag.SetTeamColorsSynched(color, color2);
			foreach (SynchedMissionObject synchedMissionObject in this._flagDependentObjects)
			{
				synchedMissionObject.SetTeamColorsSynched(color, color2);
			}
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000DDEB0 File Offset: 0x000DC0B0
		public uint GetFlagColor()
		{
			return this._theFlag.Color;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000DDEBD File Offset: 0x000DC0BD
		public uint GetFlagColor2()
		{
			return this._theFlag.Color2;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000DDECC File Offset: 0x000DC0CC
		public float GetFlagProgress()
		{
			return MathF.Clamp((this._theFlag.GameEntity.GlobalPosition.z - this._flagBottomBoundary.GlobalPosition.z) / (this._flagTopBoundary.GlobalPosition.z - this._flagBottomBoundary.GlobalPosition.z), 0f, 1f);
		}

		// Token: 0x040016DE RID: 5854
		public const float PointRadius = 4f;

		// Token: 0x040016DF RID: 5855
		public const float RadiusMultiplierForContestedArea = 1.5f;

		// Token: 0x040016E0 RID: 5856
		private const float TimeToTravelBetweenBoundaries = 10f;

		// Token: 0x040016E1 RID: 5857
		public int FlagIndex;

		// Token: 0x040016E2 RID: 5858
		private SynchedMissionObject _theFlag;

		// Token: 0x040016E3 RID: 5859
		private SynchedMissionObject _flagHolder;

		// Token: 0x040016E4 RID: 5860
		private GameEntity _flagBottomBoundary;

		// Token: 0x040016E5 RID: 5861
		private GameEntity _flagTopBoundary;

		// Token: 0x040016E6 RID: 5862
		private List<SynchedMissionObject> _flagDependentObjects;

		// Token: 0x040016E7 RID: 5863
		private CaptureTheFlagFlagDirection _currentDirection = CaptureTheFlagFlagDirection.None;
	}
}
