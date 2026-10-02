using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000040 RID: 64
	[DataContract]
	[Serializable]
	public sealed class RestResponse : RestData
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00005448 File Offset: 0x00003648
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00005450 File Offset: 0x00003650
		[DataMember]
		public bool Successful { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00005459 File Offset: 0x00003659
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00005461 File Offset: 0x00003661
		[DataMember]
		public string SuccessfulReason { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000546A File Offset: 0x0000366A
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00005472 File Offset: 0x00003672
		[DataMember]
		public RestFunctionResult FunctionResult { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000547B File Offset: 0x0000367B
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00005483 File Offset: 0x00003683
		[DataMember]
		public byte[] UserCertificate { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000548C File Offset: 0x0000368C
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00005494 File Offset: 0x00003694
		[DataMember]
		public bool Polled { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000549D File Offset: 0x0000369D
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000054A5 File Offset: 0x000036A5
		[DataMember]
		public string ErrorDetail { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000054AE File Offset: 0x000036AE
		public int RemainingMessageCount
		{
			get
			{
				if (this._responseMessages != null)
				{
					return this._responseMessages.Count;
				}
				return 0;
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000054C5 File Offset: 0x000036C5
		public RestResponse()
		{
			this._responseMessages = new List<RestResponseMessage>();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000054D8 File Offset: 0x000036D8
		public void SetSuccessful(bool successful, string successfulReason)
		{
			this.Successful = successful;
			this.SuccessfulReason = successfulReason;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000054E8 File Offset: 0x000036E8
		public static RestResponse Create(bool successful, string successfulReason)
		{
			RestResponse restResponse = new RestResponse();
			restResponse.SetSuccessful(successful, successfulReason);
			return restResponse;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000054F7 File Offset: 0x000036F7
		public static RestResponse CreateFailure(string reason, string errorDetail = null)
		{
			RestResponse restResponse = new RestResponse();
			restResponse.SetSuccessful(false, reason);
			restResponse.ErrorDetail = errorDetail;
			return restResponse;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000550D File Offset: 0x0000370D
		public RestResponseMessage TryDequeueMessage()
		{
			if (this._responseMessages != null && this._responseMessages.Count > 0)
			{
				RestResponseMessage restResponseMessage = this._responseMessages[0];
				this._responseMessages.RemoveAt(0);
				return restResponseMessage;
			}
			return null;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000553F File Offset: 0x0000373F
		public void ClearMessageQueue()
		{
			this._responseMessages.Clear();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000554C File Offset: 0x0000374C
		public void EnqueueMessage(RestResponseMessage message)
		{
			this._responseMessages.Add(message);
		}

		// Token: 0x04000099 RID: 153
		[DataMember]
		private List<RestResponseMessage> _responseMessages;
	}
}
