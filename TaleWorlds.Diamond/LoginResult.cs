using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public sealed class LoginResult : FunctionResult
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002AF8 File Offset: 0x00000CF8
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002B00 File Offset: 0x00000D00
		[JsonProperty]
		public PeerId PeerId { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002B09 File Offset: 0x00000D09
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00002B11 File Offset: 0x00000D11
		[JsonProperty]
		public SessionKey SessionKey { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00002B1A File Offset: 0x00000D1A
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00002B22 File Offset: 0x00000D22
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00002B2B File Offset: 0x00000D2B
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002B33 File Offset: 0x00000D33
		[JsonProperty]
		public string ErrorCode { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002B3C File Offset: 0x00000D3C
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002B44 File Offset: 0x00000D44
		[JsonProperty]
		public Dictionary<string, string> ErrorParameters { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002B4D File Offset: 0x00000D4D
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00002B55 File Offset: 0x00000D55
		[JsonProperty]
		public string ProviderResponse { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002B5E File Offset: 0x00000D5E
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00002B66 File Offset: 0x00000D66
		[JsonProperty]
		public LoginResultObject LoginResultObject { get; private set; }

		// Token: 0x0600009F RID: 159 RVA: 0x00002B6F File Offset: 0x00000D6F
		public LoginResult()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002B77 File Offset: 0x00000D77
		public LoginResult(PeerId peerId, SessionKey sessionKey, LoginResultObject loginResultObject)
		{
			this.PeerId = peerId;
			this.SessionKey = sessionKey;
			this.Successful = true;
			this.ErrorCode = "";
			this.LoginResultObject = loginResultObject;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002BA6 File Offset: 0x00000DA6
		public LoginResult(PeerId peerId, SessionKey sessionKey)
			: this(peerId, sessionKey, null)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002BB1 File Offset: 0x00000DB1
		public LoginResult(string errorCode, Dictionary<string, string> parameters = null)
		{
			this.ErrorCode = errorCode;
			this.Successful = false;
			this.ErrorParameters = parameters;
		}
	}
}
