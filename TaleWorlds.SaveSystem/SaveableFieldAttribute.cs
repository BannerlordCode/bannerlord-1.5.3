using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000005 RID: 5
	[AttributeUsage(AttributeTargets.Field)]
	public class SaveableFieldAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000021C7 File Offset: 0x000003C7
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000021CF File Offset: 0x000003CF
		public short LocalSaveId { get; set; }

		// Token: 0x0600000F RID: 15 RVA: 0x000021D8 File Offset: 0x000003D8
		public SaveableFieldAttribute(short localSaveId)
		{
			this.LocalSaveId = localSaveId;
		}
	}
}
