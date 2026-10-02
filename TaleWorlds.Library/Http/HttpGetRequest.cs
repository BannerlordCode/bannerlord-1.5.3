using System;
using System.Net;
using System.Net.Http;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B0 RID: 176
	public class HttpGetRequest : IHttpRequestTask
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00016FC6 File Offset: 0x000151C6
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00016FCE File Offset: 0x000151CE
		public HttpRequestTaskState State { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00016FD7 File Offset: 0x000151D7
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x00016FDF File Offset: 0x000151DF
		public bool Successful { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00016FE8 File Offset: 0x000151E8
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x00016FF0 File Offset: 0x000151F0
		public string ResponseData { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00016FF9 File Offset: 0x000151F9
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x00017001 File Offset: 0x00015201
		public HttpStatusCode ResponseStatusCode { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001700A File Offset: 0x0001520A
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x00017012 File Offset: 0x00015212
		public Exception Exception { get; private set; }

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001701B File Offset: 0x0001521B
		public HttpGetRequest(HttpClient httpClient, string address)
			: this(httpClient, address, new Version("1.1"))
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001702F File Offset: 0x0001522F
		public HttpGetRequest(HttpClient httpClient, string address, Version version)
		{
			this._versionToUse = version;
			this._address = address;
			this._httpClient = httpClient;
			this.State = HttpRequestTaskState.NotStarted;
			this.ResponseData = "";
			this.ResponseStatusCode = HttpStatusCode.OK;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00017069 File Offset: 0x00015269
		private void SetFinishedAsSuccessful(string responseData, HttpStatusCode statusCode)
		{
			this.Successful = true;
			this.ResponseData = responseData;
			this.ResponseStatusCode = statusCode;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00017087 File Offset: 0x00015287
		private void SetFinishedAsUnsuccessful(Exception e)
		{
			this.Successful = false;
			this.Exception = e;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001709E File Offset: 0x0001529E
		public void Start()
		{
			this.DoTask();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000170A8 File Offset: 0x000152A8
		private async void DoTask()
		{
			this.State = HttpRequestTaskState.Working;
			try
			{
				using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, this._address))
				{
					requestMessage.Version = this._versionToUse;
					requestMessage.Headers.Add("Accept", "application/json");
					requestMessage.Headers.Add("UserAgent", "TaleWorlds Client");
					HttpResponseMessage httpResponseMessage = await this._httpClient.SendAsync(requestMessage);
					using (HttpResponseMessage response = httpResponseMessage)
					{
						bool isSuccessStatusCode = response.IsSuccessStatusCode;
						response.EnsureSuccessStatusCode();
						Debug.Print(string.Concat(new object[] { "Protocol version used for get request to ", this._address, " is: ", response.Version }), 0, Debug.DebugColor.White, 17592186044416UL);
						using (HttpContent content = response.Content)
						{
							this.SetFinishedAsSuccessful(await content.ReadAsStringAsync(), response.StatusCode);
						}
						HttpContent content = null;
					}
					HttpResponseMessage response = null;
				}
				HttpRequestMessage requestMessage = null;
			}
			catch (Exception ex)
			{
				this.SetFinishedAsUnsuccessful(ex);
			}
		}

		// Token: 0x040001FE RID: 510
		private const int BufferSize = 1024;

		// Token: 0x040001FF RID: 511
		private HttpClient _httpClient;

		// Token: 0x04000200 RID: 512
		private readonly string _address;

		// Token: 0x04000206 RID: 518
		private Version _versionToUse;
	}
}
