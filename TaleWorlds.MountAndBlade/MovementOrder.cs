using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015B RID: 347
	public struct MovementOrder
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x000379E5 File Offset: 0x00035BE5
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x000379ED File Offset: 0x00035BED
		public Formation TargetFormation { get; private set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x000379F6 File Offset: 0x00035BF6
		public Agent _targetAgent { get; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00037A00 File Offset: 0x00035C00
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case MovementOrder.MovementOrderEnum.AttackEntity:
					return OrderType.AttackEntity;
				case MovementOrder.MovementOrderEnum.Charge:
					return OrderType.Charge;
				case MovementOrder.MovementOrderEnum.ChargeToTarget:
					return OrderType.ChargeWithTarget;
				case MovementOrder.MovementOrderEnum.Follow:
					return OrderType.FollowMe;
				case MovementOrder.MovementOrderEnum.FollowEntity:
					return OrderType.FollowEntity;
				case MovementOrder.MovementOrderEnum.Move:
					return OrderType.Move;
				case MovementOrder.MovementOrderEnum.Retreat:
					return OrderType.Retreat;
				case MovementOrder.MovementOrderEnum.Stop:
					return OrderType.StandYourGround;
				case MovementOrder.MovementOrderEnum.Advance:
					return OrderType.Advance;
				case MovementOrder.MovementOrderEnum.FallBack:
					return OrderType.FallBack;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "OrderType", 114);
				return OrderType.Move;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00037A7C File Offset: 0x00035C7C
		public MovementOrder.MovementStateEnum MovementState
		{
			get
			{
				MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
				if (orderEnum - MovementOrder.MovementOrderEnum.Charge > 1)
				{
					if (orderEnum == MovementOrder.MovementOrderEnum.Retreat)
					{
						return MovementOrder.MovementStateEnum.Retreat;
					}
					if (orderEnum != MovementOrder.MovementOrderEnum.Stop)
					{
						return MovementOrder.MovementStateEnum.Hold;
					}
					return MovementOrder.MovementStateEnum.StandGround;
				}
				else
				{
					if (this._position.IsValid)
					{
						return MovementOrder.MovementStateEnum.Hold;
					}
					return MovementOrder.MovementStateEnum.Charge;
				}
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00037AB8 File Offset: 0x00035CB8
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.Charge)
			{
				switch (orderEnum)
				{
				case MovementOrder.MovementOrderEnum.Retreat:
					this._positionLambda = null;
					goto IL_0050;
				case MovementOrder.MovementOrderEnum.Advance:
					this._positionLambda = null;
					goto IL_0050;
				case MovementOrder.MovementOrderEnum.FallBack:
					this._positionLambda = null;
					goto IL_0050;
				}
				this._positionLambda = null;
			}
			else
			{
				this._positionLambda = null;
			}
			IL_0050:
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00037BA8 File Offset: 0x00035DA8
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, Formation targetFormation)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = null;
			this.TargetFormation = targetFormation;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00037C58 File Offset: 0x00035E58
		private WorldPosition ComputeAttackEntityWaitPosition(Formation formation, WeakGameEntity targetEntity)
		{
			Scene scene = formation.Team.Mission.Scene;
			WorldPosition worldPosition = new WorldPosition(scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
			Vec2 vec = formation.CachedAveragePosition - worldPosition.AsVec2;
			MatrixFrame matrixFrame = targetEntity.GetGlobalFrame();
			Vec2 vec2 = matrixFrame.rotation.f.AsVec2.Normalized();
			Vec2 vec3 = ((vec.DotProduct(vec2) >= 0f) ? vec2 : (-vec2));
			WorldPosition worldPosition2 = worldPosition;
			worldPosition2.SetVec2(worldPosition.AsVec2 + vec3 * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition2, formation.CachedMedianPosition))
			{
				return worldPosition2;
			}
			WorldPosition worldPosition3 = worldPosition;
			worldPosition3.SetVec2(worldPosition.AsVec2 - vec3 * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition3;
			}
			worldPosition3 = worldPosition;
			Vec2 asVec = worldPosition.AsVec2;
			matrixFrame = targetEntity.GetGlobalFrame();
			worldPosition3.SetVec2(asVec + matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition3;
			}
			worldPosition3 = worldPosition;
			Vec2 asVec2 = worldPosition.AsVec2;
			matrixFrame = targetEntity.GetGlobalFrame();
			worldPosition3.SetVec2(asVec2 - matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
			if (!scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition2;
			}
			return worldPosition3;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00037DF4 File Offset: 0x00035FF4
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, GameEntity targetEntity, bool surroundEntity)
		{
			targetEntity.GetFirstScriptOfType<UsableMachine>();
			this.OrderEnum = orderEnum;
			this._positionLambda = delegate(Formation f)
			{
				WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
				Vec2 vec = f.CachedAveragePosition - worldPosition.AsVec2;
				MatrixFrame matrixFrame = targetEntity.GetGlobalFrame();
				Vec2 vec2 = matrixFrame.rotation.f.AsVec2.Normalized();
				Vec2 vec3 = ((vec.DotProduct(vec2) >= 0f) ? vec2 : (-vec2));
				WorldPosition worldPosition2 = worldPosition;
				worldPosition2.SetVec2MT(worldPosition.AsVec2 + vec3 * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition2, f.CachedMedianPosition))
				{
					return worldPosition2;
				}
				WorldPosition worldPosition3 = worldPosition;
				worldPosition3.SetVec2MT(worldPosition.AsVec2 - vec3 * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				worldPosition3 = worldPosition;
				Vec2 asVec = worldPosition.AsVec2;
				matrixFrame = targetEntity.GetGlobalFrame();
				worldPosition3.SetVec2MT(asVec + matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				worldPosition3 = worldPosition;
				Vec2 asVec2 = worldPosition.AsVec2;
				matrixFrame = targetEntity.GetGlobalFrame();
				worldPosition3.SetVec2MT(asVec2 - matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				return worldPosition2;
			};
			this.TargetEntity = targetEntity;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this.TargetFormation = null;
			this._targetAgent = null;
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00037ECC File Offset: 0x000360CC
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, Agent targetAgent)
		{
			this.OrderEnum = orderEnum;
			WorldPosition targetAgentPos = targetAgent.GetWorldPosition();
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				this._positionLambda = delegate(Formation f)
				{
					WorldPosition targetAgentPos3 = targetAgentPos;
					targetAgentPos3.SetVec2(targetAgentPos3.AsVec2 - f.GetMiddleFrontUnitPositionOffset());
					return targetAgentPos3;
				};
			}
			else
			{
				this._positionLambda = delegate(Formation f)
				{
					WorldPosition targetAgentPos2 = targetAgentPos;
					targetAgentPos2.SetVec2(targetAgentPos2.AsVec2 - 4f * (f.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - targetAgentPos.AsVec2).Normalized());
					Vec2 asVec = targetAgentPos2.AsVec2;
					WorldPosition lastPosition = f.GetReadonlyMovementOrderReference()._lastPosition;
					if (asVec.DistanceSquared(lastPosition.AsVec2) > 6.25f)
					{
						return targetAgentPos2;
					}
					return f.GetReadonlyMovementOrderReference()._lastPosition;
				};
			}
			this._targetAgent = targetAgent;
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._tickTimer = new Timer(targetAgent.Mission.CurrentTime, 0.5f, true);
			this._lastPosition = targetAgentPos;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00037FB0 File Offset: 0x000361B0
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, GameEntity targetEntity)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = delegate(Formation f)
			{
				WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
				worldPosition.SetVec2(worldPosition.AsVec2);
				return worldPosition;
			};
			this.TargetEntity = targetEntity;
			this.TargetFormation = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003807C File Offset: 0x0003627C
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, WorldPosition position)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = null;
			this._isFacingDirection = false;
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._position = position;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00038128 File Offset: 0x00036328
		public override bool Equals(object obj)
		{
			if (obj is MovementOrder)
			{
				MovementOrder movementOrder = (MovementOrder)obj;
				return (in movementOrder) == this;
			}
			return false;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00038153 File Offset: 0x00036353
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003815B File Offset: 0x0003635B
		public static bool operator !=(in MovementOrder m, MovementOrder obj)
		{
			return m.OrderEnum != obj.OrderEnum;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003816E File Offset: 0x0003636E
		public static bool operator ==(in MovementOrder m, MovementOrder obj)
		{
			return m.OrderEnum == obj.OrderEnum;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003817E File Offset: 0x0003637E
		public static MovementOrder MovementOrderChargeToTarget(Formation targetFormation)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.ChargeToTarget, targetFormation);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00038187 File Offset: 0x00036387
		public static MovementOrder MovementOrderFollow(Agent targetAgent)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.Follow, targetAgent);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00038190 File Offset: 0x00036390
		public static MovementOrder MovementOrderFollowEntity(GameEntity targetEntity)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.FollowEntity, targetEntity);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00038199 File Offset: 0x00036399
		public static MovementOrder MovementOrderMove(WorldPosition position)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.Move, position);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000381A2 File Offset: 0x000363A2
		public static MovementOrder MovementOrderAttackEntity(GameEntity targetEntity, bool surroundEntity)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.AttackEntity, targetEntity, surroundEntity);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000381AC File Offset: 0x000363AC
		public static int GetMovementOrderDefensiveness(MovementOrder.MovementOrderEnum orderEnum)
		{
			if (orderEnum == MovementOrder.MovementOrderEnum.Charge || orderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000381B9 File Offset: 0x000363B9
		public static int GetMovementOrderDefensivenessChange(MovementOrder.MovementOrderEnum previousOrderEnum, MovementOrder.MovementOrderEnum nextOrderEnum)
		{
			if (previousOrderEnum == MovementOrder.MovementOrderEnum.Charge || previousOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
			{
				if (nextOrderEnum != MovementOrder.MovementOrderEnum.Charge && nextOrderEnum != MovementOrder.MovementOrderEnum.ChargeToTarget)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (nextOrderEnum == MovementOrder.MovementOrderEnum.Charge || nextOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000381DC File Offset: 0x000363DC
		private static void RetreatAux(Formation formation)
		{
			for (int i = formation.Detachments.Count - 1; i >= 0; i--)
			{
				formation.LeaveDetachment(formation.Detachments[i]);
			}
			formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
			{
				if (agent.IsAIControlled)
				{
					agent.Retreat(true);
				}
			});
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00038238 File Offset: 0x00036438
		private static WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(Formation f, WorldPosition originalPosition)
		{
			float num = 1f;
			WorldPosition alternatePositionForNavmeshlessOrOutOfBoundsPosition = Mission.Current.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(originalPosition.AsVec2 - f.CachedAveragePosition, originalPosition, ref num);
			FormationAI ai = f.AI;
			if (((ai != null) ? ai.ActiveBehavior : null) != null)
			{
				f.AI.ActiveBehavior.NavmeshlessTargetPositionPenalty = num;
			}
			return alternatePositionForNavmeshlessOrOutOfBoundsPosition;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00038290 File Offset: 0x00036490
		private void GetPositionAuxFollow(Formation f)
		{
			Vec2 vec = Vec2.Zero;
			if (this._followState != MovementOrder.FollowState.Move && this._targetAgent.MountAgent != null)
			{
				vec += f.Direction * -2f;
			}
			if (this._followState == MovementOrder.FollowState.Move && f.PhysicalClass.IsMounted())
			{
				vec += 2f * this._targetAgent.Velocity.AsVec2;
			}
			else if (this._followState == MovementOrder.FollowState.Move)
			{
				f.PhysicalClass.IsMounted();
			}
			WorldPosition worldPosition = this._targetAgent.GetWorldPosition();
			worldPosition.SetVec2(worldPosition.AsVec2 - f.GetMiddleFrontUnitPositionOffset() + vec);
			if (this._followState == MovementOrder.FollowState.Stop || this._followState == MovementOrder.FollowState.Depart)
			{
				float num = (f.PhysicalClass.IsMounted() ? 4f : 2.5f);
				if (Mission.Current.IsTeleportingAgents || worldPosition.AsVec2.DistanceSquared(this._lastPosition.AsVec2) > num * num)
				{
					this._lastPosition = worldPosition;
					return;
				}
			}
			else
			{
				this._lastPosition = worldPosition;
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000383B4 File Offset: 0x000365B4
		public Vec2 GetPosition(Formation f)
		{
			return this.CreateNewOrderWorldPositionMT(f, WorldPosition.WorldPositionEnforcedCache.None).AsVec2;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000383D4 File Offset: 0x000365D4
		public Vec2 GetTargetVelocity()
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
			case MovementOrder.MovementOrderEnum.Charge:
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
			case MovementOrder.MovementOrderEnum.FollowEntity:
			case MovementOrder.MovementOrderEnum.Move:
			case MovementOrder.MovementOrderEnum.Retreat:
			case MovementOrder.MovementOrderEnum.Stop:
			case MovementOrder.MovementOrderEnum.Advance:
			case MovementOrder.MovementOrderEnum.FallBack:
				return Vec2.Zero;
			case MovementOrder.MovementOrderEnum.Follow:
				return this._targetAgent.AverageVelocity.AsVec2;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetTargetVelocity", 842);
			return Vec2.Zero;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00038458 File Offset: 0x00036658
		public WorldPosition CreateNewOrderWorldPositionMT(Formation f, WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			object movementOrderPositionLock = f.MovementOrderPositionLock;
			WorldPosition worldPosition;
			lock (movementOrderPositionLock)
			{
				if (!this.IsApplicable(f))
				{
					worldPosition = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				else
				{
					MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
					WorldPosition worldPosition2;
					if (orderEnum != MovementOrder.MovementOrderEnum.Follow)
					{
						if (orderEnum - MovementOrder.MovementOrderEnum.Advance > 1)
						{
							Func<Formation, WorldPosition> positionLambda = this._positionLambda;
							worldPosition2 = ((positionLambda != null) ? positionLambda(f) : this._position);
						}
						else
						{
							worldPosition2 = this.GetPositionAux(f, worldPositionEnforcedCache);
						}
					}
					else
					{
						this.GetPositionAuxFollow(f);
						worldPosition2 = this._lastPosition;
					}
					if (Mission.Current.Mode == MissionMode.Deployment)
					{
						if (!Mission.Current.IsOrderPositionAvailable(in worldPosition2, f.Team))
						{
							worldPosition2 = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
						}
						else
						{
							if (Mission.Current.DeploymentPlan.SupportsNavmesh(f.Team))
							{
								Mission.Current.DeploymentPlan.ProjectPositionToDeploymentBoundaries(f.Team, ref worldPosition2);
							}
							if (!Mission.Current.IsOrderPositionAvailable(in worldPosition2, f.Team))
							{
								worldPosition2 = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
							}
						}
					}
					bool flag2 = false;
					if (this._getPositionFirstSectionCache.AsVec2 != worldPosition2.AsVec2)
					{
						this._getPositionIsNavmeshlessCache = false;
						if (worldPosition2.IsValid)
						{
							if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
							{
								if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
								{
									worldPosition2.GetGroundVec3MT();
								}
							}
							else
							{
								worldPosition2.GetNavMeshVec3MT();
							}
							this._getPositionFirstSectionCache = worldPosition2;
							if (this.OrderEnum != MovementOrder.MovementOrderEnum.Follow && (worldPosition2.GetNavMeshMT() == UIntPtr.Zero || !Mission.Current.IsPositionInsideBoundaries(worldPosition2.AsVec2)))
							{
								worldPosition2 = MovementOrder.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(f, worldPosition2);
								if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
								{
									if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
									{
										worldPosition2.GetGroundVec3MT();
									}
								}
								else
								{
									worldPosition2.GetNavMeshVec3MT();
								}
							}
							else
							{
								flag2 = true;
								this._getPositionIsNavmeshlessCache = true;
							}
							this._getPositionResultCache = worldPosition2;
						}
					}
					else
					{
						if (this._getPositionResultCache.IsValid)
						{
							if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
							{
								if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
								{
									this._getPositionResultCache.GetGroundVec3MT();
								}
							}
							else
							{
								this._getPositionResultCache.GetNavMeshVec3MT();
							}
						}
						worldPosition2 = this._getPositionResultCache;
					}
					if (this._getPositionIsNavmeshlessCache || flag2)
					{
						FormationAI ai = f.AI;
						if (((ai != null) ? ai.ActiveBehavior : null) != null)
						{
							f.AI.ActiveBehavior.NavmeshlessTargetPositionPenalty = 1f;
						}
					}
					worldPosition = worldPosition2;
				}
			}
			return worldPosition;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000386A0 File Offset: 0x000368A0
		public void ResetPositionCache()
		{
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000386B8 File Offset: 0x000368B8
		public bool AreOrdersPracticallySame(MovementOrder m1, MovementOrder m2, bool isAIControlled)
		{
			if (m1.OrderEnum != m2.OrderEnum)
			{
				return false;
			}
			switch (m1.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				return m1.TargetEntity == m2.TargetEntity;
			case MovementOrder.MovementOrderEnum.Charge:
				return true;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				return m1.TargetFormation == m2.TargetFormation;
			case MovementOrder.MovementOrderEnum.Follow:
				return m1._targetAgent == m2._targetAgent;
			case MovementOrder.MovementOrderEnum.FollowEntity:
				return m1.TargetEntity == m2.TargetEntity;
			case MovementOrder.MovementOrderEnum.Move:
				return isAIControlled && m1._position.AsVec2.DistanceSquared(m2._position.AsVec2) < 1f;
			case MovementOrder.MovementOrderEnum.Retreat:
				return true;
			case MovementOrder.MovementOrderEnum.Stop:
				return true;
			case MovementOrder.MovementOrderEnum.Advance:
				return true;
			case MovementOrder.MovementOrderEnum.FallBack:
				return true;
			}
			return true;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00038790 File Offset: 0x00036990
		public void OnApply(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				formation.FormAttackEntityDetachment(this.TargetEntity);
				break;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				formation.SetTargetFormation(this.TargetFormation);
				break;
			case MovementOrder.MovementOrderEnum.Follow:
				formation.Arrangement.ReserveMiddleFrontUnitPosition(this._targetAgent);
				break;
			case MovementOrder.MovementOrderEnum.Move:
				formation.SetPositioning(new WorldPosition?(this.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None)), null, null);
				break;
			case MovementOrder.MovementOrderEnum.Retreat:
				MovementOrder.RetreatAux(formation);
				break;
			}
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if ((orderEnum == MovementOrder.MovementOrderEnum.Charge || orderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && this.GetPosition(formation).IsValid)
			{
				orderEnum = MovementOrder.MovementOrderEnum.Move;
			}
			formation.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.RefreshBehaviorValues(orderEnum, formation.ArrangementOrder.OrderEnum);
			}, null);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000388AC File Offset: 0x00036AAC
		public void OnCancel(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				formation.DisbandAttackEntityDetachment();
				return;
			case MovementOrder.MovementOrderEnum.Charge:
				this.CancelChargeOrder(formation);
				return;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				formation.SetTargetFormation(null);
				this.CancelChargeOrder(formation);
				return;
			case MovementOrder.MovementOrderEnum.Follow:
				formation.Arrangement.ReleaseMiddleFrontUnitPosition();
				return;
			case MovementOrder.MovementOrderEnum.FollowEntity:
			case (MovementOrder.MovementOrderEnum)6:
			case MovementOrder.MovementOrderEnum.Move:
			case MovementOrder.MovementOrderEnum.Stop:
			case MovementOrder.MovementOrderEnum.Advance:
				break;
			case MovementOrder.MovementOrderEnum.Retreat:
				formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
				{
					if (agent.IsAIControlled)
					{
						agent.StopRetreatingMoraleComponent();
					}
				});
				return;
			case MovementOrder.MovementOrderEnum.FallBack:
				if (!Mission.Current.IsPositionInsideBoundaries(this.GetPosition(formation)))
				{
					formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
					{
						if (agent.IsAIControlled)
						{
							agent.StopRetreatingMoraleComponent();
						}
					});
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00038980 File Offset: 0x00036B80
		public void OnUnitJoinOrLeave(Formation formation, Agent unit, bool isJoining)
		{
			if (!this.IsApplicable(formation))
			{
				return;
			}
			if (isJoining)
			{
				if (this.OrderEnum == MovementOrder.MovementOrderEnum.Retreat)
				{
					if (unit.IsAIControlled)
					{
						unit.Retreat(false);
						return;
					}
				}
				else
				{
					if ((this.OrderEnum == MovementOrder.MovementOrderEnum.Charge || this.OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && this.GetPosition(formation).IsValid)
					{
						unit.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Move, formation.ArrangementOrder.OrderEnum);
						return;
					}
					unit.RefreshBehaviorValues(this.OrderEnum, formation.ArrangementOrder.OrderEnum);
					return;
				}
			}
			else if (this.OrderEnum == MovementOrder.MovementOrderEnum.Retreat && unit.IsAIControlled && unit.IsActive())
			{
				unit.StopRetreatingMoraleComponent();
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00038A20 File Offset: 0x00036C20
		public bool IsApplicable(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
			{
				UsableMachine firstScriptOfType = this.TargetEntity.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType != null)
				{
					return !firstScriptOfType.IsDestroyed;
				}
				DestructableComponent firstScriptOfType2 = this.TargetEntity.GetFirstScriptOfType<DestructableComponent>();
				return firstScriptOfType2 != null && !firstScriptOfType2.IsDestroyed;
			}
			case MovementOrder.MovementOrderEnum.Charge:
			{
				for (int i = 0; i < Mission.Current.Teams.Count; i++)
				{
					Team team = Mission.Current.Teams[i];
					if (team.IsEnemyOf(formation.Team) && team.ActiveAgents.Count > 0)
					{
						return true;
					}
				}
				return false;
			}
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				return this.TargetFormation.CountOfUnits > 0;
			case MovementOrder.MovementOrderEnum.Follow:
				return this._targetAgent.IsActive();
			case MovementOrder.MovementOrderEnum.FollowEntity:
			{
				UsableMachine firstScriptOfType3 = this.TargetEntity.GetFirstScriptOfType<UsableMachine>();
				return firstScriptOfType3 == null || !firstScriptOfType3.IsDestroyed;
			}
			default:
				return true;
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00038B11 File Offset: 0x00036D11
		private bool IsInstance()
		{
			return this.OrderEnum != MovementOrder.MovementOrderEnum.Invalid && this.OrderEnum != MovementOrder.MovementOrderEnum.Charge && this.OrderEnum != MovementOrder.MovementOrderEnum.Retreat && this.OrderEnum != MovementOrder.MovementOrderEnum.Stop && this.OrderEnum != MovementOrder.MovementOrderEnum.Advance && this.OrderEnum != MovementOrder.MovementOrderEnum.FallBack;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00038B50 File Offset: 0x00036D50
		public bool Tick(Formation formation)
		{
			object obj = !this.IsInstance() || this._tickTimer.Check(Mission.Current.CurrentTime);
			this.TickAux();
			object obj2 = obj;
			if (obj2 != null)
			{
				this.TickOccasionally(formation, this._tickTimer.PreviousDeltaTime);
			}
			return obj2 != null;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00038B90 File Offset: 0x00036D90
		private void TickOccasionally(Formation formation, float dt)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.AttackEntity)
			{
				if (orderEnum - MovementOrder.MovementOrderEnum.Charge > 1)
				{
					if (orderEnum == MovementOrder.MovementOrderEnum.FallBack && !Mission.Current.IsPositionInsideBoundaries(this.GetPosition(formation)))
					{
						MovementOrder.RetreatAux(formation);
						return;
					}
				}
				else
				{
					Team team = formation.Team;
					TeamAISiegeComponent teamAISiegeComponent = ((team != null) ? team.TeamAI : null) as TeamAISiegeComponent;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					if (!Mission.Current.IsTeleportingAgents && teamAISiegeComponent != null)
					{
						flag4 = TeamAISiegeComponent.IsFormationInsideCastle(formation, false, 0.4f);
						bool flag5 = false;
						foreach (Team team2 in formation.Team.Mission.Teams)
						{
							if (team2.IsEnemyOf(formation.Team))
							{
								foreach (Formation formation2 in team2.FormationsIncludingEmpty)
								{
									if (formation2.CountOfUnits > 0 && flag4 == TeamAISiegeComponent.IsFormationInsideCastle(formation2, false, 0.4f))
									{
										flag5 = true;
										break;
									}
								}
								if (flag5)
								{
									break;
								}
							}
						}
						if (!flag5)
						{
							if (flag4 && !teamAISiegeComponent.CalculateIsAnyLaneOpenToGoOutside())
							{
								CastleGate gateToGetThrough = ((!teamAISiegeComponent.InnerGate.IsGateOpen) ? teamAISiegeComponent.InnerGate : teamAISiegeComponent.OuterGate);
								if (gateToGetThrough != null)
								{
									if (!gateToGetThrough.IsUsedByFormation(formation))
									{
										formation.StartUsingMachine(gateToGetThrough, true);
										SiegeLane siegeLane;
										if ((siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == gateToGetThrough.DefenseSide)) == null)
										{
											siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == FormationAI.BehaviorSide.Middle);
										}
										SiegeLane siegeLane2 = siegeLane;
										TacticalPosition tacticalPosition;
										if (siegeLane2 == null)
										{
											tacticalPosition = null;
										}
										else
										{
											ICastleKeyPosition castleKeyPosition = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>(delegate(ICastleKeyPosition dp)
											{
												UsableMachine usableMachine;
												return (usableMachine = dp.AttackerSiegeWeapon as UsableMachine) != null && !usableMachine.IsDisabled;
											});
											tacticalPosition = ((castleKeyPosition != null) ? castleKeyPosition.WaitPosition : null);
										}
										TacticalPosition tacticalPosition2 = tacticalPosition;
										if (tacticalPosition2 != null)
										{
											this._position = tacticalPosition2.Position;
										}
										else
										{
											WorldFrame? worldFrame;
											if (siegeLane2 == null)
											{
												worldFrame = null;
											}
											else
											{
												ICastleKeyPosition castleKeyPosition2 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>(delegate(ICastleKeyPosition dp)
												{
													UsableMachine usableMachine2;
													return (usableMachine2 = dp.AttackerSiegeWeapon as UsableMachine) != null && !usableMachine2.IsDisabled;
												});
												worldFrame = ((castleKeyPosition2 != null) ? new WorldFrame?(castleKeyPosition2.DefenseWaitFrame) : null);
											}
											WorldFrame? worldFrame2 = worldFrame;
											WorldFrame worldFrame4;
											if (worldFrame2 == null)
											{
												WorldFrame? worldFrame3;
												if (siegeLane2 == null)
												{
													worldFrame3 = null;
												}
												else
												{
													ICastleKeyPosition castleKeyPosition3 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>();
													worldFrame3 = ((castleKeyPosition3 != null) ? new WorldFrame?(castleKeyPosition3.DefenseWaitFrame) : null);
												}
												worldFrame4 = worldFrame3 ?? WorldFrame.Invalid;
											}
											else
											{
												worldFrame4 = worldFrame2.GetValueOrDefault();
											}
											WorldFrame worldFrame5 = worldFrame4;
											this._position = (worldFrame5.Origin.IsValid ? worldFrame5.Origin : formation.CachedMedianPosition);
										}
									}
									flag = true;
								}
							}
							else if (!teamAISiegeComponent.CalculateIsAnyLaneOpenToGetInside())
							{
								SiegeLadder siegeLadder = null;
								float num = float.MaxValue;
								foreach (SiegeLadder siegeLadder2 in teamAISiegeComponent.Ladders)
								{
									if (!siegeLadder2.IsDeactivated && !siegeLadder2.IsDisabled)
									{
										float num2 = siegeLadder2.WaitFrame.origin.DistanceSquared(formation.CachedMedianPosition.GetNavMeshVec3());
										if (num2 < num)
										{
											num = num2;
											siegeLadder = siegeLadder2;
										}
									}
								}
								if (siegeLadder != null)
								{
									if (!siegeLadder.IsUsedByFormation(formation))
									{
										formation.StartUsingMachine(siegeLadder, true);
										this._position = siegeLadder.WaitFrame.origin.ToWorldPosition();
									}
									else if (!this._position.IsValid)
									{
										this._position = siegeLadder.WaitFrame.origin.ToWorldPosition();
									}
									flag2 = true;
								}
								else
								{
									CastleGate castleGate = ((!teamAISiegeComponent.OuterGate.IsGateOpen) ? teamAISiegeComponent.OuterGate : teamAISiegeComponent.InnerGate);
									if (castleGate != null)
									{
										flag3 = true;
										if (formation.AttackEntityOrderSecondaryDetachment == null)
										{
											GameEntity gameEntity = GameEntity.CreateFromWeakEntity(castleGate.GameEntity);
											formation.FormAttackEntityDetachment(gameEntity);
											this.TargetEntity = gameEntity;
											this._position = this.ComputeAttackEntityWaitPosition(formation, castleGate.GameEntity);
										}
										else if (this.TargetEntity != castleGate.GameEntity)
										{
											GameEntity gameEntity2 = GameEntity.CreateFromWeakEntity(castleGate.GameEntity);
											formation.DisbandAttackEntityDetachment();
											formation.FormAttackEntityDetachment(gameEntity2);
											this.TargetEntity = gameEntity2;
											this._position = this.ComputeAttackEntityWaitPosition(formation, castleGate.GameEntity);
										}
										formation.AttackEntityOrderSecondaryDetachment.TickOccasionally(formation);
									}
								}
							}
						}
					}
					if (teamAISiegeComponent != null && flag4 && this._position.IsValid && !flag)
					{
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (teamAISiegeComponent != null && !flag4 && this._position.IsValid && !flag2 && !flag3)
					{
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (teamAISiegeComponent != null && formation.AttackEntityOrderSecondaryDetachment != null && !flag3)
					{
						formation.DisbandAttackEntityDetachment();
						this.TargetEntity = null;
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (this._position.IsValid)
					{
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Move, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
				}
				return;
			}
			formation.AttackEntityOrderSecondaryDetachment.TickOccasionally(formation);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00039290 File Offset: 0x00037490
		private void TickAux()
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				float length = this._targetAgent.GetCurrentVelocity().Length;
				if (length < 0.01f)
				{
					this._followState = MovementOrder.FollowState.Stop;
					return;
				}
				if (length < this._targetAgent.Monster.WalkingSpeedLimit * 0.7f)
				{
					if (this._followState == MovementOrder.FollowState.Stop)
					{
						this._followState = MovementOrder.FollowState.Depart;
						this._departStartTime = Mission.Current.CurrentTime;
						return;
					}
					if (this._followState == MovementOrder.FollowState.Move)
					{
						this._followState = MovementOrder.FollowState.Arrive;
						return;
					}
				}
				else if (this._followState == MovementOrder.FollowState.Depart)
				{
					if (Mission.Current.CurrentTime - this._departStartTime > 1f)
					{
						this._followState = MovementOrder.FollowState.Move;
						return;
					}
				}
				else
				{
					this._followState = MovementOrder.FollowState.Move;
				}
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0003934C File Offset: 0x0003754C
		public void OnArrangementChanged(Formation formation)
		{
			if (!this.IsApplicable(formation))
			{
				return;
			}
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				formation.Arrangement.ReserveMiddleFrontUnitPosition(this._targetAgent);
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00039380 File Offset: 0x00037580
		public void Advance(Formation formation, float distance)
		{
			WorldPosition currentPosition = this.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None);
			Vec2 direction = formation.Direction;
			currentPosition.SetVec2(currentPosition.AsVec2 + direction * distance);
			this._positionLambda = (Formation f) => currentPosition;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000393DC File Offset: 0x000375DC
		public void FallBack(Formation formation, float distance)
		{
			this.Advance(formation, -distance);
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x000393E8 File Offset: 0x000375E8
		private ValueTuple<Agent, float> GetBestAgent(List<Agent> candidateAgents)
		{
			if (candidateAgents.IsEmpty<Agent>())
			{
				return new ValueTuple<Agent, float>(null, float.MaxValue);
			}
			GameEntity targetEntity = this.TargetEntity;
			Vec3 targetEntityPos = targetEntity.GlobalPosition;
			Agent agent = candidateAgents.MinBy<Agent, float>((Agent ca) => ca.Position.DistanceSquared(targetEntityPos));
			return new ValueTuple<Agent, float>(agent, agent.Position.DistanceSquared(targetEntityPos));
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00039450 File Offset: 0x00037650
		private ValueTuple<Agent, float> GetWorstAgent(List<Agent> currentAgents, int requiredAgentCount)
		{
			if (requiredAgentCount <= 0 || currentAgents.Count < requiredAgentCount)
			{
				return new ValueTuple<Agent, float>(null, float.MaxValue);
			}
			GameEntity targetEntity = this.TargetEntity;
			Vec3 targetEntityPos = targetEntity.GlobalPosition;
			Agent agent = currentAgents.MaxBy<Agent, float>((Agent ca) => ca.Position.DistanceSquared(targetEntityPos));
			return new ValueTuple<Agent, float>(agent, agent.Position.DistanceSquared(targetEntityPos));
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000394BC File Offset: 0x000376BC
		public MovementOrder GetSubstituteOrder(Formation formation)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Charge)
			{
				return MovementOrder.MovementOrderStop;
			}
			return MovementOrder.MovementOrderCharge;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000394E0 File Offset: 0x000376E0
		private Vec2 GetDirectionAux(Formation f)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum - MovementOrder.MovementOrderEnum.Advance > 1)
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetDirectionAux", 1742);
				return Vec2.One;
			}
			Formation targetFormation = f.TargetFormation;
			FormationQuerySystem formationQuerySystem = ((targetFormation != null) ? targetFormation.QuerySystem : null) ?? f.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
			if (formationQuerySystem != null)
			{
				return (formationQuerySystem.Formation.CachedMedianPosition.AsVec2 - f.CachedAveragePosition).Normalized();
			}
			return Vec2.One;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0003956C File Offset: 0x0003776C
		private WorldPosition GetPositionAux(Formation f, WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.Advance)
			{
				if (orderEnum != MovementOrder.MovementOrderEnum.FallBack)
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetPositionAux", 1852);
					return WorldPosition.Invalid;
				}
				if (Mission.Current.Mode == MissionMode.Deployment)
				{
					return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				Vec2 directionAux = this.GetDirectionAux(f);
				WorldPosition cachedMedianPosition = f.CachedMedianPosition;
				cachedMedianPosition.SetVec2(f.CachedAveragePosition - directionAux * 7f);
				return cachedMedianPosition;
			}
			else
			{
				if (Mission.Current.Mode == MissionMode.Deployment)
				{
					return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				Vec2 vec = f.Direction;
				FormationQuerySystem querySystem = f.QuerySystem;
				Formation targetFormation = f.TargetFormation;
				FormationQuerySystem formationQuerySystem = ((targetFormation != null) ? targetFormation.QuerySystem : null) ?? f.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
				WorldPosition worldPosition;
				if (formationQuerySystem == null)
				{
					Agent closestEnemyAgent = querySystem.ClosestEnemyAgent;
					if (closestEnemyAgent == null)
					{
						return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
					}
					worldPosition = closestEnemyAgent.GetWorldPosition();
				}
				else
				{
					worldPosition = formationQuerySystem.Formation.CachedMedianPosition;
				}
				if (querySystem.IsRangedFormation || querySystem.IsRangedCavalryFormation)
				{
					vec = this.GetDirectionAux(f);
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * querySystem.MaximumMissileRange);
					if (vec.DotProduct(worldPosition.AsVec2 - f.CurrentPosition) < 0f)
					{
						float num = 4f / querySystem.MovementSpeedMaximum;
						FormationQuerySystem closestSignificantlyLargeEnemyFormation = f.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
						float num2 = MathF.Min(num * ((closestSignificantlyLargeEnemyFormation != null) ? closestSignificantlyLargeEnemyFormation.Formation.CachedMovementSpeed : 1f), 4f);
						worldPosition.SetVec2(f.CurrentPosition - vec * num2);
					}
				}
				else if (formationQuerySystem != null)
				{
					vec = (formationQuerySystem.Formation.CachedAveragePosition - f.CachedAveragePosition).Normalized();
					float num3 = 2f;
					if (formationQuerySystem.FormationPower < f.QuerySystem.FormationPower * 0.2f)
					{
						num3 = 0.1f;
					}
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * num3);
				}
				if (!this._engageTargetPositionCache.IsValid)
				{
					this._engageTargetPositionCache = worldPosition;
				}
				float num4 = f.QuerySystem.MovementSpeedMaximum * f.QuerySystem.MovementSpeedMaximum * 9f * f.Depth;
				if (this._engageTargetPositionCache.AsVec2.DistanceSquared(worldPosition.AsVec2) > f.CurrentPosition.DistanceSquared(this._engageTargetPositionCache.AsVec2) * 0.1f && worldPosition.AsVec2.DistanceSquared(f.CurrentPosition) <= num4)
				{
					this._engageTargetPositionCache = worldPosition;
					this._engageTargetPositionOffset = 0f;
				}
				worldPosition = this._engageTargetPositionCache;
				LineFormation lineFormation;
				if (worldPosition.AsVec2.DistanceSquared(f.CurrentPosition) > num4 && vec.DotProduct(worldPosition.AsVec2 - f.CurrentPosition) > 0f && (lineFormation = f.Arrangement as LineFormation) != null && (double)lineFormation.GetUnavailableUnitPositions().Count<Vec2>() > (double)lineFormation.UnitCount * 0.03)
				{
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * 10f);
					this._engageTargetPositionOffset += 10f;
				}
				this._engageTargetPositionCache = worldPosition;
				return worldPosition;
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000398D8 File Offset: 0x00037AD8
		private void CancelChargeOrder(Formation formation)
		{
			Team team = formation.Team;
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = ((team != null) ? team.TeamAI : null) as TeamAISiegeComponent) != null)
			{
				if (teamAISiegeComponent.InnerGate != null && teamAISiegeComponent.InnerGate.IsUsedByFormation(formation))
				{
					formation.StopUsingMachine(teamAISiegeComponent.InnerGate, true);
				}
				if (teamAISiegeComponent.OuterGate != null && teamAISiegeComponent.OuterGate.IsUsedByFormation(formation))
				{
					formation.StopUsingMachine(teamAISiegeComponent.OuterGate, true);
				}
				foreach (SiegeLadder siegeLadder in teamAISiegeComponent.Ladders)
				{
					if (siegeLadder.IsUsedByFormation(formation))
					{
						formation.StopUsingMachine(siegeLadder, true);
					}
				}
				if (formation.AttackEntityOrderSecondaryDetachment != null)
				{
					formation.DisbandAttackEntityDetachment();
					this.TargetEntity = null;
				}
				this._position = WorldPosition.Invalid;
			}
		}

		// Token: 0x04000464 RID: 1124
		public static readonly MovementOrder MovementOrderNull = new MovementOrder(MovementOrder.MovementOrderEnum.Invalid);

		// Token: 0x04000465 RID: 1125
		public static readonly MovementOrder MovementOrderCharge = new MovementOrder(MovementOrder.MovementOrderEnum.Charge);

		// Token: 0x04000466 RID: 1126
		public static readonly MovementOrder MovementOrderRetreat = new MovementOrder(MovementOrder.MovementOrderEnum.Retreat);

		// Token: 0x04000467 RID: 1127
		public static readonly MovementOrder MovementOrderStop = new MovementOrder(MovementOrder.MovementOrderEnum.Stop);

		// Token: 0x04000468 RID: 1128
		public static readonly MovementOrder MovementOrderAdvance = new MovementOrder(MovementOrder.MovementOrderEnum.Advance);

		// Token: 0x04000469 RID: 1129
		public static readonly MovementOrder MovementOrderFallBack = new MovementOrder(MovementOrder.MovementOrderEnum.FallBack);

		// Token: 0x0400046A RID: 1130
		private MovementOrder.FollowState _followState;

		// Token: 0x0400046B RID: 1131
		private float _departStartTime;

		// Token: 0x0400046C RID: 1132
		public readonly MovementOrder.MovementOrderEnum OrderEnum;

		// Token: 0x0400046D RID: 1133
		private Func<Formation, WorldPosition> _positionLambda;

		// Token: 0x0400046E RID: 1134
		private WorldPosition _position;

		// Token: 0x0400046F RID: 1135
		private WorldPosition _getPositionResultCache;

		// Token: 0x04000470 RID: 1136
		private WorldPosition _engageTargetPositionCache;

		// Token: 0x04000471 RID: 1137
		private float _engageTargetPositionOffset;

		// Token: 0x04000472 RID: 1138
		private bool _getPositionIsNavmeshlessCache;

		// Token: 0x04000473 RID: 1139
		private WorldPosition _getPositionFirstSectionCache;

		// Token: 0x04000475 RID: 1141
		public GameEntity TargetEntity;

		// Token: 0x04000477 RID: 1143
		private readonly Timer _tickTimer;

		// Token: 0x04000478 RID: 1144
		private WorldPosition _lastPosition;

		// Token: 0x04000479 RID: 1145
		public readonly bool _isFacingDirection;

		// Token: 0x02000481 RID: 1153
		public enum MovementOrderEnum
		{
			// Token: 0x04001AF6 RID: 6902
			Invalid,
			// Token: 0x04001AF7 RID: 6903
			AttackEntity,
			// Token: 0x04001AF8 RID: 6904
			Charge,
			// Token: 0x04001AF9 RID: 6905
			ChargeToTarget,
			// Token: 0x04001AFA RID: 6906
			Follow,
			// Token: 0x04001AFB RID: 6907
			FollowEntity,
			// Token: 0x04001AFC RID: 6908
			Move = 7,
			// Token: 0x04001AFD RID: 6909
			Retreat,
			// Token: 0x04001AFE RID: 6910
			Stop,
			// Token: 0x04001AFF RID: 6911
			Advance,
			// Token: 0x04001B00 RID: 6912
			FallBack
		}

		// Token: 0x02000482 RID: 1154
		public enum MovementStateEnum
		{
			// Token: 0x04001B02 RID: 6914
			Charge,
			// Token: 0x04001B03 RID: 6915
			Hold,
			// Token: 0x04001B04 RID: 6916
			Retreat,
			// Token: 0x04001B05 RID: 6917
			StandGround
		}

		// Token: 0x02000483 RID: 1155
		public enum Side
		{
			// Token: 0x04001B07 RID: 6919
			Front,
			// Token: 0x04001B08 RID: 6920
			Rear,
			// Token: 0x04001B09 RID: 6921
			Left,
			// Token: 0x04001B0A RID: 6922
			Right
		}

		// Token: 0x02000484 RID: 1156
		private enum FollowState
		{
			// Token: 0x04001B0C RID: 6924
			Stop,
			// Token: 0x04001B0D RID: 6925
			Depart,
			// Token: 0x04001B0E RID: 6926
			Move,
			// Token: 0x04001B0F RID: 6927
			Arrive
		}
	}
}
