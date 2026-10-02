using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012F RID: 303
	[Serializable]
	public class JoinGameData
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0000C0FB File Offset: 0x0000A2FB
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0000C103 File Offset: 0x0000A303
		public GameServerProperties GameServerProperties { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0000C10C File Offset: 0x0000A30C
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x0000C114 File Offset: 0x0000A314
		public int PeerIndex { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0000C11D File Offset: 0x0000A31D
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0000C125 File Offset: 0x0000A325
		public int SessionKey { get; set; }

		// Token: 0x06000818 RID: 2072 RVA: 0x0000C12E File Offset: 0x0000A32E
		public JoinGameData()
		{
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0000C136 File Offset: 0x0000A336
		public JoinGameData(GameServerProperties gameServerProperties, int peerIndex, int sessionKey)
		{
			this.GameServerProperties = gameServerProperties;
			this.PeerIndex = peerIndex;
			this.SessionKey = sessionKey;
		}
	}
}
