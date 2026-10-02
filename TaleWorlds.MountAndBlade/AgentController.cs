using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000103 RID: 259
	public class AgentController
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00017569 File Offset: 0x00015769
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00017571 File Offset: 0x00015771
		public Agent Owner { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0001757A File Offset: 0x0001577A
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x00017582 File Offset: 0x00015782
		public Mission Mission { get; set; }

		// Token: 0x06000C6F RID: 3183 RVA: 0x0001758B File Offset: 0x0001578B
		public virtual void OnInitialize()
		{
		}
	}
}
