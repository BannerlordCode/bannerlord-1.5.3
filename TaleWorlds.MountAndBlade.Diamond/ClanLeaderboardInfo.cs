using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	public class ClanLeaderboardInfo
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x000073A9 File Offset: 0x000055A9
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x000073B0 File Offset: 0x000055B0
		public static ClanLeaderboardInfo Empty { get; private set; } = new ClanLeaderboardInfo(new ClanLeaderboardEntry[0]);

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000073CA File Offset: 0x000055CA
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x000073D2 File Offset: 0x000055D2
		[JsonProperty]
		public ClanLeaderboardEntry[] ClanEntries { get; private set; }

		// Token: 0x060005D0 RID: 1488 RVA: 0x000073DB File Offset: 0x000055DB
		public ClanLeaderboardInfo(ClanLeaderboardEntry[] entries)
		{
			this.ClanEntries = entries;
		}
	}
}
