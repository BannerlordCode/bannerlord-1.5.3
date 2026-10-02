using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class GOGAccessObject : AccessObject
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000296E File Offset: 0x00000B6E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002976 File Offset: 0x00000B76
		[JsonProperty]
		public ulong GogId { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000297F File Offset: 0x00000B7F
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002987 File Offset: 0x00000B87
		[JsonProperty]
		public ulong OldId { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002990 File Offset: 0x00000B90
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002998 File Offset: 0x00000B98
		[JsonProperty]
		public string UserName { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000029A1 File Offset: 0x00000BA1
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000029A9 File Offset: 0x00000BA9
		[JsonProperty]
		public string Ticket { get; set; }

		// Token: 0x06000045 RID: 69 RVA: 0x000029B2 File Offset: 0x00000BB2
		public GOGAccessObject()
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000029BA File Offset: 0x00000BBA
		public GOGAccessObject(string userName, ulong gogId, ulong oldId, string ticket)
		{
			base.Type = "GOG";
			this.UserName = userName;
			this.GogId = gogId;
			this.Ticket = ticket;
			this.OldId = oldId;
		}
	}
}
