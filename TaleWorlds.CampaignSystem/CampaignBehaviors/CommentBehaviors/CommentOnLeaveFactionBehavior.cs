using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000484 RID: 1156
	public class CommentOnLeaveFactionBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AEA RID: 19178 RVA: 0x00179DE6 File Offset: 0x00177FE6
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanLeaveKingdom));
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00179DFF File Offset: 0x00177FFF
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00179E01 File Offset: 0x00178001
		private void OnClanLeaveKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			LogEntry.AddLogEntry(new ClanChangeKingdomLogEntry(clan, oldKingdom, newKingdom, detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion));
		}
	}
}
