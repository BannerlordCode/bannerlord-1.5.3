using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047D RID: 1149
	public class CommentOnClanDestroyedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004ACE RID: 19150 RVA: 0x00179BDC File Offset: 0x00177DDC
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x00179BF5 File Offset: 0x00177DF5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00179BF7 File Offset: 0x00177DF7
		private void OnClanDestroyed(Clan destroyedClan)
		{
			LogEntry.AddLogEntry(new ClanDestroyedLogEntry(destroyedClan));
		}
	}
}
