using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200005A RID: 90
	public struct LineSegment2D
	{
		// Token: 0x0600029F RID: 671 RVA: 0x00007DDC File Offset: 0x00005FDC
		public LineSegment2D(Vec2 point1, Vec2 point2)
		{
			this.Point1 = point1;
			this.Point2 = point2;
			this.Normal = (point1 - point2).Normalized().RightVec();
		}

		// Token: 0x17000039 RID: 57
		public Vec2 this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.Point1;
				}
				if (index != 1)
				{
					Debug.FailedAssert("Invalid index", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\LineSegment2D.cs", "Item", 30);
					return Vec2.Invalid;
				}
				return this.Point2;
			}
		}

		// Token: 0x040000FC RID: 252
		public Vec2 Point1;

		// Token: 0x040000FD RID: 253
		public Vec2 Point2;

		// Token: 0x040000FE RID: 254
		public Vec2 Normal;
	}
}
