using System;
using System.Reflection;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CE RID: 462
	public class MissionInfo
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x000609D1 File Offset: 0x0005EBD1
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x000609D9 File Offset: 0x0005EBD9
		public string Name { get; set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x000609E2 File Offset: 0x0005EBE2
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x000609EA File Offset: 0x0005EBEA
		public MethodInfo Creator { get; set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000609F3 File Offset: 0x0005EBF3
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x000609FB File Offset: 0x0005EBFB
		public Type Manager { get; set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x00060A04 File Offset: 0x0005EC04
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x00060A0C File Offset: 0x0005EC0C
		public bool UsableByEditor { get; set; }
	}
}
