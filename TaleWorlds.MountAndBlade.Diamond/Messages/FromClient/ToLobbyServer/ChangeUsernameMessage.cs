using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000083 RID: 131
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeUsernameMessage : Message
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00003BF5 File Offset: 0x00001DF5
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00003BFD File Offset: 0x00001DFD
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x06000285 RID: 645 RVA: 0x00003C06 File Offset: 0x00001E06
		public ChangeUsernameMessage()
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00003C0E File Offset: 0x00001E0E
		public ChangeUsernameMessage(string username)
		{
			this.Username = username;
		}
	}
}
