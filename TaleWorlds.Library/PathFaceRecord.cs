using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000079 RID: 121
	public struct PathFaceRecord
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x0000F7BC File Offset: 0x0000D9BC
		public PathFaceRecord(int index, int groupIndex, int islandIndex)
		{
			this.FaceIndex = index;
			this.FaceGroupIndex = groupIndex;
			this.FaceIslandIndex = islandIndex;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000F7D3 File Offset: 0x0000D9D3
		public bool IsValid()
		{
			return this.FaceIndex != -1;
		}

		// Token: 0x04000156 RID: 342
		public int FaceIndex;

		// Token: 0x04000157 RID: 343
		public int FaceGroupIndex;

		// Token: 0x04000158 RID: 344
		public int FaceIslandIndex;

		// Token: 0x04000159 RID: 345
		public static readonly PathFaceRecord NullFaceRecord = new PathFaceRecord(-1, -1, -1);
	}
}
