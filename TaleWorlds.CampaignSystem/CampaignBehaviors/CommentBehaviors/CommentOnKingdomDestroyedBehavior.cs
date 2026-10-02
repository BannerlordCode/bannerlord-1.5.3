using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000483 RID: 1155
	public class CommentOnKingdomDestroyedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AE6 RID: 19174 RVA: 0x00179DB6 File Offset: 0x00177FB6
		public override void RegisterEvents()
		{
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00179DCF File Offset: 0x00177FCF
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x00179DD1 File Offset: 0x00177FD1
		private void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			LogEntry.AddLogEntry(new KingdomDestroyedLogEntry(destroyedKingdom));
		}
	}
}
