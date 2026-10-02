using System;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x0200001D RID: 29
	public class DLLCheckData
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005D66 File Offset: 0x00003F66
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00005D6E File Offset: 0x00003F6E
		public string DLLName { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005D77 File Offset: 0x00003F77
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00005D7F File Offset: 0x00003F7F
		public string DLLVerifyInformation { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00005D88 File Offset: 0x00003F88
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00005D90 File Offset: 0x00003F90
		public uint LatestSizeInBytes { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005D99 File Offset: 0x00003F99
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00005DA1 File Offset: 0x00003FA1
		public bool IsDangerous { get; set; }

		// Token: 0x06000132 RID: 306 RVA: 0x00005DAA File Offset: 0x00003FAA
		public DLLCheckData(string dllname)
		{
			this.LatestSizeInBytes = 0U;
			this.IsDangerous = true;
			this.DLLName = dllname;
			this.DLLVerifyInformation = "";
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005DD2 File Offset: 0x00003FD2
		public DLLCheckData()
		{
		}
	}
}
