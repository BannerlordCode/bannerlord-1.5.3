using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000089 RID: 137
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CreatePremadeGameMessage : Message
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00003D0E File Offset: 0x00001F0E
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00003D16 File Offset: 0x00001F16
		[JsonProperty]
		public string PremadeGameName { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00003D1F File Offset: 0x00001F1F
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00003D27 File Offset: 0x00001F27
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00003D30 File Offset: 0x00001F30
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00003D38 File Offset: 0x00001F38
		[JsonProperty]
		public string MapName { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00003D41 File Offset: 0x00001F41
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00003D49 File Offset: 0x00001F49
		[JsonProperty]
		public string FactionA { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00003D52 File Offset: 0x00001F52
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x00003D5A File Offset: 0x00001F5A
		[JsonProperty]
		public string FactionB { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00003D63 File Offset: 0x00001F63
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x00003D6B File Offset: 0x00001F6B
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00003D74 File Offset: 0x00001F74
		// (set) Token: 0x060002AB RID: 683 RVA: 0x00003D7C File Offset: 0x00001F7C
		[JsonProperty]
		public string SpectatorPassword { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00003D85 File Offset: 0x00001F85
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00003D8D File Offset: 0x00001F8D
		[JsonProperty]
		public int MaxSpectatorCount { get; private set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00003D96 File Offset: 0x00001F96
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00003D9E File Offset: 0x00001F9E
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x060002B0 RID: 688 RVA: 0x00003DA7 File Offset: 0x00001FA7
		public CreatePremadeGameMessage()
		{
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public CreatePremadeGameMessage(string premadeGameName, string gameType, string mapName, string factionA, string factionB, string password, PremadeGameType premadeGameType, string spectatorPassword, int maxSpectatorCount)
		{
			this.PremadeGameName = premadeGameName;
			this.GameType = gameType;
			this.MapName = mapName;
			this.FactionA = factionA;
			this.FactionB = factionB;
			this.Password = password;
			this.PremadeGameType = premadeGameType;
			this.SpectatorPassword = spectatorPassword;
			this.MaxSpectatorCount = maxSpectatorCount;
		}
	}
}
