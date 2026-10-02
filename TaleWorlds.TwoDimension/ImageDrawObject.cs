using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200001C RID: 28
	public struct ImageDrawObject : IDrawObject
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006E9A File Offset: 0x0000509A
		public static ImageDrawObject Invalid
		{
			get
			{
				return ImageDrawObject.CreateInvalid();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006EA1 File Offset: 0x000050A1
		bool IDrawObject.IsValid
		{
			get
			{
				return this.IsValid;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006EA9 File Offset: 0x000050A9
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00006EB1 File Offset: 0x000050B1
		Rectangle2D IDrawObject.Rectangle
		{
			get
			{
				return this.Rectangle;
			}
			set
			{
				this.Rectangle = value;
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006EBC File Offset: 0x000050BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ImageDrawObject CreateInvalid()
		{
			return new ImageDrawObject
			{
				IsValid = false
			};
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006EDC File Offset: 0x000050DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ImageDrawObject Create(in Rectangle2D rectangle, in Vec2 uvMin, in Vec2 uvMax)
		{
			return new ImageDrawObject
			{
				IsValid = true,
				Scale = 1f,
				Uvs = new Vec3(uvMin.x, uvMin.y, uvMax.x, uvMax.y),
				Rectangle = rectangle
			};
		}

		// Token: 0x040000A5 RID: 165
		public bool IsValid;

		// Token: 0x040000A6 RID: 166
		public float Scale;

		// Token: 0x040000A7 RID: 167
		public Rectangle2D Rectangle;

		// Token: 0x040000A8 RID: 168
		public Vec3 Uvs;
	}
}
