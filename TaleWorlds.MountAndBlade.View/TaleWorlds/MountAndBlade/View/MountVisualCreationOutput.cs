using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200001B RID: 27
	public struct MountVisualCreationOutput
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005D38 File Offset: 0x00003F38
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00005D40 File Offset: 0x00003F40
		public MetaMesh HorseManeMesh { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005D49 File Offset: 0x00003F49
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00005D51 File Offset: 0x00003F51
		public MetaMesh MountMesh { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00005D5A File Offset: 0x00003F5A
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00005D62 File Offset: 0x00003F62
		public MetaMesh ReinMesh { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00005D6B File Offset: 0x00003F6B
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00005D73 File Offset: 0x00003F73
		public MetaMesh MountHarnessMesh { get; private set; }

		// Token: 0x060000B9 RID: 185 RVA: 0x00005D7C File Offset: 0x00003F7C
		public MountVisualCreationOutput(MetaMesh horseManeMesh, MetaMesh mountMesh, MetaMesh reinMesh, MetaMesh mountHarnessMesh)
		{
			this.HorseManeMesh = horseManeMesh;
			this.MountMesh = mountMesh;
			this.ReinMesh = reinMesh;
			this.MountHarnessMesh = mountHarnessMesh;
		}
	}
}
