using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000177 RID: 375
	public class PlayerInfo
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00011269 File Offset: 0x0000F469
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x00011271 File Offset: 0x0000F471
		public string PlayerId { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0001127A File Offset: 0x0000F47A
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x00011282 File Offset: 0x0000F482
		public string Username { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0001128B File Offset: 0x0000F48B
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x00011293 File Offset: 0x0000F493
		public int ForcedIndex { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0001129C File Offset: 0x0000F49C
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x000112A4 File Offset: 0x0000F4A4
		public int TeamNo { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x000112AD File Offset: 0x0000F4AD
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x000112B5 File Offset: 0x0000F4B5
		public int Kill { get; set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x000112BE File Offset: 0x0000F4BE
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x000112C6 File Offset: 0x0000F4C6
		public int Death { get; set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x000112CF File Offset: 0x0000F4CF
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x000112D7 File Offset: 0x0000F4D7
		public int Assist { get; set; }

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000112E0 File Offset: 0x0000F4E0
		public bool HasSameContentWith(PlayerInfo other)
		{
			return this.PlayerId == other.PlayerId && this.Username == other.Username && this.ForcedIndex == other.ForcedIndex && this.TeamNo == other.TeamNo && this.Kill == other.Kill && this.Death == other.Death && this.Assist == other.Assist;
		}
	}
}
