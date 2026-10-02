using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000156 RID: 342
	public struct ArrangementOrder
	{
		// Token: 0x060011E4 RID: 4580 RVA: 0x000367FA File Offset: 0x000349FA
		public static int GetUnitSpacingOf(ArrangementOrder.ArrangementOrderEnum a)
		{
			switch (a)
			{
			case ArrangementOrder.ArrangementOrderEnum.Loose:
				return 6;
			case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
			case ArrangementOrder.ArrangementOrderEnum.Square:
				return 0;
			}
			return 2;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0003681F File Offset: 0x00034A1F
		public static bool GetUnitLooseness(ArrangementOrder.ArrangementOrderEnum a)
		{
			return a != ArrangementOrder.ArrangementOrderEnum.ShieldWall;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00036828 File Offset: 0x00034A28
		public ArrangementOrder(ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			this._walkRestriction = null;
			switch (this.OrderEnum)
			{
			case ArrangementOrder.ArrangementOrderEnum.Circle:
				this._runRestriction = new float?(0.5f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.Line:
				this._runRestriction = new float?(0.8f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.Loose:
			case ArrangementOrder.ArrangementOrderEnum.Scatter:
			case ArrangementOrder.ArrangementOrderEnum.Skein:
				this._runRestriction = new float?(0.9f);
				goto IL_009A;
			case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
			case ArrangementOrder.ArrangementOrderEnum.Square:
				this._runRestriction = new float?(0.3f);
				goto IL_009A;
			}
			this._runRestriction = new float?(1f);
			IL_009A:
			this._unitSpacing = ArrangementOrder.GetUnitSpacingOf(this.OrderEnum);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x000368E0 File Offset: 0x00034AE0
		public void GetMovementSpeedRestriction(out float? runRestriction, out float? walkRestriction)
		{
			runRestriction = this._runRestriction;
			walkRestriction = this._walkRestriction;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x000368FC File Offset: 0x00034AFC
		public IFormationArrangement GetArrangement(Formation formation)
		{
			ArrangementOrder.ArrangementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum <= ArrangementOrder.ArrangementOrderEnum.Column)
			{
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle)
				{
					return new CircularFormation(formation);
				}
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
				{
					return new ColumnFormation(formation, null, 1);
				}
			}
			else
			{
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Skein)
				{
					return new SkeinFormation(formation);
				}
				if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return new RectilinearSchiltronFormation(formation);
				}
			}
			return new LineFormation(formation, true);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0003694C File Offset: 0x00034B4C
		public unsafe void OnApply(Formation formation)
		{
			formation.SetPositioning(null, null, new int?(this.GetUnitSpacing()));
			this.Rearrange(formation);
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				this.TickOccasionally(formation);
				formation.ResetArrangementOrderTickTimer();
			}
			ArrangementOrder.ArrangementOrderEnum orderEnum = this.OrderEnum;
			formation.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				if (agent.IsAIControlled)
				{
					Agent.UsageDirection shieldDirectionOfUnit = ArrangementOrder.GetShieldDirectionOfUnit(formation, agent, orderEnum);
					agent.EnforceShieldUsage(shieldDirectionOfUnit);
				}
				agent.UpdateAgentProperties();
				MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
				MovementOrder.MovementOrderEnum movementOrderEnum = movementOrder.OrderEnum;
				if ((movementOrderEnum == MovementOrder.MovementOrderEnum.Charge || movementOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && movementOrder.GetPosition(formation).IsValid)
				{
					movementOrderEnum = MovementOrder.MovementOrderEnum.Move;
				}
				agent.RefreshBehaviorValues(movementOrderEnum, orderEnum);
			}, null);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000369DE File Offset: 0x00034BDE
		public void SoftUpdate(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				this.TickOccasionally(formation);
				formation.ResetArrangementOrderTickTimer();
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000369F8 File Offset: 0x00034BF8
		public static Agent.UsageDirection GetShieldDirectionOfUnit(Formation formation, Agent unit, ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			Agent.UsageDirection usageDirection;
			if (unit.IsDetachedFromFormation)
			{
				usageDirection = Agent.UsageDirection.None;
			}
			else if (orderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
			{
				if (((IFormationUnit)unit).FormationRankIndex == 0)
				{
					usageDirection = Agent.UsageDirection.DefendDown;
				}
				else if (formation.Arrangement.GetNeighborUnitOfLeftSide(unit) == null)
				{
					usageDirection = Agent.UsageDirection.DefendLeft;
				}
				else if (formation.Arrangement.GetNeighborUnitOfRightSide(unit) == null)
				{
					usageDirection = Agent.UsageDirection.DefendRight;
				}
				else
				{
					usageDirection = Agent.UsageDirection.AttackEnd;
				}
			}
			else if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				if (((IFormationUnit)unit).IsShieldUsageEncouraged)
				{
					if (((IFormationUnit)unit).FormationRankIndex == 0)
					{
						usageDirection = Agent.UsageDirection.DefendDown;
					}
					else
					{
						usageDirection = Agent.UsageDirection.AttackEnd;
					}
				}
				else
				{
					usageDirection = Agent.UsageDirection.None;
				}
			}
			else
			{
				usageDirection = Agent.UsageDirection.None;
			}
			return usageDirection;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00036A73 File Offset: 0x00034C73
		public int GetUnitSpacing()
		{
			return this._unitSpacing;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00036A7B File Offset: 0x00034C7B
		public void Rearrange(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				this.RearrangeAux(formation, false);
				return;
			}
			formation.Rearrange(this.GetArrangement(formation));
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00036A9C File Offset: 0x00034C9C
		public void RearrangeAux(Formation formation, bool isDirectly)
		{
			if (!isDirectly)
			{
				ArrangementOrder.TransposeLineFormation(formation);
				formation.OnTick += formation.TickForColumnArrangementInitialPositioning;
				return;
			}
			formation.OnTick -= formation.TickForColumnArrangementInitialPositioning;
			formation.ReferencePosition = null;
			formation.Rearrange(this.GetArrangement(formation));
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00036AF0 File Offset: 0x00034CF0
		public unsafe static void TransposeLineFormation(Formation formation)
		{
			formation.Rearrange(new TransposedLineFormation(formation));
			MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
			formation.SetPositioning(new WorldPosition?(movementOrder.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None)), null, null);
			formation.ReferencePosition = new Vec2?(formation.OrderPosition);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00036B4C File Offset: 0x00034D4C
		public void OnCancel(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				Team team = formation.Team;
				if (((team != null) ? team.TeamAI : null) != null)
				{
					MBReadOnlyList<StrategicArea> strategicAreas = formation.Team.TeamAI.StrategicAreas;
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						IDetachment detachment = formation.Detachments[i];
						foreach (StrategicArea strategicArea in strategicAreas)
						{
							if (detachment == strategicArea)
							{
								formation.LeaveDetachment(detachment);
								break;
							}
						}
					}
				}
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					if (agent.IsAIControlled)
					{
						agent.EnforceShieldUsage(Agent.UsageDirection.None);
					}
				}, null);
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				formation.OnTick -= formation.TickForColumnArrangementInitialPositioning;
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00036C54 File Offset: 0x00034E54
		private static StrategicArea CreateStrategicArea(Scene scene, WorldPosition position, Vec2 direction, float width, int capacity, BattleSideEnum side)
		{
			WorldFrame worldFrame = new WorldFrame(new Mat3
			{
				f = direction.ToVec3(0f),
				u = Vec3.Up
			}, position);
			GameEntity gameEntity = GameEntity.Instantiate(scene, "strategic_area_autogen", worldFrame.ToNavMeshMatrixFrame(), true);
			gameEntity.SetMobility(GameEntity.Mobility.Dynamic);
			StrategicArea firstScriptOfType = gameEntity.GetFirstScriptOfType<StrategicArea>();
			firstScriptOfType.InitializeAutogenerated(width, capacity, side);
			return firstScriptOfType;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00036CBC File Offset: 0x00034EBC
		private static IEnumerable<StrategicArea> CreateStrategicAreas(Mission mission, int count, WorldPosition center, float distance, WorldPosition target, float width, int capacity, BattleSideEnum side)
		{
			Scene scene = mission.Scene;
			float distanceMultiplied = distance * 0.7f;
			Func<WorldPosition> func = delegate
			{
				WorldPosition center2 = center;
				float num2 = MBRandom.RandomFloat * 3.1415927f * 2f;
				center2.SetVec2(center.AsVec2 + Vec2.FromRotation(num2) * distanceMultiplied);
				return center2;
			};
			WorldPosition[] array = delegate
			{
				float num3 = MBRandom.RandomFloat * 3.1415927f * 2f;
				switch (count)
				{
				case 2:
				{
					WorldPosition center3 = center;
					center3.SetVec2(center.AsVec2 + Vec2.FromRotation(num3) * distanceMultiplied);
					WorldPosition center4 = center;
					center4.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 3.1415927f) * distanceMultiplied);
					return new WorldPosition[] { center3, center4 };
				}
				case 3:
				{
					WorldPosition center5 = center;
					center5.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 0f) * distanceMultiplied);
					WorldPosition center6 = center;
					center6.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 2.0943952f) * distanceMultiplied);
					WorldPosition center7 = center;
					center7.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 4.1887903f) * distanceMultiplied);
					return new WorldPosition[] { center5, center6, center7 };
				}
				case 4:
				{
					WorldPosition center8 = center;
					center8.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 0f) * distanceMultiplied);
					WorldPosition center9 = center;
					center9.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 1.5707964f) * distanceMultiplied);
					WorldPosition center10 = center;
					center10.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 3.1415927f) * distanceMultiplied);
					WorldPosition center11 = center;
					center11.SetVec2(center.AsVec2 + Vec2.FromRotation(num3 + 4.712389f) * distanceMultiplied);
					return new WorldPosition[] { center8, center9, center10, center11 };
				}
				default:
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\ArrangementOrder.cs", "CreateStrategicAreas", 369);
					return new WorldPosition[0];
				}
			}();
			List<WorldPosition> positions = new List<WorldPosition>();
			WorldPosition[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				WorldPosition worldPosition = array2[i];
				WorldPosition worldPosition2 = worldPosition;
				WorldPosition position = mission.FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref worldPosition2, distance * 0.25f, ref target);
				Func<WorldPosition, bool> func2 = delegate(WorldPosition p)
				{
					float num4;
					if (!positions.Any<WorldPosition>((WorldPosition wp) => wp.AsVec2.DistanceSquared(p.AsVec2) < 1f) && (scene.GetPathDistanceBetweenPositions(ref center, ref p, 0f, out num4) && num4 < center.AsVec2.Distance(p.AsVec2) * 2f))
					{
						positions.Add(position);
						return true;
					}
					return false;
				};
				if (!func2(position) && !func2(worldPosition))
				{
					int num = 0;
					while (num++ < 10 && !func2(func()))
					{
					}
					if (num >= 10)
					{
						positions.Add(center);
					}
				}
			}
			Vec2 direction = (target.AsVec2 - center.AsVec2).Normalized();
			foreach (WorldPosition worldPosition3 in positions)
			{
				yield return ArrangementOrder.CreateStrategicArea(scene, worldPosition3, direction, width, capacity, side);
			}
			List<WorldPosition>.Enumerator enumerator = default(List<WorldPosition>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00036D0C File Offset: 0x00034F0C
		private bool IsStrategicAreaClose(StrategicArea strategicArea, Formation formation)
		{
			if (formation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Charge || formation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget || !strategicArea.IsUsableBy(formation.Team.Side))
			{
				return false;
			}
			if (strategicArea.IgnoreHeight)
			{
				return MathF.Abs(strategicArea.GameEntity.GlobalPosition.x - formation.OrderPosition.X) <= strategicArea.DistanceToCheck && MathF.Abs(strategicArea.GameEntity.GlobalPosition.y - formation.OrderPosition.Y) <= strategicArea.DistanceToCheck;
			}
			WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
			Vec3 globalPosition = strategicArea.GameEntity.GlobalPosition;
			return worldPosition.DistanceSquaredWithLimit(in globalPosition, strategicArea.DistanceToCheck * strategicArea.DistanceToCheck + 1E-05f) < strategicArea.DistanceToCheck * strategicArea.DistanceToCheck;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00036E04 File Offset: 0x00035004
		public void TickOccasionally(Formation formation)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Scatter)
			{
				Team team = formation.Team;
				if (((team != null) ? team.TeamAI : null) != null)
				{
					MBReadOnlyList<StrategicArea> strategicAreas = formation.Team.TeamAI.StrategicAreas;
					foreach (StrategicArea strategicArea in strategicAreas)
					{
						if (this.IsStrategicAreaClose(strategicArea, formation))
						{
							bool flag = false;
							foreach (IDetachment detachment in formation.Detachments)
							{
								if (strategicArea == detachment)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								formation.JoinDetachment(strategicArea);
							}
						}
					}
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						IDetachment detachment2 = formation.Detachments[i];
						foreach (StrategicArea strategicArea2 in strategicAreas)
						{
							if (detachment2 == strategicArea2 && !this.IsStrategicAreaClose(strategicArea2, formation))
							{
								formation.LeaveDetachment(detachment2);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00036F58 File Offset: 0x00035158
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case ArrangementOrder.ArrangementOrderEnum.Circle:
					return OrderType.ArrangementCircular;
				case ArrangementOrder.ArrangementOrderEnum.Column:
					return OrderType.ArrangementColumn;
				case ArrangementOrder.ArrangementOrderEnum.Line:
					return OrderType.ArrangementLine;
				case ArrangementOrder.ArrangementOrderEnum.Loose:
					return OrderType.ArrangementLoose;
				case ArrangementOrder.ArrangementOrderEnum.Scatter:
					return OrderType.ArrangementScatter;
				case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
					return OrderType.ArrangementCloseOrder;
				case ArrangementOrder.ArrangementOrderEnum.Skein:
					return OrderType.ArrangementVee;
				case ArrangementOrder.ArrangementOrderEnum.Square:
					return OrderType.ArrangementSchiltron;
				default:
					return OrderType.ArrangementLine;
				}
			}
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00036FAE File Offset: 0x000351AE
		public ArrangementOrder.ArrangementOrderEnum GetNativeEnum()
		{
			return this.OrderEnum;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00036FB6 File Offset: 0x000351B6
		public override bool Equals(object obj)
		{
			return obj is ArrangementOrder && (ArrangementOrder)obj == this;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00036FD3 File Offset: 0x000351D3
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00036FDB File Offset: 0x000351DB
		public static bool operator !=(ArrangementOrder a1, ArrangementOrder a2)
		{
			return a1.OrderEnum != a2.OrderEnum;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00036FEE File Offset: 0x000351EE
		public static bool operator ==(ArrangementOrder a1, ArrangementOrder a2)
		{
			return a1.OrderEnum == a2.OrderEnum;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00037000 File Offset: 0x00035200
		public void OnOrderPositionChanged(Formation formation, Vec2 previousOrderPosition)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column && formation.Arrangement is TransposedLineFormation)
			{
				Vec2 direction = formation.Direction;
				Vec2 vec = (formation.OrderPosition - previousOrderPosition).Normalized();
				float num = direction.AngleBetween(vec);
				if ((num > 1.5707964f || num < -1.5707964f) && formation.CachedAveragePosition.DistanceSquared(formation.OrderPosition) < formation.Depth * formation.Depth / 10f)
				{
					formation.ReferencePosition = new Vec2?(formation.OrderPosition);
				}
			}
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00037092 File Offset: 0x00035292
		public static int GetArrangementOrderDefensiveness(ArrangementOrder.ArrangementOrderEnum orderEnum)
		{
			if (orderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || orderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || orderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x000370A2 File Offset: 0x000352A2
		public static int GetArrangementOrderDefensivenessChange(ArrangementOrder.ArrangementOrderEnum previousOrderEnum, ArrangementOrder.ArrangementOrderEnum nextOrderEnum)
		{
			if (previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || previousOrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				if (nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.Circle && nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.ShieldWall && nextOrderEnum != ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || nextOrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000370CC File Offset: 0x000352CC
		public float CalculateFormationDirectionEnforcingFactorForRank(int formationRankIndex, int rankCount)
		{
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Circle || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall || this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Square)
			{
				return 1f - MBMath.ClampFloat(((float)formationRankIndex + 1f) / ((float)rankCount * 2f), 0f, 1f);
			}
			if (this.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				return 0f;
			}
			return 1f - MBMath.ClampFloat(((float)formationRankIndex + 1f) / ((float)rankCount * 0.5f), 0f, 1f);
		}

		// Token: 0x0400044D RID: 1101
		private float? _walkRestriction;

		// Token: 0x0400044E RID: 1102
		private float? _runRestriction;

		// Token: 0x0400044F RID: 1103
		private int _unitSpacing;

		// Token: 0x04000450 RID: 1104
		public readonly ArrangementOrder.ArrangementOrderEnum OrderEnum;

		// Token: 0x04000451 RID: 1105
		public static readonly ArrangementOrder ArrangementOrderCircle = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Circle);

		// Token: 0x04000452 RID: 1106
		public static readonly ArrangementOrder ArrangementOrderColumn = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Column);

		// Token: 0x04000453 RID: 1107
		public static readonly ArrangementOrder ArrangementOrderLine = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Line);

		// Token: 0x04000454 RID: 1108
		public static readonly ArrangementOrder ArrangementOrderLoose = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Loose);

		// Token: 0x04000455 RID: 1109
		public static readonly ArrangementOrder ArrangementOrderScatter = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Scatter);

		// Token: 0x04000456 RID: 1110
		public static readonly ArrangementOrder ArrangementOrderShieldWall = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.ShieldWall);

		// Token: 0x04000457 RID: 1111
		public static readonly ArrangementOrder ArrangementOrderSkein = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Skein);

		// Token: 0x04000458 RID: 1112
		public static readonly ArrangementOrder ArrangementOrderSquare = new ArrangementOrder(ArrangementOrder.ArrangementOrderEnum.Square);

		// Token: 0x02000477 RID: 1143
		public enum ArrangementOrderEnum
		{
			// Token: 0x04001AC0 RID: 6848
			Circle,
			// Token: 0x04001AC1 RID: 6849
			Column,
			// Token: 0x04001AC2 RID: 6850
			Line,
			// Token: 0x04001AC3 RID: 6851
			Loose,
			// Token: 0x04001AC4 RID: 6852
			Scatter,
			// Token: 0x04001AC5 RID: 6853
			ShieldWall,
			// Token: 0x04001AC6 RID: 6854
			Skein,
			// Token: 0x04001AC7 RID: 6855
			Square
		}
	}
}
