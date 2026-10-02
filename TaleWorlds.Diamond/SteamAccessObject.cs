using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public class SteamAccessObject : AccessObject
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003505 File Offset: 0x00001705
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000350D File Offset: 0x0000170D
		[JsonProperty]
		public string UserName { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003516 File Offset: 0x00001716
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000351E File Offset: 0x0000171E
		[JsonProperty]
		public string ExternalAccessToken { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003527 File Offset: 0x00001727
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000352F File Offset: 0x0000172F
		[JsonProperty]
		public int AppId { get; private set; }

		// Token: 0x060000E9 RID: 233 RVA: 0x00003538 File Offset: 0x00001738
		public SteamAccessObject()
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003540 File Offset: 0x00001740
		public SteamAccessObject(string userName, string externalAccessToken, int appId)
		{
			base.Type = "Steam";
			this.UserName = userName;
			this.ExternalAccessToken = externalAccessToken;
			this.AppId = appId;
		}
	}
}
