using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.Class)]
	public class SaveableRootClassAttribute : Attribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002227 File Offset: 0x00000427
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000222F File Offset: 0x0000042F
		public int SaveId { get; set; }

		// Token: 0x06000018 RID: 24 RVA: 0x00002238 File Offset: 0x00000438
		public SaveableRootClassAttribute(int saveId)
		{
			this.SaveId = saveId;
		}
	}
}
