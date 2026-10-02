using System;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x02000009 RID: 9
	public class GOGAchievement
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003135 File Offset: 0x00001335
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000313D File Offset: 0x0000133D
		public string AchievementName { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003146 File Offset: 0x00001346
		// (set) Token: 0x0600006C RID: 108 RVA: 0x0000314E File Offset: 0x0000134E
		public string Name { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003157 File Offset: 0x00001357
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000315F File Offset: 0x0000135F
		public string Description { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003168 File Offset: 0x00001368
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003170 File Offset: 0x00001370
		public bool Achieved { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003179 File Offset: 0x00001379
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003181 File Offset: 0x00001381
		public int Progress { get; set; }

		// Token: 0x0400001A RID: 26
		public int AchievementID;
	}
}
