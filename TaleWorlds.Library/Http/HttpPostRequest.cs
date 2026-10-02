using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B1 RID: 177
	public class HttpPostRequest : IHttpRequestTask
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000170E1 File Offset: 0x000152E1
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x000170E9 File Offset: 0x000152E9
		public HttpRequestTaskState State { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x000170F2 File Offset: 0x000152F2
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x000170FA File Offset: 0x000152FA
		public bool Successful { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00017103 File Offset: 0x00015303
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0001710B File Offset: 0x0001530B
		public string ResponseData { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00017114 File Offset: 0x00015314
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0001711C File Offset: 0x0001531C
		public Exception Exception { get; private set; }

		// Token: 0x060006B7 RID: 1719 RVA: 0x00017125 File Offset: 0x00015325
		public HttpPostRequest(HttpClient httpClient, string address, string postData, CancellationToken cancellationToken)
			: this(httpClient, address, postData, new Version("1.1"), cancellationToken)
		{
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001713C File Offset: 0x0001533C
		public HttpPostRequest(HttpClient httpClient, string address, string postData, Version version, CancellationToken cancellationToken)
		{
			this._httpClient = httpClient;
			this._postData = postData;
			this._address = address;
			this.State = HttpRequestTaskState.NotStarted;
			this.ResponseData = "";
			this._versionToUse = version;
			this._cancellationToken = cancellationToken;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001717B File Offset: 0x0001537B
		private void SetFinishedAsSuccessful(string responseData)
		{
			this.Successful = true;
			this.ResponseData = responseData;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00017192 File Offset: 0x00015392
		private void SetFinishedAsUnsuccessful(Exception e)
		{
			this.Successful = false;
			this.Exception = e;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000171A9 File Offset: 0x000153A9
		public void Start()
		{
			this.DoTask();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000171B1 File Offset: 0x000153B1
		private static Exception GetRootCause(Exception e)
		{
			while (e.InnerException != null)
			{
				e = e.InnerException;
			}
			return e;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000171C8 File Offset: 0x000153C8
		private async void DoTask()
		{
			this.State = HttpRequestTaskState.Working;
			try
			{
				Debug.Print("Http Post Request to " + this._address, 0, Debug.DebugColor.White, 17592186044416UL);
				using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, this._address))
				{
					requestMessage.Version = this._versionToUse;
					requestMessage.Headers.Add("Accept", "application/json");
					requestMessage.Headers.Add("UserAgent", "TaleWorlds Client");
					requestMessage.Content = new StringContent(this._postData, Encoding.Unicode, "application/json");
					HttpResponseMessage httpResponseMessage = await this._httpClient.SendAsync(requestMessage, this._cancellationToken);
					using (HttpResponseMessage response = httpResponseMessage)
					{
						bool isSuccessStatusCode = response.IsSuccessStatusCode;
						response.EnsureSuccessStatusCode();
						Debug.Print(string.Concat(new object[] { "Protocol version used for post request to ", this._address, " is: ", response.Version }), 0, Debug.DebugColor.White, 17592186044416UL);
						using (HttpContent content = response.Content)
						{
							this.SetFinishedAsSuccessful(await content.ReadAsStringAsync());
						}
						HttpContent content = null;
					}
					HttpResponseMessage response = null;
				}
				HttpRequestMessage requestMessage = null;
			}
			catch (Exception ex)
			{
				bool flag = ex is OperationCanceledException || this._cancellationToken.IsCancellationRequested;
				Exception rootCause = HttpPostRequest.GetRootCause(ex);
				if (flag | (ex is HttpRequestException && (rootCause is SocketException || rootCause is IOException)))
				{
					Debug.Print(string.Concat(new string[]
					{
						"Http post request to ",
						this._address,
						" aborted (shutdown or target unavailable): ",
						ex.Message,
						" [",
						rootCause.GetType().Name,
						"]"
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				this.SetFinishedAsUnsuccessful(ex);
			}
		}

		// Token: 0x04000207 RID: 519
		private HttpClient _httpClient;

		// Token: 0x04000208 RID: 520
		private readonly string _address;

		// Token: 0x04000209 RID: 521
		private string _postData;

		// Token: 0x0400020E RID: 526
		private Version _versionToUse;

		// Token: 0x0400020F RID: 527
		private CancellationToken _cancellationToken;
	}
}
