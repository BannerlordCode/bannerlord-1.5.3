using System;

namespace Jose
{
	// Token: 0x0200000D RID: 13
	public class JwtOptions
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002D55 File Offset: 0x00000F55
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002D5D File Offset: 0x00000F5D
		public bool EncodePayload { get; set; } = true;

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002D66 File Offset: 0x00000F66
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002D6E File Offset: 0x00000F6E
		public bool DetachPayload { get; set; }

		// Token: 0x0400003D RID: 61
		public static readonly JwtOptions Default = new JwtOptions
		{
			EncodePayload = true,
			DetachPayload = false
		};
	}
}
