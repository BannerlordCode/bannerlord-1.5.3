using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000157 RID: 343
	public struct FacingOrder
	{
		// Token: 0x06001200 RID: 4608 RVA: 0x000371B5 File Offset: 0x000353B5
		public static FacingOrder FacingOrderLookAtDirection(Vec2 direction)
		{
			return new FacingOrder(FacingOrder.FacingOrderEnum.LookAtDirection, direction);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000371BE File Offset: 0x000353BE
		private FacingOrder(FacingOrder.FacingOrderEnum orderEnum, Vec2 direction)
		{
			this.OrderEnum = orderEnum;
			this._lookAtDirection = direction;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000371CE File Offset: 0x000353CE
		private FacingOrder(FacingOrder.FacingOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			this._lookAtDirection = Vec2.Invalid;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000371E4 File Offset: 0x000353E4
		private Vec2 GetDirectionAux(Formation f, Agent targetAgent)
		{
			if (f.PhysicalClass.IsMounted() && targetAgent != null && targetAgent.Velocity.LengthSquared > targetAgent.GetMaximumForwardUnlimitedSpeed() * targetAgent.GetMaximumForwardUnlimitedSpeed() * 0.09f)
			{
				return targetAgent.Velocity.AsVec2.Normalized();
			}
			if (this.OrderEnum == FacingOrder.FacingOrderEnum.LookAtDirection)
			{
				return this._lookAtDirection;
			}
			if (f.Arrangement is CircularFormation || f.Arrangement is SquareFormation)
			{
				return f.Direction;
			}
			Vec2 currentPosition = f.CurrentPosition;
			Vec2 weightedAverageEnemyPosition = f.QuerySystem.WeightedAverageEnemyPosition;
			if (!weightedAverageEnemyPosition.IsValid)
			{
				return f.Direction;
			}
			Vec2 vec = (weightedAverageEnemyPosition - currentPosition).Normalized();
			float length = (weightedAverageEnemyPosition - currentPosition).Length;
			int enemyUnitCount = f.QuerySystem.Team.EnemyUnitCount;
			int countOfUnits = f.CountOfUnits;
			Vec2 vec2 = f.Direction;
			bool flag = length >= (float)countOfUnits * 0.2f;
			if (enemyUnitCount == 0 || countOfUnits == 0)
			{
				flag = false;
			}
			float num = ((!flag) ? 1f : (MBMath.ClampFloat((float)countOfUnits * 1f / (float)enemyUnitCount, 0.33333334f, 3f) * MBMath.ClampFloat(length / (float)countOfUnits, 0.33333334f, 3f)));
			if (flag && MathF.Abs(vec.AngleBetween(vec2)) > 0.17453292f * num)
			{
				vec2 = vec;
			}
			return vec2;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x00037350 File Offset: 0x00035550
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum != FacingOrder.FacingOrderEnum.LookAtDirection)
				{
					return OrderType.LookAtEnemy;
				}
				return OrderType.LookAtDirection;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0003735F File Offset: 0x0003555F
		public Vec2 GetDirection(Formation f, Agent targetAgent = null)
		{
			return this.GetDirectionAux(f, targetAgent);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0003736C File Offset: 0x0003556C
		public override bool Equals(object obj)
		{
			if (obj is FacingOrder)
			{
				FacingOrder facingOrder = (FacingOrder)obj;
				return facingOrder == this;
			}
			return false;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00037398 File Offset: 0x00035598
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000373A0 File Offset: 0x000355A0
		public static bool operator !=(FacingOrder f1, FacingOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000373B3 File Offset: 0x000355B3
		public static bool operator ==(FacingOrder f1, FacingOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x04000459 RID: 1113
		public readonly FacingOrder.FacingOrderEnum OrderEnum;

		// Token: 0x0400045A RID: 1114
		private readonly Vec2 _lookAtDirection;

		// Token: 0x0400045B RID: 1115
		public static readonly FacingOrder FacingOrderLookAtEnemy = new FacingOrder(FacingOrder.FacingOrderEnum.LookAtEnemy);

		// Token: 0x0200047E RID: 1150
		public enum FacingOrderEnum
		{
			// Token: 0x04001AEB RID: 6891
			LookAtDirection,
			// Token: 0x04001AEC RID: 6892
			LookAtEnemy
		}
	}
}
