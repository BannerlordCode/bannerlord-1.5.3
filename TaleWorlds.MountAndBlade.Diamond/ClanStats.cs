using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000110 RID: 272
	[Serializable]
	public class ClanStats
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000074EE File Offset: 0x000056EE
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x000074F6 File Offset: 0x000056F6
		public int WinCount { get; private set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x000074FF File Offset: 0x000056FF
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00007507 File Offset: 0x00005707
		public int LossCount { get; private set; }

		// Token: 0x060005EB RID: 1515 RVA: 0x00007510 File Offset: 0x00005710
		public ClanStats(int winCount, int lossCount)
		{
			this.WinCount = winCount;
			this.LossCount = lossCount;
		}
	}
}
