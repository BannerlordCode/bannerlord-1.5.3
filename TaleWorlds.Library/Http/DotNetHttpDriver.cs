using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000AE RID: 174
	public class DotNetHttpDriver : IHttpDriver
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x00016DE0 File Offset: 0x00014FE0
		public DotNetHttpDriver()
		{
			ServicePointManager.DefaultConnectionLimit = 5;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			this._httpClient = new HttpClient();
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00016E03 File Offset: 0x00015003
		IHttpRequestTask IHttpDriver.CreateHttpPostRequestTask(string address, string postData, bool withUserToken, CancellationToken cancellationToken)
		{
			return new HttpPostRequest(this._httpClient, address, postData, cancellationToken);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00016E14 File Offset: 0x00015014
		IHttpRequestTask IHttpDriver.CreateHttpGetRequestTask(string address, bool withUserToken)
		{
			return new HttpGetRequest(this._httpClient, address);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00016E24 File Offset: 0x00015024
		async Task<string> IHttpDriver.HttpGetString(string url, bool withUserToken)
		{
			HttpResponseMessage httpResponseMessage = await this._httpClient.GetAsync(url);
			HttpResponseMessage responseMessage = httpResponseMessage;
			string text = await responseMessage.Content.ReadAsStringAsync();
			if (!responseMessage.IsSuccessStatusCode)
			{
				throw new Exception(text);
			}
			return text;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00016E74 File Offset: 0x00015074
		async Task<string> IHttpDriver.HttpPostString(string url, string postData, string mediaType, bool withUserToken)
		{
			HttpResponseMessage httpResponseMessage = await this._httpClient.PostAsync(url, new StringContent(postData, Encoding.Unicode, mediaType));
			string text;
			using (HttpResponseMessage response = httpResponseMessage)
			{
				using (HttpContent content = response.Content)
				{
					text = await content.ReadAsStringAsync();
				}
			}
			return text;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00016ED4 File Offset: 0x000150D4
		async Task<byte[]> IHttpDriver.HttpDownloadData(string url)
		{
			return await this._httpClient.GetByteArrayAsync(url);
		}

		// Token: 0x040001FB RID: 507
		private HttpClient _httpClient;
	}
}
