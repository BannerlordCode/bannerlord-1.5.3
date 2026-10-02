using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CC RID: 204
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class WhisperMessage : Message
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00004912 File Offset: 0x00002B12
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0000491A File Offset: 0x00002B1A
		[JsonProperty]
		public string TargetPlayerName { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00004923 File Offset: 0x00002B23
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0000492B File Offset: 0x00002B2B
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x060003C3 RID: 963 RVA: 0x00004934 File Offset: 0x00002B34
		public WhisperMessage()
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000493C File Offset: 0x00002B3C
		public WhisperMessage(string targetPlayerName, string message)
		{
			this.TargetPlayerName = targetPlayerName;
			this.Message = message;
		}
	}
}
