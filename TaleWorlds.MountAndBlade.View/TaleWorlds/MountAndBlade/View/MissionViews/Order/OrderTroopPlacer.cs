using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Order
{
	// Token: 0x020000A8 RID: 168
	public class OrderTroopPlacer : MissionView
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00029D24 File Offset: 0x00027F24
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x00029D2C File Offset: 0x00027F2C
		public bool SuspendTroopPlacer
		{
			get
			{
				return this._suspendTroopPlacer;
			}
			set
			{
				this._suspendTroopPlacer = value;
				if (value)
				{
					this.HideOrderPositionEntities();
				}
				else
				{
					this._formationDrawingStartingPosition = null;
				}
				this.Reset();
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00029D52 File Offset: 0x00027F52
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00029D5A File Offset: 0x00027F5A
		public OrderFlag OrderFlag { get; private set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00029D63 File Offset: 0x00027F63
		private Team _playerTeam
		{
			get
			{
				return base.Mission.PlayerTeam;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00029D70 File Offset: 0x00027F70
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00029D78 File Offset: 0x00027F78
		private protected OrderTroopPlacer.CursorState ActiveCursorState { protected get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00029D81 File Offset: 0x00027F81
		protected OrderController OrderController
		{
			get
			{
				if (this._orderController != null)
				{
					return this._orderController;
				}
				return Mission.Current.PlayerTeam.PlayerOrderController;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00029DA1 File Offset: 0x00027FA1
		public OrderTroopPlacer(OrderController orderController)
		{
			this._orderController = orderController;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00029DB0 File Offset: 0x00027FB0
		protected virtual OrderFlag CreateOrderFlag()
		{
			return new OrderFlag(base.Mission, base.MissionScreen, 10f);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00029DC8 File Offset: 0x00027FC8
		protected virtual bool CanUpdate()
		{
			return this.OrderController.SelectedFormations.Count > 0;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00029DDD File Offset: 0x00027FDD
		protected virtual bool HasSelectedFormations()
		{
			return this.OrderController.SelectedFormations.Count > 0;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00029DF4 File Offset: 0x00027FF4
		protected virtual OrderTroopPlacer.CursorState GetCursorState()
		{
			OrderTroopPlacer.CursorState cursorState = OrderTroopPlacer.CursorState.Invisible;
			if (this.HasSelectedFormations())
			{
				WorldPosition worldPosition;
				float num;
				WeakGameEntity weakGameEntity;
				if (!this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out weakGameEntity))
				{
					num = 1000f;
				}
				if (cursorState == OrderTroopPlacer.CursorState.Invisible && num < 1000f)
				{
					if (!this._formationDrawingMode && !weakGameEntity.IsValid)
					{
						for (int i = 0; i < this._orderRotationEntities.Count; i++)
						{
							GameEntity gameEntity = this._orderRotationEntities[i];
							if (gameEntity.IsVisibleIncludeParents() && weakGameEntity == gameEntity)
							{
								this._mouseOverFormation = this.OrderController.SelectedFormations.ElementAt<Formation>(i / 2);
								this._mouseOverDirection = 1 - (i & 1);
								cursorState = OrderTroopPlacer.CursorState.Rotation;
								break;
							}
						}
					}
					if (cursorState == OrderTroopPlacer.CursorState.Invisible)
					{
						OrderFlag orderFlag = base.MissionScreen.OrderFlag;
						if (((orderFlag != null) ? orderFlag.FocusedOrderableObject : null) != null)
						{
							cursorState = OrderTroopPlacer.CursorState.OrderableEntity;
						}
					}
					if (cursorState == OrderTroopPlacer.CursorState.Invisible)
					{
						cursorState = this.GetGroundOrNormalCursor();
					}
				}
			}
			if (cursorState != OrderTroopPlacer.CursorState.Ground && cursorState != OrderTroopPlacer.CursorState.Rotation)
			{
				this._mouseOverDirection = 0;
			}
			return cursorState;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00029EE2 File Offset: 0x000280E2
		protected virtual Vec3 GetGroundedVec3(WorldPosition worldPosition)
		{
			return worldPosition.GetGroundVec3();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00029EEC File Offset: 0x000280EC
		protected virtual bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out float collisionDistance, out WeakGameEntity collidedEntity)
		{
			Vec3 vec;
			Vec3 vec2;
			base.MissionScreen.ScreenPointToWorldRay(this.GetScreenPoint(), out vec, out vec2);
			float num;
			WeakGameEntity weakGameEntity;
			if (base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec, vec2, out num, out weakGameEntity, 0.3f, BodyFlags.Disabled | BodyFlags.AILimiter | BodyFlags.Barrier | BodyFlags.Barrier3D | BodyFlags.Ragdoll | BodyFlags.RagdollLimiter | BodyFlags.DoNotCollideWithRaycast | BodyFlags.BodyOwnerFlora))
			{
				Vec3 vec3 = vec2 - vec;
				vec3.Normalize();
				collisionDistance = num;
				collidedEntity = weakGameEntity;
				worldPosition = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, vec + vec3 * collisionDistance, false);
				return true;
			}
			worldPosition = WorldPosition.Invalid;
			collisionDistance = 0f;
			collidedEntity = WeakGameEntity.Invalid;
			return false;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00029F98 File Offset: 0x00028198
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out float collisionDistance)
		{
			WeakGameEntity weakGameEntity;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out collisionDistance, out weakGameEntity);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00029FB0 File Offset: 0x000281B0
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out WeakGameEntity collidedEntity)
		{
			float num;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out collidedEntity);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00029FC8 File Offset: 0x000281C8
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition)
		{
			float num;
			WeakGameEntity weakGameEntity;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out weakGameEntity);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00029FE0 File Offset: 0x000281E0
		protected Vec2 GetScreenPoint()
		{
			if (!base.MissionScreen.MouseVisible)
			{
				return new Vec2(0.5f, 0.5f) + this._deltaMousePosition;
			}
			return base.Input.GetMousePositionRanged() + this._deltaMousePosition;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002A020 File Offset: 0x00028220
		public OrderTroopPlacer.CursorState GetGroundOrNormalCursor()
		{
			if (!this._formationDrawingMode)
			{
				return OrderTroopPlacer.CursorState.Normal;
			}
			return OrderTroopPlacer.CursorState.Ground;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0002A030 File Offset: 0x00028230
		public override void AfterStart()
		{
			base.AfterStart();
			this.OrderFlag = this.CreateOrderFlag();
			this._formationDrawingStartingPosition = null;
			this._formationDrawingStartingPointOfMouse = null;
			this._formationDrawingStartingTime = null;
			this._orderRotationEntities = new List<GameEntity>();
			this._orderPositionEntities = new List<GameEntity>();
			this.formationDrawTimer = new Timer(MBCommon.GetApplicationTime(), 0.033333335f, true);
			this._widthEntityLeft = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
			this._widthEntityLeft.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthEntityLeft.SetVisibilityExcludeParents(false);
			this._widthEntityRight = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
			this._widthEntityRight.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthEntityRight.SetVisibilityExcludeParents(false);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0002A118 File Offset: 0x00028318
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._initialized)
			{
				MissionPeer missionPeer = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
				if (base.Mission.PlayerTeam != null || (missionPeer != null && (missionPeer.Team == base.Mission.AttackerTeam || missionPeer.Team == base.Mission.DefenderTeam)))
				{
					this._initialized = true;
				}
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002A186 File Offset: 0x00028386
		public void RestrictOrdersToDeploymentBoundaries(bool enabled)
		{
			this._restrictOrdersToDeploymentBoundaries = enabled;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002A190 File Offset: 0x00028390
		private void UpdateFormationDrawingForFacingOrder(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			Vec3 vec = base.MissionScreen.GetOrderFlagPosition();
			Vec2 asVec = vec.AsVec2;
			Vec2 orderLookAtDirection = OrderController.GetOrderLookAtDirection(this.OrderController.SelectedFormations, asVec);
			List<WorldPosition> list;
			this.OrderController.SimulateNewFacingOrder(orderLookAtDirection, out list);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				vec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in vec, giveOrder, -1f);
				num++;
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0002A23C File Offset: 0x0002843C
		private void UpdateFormationDrawingForDestination(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			List<WorldPosition> list;
			this.OrderController.SimulateDestinationFrames(out list, 3f);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				Vec3 groundedVec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in groundedVec, giveOrder, 0.7f);
				num++;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0002A2C0 File Offset: 0x000284C0
		private void UpdateFormationDrawingForFormingOrder(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			MatrixFrame orderFlagFrame = base.MissionScreen.GetOrderFlagFrame();
			Vec3 origin = orderFlagFrame.origin;
			Vec2 asVec = orderFlagFrame.rotation.f.AsVec2;
			float orderFormCustomWidth = OrderController.GetOrderFormCustomWidth(this.OrderController.SelectedFormations, origin);
			List<WorldPosition> list;
			this.OrderController.SimulateNewCustomWidthOrder(orderFormCustomWidth, out list);
			Formation formation = this.OrderController.SelectedFormations.MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				worldPosition.GetNavMesh();
				int num2 = num;
				Vec3 vec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in vec, giveOrder, -1f);
				num++;
			}
			float unitDiameter = formation.UnitDiameter;
			float interval = formation.Interval;
			int num3 = MathF.Max(0, (int)((orderFormCustomWidth - unitDiameter) / (interval + unitDiameter) + 1E-05f)) + 1;
			float num4 = (float)(num3 - 1) * (interval + unitDiameter);
			for (int i = 0; i < num3; i++)
			{
				Vec2 vec2 = new Vec2((float)i * (interval + unitDiameter) - num4 / 2f, 0f);
				Vec2 vec3 = asVec.TransformToParentUnitF(vec2);
				WorldPosition worldPosition2 = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, origin, false);
				worldPosition2.SetVec2(worldPosition2.AsVec2 + vec3);
				int num5 = num++;
				Vec3 vec = this.GetGroundedVec3(worldPosition2);
				this.AddOrderPositionEntity(num5, in vec, false, -1f);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0002A47C File Offset: 0x0002867C
		public void UpdateFormationDrawing(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			this.HideOrderPositionEntities();
			if (this._formationDrawingStartingPosition == null)
			{
				return;
			}
			WorldPosition worldPosition = WorldPosition.Invalid;
			bool flag = false;
			if (base.MissionScreen.MouseVisible && this._formationDrawingStartingPointOfMouse != null)
			{
				Vec2 vec = this._formationDrawingStartingPointOfMouse.Value - base.Input.GetMousePositionPixel();
				if (MathF.Abs(vec.x) < 10f && MathF.Abs(vec.y) < 10f)
				{
					flag = true;
					worldPosition = this._formationDrawingStartingPosition.Value;
				}
			}
			if (base.MissionScreen.MouseVisible && this._formationDrawingStartingTime != null && base.Mission.CurrentTime - this._formationDrawingStartingTime.Value < 0.3f)
			{
				flag = true;
				worldPosition = this._formationDrawingStartingPosition.Value;
			}
			if (!flag)
			{
				WorldPosition worldPosition2;
				if (!this.TryGetScreenMiddleToWorldPosition(out worldPosition2))
				{
					return;
				}
				worldPosition = worldPosition2;
			}
			WorldPosition worldPosition3;
			if (this._mouseOverDirection == 1)
			{
				worldPosition3 = worldPosition;
				worldPosition = this._formationDrawingStartingPosition.Value;
			}
			else
			{
				worldPosition3 = this._formationDrawingStartingPosition.Value;
			}
			if (!this.OrderFlag.IsPositionOnValidGround(worldPosition3))
			{
				return;
			}
			Vec2 vec2;
			if (this._restrictOrdersToDeploymentBoundaries && base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
			{
				IMissionDeploymentPlan deploymentPlan = base.Mission.DeploymentPlan;
				Team playerTeam = base.Mission.PlayerTeam;
				vec2 = worldPosition3.AsVec2;
				if (!deploymentPlan.IsPositionInsideDeploymentBoundaries(playerTeam, in vec2))
				{
					return;
				}
			}
			bool flag2 = !base.DebugInput.IsControlDown();
			this.UpdateFormationDrawingForMovementOrder(giveOrder, worldPosition3, worldPosition, flag2);
			Vec2 deltaMousePosition = this._deltaMousePosition;
			float num = 1f;
			vec2 = base.Input.GetMousePositionRanged() - this._lastMousePosition;
			this._deltaMousePosition = deltaMousePosition * MathF.Max(num - vec2.Length * 10f, 0f);
			this._lastMousePosition = base.Input.GetMousePositionRanged();
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002A664 File Offset: 0x00028864
		private void UpdateFormationDrawingForMovementOrder(bool giveOrder, WorldPosition formationRealStartingPosition, WorldPosition formationRealEndingPosition, bool isFormationLayoutVertical)
		{
			this._isDrawnThisFrame = true;
			List<WorldPosition> list;
			this.OrderController.SimulateNewOrderWithPositionAndDirection(formationRealStartingPosition, formationRealEndingPosition, out list, isFormationLayoutVertical);
			if (giveOrder)
			{
				if (!isFormationLayoutVertical)
				{
					this.OrderController.SetOrderWithTwoPositions(OrderType.MoveToLineSegmentWithHorizontalLayout, formationRealStartingPosition, formationRealEndingPosition);
				}
				else
				{
					this.OrderController.SetOrderWithTwoPositions(OrderType.MoveToLineSegment, formationRealStartingPosition, formationRealEndingPosition);
				}
			}
			int num = 0;
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				Vec3 groundedVec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in groundedVec, giveOrder, -1f);
				num++;
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002A708 File Offset: 0x00028908
		private void HandleMouseDown()
		{
			if (this.HasSelectedFormations())
			{
				switch (this.ActiveCursorState)
				{
				case OrderTroopPlacer.CursorState.Invisible:
				case OrderTroopPlacer.CursorState.Ground:
					break;
				case OrderTroopPlacer.CursorState.Normal:
				{
					this._formationDrawingMode = true;
					WorldPosition worldPosition;
					if (this.TryGetScreenMiddleToWorldPosition(out worldPosition))
					{
						this._formationDrawingStartingPosition = new WorldPosition?(worldPosition);
						this._formationDrawingStartingPointOfMouse = new Vec2?(base.Input.GetMousePositionPixel());
						this._formationDrawingStartingTime = new float?(base.Mission.CurrentTime);
						return;
					}
					this._formationDrawingStartingPosition = null;
					this._formationDrawingStartingPointOfMouse = null;
					this._formationDrawingStartingTime = null;
					return;
				}
				case OrderTroopPlacer.CursorState.Rotation:
					if (this._mouseOverFormation.CountOfUnits > 0)
					{
						this.HideNonSelectedOrderRotationEntities(this._mouseOverFormation);
						this.OrderController.ClearSelectedFormations();
						this.OrderController.SelectFormation(this._mouseOverFormation);
						this._formationDrawingMode = true;
						WorldPosition worldPosition2 = this._mouseOverFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
						Vec2 direction = this._mouseOverFormation.Direction;
						direction.RotateCCW(-1.5707964f);
						this._formationDrawingStartingPosition = new WorldPosition?(worldPosition2);
						this._formationDrawingStartingPosition.Value.SetVec2(this._formationDrawingStartingPosition.Value.AsVec2 + direction * ((this._mouseOverDirection == 1) ? 0.5f : (-0.5f)) * this._mouseOverFormation.Width);
						WorldPosition worldPosition3 = worldPosition2;
						worldPosition3.SetVec2(worldPosition3.AsVec2 + direction * ((this._mouseOverDirection == 1) ? (-0.5f) : 0.5f) * this._mouseOverFormation.Width);
						Vec2 vec = base.MissionScreen.SceneView.WorldPointToScreenPoint(this.GetGroundedVec3(worldPosition3));
						Vec2 screenPoint = this.GetScreenPoint();
						this._deltaMousePosition = vec - screenPoint;
						this._lastMousePosition = base.Input.GetMousePositionRanged();
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002A8FC File Offset: 0x00028AFC
		private void HandleMouseUp()
		{
			if (this.ActiveCursorState == OrderTroopPlacer.CursorState.Ground)
			{
				if (this.IsDrawingFacing || this._wasDrawingFacing)
				{
					this.UpdateFormationDrawingForFacingOrder(true);
				}
				else if (this.IsDrawingForming || this._wasDrawingForming)
				{
					this.UpdateFormationDrawingForFormingOrder(true);
				}
				else
				{
					this.UpdateFormationDrawing(true);
				}
				if (this.IsDeployment)
				{
					Action onUnitDeployed = this.OnUnitDeployed;
					if (onUnitDeployed != null)
					{
						onUnitDeployed();
					}
					UISoundsHelper.PlayUISound("event:/ui/mission/deploy");
				}
			}
			this._formationDrawingMode = false;
			this._deltaMousePosition = Vec2.Zero;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002A980 File Offset: 0x00028B80
		private void AddOrderPositionEntity(int entityIndex, in Vec3 groundPosition, bool fadeOut, float alpha = -1f)
		{
			while (this._orderPositionEntities.Count <= entityIndex)
			{
				GameEntity gameEntity = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
				gameEntity.EntityFlags |= EntityFlags.NotAffectedBySeason;
				MetaMesh copy = MetaMesh.GetCopy("order_flag_small", true, false);
				gameEntity.AddComponent(copy);
				gameEntity.SetVisibilityExcludeParents(false);
				this._orderPositionEntities.Add(gameEntity);
			}
			GameEntity gameEntity2 = this._orderPositionEntities[entityIndex];
			Mat3 identity = Mat3.Identity;
			MatrixFrame matrixFrame = new MatrixFrame(in identity, in groundPosition);
			gameEntity2.SetFrame(ref matrixFrame, true);
			if (alpha != -1f)
			{
				gameEntity2.SetVisibilityExcludeParents(true);
				gameEntity2.SetAlpha(alpha);
				return;
			}
			if (fadeOut)
			{
				gameEntity2.FadeOut(0.3f, false);
				return;
			}
			gameEntity2.FadeIn(true);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002AA40 File Offset: 0x00028C40
		private void HideNonSelectedOrderRotationEntities(Formation formation)
		{
			for (int i = 0; i < this._orderRotationEntities.Count; i++)
			{
				GameEntity gameEntity = this._orderRotationEntities[i];
				if (gameEntity == null && gameEntity.IsVisibleIncludeParents() && this.OrderController.SelectedFormations.ElementAt<Formation>(i / 2) != formation)
				{
					gameEntity.SetVisibilityExcludeParents(false);
					gameEntity.BodyFlag |= BodyFlags.Disabled;
				}
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002AAAC File Offset: 0x00028CAC
		private void HideOrderPositionEntities()
		{
			foreach (GameEntity gameEntity in this._orderPositionEntities)
			{
				gameEntity.HideIfNotFadingOut();
			}
			for (int i = 0; i < this._orderRotationEntities.Count; i++)
			{
				GameEntity gameEntity2 = this._orderRotationEntities[i];
				gameEntity2.SetVisibilityExcludeParents(false);
				gameEntity2.BodyFlag |= BodyFlags.Disabled;
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002AB34 File Offset: 0x00028D34
		[Conditional("DEBUG")]
		private void DebugTick(float dt)
		{
			bool initialized = this._initialized;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002AB40 File Offset: 0x00028D40
		private void Reset()
		{
			this._isMouseDown = false;
			this._formationDrawingMode = false;
			this._formationDrawingStartingPosition = null;
			this._formationDrawingStartingPointOfMouse = null;
			this._formationDrawingStartingTime = null;
			this._mouseOverFormation = null;
			this.ActiveCursorState = this.GetCursorState();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002AB94 File Offset: 0x00028D94
		public override void OnMissionScreenTick(float dt)
		{
			if (!this._initialized)
			{
				return;
			}
			this.ActiveCursorState = this.GetCursorState();
			base.OnMissionScreenTick(dt);
			if (!this.CanUpdate())
			{
				return;
			}
			this._isDrawnThisFrame = false;
			if (this.SuspendTroopPlacer)
			{
				return;
			}
			if (base.Input.IsKeyPressed(InputKey.LeftMouseButton) || base.Input.IsKeyPressed(InputKey.ControllerRTrigger))
			{
				this._isMouseDown = true;
				this.HandleMouseDown();
			}
			if ((base.Input.IsKeyReleased(InputKey.LeftMouseButton) || base.Input.IsKeyReleased(InputKey.ControllerRTrigger)) && this._isMouseDown)
			{
				this._isMouseDown = false;
				this.HandleMouseUp();
			}
			else if ((base.Input.IsKeyDown(InputKey.LeftMouseButton) || base.Input.IsKeyDown(InputKey.ControllerRTrigger)) && this._isMouseDown)
			{
				if (this.formationDrawTimer.Check(MBCommon.GetApplicationTime()) && !this.IsDrawingFacing && !this.IsDrawingForming && this.ActiveCursorState == OrderTroopPlacer.CursorState.Ground && this.GetGroundOrNormalCursor() == OrderTroopPlacer.CursorState.Ground)
				{
					this.UpdateFormationDrawing(false);
				}
			}
			else if (this.IsDrawingForced)
			{
				if (this.formationDrawTimer.Check(MBCommon.GetApplicationTime()))
				{
					this.Reset();
					this.HandleMouseDown();
					this.UpdateFormationDrawing(false);
				}
			}
			else if (this.IsDrawingFacing || this._wasDrawingFacing)
			{
				if (this.IsDrawingFacing)
				{
					this.Reset();
					this.UpdateFormationDrawingForFacingOrder(false);
				}
			}
			else if (this.IsDrawingForming || this._wasDrawingForming)
			{
				if (this.IsDrawingForming)
				{
					this.Reset();
					this.UpdateFormationDrawingForFormingOrder(false);
				}
			}
			else if (this._wasDrawingForced)
			{
				this.Reset();
			}
			else
			{
				this.UpdateFormationDrawingForDestination(false);
			}
			if (!base.Input.IsKeyDown(InputKey.LeftMouseButton) && !base.Input.IsKeyDown(InputKey.ControllerRTrigger) && this._isMouseDown)
			{
				this.Reset();
			}
			foreach (GameEntity gameEntity in this._orderPositionEntities)
			{
				gameEntity.SetPreviousFrameInvalid();
			}
			foreach (GameEntity gameEntity2 in this._orderRotationEntities)
			{
				gameEntity2.SetPreviousFrameInvalid();
			}
			this._wasDrawingForced = this.IsDrawingForced;
			this._wasDrawingFacing = this.IsDrawingFacing;
			this._wasDrawingForming = this.IsDrawingForming;
			this._wasDrawnPreviousFrame = this._isDrawnThisFrame;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0002AE38 File Offset: 0x00029038
		private bool IsDeployment
		{
			get
			{
				Mission mission = base.Mission;
				return mission != null && mission.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x0400032F RID: 815
		private bool _suspendTroopPlacer;

		// Token: 0x04000331 RID: 817
		public bool IsDrawingForced;

		// Token: 0x04000332 RID: 818
		public bool IsDrawingFacing;

		// Token: 0x04000333 RID: 819
		public bool IsDrawingForming;

		// Token: 0x04000334 RID: 820
		public Action OnUnitDeployed;

		// Token: 0x04000335 RID: 821
		private bool _isMouseDown;

		// Token: 0x04000336 RID: 822
		private List<GameEntity> _orderPositionEntities;

		// Token: 0x04000337 RID: 823
		private List<GameEntity> _orderRotationEntities;

		// Token: 0x04000338 RID: 824
		private bool _formationDrawingMode;

		// Token: 0x04000339 RID: 825
		private Formation _mouseOverFormation;

		// Token: 0x0400033A RID: 826
		private Vec2 _lastMousePosition;

		// Token: 0x0400033B RID: 827
		private Vec2 _deltaMousePosition;

		// Token: 0x0400033C RID: 828
		private int _mouseOverDirection;

		// Token: 0x0400033D RID: 829
		private WorldPosition? _formationDrawingStartingPosition;

		// Token: 0x0400033E RID: 830
		private Vec2? _formationDrawingStartingPointOfMouse;

		// Token: 0x0400033F RID: 831
		private float? _formationDrawingStartingTime;

		// Token: 0x04000340 RID: 832
		private bool _restrictOrdersToDeploymentBoundaries;

		// Token: 0x04000341 RID: 833
		private bool _initialized;

		// Token: 0x04000343 RID: 835
		private Timer formationDrawTimer;

		// Token: 0x04000344 RID: 836
		private bool _wasDrawingForced;

		// Token: 0x04000345 RID: 837
		private bool _wasDrawingFacing;

		// Token: 0x04000346 RID: 838
		private bool _wasDrawingForming;

		// Token: 0x04000347 RID: 839
		private GameEntity _widthEntityLeft;

		// Token: 0x04000348 RID: 840
		private GameEntity _widthEntityRight;

		// Token: 0x04000349 RID: 841
		private bool _isDrawnThisFrame;

		// Token: 0x0400034A RID: 842
		private bool _wasDrawnPreviousFrame;

		// Token: 0x0400034B RID: 843
		private OrderController _orderController;

		// Token: 0x020000F1 RID: 241
		public enum CursorState
		{
			// Token: 0x04000431 RID: 1073
			Invisible,
			// Token: 0x04000432 RID: 1074
			Normal,
			// Token: 0x04000433 RID: 1075
			Ground,
			// Token: 0x04000434 RID: 1076
			Rotation,
			// Token: 0x04000435 RID: 1077
			Count,
			// Token: 0x04000436 RID: 1078
			OrderableEntity
		}
	}
}
