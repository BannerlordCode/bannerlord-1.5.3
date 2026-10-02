using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x0200001A RID: 26
	public class UserGameTypeData
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00005C8F File Offset: 0x00003E8F
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00005C97 File Offset: 0x00003E97
		public List<UserModData> ModDatas { get; set; }

		// Token: 0x0600011D RID: 285 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public UserGameTypeData()
		{
			this.ModDatas = new List<UserModData>();
		}
	}
}
