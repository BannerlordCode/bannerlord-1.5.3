using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006D RID: 109
	public class CustomField
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001157C File Offset: 0x0000F77C
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00011584 File Offset: 0x0000F784
		public string Name { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0001158D File Offset: 0x0000F78D
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00011595 File Offset: 0x0000F795
		public short SaveId { get; private set; }

		// Token: 0x060003C1 RID: 961 RVA: 0x0001159E File Offset: 0x0000F79E
		public CustomField(string name, short saveId)
		{
			this.Name = name;
			this.SaveId = saveId;
		}
	}
}
