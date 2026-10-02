using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000179 RID: 377
	public class TauntSlotData : MultiplayerLocalData
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000116E6 File Offset: 0x0000F8E6
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x000116EE File Offset: 0x0000F8EE
		public string PlayerId { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000116F7 File Offset: 0x0000F8F7
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x000116FF File Offset: 0x0000F8FF
		public List<TauntIndexData> TauntIndices { get; set; }

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00011708 File Offset: 0x0000F908
		public TauntSlotData(string playerId)
		{
			this.PlayerId = playerId;
			this.TauntIndices = new List<TauntIndexData>();
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00011724 File Offset: 0x0000F924
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			TauntSlotData tauntSlotData;
			return (tauntSlotData = other as TauntSlotData) != null && this.PlayerId == tauntSlotData.PlayerId && this.TauntIndices.SequenceEqual<TauntIndexData>(tauntSlotData.TauntIndices);
		}
	}
}
