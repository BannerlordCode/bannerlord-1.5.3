using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.Property)]
	public class SaveablePropertyAttribute : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002207 File Offset: 0x00000407
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000220F File Offset: 0x0000040F
		public short LocalSaveId { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x00002218 File Offset: 0x00000418
		public SaveablePropertyAttribute(short localSaveId)
		{
			this.LocalSaveId = localSaveId;
		}
	}
}
