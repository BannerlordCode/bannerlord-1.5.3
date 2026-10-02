using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002B RID: 43
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CustomBattleOverMessage : Message
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00002B48 File Offset: 0x00000D48
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00002B50 File Offset: 0x00000D50
		[JsonProperty]
		public int OldExperience { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002B59 File Offset: 0x00000D59
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00002B61 File Offset: 0x00000D61
		[JsonProperty]
		public int NewExperience { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00002B6A File Offset: 0x00000D6A
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00002B72 File Offset: 0x00000D72
		[JsonProperty]
		public int GoldGain { get; set; }

		// Token: 0x060000F4 RID: 244 RVA: 0x00002B7B File Offset: 0x00000D7B
		public CustomBattleOverMessage()
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00002B83 File Offset: 0x00000D83
		public CustomBattleOverMessage(int oldExperience, int newExperience, int goldGain)
		{
			this.OldExperience = oldExperience;
			this.NewExperience = newExperience;
			this.GoldGain = goldGain;
		}
	}
}
