using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A8 RID: 168
	public class VirtualFileAttribute : Attribute
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x000166F0 File Offset: 0x000148F0
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x000166F8 File Offset: 0x000148F8
		public string Name { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00016701 File Offset: 0x00014901
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00016709 File Offset: 0x00014909
		public string Content { get; private set; }

		// Token: 0x0600066D RID: 1645 RVA: 0x00016712 File Offset: 0x00014912
		public VirtualFileAttribute(string name, string content)
		{
			this.Name = name;
			this.Content = content;
		}
	}
}
