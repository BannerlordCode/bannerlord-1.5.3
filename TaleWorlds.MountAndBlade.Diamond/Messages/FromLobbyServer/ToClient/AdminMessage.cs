using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000016 RID: 22
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class AdminMessage : Message
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002784 File Offset: 0x00000984
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000278C File Offset: 0x0000098C
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00002795 File Offset: 0x00000995
		public AdminMessage()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000279D File Offset: 0x0000099D
		public AdminMessage(string message)
		{
			this.Message = message;
		}
	}
}
