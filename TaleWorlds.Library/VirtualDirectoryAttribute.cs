using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A7 RID: 167
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class VirtualDirectoryAttribute : Attribute
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x000166D0 File Offset: 0x000148D0
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x000166D8 File Offset: 0x000148D8
		public string Name { get; private set; }

		// Token: 0x06000668 RID: 1640 RVA: 0x000166E1 File Offset: 0x000148E1
		public VirtualDirectoryAttribute(string name)
		{
			this.Name = name;
		}
	}
}
