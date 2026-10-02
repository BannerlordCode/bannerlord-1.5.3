using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x0200003D RID: 61
	[DataContract]
	[Serializable]
	public class RestObjectRequestMessage : RestRequestMessage
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000053D7 File Offset: 0x000035D7
		// (set) Token: 0x06000193 RID: 403 RVA: 0x000053DF File Offset: 0x000035DF
		[DataMember]
		public MessageType MessageType { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000053E8 File Offset: 0x000035E8
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000053F0 File Offset: 0x000035F0
		[DataMember]
		public SessionCredentials SessionCredentials { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000053F9 File Offset: 0x000035F9
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00005401 File Offset: 0x00003601
		[DataMember]
		public Message Message { get; private set; }

		// Token: 0x06000198 RID: 408 RVA: 0x0000540A File Offset: 0x0000360A
		public RestObjectRequestMessage()
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005412 File Offset: 0x00003612
		public RestObjectRequestMessage(SessionCredentials sessionCredentials, Message message, MessageType messageType)
		{
			this.Message = message;
			this.MessageType = messageType;
			this.SessionCredentials = sessionCredentials;
		}
	}
}
