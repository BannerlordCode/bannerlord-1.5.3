using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012C RID: 300
	[Serializable]
	public class PlayerJoinGameData
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0000BCFD File Offset: 0x00009EFD
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0000BD05 File Offset: 0x00009F05
		public PlayerData PlayerData { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0000BD0E File Offset: 0x00009F0E
		public PlayerId PlayerId
		{
			get
			{
				return this.PlayerData.PlayerId;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0000BD1B File Offset: 0x00009F1B
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x0000BD23 File Offset: 0x00009F23
		public string Name { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0000BD2C File Offset: 0x00009F2C
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0000BD34 File Offset: 0x00009F34
		public Guid? PartyId { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0000BD3D File Offset: 0x00009F3D
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0000BD45 File Offset: 0x00009F45
		public Dictionary<string, List<string>> UsedCosmetics { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0000BD4E File Offset: 0x00009F4E
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0000BD56 File Offset: 0x00009F56
		[JsonProperty]
		public string IpAddress { get; private set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0000BD5F File Offset: 0x00009F5F
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0000BD67 File Offset: 0x00009F67
		[JsonProperty]
		public CustomGameJoinType JoinType { get; private set; }

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000BD70 File Offset: 0x00009F70
		public PlayerJoinGameData()
		{
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000BD78 File Offset: 0x00009F78
		public PlayerJoinGameData(PlayerData playerData, string name, Guid? partyId, Dictionary<string, List<string>> usedCosmetics, string ipAddress, CustomGameJoinType joinType)
		{
			this.PlayerData = playerData;
			this.Name = name;
			this.PartyId = partyId;
			this.UsedCosmetics = usedCosmetics;
			this.IpAddress = ipAddress;
			this.JoinType = joinType;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0000BDB0 File Offset: 0x00009FB0
		public override string ToString()
		{
			return string.Format("Player Join Game Data: {0}, name={1}, party={2}, cosmetics={3}, ip={4}, joinType={5}", new object[]
			{
				this.PlayerId,
				this.Name,
				this.PartyId,
				this.UsedCosmetics.Count,
				this.IpAddress,
				this.JoinType
			});
		}
	}
}
