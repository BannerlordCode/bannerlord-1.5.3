using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000109 RID: 265
	[Serializable]
	public class ClanPlayerInfo
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00007234 File Offset: 0x00005434
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0000723C File Offset: 0x0000543C
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00007245 File Offset: 0x00005445
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0000724D File Offset: 0x0000544D
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00007256 File Offset: 0x00005456
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0000725E File Offset: 0x0000545E
		[JsonProperty]
		public AnotherPlayerState State { get; private set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00007267 File Offset: 0x00005467
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0000726F File Offset: 0x0000546F
		[JsonProperty]
		public string ActiveBadgeId { get; private set; }

		// Token: 0x060005B8 RID: 1464 RVA: 0x00007278 File Offset: 0x00005478
		public ClanPlayerInfo(PlayerId playerId, string playerName, AnotherPlayerState anotherPlayerState, string activeBadgeId)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ActiveBadgeId = activeBadgeId;
			this.State = anotherPlayerState;
		}
	}
}
