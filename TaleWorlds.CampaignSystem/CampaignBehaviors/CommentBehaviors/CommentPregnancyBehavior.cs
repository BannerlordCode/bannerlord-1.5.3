using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000487 RID: 1159
	public class CommentPregnancyBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AF6 RID: 19190 RVA: 0x00179EDF File Offset: 0x001780DF
		public override void RegisterEvents()
		{
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnChildConceived));
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00179EF8 File Offset: 0x001780F8
		private void OnChildConceived(Hero mother)
		{
			LogEntry.AddLogEntry(new PregnancyLogEntry(mother));
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x00179F05 File Offset: 0x00178105
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
