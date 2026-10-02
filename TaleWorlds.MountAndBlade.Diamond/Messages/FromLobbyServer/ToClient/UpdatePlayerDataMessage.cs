using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006B RID: 107
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class UpdatePlayerDataMessage : Message
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000384D File Offset: 0x00001A4D
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00003855 File Offset: 0x00001A55
		[JsonProperty]
		public PlayerData PlayerData { get; private set; }

		// Token: 0x06000229 RID: 553 RVA: 0x0000385E File Offset: 0x00001A5E
		public UpdatePlayerDataMessage()
		{
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00003866 File Offset: 0x00001A66
		public UpdatePlayerDataMessage(PlayerData playerData)
		{
			this.PlayerData = playerData;
		}
	}
}
