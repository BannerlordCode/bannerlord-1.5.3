using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000099 RID: 153
	public struct LinearFrictionTerm
	{
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001D15D File Offset: 0x0001B35D
		public static LinearFrictionTerm Invalid
		{
			get
			{
				return new LinearFrictionTerm(0f, 0f, 0f, 0f, 0f, 0f);
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0001D182 File Offset: 0x0001B382
		public static LinearFrictionTerm One
		{
			get
			{
				return new LinearFrictionTerm(1f, 1f, 1f, 1f, 1f, 1f);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		public bool IsValid
		{
			get
			{
				return this.Right > 0f && this.Left > 0f && this.Forward > 0f && this.Backward > 0f && this.Up > 0f && this.Down > 0f;
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001D205 File Offset: 0x0001B405
		public LinearFrictionTerm(float right, float left, float forward, float backward, float up, float down)
		{
			this.Right = right;
			this.Left = left;
			this.Forward = forward;
			this.Backward = backward;
			this.Up = up;
			this.Down = down;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001D234 File Offset: 0x0001B434
		public static LinearFrictionTerm operator /(LinearFrictionTerm o, float f)
		{
			return o * (1f / f);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001D243 File Offset: 0x0001B443
		public static LinearFrictionTerm operator *(LinearFrictionTerm o, float f)
		{
			return new LinearFrictionTerm(o.Right * f, o.Left * f, o.Forward * f, o.Backward * f, o.Up * f, o.Down * f);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001D27C File Offset: 0x0001B47C
		public LinearFrictionTerm ElementWiseProduct(LinearFrictionTerm o)
		{
			return new LinearFrictionTerm(this.Right * o.Right, this.Left * o.Left, this.Forward * o.Forward, this.Backward * o.Backward, this.Up * o.Up, this.Down * o.Down);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001D2DC File Offset: 0x0001B4DC
		public bool NearlyEquals(in LinearFrictionTerm o, float epsilon = 1E-05f)
		{
			return this.Right.ApproximatelyEqualsTo(o.Right, epsilon) && this.Left.ApproximatelyEqualsTo(o.Left, epsilon) && this.Forward.ApproximatelyEqualsTo(o.Forward, epsilon) && this.Backward.ApproximatelyEqualsTo(o.Backward, epsilon) && this.Up.ApproximatelyEqualsTo(o.Up, epsilon) && this.Down.ApproximatelyEqualsTo(o.Down, epsilon);
		}

		// Token: 0x040004B4 RID: 1204
		public readonly float Right;

		// Token: 0x040004B5 RID: 1205
		public readonly float Left;

		// Token: 0x040004B6 RID: 1206
		public readonly float Forward;

		// Token: 0x040004B7 RID: 1207
		public readonly float Backward;

		// Token: 0x040004B8 RID: 1208
		public readonly float Up;

		// Token: 0x040004B9 RID: 1209
		public readonly float Down;
	}
}
