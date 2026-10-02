using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C3 RID: 195
	public class JsonSelectSettings
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002A820 File Offset: 0x00028A20
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0002A828 File Offset: 0x00028A28
		public TimeSpan? RegexMatchTimeout { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002A831 File Offset: 0x00028A31
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0002A839 File Offset: 0x00028A39
		public bool ErrorWhenNoMatch { get; set; }
	}
}
