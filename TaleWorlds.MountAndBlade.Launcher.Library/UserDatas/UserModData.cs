using System;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x0200001B RID: 27
	public class UserModData
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005CB3 File Offset: 0x00003EB3
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005CBB File Offset: 0x00003EBB
		public string Id { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005CC4 File Offset: 0x00003EC4
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005CCC File Offset: 0x00003ECC
		public string LastKnownVersion { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005CD5 File Offset: 0x00003ED5
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00005CDD File Offset: 0x00003EDD
		public bool IsSelected { get; set; }

		// Token: 0x06000124 RID: 292 RVA: 0x00005CE6 File Offset: 0x00003EE6
		public UserModData()
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005CEE File Offset: 0x00003EEE
		public UserModData(string id, string lastKnownVersion, bool isSelected)
		{
			this.Id = id;
			this.LastKnownVersion = lastKnownVersion;
			this.IsSelected = isSelected;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005D0C File Offset: 0x00003F0C
		public bool IsUpdatedToBeDefault(ModuleInfo module)
		{
			return this.LastKnownVersion != module.Version.ToString() && module.IsDefault;
		}
	}
}
