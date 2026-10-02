using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DC RID: 732
	public class ViewMethod : Attribute
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x000A1E4B File Offset: 0x000A004B
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x000A1E53 File Offset: 0x000A0053
		public string Name { get; private set; }

		// Token: 0x06002A66 RID: 10854 RVA: 0x000A1E5C File Offset: 0x000A005C
		public ViewMethod(string name)
		{
			this.Name = name;
		}
	}
}
