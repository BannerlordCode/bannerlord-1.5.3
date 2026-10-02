using System;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015C RID: 348
	public abstract class OrderComponent
	{
		// Token: 0x06001259 RID: 4697 RVA: 0x00039A10 File Offset: 0x00037C10
		public Vec2 GetDirection(Formation f)
		{
			Vec2 vec = this.Direction(f);
			if (f.IsAIControlled && vec.DotProduct(this._previousDirection) > 0.87f)
			{
				vec = this._previousDirection;
			}
			else
			{
				this._previousDirection = vec;
			}
			return vec;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00039A57 File Offset: 0x00037C57
		protected void CopyPositionAndDirectionFrom(OrderComponent order)
		{
			this.Position = order.Position;
			this.Direction = order.Direction;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00039A71 File Offset: 0x00037C71
		protected OrderComponent(float tickTimerDuration = 0.5f)
		{
			this._tickTimer = new Timer(Mission.Current.CurrentTime, tickTimerDuration, true);
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x0600125C RID: 4700
		public abstract OrderType OrderType { get; }

		// Token: 0x0600125D RID: 4701 RVA: 0x00039A9B File Offset: 0x00037C9B
		internal bool Tick(Formation formation)
		{
			bool flag = this._tickTimer.Check(Mission.Current.CurrentTime);
			if (flag)
			{
				this.TickOccasionally(formation, this._tickTimer.PreviousDeltaTime);
			}
			return flag;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00039AC7 File Offset: 0x00037CC7
		[Conditional("DEBUG")]
		protected virtual void TickDebug(Formation formation)
		{
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00039AC9 File Offset: 0x00037CC9
		protected internal virtual void TickOccasionally(Formation formation, float dt)
		{
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00039ACB File Offset: 0x00037CCB
		protected internal virtual void OnApply(Formation formation)
		{
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00039ACD File Offset: 0x00037CCD
		protected internal virtual void OnCancel(Formation formation)
		{
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00039ACF File Offset: 0x00037CCF
		protected internal virtual void OnUnitJoinOrLeave(Agent unit, bool isJoining)
		{
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00039AD1 File Offset: 0x00037CD1
		protected internal virtual bool IsApplicable(Formation formation)
		{
			return true;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x00039AD4 File Offset: 0x00037CD4
		protected internal virtual bool CanStack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00039AD7 File Offset: 0x00037CD7
		protected internal virtual bool CancelsPreviousDirectionOrder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x00039ADA File Offset: 0x00037CDA
		protected internal virtual bool CancelsPreviousArrangementOrder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00039ADD File Offset: 0x00037CDD
		protected internal virtual MovementOrder GetSubstituteOrder(Formation formation)
		{
			return MovementOrder.MovementOrderCharge;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00039AE4 File Offset: 0x00037CE4
		protected internal virtual void OnArrangementChanged(Formation formation)
		{
		}

		// Token: 0x0400047A RID: 1146
		private readonly Timer _tickTimer;

		// Token: 0x0400047B RID: 1147
		protected Func<Formation, Vec3> Position;

		// Token: 0x0400047C RID: 1148
		protected Func<Formation, Vec2> Direction;

		// Token: 0x0400047D RID: 1149
		private Vec2 _previousDirection = Vec2.Invalid;
	}
}
