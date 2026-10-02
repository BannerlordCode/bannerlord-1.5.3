using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000290 RID: 656
	public struct MissionFormationSpawnData
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x00084BC2 File Offset: 0x00082DC2
		public int NumTroops
		{
			get
			{
				return this.FootTroopCount + this.MountedTroopCount;
			}
		}

		// Token: 0x04000E1F RID: 3615
		public int FootTroopCount;

		// Token: 0x04000E20 RID: 3616
		public int MountedTroopCount;
	}
}
