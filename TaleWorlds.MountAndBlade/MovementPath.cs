using System;
using System.Diagnostics;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000152 RID: 338
	public class MovementPath
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00032769 File Offset: 0x00030969
		private int LineCount
		{
			get
			{
				return this._navigationData.PointSize - 1;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x00032778 File Offset: 0x00030978
		public Vec2 InitialDirection { get; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00032780 File Offset: 0x00030980
		public Vec2 FinalDirection { get; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00032788 File Offset: 0x00030988
		public Vec3 Destination
		{
			get
			{
				return this._navigationData.EndPoint;
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00032795 File Offset: 0x00030995
		public MovementPath(NavigationData navigationData, Vec2 initialDirection, Vec2 finalDirection)
		{
			this._navigationData = navigationData;
			this.InitialDirection = initialDirection;
			this.FinalDirection = finalDirection;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000327B2 File Offset: 0x000309B2
		public MovementPath(Vec3 currentPosition, Vec3 orderPosition, float agentRadius, Vec2 previousDirection, Vec2 finalDirection)
			: this(new NavigationData(currentPosition, orderPosition, agentRadius), previousDirection, finalDirection)
		{
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000327C8 File Offset: 0x000309C8
		private void UpdateLineLengths()
		{
			if (this._lineLengthAccumulations == null)
			{
				this._lineLengthAccumulations = new float[this.LineCount];
				for (int i = 0; i < this.LineCount; i++)
				{
					this._lineLengthAccumulations[i] = (this._navigationData.Points[i + 1] - this._navigationData.Points[i]).Length;
					if (i > 0)
					{
						this._lineLengthAccumulations[i] += this._lineLengthAccumulations[i - 1];
					}
				}
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00032858 File Offset: 0x00030A58
		private float GetPathProggress(Vec2 point, int lineIndex)
		{
			this.UpdateLineLengths();
			float num = this._lineLengthAccumulations[this.LineCount - 1];
			if (num == 0f)
			{
				return 1f;
			}
			return (((lineIndex > 0) ? this._lineLengthAccumulations[lineIndex - 1] : 0f) + (point - this._navigationData.Points[lineIndex]).Length) / num;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000328C0 File Offset: 0x00030AC0
		private void GetClosestPointTo(Vec2 point, out Vec2 closest, out int lineIndex)
		{
			closest = Vec2.Invalid;
			lineIndex = -1;
			float num = float.MaxValue;
			for (int i = 0; i < this.LineCount; i++)
			{
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in this._navigationData.Points[i], in this._navigationData.Points[i + 1], in point);
				float num2 = closestPointOnLineSegmentToPoint.DistanceSquared(point);
				if (num2 < num)
				{
					num = num2;
					closest = closestPointOnLineSegmentToPoint;
					lineIndex = i;
				}
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00032938 File Offset: 0x00030B38
		[Conditional("DEBUG")]
		public void TickDebug(Vec2 position)
		{
			Vec2 vec;
			int num;
			this.GetClosestPointTo(position, out vec, out num);
			float pathProggress = this.GetPathProggress(vec, num);
			Vec2.Slerp(this.InitialDirection, this.FinalDirection, pathProggress).Normalize();
		}

		// Token: 0x0400040D RID: 1037
		private float[] _lineLengthAccumulations;

		// Token: 0x0400040E RID: 1038
		private NavigationData _navigationData;
	}
}
