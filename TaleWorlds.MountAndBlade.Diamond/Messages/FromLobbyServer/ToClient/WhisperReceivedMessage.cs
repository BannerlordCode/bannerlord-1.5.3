using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006E RID: 110
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class WhisperReceivedMessage : Message
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000038C5 File Offset: 0x00001AC5
		// (set) Token: 0x06000234 RID: 564 RVA: 0x000038CD File Offset: 0x00001ACD
		[JsonProperty]
		public string FromPlayer { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000235 RID: 565 RVA: 0x000038D6 File Offset: 0x00001AD6
		// (set) Token: 0x06000236 RID: 566 RVA: 0x000038DE File Offset: 0x00001ADE
		[JsonProperty]
		public string ToPlayer { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000038E7 File Offset: 0x00001AE7
		// (set) Token: 0x06000238 RID: 568 RVA: 0x000038EF File Offset: 0x00001AEF
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000239 RID: 569 RVA: 0x000038F8 File Offset: 0x00001AF8
		public WhisperReceivedMessage()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00003900 File Offset: 0x00001B00
		public WhisperReceivedMessage(string fromPlayer, string toPlayer, string message)
		{
			this.FromPlayer = fromPlayer;
			this.ToPlayer = toPlayer;
			this.Message = message;
		}
	}
}
