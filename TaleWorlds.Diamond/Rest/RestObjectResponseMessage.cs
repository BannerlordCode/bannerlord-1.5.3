using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000036 RID: 54
	[DataContract]
	[Serializable]
	public class RestObjectResponseMessage : RestResponseMessage
	{
		// Token: 0x06000140 RID: 320 RVA: 0x00003F33 File Offset: 0x00002133
		public override Message GetMessage()
		{
			return this._message;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003F3B File Offset: 0x0000213B
		public RestObjectResponseMessage()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00003F43 File Offset: 0x00002143
		public RestObjectResponseMessage(Message message)
		{
			this._message = message;
		}

		// Token: 0x04000064 RID: 100
		[DataMember]
		private Message _message;
	}
}
