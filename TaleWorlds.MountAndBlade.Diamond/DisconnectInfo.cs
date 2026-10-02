using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000116 RID: 278
	public class DisconnectInfo
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x000080C5 File Offset: 0x000062C5
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x000080CD File Offset: 0x000062CD
		public DisconnectType Type { get; set; }

		// Token: 0x0600062E RID: 1582 RVA: 0x000080D6 File Offset: 0x000062D6
		public DisconnectInfo()
		{
			this.Type = DisconnectType.Unknown;
		}
	}
}
