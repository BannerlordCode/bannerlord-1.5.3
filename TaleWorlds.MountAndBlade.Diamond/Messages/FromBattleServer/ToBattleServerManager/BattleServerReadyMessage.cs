using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.Library;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D5 RID: 213
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleServerReadyMessage : LoginMessage
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00004B3D File Offset: 0x00002D3D
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00004B45 File Offset: 0x00002D45
		[JsonProperty]
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00004B4E File Offset: 0x00002D4E
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00004B56 File Offset: 0x00002D56
		[JsonProperty]
		public string AssignedAddress { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00004B5F File Offset: 0x00002D5F
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00004B67 File Offset: 0x00002D67
		[JsonProperty]
		public ushort AssignedPort { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00004B70 File Offset: 0x00002D70
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00004B78 File Offset: 0x00002D78
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00004B81 File Offset: 0x00002D81
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00004B89 File Offset: 0x00002D89
		[JsonProperty]
		public sbyte Priority { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00004B92 File Offset: 0x00002D92
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00004B9A File Offset: 0x00002D9A
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00004BA3 File Offset: 0x00002DA3
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00004BAB File Offset: 0x00002DAB
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x060003F8 RID: 1016 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public BattleServerReadyMessage()
		{
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00004BBC File Offset: 0x00002DBC
		public BattleServerReadyMessage(PeerId peerId, ApplicationVersion applicationVersion, string assignedAddress, ushort assignedPort, string region, sbyte priority, string password, string gameType)
			: base(peerId, null)
		{
			this.ApplicationVersion = applicationVersion;
			this.AssignedAddress = assignedAddress;
			this.AssignedPort = assignedPort;
			this.Region = region;
			this.Priority = priority;
			this.Password = password;
			this.GameType = gameType;
		}
	}
}
