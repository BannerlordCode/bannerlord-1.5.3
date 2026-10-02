using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032B RID: 811
	[AttributeUsage(AttributeTargets.Method)]
	public class ConsoleCommandMethod : Attribute
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x000B3314 File Offset: 0x000B1514
		// (set) Token: 0x06002E48 RID: 11848 RVA: 0x000B331C File Offset: 0x000B151C
		public string CommandName { get; private set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002E49 RID: 11849 RVA: 0x000B3325 File Offset: 0x000B1525
		// (set) Token: 0x06002E4A RID: 11850 RVA: 0x000B332D File Offset: 0x000B152D
		public string Description { get; private set; }

		// Token: 0x06002E4B RID: 11851 RVA: 0x000B3336 File Offset: 0x000B1536
		public ConsoleCommandMethod(string commandName, string description)
		{
			this.CommandName = commandName;
			this.Description = description;
		}
	}
}
