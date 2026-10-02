using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000153 RID: 339
	[Serializable]
	public class PremadeGameList
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0000E0AB File Offset: 0x0000C2AB
		public static PremadeGameList Empty { get; private set; } = new PremadeGameList(new PremadeGameEntry[0]);

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0000E0C5 File Offset: 0x0000C2C5
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x0000E0CD File Offset: 0x0000C2CD
		[JsonProperty]
		public PremadeGameEntry[] PremadeGameEntries { get; private set; }

		// Token: 0x0600098E RID: 2446 RVA: 0x0000E0D6 File Offset: 0x0000C2D6
		public PremadeGameList(PremadeGameEntry[] entries)
		{
			this.PremadeGameEntries = entries;
		}
	}
}
