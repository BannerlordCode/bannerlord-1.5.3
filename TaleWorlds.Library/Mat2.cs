using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000063 RID: 99
	public struct Mat2
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x0000884D File Offset: 0x00006A4D
		public Mat2(float sx, float sy, float fx, float fy)
		{
			this.s.x = sx;
			this.s.y = sy;
			this.f.x = fx;
			this.f.y = fy;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00008880 File Offset: 0x00006A80
		public void RotateCounterClockWise(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			Vec2 vec = this.s * num2 + this.f * num;
			Vec2 vec2 = this.f * num2 - this.s * num;
			this.s = vec;
			this.f = vec2;
		}

		// Token: 0x04000125 RID: 293
		public Vec2 s;

		// Token: 0x04000126 RID: 294
		public Vec2 f;

		// Token: 0x04000127 RID: 295
		public static readonly Mat2 Identity = new Mat2(1f, 0f, 0f, 1f);
	}
}
