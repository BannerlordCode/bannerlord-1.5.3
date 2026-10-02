using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000CE RID: 206
	public class PlayerToggleTrackSettlementFromEncyclopediaEvent : EventBase
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0004E8A3 File Offset: 0x0004CAA3
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x0004E8AB File Offset: 0x0004CAAB
		public bool IsCurrentlyTracked { get; private set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x0004E8B4 File Offset: 0x0004CAB4
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0004E8BC File Offset: 0x0004CABC
		public Settlement ToggledTrackedSettlement { get; private set; }

		// Token: 0x06001363 RID: 4963 RVA: 0x0004E8C5 File Offset: 0x0004CAC5
		public PlayerToggleTrackSettlementFromEncyclopediaEvent(Settlement toggleTrackedSettlement, bool isCurrentlyTracked)
		{
			this.ToggledTrackedSettlement = toggleTrackedSettlement;
			this.IsCurrentlyTracked = isCurrentlyTracked;
		}
	}
}
