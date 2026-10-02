using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x0200003F RID: 63
	[DataContract]
	[Serializable]
	public abstract class RestRequestMessage : RestData
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000542F File Offset: 0x0000362F
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00005437 File Offset: 0x00003637
		[DataMember]
		public byte[] UserCertificate { get; set; }
	}
}
