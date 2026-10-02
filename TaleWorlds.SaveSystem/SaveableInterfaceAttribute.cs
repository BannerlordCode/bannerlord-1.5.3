using System;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Interface)]
	public class SaveableInterfaceAttribute : Attribute
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021E7 File Offset: 0x000003E7
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000021EF File Offset: 0x000003EF
		public int SaveId { get; set; }

		// Token: 0x06000012 RID: 18 RVA: 0x000021F8 File Offset: 0x000003F8
		public SaveableInterfaceAttribute(int saveId)
		{
			this.SaveId = saveId;
		}
	}
}
