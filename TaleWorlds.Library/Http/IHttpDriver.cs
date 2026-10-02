using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B3 RID: 179
	public interface IHttpDriver
	{
		// Token: 0x060006BE RID: 1726
		Task<string> HttpGetString(string url, bool withUserToken);

		// Token: 0x060006BF RID: 1727
		Task<string> HttpPostString(string url, string postData, string mediaType, bool withUserToken);

		// Token: 0x060006C0 RID: 1728
		Task<byte[]> HttpDownloadData(string url);

		// Token: 0x060006C1 RID: 1729
		IHttpRequestTask CreateHttpPostRequestTask(string address, string postData, bool withUserToken, CancellationToken cancellationToken);

		// Token: 0x060006C2 RID: 1730
		IHttpRequestTask CreateHttpGetRequestTask(string address, bool withUserToken);
	}
}
