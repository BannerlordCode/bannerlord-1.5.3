using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200000D RID: 13
	public class HandlerResult
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000029EA File Offset: 0x00000BEA
		public bool IsSuccessful { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000029F2 File Offset: 0x00000BF2
		public string Error { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000029FA File Offset: 0x00000BFA
		public Message NextMessage { get; }

		// Token: 0x0600004A RID: 74 RVA: 0x00002A02 File Offset: 0x00000C02
		protected HandlerResult(bool isSuccessful, string error = null, Message followUp = null)
		{
			this.IsSuccessful = isSuccessful;
			this.Error = error;
			this.NextMessage = followUp;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A1F File Offset: 0x00000C1F
		public static HandlerResult CreateSuccessful()
		{
			return new HandlerResult(true, null, null);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A29 File Offset: 0x00000C29
		public static HandlerResult CreateSuccessful(Message nextMessage)
		{
			return new HandlerResult(true, null, nextMessage);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002A33 File Offset: 0x00000C33
		public static HandlerResult CreateFailed(string error)
		{
			return new HandlerResult(false, error, null);
		}
	}
}
