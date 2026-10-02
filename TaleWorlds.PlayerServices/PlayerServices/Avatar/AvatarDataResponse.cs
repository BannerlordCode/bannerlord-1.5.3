using System;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000B RID: 11
	public class AvatarDataResponse
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002F2B File Offset: 0x0000112B
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002F33 File Offset: 0x00001133
		public bool IsFallBack { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002F3C File Offset: 0x0000113C
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002F44 File Offset: 0x00001144
		public AvatarData AvatarData { get; private set; }

		// Token: 0x06000062 RID: 98 RVA: 0x00002F4D File Offset: 0x0000114D
		public AvatarDataResponse(bool isFallBack, AvatarData avatarData)
		{
			this.IsFallBack = isFallBack;
			this.AvatarData = avatarData;
		}
	}
}
