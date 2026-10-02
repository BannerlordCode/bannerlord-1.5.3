using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028F RID: 655
	public class MissionSpawnPhase
	{
		// Token: 0x06002498 RID: 9368 RVA: 0x00084BA5 File Offset: 0x00082DA5
		public void OnInitialTroopsSpawned()
		{
			this.InitialSpawnedNumber = this.InitialSpawnNumber;
			this.InitialSpawnNumber = 0;
		}

		// Token: 0x04000E1A RID: 3610
		public int TotalSpawnNumber;

		// Token: 0x04000E1B RID: 3611
		public int InitialSpawnedNumber;

		// Token: 0x04000E1C RID: 3612
		public int InitialSpawnNumber;

		// Token: 0x04000E1D RID: 3613
		public int RemainingSpawnNumber;

		// Token: 0x04000E1E RID: 3614
		public int NumberActiveTroops;
	}
}
