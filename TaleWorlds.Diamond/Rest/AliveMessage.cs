using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000038 RID: 56
	[DataContract]
	[Serializable]
	public class AliveMessage : RestRequestMessage
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00003F5A File Offset: 0x0000215A
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00003F62 File Offset: 0x00002162
		[DataMember]
		public SessionCredentials SessionCredentials { get; private set; }

		// Token: 0x06000147 RID: 327 RVA: 0x00003F6B File Offset: 0x0000216B
		public AliveMessage()
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00003F73 File Offset: 0x00002173
		[JsonConstructor]
		public AliveMessage(SessionCredentials sessionCredentials)
		{
			this.SessionCredentials = sessionCredentials;
		}
	}
}
