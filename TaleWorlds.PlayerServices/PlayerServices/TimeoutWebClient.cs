using System;
using System.Net;

namespace TaleWorlds.PlayerServices
{
	// Token: 0x02000007 RID: 7
	public class TimeoutWebClient : WebClient
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000029C3 File Offset: 0x00000BC3
		public TimeoutWebClient()
		{
			this.Timeout = 15000;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000029D6 File Offset: 0x00000BD6
		public TimeoutWebClient(int timeout)
		{
			this.Timeout = timeout;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000029E5 File Offset: 0x00000BE5
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000029ED File Offset: 0x00000BED
		public int Timeout { get; set; }

		// Token: 0x0600002E RID: 46 RVA: 0x000029F6 File Offset: 0x00000BF6
		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			webRequest.Timeout = this.Timeout;
			return webRequest;
		}
	}
}
