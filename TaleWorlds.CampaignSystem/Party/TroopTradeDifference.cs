using System;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000316 RID: 790
	public struct TroopTradeDifference
	{
		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000C1B10 File Offset: 0x000BFD10
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x000C1B18 File Offset: 0x000BFD18
		public CharacterObject Troop { get; set; }

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000C1B21 File Offset: 0x000BFD21
		// (set) Token: 0x06002E33 RID: 11827 RVA: 0x000C1B29 File Offset: 0x000BFD29
		public bool IsPrisoner { get; set; }

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000C1B32 File Offset: 0x000BFD32
		// (set) Token: 0x06002E35 RID: 11829 RVA: 0x000C1B3A File Offset: 0x000BFD3A
		public int FromCount { get; set; }

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06002E36 RID: 11830 RVA: 0x000C1B43 File Offset: 0x000BFD43
		// (set) Token: 0x06002E37 RID: 11831 RVA: 0x000C1B4B File Offset: 0x000BFD4B
		public int ToCount { get; set; }

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002E38 RID: 11832 RVA: 0x000C1B54 File Offset: 0x000BFD54
		public int DifferenceCount
		{
			get
			{
				return this.FromCount - this.ToCount;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002E39 RID: 11833 RVA: 0x000C1B63 File Offset: 0x000BFD63
		// (set) Token: 0x06002E3A RID: 11834 RVA: 0x000C1B6B File Offset: 0x000BFD6B
		public bool IsEmpty { get; private set; }

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000C1B74 File Offset: 0x000BFD74
		public static TroopTradeDifference Empty
		{
			get
			{
				return new TroopTradeDifference
				{
					IsEmpty = true
				};
			}
		}
	}
}
