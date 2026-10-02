using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000027 RID: 39
	[DataContract]
	[Serializable]
	public sealed class SessionCredentials
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000033D2 File Offset: 0x000015D2
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000033DA File Offset: 0x000015DA
		[DataMember]
		public PeerId PeerId { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000033E3 File Offset: 0x000015E3
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x000033EB File Offset: 0x000015EB
		[DataMember]
		public SessionKey SessionKey { get; private set; }

		// Token: 0x060000D7 RID: 215 RVA: 0x000033F4 File Offset: 0x000015F4
		[JsonConstructor]
		public SessionCredentials(PeerId peerId, SessionKey sessionKey)
		{
			this.PeerId = peerId;
			this.SessionKey = sessionKey;
		}
	}
}
