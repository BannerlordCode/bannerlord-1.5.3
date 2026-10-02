using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x020000A0 RID: 160
	public struct WorldFrame
	{
		// Token: 0x06000F23 RID: 3875 RVA: 0x00011C04 File Offset: 0x0000FE04
		public WorldFrame(Mat3 rotation, WorldPosition origin)
		{
			this.Rotation = rotation;
			this.Origin = origin;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00011C14 File Offset: 0x0000FE14
		public bool IsValid
		{
			get
			{
				return this.Origin.IsValid;
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00011C24 File Offset: 0x0000FE24
		public MatrixFrame ToGroundMatrixFrame()
		{
			Vec3 groundVec = this.Origin.GetGroundVec3();
			return new MatrixFrame(in this.Rotation, in groundVec);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00011C4C File Offset: 0x0000FE4C
		public MatrixFrame ToGroundMatrixFrameMT()
		{
			Vec3 groundVec3MT = this.Origin.GetGroundVec3MT();
			return new MatrixFrame(in this.Rotation, in groundVec3MT);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00011C74 File Offset: 0x0000FE74
		public MatrixFrame ToNavMeshMatrixFrame()
		{
			Vec3 navMeshVec = this.Origin.GetNavMeshVec3();
			return new MatrixFrame(in this.Rotation, in navMeshVec);
		}

		// Token: 0x0400020A RID: 522
		public Mat3 Rotation;

		// Token: 0x0400020B RID: 523
		public WorldPosition Origin;

		// Token: 0x0400020C RID: 524
		public static readonly WorldFrame Invalid = new WorldFrame(Mat3.Identity, WorldPosition.Invalid);
	}
}
