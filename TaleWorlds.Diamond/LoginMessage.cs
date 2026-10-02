using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000019 RID: 25
	[DataContract]
	[Serializable]
	public abstract class LoginMessage : Message
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002AB8 File Offset: 0x00000CB8
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00002AC0 File Offset: 0x00000CC0
		[DataMember]
		public PeerId PeerId { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002AC9 File Offset: 0x00000CC9
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00002AD1 File Offset: 0x00000CD1
		[JsonProperty]
		public AccessObject AccessObject { get; private set; }

		// Token: 0x0600008F RID: 143 RVA: 0x00002ADA File Offset: 0x00000CDA
		public LoginMessage()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002AE2 File Offset: 0x00000CE2
		protected LoginMessage(PeerId peerId, AccessObject accessObject)
		{
			this.PeerId = peerId;
			this.AccessObject = accessObject;
		}
	}
}
