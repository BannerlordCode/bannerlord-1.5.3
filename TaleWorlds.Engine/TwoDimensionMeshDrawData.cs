using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000099 RID: 153
	[EngineStruct("rglTwo_dimension_mesh_draw_data", false, null)]
	public struct TwoDimensionMeshDrawData
	{
		// Token: 0x040001DA RID: 474
		public MatrixFrame MatrixFrame;

		// Token: 0x040001DB RID: 475
		public Vec3 ClipRectInfo;

		// Token: 0x040001DC RID: 476
		public Vec3 Uvs;

		// Token: 0x040001DD RID: 477
		public Vec2 SpriteSize;

		// Token: 0x040001DE RID: 478
		public Vec2 ScreenSize;

		// Token: 0x040001DF RID: 479
		public Vec2 ScreenScale;

		// Token: 0x040001E0 RID: 480
		public Vec3 NinePatchBorders;

		// Token: 0x040001E1 RID: 481
		public Vec2 ClipCircleCenter;

		// Token: 0x040001E2 RID: 482
		public float ClipCircleRadius;

		// Token: 0x040001E3 RID: 483
		public float ClipCircleSmoothingRadius;

		// Token: 0x040001E4 RID: 484
		public uint Color;

		// Token: 0x040001E5 RID: 485
		public float ColorFactor;

		// Token: 0x040001E6 RID: 486
		public float AlphaFactor;

		// Token: 0x040001E7 RID: 487
		public float HueFactor;

		// Token: 0x040001E8 RID: 488
		public float SaturationFactor;

		// Token: 0x040001E9 RID: 489
		public float ValueFactor;

		// Token: 0x040001EA RID: 490
		public Vec2 OverlayOffset;

		// Token: 0x040001EB RID: 491
		public Vec2 OverlayScale;

		// Token: 0x040001EC RID: 492
		public int Layer;
	}
}
