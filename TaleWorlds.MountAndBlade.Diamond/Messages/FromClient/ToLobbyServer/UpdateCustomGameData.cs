using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C7 RID: 199
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateCustomGameData : Message
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000481A File Offset: 0x00002A1A
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00004822 File Offset: 0x00002A22
		[JsonProperty]
		public string NewGameType { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000482B File Offset: 0x00002A2B
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00004833 File Offset: 0x00002A33
		[JsonProperty]
		public string NewMap { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000483C File Offset: 0x00002A3C
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00004844 File Offset: 0x00002A44
		[JsonProperty]
		public int NewMaxNumberOfPlayers { get; private set; }

		// Token: 0x060003AD RID: 941 RVA: 0x0000484D File Offset: 0x00002A4D
		public UpdateCustomGameData()
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00004855 File Offset: 0x00002A55
		public UpdateCustomGameData(string newGameType, string newMap, int newMaxNumberOfPlayers)
		{
			this.NewGameType = newGameType;
			this.NewMap = newMap;
			this.NewMaxNumberOfPlayers = newMaxNumberOfPlayers;
		}
	}
}
