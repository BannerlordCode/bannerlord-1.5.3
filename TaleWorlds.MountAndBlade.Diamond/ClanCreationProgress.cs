using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	public class ClanCreationProgress
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00007040 File Offset: 0x00005240
		public Progress Progress
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (ClanCreationPlayerData clanCreationPlayerData2 in this.ClanCreationPlayerData)
				{
					if (clanCreationPlayerData2.ClanCreationAnswer == ClanCreationAnswer.Accepted)
					{
						num++;
					}
					else if (clanCreationPlayerData2.ClanCreationAnswer == ClanCreationAnswer.Declined)
					{
						num2++;
					}
				}
				if (num == this.ClanCreationPlayerData.Length)
				{
					return Progress.Success;
				}
				if (num2 > 0)
				{
					return Progress.Fail;
				}
				return Progress.Undecided;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000709D File Offset: 0x0000529D
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x000070A5 File Offset: 0x000052A5
		public ClanCreationPlayerData[] ClanCreationPlayerData { get; private set; }

		// Token: 0x0600058F RID: 1423 RVA: 0x000070AE File Offset: 0x000052AE
		public ClanCreationProgress(ClanCreationPlayerData[] clanCreationPlayerData)
		{
			this.ClanCreationPlayerData = clanCreationPlayerData;
		}
	}
}
