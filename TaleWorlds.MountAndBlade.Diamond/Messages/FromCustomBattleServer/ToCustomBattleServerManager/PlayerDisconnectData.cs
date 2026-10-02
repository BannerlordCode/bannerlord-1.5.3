using System;
using Newtonsoft.Json;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class PlayerDisconnectData
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002377 File Offset: 0x00000577
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000237F File Offset: 0x0000057F
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002388 File Offset: 0x00000588
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002390 File Offset: 0x00000590
		[JsonProperty]
		public DisconnectType Type { get; private set; }

		// Token: 0x06000039 RID: 57 RVA: 0x00002399 File Offset: 0x00000599
		public PlayerDisconnectData()
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000023A1 File Offset: 0x000005A1
		public PlayerDisconnectData(PlayerId playerId, DisconnectType type)
		{
			this.PlayerId = playerId;
			this.Type = type;
		}
	}
}
