using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B5 RID: 693
	public class BotData
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x00091333 File Offset: 0x0008F533
		public int Score
		{
			get
			{
				return this.KillCount * 3 + this.AssistCount;
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x00091344 File Offset: 0x0008F544
		public BotData()
		{
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0009134C File Offset: 0x0008F54C
		public BotData(int kill, int assist, int death, int alive)
		{
			this.KillCount = kill;
			this.DeathCount = death;
			this.AssistCount = assist;
			this.AliveCount = alive;
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x00091371 File Offset: 0x0008F571
		public bool IsAnyValid
		{
			get
			{
				return this.KillCount != 0 || this.DeathCount != 0 || this.AssistCount != 0 || this.AliveCount != 0;
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00091396 File Offset: 0x0008F596
		public void ResetKillDeathAssist()
		{
			this.KillCount = 0;
			this.DeathCount = 0;
			this.AssistCount = 0;
		}

		// Token: 0x04000ED6 RID: 3798
		public int AliveCount;

		// Token: 0x04000ED7 RID: 3799
		public int KillCount;

		// Token: 0x04000ED8 RID: 3800
		public int DeathCount;

		// Token: 0x04000ED9 RID: 3801
		public int AssistCount;
	}
}
