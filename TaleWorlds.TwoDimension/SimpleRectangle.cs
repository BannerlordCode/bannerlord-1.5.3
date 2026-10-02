using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002D RID: 45
	public struct SimpleRectangle
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001FA RID: 506 RVA: 0x000082B7 File Offset: 0x000064B7
		public float Width
		{
			get
			{
				return this.X2 - this.X;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000082C6 File Offset: 0x000064C6
		public float Height
		{
			get
			{
				return this.Y2 - this.Y;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000082D5 File Offset: 0x000064D5
		public SimpleRectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.X2 = x + width;
			this.Y2 = y + height;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000082F8 File Offset: 0x000064F8
		public bool IsCollide(SimpleRectangle other)
		{
			return other.X <= this.X2 && other.X2 >= this.X && other.Y <= this.Y2 && other.Y2 >= this.Y;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00008337 File Offset: 0x00006537
		public Vector2 GetCenter()
		{
			return new Vector2((this.X + this.X2) * 0.5f, (this.Y + this.Y2) * 0.5f);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008364 File Offset: 0x00006564
		public bool IsSubRectOf(SimpleRectangle other)
		{
			return other.X <= this.X && other.X2 >= this.X2 && other.Y <= this.Y && other.Y2 >= this.Y2;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000083A3 File Offset: 0x000065A3
		public bool IsValid()
		{
			return this.Width > 0f && this.Height > 0f;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000083C1 File Offset: 0x000065C1
		public bool IsPointInside(Vector2 point)
		{
			return point.X >= this.X && point.Y >= this.Y && point.X <= this.X2 && point.Y <= this.Y2;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008400 File Offset: 0x00006600
		public void ReduceToIntersection(SimpleRectangle other)
		{
			this.X = Mathf.Max(this.X, other.X);
			this.Y = Mathf.Max(this.Y, other.Y);
			this.X2 = Mathf.Min(this.Y2, other.Y2);
			this.Y2 = Mathf.Min(this.Y2, other.Y2);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000846C File Offset: 0x0000666C
		public static SimpleRectangle Lerp(SimpleRectangle from, SimpleRectangle to, float ratio)
		{
			return new SimpleRectangle(Mathf.Lerp(from.X, to.X, ratio), Mathf.Lerp(from.Y, to.Y, ratio), Mathf.Lerp(from.Width, to.Width, ratio), Mathf.Lerp(from.Height, to.Height, ratio));
		}

		// Token: 0x04000101 RID: 257
		public float X;

		// Token: 0x04000102 RID: 258
		public float Y;

		// Token: 0x04000103 RID: 259
		public float X2;

		// Token: 0x04000104 RID: 260
		public float Y2;
	}
}
