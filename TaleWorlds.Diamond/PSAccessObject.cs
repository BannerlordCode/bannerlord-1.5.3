using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public class PSAccessObject : AccessObject
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003387 File Offset: 0x00001587
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000338F File Offset: 0x0000158F
		[JsonProperty]
		public int IssuerId { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003398 File Offset: 0x00001598
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x000033A0 File Offset: 0x000015A0
		[JsonProperty]
		public string AuthCode { get; private set; }

		// Token: 0x060000D1 RID: 209 RVA: 0x000033A9 File Offset: 0x000015A9
		public PSAccessObject()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000033B1 File Offset: 0x000015B1
		public PSAccessObject(int issuerId, string authCode)
		{
			base.Type = "PS";
			this.IssuerId = issuerId;
			this.AuthCode = authCode;
		}
	}
}
