using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047E RID: 1150
	public class CommentOnClanLeaderChangedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AD2 RID: 19154 RVA: 0x00179C0C File Offset: 0x00177E0C
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(CommentOnClanLeaderChangedBehavior.OnClanLeaderChanged));
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x00179C25 File Offset: 0x00177E25
		private static void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			LogEntry.AddLogEntry(new ClanLeaderChangedLogEntry(oldLeader, newLeader));
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00179C33 File Offset: 0x00177E33
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
