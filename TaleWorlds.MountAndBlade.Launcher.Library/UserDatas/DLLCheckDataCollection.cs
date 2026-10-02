using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x0200001C RID: 28
	public class DLLCheckDataCollection
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005D42 File Offset: 0x00003F42
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00005D4A File Offset: 0x00003F4A
		public List<DLLCheckData> DLLData { get; set; }

		// Token: 0x06000129 RID: 297 RVA: 0x00005D53 File Offset: 0x00003F53
		public DLLCheckDataCollection()
		{
			this.DLLData = new List<DLLCheckData>();
		}
	}
}
