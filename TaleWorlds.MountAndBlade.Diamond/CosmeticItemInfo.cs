using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000111 RID: 273
	[Serializable]
	public class CosmeticItemInfo
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00007526 File Offset: 0x00005726
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0000752E File Offset: 0x0000572E
		public string TroopId { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00007537 File Offset: 0x00005737
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0000753F File Offset: 0x0000573F
		public string CosmeticIndex { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00007548 File Offset: 0x00005748
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x00007550 File Offset: 0x00005750
		public bool IsEquipped { get; set; }

		// Token: 0x060005F2 RID: 1522 RVA: 0x00007559 File Offset: 0x00005759
		public CosmeticItemInfo()
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00007561 File Offset: 0x00005761
		public CosmeticItemInfo(string troopId, string cosmeticIndex, bool isEquipped)
		{
			this.TroopId = troopId;
			this.CosmeticIndex = cosmeticIndex;
			this.IsEquipped = isEquipped;
		}
	}
}
